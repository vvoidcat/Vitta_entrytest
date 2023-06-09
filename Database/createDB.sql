-- CREATION

CREATE DATABASE test;
GO

USE test;
GO

CREATE SCHEMA vitta;
GO

-- TABLE: orders / заказы
CREATE TABLE test.vitta.orders (
    id bigint PRIMARY KEY IDENTITY,
    date datetime NOT NULL DEFAULT GETDATE(),
    sum_total money NOT NULL DEFAULT 0,
    CONSTRAINT chk_sum_total_notnegative CHECK(sum_total >= 0),
    sum_payed money NOT NULL DEFAULT 0,
    CONSTRAINT chk_sum_payed_notnegative CHECK(sum_payed >= 0),
    CONSTRAINT chk_sum_payed_notexceeding CHECK(sum_payed <= sum_total)
);

-- TABLE: money_incomes / приход денег
CREATE TABLE test.vitta.money_incomes (
    id bigint PRIMARY KEY IDENTITY NOT NULL,
    date datetime NOT NULL DEFAULT GETDATE(),
    income money NOT NULL DEFAULT 0,
    CONSTRAINT chk_income_notnegative CHECK(income >= 0),
    balance money NOT NULL DEFAULT 0,
    CONSTRAINT chk_balance_notnegative CHECK(balance >= 0),
    CONSTRAINT chk_balance_notexceeding CHECK(balance <= income)
);

-- TABLE: payments / платежи
CREATE TABLE test.vitta.payments (
    id bigint PRIMARY KEY IDENTITY,     -- added a PKEY column to implement a deletion by payment id trigger
    order_id bigint NOT NULL,
    income_id bigint NOT NULL,
    sum money NOT NULL DEFAULT 0,
    CONSTRAINT chk_sum_notnegative CHECK(sum >= 0),
    CONSTRAINT fk_payment_order_id FOREIGN KEY (order_id) REFERENCES test.vitta.orders(id),
    CONSTRAINT fk_payment_income_id FOREIGN KEY (income_id) REFERENCES test.vitta.money_incomes(id)
);
GO

-- TRIGGERS

-- the DML trigger that modifies the *payments*, *orders* and *money_incomes* tables on INSERT.
-- upon its execution, *orders.sum_payed* should increase and *money_incomes.balance* should decrease respectfully.
-- *money_incomes.balance* cannot become negative (can only become 0).
-- *payments.sum* + *orders.sum_payed* should not be above *orders.sum_total* (implemented as a forced return of the excess money).
CREATE OR ALTER TRIGGER vitta.trg_payments_insert ON test.vitta.payments
INSTEAD OF INSERT
AS
    BEGIN
        DECLARE @i_inc_id bigint, @i_ord_id bigint, @i_sum money;
        SET @i_inc_id = (SELECT income_id FROM inserted);
        SET @i_ord_id = (SELECT order_id FROM inserted);
        SET @i_sum = (SELECT sum FROM inserted);

        IF @i_ord_id NOT IN (SELECT id FROM orders) OR @i_inc_id NOT IN (SELECT id FROM money_incomes)
        BEGIN
            -- fk constraint error
            RAISERROR ('fk constraint mismatch, raised by vitta.trg_payments_insert', 16, 1);
        END

        IF (SELECT balance FROM money_incomes WHERE id = @i_inc_id) - @i_sum >= 0
            BEGIN
                DECLARE @excess money;
                SET @excess = (SELECT sum_payed FROM orders WHERE id = @i_ord_id)
                            - (SELECT sum_total FROM orders WHERE id = @i_ord_id) + @i_sum;

                IF @excess > 0
                    BEGIN
                        SET @i_sum -= @excess;
                    END
                IF @excess > 0 AND @i_sum = 0
                    BEGIN
                        -- if the order was already payed in full
                        RAISERROR('unrequired operation, raised by vitta.trg_payments_insert', 16, 1);
                        return;
                    END

                INSERT INTO test.vitta.payments(order_id, income_id, sum)
                VALUES (
                    @i_ord_id, 
                    @i_inc_id, 
                    @i_sum
                );

                UPDATE test.vitta.money_incomes
                SET
                    date = GETDATE(),
                    balance -= @i_sum
                WHERE @i_inc_id = id;

                UPDATE test.vitta.orders
                SET
                    date = GETDATE(),
                    sum_payed += @i_sum 
                WHERE @i_ord_id = id;
            END
    END
GO

-- (i ended up not using any update queries in the UI application, but the trigger is functional)
-- the DML trigger that modifies the *payments*, *orders* and *money_incomes* tables on UPDATE.
-- performes comparison of old and new states of column values, can process updating row values separately or all at once.
-- the update is rendered impossible if neither new or old *money_incomes.balance* values can accept the amount of money entered by the user.
-- if the user tries to pay more money than the order requires, any excess money is returned to the *money_incomes* row it has been previously taken from.
-- if the user tries to change the values in the *income_id* or *order_id* columns, the money transaction involving the previous values will be revoked.
CREATE OR ALTER TRIGGER vitta.trg_payments_update ON test.vitta.payments
INSTEAD OF UPDATE
AS
    BEGIN
        DECLARE @old_inc_id bigint, @old_ord_id bigint, @old_sum money;
        DECLARE @new_id bigint, @new_inc_id bigint, @new_ord_id bigint, @new_sum money;

        SET @old_inc_id = (SELECT income_id FROM deleted);
        SET @old_ord_id = (SELECT order_id FROM deleted);
        SET @old_sum = (SELECT sum FROM deleted);

        SET @new_id = (SELECT id FROM inserted);
        SET @new_inc_id = (SELECT income_id FROM inserted);
        SET @new_ord_id = (SELECT order_id FROM inserted);
        SET @new_sum = (SELECT sum FROM inserted);

        IF @new_ord_id NOT IN (SELECT id FROM orders) OR @new_inc_id NOT IN (SELECT id FROM money_incomes)
            BEGIN
                -- fk constraint error
                RAISERROR ('fk constraint mismatch, raised by vitta.trg_payments_insert', 16, 1);
            END

        IF (@new_inc_id = @old_inc_id
            AND (SELECT balance FROM money_incomes WHERE id = @new_inc_id) + @old_sum - @new_sum >= 0)
        OR (@new_inc_id != @old_inc_id
            AND (SELECT balance FROM money_incomes WHERE id = @new_inc_id) - @new_sum >= 0
            AND (SELECT balance FROM money_incomes WHERE id = @old_inc_id) + @old_sum <=
                (SELECT income FROM money_incomes WHERE id = @old_inc_id))
            BEGIN
                DECLARE @excess money;
                SET @excess = (SELECT sum_payed FROM orders WHERE id = @new_ord_id)
                            - (SELECT sum_total FROM orders WHERE id = @new_ord_id) + @new_sum;

                IF @excess > 0
                    BEGIN
                        SET @new_sum -= @excess;
                    END
                IF @excess > 0 AND @new_sum = 0
                    BEGIN
                        -- if the order was already payed in full
                        RAISERROR('unrequired operation, raised by vitta.trg_payments_update', 16, 1);
                        return;
                    END

                UPDATE money_incomes
                SET
                    date = GETDATE(),
                    balance += @old_sum
                WHERE id = @old_inc_id;

                UPDATE orders
                SET
                    date = GETDATE(),
                    sum_payed -= @old_sum
                WHERE id = @old_ord_id;

                UPDATE test.vitta.money_incomes
                SET
                    date = GETDATE(),
                    balance -= @new_sum
                WHERE @new_inc_id = id;

                UPDATE test.vitta.orders
                SET
                    date = GETDATE(),
                    sum_payed += @new_sum
                WHERE @new_ord_id = id;

                UPDATE test.vitta.payments
                SET
                    order_id = @new_ord_id,
                    income_id = @new_inc_id,
                    sum = @new_sum
                WHERE @new_id = id;
            END
    END
GO

-- the DML trigger that modifies the *orders* and *money_incomes* tables on DELETE.
-- if a deletion from the payments table is performed, money from the *orders* table gets transferred back to the *money_incomes* table.
-- (in the situation where the user tries to delete a payment, i assume that the payment has been cancelled).
CREATE OR ALTER TRIGGER vitta.trg_payments_delete ON test.vitta.payments
AFTER DELETE
AS
    BEGIN
        DECLARE @d_inc_id bigint, @d_ord_id bigint, @d_sum money;
        SET @d_inc_id = (SELECT income_id FROM deleted);
        SET @d_ord_id = (SELECT order_id FROM deleted);
        SET @d_sum = (SELECT sum FROM deleted);

        IF @d_ord_id IN (SELECT id FROM orders)
        AND @d_inc_id IN (SELECT id FROM money_incomes)
        AND @d_sum > 0
            BEGIN
                UPDATE test.vitta.money_incomes
                SET
                    date = GETDATE(),
                    balance += @d_sum
                WHERE @d_inc_id = id;

                UPDATE test.vitta.orders
                SET
                    date = GETDATE(),
                    sum_payed -= @d_sum 
                WHERE @d_ord_id = id;
            END
    END
GO


-- INITIAL VALUES INSERTION

INSERT INTO test.vitta.orders(sum_total, sum_payed) VALUES
(1000, 0),
(5500, 0);
GO

INSERT INTO test.vitta.money_incomes(income, balance) VALUES
(2000, 2000),
(7000, 7000);
GO

INSERT INTO test.vitta.payments(order_id, income_id, sum) VALUES
(2, 2, 1000);
GO

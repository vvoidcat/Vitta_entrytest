-- CREATION

CREATE DATABASE test;
GO

USE test;
GO

CREATE SCHEMA vitta;
GO

-- orders
CREATE TABLE test.vitta.orders (
    id bigint PRIMARY KEY IDENTITY,
    date datetime NOT NULL DEFAULT GETDATE(),
    sum_whole money NOT NULL DEFAULT 0,
    CONSTRAINT chk_sum_whole_notnegative CHECK(sum_whole >= 0),
    sum_payed money NOT NULL DEFAULT 0,
    CONSTRAINT chk_sum_payed_notnegative CHECK(sum_payed >= 0)
);

-- money incomes
CREATE TABLE test.vitta.money_incomes (
    id bigint PRIMARY KEY IDENTITY NOT NULL,
    date datetime NOT NULL DEFAULT GETDATE(),
    incoming_payment money NOT NULL DEFAULT 0,
    CONSTRAINT chk_incoming_payment_notnegative CHECK(incoming_payment >= 0),
    balance money NOT NULL DEFAULT 0,
    CONSTRAINT chk_balance_notnegative CHECK(balance >= 0)
);

-- payments
CREATE TABLE test.vitta.payments (
    id bigint PRIMARY KEY IDENTITY,
    order_id bigint NOT NULL,
    income_id bigint NOT NULL,
    sum money NOT NULL DEFAULT 0,
    CONSTRAINT chk_sum_notnegative CHECK(sum >= 0),
    CONSTRAINT fk_payment_order_id FOREIGN KEY (order_id) REFERENCES test.vitta.orders(id),
    CONSTRAINT fk_payment_income_id FOREIGN KEY (income_id) REFERENCES test.vitta.money_incomes(id)
);
GO

-- TRIGGERS

-- the DML trigger modifies the orders and money_incomes tables on INSERT
-- upon its execution, orders.sum_payed should increase and money_incomes.balance should decrease respectfully
-- money_incomes.balance cannot become negative (can only become 0)
-- payments.sum + orders.sum_payed should not be above orders.sum_whole (implemented as a forced return of excess money)
CREATE OR ALTER TRIGGER vitta.trg_payments_insert ON test.vitta.payments
INSTEAD OF INSERT
AS
    BEGIN
        DECLARE @i_inc_id bigint, @i_ord_id bigint, @i_sum money;
        SET @i_inc_id = (SELECT income_id FROM inserted);
        SET @i_ord_id = (SELECT order_id FROM inserted);
        SELECT @i_sum = (SELECT sum FROM inserted);

        IF @i_ord_id IN (SELECT id FROM orders)
        AND @i_inc_id IN (SELECT id FROM money_incomes)
        AND (SELECT balance FROM money_incomes mi WHERE mi.id = @i_inc_id) - @i_sum >= 0
            BEGIN
                -- should be a transaction !!
                DECLARE @excess money;
                SET @excess = (SELECT sum_payed FROM orders) - (SELECT sum_whole FROM orders) + @i_sum;

                IF @excess > 0
                    BEGIN
                        SET @i_sum -= @excess;
                    END

                INSERT INTO test.vitta.payments(order_id, income_id, sum)
                VALUES (@i_ord_id, @i_inc_id, @i_sum);

                UPDATE test.vitta.money_incomes
                SET balance -= @i_sum
                WHERE @i_inc_id = id;

                UPDATE test.vitta.orders
                SET sum_payed += @i_sum 
                WHERE @i_ord_id = id;
            END
    END
GO

CREATE OR ALTER TRIGGER vitta.trg_payments_update ON test.vitta.payments
INSTEAD OF UPDATE
AS
GO

CREATE OR ALTER TRIGGER vitta.trg_payments_delete
INSTEAD OF DELETE
AS
GO


-- VALUES INSERTION

INSERT INTO test.vitta.orders(sum_whole, sum_payed) VALUES
(100, 0),
(200, 100),
(333, 50);

INSERT INTO test.vitta.money_incomes(incoming_payment, balance) VALUES
(500, 500),
(600, 400),
(700, 100.10);


-- TESTING

insert into test.vitta.orders(sum_whole, sum_payed) VALUES (1000, 0);
insert into test.vitta.money_incomes(incoming_payment, balance) VALUES (2000, 2000);
go

INSERT INTO test.vitta.payments(order_id, income_id, sum) VALUES
(1, 1, 1900);
GO
select * from test.vitta.payments;
go

select * from test.vitta.orders;
select * from test.vitta.money_incomes;
select * from test.vitta.payments;
go
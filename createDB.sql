-- DROP TABLE vitta.payments, vitta.orders, vitta.money_incomes;
-- GO

-- DROP DATABASE vitta;
-- DROP SCHEMA test;
-- GO

CREATE SCHEMA vitta;
GO

CREATE TABLE vitta.orders (
    id bigint PRIMARY KEY IDENTITY,
    date date NOT NULL DEFAULT GETDATE(),
    sum_whole numeric NOT NULL DEFAULT 0,
    sum_payed numeric NOT NULL DEFAULT 0
    -- todo add check constraint
);

CREATE TABLE vitta.money_incomes (
    id bigint PRIMARY KEY IDENTITY NOT NULL,
    date date NOT NULL DEFAULT GETDATE(),
    incoming_payment numeric NOT NULL DEFAULT 0,
    balance numeric NOT NULL DEFAULT 0
    -- todo add check constraint
);

CREATE TABLE vitta.payments (
    order_id bigint NOT NULL,
    income_id bigint NOT NULL,
    sum numeric NOT NULL DEFAULT 0,
    CONSTRAINT fk_payment_order_id FOREIGN KEY (order_id) REFERENCES vitta.orders(id),
    CONSTRAINT fk_payment_income_id FOREIGN KEY (income_id) REFERENCES vitta.money_incomes(id)
    -- todo add check constraint
);
GO

INSERT INTO vitta.orders(sum_whole, sum_payed) VALUES
(100, 0),
(200, 100),
(333, 1);

INSERT INTO vitta.money_incomes(incoming_payment, balance) VALUES
(500, 500),
(600, 400),
(700, 11);

INSERT INTO vitta.payments(order_id, income_id, sum) VALUES
(1, 1, 30),
(2, 1, 100),
(1, 1, 90);

GO

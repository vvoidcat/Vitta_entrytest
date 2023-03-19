DROP TRIGGER vitta.trg_payments_insert;
DROP TRIGGER vitta.trg_payments_update;
DROP TRIGGER vitta.trg_payments_delete;
GO

DROP TABLE IF EXISTS test.vitta.payments, test.vitta.orders, test.vitta.money_incomes;
GO

DROP SCHEMA test;
GO

DROP DATABASE vitta;
GO

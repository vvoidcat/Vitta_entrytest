DROP TRIGGER vitta.trg_payments_insert;
DROP TRIGGER vitta.trg_payments_update;
DROP TRIGGER vitta.trg_payments_delete;
GO

DROP TABLE test.vitta.payments, test.vitta.orders, test.vitta.money_incomes;
GO

DROP SCHEMA vitta;
GO

DROP DATABASE test;
GO

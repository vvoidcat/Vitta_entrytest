using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace VittatestApp.Model
{
    static class DataAccess
    {
        public static List<Order> GetAllOrdersOrdered()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                return connection.Query<Order>("select * from test.vitta.orders order by id").ToList();
            }
        }

        public static List<Income> GetAllIncomesOrdered()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                return connection.Query<Income>("select * from test.vitta.money_incomes order by id").ToList();
            }
        }

        public static List<Payment> GetAllPaymentsOrdered()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                return connection.Query<Payment>("select * from test.vitta.payments order by id").ToList();
            }
        }

        public static bool InsertIntoOrders(DateTime? date, decimal? sum_whole, decimal? sum_payed)
        {
            bool noErr = true;

            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                string query = @"insert into test.vitta.orders(date, sum_whole, sum_payed) values(@date, @sum_whole, @sum_payed)";

                try
                {
                    connection.Execute(query, new
                    {
                        date,
                        sum_whole,
                        sum_payed
                    });
                }
                catch (SqlException)
                {
                    noErr = false;
                }
            }
            return noErr;
        }

        public static bool InsertIntoIncomes(DateTime? date, decimal? incoming, decimal? balance)
        {
            bool noErr = true;

            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                string query = @"insert into test.vitta.money_incomes(date, incoming_payment, balance) values(@date, @incoming, @balance)";

                try
                {
                    connection.Execute(query, new
                    {
                        date,
                        incoming,
                        balance
                    });
                }
                catch (SqlException)
                {
                    noErr = false;
                }
            }
            return noErr;
        }

        public static bool InsertIntoPayments(long? order_id, long? income_id, decimal? sum)
        {
            bool noErr = true;

            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                string query = @"insert into test.vitta.payments(order_id, income_id, sum) values(@order_id, @income_id, @sum)";

                try
                {
                    connection.Execute(query, new
                    {
                        order_id,
                        income_id,
                        sum
                    });
                }
                catch (SqlException)
                {
                    noErr = false;
                }
            }
            return noErr;
        }

        public static bool DeleteFromPayments(long payment_id)
        {
            bool updateNeeded = true;

            using (IDbConnection connection = new SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                int rowsAffected = 0;
                string query = @"delete from test.vitta.payments where id = @payment_id";

                try
                {
                    rowsAffected = connection.Execute(query, new
                    {
                        payment_id
                    });
                }
                catch (SqlException)
                {
                    updateNeeded = false;
                }
                finally
                {
                    if (rowsAffected > 0)
                    {
                        updateNeeded = true;
                    }
                }
            }
            return updateNeeded;
        }
    }
}

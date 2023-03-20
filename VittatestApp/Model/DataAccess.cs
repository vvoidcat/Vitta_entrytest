using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace VittatestApp.Model
{
    static class DataAccess
    {
        public static List<Order> GetAllOrdersByID()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                return connection.Query<Order>("select * from test.vitta.orders order by id").ToList();
            }
        }

        public static List<Income> GetAllIncomesByID()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                return connection.Query<Income>("select * from test.vitta.money_incomes order by id").ToList();
            }
        }

        public static List<Payment> GetAllPaymentsByID()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.GetValue("VittaTestDB")))
            {
                return connection.Query<Payment>("select * from test.vitta.payments order by id").ToList();
            }
        }
    }
}

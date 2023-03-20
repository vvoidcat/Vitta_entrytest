using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VittatestApp.Model;

namespace VittatestApp.ViewModel
{
    class ViewModelMain : ObservableObject
    {
        public List<Order> orders { get; set; } = new List<Order>();
        public List<Income> incomes { get; set; } = new List<Income>();
        public List<Payment> payments { get; set; } = new List<Payment>();

        public ViewModelMain() {
            orders = DataAccess.GetAllOrdersByID();
            incomes = DataAccess.GetAllIncomesByID();
            payments = DataAccess.GetAllPaymentsByID();
        }
    }
}

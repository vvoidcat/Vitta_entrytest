using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public ICommand deleteSelectedPaymentCommand { get; set; }
        public ICommand insertIntoPaymentsCommand { get; set; }
        public ICommand insertIntoOrdersCommand { get; set; }
        public ICommand insertIntoIncomesCommand { get; set; }


        public ViewModelMain() {
            deleteSelectedPaymentCommand = new RelayCommand<int>(DeleteSelectedPayment);
            
            insertIntoPaymentsCommand = new RelayCommand<object>(InsertIntoPayments);
            insertIntoOrdersCommand = new RelayCommand<object>(InsertIntoOrders);
            insertIntoIncomesCommand = new RelayCommand<object>(InsertIntoIncomes); 

            orders = DataAccess.GetAllOrdersByID();
            incomes = DataAccess.GetAllIncomesByID();
            payments = DataAccess.GetAllPaymentsByID();
        }

        public void DeleteSelectedPayment(int selectedIndex)
        {
            // deletion from payments query
        }

        public void InsertIntoPayments(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
                // call insert into payments query
            }
        }

        public void InsertIntoOrders(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
            }
        }

        public void InsertIntoIncomes(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
            }
        }
    }
}

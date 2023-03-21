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
        private string _errorMessage = string.Empty;
        public string errorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value != _errorMessage)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(errorMessage));
                }
            }
        }

        public List<Order> orders { get; set; } = new List<Order>();
        public List<Income> incomes { get; set; } = new List<Income>();
        public List<Payment> payments { get; set; } = new List<Payment>();

        public ICommand updateTables { get; set; }
        public ICommand deleteSelectedPaymentCommand { get; set; }
        public ICommand insertIntoPaymentsCommand { get; set; }
        public ICommand insertIntoOrdersCommand { get; set; }
        public ICommand insertIntoIncomesCommand { get; set; }


        public ViewModelMain() {
            updateTables = new RelayCommand<object>(UpdateTables);

            deleteSelectedPaymentCommand = new RelayCommand<int>(DeleteSelectedPayment);
            
            insertIntoPaymentsCommand = new RelayCommand<object>(InsertIntoPayments);
            insertIntoOrdersCommand = new RelayCommand<object>(InsertIntoOrders);
            insertIntoIncomesCommand = new RelayCommand<object>(InsertIntoIncomes);

            UpdateTables(null);
        }

        private void UpdateTables(object? sender)
        {
            UpdateTableOrders();
            UpdateTableIncomes();
            UpdateTablePayments();
        }

        private void UpdateTableOrders()
        {
            orders = DataAccess.GetAllOrdersOrdered();
            OnPropertyChanged(nameof(orders));
        }

        private void UpdateTableIncomes()
        {
            incomes = DataAccess.GetAllIncomesOrdered();
            OnPropertyChanged(nameof(incomes));
        }

        private void UpdateTablePayments()
        {
            payments = DataAccess.GetAllPaymentsOrdered();
            OnPropertyChanged(nameof(payments));
        }

        private void DeleteSelectedPayment(int selectedIndex)
        {
            // checks for the need to update and deletes a payment
            if (payments is not null && payments.Count > 0 && DataAccess.DeleteFromPayments(payments[selectedIndex].id))
            {
                // should update only corresponding rows !!
                UpdateTables(null);
            }
        }

        private void InsertIntoPayments(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
                long order_id = StringToLong(tuple.Item1);
                long income_id = StringToLong(tuple.Item2);
                decimal sum = StringToDecimal(tuple.Item3);
                
                if (DataAccess.InsertIntoPayments(order_id, income_id, sum))
                {
                    // should update only corresponding rows !!
                    UpdateTables(null);
                }
                else
                {
                    errorMessage = "Payments: data insertion failure: incorrect values";
                }
            }
        }

        private void InsertIntoOrders(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
                decimal sum_total = StringToDecimal(tuple.Item2);
                decimal sum_payed = StringToDecimal(tuple.Item3);

                if (DataAccess.InsertIntoOrders(DateTime.Now, sum_total, sum_payed))
                {
                    UpdateTableOrders();
                } 
                else
                {
                    errorMessage = "Orders: data insertion failure: incorrect values";
                }
            }
        }

        private void InsertIntoIncomes(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
                decimal income = StringToDecimal(tuple.Item2);
                decimal balance = StringToDecimal(tuple.Item3);

                if (DataAccess.InsertIntoIncomes(DateTime.Now, income, balance))
                {
                    UpdateTableIncomes();
                }
                else
                {
                    errorMessage = "Money Incomes: data insertion failure: incorrect values";
                }
            }
        }

        private static decimal StringToDecimal(string str)
        {
            return (Decimal.TryParse(str, out decimal res)) ? res : 0;
        }

        private static long StringToLong(string str)
        {
            return (Int64.TryParse(str, out long res)) ? res : 0;
        }
    }
}

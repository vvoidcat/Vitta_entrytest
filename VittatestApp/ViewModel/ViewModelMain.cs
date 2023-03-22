using System;
using System.Collections.Generic;
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

            orders = DataAccess.GetAllOrdersOrdered();
            incomes = DataAccess.GetAllIncomesOrdered();
            payments = DataAccess.GetAllPaymentsOrdered();
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
            if (payments is not null && payments.Count > 0)
            {
                if (DataAccess.DeleteFromPayments(payments[selectedIndex].id))
                {
                    UpdateTables(null);
                } else
                {
                    UpdateTablePayments();
                }
            }
        }

        private void InsertIntoPayments(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
                long order_id = StringConverter.StringToLong(tuple.Item1);
                long income_id = StringConverter.StringToLong(tuple.Item2);
                decimal sum = StringConverter.StringToDecimal(tuple.Item3);

                if (DataAccess.InsertIntoPayments(order_id, income_id, sum))
                {
                    errorMessage = String.Empty;
                    UpdateTables(null);
                }
                else
                {
                    errorMessage = "Payments: data insertion failure: incorrect values / " +
                        "linking payment to a fully payed order (refresh the page to display the updated order status)";
                }
            }
        }

        private void InsertIntoOrders(object param)
        {
            if (param is not null && param is Tuple<string, string, string>)
            {
                Tuple<string, string, string> tuple = (Tuple<string, string, string>)param;
                decimal sum_total = StringConverter.StringToDecimal(tuple.Item2);
                decimal sum_payed = StringConverter.StringToDecimal(tuple.Item3);

                if (DataAccess.InsertIntoOrders(DateTime.Now, sum_total, sum_payed))
                {
                    errorMessage = String.Empty;
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
                decimal income = StringConverter.StringToDecimal(tuple.Item2);
                decimal balance = StringConverter.StringToDecimal(tuple.Item3);

                if (DataAccess.InsertIntoIncomes(DateTime.Now, income, balance))
                {
                    errorMessage = String.Empty;
                    UpdateTableIncomes();
                }
                else
                {
                    errorMessage = "Money Incomes: data insertion failure: incorrect values";
                }
            }
        }
    }
}

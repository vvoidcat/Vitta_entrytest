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
        public ObservableCollection<Order> orders { get; set; } = new ObservableCollection<Order>()
        {
            new Order(0, DateTime.Now, 100, 100),
            new Order(1, DateTime.Now, 100, 100)
        };

        //public ICommand getOrdersCommand { get; set; }

        public ViewModelMain() { }
    }
}

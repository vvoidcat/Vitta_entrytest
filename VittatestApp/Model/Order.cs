using System;

namespace VittatestApp.Model
{
    class Order
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public decimal sum_total { get; set; }
        public decimal sum_payed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VittatestApp.Model
{
    class Order
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public Decimal sum_whole { get; set; }
        public Decimal sum_payed { get; set; }

        //public Order (long id, DateTime date, Decimal sum_whole, Decimal sum_payed)
        //{
        //    this.id = id;
        //    this.date = date;
        //    this.sum_whole = sum_whole;
        //    this.sum_payed = sum_payed;
        //}
    }
}

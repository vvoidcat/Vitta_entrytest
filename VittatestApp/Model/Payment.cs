using System;

namespace VittatestApp.Model
{
    class Payment
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public long income_id { get; set; }
        public decimal sum { get; set; }
    }
}

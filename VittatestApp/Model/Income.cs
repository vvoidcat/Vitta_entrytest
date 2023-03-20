using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VittatestApp.Model
{
    class Income
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public Decimal incoming_payment { get; set; }
        public Decimal balance { get; set; }
    }
}

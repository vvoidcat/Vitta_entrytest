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
        public decimal income { get; set; }
        public decimal balance { get; set; }
    }
}

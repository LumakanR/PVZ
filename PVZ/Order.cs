using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime ArrivedDate { get; set; }
        public string Status { get; set; }

        public int CellNumber { get; set; }

        public int ClientID { get; set; }
    }
}

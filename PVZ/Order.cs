using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ
{
    public class Order
    {
        public string OrderID { get; set; }
        public DateTime ArrivedDate { get; set; }
        public string Status { get; set; }
        public string ClientPhoneNumber { get; set; }
        public int RackID { get; set; }
        public int CellID { get; set; }
    }
}

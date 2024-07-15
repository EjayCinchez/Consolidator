using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatorScript.Model
{
    public class Tbl_rPhar_StockIn
    {
        public int Indx { get; set; }
        public string Transactionno { get; set; }
        public string Deliveryno { get; set; }
        public DateTime Deliverydate { get; set; }
        public int Supplier { get; set; }
        public long Itemcode { get; set; }
        public int Brandcode { get; set; }
        public int Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal Compprice { get; set; }
        public decimal Selprice { get; set; }
        public DateTime Expirydate { get; set; }
        public int Accountcode { get; set; }
        public int Deliverytype { get; set; }
        public int Location { get; set; }
        public short IsConsumptionAccounted { get; set; }
        public decimal Consumed { get; set; }
        public string Userid { get; set; }
        public string Transdate { get; set; }
        public short Actioncode { get; set; }
        public int Received_by { get; set; }
        public int Balance_updated_by { get; set; }
        public DateTime Balance_updated_on { get; set; }
        public short Is_On_Retail { get; set; }
        public int Tag_on_retail_by { get; set; }
        public DateTime Tag_on_retail_on { get; set; }
        public DateTime Date_received { get; set; }
    }
}

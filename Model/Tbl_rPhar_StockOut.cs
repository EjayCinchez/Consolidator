using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatorScript.Model
{
    public class Tbl_rPhar_StockOut
    {
        public int Indx { get; set; }
        public string Hrno { get; set; }
        public long Logid { get; set; }
        public string Ref_transno { get; set; }
        public string Customername { get; set; }
        public int Accountcode { get; set; }
        public long Itemcode { get; set; }
        public int Brand { get; set; }
        public decimal Rqty { get; set; }
        public decimal Qty { get; set; }
        public decimal Capital { get; set; }
        public decimal Unitprice { get; set; }
        public string Orno { get; set; }
        public string Remarks { get; set; }
        public long Supplier { get; set; }
        public string Enccode { get; set; }
        public short IsConsumptionAccounted { get; set; }
        public decimal Consumed { get; set; }
        public string Userid { get; set; }
        public string Transdate { get; set; }
        public DateTime Date_time_served { get; set; }
        public short Actioncode { get; set; }
    }
}

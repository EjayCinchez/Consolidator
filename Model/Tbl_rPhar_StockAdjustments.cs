using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatorScript.Model
{
    public class Tbl_rPhar_StockAdjustments
    {
        public long Indx { get; set; }
        public string Hrno { get; set; }
        public long Logid { get; set; }
        public short Dept { get; set; }
        public string Ref_transno { get; set; }
        public long Itemcode { get; set; }
        public long Brand { get; set; }
        public long Supplier { get; set; }
        public decimal Qty { get; set; }
        public int Adjtype { get; set; }
        public short Adjcategory { get; set; }
        public string Remarks { get; set; }
        public int Pc_transsource { get; set; }
        public int Pc_costcenter { get; set; }
        public long Pc_categoryid { get; set; }
        public string Pc_unit { get; set; }
        public DateTime Pc_chargeslipdate { get; set; }
        public string Pc_chargeslipno { get; set; }
        public string Pc_accountno { get; set; }
        public decimal Capital { get; set; }
        public decimal Pc_unitprice { get; set; }
        public long Res_person { get; set; }
        public DateTime Datereturned { get; set; }
        public int Fund { get; set; }
        public bool Ischarged { get; set; }
        public short IsConsumptionAccounted { get; set; }
        public decimal Consumed { get; set; }
        public string Dop_key { get; set; }
        public string Dop_enccode { get; set; }
        public int Received_by { get; set; }
        public DateTime Received_on { get; set; }
        public string Userid { get; set; }
        public string Transdate { get; set; }
        public short Actioncode { get; set; }
    }
}

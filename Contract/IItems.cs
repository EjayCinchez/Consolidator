using ConsolidatorScript.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatorScript.Contract
{
    public interface IItems
    {
        Task<IEnumerable<Tbl_rPhar_StockIn>> GetAllItems();
        Task<IEnumerable<Tbl_rPhar_StockIn>> GetAllItems2();
        Task<IEnumerable<Tbl_rPhar_StockIn>> GetItems();
        Task<IEnumerable<Tbl_rPhar_StockOut>> Tbl_rPhar_StockOut(long ItemCode, int AccountCode, int SupplierID, int BrandID);
        Task<IEnumerable<Tbl_rPhar_StockAdjustments>> Tbl_rPhar_StockAdjustments(long ItemCode, int AccountCode, int SupplierID, int BrandID);
        Task<IEnumerable<ConsolidateModel>> Tbl_rPhar_StockOutNew(long ItemCode, int AccountCode, int SupplierID, int BrandID);
    }
}

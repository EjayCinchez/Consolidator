using ConsolidatorScript.Base;
using ConsolidatorScript.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolidatorScript.Model;
using Dapper;

namespace ConsolidatorScript.Implementation
{
    public class Items : IItems
    {
        readonly Connection Connection = new Connection();

        public async Task<IEnumerable<Tbl_rPhar_StockIn>> GetAllItems()
        {
            var SQL = @"select * from tbl_rPhar_StockIn where actioncode = 1";
            using (IDbConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                return await sqlCon.QueryAsync<Tbl_rPhar_StockIn>(SQL, new { }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<Tbl_rPhar_StockIn>> GetAllItems2()
        {
            var SQL = @"SELECT * FROM dbo.tbl_rPhar_StockIn WHERE itemcode NOT IN (SELECT DISTINCT ItemCode FROM dbo.Tbl_ConsolidateItems) AND actioncode = 1";
            using (IDbConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                return await sqlCon.QueryAsync<Tbl_rPhar_StockIn>(SQL, new { }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<Tbl_rPhar_StockIn>> GetItems()
        {
            var SQL = @"with cte as (
	                        select ROW_NUMBER() OVER(partition by supplier, itemcode, brandcode, accountcode order by supplier, itemcode, brandcode,accountcode) rn,* 
	                        from tbl_rPhar_StockIn where actioncode = 1
                        )
                        select * from cte where rn = 1";
            using (IDbConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                return await sqlCon.QueryAsync<Tbl_rPhar_StockIn>(SQL, new { }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<Tbl_rPhar_StockOut>> Tbl_rPhar_StockOut(long ItemCode, int AccountCode, int SupplierID, int BrandID)
        {
            var SQL = @"SELECT *      
                        from tbl_rPhar_StockOut
                        where actioncode=1 and itemcode = @ItemCode and accountcode = @AccountCode and supplier = @SupplierID and brand = @BrandID";
            using (IDbConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                return await sqlCon.QueryAsync<Tbl_rPhar_StockOut>(SQL, new { ItemCode, AccountCode, SupplierID, BrandID }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<Tbl_rPhar_StockAdjustments>> Tbl_rPhar_StockAdjustments(long ItemCode, int AccountCode, int SupplierID, int BrandID)
        {
            var SQL = @"select * from tbl_rPhar_StockAdjustments
                        where actioncode=1 and itemcode = @ItemCode and fund = @FundCode and supplier = @SupplierID and brand = @BrandID";
            using (IDbConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                return await sqlCon.QueryAsync<Tbl_rPhar_StockAdjustments>(SQL, new { ItemCode, FundCode = AccountCode, SupplierID, BrandID }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<ConsolidateModel>> Tbl_rPhar_StockOutNew(long ItemCode, int AccountCode, int SupplierID, int BrandID)
        {
            var SQL = @"SELECT so.accountcode AccountCode,so.supplier SupplierID, so.itemcode ItemCode, so.brand BrandCode, so.capital Sellprice, -sum(case when sa.adjtype=1 then so.qty-isnull(sa.qty,0) else so.qty+isnull(sa.qty,0) end) Balance      
	                   from tbl_rPhar_StockOut so 
	                   left join tbl_rPhar_StockAdjustments sa on sa.ref_transno=so.ref_transno and sa.itemcode=so.itemcode and sa.actioncode=1 
	                   where so.actioncode=1 and so.itemcode = @ItemCode and so.accountcode = @AccountCode and so.supplier = @SupplierID and so.brand = @BrandID
	                   group by so.accountcode,so.supplier, so.itemcode, so.brand , so.capital";
            using (IDbConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                return await sqlCon.QueryAsync<ConsolidateModel>(SQL, new { ItemCode,  AccountCode, SupplierID, BrandID }, commandTimeout: int.MaxValue, commandType: CommandType.Text);
            }
        }
    }
}

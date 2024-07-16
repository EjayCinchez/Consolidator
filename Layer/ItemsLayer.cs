using ConsolidatorScript.Base;
using ConsolidatorScript.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConsolidatorScript.Layer
{
    public class ItemsLayer
    {
        readonly Connection Connection = new Connection();

        public async Task<string> SaveToTable(ConsolidateModel ConsolidateModel)
        {

            var result = "";
            string SQL;

            using (IDbConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SQL = "select count(*) from Tbl_ConsolidateItems where ItemCode=@ItemCode and AccountCode=@AccountCode and BrandCode=@BrandCode and SupplierID=@SupplierID and Sellprice=@Sellprice";
                        int cnt = (int)await connection.ExecuteScalarAsync(SQL, new { 
                            ConsolidateModel.ItemCode,
                            ConsolidateModel.AccountCode,
                            ConsolidateModel.BrandCode,
                            ConsolidateModel.SupplierID,
                            ConsolidateModel.Sellprice,
                        }, transaction, commandTimeout:int.MaxValue, commandType: CommandType.Text);

                        if (cnt >= 1)
                        {
                            SQL = "update Tbl_ConsolidateItems set Balance=@Balance where ItemCode=@ItemCode and AccountCode=@AccountCode and BrandCode=@BrandCode and SupplierID=@SupplierID and Sellprice=@Sellprice";
                            await connection.ExecuteAsync(SQL, new
                            {
                                ConsolidateModel.ItemCode,
                                ConsolidateModel.AccountCode,
                                ConsolidateModel.BrandCode,
                                ConsolidateModel.SupplierID,
                                ConsolidateModel.Sellprice,
                                ConsolidateModel.Balance
                            }, transaction, commandTimeout: int.MaxValue, commandType: CommandType.Text);
                        }
                        else
                        {
                            SQL = "insert into Tbl_ConsolidateItems(ItemCode,AccountCode,SupplierID,BrandCode,Sellprice,Balance) values(@ItemCode,@AccountCode,@SupplierID,@BrandCode,@Sellprice,@Balance)";
                            await connection.ExecuteAsync(SQL, new
                            {
                                ConsolidateModel.ItemCode,
                                ConsolidateModel.AccountCode,
                                ConsolidateModel.BrandCode,
                                ConsolidateModel.SupplierID,
                                ConsolidateModel.Sellprice,
                                ConsolidateModel.Balance
                            }, transaction, commandTimeout: int.MaxValue, commandType: CommandType.Text);
                        }

                        SQL = "insert into Tbl_ConsolidateItems_History(ItemCode,AccountCode,SupplierID,BrandCode,Sellprice,Balance) values(@ItemCode,@AccountCode,@SupplierID,@BrandCode,@Sellprice,@Balance)";
                        await connection.ExecuteAsync(SQL, new
                        {
                            ConsolidateModel.ItemCode,
                            ConsolidateModel.AccountCode,
                            ConsolidateModel.BrandCode,
                            ConsolidateModel.SupplierID,
                            ConsolidateModel.Sellprice,
                            ConsolidateModel.Balance
                        }, transaction, commandTimeout: int.MaxValue, commandType: CommandType.Text);

                        transaction.Commit();
                        result = "success";
                    }
                    catch (Exception ex)
                    {
                        result = $"{ex.Message}";
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            result = $"{ex2.Message}";
                        }
                    }
                }
            }

            return result;
        }
    }
}

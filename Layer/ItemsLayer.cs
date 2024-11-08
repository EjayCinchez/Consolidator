﻿using ConsolidatorScript.Base;
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

        public async Task<ConsolidateModelNew> SaveToTable(ConsolidateModel ConsolidateModel, int LastID)
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
                            SQL = "update Tbl_ConsolidateItems set Balance=@Balance, TransactionDate=getdate() where ItemCode=@ItemCode and AccountCode=@AccountCode and BrandCode=@BrandCode and SupplierID=@SupplierID and Sellprice=@Sellprice";
                            await connection.ExecuteAsync(SQL, new
                            {
                                ConsolidateModel.ItemCode,
                                ConsolidateModel.AccountCode,
                                ConsolidateModel.BrandCode,
                                ConsolidateModel.SupplierID,
                                ConsolidateModel.Sellprice,
                                ConsolidateModel.Balance
                            }, transaction, commandTimeout: int.MaxValue, commandType: CommandType.Text);
                            result = "UPDATED";
                        }
                        else
                        {
                            SQL = "insert into Tbl_ConsolidateItems(ItemCode,AccountCode,SupplierID,BrandCode,Sellprice,Balance,ID) values(@ItemCode,@AccountCode,@SupplierID,@BrandCode,@Sellprice,@Balance,@ID)";
                            await connection.ExecuteAsync(SQL, new
                            {
                                ConsolidateModel.ItemCode,
                                ConsolidateModel.AccountCode,
                                ConsolidateModel.BrandCode,
                                ConsolidateModel.SupplierID,
                                ConsolidateModel.Sellprice,
                                ConsolidateModel.Balance,
                                ID = LastID
                            }, transaction, commandTimeout: int.MaxValue, commandType: CommandType.Text);
                            result = "ADDED";
                        }

                        SQL = "insert into Tbl_ConsolidateItems_History(ItemCode,AccountCode,SupplierID,BrandCode,Sellprice,Balance) values(@ItemCode,@AccountCode,@SupplierID,@BrandCode,@Sellprice,@Balance)";
                        await connection.ExecuteAsync(SQL, new
                        {
                            ConsolidateModel.ItemCode,
                            ConsolidateModel.AccountCode,
                            ConsolidateModel.SupplierID,
                            ConsolidateModel.BrandCode,
                            ConsolidateModel.Sellprice,
                            ConsolidateModel.Balance
                        }, transaction, commandTimeout: int.MaxValue, commandType: CommandType.Text);

                        transaction.Commit();
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

            var Model = new ConsolidateModelNew {
                AccountCode = ConsolidateModel.AccountCode,
                SupplierID = ConsolidateModel.SupplierID,
                ItemCode = ConsolidateModel.ItemCode,
                BrandCode = ConsolidateModel.BrandCode,
                Sellprice = ConsolidateModel.Sellprice,
                Balance = ConsolidateModel.Balance,
                Status = result
            };

            return Model;
        }
    }
}

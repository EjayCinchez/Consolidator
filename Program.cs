using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsolidatorScript.Implementation;
using ConsolidatorScript.Layer;
using ConsolidatorScript.Model;

namespace ConsolidatorScript
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var TimeString = "02:26 pm";
            DateTime Time = DateTime.Parse(TimeString);
            
            

            while (true)
            {
                var i = 1;
                var AllItems = await new Items().GetAllItems2();
                //AllItems = from a in AllItems where a.Itemcode == 1358 && a.Accountcode == 1 && a.Supplier == 6 && a.Brandcode == 24 select a;
                //var Items = await new Items().GetItems();
                if (AllItems.Any()) {
                    foreach (var item in AllItems.OrderByDescending(o => o.Itemcode))
                    {
                        var FinalStock = new List<ConsolidateModel>();


                        var StockIn = from a in AllItems where a.Itemcode == item.Itemcode && a.Supplier == item.Supplier && a.Accountcode == item.Accountcode && a.Brandcode == item.Brandcode select a;
                        var NewStockIn = StockIn.GroupBy(g => new { g.Itemcode, g.Supplier, g.Accountcode, g.Brandcode, g.Compprice })
                            .Select(group => new ConsolidateModel
                            {
                                ItemCode = group.Key.Itemcode,
                                SupplierID = group.Key.Supplier,
                                AccountCode = group.Key.Accountcode,
                                BrandCode = group.Key.Brandcode,
                                Sellprice = group.Key.Compprice,
                                Balance = group.Sum(a => a.Qty)
                            }).ToList();

                        var StockOut = (List<ConsolidateModel>)await new Items().Tbl_rPhar_StockOutNew(item.Itemcode, item.Accountcode, item.Supplier, item.Brandcode);

                        FinalStock.AddRange(NewStockIn);
                        FinalStock.AddRange(StockOut);

                        FinalStock = FinalStock.GroupBy(g => new { g.ItemCode, g.SupplierID, g.AccountCode, g.BrandCode, g.Sellprice })
                                    .Select(group => new ConsolidateModel
                                    {
                                        ItemCode = group.Key.ItemCode,
                                        SupplierID = group.Key.SupplierID,
                                        AccountCode = group.Key.AccountCode,
                                        BrandCode = group.Key.BrandCode,
                                        Sellprice = group.Key.Sellprice,
                                        Balance = group.Sum(a => a.Balance)
                                    }).ToList();

                        if (FinalStock.Any())
                        {
                            foreach (var itemsList in FinalStock)
                            {
                                var ConsolidatedItems = await new Items().ConsolidateModel();
                                var InsertData = await new ItemsLayer().SaveToTable(itemsList, ConsolidatedItems.Max(m => m.ID) + 1);
                                if (InsertData.Status == "ADDED" || InsertData.Status == "UPDATED")
                                    Console.WriteLine("{0}-->{1} . ItemCode:{2}, AccountCode: {3}, SupplierID: {4}, BrandCode: {5}, Sellprice : {6} Updated : {7} ", i, InsertData.Status, InsertData.ItemCode, InsertData.AccountCode, InsertData.SupplierID, InsertData.BrandCode, InsertData.Sellprice, DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                                else
                                    Console.WriteLine("ItemCode:{0}, AccountCode: {1}, SupplierID: {2}, BrandCode: {3}, Sellprice : {4} UNABLE TO UPDATE ", itemsList.ItemCode, itemsList.AccountCode, itemsList.SupplierID, itemsList.BrandCode, itemsList.Sellprice);

                                i++;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine(". . .");
                }
                Thread.Sleep(60 * 500);
                
            }
            
        }
    }
}

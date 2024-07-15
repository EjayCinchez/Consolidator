using System;
using System.Collections.Generic;
using System.Linq;
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
            var TimeString = "02:15 pm";
            DateTime Time = DateTime.Parse(TimeString);
            
            while (true)
            {
                while (DateTime.Now.ToString("hh:mm tt") == Time.ToString("hh:mm tt"))
                {
                    var AllItems = await new Items().GetAllItems();
                    var Items = await new Items().GetItems();
                    foreach (var item in Items)
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

                        var StockOut = (List<ConsolidateModel>) await new Items().Tbl_rPhar_StockOutNew(item.Itemcode, item.Accountcode, item.Supplier, item.Brandcode);
                        
                        FinalStock.AddRange(NewStockIn);
                        FinalStock.AddRange(StockOut);

                        FinalStock = FinalStock.GroupBy(g => new { g.ItemCode, g.SupplierID, g.AccountCode, g.BrandCode,g.Sellprice })
                                    .Select(group => new ConsolidateModel
                                    {
                                        ItemCode = group.Key.ItemCode,
                                        SupplierID = group.Key.SupplierID,
                                        AccountCode = group.Key.AccountCode,
                                        BrandCode = group.Key.BrandCode,
                                        Sellprice = group.Key.Sellprice,
                                        Balance = group.Sum(a => a.Balance)
                                    }).ToList();

                        var InsertData = await new ItemsLayer().SaveToTable(FinalStock);
                        if(InsertData == "success")
                            Console.WriteLine("ItemCode:{0}, AccountCode: {1}, SupplierID: {2}, BrandCode: {3}, Sellprice : {4} Updated : {5} ", FinalStock.FirstOrDefault().ItemCode, FinalStock.FirstOrDefault().AccountCode, FinalStock.FirstOrDefault().SupplierID, FinalStock.FirstOrDefault().BrandCode, FinalStock.FirstOrDefault().Sellprice, DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        else
                            Console.WriteLine("ItemCode:{0}, AccountCode: {1}, SupplierID: {2}, BrandCode: {3}, Sellprice : {4} UNABLE TO UPDATE ", FinalStock.FirstOrDefault().ItemCode, FinalStock.FirstOrDefault().AccountCode, FinalStock.FirstOrDefault().SupplierID, FinalStock.FirstOrDefault().BrandCode, FinalStock.FirstOrDefault().Sellprice);
                        //Console.WriteLine("AccountCode :" +  FinalStock.FirstOrDefault().AccountCode);
                        //Console.WriteLine("SupplierID :" + FinalStock.FirstOrDefault().SupplierID);
                        //Console.WriteLine("ItemCode :" + FinalStock.FirstOrDefault().ItemCode);
                        //Console.WriteLine("BrandCode :" + FinalStock.FirstOrDefault().BrandCode);
                        //Console.WriteLine("Sellprice :" + FinalStock.FirstOrDefault().Sellprice);
                        //Console.WriteLine("Balance :" + FinalStock.FirstOrDefault().Balance);
                        //Thread.Sleep(60 * 500);
                    }
                    Thread.Sleep(60 * 500);
                }
            }
            
        }
    }
}

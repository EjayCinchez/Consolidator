﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatorScript.Model
{
    public class ConsolidateModel
    {
        public int AccountCode { get; set; }
        public long SupplierID { get; set; }
        public long ItemCode { get; set; }
        public long BrandCode { get; set; }
        public decimal Sellprice { get; set; }
        public int Adjtype { get; set; }
        public decimal Balance { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsActive { get; set; }
        public int ID { get; set; }
    }
    public class ConsolidateModelNew : ConsolidateModel
    {
        public string Status { get; set; }
    }
}

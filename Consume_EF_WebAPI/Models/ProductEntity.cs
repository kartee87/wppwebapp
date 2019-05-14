using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consume_EF_WebAPI
{
    public class ProductEntity
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public string ProdDescription { get; set; }
        public decimal Price { get; set; }
    }
}
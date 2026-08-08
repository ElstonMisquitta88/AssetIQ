using System;
using System.Collections.Generic;
using System.Text;

namespace AssetIQ.Models
{
    public class ClientPortfolio
    {
        public string ClientCode { get; set; }
        public Dictionary<string, decimal> Values { get; set; }
    }
}

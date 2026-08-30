using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetIQ.CalculationMcpServer.Models;

public class CalculationResult
{
    public string Formula { get; set; }

    public decimal Result { get; set; }

    public Dictionary<string, decimal> Inputs { get; set; }
}
public class CalculationRequest
{
    public string Formula { get; set; } = string.Empty;

    public Dictionary<string, decimal> Inputs { get; set; }
        = new();
}
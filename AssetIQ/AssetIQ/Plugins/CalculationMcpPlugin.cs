using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

public class CalculationMcpPlugin
{
    private readonly Kernel _kernel;
    private readonly KernelFunction _mcpCalculateFunction;

    public CalculationMcpPlugin(
        Kernel kernel,
        KernelFunction mcpCalculateFunction)
    {
        _kernel = kernel;
        _mcpCalculateFunction = mcpCalculateFunction;
    }

    [KernelFunction]
    [Description(
        "Calculates a financial formula using the supplied input values. " +
        "Use this tool for all financial calculations."
    )]
    public async Task<string> CalculateAsync(
        string formula,
        Dictionary<string, decimal> inputs)
    {
        var inputsJson = JsonSerializer.SerializeToElement(inputs);

        var result = await _kernel.InvokeAsync(
            _mcpCalculateFunction,
            new KernelArguments
            {
                ["formula"] = formula,
                ["inputs"] = inputsJson
            });

        return result.ToString();
    }
}
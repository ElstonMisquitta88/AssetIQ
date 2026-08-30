using AssetIQ.CalculationMcpServer.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AssetIQ.CalculationMcpServer.Tools;

[McpServerToolType]
public class CalculationTools
{
    [McpServerTool]
    [Description(
     "Calculates a financial formula using the supplied input values."
 )]
    public CalculationResult Calculate(
     string formula,
     Dictionary<string, decimal> inputs)
    {
        try
        {
            Console.Error.WriteLine("=== Calculate invoked ===");
            Console.Error.WriteLine($"Formula: {formula}");

            Console.Error.WriteLine(
                $"Inputs: {string.Join(", ",
                    inputs.Select(x => $"{x.Key}={x.Value}"))}");

            // Make keys case-insensitive
            inputs = new Dictionary<string, decimal>(
                inputs,
                StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(formula))
                throw new ArgumentException("Formula cannot be empty.");

            if (inputs == null || inputs.Count == 0)
                throw new ArgumentException("Input values cannot be empty.");

            var tokens = formula.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
                throw new ArgumentException("Invalid formula.");

            if (!inputs.TryGetValue(tokens[0], out var result))
                throw new ArgumentException(
                    $"Value for '{tokens[0]}' was not provided.");

            for (int i = 1; i < tokens.Length; i += 2)
            {
                if (i + 1 >= tokens.Length)
                    throw new ArgumentException("Invalid formula.");

                var operation = tokens[i];
                var field = tokens[i + 1];

                if (!inputs.TryGetValue(field, out var value))
                    throw new ArgumentException(
                        $"Value for '{field}' was not provided.");

                switch (operation)
                {
                    case "+":
                        result += value;
                        break;

                    case "-":
                        result -= value;
                        break;

                    default:
                        throw new ArgumentException(
                            $"Operator '{operation}' is not supported.");
                }
            }

            return new CalculationResult
            {
                Formula = formula,
                Result = result,
                Inputs = inputs
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("=== CALCULATION ERROR ===");
            Console.Error.WriteLine(ex.ToString());

            throw;
        }
    }
}
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using AssetIQ.Models;

namespace AssetIQ.Plugins
{
    public class PortfolioPlugin
    {
        private readonly string _clientCode;
        public PortfolioPlugin(string clientCode)
        {
            _clientCode = clientCode;
        }

        [KernelFunction]
        [Description(
            "Retrieves portfolio values required to calculate a financial metric. " +
            "When a metric definition contains RequiredFields such as ALB, SPAN, THV or MTF, " +
            "call this function to retrieve those values for the current client. " +
            "Do not ask the user to provide these values."
        )]
        public Dictionary<string, decimal> GetPortfolioValues(
        List<string> requiredFields)
        {
            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "portfolio.json");

            if (!File.Exists(filePath))
                return new Dictionary<string, decimal>();

            var json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var clients = JsonSerializer.Deserialize<List<ClientPortfolio>>(
                json, options) ?? new List<ClientPortfolio>();

            var client = clients.FirstOrDefault(x =>
                x.ClientCode.Equals(_clientCode, StringComparison.OrdinalIgnoreCase));

            if (client == null)
                return new Dictionary<string, decimal>();

            Dictionary<string, decimal> test = client.Values
                .Where(x => requiredFields.Contains(
                    x.Key, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value);

            return client.Values
                .Where(x => requiredFields.Contains(
                    x.Key, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

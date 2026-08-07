using AssetIQ.Models;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace AssetIQ
{
    public class MetricsPlugin
    {
        //[KernelFunction]
        //[Description("Retrieves all supported capital market metrics and their metadata. " +
        //    "Use this to determine the correct metric for user requests such as " +
        //    "net worth, available margin, pledged value, or other financial metrics.")]
        //public List<MetricDefinition> GetAllMetrics()
        //{
        //    var filePath = Path.Combine(
        //    AppDomain.CurrentDomain.BaseDirectory,
        //    "Data",
        //    "metrics.json");

        //    var json = File.ReadAllText(filePath);
        //    var options = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };


        //    if (!File.Exists(filePath))
        //        return new List<MetricDefinition>();

        //    return JsonSerializer.Deserialize<List<MetricDefinition>>(json, options)
        //           ?? new List<MetricDefinition>();
        //}

        [KernelFunction]
        [Description("Finds the financial metric that best matches the user's request.")]
        public MetricDefinition? FindMetric(string userRequest)
        {
            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "metrics.json");

            var json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var metrics = JsonSerializer.Deserialize<List<MetricDefinition>>(json, options)
                          ?? new List<MetricDefinition>();

            userRequest = userRequest.ToLowerInvariant();

            MetricDefinition ?RefinedSerach = metrics.FirstOrDefault(m =>
               m.Metric.Contains(userRequest, StringComparison.OrdinalIgnoreCase)
            || m.DisplayName.Contains(userRequest, StringComparison.OrdinalIgnoreCase)
            || m.Description.Contains(userRequest, StringComparison.OrdinalIgnoreCase)
            || m.Aliases.Any(a =>
                   userRequest.Contains(a, StringComparison.OrdinalIgnoreCase)));

            return RefinedSerach;
        }
    }
}

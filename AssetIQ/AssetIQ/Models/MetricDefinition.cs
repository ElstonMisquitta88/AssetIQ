namespace AssetIQ.Models;
public class MetricDefinition
{
    public string Metric { get; set; }

    public string DisplayName { get; set; }

    public List<string> Aliases { get; set; }

    public string Description { get; set; }

    public string Formula { get; set; }

    public List<string> RequiredFields { get; set; }
}

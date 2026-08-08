using AssetIQ.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AssetIQ;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        string currentClientCode = "C002";

        var configuration = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: true)
             .AddUserSecrets<Form1>()
             .Build();

        string model = configuration.GetValue<string>("OpenAI:Model") ?? throw new Exception("OpenAI:Model not found.");
        string apiKey = configuration.GetValue<string>("OpenAI:ApiKey") ?? throw new Exception("OpenAI:ApiKey not found.");

        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(
            modelId: model,
            apiKey: apiKey
        );
        var kernel = builder.Build();
        
        kernel.Plugins.AddFromObject(new MetricsPlugin());
        kernel.Plugins.AddFromObject(new PortfolioPlugin(currentClientCode));

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var result = await kernel.InvokePromptAsync(
    "networth",
       new(settings));

        txt_result.Text = result.ToString();
    }
}


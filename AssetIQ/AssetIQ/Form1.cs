using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;

namespace AssetIQ;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
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
        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var result = await kernel.InvokePromptAsync(
    "margin",
       new(settings));

        Console.WriteLine(result);

    }
}


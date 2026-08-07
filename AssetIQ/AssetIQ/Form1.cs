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

    private void Form1_Load(object sender, EventArgs e)
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



    //    kernel.Plugins.AddFromObject(new GreetingPlugin());
    //    kernel.Plugins.AddFromObject(new RepldgeFilePlugin());

    //    var settings = new OpenAIPromptExecutionSettings
    //    {
    //        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    //    };

    //    var result = kernel.InvokePromptAsync(
    //"Need to generate Repledge File for the day",
    //   new(settings));

    //    Console.WriteLine(result);

    }
}

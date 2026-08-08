using AssetIQ.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AssetIQ;

public partial class Form1 : Form
{
    string currentClientCode = "";
    private readonly ChatHistory _chatHistory = new();

    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {

        txt_question_log.AppendText(
      $"User Question : History \r\n");

    }

    private async void btn_query_Click(object sender, EventArgs e)
    {
       try
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                MessageBox.Show("Please enter a question.");
                return;
            }

            btn_query.Enabled = false;
            btn_query.Text = "Thinking...";

            currentClientCode = "C002";

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
            kernel.Plugins.AddFromObject(new CalculationPlugin());

            var question = txt_userquestion.Text;
            txt_question_log.AppendText(
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - User Question: {question}\r\n");
            _chatHistory.AddUserMessage(question);

            var settings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };
            var chatService = kernel.GetRequiredService<IChatCompletionService>();

            var result = await chatService.GetChatMessageContentAsync(
             _chatHistory,
             settings,
             kernel);
            _chatHistory.AddAssistantMessage(result.Content!);
            txt_result.Text = result.Content!.ToString();
            txt_userquestion.Text = string.Empty;
        }
        catch
        {
            MessageBox.Show("An error occurred while processing your request.");
        }
        finally
        {
            btn_query.Text = "Ask";
            btn_query.Enabled = true;
        }
    }
}


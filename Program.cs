using Azure;
using Azure.AI.Inference;
using System.Text.Json;

class Program
{
    private const int MaxTokens = 2048;
    private class AppConfig
    {
        public string? ApiKey { get; set; }
        public string? ModelName { get; set; }
        public string? Endpoint { get; set; }
    }

    static void Main(string[] args)
    {
        const string ConfigFileName = "appsettings.secret.json";
        if (!File.Exists(ConfigFileName))
        {
            Console.WriteLine($"Configuration file '{ConfigFileName}' not found. Please create it with ApiKey, ModelName, and Endpoint.");
            return;
        }
        AppConfig? config;
        try
        {
            string configJson = File.ReadAllText(ConfigFileName);
            config = JsonSerializer.Deserialize<AppConfig>(configJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read or parse configuration: {ex.Message}");
            return;
        }
        if (config == null || string.IsNullOrWhiteSpace(config.ApiKey) || string.IsNullOrWhiteSpace(config.ModelName) || string.IsNullOrWhiteSpace(config.Endpoint))
        {
            Console.WriteLine("ApiKey, ModelName, and Endpoint must be set in the configuration file.");
            return;
        }
        System.Diagnostics.Debug.Assert(config.ApiKey != null);
        System.Diagnostics.Debug.Assert(config.ModelName != null);
        System.Diagnostics.Debug.Assert(config.Endpoint != null);
        var modelName = config.ModelName!;
        var endpointUri = new Uri(config.Endpoint!);
        var apiKey = config.ApiKey!;
        var credential = new AzureKeyCredential(apiKey);
        var client = new ChatCompletionsClient(
            endpointUri,
            credential,
            new AzureAIInferenceClientOptions(AzureAIInferenceClientOptions.ServiceVersion.V2024_05_01_Preview)
        );
        var chatHistory = new List<ChatRequestMessage>();
        Console.WriteLine("Chat with your Azure AI agent! Type 'exit' to quit.");
        while (true)
        {
            Console.Write("User > ");
            string? userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput) || userInput.Trim().ToLower() == "exit")
                break;
            chatHistory.Add(new ChatRequestUserMessage(userInput));
            var requestOptions = new ChatCompletionsOptions()
            {
                MaxTokens = MaxTokens,
                Model = modelName
            };
            foreach (var message in chatHistory)
                requestOptions.Messages.Add(message);
            try
            {
                Response<ChatCompletions> response = client.Complete(requestOptions);
                string assistantReply = response.Value.Content;
                Console.WriteLine($"Assistant > {assistantReply}");
                chatHistory.Add(new ChatRequestAssistantMessage(assistantReply));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }
        Console.WriteLine("Chat ended.");
    }
}
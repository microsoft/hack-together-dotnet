using Microsoft.Extensions.Configuration;
using Azure.AI.OpenAI;
using Azure;

var builder = new ConfigurationBuilder();
builder.AddUserSecrets<Program>();
var configuration = builder.Build();

Uri proxyUrl = new(configuration["aoai-endpoint-url"]!);
AzureKeyCredential token = new(configuration["aoai-token"]!);

string systemMessage = "You are a hilarious assistant that helps people find hikes where they live. You are helpful and like to tell jokes while helping.";

OpenAIClient openAIClient = new(proxyUrl, token);

string userPrompt = "What is the best hike near Seattle?";

ChatCompletionsOptions completionsOptions = new() {
    MaxTokens=2048,
    Temperature=0.7f,
    NucleusSamplingFactor= 0.95f,
    DeploymentName = "gpt-35-turbo"
};

completionsOptions.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));
completionsOptions.Messages.Add(new ChatMessage(ChatRole.User, userPrompt));

ChatCompletions response = await openAIClient.GetChatCompletionsAsync(completionsOptions);

Console.WriteLine(response.Choices.First().Message.Content);

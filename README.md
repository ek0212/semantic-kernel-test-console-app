# Semantic Kernel Test Console App

A C# console app for chatting with Azure OpenAI models using the Azure.AI.Inference SDK.

## Technologies Used
- C#
- .NET 9.0
- Azure.AI.Inference SDK
- Azure OpenAI

## Quick Start
1. **Clone & Restore:**
   ```sh
   git clone <your-repo-url>
   cd semantic-kernel-test
   dotnet restore
   ```
2. **Configure:**
Create `SemanticKernelTestConsoleApp/appsettings.secret.json`:
```json
{
   "ApiKey": "YOUR_AZURE_API_KEY_HERE",
   "ModelName": "YOUR_MODEL_NAME_HERE",
   "Endpoint": "https://YOUR_ENDPOINT_HERE"
}
```
3. **Run:**
   ```sh
   dotnet run --project SemanticKernelTestConsoleApp
   ```

## Security
- `appsettings.secret.json` is gitignored. Never commit secrets.

## Troubleshooting
- Check config file for typos or missing fields.
- Ensure .NET 9.0+ is installed (`dotnet --version`).

## Modularity & Extensibility
- The app is designed for easy extension. Add new features or refactor logic as needed.
- Use named constants and explicit variable names for maintainability.
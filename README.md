# PaymentApplicationWithAzureFunction

![image](https://user-images.githubusercontent.com/3984110/211048267-c87bbab4-5a5b-4e34-bb3b-06f792d33a17.png)



local.settings.json

{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage":"",
        "SendGridApiKey" : "", // Read from key vault
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "EmailSender": ""
    }
}

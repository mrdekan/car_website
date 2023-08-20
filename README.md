
To run the project, you need to create an appsettings.json file next to the Program.cs file and paste the following content (note that you need to fill in all the specified fields):
```
{
  "ConnectionStrings": {
    "MongoDB": "YOUR_MONGODB_CONNECTION_STRING",
    "DBName": "YOUR_MONGODB_DATABASE_NAME"
  },
  "SendGridSettings": {
    "ApiKey": "YOUR_SENDGRID_API_KEY"
  },
  "GoogleApiSettings": {
    "ApiKey": "YOUR_GOOGLE_API_KEY"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "PhotoStoragePath": "Photos",
  "AllowedHosts": "*"
}

```

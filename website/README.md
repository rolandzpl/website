# __website__ project

## Configuring ```website```

```json
{
  "Website": {
    "Name": "My Web Page",
    "StartPage": "/start",
    "DataPath":"...\\website-data\\pages",
    "ImagesPath": "...\\website-data\\images",
    "Administrator": {
      "Email": "Jon Smith <john.smith@mail.com>"
    }
  },
  "Smtp": {
    "SmtpServer": "mail.com",
    "Port": 465,
    "SslEnabled": true,
    "Username": "john.smith@mail.com"
    "Password": "***********"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## Smtp functionality in ```website```

``website`` can send emails through a SMTP server. You configure it in _Smtp_ section in the configuration. However,
it is not the right place for username and password so you should set it as environment variables. Set environment variables ``Bronekfoto_Smtp__Password`` and ``Bronekfoto_Smtp__Username`` and remove those from _appsettings.json_ file

## Deploying to __raspberry pi__

Service file:
```
[Unit]
Description=Description of the service

[Service]
WorkingDirectory=/var/www/my-web-service
ExecStart=/var/www/my-web-service/website --urls=http://0:5100
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
#SyslogIdentifier=dotnet-example
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_CLI_UI_LANGUAGE=en
Environment=DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

[Install]
WantedBy=multi-user.target
```

## Developer's Notes

### Conflict between dynamic route and static files provider

When using ``app.MapDynamicPageRoute<MyTransformer>("{**slug}")`` as a catch-all route there was an issue
that all urls were being recognized by that route causing all static files requests (like js libraries) going
to ``NyTransformer`` and ``DynamicPage`` which obviously couldn't find the files. The entire page stopped working.

Solution came with route constraints. Implemented as ``PageExistsRouteConstraint``, checking if the real
page exists in pages' directory and accepting the route then. Otherwise route gets declined and static files middleware
gets into the game.

### Create directory only if not exists

During deployment, the directories must be assured to exist. Nice command to create one, in linux environment, looks like:

```
ssh pi@192.168.0.169 "sudo mkdir -p /var/log/bronekfoto"
```

The switch ``-p`` assures the diectory and all it's parent directories to be created if missing.


# BuscaPartituraMQ
A music sheet searcher that uses some cool things on its architecture.

Technologies used:

* RabbitMQ
* SQLite
* .NET Core 3.1
* Azure Functions
* Telegram API (as interface)
---

## Architecture

![Architecture diagram](.\Images\Diagram.png)

---
## Testing

At EnvironmentHelper.cs (BuscadorPartitura.Infra project) you can mock a file with centralized variables, such as MQ informations, tokens, connection strings etc. with this sintax:

VARIABLE1|VALUE1

VARIABLE2|VALUE2

(...)

Save this file and fill GetValue() method with its full path.

Debug "BuscadorPartitura.Orquestrador", "BuscadorPartitura.Controller" and "BuscadorPartitura.Presentation.Telegram" projects.

---
## Some useful links
* [Rabbit MQ API](https://www.rabbitmq.com/dotnet-api-guide.html)
* [Telegram API](https://github.com/TelegramBots/Telegram.Bot)
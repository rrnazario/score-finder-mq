using ScoreFinder.Core.Interfaces;
using ScoreFinder.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ScoreFinder.Presentation.Telegram
{
    public static class Injection
    {
        private static ServiceProvider _provider;

        /// <summary>
        /// For more info about static constructors, see: https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/classes-and-structs/static-constructors
        /// </summary>
        static Injection()
        {
            _provider = new ServiceCollection()
            .AddSingleton<IMessageQueueConnection, RabbitConnectionService>()
            .BuildServiceProvider();
        }

        /// <summary>
        /// Get service injected based on T type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() => _provider.GetService<T>();
    }
}

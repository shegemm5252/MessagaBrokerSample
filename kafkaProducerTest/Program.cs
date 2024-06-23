
using Kafka.Shared;
using KafkaLibrary;
using KafkaLibrary.KafkaProducer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = GetConfiguration();

var services = new ServiceCollection()
       .AddSingleton<IConfiguration>(GetConfiguration());



services.AddKafkaServices(configuration.GetSection("producer"));

var serviceProvider = services.BuildServiceProvider();

var messageProducers = serviceProvider.GetService<IMessageProducers>()!;

Console.WriteLine("Supply message for the topic testTopic");
var msg = Console.ReadLine();

while (msg != null)
{
    var producer = messageProducers.ProduceAsync<string>("testTopic", msg).ConfigureAwait(false);
    msg = Console.ReadLine();
}



Console.WriteLine("Hello, World!");




static IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
}




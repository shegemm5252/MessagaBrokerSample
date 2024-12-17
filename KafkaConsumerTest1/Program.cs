
using Kafka.Consumer;
using KafkaLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var configuration = GetConfiguration();

var services = new ServiceCollection()
       .AddSingleton<IConfiguration>(configuration);

var serviceProvider = services.BuildServiceProvider();
//services.AddScoped<Consumer>();
services.AddKafkaServices<Consumer>(configuration.GetSection("Kafka"));




Console.WriteLine("Consume message from the topic testTopic 2");
var msg = Console.ReadLine();

while (msg != null)
{

    msg = Console.ReadLine();
}





Console.WriteLine("Hello, World!");




static IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
}




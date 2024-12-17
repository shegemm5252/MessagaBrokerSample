


using Confluent.Kafka;
using Kafka.Shared;
using KafkaLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


var configuration = GetConfiguration();

var services = new ServiceCollection()
       .AddSingleton(configuration);

var serviceProvider = services.BuildServiceProvider();
//services.AddScoped<Consumer>();
//services.AddKafkaServices<Consumer>(configuration.GetSection("Kafka"));

//services.AddKafkaServices<Consumer2>(configuration.GetSection("Kafka"));



services.AddKafkaServices(cfg =>
{
    cfg.Configure(configuration.GetSection("Kafka"));
    //cfg.RegisterConsumer<Consumer>();
    //cfg.RegisterConsumer<Consumer2>();

     //cfg.RegisterConsumer(Assembly.GetExecutingAssembly());

    //cfg.RegisterConsumer(Assembly.GetCallingAssembly());

    cfg.RegisterConsumer(Assembly.Load("Kafka.Shared"));
});

Console.WriteLine("Consume message from the topic testTopic 1");
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




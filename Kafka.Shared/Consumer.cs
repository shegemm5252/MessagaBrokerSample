using KafkaLibrary;
using KafkaLibrary.Consumer;
using KafkaLibrary.KafkaProducer;
using Microsoft.Extensions.Configuration;

namespace Kafka.Shared
{
    public class Consumer : ConsumerBase
    {
    
        public Consumer(KafkaConfig configuration, IMessageProducers producers) : base("testKafka", configuration)
        {

        }


        public override async Task Invoke()
        {

            await ConsumeAsync<string>("testTopic", (value) =>
            {
                Console.WriteLine(value);


            });



            await base.Invoke();
        }
    }
}

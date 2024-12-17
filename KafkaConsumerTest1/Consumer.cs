using KafkaLibrary;
using KafkaLibrary.Consumer;
using KafkaLibrary.KafkaProducer;

namespace Kafka.Consumer
{
    public class Consumer : ConsumerBase
    {
    
        public Consumer(KafkaConfig configuration, IMessageProducers producers) : base("test2", configuration)
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

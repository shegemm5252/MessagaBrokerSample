using KafkaLibrary;
using KafkaLibrary.Consumer; 
using KafkaLibrary.Interface;
using KafkaLibrary.KafkaProducer;

namespace Kafka.Shared
{
    public class Consumer : ConsumerBase
    {
    
        public Consumer(KafkaConfig configuration, IMessageProducers producers, IMessageAdmin messageAdmin) : base("test1", configuration, messageAdmin)
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

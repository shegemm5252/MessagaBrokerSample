using KafkaLibrary.Consumer;
using KafkaLibrary.KafkaProducer;
using KafkaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaLibrary.Interface;

namespace Kafka.Shared
{
    public class Consumer2 : ConsumerBase
    {

        public Consumer2(KafkaConfig configuration, IMessageProducers producers, IMessageAdmin messageAdmin) : base("test2", configuration, messageAdmin)
        {

        }

        public override async Task Invoke()
        {

            await ConsumeAsync<string>("testTopic44455", (value) =>
            {
                Console.WriteLine(value);


            });



            await base.Invoke();
        }
    }
}

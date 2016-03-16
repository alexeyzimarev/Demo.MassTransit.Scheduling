using System;
using System.Threading.Tasks;
using MassTransit;

namespace CheckMTScheduler
{
    public class NotificationConsumer : IConsumer<ISendNotification>
    {
        public async Task Consume(ConsumeContext<ISendNotification> context)
        {
            Console.WriteLine(context.Message.EmailAddress);
        }
    }
}
using System;
using System.Threading.Tasks;
using MassTransit;

namespace CheckMTScheduler
{
    public class ScheduleNotificationConsumer : IConsumer<IScheduleNotification>
    {
        private readonly Uri _notificationService = new Uri("rabbitmq://localhost/schedule_test_queue");

        public async Task Consume(ConsumeContext<IScheduleNotification> context)
        {
            Console.WriteLine("Received the request, scheduling message in the future");
            await context.ScheduleMessage(_notificationService,
                context.Message.DeliveryTime,
                new SendNotificationCommand
                {
                    EmailAddress = context.Message.EmailAddress,
                    Body = context.Message.Body
                });
        }

        class SendNotificationCommand : ISendNotification
        {
            public string EmailAddress { get; set; }
            public string Body { get; set; }
        }
    }
}
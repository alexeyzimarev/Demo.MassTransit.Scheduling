using System;
using MassTransit;

namespace CheckMTScheduler
{
    internal class Program
    {
        private static void Main()
        {
            var busControl = ConfigureBus();

            busControl.Start();

            var sendEndpoint = busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/schedule_test_queue")).Result;
            sendEndpoint.Send(new ScheduleNotification
            {
                EmailAddress = "test@acme.org",
                Body = "Message body",
                DeliveryTime = DateTime.Now + TimeSpan.FromSeconds(30)
            });
            Console.WriteLine("Message sent, press Enter to stop waiting");
            Console.ReadLine();

            busControl.Stop();
        }

        private static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.UseMessageScheduler(new Uri("rabbitmq://localhost/masstransit_quartz_scheduler"));

                cfg.ReceiveEndpoint(host, "schedule_test_queue", e =>
                {
                    e.Consumer<ScheduleNotificationConsumer>();
                    e.Consumer<NotificationConsumer>();
                });
            });
        }

        public class ScheduleNotification : IScheduleNotification
        {
            public DateTime DeliveryTime { get; set; }
            public string EmailAddress { get; set; }
            public string Body { get; set; }
        }
    }
}
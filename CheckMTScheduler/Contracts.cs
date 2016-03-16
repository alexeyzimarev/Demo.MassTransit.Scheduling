using System;

namespace CheckMTScheduler
{
    public interface IScheduleNotification
    {
        DateTime DeliveryTime { get; }
        string EmailAddress { get; }
        string Body { get; }
    }

    public interface ISendNotification
    {
        string EmailAddress { get; }
        string Body { get; }
    }
}
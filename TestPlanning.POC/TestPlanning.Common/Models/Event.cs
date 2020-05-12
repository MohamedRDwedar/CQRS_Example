using System;
using TestPlanning.Common.Interfaces;

namespace TestPlanning.Common.Models
{
    public abstract class Event : IEvent
    {
        public long AggregateRootId { get; private set; }

        public long TimeStamp { get; private set; }

        private Event()
        {
            TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }
    }
}

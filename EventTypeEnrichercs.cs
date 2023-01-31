﻿using Serilog.Core;
using Serilog.Events;
using System.Text;

namespace RestaurantScheduler
{
    class EventTypeEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var numericHash = Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text.Substring(0,7));
            var eventId = propertyFactory.CreateProperty("EventType", numericHash);
            logEvent.AddPropertyIfAbsent(eventId);
        }
    }

}

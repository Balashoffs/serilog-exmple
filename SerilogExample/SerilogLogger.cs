using System;
using System.Collections.Generic;
using CustomLogger;
using Serilog;
using Serilog.Core;
using SerilogExample.SeriogLogger;

namespace SerilogExample
{
    public class SerilogLogger : ICustomLogger
    {
        public void Init(SeqSecrets secrets, Dictionary<string, object> properties)
        {
            ILogEventEnricher[] eventEnrichers = EventEnrichers.Build(properties);
            Log.Logger = SerilogLoggerConfig.Build(secrets, eventEnrichers);
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            Log.Logger.Information(messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            Log.Logger.Debug(messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            Log.Logger.Warning(messageTemplate, propertyValues);
        }
        
        public void Warning(Exception exception, string message = "")
        {
            Log.Logger.Warning(exception, message);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            Log.Logger.Error(messageTemplate, propertyValues);
        }
        
        public void Error(Exception exception, string message = "")
        {
            Log.Logger.Error(exception, message);
        }


        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            Log.Logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string message = "")
        {
            Log.Logger.Fatal(exception, message);
        }
        

        public void Close()
        {
            Log.CloseAndFlush();
        }
    }
}
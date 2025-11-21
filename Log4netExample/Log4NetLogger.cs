using System;
using System.Collections.Generic;
using System.IO;
using CustomLogger;
using log4net;
using Log4Net.Async;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Seq.Client.Log4Net;

namespace Log4netExample
{
    public class Log4NetLogger : ICustomLogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Log4NetLogger));


        public void Init(SeqSecrets secrets, Dictionary<string, object> properties)
        {
           
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            SetSeqAppender(secrets);
            SetGlobalTag(properties);
            
        }

        private void SetGlobalTag(Dictionary<string, object> properties)
        {
            foreach (var keyValuePair in properties)
            {
                LogicalThreadContext.Properties[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        private void SetSeqAppender(SeqSecrets secrets)
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.Level = Level.All;

            // Create a Seq appender
            var seqAppender = new SeqAppender
            {
                ServerUrl = secrets.Host,
                ApiKey = secrets.ApiKey,
                Name = "SeqAppender",
                BufferSize = 1,
                Evaluator = new LevelEvaluator(Level.All),
            };
            
            AsyncForwardingAppender asyncForwardingAppender = new AsyncForwardingAppender();
            asyncForwardingAppender.AddAppender(seqAppender);
            asyncForwardingAppender.ActivateOptions();
            // Add the appender to the root logger
            hierarchy.Root.AddAppender(asyncForwardingAppender);
            
            // Configure the repository
            hierarchy.Configured = true;
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            log.InfoFormat(messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            log.DebugFormat(messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            log.WarnFormat(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string message = "")
        {
            log.Warn(message, exception);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            log.ErrorFormat(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string message = "")
        {
            log.Error(message, exception);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            log.FatalFormat(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string message  = "")
        {
            log.Fatal(message, exception);
        }

        public void Close()
        {
            LogManager.Shutdown();
        }
    }
}
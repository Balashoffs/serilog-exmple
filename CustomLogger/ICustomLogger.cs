using System;
using System.Collections.Generic;

namespace CustomLogger
{
    public interface ICustomLogger
    {
        void Init(SeqSecrets secrets, Dictionary<string, object> properties);
        void Info(string messageTemplate, params object[] propertyValues);
        void Debug(string messageTemplate, params object[] propertyValues);
        void Warning(string messageTemplate, params object[] propertyValues);
        void Warning(Exception exception, string message = "");
        
        void Error(string messageTemplate, params object[] propertyValues);
        void Error(Exception exception, string message = "");
        void Fatal(string messageTemplate, params object[] propertyValues);
        void Fatal(Exception exception, string message = "");
        void Close();
    }
}
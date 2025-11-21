using System.Collections.Generic;
using CustomLogger;

namespace RevitPlugin
{
    public class SetupData
    {
        public SeqSecrets Secrets { get; private set; }
        public Dictionary<string, object> Properties { get; private set; }
        public ICustomLogger CustomLogger { get; private set; }     

        public SetupData(ICustomLogger logger,SeqSecrets secrets, Dictionary<string, object> properties )
        {
            Properties = properties;
            Secrets = secrets;
            CustomLogger = logger;
        }
    }
}
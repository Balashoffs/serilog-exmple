using System;
using System.Collections.Generic;
using System.Threading;
using CustomLogger;
using Log4netExample;
using SerilogExample;

namespace RevitPlugin
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<ICustomLogger> loggers = new List<ICustomLogger>();
            // loggers.Add(new SerilogLogger());
            loggers.Add(new Log4NetLogger());
            SeqSecrets secrets = new SeqSecrets();
            for (var i = 0; i < loggers.Count; i++)
            {
                ICustomLogger logger = loggers[i];
                Thread thread = new Thread(RunLogger);
                Dictionary<string, object> properties = new Dictionary<string, object>()
                {
                    { "PluginName", $"Plugin_{i}" },
                    { "RevitUser", "bau" },
                    { "Logger", logger.GetType().ToString() },
                };
                SetupData setupData = new SetupData(logger, secrets, properties);
               thread.Start(setupData);
            }
        }

        private static void RunLogger(object obj)
        {
            SetupData setupData = (SetupData)obj;
            ICustomLogger logger = setupData.CustomLogger;

            logger.Init(setupData.Secrets, setupData.Properties);
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    var ex = eventArgs.ExceptionObject as Exception;
                    logger.Fatal(ex, "Unhandled exception");
                    logger.Close();
                };

                Thread.Sleep(1000);
                long id = DateTime.Now.Ticks;
                int i = 0;
                logger.Info("{0}: Start loop", id);

                while (true)
                {
                    var message = RandomStringGenerator.GenerateRandomString();
                    var delay = RandomStringGenerator.GetRandomNumber();

                    Thread.Sleep(delay);
                    message = RandomStringGenerator.GenerateRandomString();
                    delay = RandomStringGenerator.GetRandomNumber();
                    logger.Info("{0}: {1} - {2}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    logger.Debug("{0}: {1} - {2}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    logger.Warning("{0}: {1} - {2}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    logger.Error("{0}: {1} - {2}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    logger.Fatal("{0}: {1} - {2}", id, delay, message);
                    Thread.Sleep(delay);
                    try
                    {
                        throw new Exception("Erorr");
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, e.ToString());
                    }

                    i++;
                    if (i > 100)
                    {
                        logger.Info("{0}: Close loop", id);
                        break;
                    }
                }

                Thread.Sleep(1000);
                throw new InvalidOperationException("Unhandled exception");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Unhandled exception");
            }
            finally
            {
                logger.Close();
            }
        }
    }
}
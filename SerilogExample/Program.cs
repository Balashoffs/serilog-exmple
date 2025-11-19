using System;
using System.Collections.Generic;
using System.Threading;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using SerilogExample.SeriogLogger;

namespace SerilogExample
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SeqSecrets secrets = new SeqSecrets();
            ILogEventEnricher[] eventEnrichers = EventEnrichers.Build(new Dictionary<string, string>());
            Log.Logger = SerilogLoggerConfig.Build(secrets, eventEnrichers);

            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    var ex = eventArgs.ExceptionObject as Exception;
                    Log.Fatal(ex, "Unhandled exception");
                    Log.CloseAndFlush();
                };

                Thread.Sleep(1000);
                int id = DateTime.Now.Millisecond;
                int i = 0;
                Log.Information("{ID}: Start loop", id);

                while (true)
                {
                    var message = RandomStringGenerator.GenerateRandomString();
                    var delay = RandomStringGenerator.GetRandomNumber();
                    LogContext.PushProperty("{id}", message);
                    Thread.Sleep(delay);
                    message = RandomStringGenerator.GenerateRandomString();
                    delay = RandomStringGenerator.GetRandomNumber();
                    Log.Information("{ID}: {Delay} - {Message}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    Log.Debug("{ID}: {Delay} - {Message}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    Log.Warning("{ID}: {Delay} - {Message}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    Log.Error("{ID}: {Delay} - {Message}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    Log.Fatal("{ID}: {Delay} - {Message}", id, delay, message);
                    Thread.Sleep(delay);
                    delay = RandomStringGenerator.GetRandomNumber();
                    message = RandomStringGenerator.GenerateRandomString();
                    Log.Verbose("{ID}: {Delay} - {Message}", id, delay, message);
                    Thread.Sleep(delay);
                    i++;
                    if (i > 100)
                    {
                        Log.Information("{ID}: Close loop", id);
                        break;
                    }
                }

                Thread.Sleep(1000);
                throw new InvalidOperationException("Unhandled exception");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unhandled exception");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
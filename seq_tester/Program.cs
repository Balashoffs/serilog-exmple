using System;
using System.Linq;
using System.Threading;
using Serilog;
using Serilog.Context;

namespace seq_tester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var logger = new LoggerConfiguration()
                    // Read from appsettings.json
                    .MinimumLevel.Debug()
                    .Enrich.WithProperty("Application", "seq-tester")
                    .Enrich.WithProperty("RevitUser", "BAU")
                    .WriteTo.Console() // Write logs to console
                    .WriteTo.File("seq_tester/.log/log.txt",
                        outputTemplate:
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        // File rotation
                        rollingInterval: RollingInterval.Day,
                        // Maximum size before rollover (e.g., 10MB)
                        fileSizeLimitBytes: 10_485_760,
                        // Encoding
                        encoding: System.Text.Encoding.UTF8,
                        // Shared access for file (allow multiple processes)
                        shared: false,
                        // Auto-flush after each write
                        flushToDiskInterval: TimeSpan.FromSeconds(1))
                    .WriteTo.Seq("http://localhost:5341") // Write to Seq server
                    .CreateLogger();
                Log.Logger = logger;

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

    public class RandomStringGenerator
    {
        private static readonly Random _random = new Random();
        private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateRandomString()
        {
            int length = _random.Next(20, 51); // Random length between 20-50 inclusive
            return new string(Enumerable.Range(0, length)
                .Select(_ => ValidChars[_random.Next(ValidChars.Length)])
                .ToArray());
        }

        public static int GetRandomNumber()
        {
            int min = 1000, max = 5000;
            return _random.Next(min, max);
        }
    }
}
using System;
using System.Runtime.InteropServices;
using CustomLogger;
using Serilog;
using Serilog.Core;

namespace SerilogExample.SeriogLogger
{
    /// <summary>
    /// Иницилизация глобального инстанса логгера для отправки логов в консоль, локальный файл и на сервер
    /// 
    /// WriteTo.Console() - функция для конфигурирования вывода в консоль
    /// WriteTo.File() - функция для конфигурирования вывода в файл
    /// WriteTo.Seq() - функция для конфигурирования подключения и работы с Seq сервером
    /// </summary>
    public static class SerilogLoggerConfig
    {
        public static Logger Build(SeqSecrets secrets, ILogEventEnricher[] eventEnrichers)
        {
            return new LoggerConfiguration()
                // Read from appsettings.json
                .MinimumLevel.Debug()
                .Enrich.With(eventEnrichers)
                .WriteTo.Console() // Write logs to console
                .WriteTo.File("seq_SerilogExampleg/log.txt",
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
                .WriteTo.Seq(secrets.Host, apiKey: secrets.ApiKey) // Write to Seq server
                .CreateLogger();
        }
    }
}
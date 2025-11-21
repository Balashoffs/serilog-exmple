using System;

namespace CustomLogger
{
    /// <summary>
    /// Секреты для доступа к Seq сервер
    /// Требуется внесение записей в переменные путей
    /// После установки в переменных путей требуется перезапуск IDE
    /// </summary>
    public class SeqSecrets
    {
        /// <summary>
        /// Url Seq сервера, например http://127.0.0.1:5341, https://serqserver.com
        /// </summary>
        public string Host { get; private set; } = Environment.GetEnvironmentVariable("SEQ_HOST");
        /// <summary>
        /// Api ключ для отравки сообщение на сервер, необходимо для безопасной работы.
        /// </summary>
        public string ApiKey { get; private set; } = Environment.GetEnvironmentVariable("SEQ_API_KEY");
        
        
    }
}
## Описание
Для записи логов на удаленный сервер предлагается использовать связку 
* Seq - [сервер](https://datalust.co/docs/getting-started)
* Serilog - [клиент](https://datalust.co/docs/using-serilog)

## Seq
Seq — это сервер для поиска и анализа в реальном времени структурированных логов и трассировок приложений. Его тщательно разработанный интерфейс пользователя, хранилище событий в формате JSON и знакомый язык запросов делают его эффективной платформой для обнаружения и диагностики проблем в сложных приложениях и микросервисах.

Seq работает на платформе Windows или под Docker/Linux. Вы можете развернуть Seq на своем оборудовании или легко запустить экземпляр в любом публичном облаке.

Телеметрия приложения может быть получена с использованием различных библиотек логирования и протоколов. Оповещения и уведомления могут быть отправлены на различные выходные устройства. Входные и выходные плагины могут быть написаны на любом языке или с использованием .NET SDK для приложений.

### Пример запуска в Docker
1. Содать docker-compose.yaml в корне проекта
```docker-compose
version: '3.8'

services:
  seq:
    image: datalust/seq
    container_name: seq
    restart: unless-stopped
    environment:
      ACCEPT_EULA: 'Y'
      SEQ_FIRSTRUN_ADMINPASSWORD: '123qwe'
    volumes:
      - .seg:/data
    ports:
      - '5341:80'
```
2. Запустить Docker Desktop (для Windows, Macos)
3. Выполнить к командной строке, так чтобы командная строка указывала на корневую директорию проекта
```bash/cmd
    docker-compose up -d
```
4. Открыть браузер, ввести http://127.0.0.1:5341. Откроется веб панель. Логин для входа - admin, пароль - см. в файле выше
5. Ввести новый пароль (минимум 8 символов)

    ![Веб панель](.img/img_1.png)

### Базовые примеры работы 

## Serilog

Serilog — это библиотека для логирования в .NET-приложениях, которая поддерживает структурированные логи. Она позволяет легко записывать и анализировать события, предоставляя гибкие возможности для интеграции с различными хранилищами и платформами. Serilog поддерживает множество выходных форматов, включая JSON, и предлагает широкие возможности для фильтрации и настройки логов. Библиотека обеспечивает простоту использования и высокую производительность, позволяя получать подробную информацию о работе приложений и быстрее решать возникающие проблемы.

### Примеры конфигурирования
1. Установить следующие библиотеки
```bash
$ dotnet add package Serilog
$ dotnet add package Serilog.Sinks.Console
$ dotnet add package Serilog.Sinks.File
$ dotnet add package System.Net.Http
```
2. Список зависимостей, которые подгружаются и нужны для сборки
```txt
package id="Serilog" version="4.2.0" targetFramework="net472"
package id="Serilog.Sinks.Console" version="6.1.1" targetFramework="net472"
package id="Serilog.Sinks.File" version="6.0.0" targetFramework="net472"
package id="Serilog.Sinks.Seq" version="9.0.0" targetFramework="net472"
package id="System.Buffers" version="4.5.1" targetFramework="net472"
package id="System.Diagnostics.DiagnosticSource" version="8.0.1" targetFramework="net472"
package id="System.IO" version="4.3.0" targetFramework="net472"
package id="System.Memory" version="4.5.5" targetFramework="net472"
package id="System.Net.Http" version="4.3.4" targetFramework="net472"
package id="System.Numerics.Vectors" version="4.5.0" targetFramework="net472"
package id="System.Runtime" version="4.3.0" targetFramework="net472"
package id="System.Runtime.CompilerServices.Unsafe" version="6.0.0" targetFramework="net472"
package id="System.Security.Cryptography.Algorithms" version="4.3.0" targetFramework="net472"
package id="System.Security.Cryptography.Encoding" version="4.3.0" targetFramework="net472"
package id="System.Security.Cryptography.Primitives" version="4.3.0" targetFramework="net472"
package id="System.Security.Cryptography.X509Certificates" version="4.3.0" targetFramework="net472"
package id="System.Threading.Channels" version="8.0.0" targetFramework="net472"
package id="System.Threading.Tasks.Extensions" version="4.5.4" targetFramework="net472"
```
3. Создание интстанс Serilog
```csharp
var logger = new LoggerConfiguration()
    //Минимальный уровень логирования
    /*
    Log Level	Importance
    Fatal	One or more key business functionalities are not working and the whole system doesn’t fulfill the business functionalities.
    Error	One or more functionalities are not working, preventing some functionalities from working correctly.
    Warn	Unexpected behavior happened inside the application, but it is continuing its work and the key business features are operating as expected.
    Info	An event happened, the event is purely informative and can be ignored during normal operations.
    Debug	A log level used for events considered to be useful during software debugging when more granular information is needed.
    Trace	A log level describing events showing step by step execution of your code that can be ignored during the standard operation, but may be useful during extended debugging sessions.
    */
    .MinimumLevel.Debug()
    //Дополнительные поля к сообщению, которые можно настроить для всего лога
    //Например - имя приложения, имя пользователя
    .Enrich.WithProperty("Application", "seq-tester")
    .Enrich.WithProperty("RevitUser", "BAU")
     //Настройка записи лога в файл
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
    //Настройка вывода в консоль
    .WriteTo.Console() // Write logs to console   
     //Подключение к Seq серверу
    .WriteTo.Seq("http://localhost:5341") // Write to Seq server
    .CreateLogger();
    
    Log.Logger = logger;
```
4. Привязка к Singleton Serilog.ILog, доступ к которому возможен из любого участка кода
```csharp
Log.Logger = logger;
```
5. Примеры отправки логов для разных уровней логгирования
```csharp
    Log.Information("{ID}: {Arg1} - {Message}", id, arg1, message);
    Log.Debug("{ID}: {Arg1} - {Message}", id, arg1, message);
    Log.Warning("{ID}: {Arg1} - {Message}", id, arg1, message);     
    Log.Error("{ID}: {Arg1} - {Message}", id, arg1, message);
    Log.Fatal("{ID}: {Arg1} - {Message}", id, arg1, message);
    Log.Verbose("{ID}: {Arg1} - {Message}", id, arg1, message);
```
где 
* {ID}, {Arg1}, {Message} - наименования полей, по которым можно искать логи на сервер через веб панель
* id, arg1, message - переменные, которые передаются в виде лога на сервер
* Пример отображения лога

    ![Пример лога](.img/img.png)

6. Обязательно!!! Настроить обработку необрабатываемых исключений и закрытие приложения - принудительное или по дейсвтию пользователя - через вызов метода Log.CloseAndFlush();

Перехват необрабатываемых исключений
```csharp
    AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
    {
        var ex = eventArgs.ExceptionObject as Exception;
        Log.Fatal(ex, "Unhandled exception");
        Log.CloseAndFlush();
    };
```
    
Конструкия try - >catch -> finally в функции main(String[] args)
```csharp
    try
    {
        Log.Logger ...
        ///Some main code
    }catch(Exception){
        Log.Fatal();
    }finally{
        Log.CloseAndFlush();
    }
```
    


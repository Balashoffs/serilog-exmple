// using System;
// using Serilog;
// using Serilog.Context;
//
// // ============================================
// // 1. CONFIGURATION (Program.cs or Startup)
// // ============================================
//
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .Enrich.FromLogContext()
//     
//     .Enrich.WithProperty("Application", "MyApp")
//     .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable(""))
//     .WriteTo.Seq("http://localhost:5341")
//     .WriteTo.Console(
//         outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
//     .CreateLogger();
//
// // ============================================
// // 2. DEPENDENCY INJECTION
// // ============================================
//
// builder.Services.AddLogging(logging =>
// {
//     logging.ClearProviders();
//     logging.AddSerilog();
// });
//
// // ============================================
// // 3. BEST PRACTICES - STRUCTURED LOGGING
// // ============================================
//
// public class OrderService
// {
//     private readonly ILogger<OrderService> _logger;
//
//     public OrderService(ILogger<OrderService> logger)
//     {
//         _logger = logger;
//     }
//
//     public async Task ProcessOrderAsync(int orderId, string customerId, decimal amount)
//     {
//         try
//         {
//             _logger.LogInformation("Processing order started. OrderId: {OrderId}, CustomerId: {CustomerId}, Amount: {Amount}",
//                 orderId, customerId, amount);
//
//             // Use LogContext for correlation tracking
//             using (LogContext.PushProperty("CorrelationId", Guid.NewGuid()))
//             using (LogContext.PushProperty("UserId", customerId))
//             {
//                 // Business logic here
//                 await ValidateOrder(orderId);
//                 await ProcessPayment(orderId, amount);
//                 await UpdateInventory(orderId);
//
//                 _logger.LogInformation("Order processed successfully. OrderId: {OrderId}, Duration: {DurationMs}ms",
//                     orderId, DateTime.UtcNow.Ticks);
//             }
//         }
//         catch (InvalidOperationException ex)
//         {
//             _logger.LogWarning(ex, "Order validation failed. OrderId: {OrderId}, Reason: {Reason}",
//                 orderId, ex.Message);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Critical error processing order. OrderId: {OrderId}, CustomerId: {CustomerId}",
//                 orderId, customerId);
//             throw;
//         }
//     }
//
//     private async Task ProcessPayment(int orderId, decimal amount)
//     {
//         try
//         {
//             _logger.LogDebug("Processing payment. OrderId: {OrderId}, Amount: {Amount}, Provider: {PaymentProvider}",
//                 orderId, amount, "Stripe");
//
//             // Payment logic
//             await Task.Delay(100);
//
//             _logger.LogInformation("Payment processed successfully. OrderId: {OrderId}, TransactionId: {TransactionId}",
//                 orderId, Guid.NewGuid());
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Payment processing failed. OrderId: {OrderId}, Amount: {Amount}",
//                 orderId, amount);
//             throw;
//         }
//     }
//
//     private async Task ValidateOrder(int orderId)
//     {
//         _logger.LogDebug("Validating order. OrderId: {OrderId}", orderId);
//         await Task.Delay(50);
//     }
//
//     private async Task UpdateInventory(int orderId)
//     {
//         _logger.LogDebug("Updating inventory. OrderId: {OrderId}", orderId);
//         await Task.Delay(75);
//     }
// }
//
// // ============================================
// // 4. LOG MESSAGE STRUCTURE EXAMPLES
// // ============================================
//
// /*
// GOOD - Structured with Named Properties:
// _logger.LogInformation("User login attempt. UserId: {UserId}, Email: {Email}, IpAddress: {IpAddress}",
//     userId, email, ipAddress);
//
// BAD - String interpolation (loses structure):
// _logger.LogInformation($"User login attempt. UserId: {userId}, Email: {email}");
//
// GOOD - Context enrichment:
// using (LogContext.PushProperty("RequestId", httpContext.TraceIdentifier))
// {
//     _logger.LogInformation("Processing request. Path: {Path}", request.Path);
// }
//
// GOOD - Exception logging:
// _logger.LogError(exception, "Database error. Operation: {Operation}, Table: {Table}",
//     operation, "Orders");
//
// GOOD - Timing/Performance:
// var stopwatch = Stopwatch.StartNew();
// // ... operation ...
// stopwatch.Stop();
// _logger.LogInformation("Database query completed. Query: {Query}, Duration: {DurationMs}ms",
//     queryName, stopwatch.ElapsedMilliseconds);
//
// GOOD - Correlation tracking:
// var correlationId = Guid.NewGuid();
// using (LogContext.PushProperty("CorrelationId", correlationId))
// {
//     _logger.LogInformation("Starting batch processing. BatchId: {BatchId}, Count: {Count}",
//         batchId, itemCount);
// }
// */
//
// // ============================================
// // 5. MIDDLEWARE FOR REQUEST LOGGING
// // ============================================
//
// public class RequestLoggingMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly ILogger<RequestLoggingMiddleware> _logger;
//
//     public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
//     {
//         _next = next;
//         _logger = logger;
//     }
//
//     public async Task InvokeAsync(HttpContext context)
//     {
//         var correlationId = context.TraceIdentifier;
//         
//         using (LogContext.PushProperty("CorrelationId", correlationId))
//         using (LogContext.PushProperty("RequestPath", context.Request.Path))
//         using (LogContext.PushProperty("RequestMethod", context.Request.Method))
//         {
//             var stopwatch = System.Diagnostics.Stopwatch.StartNew();
//
//             _logger.LogInformation("HTTP request started. Path: {Path}, Method: {Method}",
//                 context.Request.Path, context.Request.Method);
//
//             await _next(context);
//
//             stopwatch.Stop();
//
//             _logger.LogInformation("HTTP request completed. StatusCode: {StatusCode}, Duration: {DurationMs}ms",
//                 context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
//         }
//     }
// }
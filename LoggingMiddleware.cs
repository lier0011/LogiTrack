using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogiTrack
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            var now = DateTime.UtcNow;
            _logger.LogInformation("[{dateTime}] Incoming request: {method} {path}", now, request.Method, request.Path);

            await _next(context);

            stopwatch.Stop();
            var response = context.Response;
            now = DateTime.UtcNow;
            _logger.LogInformation("[{dateTime}] Response: {statusCode} for {method} {path} in {elapsed} ms", now, response.StatusCode, request.Method, request.Path, stopwatch.ElapsedMilliseconds);
        }
    }
}

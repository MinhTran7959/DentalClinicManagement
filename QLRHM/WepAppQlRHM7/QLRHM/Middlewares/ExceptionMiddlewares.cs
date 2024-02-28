namespace QLRHM7.Middlewares
{
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddlewares(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger("ExceptionMiddlewares");
        }


        public async Task Invoke(HttpContext httpContext) {
             _logger.LogInformation("Cust Middleware Initiate");
             await _next(httpContext);
        }

     
    }
    public static class CustMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddlewares>();
        }
    }
}

namespace server.MiddleWere
{
    public class LoggerMiddlewere
    {
        private readonly RequestDelegate next;

        public LoggerMiddlewere(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger<LoggerMiddlewere> logger)
        {
            logger.LogInformation($"{httpContext.Request.Path}:{httpContext.Request.Method}/ {httpContext.Request.QueryString.Value}, parameters: {httpContext.Request.Body.ToString}, headers : {httpContext.Request.Headers.Authorization}");

            var originalBodyStream = httpContext.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                httpContext.Response.Body = responseBody;

                await next(httpContext);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                if (httpContext.Response.StatusCode >= 400)
                {
                    logger.LogError($"{httpContext.Response.StatusCode}: {responseBodyText}");
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    public static class ExtentionsClass
    {
        public static IApplicationBuilder UseLoggerMiddlere(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<LoggerMiddlewere>();
        }
    }
}

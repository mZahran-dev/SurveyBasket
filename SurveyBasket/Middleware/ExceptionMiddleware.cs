namespace SurveyBasket.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
		try
		{
			await _next(httpContext);
		}
		catch (Exception exception)
		{
			// log error
			_logger.LogError(exception, "Something went Wrong: {message}", exception.Message);

			var problemDetails = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Title = "Internal Server Error",
				Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };

			httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await httpContext.Response.WriteAsJsonAsync(problemDetails);
		}
    }
}

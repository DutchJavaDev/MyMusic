namespace MyMusic.Api.Middleware
{
  public class RequestLoggingMiddleware
  {
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      // Log the request details
      Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

      // Copy the request body so it can be read again later
      var requestBodyStream = new MemoryStream();
      await context.Request.Body.CopyToAsync(requestBodyStream);
      requestBodyStream.Seek(0, SeekOrigin.Begin);
      string requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
      Console.WriteLine($"Request Body: {requestBodyText}");

      // Ensure the request body can be read later
      requestBodyStream.Seek(0, SeekOrigin.Begin);
      context.Request.Body = requestBodyStream;

      // Call the next middleware in the pipeline
      await _next(context);
    }
  }
}

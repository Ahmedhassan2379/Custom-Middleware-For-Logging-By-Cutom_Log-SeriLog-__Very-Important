using CustomMiddlewareForLogging.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace CustomMiddlewareForLogging.Middlewares
{
    public class RequestAndResponseLoggingMiddleware
    {
        //private readonly RequestDelegate _next;
        //private readonly IServiceProvider _serviceProvider;

        //public RequestAndResponseLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        //{
        //    _next = next;
        //    _serviceProvider = serviceProvider;
        //}

        //public async Task Invoke(HttpContext context)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var logger = scope.ServiceProvider.GetRequiredService<IUserActionLogger>();

        //        if (context.Request.Path.StartsWithSegments("/api"))
        //        {
        //            string requestBody = string.Empty;
        //            string userIdentity;
        //            Dictionary<string, string> Param = new();
        //            context.Request.EnableBuffering();
        //            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
        //            {
        //                requestBody = await reader.ReadToEndAsync();
        //                context.Request.Body.Position = 0;
        //            }
        //            if (!requestBody.IsNullOrEmpty() && context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
        //            {
        //                var paramDict = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);
        //                if (paramDict != null)
        //                {
        //                    foreach (var kvp in paramDict)
        //                    {
        //                        Param[kvp.Key] = kvp.Value?.ToString();
        //                    }
        //                }
        //            }

        //            if (Param != null && Param.ContainsKey("userIdentity"))
        //            {
        //                userIdentity = Param["userIdentity"];
        //                context.Session.SetString("UserIdentity", userIdentity);
        //            }
        //            else if (context.Session.TryGetValue("UserIdentity", out var storedUserIdentity))
        //            {
        //                userIdentity = Encoding.UTF8.GetString(storedUserIdentity);
        //            }
        //            else
        //            {
        //                userIdentity = "Anonymous";
        //            }
        //            var log = new UserActionLog
        //            {
        //                UserIdentity = userIdentity,
        //                Action = context.Request.Method,
        //                Controller = context.Request.RouteValues["controller"]?.ToString(),
        //                ActionName = context.Request.RouteValues["action"]?.ToString(),
        //                IPAddress = context.Connection.RemoteIpAddress.ToString(),
        //                Timestamp = DateTime.Now,
        //                Parameters = requestBody,
        //            };
        //            await logger.LogAsync(log);
        //        }
        //    }
        //    await _next(context);
        //}











        //--------------------------------------------------------------------------------
        //private readonly LogsDbContext _Logscontext;
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        public RequestAndResponseLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;

        }

        public async Task Invoke(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<IUserActionLogger>();
                if (context.Request.Path.StartsWithSegments("/api")) 
                {
                    string requestBody = string.Empty;
                    string userIdentity;
                    Dictionary<string, string> Param = new();
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                    {
                        requestBody = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                    }
                    if (!requestBody.IsNullOrEmpty() && context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
                    {
                        var paramDict = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);
                        if (paramDict != null)
                        {
                            foreach (var kvp in paramDict)
                            {
                                Param[kvp.Key] = kvp.Value?.ToString();
                            }
                        }
                    }

                    if (Param != null && Param.ContainsKey("userIdentity"))
                    {
                        userIdentity = Param["userIdentity"];
                        context.Session.SetString("UserIdentity", userIdentity);
                    }
                    else if (context.Session.TryGetValue("UserIdentity", out var storedUserIdentity))
                    {
                        userIdentity = Encoding.UTF8.GetString(storedUserIdentity);
                    }
                    else
                    {
                        userIdentity = "Anonymous";
                    }
                    var log = new UserActionLog
                    {
                        UserIdentity = userIdentity,
                        Action = context.Request.Method,
                        Controller = context.Request.RouteValues["controller"]?.ToString(),
                        ActionName = context.Request.RouteValues["action"]?.ToString(),
                        IPAddress = context.Connection.RemoteIpAddress.ToString(),
                        Timestamp = DateTime.Now,
                        Parameters = requestBody,
                    };
                    await logger.Log(log);
                }
            }
            await _next(context);
        
        }

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    // Log Request
        //    context.Request.EnableBuffering();
        //    var request = await FormatRequest(context.Request);

        //    // Copy original response stream to capture the response
        //    var originalResponseBodyStream = context.Response.Body;
        //    using var responseBody = new MemoryStream();
        //    context.Response.Body = responseBody;

        //    await _next(context);

        //    // Log Response
        //    var response = await FormatResponse(context.Response);

        //    // Log to database
        //    var dbContext = context.RequestServices.GetRequiredService<LogsDbContext>();
        //    var logEntry = new LogsModel
        //    {
        //        Request = request,
        //        Response = response,
        //        Time = DateTime.UtcNow
        //    };
        //    dbContext.Logs.Add(logEntry);
        //    await dbContext.SaveChangesAsync();

        //    // Copy the contents of the new memory stream (which contains the response) to the original stream
        //    await responseBody.CopyToAsync(originalResponseBodyStream);
        //}

        //private async Task<string> FormatRequest(HttpRequest request)
        //{
        //    request.Body.Position = 0;
        //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        //    await request.Body.ReadAsync(buffer, 0, buffer.Length);
        //    var bodyAsText = Encoding.UTF8.GetString(buffer);
        //    request.Body.Position = 0;
        //    return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        //}
        //private async Task<string> FormatResponse(HttpResponse response)
        //{
        //    response.Body.Seek(0, SeekOrigin.Begin);
        //    var text = await new StreamReader(response.Body).ReadToEndAsync();
        //    response.Body.Seek(0, SeekOrigin.Begin);
        //    return $"{response.StatusCode}: {text}";
        //}
    }
}

using System.Net;
using System.Text;
using System.Text.Json;
using Fiona.Hosting.Abstractions.Middleware;
using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Routing;

internal class CallEndpointMiddleware(Router router) : IMiddleware
{
    public async Task Invoke(HttpListenerContext context, NextMiddlewareDelegate next)
    {
        HttpListenerRequest request = context.Request;
        var result = await CallEndpoint(request);
        var responseString = GetResponseString(result);
        await SetResponse(context, responseString, result);
    }

    private async Task<ObjectResult> CallEndpoint(HttpListenerRequest request)
    {
        return await router.CallEndpoint(request.Url!, GetHttpMethodType(request), GetBody(request));
    }

    private static HttpMethodType GetHttpMethodType(HttpListenerRequest request)
    {
        return HttpMethodTypeExtensionMethods.GetHttpMethodType(request.HttpMethod) ?? HttpMethodType.Get;
    }

    private static Stream? GetBody(HttpListenerRequest request)
    {
        return request.HasEntityBody
            ? request.InputStream
            : null;
    }

    private static string? GetResponseString(ObjectResult result)
    {
        Type? resultType = result.Result?.GetType();
        string? responseString = string.Empty;
        if (resultType is not null)
        {
            if (resultType.IsPrimitive || resultType == typeof(string))
            {
                responseString = result.Result?.ToString();
            }
            else
            {
                responseString = JsonSerializer.Serialize(result.Result);
            }
        }

        return responseString;
    }

    private static async Task SetResponse(HttpListenerContext context, string? responseString, IResult result)
    {
        var response = context.Response;
        var buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        response.StatusCode = (int)result.StatusCode;
        var output = response.OutputStream;
        await output.WriteAsync(buffer);
        output.Close();
    }
}
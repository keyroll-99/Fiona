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
        ObjectResult result = await CallEndpoint(request);
        string? responseString = GetResponseString(result);
        SetCookie(result, context);
        await SetResponse(context, responseString, result);
        CloseConnection(context);
    }

    private async Task<ObjectResult> CallEndpoint(HttpListenerRequest request)
    {
        return await router.CallEndpoint(request.Url!, GetHttpMethodType(request), GetBody(request), request.Cookies);
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
        // TODO work on stream not on string
        Type? resultType = result.Result?.GetType();
        string? responseString = string.Empty;
        if (resultType is not null)
        {
            if (resultType.IsPrimitive || resultType == typeof(string))
                responseString = result.Result?.ToString();
            else
                responseString = JsonSerializer.Serialize(result.Result);
        }

        return responseString;
    }

    private static async Task SetResponse(HttpListenerContext context, string? responseString, IResult result)
    {
        HttpListenerResponse response = context.Response;
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        response.StatusCode = (int)result.StatusCode;
        Stream output = response.OutputStream;
        await output.WriteAsync(buffer);
    }

    private static void CloseConnection(HttpListenerContext context)
    {
        context.Response.OutputStream.Close();
        context.Response.Close();
    }

    private static void SetCookie(ObjectResult result, HttpListenerContext context)
    {
        foreach ((string key, string value) in result.Cookies)
        {
            System.Net.Cookie cookie = new System.Net.Cookie(key, value, "/");
            context.Response.Cookies.Add(cookie);
        }
    }
}
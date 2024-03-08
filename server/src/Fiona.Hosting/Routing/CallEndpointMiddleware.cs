using System.Net;
using System.Text;
using System.Text.Json;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Middleware;

namespace Fiona.Hosting.Routing;

internal class CallEndpointMiddleware(Router router) : IMiddleware
{

    public async Task Invoke(HttpListenerContext context, Delegate next)
    {
        HttpListenerRequest request = context.Request;

        ObjectResult result = await router.CallEndpoint(request.Url,
            HttpMethodTypeExtensionMethods.GetHttpMethodType(request.HttpMethod) ?? HttpMethodType.Get,
            request.HasEntityBody
                ? request.InputStream
                : null);

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

        var response = context.Response;

        var buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        response.StatusCode = (int)result.StatusCode;
        var output = response.OutputStream;
        await output.WriteAsync(buffer);
        output.Close();
    }
}
# Cookie

## Get cookie

To get Cookie you have to use Cookie attribute.
Cookie attribute have two constructors.

1. Without argument, when you use it, the cookie will be match by the name of argument.
1. With one argument which is the name of cookie. In this scenario name of argument don't have to be same as name of cookie

Example of use

```c#
[Route(HttpMethodType.Get, "get")]
public Task<ObjectResult> GetCookie([Cookie("fiona")] string? notFiona, [Cookie] string? secondCookie)
{
    return Task.FromResult(new ObjectResult(new { notFiona, secondCookie }, HttpStatusCode.OK));
}

```

## How to set cookie

To set a cookie you have use method SetCookie on your instance of ObjectResult.

```c#

[Route(HttpMethodType.Get, "set")]
public Task<ObjectResult> SetCookie()
{
    return Task.FromResult(new ObjectResult(null, HttpStatusCode.OK).SetCookie("Fiona", "Fiona"));
}
```

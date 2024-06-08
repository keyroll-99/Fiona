# Fiona language

## About Fiona's language

Fina's language is a special language to describe your endpoint, usually you will be modified this file by IDE, but If
you want to you can modify it by yourself.
Fiona files will be compiled to c# language

## Why I created own script language?

for two reason

1. I wanted to learn how to make my own compiler, tokenizer etc.
2. Is much easier when I have my format to work with files than adapting to the existing one


## How I can use this language?
Unfortunately, there is currently no easy way to test this language. But if you want you can do it :)

1. Download this repo
2. Run project FionaIDE
3. Create a new project
4. Create new file in the project
5. Open this file in your favorite IDE
6. Modify file
7. Close the file in your IDE (it's important, because it can block access to file)
8. Open this file in FionaIDE
9. Click the `Compile file` button

Don't worry in the future this will not be such hard


## Usings

In the file we can have only one using section. Each section begins with the keyword `using Begin;` and finish
with `usingEnd;`.

Example of use

```csharp
usingBegin;
using system; using system.collections;
using system.collections.generic;
usingEnd;
```

## Namespace

We can define namespace by using the keyword `namespace {value};`

```csharp
namespace: Token.Test;
```

## Class define

We can define a class by using the keyword `class {name};`. In the file we can have only one class

class can have 3 fields

1. route - one per class which define global route for class
2. inject - one per class which define what should be injected to controller
3. endpoint section - list of endpoints in the class

example of use:

```csharp
class IndexController;
route: /home;
inject:
- userService: IUserService
- logger: ILogger<TestController>;
endpoint: 
...
```

## Route define

We can define a route by using the keyword `route: {name};`. In the file we can have only one class

example of use:

```csharp
route: /home;
```

## Inject define

Inject work with the array, so as argument we can pass few dependencies, each element in array is separated by `-`
symbol

example of use:

```csharp
inject:
- userService: IUserService
- logger: ILogger<TestController>;
```

## Endpoint define

We can define an endpoint by using the keyword `endpoint {name};`. each endpoint always finishes with `bodyEnd` keyword

endpoint can have 5 fields, the order doesn't matter except

1. route - one per endpoint which define endpoint route
2. methods - one per endpoint which methods type for endpoint, for example GET
3. return - one per endpoint which define what endpoint should return
4. input - a list of method arguments
5. bodyBegin / bodyEnd - define where is body of method

## Route Define in method

We can define a route by using the keyword `route: {name};`. In the file we can have only one per endpoint

example of use:

## Method define

Method define works with my first attempt to array, so it have different style. Currently, we have 5 types of method
available:

1. GET
2. POST
3. PUT
4. PATCH
5. DELETE

example of use:
```
method: [GET, POST];
```

## return
We can define a route by using the keyword `return: {type};`. In the file we can have only one per endpoint

example of use
```csharp
return: User;
```

## input define

input work with the array, so as argument we can pass few parameters to method, each element in array is separated by `-`
symbol

example of use
```csharp
input:
  - [FromRoute] name: string
  - [QueryParam] age: int
  - [Body] user: User
  - [Cookie] userId: long;
```

## Body
Body is the last section in the endpoint, `bodyEnd` tag finish endpoint define. The structure of method body is:
1. bodyBegin tag which start body section
2. body section, here you can write your c# code, which one will be in the body method
3. bodyEnd which finishes body end of course finish endpoint definition

example of use:

```csharp
inject:
- userService: IUserService
- logger: ILogger<TestController>;
```

example of use:

```csharp
endpoint: Index;
route: /{name};
method: [GET, POST];
return: User;
input:
  - [FromRoute] name: string
  - [QueryParam] age: int
  - [Body] user: User
  - [Cookie] userId: long;
// Testowy komentarz
// return: Home;
bodyBegin;
var x = 10;
var y = userService.GetAge();
if(x > y)
{
  return user;
}
else
{
  return null;
}
bodyEnd;
```

example of use:

```csharp
class IndexController;
route: /home;
inject:
- userService: IUserService
- logger: ILogger<TestController>;
endpoint: 
...

## Example

code like this:

```

usingBegin;
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;
usingEnd;

namespace: comitow.LoadTest;

class IndexController;
route: /index;
inject:

- userService:    IUserService
- logger:    ILogger<TestController>
  ;
  endpoint: Index;
  route: /{name};
  method: [GET, POST];
  return: User;
  input:
    - [FromRoute] name: string
    - [QueryParam] age: int
    - [Body] user: User
    - [Cookie] userId: long;
      bodyBegin;
      bodyEnd;

endpoint: GetUsers;
route: /{name};
method: [GET, POST];
return: User;
input:
- [FromRoute] name: string
- [QueryParam] age: int
- [Body] user: User
- [Cookie] userId: long;
// Testowy komentarz
// return: Home;
bodyBegin;
var x = 10;
var y = userService.GetAge();
if(x > y)
{
return user;
}
else
{
return null;
}
bodyEnd;

```

will be generated:

```csharp
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;

namespace comitow.LoadTest;

[Controller("/index")]
public class IndexController
{
    private readonly IUserService userService;
    private readonly ILogger<TestController> logger;
    public aaabqeq(IUserService userService, ILogger<TestController> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }
    [Route(HttpMethodType.Get | HttpMethodType.Post, "/{name}", ["age"])]
    public async Task<User> Index([FromRoute] string name, [QueryParam] int age, [Body] User user, [Cookie] long userId) {}
    [Route(HttpMethodType.Get | HttpMethodType.Post, "/{name}", ["age"])]
    public async Task<User> GetUsers([FromRoute] string name, [QueryParam] int age, [Body] User user, [Cookie] long userId)
    {
        var x = 10;
        var y = userService.GetAge();
        if (x > y)
        {
            return user;
        }
        else
        {
            return null;
        }
    }
}
```
# Controller

To mark a class as a controller you have to use the Controller attribute.

```c#
[Controller]
public class HomeController(ILogger<HomeController> logger)
{
}
```

The Default route of the controller is just `/`, but you can pass an argument to the attribute which will override the base route of the controller.

```c#
[Controller("home")]
public class HomeController(ILogger<HomeController> logger)
{
}
```
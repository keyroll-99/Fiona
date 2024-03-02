namespace Fiona.Hosting.Routing;

public class RouterBuilder
{
    private IList<object> _controllers = new List<object>();
    private Router Instance { get; set; }
    
    public RouterBuilder AddController(object controller)
    {
        _controllers.Add(controller);
        return this;
    }

    public Router Build()
    {
        Instance = new Router(_controllers);
        return Instance;
    }
    
}
namespace Fiona.IDE.ProjectManager.Models;

public sealed class Input
{
    private string _name;
    private string _type;
    private string _attribute;

    public Input(string name, string type, string attribute)
    {
        _name = name;
        _type = type;
        _attribute = attribute;
    }
}
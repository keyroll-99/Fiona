using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Base;

namespace Fiona.IDE.Components.Behavior;

public class MyBehavior : Blazor.Diagrams.Core.Behavior
{
    public MyBehavior(Diagram diagram) : base(diagram)
    {
        Diagram.PointerDown += OnPointerDown;
    }

    private void OnPointerDown(Model? model, Blazor.Diagrams.Core.Events.PointerEventArgs e)
    {
        if (model == null) // Canvas
        {
            Diagram.UnselectAll();
        }
        else if (model is SelectableModel sm)
        {
            Diagram.SelectModel(sm, true);
        }
    }

    public override void Dispose()
    {
        Diagram.PointerDown -= OnPointerDown;
    }
}
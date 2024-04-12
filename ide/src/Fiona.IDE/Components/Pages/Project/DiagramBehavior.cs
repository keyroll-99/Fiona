using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models.Base;

namespace Fiona.IDE.Components.Pages.Project
{
    internal class DiagramBehavior : Blazor.Diagrams.Core.Behavior
    {
        public DiagramBehavior(Diagram diagram) : base(diagram)
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
}

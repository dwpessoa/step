using System.Collections.Generic;
using IxMilia.Step.Syntax;

namespace IxMilia.Step.Items
{
    public class StepToroidalSurface : StepElementarySurface
    {
        public override StepItemType ItemType => StepItemType.ToroidalSurface;

        public double Radius { get; set; }
        public double CurveRadius { get; set; }

        private StepToroidalSurface()
            : base()
        {
        }

        public StepToroidalSurface(string name, StepAxis2Placement3D position, double radius, double curveRadius)
            : base(name, position)
        {
            Radius = radius;
            CurveRadius = curveRadius;
        }

        internal override IEnumerable<StepSyntax> GetParameters(StepWriter writer)
        {
            foreach (var parameter in base.GetParameters(writer))
            {
                yield return parameter;
            }

            yield return new StepRealSyntax(Radius);
            yield return new StepRealSyntax(CurveRadius);
        }

        internal static StepRepresentationItem CreateFromSyntaxList(StepBinder binder, StepSyntaxList syntaxList)
        {
            syntaxList.AssertListCount(4);
            var surface = new StepToroidalSurface();
            surface.Name = syntaxList.Values[0].GetStringValue();
            binder.BindValue(syntaxList.Values[1], v => surface.Position = v.AsType<StepAxis2Placement3D>());
            surface.CurveRadius = syntaxList.Values[2].GetRealVavlue();
            surface.Radius = syntaxList.Values[3].GetRealVavlue();
            return surface;
        }
    }
}

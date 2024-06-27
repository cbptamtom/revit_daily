using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using Nice3point.Revit.Toolkit.Options;
using System.Windows;

namespace Create_section.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            var selectionConfiguration = new SelectionConfiguration()
                .Allow.Element(e => e is Wall);

            var reference = UiDocument.Selection.PickObject(ObjectType.Element, selectionConfiguration.Filter);


            var wall = reference.ElementId.ToElement(Document)!;
            var wallLocation = wall.Location as LocationCurve;
            var line = wallLocation.Curve as Line;

            // Determine section box
            var p = line.GetEndPoint(0);
            var q = line.GetEndPoint(1);
            var v = q - p;
            BoundingBoxXYZ bb = wall.get_BoundingBox(null);
            double minZ = bb.Min.Z;
            double maxZ = bb.Max.Z;
            double w = v.GetLength();
            double h = maxZ - minZ;
            double offset = 0.1 * w;
            //var min = new XYZ(-w, minZ - offset, -offset);
            //var max = new XYZ(w, maxZ + offset, 0);

            //var temp = new XYZ(X, Y, X);

            var min = new XYZ(-w + 2 * h, -offset - h - 3, -offset);
            var max = new XYZ(w - 2 * h, 3, 0);

            var midpoint = p + 0.5 * v;
            var walldir = v.Normalize();
            var up = XYZ.BasisZ;
            var viewdir = walldir.CrossProduct(up);
            var t = Transform.Identity;
            t.Origin = midpoint;
            t.BasisX = walldir;
            t.BasisY = up;
            t.BasisZ = viewdir;
            var sectionBox = new BoundingBoxXYZ
            {
                Transform = t,
                Min = min,
                Max = max
            };


            //get section type
            var section = new FilteredElementCollector(UiDocument.Document)
                                .OfClass(typeof(ViewFamilyType))
                                .Cast<ViewFamilyType>()
                                .FirstOrDefault(x => ViewFamily.Section == x.ViewFamily);



            using Transaction tx = new(Document);
            tx.Start("Create Wall Section View");
            var createdSec = ViewSection.CreateSection(Document, section.Id, sectionBox);





            //MessageBox.Show(createdSec.Name);

            tx.Commit();


        }
    }
}
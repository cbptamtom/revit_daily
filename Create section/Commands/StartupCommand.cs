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
            //var selectionConfiguration = new SelectionConfiguration()
            //    .Allow.Element(e => e is Wall);
            //var reference = UiDocument.Selection.PickObject(ObjectType.Element, selectionConfiguration.Filter);


            var activeView = UiDocument.Document.ActiveView;
            if (activeView is ViewSheet sheet)
            {
                foreach (var view in sheet.GetAllPlacedViews())
                {
                    var stdBreak = new FilteredElementCollector(UiDocument.Document, view)
                               .OfClass(typeof(FamilyInstance))
                               .Cast<FamilyInstance>()
                               .Where(x => x.Name.Equals("Std Break") || x.Name.Equals("Pipe Break"));
                    var placeView = UiDocument.Document.GetElement(view) as View;
                    var scale = placeView.Scale;
                    using Transaction tx = new(Document);
                    tx.Start("x");
                    foreach (var element in stdBreak)
                    {
                        element.LookupParameter("Scale").Set(scale / 304.8);
                    }
                    tx.Commit();
                }
                MessageBox.Show("Success");

            }




            //get section type
            //var section = new FilteredElementCollector(UiDocument.Document)
            //                    .OfClass(typeof(ViewFamilyType))
            //                    .Cast<ViewFamilyType>()
            //                    .FirstOrDefault(x => ViewFamily.Section == x.ViewFamily);






        }
    }
}
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestingMethod.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class SetParameterSheet : ExternalCommand
    {
        public override void Execute()
        {
            var document = UiDocument.Document;
            var activeView = document.ActiveView;
            using Transaction tx = new(Document);
            tx.Start("x");
            var views = new FilteredElementCollector(document).OfClass(typeof(View)).Cast<View>().ToList();

            MessageBox.Show(views.FirstOrDefault().LookupParameter("View Sort").AsValueString());
            //if (views != null)
            //{
            //    foreach (var view in views)
            //    {
            //        var value = "MEL" + view.LookupParameter("Sub-Discipline");

            //        if (value != null)
            //        {

            //            view.LookupParameter("Sub-Discipline").Set(value);
            //        }
            //    }
            //}

            tx.Commit();


        }
    }
}

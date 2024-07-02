using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
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
    public class CopyTextNotePlus : ExternalCommand
    {
        public override void Execute()
        {


            while (true)
            {
                var referent = UiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                if (referent != null)
                {
                    var text = UiDocument.Document.GetElement(referent) as TextNote;
                    var textValue = (text?.Text);
                    var word = int.Parse(textValue.Split('C')[1]) - 1;
                    //MessageBox.Show(words);
                    using Transaction tx = new(Document);
                    tx.Start("x");
                    text.Text = "C" + word;
                    tx.Commit();
                }
            }



            //if (activeView is ViewSheet sheet)
            //{
            //    foreach (var view in sheet.GetAllPlacedViews())
            //    {
            //        var stdBreak = new FilteredElementCollector(UiDocument.Document, view)
            //                   .OfClass(typeof(FamilyInstance))
            //                   .Cast<FamilyInstance>()
            //                   .Where(x => x.Name.Equals("Std Break") || x.Name.Equals("Pipe Break"));
            //        var placeView = UiDocument.Document.GetElement(view) as View;
            //        var scale = placeView.Scale;
            //        using Transaction tx = new(Document);
            //        tx.Start("x");
            //        foreach (var element in stdBreak)
            //        {
            //            element.LookupParameter("Scale").Set(scale / 304.8);
            //        }
            //        tx.Commit();
            //    }
            //    MessageBox.Show("Success");

            //}
        }
    }
}

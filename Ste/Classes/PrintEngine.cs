using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Ste
{
    class PrintEngine
    {
        public void Export( List<Canvas> vc)
        {
        /*    if (path == null) return;

            if (File.Exists(path.LocalPath))
            {
                File.Delete(path.LocalPath);
            }*/

            // Save current canvas transorm
            Transform transform = vc[0].LayoutTransform;
            // Temporarily reset the layout transform before saving
            for (int i = 0; i < vc.Count; i++)
            {
                vc[i].LayoutTransform = null;

                // Get the size of the canvas
                Size size = new Size(vc[i].Width, vc[i].Height);
                // Measure and arrange elements
                vc[i].Measure(size);
                vc[i].Arrange(new Rect(size));
               
            }
            foreach (var item in vc)
            {
               Print(item);
            }

            // Open new package
            try
            {
             //   Package package = Package.Open(path.LocalPath, FileMode.Create);

                // Create new xps document based on the package opened
             //   XpsDocument doc = new XpsDocument(package);
                // Create an instance of XpsDocumentWriter for the document
             //   XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                // Write the canvas (as Visual) to the document

                // Setup for writing multiple visuals
              //  VisualsToXpsDocument vToXpsD = (VisualsToXpsDocument)writer.CreateVisualsCollator();

                foreach (Visual v in vc)
                {
                 //   vToXpsD.Write(v);   //Write each visual to single page
                }

                // End writing multiple visuals
              ///  vToXpsD.EndBatchWrite();

                // writer.Write(surface);
                // Close document
             //   doc.Close();
                // Close package
              //  package.Close();

                // Restore previously saved layout
                for (int i = 0; i < vc.Count; i++)
                {
                    vc[0].LayoutTransform = transform;
                }
            }
            catch (IOException e) { MessageBox.Show( e.Message); }
        }
        public TextBlock Search(int column, int row, Grid gri)
        {
            foreach (UIElement b in gri.Children)
                if (Grid.GetRow(b) == row && Grid.GetColumn(b) == column)
                    return b as TextBlock;

            return null;
        }
        private void Print(Visual v)
        {

            System.Windows.FrameworkElement e = v as System.Windows.FrameworkElement;
            if (e == null)
                return;

            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                //store original scale
                Transform originalScale = e.LayoutTransform;
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = pd.PrintQueue.GetPrintCapabilities(pd.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / e.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                               e.ActualHeight);

                //Transform the Visual to scale
                e.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                e.Measure(sz);
                e.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                //now print the visual to printer to fit on the one page.
                pd.PrintVisual(v, "My Print");

                //apply the original transform.
                e.LayoutTransform = originalScale;
            }
        }
        public void Export2(Uri path, List<Canvas> vc)
        {
            if (path == null) return;

            if (File.Exists(path.LocalPath))
            {
                File.Delete(path.LocalPath);
            }

            // Save current canvas transorm
            Transform transform = vc[0].LayoutTransform;
            // Temporarily reset the layout transform before saving
            for (int i = 0; i < vc.Count; i++)
            {
                vc[i].LayoutTransform = null;

                // Get the size of the canvas
                Size size = new Size(vc[i].Width, vc[i].Height);
                // Measure and arrange elements
                vc[i].Measure(size);
                vc[i].Arrange(new Rect(size));

            }
          

            // Open new package
            try
            {
                Package package = Package.Open(path.LocalPath, FileMode.Create);

                // Create new xps document based on the package opened
                XpsDocument doc = new XpsDocument(package);
                // Create an instance of XpsDocumentWriter for the document
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                // Write the canvas (as Visual) to the document

                // Setup for writing multiple visuals
                VisualsToXpsDocument vToXpsD = (VisualsToXpsDocument)writer.CreateVisualsCollator();

                foreach (Visual v in vc)
                {
                    vToXpsD.Write(v);   //Write each visual to single page
                }

                // End writing multiple visuals
                vToXpsD.EndBatchWrite();

                // writer.Write(surface);
                // Close document
                doc.Close();
                // Close package
                package.Close();

                // Restore previously saved layout
                for (int i = 0; i < vc.Count; i++)
                {
                    vc[0].LayoutTransform = transform;
                }
            }
            catch (IOException e) { MessageBox.Show(e.Message); }
        }
        public TextBlock Search2(int column, int row, Grid gri)
        {
            foreach (UIElement b in gri.Children)
                if (Grid.GetRow(b) == row && Grid.GetColumn(b) == column)
                    return b as TextBlock;

            return null;
        }
    }
}

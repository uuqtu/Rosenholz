using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MySnippingTool
{
    class Utility
    {
        public static void SaveAsImages(List<Bitmap> images)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int count = 1;
                    foreach (Bitmap img in images)
                    {
                        img.Save(dialog.SelectedPath + "\\Snap_" + (count++) + ".bmp");
                    }
                }
            }
        }

        public static void SaveAsWord(List<Bitmap> images)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Word File | *.doc";
                dialog.DefaultExt = "doc";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Microsoft.Office.Interop.Word.Application app;
                    Microsoft.Office.Interop.Word.Document doc;
                    object miss = System.Reflection.Missing.Value;

                    app = new Microsoft.Office.Interop.Word.Application();
                    app.Visible = false;

                    doc = app.Documents.Add(ref miss, ref miss, ref miss, ref miss);

                    foreach (var img in images)
                    {
                        object start = doc.Content.End - 1;
                        object end = doc.Content.End;
                        Microsoft.Office.Interop.Word.Range rng = doc.Range(ref start, ref end);
                        Clipboard.SetDataObject(img, true);
                        rng.Paste();
                    }

                    doc.SaveAs2(dialog.FileName);

                    doc.Close(ref miss, ref miss, ref miss);
                    app.Quit(ref miss, ref miss, ref miss);
                }
            }
        }
    }    
}

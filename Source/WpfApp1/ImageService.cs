using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


public static class ImageService
{
    public static bool SaveImage(UIElement Source, string FileName =null)
    {
        Nullable<bool> result=true;
        if (String.IsNullOrEmpty(FileName))
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            // Show save file dialog box
            result = dlg.ShowDialog();
            FileName = dlg.FileName;
        }
        // Process save file dialog box results
        if (result == true)
        {
            if (RenderVisualService.RenderToPNGFile(Source, FileName))
                return true ;
        }
        return false;
           
    }
    public static void EmailImage(UIElement Source)
    {
        string fileName = String.Format(Path.GetTempPath()+"work_{0:yyyy-MM-dd_HHmmss}.jpg", DateTime.Now);
        bool status = SaveImage(Source, fileName);
        if (status)
        {
    
            string url = "mailto:?subject=Postcard&attachment="+fileName;
            Process.Start(url);
        }
    }
}

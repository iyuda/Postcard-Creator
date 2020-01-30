using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApp1;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Work : Window
    {
        private const double defaultDpi = 96.0;
        public Work()
        {
         
        }
        /// <summary>
        /// Here we create an object of OpenFileDialog class. For using any DialogBox in
        /// WPF you need Microsoft.Win32 namespace.  In this method
        /// I open the open file dialog box for choosing file which I need to open and 
        /// then load it on to to the image canvas by setting image1.Source = src; 
        /// It also sets evables the dragging for the textbox over the canvas (SetTextDrag) This event is fired on the click event
        /// of Browse Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*"; 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                txtBrowseFile.Text = dlg.FileName;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(txtBrowseFile.Text.Trim(), UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                image1.Source = src;

                SetTextDrag(txtInfo);
                txtInfo.Visibility = Visibility.Visible;
                txtInfo.SelectAll();
            }
        }

        public void SaveImage_click(object sender, RoutedEventArgs e)
        {


            ImageService.SaveImage(canvasMain);
        }
        public void EmailImage_click(object sender, RoutedEventArgs e)
        {
            ImageService.EmailImage(canvasMain);
            //string fileName =SaveImage
            //string url = "mailto:?subject=Postcard&attachment=c:\myfolder\myfile.txt";
            //Process.Start(url);
            //// Process save file dialog box results
            //if (result == true)
            //{
            //    RenderVisualService.RenderToPNGFile(canvasMain, dlg.FileName);
            //}

        }

        /// <summary>
        /// Here we set up mouse events to enable dragging of the control specified by the parameter
        /// </summary>
        /// <param name="control"></param>
        public void SetTextDrag(UIElement control)
        {
            try
            {
                Nullable<Point> dragStart = null;
                MouseButtonEventHandler mouseDown = (sender, args) =>
                {
                    var element = (UIElement)sender;
                    dragStart = args.GetPosition(element);
                    element.CaptureMouse();
                    ((TextBox)sender).Cursor = Cursors.Hand;
                };
                MouseButtonEventHandler mouseUp = (sender, args) =>
                {
                    var element = (UIElement)sender;
                    dragStart = null;
                    element.ReleaseMouseCapture();
                    ((TextBox)sender).Cursor = Cursors.Arrow;
                };
                MouseEventHandler mouseMove = (sender, args) =>
                {
                    if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                    {
                        var element = (UIElement)sender;
                        var p2 = args.GetPosition(canvasMain);
                        Point elementOffset = args.GetPosition(this.Parent as Canvas);

                        Canvas.SetLeft(element, elementOffset.X - dragStart.Value.X);
                        Canvas.SetTop(element, elementOffset.Y - dragStart.Value.Y);
                    }
                    else if (((TextBox)sender).Cursor != Cursors.Arrow)
                        ((TextBox)sender).Cursor = Cursors.Arrow;
                };
                Action<UIElement> enableDrag = (element) =>
                {
                    element.PreviewMouseDown += mouseDown;
                    element.PreviewMouseMove += mouseMove;
                    element.MouseUp += mouseUp;
                };

                enableDrag(control);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errror setting drag events for the text box: {ex.Message}");
                
            }
        }

    }
}

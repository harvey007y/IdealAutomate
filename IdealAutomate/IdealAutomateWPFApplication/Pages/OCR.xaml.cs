using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hardcodet.Wpf.Samples.Pages
{
    /// <summary>
    /// Interaction logic for OCR.xaml
    /// </summary>
    public partial class OCR : UserControl
    {
        private Tesseract _ocr;
        public OCR()
        {
            InitializeComponent();
            _ocr = new Tesseract(AppDomain.CurrentDomain.BaseDirectory + "tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED);
            languageNameLabel.Content = "eng : tesseract + cube";
        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            // Configure open file dialog box 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Image"; // Default file name 
            dlg.DefaultExt = ".png"; // Default file extension 
            dlg.Filter = "Images (.png)|*.png"; // Filter files by extension 

            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Bgr drawColor = new Bgr(System.Drawing.Color.Blue);
                try
                {
                    Image<Bgr, Byte> image = new Image<Bgr, byte>(dlg.FileName);

                    using (Image<Gray, byte> gray = image.Convert<Gray, Byte>())
                    {
                        _ocr.Recognize(gray);
                        Tesseract.Charactor[] charactors = _ocr.GetCharactors();
                        foreach (Tesseract.Charactor c in charactors)
                        {
                            image.Draw(c.Region, drawColor, 1);
                        }

                       // imageBox1.Source = image;

                        //String text = String.Concat( Array.ConvertAll(charactors, delegate(Tesseract.Charactor t) { return t.Text; }) );
                        String text = _ocr.GetText();
                        ocrTextBox.Text = text;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            
        }

        //private void loadLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (openLanguageFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        _ocr.Dispose();
        //        string path = Path.GetDirectoryName(openLanguageFileDialog.FileName);
        //        string lang = Path.GetFileNameWithoutExtension(openLanguageFileDialog.FileName).Split('.')[0];
        //        _ocr = new Tesseract(path, lang, Tesseract.OcrEngineMode.OEM_DEFAULT);
        //        languageNameLabel.Text = String.Format("{0} : tesseract", lang);
        //    }
        //}


    }
}

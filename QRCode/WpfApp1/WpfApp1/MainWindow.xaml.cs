using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.QRCODE;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private System.Drawing.Color SegmentColor { get; set; } = System.Drawing.Color.Black;

        private struct QRSegment
        {
            public QRSegment(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateQRCode_Click(object sender, RoutedEventArgs e)
        {
            string strCorrectionLevel = ((ComboBoxItem)CorrectionLevelComboBox.SelectedValue).Content.ToString();

            QRCode.CorrectionLevel correctionLevel =
                (QRCode.CorrectionLevel)Enum.Parse(typeof(QRCode.CorrectionLevel),
                strCorrectionLevel, true);

            QRCode QRCode1 = new QRCode(); // Вариант 1

            bool[,] Matrix = QRCode1.Generate(DataTextBox.Text, correctionLevel); // Перегенерация

            int FrameSize = 2;

            Bitmap QRCodeBtm = new Bitmap(Matrix.GetLength(0) + (FrameSize * 2), Matrix.GetLength(1) + (FrameSize * 2));

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j])
                    {
                        QRCodeBtm.SetPixel(i + FrameSize, j + FrameSize, SegmentColor);
                    } 
                    else
                    {
                        QRCodeBtm.SetPixel(i + FrameSize, j + FrameSize, System.Drawing.Color.White);
                    }
                }
            }

            BitmapSource b = Imaging.CreateBitmapSourceFromHBitmap(
               QRCodeBtm.GetHbitmap(),
               IntPtr.Zero,
               Int32Rect.Empty,
               BitmapSizeOptions.FromEmptyOptions());

            QRCodeImage.Source = b;
        }

        private void CorrectionLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateQRCode_Click(sender, e);
        }
    }
}

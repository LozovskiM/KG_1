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

namespace KG1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private byte R = 0, G=0, B=0;
        private double X = 0, Y = 0, Z = 0;
        private double H = 0, S = 0, V = 0;
        private double L =0, A =0, B_2 =0;
        private bool xyz = false;
        private bool lab = false;
        private bool hsv = false;

        private void Rect_Change()// byte R, byte G, byte B)
        {
            MRectangle.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
            
        }

        private void RGB_to_XYZ()
        {
            double r = this.R / 255.0;
            double g = this.G / 255.0;
            double b = this.B / 255.0;
            if (r > 0.04045)
                r = Math.Pow(((r + 0.055) / 1.055), 2.4);
            else
                r /= 12.92;
            if (g > 0.04045)
                g = Math.Pow(((g + 0.055) / 1.055), 2.4);
            else
                g /= 12.92;
            if (b > 0.04045)
                b = Math.Pow(((b + 0.055) / 1.055), 2.4);
            else
                b /= 12.92;
            r *= 100;
            g *= 100;
            b *= 100;
            this.X = r * 0.4124 + g * 0.3576 + b * 0.1805;
            this.Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            this.Z = r * 0.0193 + g * 0.1192 + b * 0.9505;


            XSlider.Value = this.X;
            YSlider.Value = this.Y;
            ZSlider.Value = this.Z;
        }

        private void XYZ_to_RGB()
        {       
           

            var x = this.X / 100;
            var y = this.Y / 100;
            var z = this.Z / 100;

            var r = x * 3.2406 + y * -1.5372 + z * -0.4986;
            var g = x * -0.9689 + y * 1.8758 + z * 0.0415;
            var b = x * 0.0557 + y * -0.2040 + z * 1.0570;
          
            if (r > 0.0031308)
                r = 1.055 * (Math.Pow(r, (1 / 2.4))) - 0.055;
            else
                r *= 12.92;
            if (g > 0.0031308)
                g = 1.055 * (Math.Pow(g, (1 / 2.4))) - 0.055;
            else
                g *= 12.92;
            if (b > 0.0031308)
                b = 1.055 * (Math.Pow(b, (1 / 2.4))) - 0.055;
            else
                b *= 12.92;

            r *= 255;
            g *= 255;       
            b *= 255;
            if (r <= 255 && r >= 0)
            {
                this.R = (byte)r;
            }
                
            if (g <= 255 && g >= 0)
            {
                this.G = (byte)g;
            }
            if (b <= 255 && b >= 0)
            {           
                this.B = (byte)b;
            }
        }


        private void RGB_to_HSV()
        {
             double delta, min;

             min = Math.Min(Math.Min(this.R, this.G), this.B);
             this.V = Math.Max(Math.Max(this.R, this.G), this.B);
             delta = this.V - min;

             if (this.V == 0.0)
                 this.S = 0;
             else
                 this.S = delta / this.V;

             if (this.S == 0)
                 this.H = 0.0;

             else
             {
                 if (this.R == this.V)
                     this.H = (this.G - this.B) / delta;
                 else if (this.G == this.V)
                     this.H = 2 + (this.B - this.R) / delta;
                 else if (this.B == this.H)
                     this.H = 4 + (this.R - this.G) / delta;

                 this.H *= 60;

                 if (this.H < 0.0)
                     this.H += 360;
             }

             SliderH.Value = this.H;
             SliderS.Value = this.S;
             SliderV.Value = this.V;
        }

        private void HSV_to_RGB()
        {
                double R, G, B; 
                double hf = this.H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = this.V * (1 - this.S);
                double qv = this.V * (1 - this.S * f);
                double tv = this.V * (1 - this.S * (1 - f));
                switch (i)
                {


                    case 0:
                        R = this.V;
                        G = tv;
                        B = pv;
                        break;


                    case 1:
                        R = qv;
                        G = this.V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = this.V;
                        B = tv;
                        break;


                    case 3:
                        R = pv;
                        G = qv;
                        B = this.V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = this.V;
                        break;


                    case 5:
                        R = this.V;
                        G = pv;
                        B = qv;
                        break;


                    case 6:
                        R = this.V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = this.V;
                        G = pv;
                        B = qv;
                        break;


                    default:
                        R = G = B = this.V; 
                        break;
                }
            
            this.R = (byte)Clamp((int)(R * 255.0));
            this.G = (byte)Clamp((int)(G * 255.0));
            this.B = (byte)Clamp((int)(B * 255.0));
        }

        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        private void LAB_to_XYZ()
        {

            double e = 0.008856;
            double k = 903.3;
            double fy = (this.L + 16) / 116;
            double fz = fy - this.B_2 / 200;
            double fx = this.A / 500 + fy;
            double zr, yr, xr;

            if (fz * fz * fz > e)
                zr = fz * fz * fz;
            else
                zr = (116 * fz - 16) / k;

            if (this.L > k*e)
                yr = ((this.L+16)/116)* ((this.L + 16) / 116)* ((this.L + 16) / 116);
            else
                yr = this.L/k;

            if (fx * fx * fx > e)
                xr = fx * fx * fx;
            else
                xr = (116 * fx - 16) / k;
            this.X = 95.047*xr;
            this.Y = 100*yr;
            this.Z = 108.883*zr;
            XSlider.Value = this.X;
            YSlider.Value = this.Y;
            ZSlider.Value = this.Z;
        }


        private void XYZ_to_LAB()
        {

            double e = 0.008856;
            double k = 903.3;
            double fy;
            double fz;
            double fx;
            double zr = this.Z/ 5.047, yr = this.Y/100, xr = this.X / 95.047;

            if (xr > e)
                fx = Math.Pow(xr, 1 / 3);
            else
                fx = (k * xr + 16) / 116;

            if (yr > e)
                fy = Math.Pow(yr, 1 / 3);
            else
                fy = (k * yr + 16) / 116;

            if (zr > e)
                fz = Math.Pow(zr, 1 / 3);
            else
                fz = (k * zr + 16) / 116;

            this.L = 116*fy-16;
            this.A = 500*(fx-fy);
            this.B_2 = 200*(fy-fz);
            LSlider.Value = this.L;
            ASlider.Value = this.A;
            BSlider.Value = this.B_2;
        }


        /// <summary>
        /// XYZ event functionality
        /// </summary>
        /// <param name="sender"> Event host </param> 
        /// <param name="e">changed detector</param>
        /// 

        private void XSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.lab || this.hsv)
                return;
            this.xyz = true;
            var slider = sender as Slider;
            this.X = slider.Value;
            XYZ_to_RGB();
            RGB_to_HSV();
            XYZ_to_LAB();
            Rect_Change();
            this.xyz = false;
        }
        private void YSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.lab || this.hsv)
                return;
            this.xyz = true;
            var slider = sender as Slider;
            this.Y = slider.Value;
            XYZ_to_RGB();
            RGB_to_HSV();
            XYZ_to_LAB();
            Rect_Change();
            this.xyz = false;
        }

        private void ZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.lab || this.hsv)
                return;
            this.xyz = true;
            var slider = sender as Slider;
            this.Z = slider.Value;
            XYZ_to_RGB();
            RGB_to_HSV();
            XYZ_to_LAB();
            Rect_Change();
            this.xyz = false;

        }

        

        private void BtRed_Click(object sender, RoutedEventArgs e)
        {
            this.R = 255;
            this.G = 0;
            this.B = 0;
            Rect_Change();
        }

        private void BtYellow_Click(object sender, RoutedEventArgs e)
        {
            this.R = 255;
            this.G = 255;
            Rect_Change();  

        }

        private void TbGreen_Click(object sender, RoutedEventArgs e)
        {
            this.R = 0;
            this.B = 0;
            this.G = 255;
            Rect_Change();

        }

        private void BtBlue_Click(object sender, RoutedEventArgs e)
        {
            this.R = 0;
            this.B = 255;
            this.G = 0;
            Rect_Change();
            
        }

        private void BtPurple_Click(object sender, RoutedEventArgs e)
        {
            this.R = 150;
            this.B = 100;
            this.G = 0;
            Rect_Change();

        }

        private void BtGray_Click(object sender, RoutedEventArgs e)
        {
            this.R = 150;
            this.B = 150;
            this.G = 150;
            Rect_Change();

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = tbHex as TextBox;
            String s = tb.Text;
            MRectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(s));

        }

        /// <summary>
        /// HSV event functionality
        /// </summary>
        /// <param name="sender"> Event host </param> 
        /// <param name="e">changed detector</param>
        /// 

        private void LSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.xyz || this.hsv)
                return;
            this.lab = true;
            Slider slider = sender as Slider;
            this.L = slider.Value;
            LAB_to_XYZ();
            XYZ_to_RGB();
            RGB_to_HSV();
            Rect_Change();
            this.lab = false;
        }

        private void ASlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.xyz || this.hsv)
                return;
            this.lab = true;
            Slider slider = sender as Slider;
            this.A = slider.Value;
            LAB_to_XYZ();
            XYZ_to_RGB();
            RGB_to_HSV();
            Rect_Change();
            this.lab = false;

        }

        private void BSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.xyz || this.hsv)
                return;
            this.lab = true;
            Slider slider = sender as Slider;
            this.B_2 = slider.Value;
            LAB_to_XYZ();
            XYZ_to_RGB();
            RGB_to_HSV();
            Rect_Change();
            this.lab = false;
        }



        /// <summary>
        /// HSV event functionality
        /// </summary>
        /// <param name="sender"> Event host </param> 
        /// <param name="e">changed detector</param>
        private void SliderH_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.xyz || this.lab)
                return;
            this.hsv = true;
            var slider = sender as Slider;
            this.H = slider.Value;
            HSV_to_RGB();
            RGB_to_XYZ();
            XYZ_to_LAB();
            Rect_Change();
            this.hsv = false;
        }

        private void SliderS_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.xyz || this.lab)
                return;
            this.hsv = true;
            var slider = sender as Slider;
            this.S = slider.Value;
            HSV_to_RGB();
            RGB_to_XYZ();
            XYZ_to_LAB();
            Rect_Change();
            this.hsv = false;
        }

        private void SliderV_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.xyz || this.lab)
                return;
            this.hsv = true;
            var slider = sender as Slider;
            this.V = slider.Value;
            HSV_to_RGB();
            RGB_to_XYZ();
            XYZ_to_LAB();
            Rect_Change();
            this.hsv = false;
        }
    }
}

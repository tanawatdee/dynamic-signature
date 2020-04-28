//#define DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shoulder_surfing_protecter
{
    /// <summary>
    /// Interaction logic for Signwindow.xaml
    /// </summary>
    public partial class Signwindow : Window
    {
        Point currentPoint = new Point();
        System.Windows.Threading.DispatcherTimer tPlay = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer tPenup = new System.Windows.Threading.DispatcherTimer();
        Line line;
        List<sPoint> encode = new List<sPoint>();
        BinaryReader br;
        BinaryWriter bw;
        short tick = 0;
        string id;
        bool real, passDTW, passIMP;
        User usr1 = new User();
        public Signwindow(string uName, bool real)
        {
            InitializeComponent();
            tPlay.Tick += new EventHandler(tPlay_Tick);
            tPlay.Interval = new TimeSpan(0, 0, 0, 0, 10);
            tPenup.Tick += new EventHandler(tPenup_Tick);
            tPenup.Interval = new TimeSpan(0, 0, 0, 0, 10);
            lblId.Content = id = uName;
            if (!usr1.open(uName))
            {
                MessageBox.Show("User not found.");
                this.Close();
                return;
            }
            this.Show();
            usr1.startSign();
        }

        //Mouse------------------------------------------------------------------------------------------------
        private void paintSurface_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(this);
                usr1.addSign(0x7fff, tick);
                usr1.addSign((short)currentPoint.X, (short)currentPoint.Y);
                tPlay.Start();
            }
        }

        private void paintSurface_MouseUp(object sender, MouseButtonEventArgs e)
        {
            tPlay.Stop();
            tick = 0;
            tPenup.Start();
        }

        //Button-----------------------------------------------------------------------------------------------
        private void btnok_Click(object sender, RoutedEventArgs e)
        {
            tPenup.Stop();
            tick = 0;
            usr1.zNorm();
            passDTW = usr1.check(0);

            /*opt
            for (int i = 1; i < 3; i++ )
            {
                if (passDTW) break;
                passDTW = usr1.check(i);
            }
            */

                if (passDTW)
                {
                    usr1.addEncode();
                    usr1.update();
                    usr1.save();
                }
            usr1.startSign();
            paintSurface.Children.Clear();
            MessageBox.Show(passDTW?"Pass":"Wrong");
            //this.Close();
        }

        private void btndel_Click(object sender, RoutedEventArgs e)
        {
            tPenup.Stop();
            tick = 0;
            usr1.startSign();
            paintSurface.Children.Clear();
        }

        private void btnX_Click(object sender, RoutedEventArgs e)
        {
            tPenup.Stop();
            usr1.startSign();
            tick = 0;
            this.Close();
        }

        //Timer------------------------------------------------------------------------------------------------
        private void tPlay_Tick(object sender, EventArgs e)
        {
            line = new Line();
            line.Stroke = SystemColors.WindowFrameBrush;
            line.StrokeThickness = 3;
            line.StrokeEndLineCap = PenLineCap.Round;
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            currentPoint = Mouse.GetPosition(paintSurface);
            line.X2 = currentPoint.X;
            line.Y2 = currentPoint.Y;
            paintSurface.Children.Add(line);
            usr1.addSign((short)currentPoint.X, (short)currentPoint.Y);
        }

        private void tPenup_Tick(object sender, EventArgs e)
        {
            tick++;
        }

        //UserChange----------------------------------------------------------------------------------------------------------

        /*private void lblId_TextChanged(object sender, TextChangedEventArgs e)
        {
            string uName = id = lblId.Text;
            if (!usr1.open(uName))
            {
                MessageBox.Show("User not found.");
                this.Close();
            }
        }*/

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
        
       
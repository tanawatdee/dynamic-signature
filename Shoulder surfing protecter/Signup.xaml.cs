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
    /// Interaction logic for SignUpwindow.xaml
    /// </summary>
    public partial class Signup : Window
    {
        const int NUMOFBAR = 10;
        Point currentPoint = new Point();
        System.Windows.Threading.DispatcherTimer tPlay = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer tPenup = new System.Windows.Threading.DispatcherTimer();
        Line line;
        byte iBar = 0;
        short tick = 0;
        User usr1 = new User();
        public Signup(string fName, string sName, string uName)
        {
            if(!usr1.newUser(0, uName, fName, sName))
            {
                MessageBox.Show("User already exists.");
                return;
            }
            InitializeComponent();
            tPlay.Tick += new EventHandler(tPlay_Tick);
            tPlay.Interval = new TimeSpan(0, 0, 0, 0, 10);
            tPenup.Tick += new EventHandler(tPenup_Tick);
            tPenup.Interval = new TimeSpan(0, 0, 0, 0, 10);
            lblId.Content = uName;
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

        private void btnDel_Click_1(object sender, RoutedEventArgs e)
        {
            tPenup.Stop();
            tick = 0;
            paintSurface.Children.Clear();
            if (iBar == 0) return;
            usr1.delEncode();
            usr1.startSign();
            iBar--;
            grdBar.Children.RemoveAt(iBar);
        }

        private void btnok_Click(object sender, RoutedEventArgs e)
        {
            tPenup.Stop();
            tick = 0;
            bool flag = false;
            usr1.zNorm();
            for (int i = 0; i < iBar; i++ )
            {
                if (usr1.check(i))
                {
                    flag = true;
                    break;
                }
            }
            if(iBar == 0 || flag)
            {

                Grid G = new Grid();
                G.SetValue(Grid.ColumnProperty, (int)iBar);
                G.SetValue(Grid.RowProperty, 0);
                G.SetValue(Grid.BackgroundProperty, new SolidColorBrush(Colors.Blue));
                grdBar.Children.Add(G);
                usr1.addEncode();
                if(iBar == NUMOFBAR - 1)
                {
                    usr1.create();
                    usr1.save();
                    MessageBox.Show("Signup successful.");
                    this.Close();
                }
                iBar++;
            }
            usr1.startSign();
            paintSurface.Children.Clear();
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

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}


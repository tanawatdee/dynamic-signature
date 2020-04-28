using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Createwindow.xaml
    /// </summary>
    public partial class Createwindow : Window
    {
        public Createwindow()
        {
            InitializeComponent();
        }

        private void lblpassword_Click(object sender, RoutedEventArgs e)
        {
            Regex r = new Regex("^[a-zA-Z0-9]+$");
            if (!(r.IsMatch(txtName.Text) && r.IsMatch(txtSur.Text) && r.IsMatch(txtUser.Text)))
            {
                MessageBox.Show("Alphanumberic only.");
                return;
            }
            Signup S = new Signup(txtName.Text, txtSur.Text, txtUser.Text);
            if (S.IsInitialized)
            {
                S.Show();
                this.Close();
            }
        }
    }
}

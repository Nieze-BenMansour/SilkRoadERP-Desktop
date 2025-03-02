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
using System.Windows.Shapes;

namespace Ste
{
    /// <summary>
    /// Logique d'interaction pour WindowTotalBL.xaml
    /// </summary>
    public partial class WindowTotalBL : Window
    {
        public WindowTotalBL()
        {
            InitializeComponent();
        }

       

        private void Close(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                this.Close();
            }
        }
    }
}

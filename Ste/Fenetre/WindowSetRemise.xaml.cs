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
    /// Logique d'interaction pour WindowSetRemise.xaml
    /// </summary>
    public partial class WindowSetRemise : Window
    {
        public string ALL_remise=null;
        public WindowSetRemise()
        {
            InitializeComponent();
        }


        private void CloseAndSend(object sender, KeyEventArgs e)
        {
            float o = 0;
            if(e.Key == Key.Enter)
            {
                if (float.TryParse(setterAllRemise.Text.ToString(),out o))
                {
                    ALL_remise = setterAllRemise.Text;
                    this.Close();
                }
            }
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void preview_Text_Input(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.Text, e.Text.Length - 1))
                {
                    e.Handled = true;

                }
            }
            catch (Exception)
            {
            }
        }

        
    }
}

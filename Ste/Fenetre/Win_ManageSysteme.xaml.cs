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
using Service;
using Domain.Models;


namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for Win_ManageSysteme.xaml
    /// </summary>
    public partial class Win_ManageSysteme : Window
    {
        SystemeService serv_systeme = new SystemeService();
        public Win_ManageSysteme()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Systeme sys = new Systeme();
            sys = serv_systeme.findById(1);
            nomSocieteTextBox.Text = sys.NomSociete;
            timbreTextBox.Text = sys.Timbre.ToString();
            adresseTextBox.Text = sys.adresse;
            telTextBox.Text = sys.tel.ToString();
            faxTextBox.Text = sys.fax.ToString();
            emailTextBox.Text = sys.email;
            codeTVATextBox.Text = sys.codeTVA.ToString();
            matriculeFiscaleTextBox.Text = sys.matriculeFiscale.ToString();
            codeCategorieTextBox.Text = sys.codeCategorie.ToString();
            etbSecondaireTextBox.Text = sys.etbSecondaire;
            pourcentageFodecTextBox.Text = sys.pourcentageFodec.ToString();
            adresseRetenuTextBox.Text = sys.adresseRetenu;
            pourcentageRetenuTextBox.Text = sys.pourcentageRetenu.ToString();
        }

        private void ValiderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Systeme sys = new Systeme();
                sys = serv_systeme.findById(1);
                sys.NomSociete = nomSocieteTextBox.Text;
                sys.Timbre = decimal.Parse(timbreTextBox.Text);
                sys.adresse = adresseTextBox.Text;
                sys.tel = telTextBox.Text;
                sys.fax = faxTextBox.Text;
                sys.email = emailTextBox.Text;
                sys.codeTVA = codeTVATextBox.Text;
                sys.matriculeFiscale = matriculeFiscaleTextBox.Text;
                sys.codeCategorie = codeCategorieTextBox.Text;
                sys.etbSecondaire = etbSecondaireTextBox.Text;
                sys.pourcentageFodec = decimal.Parse(pourcentageFodecTextBox.Text);
                sys.adresseRetenu = adresseRetenuTextBox.Text;
                sys.pourcentageRetenu = double.Parse(pourcentageRetenuTextBox.Text);
                serv_systeme.editSysteme(sys);
                MessageBox.Show("Données modifié !");
            }
            catch (Exception)
            {
                MessageBox.Show("Probleme ");
            }
        }
    }
}

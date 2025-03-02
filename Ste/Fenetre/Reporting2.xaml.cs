using Microsoft.Reporting.WinForms;
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
    /// Interaction logic for Reporting2.xaml
    /// </summary>
    public partial class Reporting2 : Window
    {
        BonDeReceptionService serBR = new BonDeReceptionService();
        FournisseurService ser_Four = new FournisseurService();
        LigneBonDeRecepctionService serLigneBl = new LigneBonDeRecepctionService();
        Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
        public Reporting2()
        {
            InitializeComponent();

            /*    Ste.SteDataBaseDataSet steDataBaseDataSet = ((Ste.SteDataBaseDataSet)(this.FindResource("steDataBaseDataSet")));
                // Load data into the table DataTable1. You can modify this code as needed.
                Ste.SteDataBaseDataSetTableAdapters.DataTable1TableAdapter steDataBaseDataSetDataTable1TableAdapter = new Ste.SteDataBaseDataSetTableAdapters.DataTable1TableAdapter();
                steDataBaseDataSetDataTable1TableAdapter.Fill(steDataBaseDataSet.DataTable1);
                System.Data.DataTable dd= new System.Data.DataTable("DataTable1", "steDataBaseDataSet");

                IEnumerable<BonDeReception> bls = serBR.getAllBonDeReception();
                IEnumerable<Fournisseur> Fours = ser_Four.getAllFournisseur();

                Microsoft.Reporting.WinForms.ReportViewer reportViewer = (Microsoft.Reporting.WinForms.ReportViewer)windowsFormsHost.Child;
                reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                reportViewer.LocalReport.ReportEmbeddedResource = "Ste.Report4.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", steDataBaseDataSetDataTable1TableAdapter));
             //   reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", Fours));
               // reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", bls , Fours));
                reportViewer.RefreshReport();*/
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Ste.SteDataBaseDataSet dataset = new Ste.SteDataBaseDataSet();

            dataset.BeginInit();

            reportDataSource1.Name = "DataSet1"; //Name of the report dataset in our .RDLC file
            reportDataSource1.Value = dataset.DataTable1;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.ReportEmbeddedResource = "Ste.Report3.rdlc";

            dataset.EndInit();

            //fill data into adventureWorksDataSet
            Ste.SteDataBaseDataSetTableAdapters.DataTable1TableAdapter steDataBaseDataSetDataTable1TableAdapter = new Ste.SteDataBaseDataSetTableAdapters.DataTable1TableAdapter();
            steDataBaseDataSetDataTable1TableAdapter.ClearBeforeFill = true;
            steDataBaseDataSetDataTable1TableAdapter.Fill(dataset.DataTable1,"SOREL");

            _reportViewer.RefreshReport();

            _isReportViewerLoaded = true;
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {

            // our subreport has only 1 dataset 
       //     string dataSourceName = "DataSet1";
            // get the orders for the current customer
     //       Facture fac = ser_fac.findFactureByNum(8055);
     //       IEnumerable<BonDeLivraison> orders = serBl.findBonDeLivraisonBynumFacture(fac);

      //      e.DataSources.Add(new ReportDataSource(dataSourceName, orders));
        }


        private bool _isReportViewerLoaded;

        private void ReportViewer_Load()
        {
            if (!_isReportViewerLoaded)
            {
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
                SteDataBaseDataSet1 dataset = new SteDataBaseDataSet1();

                dataset.BeginInit();

                reportDataSource1.Name = "DataSet1"; //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.Facture;
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                this._reportViewer.LocalReport.ReportEmbeddedResource = "Ste.Report2.rdlc";

                dataset.EndInit();

                //fill data into adventureWorksDataSet
                SteDataBaseDataSet1.FactureDataTable salesOrderDetailTableAdapter = new SteDataBaseDataSet1.FactureDataTable();
                //    salesOrderDetailTableAdapter.ClearBeforeFill = true;
                //  salesOrderDetailTableAdapter.Fill(dataset.Facture);

                _reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           /* Ste.SteDataBaseDataSet steDataBaseDataSet = ((Ste.SteDataBaseDataSet)(this.FindResource("steDataBaseDataSet")));
            // Load data into the table DataTable1. You can modify this code as needed.
            Ste.SteDataBaseDataSetTableAdapters.DataTable1TableAdapter steDataBaseDataSetDataTable1TableAdapter = new Ste.SteDataBaseDataSetTableAdapters.DataTable1TableAdapter();
            steDataBaseDataSetDataTable1TableAdapter.Fill(steDataBaseDataSet.DataTable1);
            System.Windows.Data.CollectionViewSource dataTable1ViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("dataTable1ViewSource")));
            dataTable1ViewSource.View.MoveCurrentToFirst();*/
        }
    }
}

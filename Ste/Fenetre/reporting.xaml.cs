using Domain.Models;
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
using Microsoft.Reporting.WinForms;

namespace Ste.Fenetre
{
    /// <summary>
    /// Interaction logic for reporting.xaml
    /// </summary>
    public partial class reporting : Window
    {
        BonDeLivraisonService serBl = new BonDeLivraisonService();
        FactureService ser_fac = new FactureService();
        LigneBLService serLigneBl = new LigneBLService();
        Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
        public reporting()
        {
            InitializeComponent();

            //Facture fac = ser_fac.findFactureByNum(8055);
            // get the customers from the OData service
            IEnumerable<LigneBL> bls = serLigneBl.findLigneBlByNumBL(4098);
            IEnumerable<Facture> facs= ser_fac.getAllFacture();
            
            // get a reference to the WinForms ReportViewer control
            Microsoft.Reporting.WinForms.ReportViewer reportViewer = (Microsoft.Reporting.WinForms.ReportViewer)windowsFormsHost.Child;
            // subscribe to SubreportProcessing event for passing the related Orders data
            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            // the report is included as an embedded resource in the assembly
            // use the full name
            reportViewer.LocalReport.ReportEmbeddedResource = "Ste.Report2.rdlc";
            // the CustomerReport has one dataset which must be filled
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", bls));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", facs));
            Facture fac = new Facture();
            fac = ser_fac.findFactureByNum(2040);
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("ReportParameter1", "2040");
            
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", serBl.findBonDeLivraisonBynumFacture(fac)));
            reportViewer.LocalReport.SetParameters(param);
            // now let the ReportViewer render the report
            reportViewer.RefreshReport();


        }



        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
           
            // our subreport has only 1 dataset 
            string dataSourceName = "DataSet1";
            // get the orders for the current customer
            Facture fac = ser_fac.findFactureByNum(8055);
            IEnumerable<BonDeLivraison> orders = serBl.findBonDeLivraisonBynumFacture(fac);

            e.DataSources.Add(new ReportDataSource(dataSourceName, orders));
        }


        private bool _isReportViewerLoaded;

        private void ReportViewer_Load( )
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
    }
}

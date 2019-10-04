using System;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace K18078
{
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            List<MyComplexObject> l = GenerateRecords();
            using (XtraReport r = new XtraReport()) {
                r.Landscape = true;
                ReportBuilderHelper rbh = new ReportBuilderHelper();
                rbh.GenerateReport(r, l);
                r.ShowPreviewDialog();
            }
        }

        private List<MyComplexObject> GenerateRecords() {
            return new List<MyComplexObject>() {
                new MyComplexObject() {
                    Address = "507 - 20th Ave. E.Apt. 2A", FirstName = "Nancy", LastName = "Davolio",
                    City = "Seattle", Age = 30, FederalId = "11-1123-12345", Salary = 6000
                },
                new MyComplexObject(){
                    Address = "908 W. Capital Way", FirstName = "Andrew", LastName = "Fuller",
                    City = "Tacoma", Age = 33, FederalId = "99-8888-11111", Salary = 8000
                },
                new MyComplexObject(){
                    Address = "722 Moss Bay Blvd.", FirstName = "Janet",  LastName = "Leverling",
                    City = "Kirkland", Age = 22, FederalId = "33-4444-55555", Salary = 6000
                }
            };
        }
    }
}
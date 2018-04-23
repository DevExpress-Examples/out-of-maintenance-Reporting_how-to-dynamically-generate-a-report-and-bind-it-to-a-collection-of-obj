using System;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace K18078 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            List<MyComplexObject> l = GenerateRecords();
            XtraReport r = new XtraReport();
            r.Landscape = true;
            ReportBuilderHelper rbh = new ReportBuilderHelper();
            rbh.GenerateReport(r, l);
            r.ShowPreviewDialog();
        }

        private List<MyComplexObject> GenerateRecords() {
            List<MyComplexObject> l = new List<MyComplexObject>();
            MyComplexObject o1 = new MyComplexObject();
            o1.Address = "507 - 20th Ave. E.Apt. 2A";
            o1.FirstName = "Nancy";
            o1.LastName = "Davolio";
            o1.City = "Seattle";
            o1.Age = 30;
            o1.FederalId = "11-1123-12345";
            o1.Salary = 6000;
            l.Add(o1);

            o1 = new MyComplexObject();
            o1.Address = "908 W. Capital Way";
            o1.FirstName = "Andrew";
            o1.LastName = "Fuller";
            o1.City = "Tacoma";
            o1.Age = 33;
            o1.FederalId = "99-8888-11111";
            o1.Salary = 8000;
            l.Add(o1);

            o1 = new MyComplexObject();
            o1.Address = "722 Moss Bay Blvd.";
            o1.FirstName = "Janet";
            o1.LastName = "Leverling";
            o1.City = "Kirkland";
            o1.Age = 22;
            o1.FederalId = "33-4444-55555";
            o1.Salary = 6000;
            l.Add(o1);
            return l;
        }
    }
}
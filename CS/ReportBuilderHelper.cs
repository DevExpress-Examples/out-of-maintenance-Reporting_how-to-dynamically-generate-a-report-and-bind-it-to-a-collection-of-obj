using System;
using System.Drawing;
using System.Reflection;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace K18078 {
    public class ReportBuilderHelper {
        List<DataSourceDefinition> dsd;
        public XtraReport GenerateReport(List<MyComplexObject> list) {
            XtraReport r = new XtraReport();
            r.DataSource = list;
            dsd = GenerateDataSourceDefinition(list[0]);
            InitBands(r);
            InitDetailsBasedOnXRLabel(r, dsd);
            return r;
        }
        public void GenerateReport(XtraReport r, List<MyComplexObject> list) {
            r.DataSource = list;
            dsd = GenerateDataSourceDefinition(list[0]);
            InitBands(r);
            InitDetailsBasedOnXRLabel(r, dsd);
        }
               private void InitDetailsBasedOnXRLabel(XtraReport rep, List<DataSourceDefinition> dsd) {
            int colCount = dsd.Count;
            int totalf=0;
            for(int i = 0; i < dsd.Count; i++) 
                totalf += dsd[i].Factor;
            int fWidth = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / totalf;
            int incShift = 0;
            for(int i = 0; i < colCount; i++) {
                XRLabel labelh = CreateLabel(dsd[i], fWidth, incShift);
                labelh.Text = dsd[i].CaptionName;

                XRLabel labeld = CreateLabel(dsd[i], fWidth, incShift);
                labeld.DataBindings.Add("Text", null, dsd[i].Fieldname);

                if(i > 0) {
                    labelh.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                    labeld.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
   
                }
                else {
                    labelh.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    labeld.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
                }
                incShift += fWidth * dsd[i].Factor;

                // Place the headers onto a PageHeader band
                rep.Bands[BandKind.PageHeader].Controls.Add(labelh);
                rep.Bands[BandKind.Detail].Controls.Add(labeld);
            }
        }

        private static XRLabel CreateLabel(DataSourceDefinition dsd, int fWidth, int incShift) {
            XRLabel labeld = new XRLabel();
            labeld.Location = new Point(incShift, 0);
            labeld.Size = new Size(fWidth * dsd.Factor, 20);
            return labeld;
        }

        private List<DataSourceDefinition> GenerateDataSourceDefinition(object myComplexObject) {
            List<DataSourceDefinition> dsdl = new List<DataSourceDefinition>();
            PropertyInfo[] pi = myComplexObject.GetType().GetProperties();
            for(int i = 0; i < pi.Length; i++) {
                Reportable[] r = pi[i].GetCustomAttributes(typeof(Reportable), false) as Reportable[];
                if(r.Length > 0) {
                    DataSourceDefinition dsd = new DataSourceDefinition();
                    dsd.CaptionName  = r[0].AlternateName == null ? pi[i].Name :  r[0].AlternateName;
                    dsd.Fieldname = pi[i].Name;
                    dsd.Factor = r[0].LenFactor == 0 ? 1 : r[0].LenFactor;
                    dsdl.Add(dsd);
                }
            }
            return dsdl;      
        }
        public void InitBands(XtraReport rep) {
            // Create bands
            DetailBand detail = new DetailBand();
            PageHeaderBand pageHeader = new PageHeaderBand();
            ReportFooterBand reportFooter = new ReportFooterBand();
            detail.Height = 20;
            reportFooter.Height = 380;
            pageHeader.Height = 20;

            // Place the bands onto a report
            rep.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { detail, pageHeader, reportFooter });
        }
    }
    public class DataSourceDefinition {
        string fieldname;

        public string Fieldname {
            get { return fieldname; }
            set { fieldname = value; }
        }
        string captionName;

        public string CaptionName {
            get { return captionName; }
            set { captionName = value; }
        }
        int factor;

        public int Factor {
            get { return factor; }
            set { factor = value; }
        }
    
    }
}

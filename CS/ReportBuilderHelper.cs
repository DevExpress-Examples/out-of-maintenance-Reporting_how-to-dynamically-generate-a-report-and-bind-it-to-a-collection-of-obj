using System;
using System.Drawing;
using System.Reflection;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace K18078
{
    public class ReportBuilderHelper
    {
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
            int totalf = 0;
            for (int i = 0; i < dsd.Count; i++)
                totalf += dsd[i].Factor;
            float fWidth = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / totalf;
            float incShift = 0;
            for (int i = 0; i < colCount; i++) {
                XRLabel labelh = CreateLabel(dsd[i], fWidth, incShift);
                labelh.Text = dsd[i].CaptionName;

                XRLabel labeld = CreateLabel(dsd[i], fWidth, incShift);
                labeld.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", String.Format("[{0}]", dsd[i].Fieldname)));


                if (i > 0) {
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

        private static XRLabel CreateLabel(DataSourceDefinition dsd, float fWidth, float incShift) {
            return new XRLabel() {
                LocationF = new PointF(incShift, 0),
                SizeF = new SizeF(fWidth * dsd.Factor, 20)
            };
        }

        private List<DataSourceDefinition> GenerateDataSourceDefinition(object myComplexObject) {
            List<DataSourceDefinition> dsdl = new List<DataSourceDefinition>();
            PropertyInfo[] pi = myComplexObject.GetType().GetProperties();
            for (int i = 0; i < pi.Length; i++) {
                Reportable[] r = pi[i].GetCustomAttributes(typeof(Reportable), false) as Reportable[];
                if (r.Length > 0) {
                    dsdl.Add(new DataSourceDefinition() {
                        CaptionName = r[0].AlternateName == null ? pi[i].Name : r[0].AlternateName,
                        Fieldname = pi[i].Name,
                        Factor = r[0].LenFactor == 0 ? 1 : r[0].LenFactor
                    });
                }
            }
            return dsdl;
        }
        public void InitBands(XtraReport rep) {
            // Create bands
            DetailBand detail = new DetailBand() {
                HeightF = 20f
            };
            PageHeaderBand pageHeader = new PageHeaderBand() {
                HeightF = 20f
            };
            ReportFooterBand reportFooter = new ReportFooterBand() {
                HeightF = 380f
            };

            // Place the bands onto a report
            rep.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { detail, pageHeader, reportFooter });
        }
    }
    public class DataSourceDefinition
    {
        string _fieldname;

        public string Fieldname {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        string _captionName;

        public string CaptionName {
            get { return _captionName; }
            set { _captionName = value; }
        }
        int _factor;

        public int Factor {
            get { return _factor; }
            set { _factor = value; }
        }

    }
}

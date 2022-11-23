Imports System.Drawing
Imports System.Reflection
Imports DevExpress.XtraReports.UI
Imports System.Collections.Generic

Namespace K18078

    Public Class ReportBuilderHelper

        Private dsd As List(Of DataSourceDefinition)

        Public Function GenerateReport(ByVal list As List(Of MyComplexObject)) As XtraReport
            Dim r As XtraReport = New XtraReport()
            r.DataSource = list
            dsd = GenerateDataSourceDefinition(list(0))
            InitBands(r)
            InitDetailsBasedOnXRLabel(r, dsd)
            Return r
        End Function

        Public Sub GenerateReport(ByVal r As XtraReport, ByVal list As List(Of MyComplexObject))
            r.DataSource = list
            dsd = GenerateDataSourceDefinition(list(0))
            InitBands(r)
            InitDetailsBasedOnXRLabel(r, dsd)
        End Sub

        Private Sub InitDetailsBasedOnXRLabel(ByVal rep As XtraReport, ByVal dsd As List(Of DataSourceDefinition))
            Dim colCount As Integer = dsd.Count
            Dim totalf As Integer = 0
            For i As Integer = 0 To dsd.Count - 1
                totalf += dsd(i).Factor
            Next

            Dim fWidth As Integer =(rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) \ totalf
            Dim incShift As Integer = 0
            For i As Integer = 0 To colCount - 1
                Dim labelh As XRLabel = CreateLabel(dsd(i), fWidth, incShift)
                labelh.Text = dsd(i).CaptionName
                Dim labeld As XRLabel = CreateLabel(dsd(i), fWidth, incShift)
                labeld.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", String.Format("[{0}]", dsd(i).Fieldname)))
                If i > 0 Then
                    labelh.Borders = DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
                    labeld.Borders = DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Bottom
                Else
                    labelh.Borders = DevExpress.XtraPrinting.BorderSide.All
                    labeld.Borders = DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Bottom
                End If

                incShift += fWidth * dsd(i).Factor
                ' Place the headers onto a PageHeader band
                rep.Bands(BandKind.PageHeader).Controls.Add(labelh)
                rep.Bands(BandKind.Detail).Controls.Add(labeld)
            Next
        End Sub

        Private Shared Function CreateLabel(ByVal dsd As DataSourceDefinition, ByVal fWidth As Single, ByVal incShift As Single) As XRLabel
            Return New XRLabel() With {.LocationF = New PointF(incShift, 0), .SizeF = New SizeF(fWidth * dsd.Factor, 20)}
        End Function

        Private Function GenerateDataSourceDefinition(ByVal myComplexObject As Object) As List(Of DataSourceDefinition)
            Dim dsdl As List(Of DataSourceDefinition) = New List(Of DataSourceDefinition)()
            Dim pi As PropertyInfo() = myComplexObject.GetType().GetProperties()
            For i As Integer = 0 To pi.Length - 1
                Dim r As Reportable() = TryCast(pi(i).GetCustomAttributes(GetType(Reportable), False), Reportable())
                If r.Length > 0 Then
                    dsdl.Add(New DataSourceDefinition() With {.CaptionName = If(Equals(r(0).AlternateName, Nothing), pi(i).Name, r(0).AlternateName), .Fieldname = pi(i).Name, .Factor = If(r(0).LenFactor = 0, 1, r(0).LenFactor)})
                End If
            Next

            Return dsdl
        End Function

        Public Sub InitBands(ByVal rep As XtraReport)
            ' Create bands
            Dim detail As DetailBand = New DetailBand() With {.HeightF = 20F}
            Dim pageHeader As PageHeaderBand = New PageHeaderBand() With {.HeightF = 20F}
            Dim reportFooter As ReportFooterBand = New ReportFooterBand() With {.HeightF = 380F}
            ' Place the bands onto a report
            rep.Bands.AddRange(New Band() {detail, pageHeader, reportFooter})
        End Sub
    End Class

    Public Class DataSourceDefinition

        Private fieldnameField As String

        Public Property Fieldname As String
            Get
                Return fieldnameField
            End Get

            Set(ByVal value As String)
                fieldnameField = value
            End Set
        End Property

        Private captionNameField As String

        Public Property CaptionName As String
            Get
                Return captionNameField
            End Get

            Set(ByVal value As String)
                captionNameField = value
            End Set
        End Property

        Private factorField As Integer

        Public Property Factor As Integer
            Get
                Return factorField
            End Get

            Set(ByVal value As Integer)
                factorField = value
            End Set
        End Property
    End Class
End Namespace

Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Reflection
Imports DevExpress.XtraReports.UI
Imports System.Collections.Generic

Namespace K18078
	Public Class ReportBuilderHelper
		Private dsd As List(Of DataSourceDefinition)
		Public Function GenerateReport(ByVal list As List(Of MyComplexObject)) As XtraReport
			Dim r As New XtraReport()
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
			Dim totalf As Integer=0
			For i As Integer = 0 To dsd.Count - 1
				totalf += dsd(i).Factor
			Next i
			Dim fWidth As Integer = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / totalf
			Dim incShift As Integer = 0
			For i As Integer = 0 To colCount - 1
				Dim labelh As XRLabel = CreateLabel(dsd(i), fWidth, incShift)
				labelh.Text = dsd(i).CaptionName

				Dim labeld As XRLabel = CreateLabel(dsd(i), fWidth, incShift)
				labeld.DataBindings.Add("Text", Nothing, dsd(i).Fieldname)

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
			Next i
			   End Sub

		Private Shared Function CreateLabel(ByVal dsd As DataSourceDefinition, ByVal fWidth As Integer, ByVal incShift As Integer) As XRLabel
			Dim labeld As New XRLabel()
			labeld.Location = New Point(incShift, 0)
			labeld.Size = New Size(fWidth * dsd.Factor, 20)
			Return labeld
		End Function

		Private Function GenerateDataSourceDefinition(ByVal myComplexObject As Object) As List(Of DataSourceDefinition)
			Dim dsdl As New List(Of DataSourceDefinition)()
			Dim pi() As PropertyInfo = myComplexObject.GetType().GetProperties()
			For i As Integer = 0 To pi.Length - 1
				Dim r() As Reportable = TryCast(pi(i).GetCustomAttributes(GetType(Reportable), False), Reportable())
				If r.Length > 0 Then
					Dim dsd As New DataSourceDefinition()
					If r(0).AlternateName Is Nothing Then
						dsd.CaptionName = pi(i).Name
					Else
						dsd.CaptionName = r(0).AlternateName
					End If
					dsd.Fieldname = pi(i).Name
					If r(0).LenFactor = 0 Then
						dsd.Factor = 1
					Else
						dsd.Factor = r(0).LenFactor
					End If
					dsdl.Add(dsd)
				End If
			Next i
			Return dsdl
		End Function
		Public Sub InitBands(ByVal rep As XtraReport)
			' Create bands
			Dim detail As New DetailBand()
			Dim pageHeader As New PageHeaderBand()
			Dim reportFooter As New ReportFooterBand()
			detail.Height = 20
			reportFooter.Height = 380
			pageHeader.Height = 20

			' Place the bands onto a report
			rep.Bands.AddRange(New DevExpress.XtraReports.UI.Band() { detail, pageHeader, reportFooter })
		End Sub
	End Class
	Public Class DataSourceDefinition
		Private fieldname_Renamed As String

		Public Property Fieldname() As String
			Get
				Return fieldname_Renamed
			End Get
			Set(ByVal value As String)
				fieldname_Renamed = value
			End Set
		End Property
		Private captionName_Renamed As String

		Public Property CaptionName() As String
			Get
				Return captionName_Renamed
			End Get
			Set(ByVal value As String)
				captionName_Renamed = value
			End Set
		End Property
		Private factor_Renamed As Integer

		Public Property Factor() As Integer
			Get
				Return factor_Renamed
			End Get
			Set(ByVal value As Integer)
				factor_Renamed = value
			End Set
		End Property

	End Class
End Namespace

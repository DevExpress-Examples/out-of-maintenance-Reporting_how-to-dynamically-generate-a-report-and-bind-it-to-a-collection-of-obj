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
			Dim totalf As Integer = 0
			For i As Integer = 0 To dsd.Count - 1
				totalf += dsd(i).Factor
			Next i
'INSTANT VB WARNING: Instant VB cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
			Dim fWidth As Integer = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / totalf
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
			Next i
		End Sub

		Private Shared Function CreateLabel(ByVal dsd As DataSourceDefinition, ByVal fWidth As Single, ByVal incShift As Single) As XRLabel
			Return New XRLabel() With {
				.LocationF = New PointF(incShift, 0),
				.SizeF = New SizeF(fWidth * dsd.Factor, 20)
			}
		End Function

		Private Function GenerateDataSourceDefinition(ByVal myComplexObject As Object) As List(Of DataSourceDefinition)
			Dim dsdl As New List(Of DataSourceDefinition)()
			Dim pi() As PropertyInfo = myComplexObject.GetType().GetProperties()
			For i As Integer = 0 To pi.Length - 1
				Dim r() As Reportable = TryCast(pi(i).GetCustomAttributes(GetType(Reportable), False), Reportable())
				If r.Length > 0 Then
					dsdl.Add(New DataSourceDefinition() With {
						.CaptionName = If(r(0).AlternateName Is Nothing, pi(i).Name, r(0).AlternateName),
						.Fieldname = pi(i).Name,
						.Factor = If(r(0).LenFactor = 0, 1, r(0).LenFactor)
					})
				End If
			Next i
			Return dsdl
		End Function
		Public Sub InitBands(ByVal rep As XtraReport)
			' Create bands
			Dim detail As New DetailBand() With {.HeightF = 20F}
			Dim pageHeader As New PageHeaderBand() With {.HeightF = 20F}
			Dim reportFooter As New ReportFooterBand() With {.HeightF = 380F}

			' Place the bands onto a report
			rep.Bands.AddRange(New DevExpress.XtraReports.UI.Band() { detail, pageHeader, reportFooter })
		End Sub
	End Class
	Public Class DataSourceDefinition
'INSTANT VB NOTE: The field fieldname was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private fieldname_Renamed As String

		Public Property Fieldname() As String
			Get
				Return fieldname_Renamed
			End Get
			Set(ByVal value As String)
				fieldname_Renamed = value
			End Set
		End Property
'INSTANT VB NOTE: The field captionName was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private captionName_Renamed As String

		Public Property CaptionName() As String
			Get
				Return captionName_Renamed
			End Get
			Set(ByVal value As String)
				captionName_Renamed = value
			End Set
		End Property
'INSTANT VB NOTE: The field factor was renamed since Visual Basic does not allow fields to have the same name as other class members:
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

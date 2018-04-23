Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports System.Collections.Generic

Namespace K18078
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim l As List(Of MyComplexObject) = GenerateRecords()
			Dim r As New XtraReport()
			r.Landscape = True
			Dim rbh As New ReportBuilderHelper()
			rbh.GenerateReport(r, l)
			r.ShowPreviewDialog()
		End Sub

		Private Function GenerateRecords() As List(Of MyComplexObject)
			Dim l As New List(Of MyComplexObject)()
			Dim o1 As New MyComplexObject()
			o1.Address = "507 - 20th Ave. E.Apt. 2A"
			o1.FirstName = "Nancy"
			o1.LastName = "Davolio"
			o1.City = "Seattle"
			o1.Age = 30
			o1.FederalId = "11-1123-12345"
			o1.Salary = 6000
			l.Add(o1)

			o1 = New MyComplexObject()
			o1.Address = "908 W. Capital Way"
			o1.FirstName = "Andrew"
			o1.LastName = "Fuller"
			o1.City = "Tacoma"
			o1.Age = 33
			o1.FederalId = "99-8888-11111"
			o1.Salary = 8000
			l.Add(o1)

			o1 = New MyComplexObject()
			o1.Address = "722 Moss Bay Blvd."
			o1.FirstName = "Janet"
			o1.LastName = "Leverling"
			o1.City = "Kirkland"
			o1.Age = 22
			o1.FederalId = "33-4444-55555"
			o1.Salary = 6000
			l.Add(o1)
			Return l
		End Function
	End Class
End Namespace
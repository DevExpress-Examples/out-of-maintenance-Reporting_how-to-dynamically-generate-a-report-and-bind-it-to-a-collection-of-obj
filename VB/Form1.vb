Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UI
Imports System.Collections.Generic

Namespace K18078

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim l As List(Of MyComplexObject) = GenerateRecords()
            Using r As XtraReport = New XtraReport()
                r.Landscape = True
                Dim rbh As ReportBuilderHelper = New ReportBuilderHelper()
                rbh.GenerateReport(r, l)
                r.ShowPreviewDialog()
            End Using
        End Sub

        Private Function GenerateRecords() As List(Of MyComplexObject)
            Return New List(Of MyComplexObject)() From {New MyComplexObject() With {.Address = "507 - 20th Ave. E.Apt. 2A", .FirstName = "Nancy", .LastName = "Davolio", .City = "Seattle", .Age = 30, .FederalId = "11-1123-12345", .Salary = 6000}, New MyComplexObject() With {.Address = "908 W. Capital Way", .FirstName = "Andrew", .LastName = "Fuller", .City = "Tacoma", .Age = 33, .FederalId = "99-8888-11111", .Salary = 8000}, New MyComplexObject() With {.Address = "722 Moss Bay Blvd.", .FirstName = "Janet", .LastName = "Leverling", .City = "Kirkland", .Age = 22, .FederalId = "33-4444-55555", .Salary = 6000}}
        End Function
    End Class
End Namespace

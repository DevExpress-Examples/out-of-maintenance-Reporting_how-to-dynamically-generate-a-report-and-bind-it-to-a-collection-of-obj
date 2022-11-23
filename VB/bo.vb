Imports System

Namespace K18078

    Public Class MyComplexObject

        Private ageField As Integer

        <Reportable(LenFactor:=1)>
        Public Property Age As Integer
            Get
                Return ageField
            End Get

            Set(ByVal value As Integer)
                ageField = value
            End Set
        End Property

        Private firstNameField As String

        '[Reportable(LenFactor = 2)]
        Public Property FirstName As String
            Get
                Return firstNameField
            End Get

            Set(ByVal value As String)
                firstNameField = value
            End Set
        End Property

        Private lastNameField As String

        '[Reportable(LenFactor = 2,AlternateName = "Surname")]
        Public Property LastName As String
            Get
                Return lastNameField
            End Get

            Set(ByVal value As String)
                lastNameField = value
            End Set
        End Property

        <Reportable(LenFactor:=3, AlternateName:="Full name")>
        Public ReadOnly Property Name As String
            Get
                Return firstNameField & " " & lastNameField
            End Get
        End Property

        Private addressField As String

        <Reportable(LenFactor:=3)>
        Public Property Address As String
            Get
                Return addressField
            End Get

            Set(ByVal value As String)
                addressField = value
            End Set
        End Property

        Private cityField As String

        <Reportable(LenFactor:=1)>
        Public Property City As String
            Get
                Return cityField
            End Get

            Set(ByVal value As String)
                cityField = value
            End Set
        End Property

        Private federalIdField As String

        <Reportable(AlternateName:="Social Security Number", LenFactor:=3)>
        Public Property FederalId As String
            Get
                Return federalIdField
            End Get

            Set(ByVal value As String)
                federalIdField = value
            End Set
        End Property

        Private salaryField As Double

        Public Property Salary As Double
            Get
                Return salaryField
            End Get

            Set(ByVal value As Double)
                salaryField = value
            End Set
        End Property
    End Class

    <AttributeUsage(AttributeTargets.All, AllowMultiple:=True)>
    Public Class Reportable
        Inherits Attribute

        Private altName As String

        Public Property AlternateName As String
            Get
                Return altName
            End Get

            Set(ByVal value As String)
                altName = value
            End Set
        End Property

        Private lenFactorField As Integer

        Public Property LenFactor As Integer
            Get
                Return lenFactorField
            End Get

            Set(ByVal value As Integer)
                lenFactorField = value
            End Set
        End Property
    End Class
End Namespace

Imports System.ComponentModel

Public Class DisplayNamePropertyDescriptor
    Inherits PropertyDescriptorBase

    Private baseProp As PropertyDescriptor

    Public Sub New(ByVal basePropertyDescriptor As PropertyDescriptor, ByVal filter As Attribute())
        MyBase.New(basePropertyDescriptor)

        baseProp = basePropertyDescriptor

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return baseProp.Name
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayName() As String
        Get
            Dim attr As Attribute

            attr = baseProp.Attributes.Item(GetType(DisplayNameAttribute))
            If Not attr Is Nothing Then

                Return CType(attr, DisplayNameAttribute).NameToDisplay

            End If

            Return baseProp.Name

        End Get
    End Property


    Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
        Return baseProp.CanResetValue(component)
    End Function

    Public Overrides ReadOnly Property ComponentType() As System.Type
        Get
            Return baseProp.ComponentType
        End Get
    End Property

    Public Overrides Function GetValue(ByVal component As Object) As Object
        Return baseProp.GetValue(component)
    End Function

    Public Overrides ReadOnly Property PropertyType() As System.Type
        Get
            Return baseProp.PropertyType
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)
        baseProp.ResetValue(component)
    End Sub

    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
        baseProp.SetValue(component, value)
    End Sub

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return baseProp.ShouldSerializeValue(component)
    End Function

End Class

Imports System

<AttributeUsage(AttributeTargets.Property, Inherited:=False)> Public Class DisplayNameAttribute
    Inherits Attribute

    Private m_NameToDisplay As String

    Public Sub New(ByVal Name As String)

        m_NameToDisplay = Name

    End Sub

    Public Property NameToDisplay() As String
        Get
            Return m_NameToDisplay
        End Get
        Set(ByVal Value As String)
            m_NameToDisplay = Value
        End Set
    End Property
End Class

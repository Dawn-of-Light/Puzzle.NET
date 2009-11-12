Imports System.ComponentModel

Public MustInherit Class PropertyDescriptorBase
    Inherits PropertyDescriptor

    Private m_ReadOnly As Boolean
    Private m_Descr As PropertyDescriptor

    Public Sub New(ByVal name As String, ByVal attrs() As Attribute)
        MyBase.New(name, attrs)

    End Sub

    Public Sub New(ByVal descr As PropertyDescriptor)
        MyBase.New(descr)
        m_Descr = descr

    End Sub

    Public Sub New(ByVal descr As PropertyDescriptor, ByVal attrs() As Attribute)
        MyBase.New(descr, attrs)
        m_Descr = descr

    End Sub


    Public Overrides ReadOnly Property IsReadOnly() As Boolean
        Get
            If m_ReadOnly Then
                Return True
            Else
                If Not m_Descr Is Nothing Then
                    Return m_Descr.IsReadOnly
                Else
                    Return False
                End If
            End If
        End Get
    End Property

    Public Overridable Sub SetReadOnly()
        m_ReadOnly = True
    End Sub
End Class

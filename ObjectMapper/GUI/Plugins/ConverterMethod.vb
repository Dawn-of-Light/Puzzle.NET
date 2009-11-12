Imports System.Reflection

Public Class ConverterMethod

    Private m_ConverterClass As ConverterClass

    Private m_MethodName As String
    Private m_MethodInfo As MethodInfo

    Public Property ConverterClass() As ConverterClass
        Get
            Return m_ConverterClass
        End Get
        Set(ByVal Value As ConverterClass)
            m_ConverterClass = Value
            m_ConverterClass.ConverterMethods.Add(Me)
        End Set
    End Property

    Public Property MethodName() As String
        Get
            Return m_MethodName
        End Get
        Set(ByVal Value As String)
            m_MethodName = Value
        End Set
    End Property

    Public Property MethodInfo() As MethodInfo
        Get
            Return m_MethodInfo
        End Get
        Set(ByVal Value As MethodInfo)
            m_MethodInfo = Value
        End Set
    End Property

    Public Function Convert(ByVal obj As Object) As Object

        Dim parameters() As Object
        Dim result As Object

        ReDim parameters(0)

        parameters(0) = obj

        result = MethodInfo.Invoke(ConverterClass.Converter, parameters)

        Return result

    End Function



End Class

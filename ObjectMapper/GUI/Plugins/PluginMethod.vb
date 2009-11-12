Imports System.Reflection

Public Class PluginMethod

    Private m_PluginAssembly As System.Reflection.Assembly

    Private m_AcceptsType As Type
    Private m_ReturnsType As Type
    Private m_Plugin As Object
    Private m_PluginName As String
    Private m_MethodName As String
    Private m_MethodInfo As MethodInfo

    Public Property PluginAssembly() As System.Reflection.Assembly
        Get
            Return m_PluginAssembly
        End Get
        Set(ByVal Value As System.Reflection.Assembly)
            m_PluginAssembly = Value
        End Set
    End Property

    Public Property AcceptsType() As Type
        Get
            Return m_AcceptsType
        End Get
        Set(ByVal Value As Type)
            m_AcceptsType = Value
        End Set
    End Property

    Public Property ReturnsType() As Type
        Get
            Return m_ReturnsType
        End Get
        Set(ByVal Value As Type)
            m_ReturnsType = Value
        End Set
    End Property

    Public Property Plugin() As Object
        Get
            Return m_Plugin
        End Get
        Set(ByVal Value As Object)
            m_Plugin = Value
        End Set
    End Property

    Public Property PluginName() As String
        Get
            Return m_PluginName
        End Get
        Set(ByVal Value As String)
            m_PluginName = Value
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

    Public Function Execute(ByVal obj As Object) As Object

        Dim parameters() As Object
        Dim result As Object

        If AcceptsType Is Nothing Then

            If Not obj Is Nothing Then

                Throw New Exception("Unmatching types!")

            End If

        Else


            ReDim parameters(0)

            parameters(0) = obj

        End If

        If ReturnsType Is Nothing Then

            MethodInfo.Invoke(Plugin, parameters)

        Else

            result = MethodInfo.Invoke(Plugin, parameters)

            Return result

        End If

    End Function

End Class

Public Class ConverterClass

    Private m_Name As String
    Private m_ConverterMethods As New ArrayList
    Private m_ConverterAssembly As System.Reflection.Assembly
    Private m_Converter As Object

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property

    Public Property ConverterMethods() As ArrayList
        Get
            Return m_ConverterMethods
        End Get
        Set(ByVal Value As ArrayList)
            m_ConverterMethods = Value
        End Set
    End Property

    Public Property ConverterAssembly() As System.Reflection.Assembly
        Get
            Return m_ConverterAssembly
        End Get
        Set(ByVal Value As System.Reflection.Assembly)
            m_ConverterAssembly = Value
        End Set
    End Property

    Public Property Converter()
        Get
            Return m_Converter
        End Get
        Set(ByVal Value)
            m_Converter = Value
        End Set
    End Property

End Class

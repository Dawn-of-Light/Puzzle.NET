Imports Puzzle.NPersist.Framework.Mapping

<AttributeUsage(AttributeTargets.Method, Inherited:=True)> Public Class PluginMethodAttribute
    Inherits Attribute

    Private m_AcceptsType As Type
    Private m_ReturnsType As Type
    Private m_DisplayName As String

    Public Sub New(ByVal acceptsType As Type)
        MyBase.New()

        SetAcceptsType(acceptsType)

    End Sub

    Public Sub New(ByVal acceptsType As Type, ByVal displayName As String)
        MyBase.New()

        SetAcceptsType(acceptsType)
        m_DisplayName = displayName

    End Sub


    Public Sub New(ByVal acceptsType As Type, ByVal returnsType As Type)
        MyBase.New()

        SetAcceptsType(acceptsType)

        m_ReturnsType = returnsType

    End Sub

    Public Sub New(ByVal acceptsType As Type, ByVal returnsType As Type, ByVal displayName As String)
        MyBase.New()

        SetAcceptsType(acceptsType)

        m_ReturnsType = returnsType
        m_DisplayName = displayName

    End Sub

    Public Property AcceptsType() As Type
        Get
            Return m_AcceptsType
        End Get
        Set(ByVal Value As Type)
            SetAcceptsType(Value)
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

    Public Property DisplayName() As String
        Get
            Return m_DisplayName
        End Get
        Set(ByVal Value As String)
            m_DisplayName = Value
        End Set
    End Property

    Protected Sub SetAcceptsType(ByVal value As Type)

        If value Is Nothing Then

            m_AcceptsType = value

        Else

            If Not value.GetInterface(GetType(IMap).ToString) Is Nothing Then

                m_AcceptsType = value

            Else

                Throw New Exception("Type not allowed: " & value.ToString & ". Only types for domain mapping objects implementing the Puzzle.NPersist.Framework.Mapping.IMap interface may be passed to the acceptsType parameter!")

            End If

        End If

    End Sub

End Class

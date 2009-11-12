Imports System.Drawing

Namespace Uml

    Public Class UmlColor

        Private m_A As Integer = 255
        Private m_R As Integer = 255
        Private m_G As Integer = 255
        Private m_B As Integer = 255

        Public Sub New()
            MyBase.New()

        End Sub

        Public Sub New(ByVal c As Color)
            MyBase.New()

            FromColor(c)

        End Sub

        Public Property A() As Integer
            Get
                Return m_A
            End Get
            Set(ByVal Value As Integer)
                m_A = Value
            End Set
        End Property

        Public Property R() As Integer
            Get
                Return m_R
            End Get
            Set(ByVal Value As Integer)
                m_R = Value
            End Set
        End Property

        Public Property G() As Integer
            Get
                Return m_G
            End Get
            Set(ByVal Value As Integer)
                m_G = Value
            End Set
        End Property

        Public Property B() As Integer
            Get
                Return m_B
            End Get
            Set(ByVal Value As Integer)
                m_B = Value
            End Set
        End Property

        Public Sub FromColor(ByVal c As Color)

            m_A = c.A
            m_R = c.R
            m_G = c.G
            m_B = c.B

        End Sub

        Public Function ToColor() As Color

            Return Color.FromArgb(m_A, m_R, m_G, m_B)

        End Function

    End Class

End Namespace


Namespace ProjectModel

    Public Class CustomMetaData

        Private m_targets As String = ""
        
        Public Property Targets() As String
            Get
                Return m_targets
            End Get
            Set(ByVal Value As String)
                m_targets = Value
            End Set
        End Property


        Private m_name As String = ""

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal Value As String)
                m_name = Value
            End Set
        End Property

        Private m_DisplayName As String = ""

        Public Property DisplayName() As String
            Get
                Return m_DisplayName
            End Get
            Set(ByVal Value As String)
                m_DisplayName = Value
            End Set
        End Property

        Private m_description As String = ""

        Public Property Description() As String
            Get
                Return m_description
            End Get
            Set(ByVal Value As String)
                m_description = Value
            End Set
        End Property

        Private m_category As String = ""

        Public Property Category() As String
            Get
                Return m_category
            End Get
            Set(ByVal Value As String)
                m_category = Value
            End Set
        End Property

        Private m_defaultValue As String = ""

        Public Property DefaultValue() As String
            Get
                Return m_defaultValue
            End Get
            Set(ByVal Value As String)
                m_defaultValue = Value
            End Set
        End Property

        Private m_type As String = "System.String"

        Public Property Type() As String
            Get
                Return m_type
            End Get
            Set(ByVal Value As String)
                m_type = Value
            End Set
        End Property

    End Class

End Namespace

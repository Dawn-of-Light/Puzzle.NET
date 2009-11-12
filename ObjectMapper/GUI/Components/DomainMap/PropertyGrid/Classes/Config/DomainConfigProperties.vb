Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.ComponentModel

Public Class DomainConfigProperties
    Inherits PropertiesBase

    Private m_DomainConfig As DomainConfig

    Public Event BeforePropertySet(ByVal mapObject As DomainConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As DomainConfig, ByVal propertyName As String)

    Public Function GetDomainConfig() As DomainConfig
        Return m_DomainConfig
    End Function

    Public Sub SetDomainConfig(ByVal value As DomainConfig)
        m_DomainConfig = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_DomainConfig

    End Function

    <Category("Design"), _
    Description("The name of this configuration."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Return m_DomainConfig.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainConfig.Name Then
                m_DomainConfig.Name = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "Name")
            End If
        End Set
    End Property


    <Category("Design"), _
    Description("The description of this configuration."), _
    DisplayName("Description"), _
    DefaultValue("")> Public Property Description() As String
        Get
            Return m_DomainConfig.Description
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainConfig.Description Then
                m_DomainConfig.Description = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "Description")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("If true then this is the configuration curently in use."), _
        DisplayName("Currently active configuration"), _
        DefaultValue(False)> Public ReadOnly Property IsActive() As Boolean
        Get
            Return m_DomainConfig.IsActive
        End Get
    End Property




    '<Category("From Model To Code"), _
    '    Description("If true then regions will be added to the generated code. Regions are required for code synchronization."), _
    '    DisplayName("Include regions"), _
    '    DefaultValue(True)> Public Property IncludeRegions() As Boolean
    '    Get
    '        Return m_DomainConfig.ClassesToCodeConfig.IncludeRegions
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_DomainConfig.ClassesToCodeConfig.IncludeRegions Then
    '            m_DomainConfig.ClassesToCodeConfig.IncludeRegions = Value
    '            RaiseEvent AfterPropertySet(m_DomainConfig, "IncludeRegions")
    '        End If
    '    End Set
    'End Property

    <Category("From Model To Code"), _
        Description("If true then comments will be included in the generated code"), _
        DisplayName("Include comments"), _
        DefaultValue(True)> Public Property IncludeComments() As Boolean
        Get
            Return m_DomainConfig.ClassesToCodeConfig.IncludeComments
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_DomainConfig.ClassesToCodeConfig.IncludeComments Then
                m_DomainConfig.ClassesToCodeConfig.IncludeComments = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "IncludeComments")
            End If
        End Set
    End Property


    <Category("From Model To Code"), _
        Description("If true then auto-documentation comments will be included in the generated code"), _
        DisplayName("Include autodoc comments"), _
        DefaultValue(True)> Public Property IncludeDocComments() As Boolean
        Get
            Return m_DomainConfig.ClassesToCodeConfig.IncludeDocComments
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_DomainConfig.ClassesToCodeConfig.IncludeDocComments Then
                m_DomainConfig.ClassesToCodeConfig.IncludeDocComments = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "IncludeComments")
            End If
        End Set
    End Property

    <Category("From Model To Code"), _
        Description("If true then information (including mapping information) from the xml model will be included in the auto-documentation comments."), _
        DisplayName("Include model info in autodoc comments"), _
        DefaultValue(True)> Public Property IncludeModelInfoInDocComments() As Boolean
        Get
            Return m_DomainConfig.ClassesToCodeConfig.IncludeModelInfoInDocComments
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_DomainConfig.ClassesToCodeConfig.IncludeModelInfoInDocComments Then
                m_DomainConfig.ClassesToCodeConfig.IncludeModelInfoInDocComments = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "IncludeModelInfoInDocComments")
            End If
        End Set
    End Property


    '<Category("From Model To Code"), _
    '    Description("If true then property access notification will only be implemented where strictly required by the framework. If you set to false, more generous property access notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that for full fledged interception 'Lightweight notification' should be set to false and 'After property get notification' and 'Before property get notification' should be set to true."), _
    '    DisplayName("Notify only when required"), _
    '    DefaultValue(True)> Public Property NotifyOnlyWhenRequired() As Boolean
    '    Get
    '        Return m_DomainConfig.ClassesToCodeConfig.NotifyOnlyWhenRequired
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_DomainConfig.ClassesToCodeConfig.NotifyOnlyWhenRequired Then
    '            m_DomainConfig.ClassesToCodeConfig.NotifyOnlyWhenRequired = Value
    '            RaiseEvent AfterPropertySet(m_DomainConfig, "NotifyOnlyWhenRequired")
    '        End If
    '    End Set
    'End Property

    '<Category("From Model To Code"), _
    '    Description("If true then property access notification will be implemented in a lightweight manner. If you set to false, more generous property access notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that for full fledged interception 'Notify only when required' should be set to false and 'After property get notification' and 'Before property get notification' should be set to true."), _
    '    DisplayName("Lightweight notification"), _
    '    DefaultValue(True)> Public Property NotifyLightWeight() As Boolean
    '    Get
    '        Return m_DomainConfig.ClassesToCodeConfig.NotifyLightWeight
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_DomainConfig.ClassesToCodeConfig.NotifyLightWeight Then
    '            m_DomainConfig.ClassesToCodeConfig.NotifyLightWeight = Value
    '            SetShouldReloadProperties()
    '            RaiseEvent AfterPropertySet(m_DomainConfig, "NotifyLightWeight")
    '        End If
    '    End Set
    'End Property

    '<Category("From Model To Code"), _
    '    Description("If true then full property get notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that if 'Lightweight notificatino' is set to true then the value of this setting will be ignored and considered false."), _
    '    DisplayName("After property get notification"), _
    '    DefaultValue(True)> Public Property NotifyAfterGet() As Boolean
    '    Get
    '        Return m_DomainConfig.ClassesToCodeConfig.NotifyAfterGet
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_DomainConfig.ClassesToCodeConfig.NotifyAfterGet Then
    '            m_DomainConfig.ClassesToCodeConfig.NotifyAfterGet = Value
    '            RaiseEvent AfterPropertySet(m_DomainConfig, "NotifyAfterGet")
    '        End If
    '    End Set
    'End Property

    '<Category("From Model To Code"), _
    '    Description("If true then full property set notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that if 'Lightweight notificatino' is set to true then the value of this setting will be ignored and considered false."), _
    '    DisplayName("After property set notification"), _
    '    DefaultValue(True)> Public Property NotifyAfterSet() As Boolean
    '    Get
    '        Return m_DomainConfig.ClassesToCodeConfig.NotifyAfterSet
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_DomainConfig.ClassesToCodeConfig.NotifyAfterSet Then
    '            m_DomainConfig.ClassesToCodeConfig.NotifyAfterSet = Value
    '            RaiseEvent AfterPropertySet(m_DomainConfig, "NotifyAfterSet")
    '        End If
    '    End Set
    'End Property

    <Category("From Model To Code"), _
        Description("The target language that the code for the domain map classes will be generated in."), _
        DisplayName("Target language"), _
        DefaultValue(True)> Public Property TargetLanguage() As TargetLanguageEnum
        Get
            Return m_DomainConfig.ClassesToCodeConfig.TargetLanguage
        End Get
        Set(ByVal Value As TargetLanguageEnum)
            If Value <> m_DomainConfig.ClassesToCodeConfig.TargetLanguage Then
                m_DomainConfig.ClassesToCodeConfig.TargetLanguage = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "TargetLanguage")
            End If
        End Set
    End Property

    <Category("From Model To Code"), _
        Description("The target Object/Relational Mapping framework that the generated source will be generated for. If you select POCO, only Plain Old CLR Object class files will be generated (but no meta data files with object/relational mapping information. This means that the POCO option is not targeted at any particular O/R Mapping framework)."), _
        DisplayName("Target mapper"), _
        DefaultValue(True)> Public Property TargetMapper() As TargetMapperEnum
        Get
            Return m_DomainConfig.ClassesToCodeConfig.TargetMapper
        End Get
        Set(ByVal Value As TargetMapperEnum)
            If Value <> m_DomainConfig.ClassesToCodeConfig.TargetMapper Then
                m_DomainConfig.ClassesToCodeConfig.TargetMapper = Value
                RaiseEvent AfterPropertySet(m_DomainConfig, "TargetMapper")
            End If
        End Set
    End Property


    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Select Case propDesc.Name

            Case "NotifyAfterGet"
                If m_DomainConfig.ClassesToCodeConfig.NotifyLightWeight Then propDesc.SetReadOnly()

            Case "NotifyAfterSet"
                If m_DomainConfig.ClassesToCodeConfig.NotifyLightWeight Then propDesc.SetReadOnly()

        End Select

        Return propDesc
    End Function

End Class


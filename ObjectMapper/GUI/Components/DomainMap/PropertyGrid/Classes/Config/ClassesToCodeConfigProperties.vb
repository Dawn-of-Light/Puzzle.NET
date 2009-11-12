Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms.Design

Public Class ClassesToCodeConfigProperties
    Inherits PropertiesBase

    Private m_ClassesToCodeConfig As ClassesToCodeConfig

    Public Event BeforePropertySet(ByVal mapObject As ClassesToCodeConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As ClassesToCodeConfig, ByVal propertyName As String)

    Public Function GetClassesToCodeConfig() As ClassesToCodeConfig
        Return m_ClassesToCodeConfig
    End Function

    Public Sub SetClassesToCodeConfig(ByVal value As ClassesToCodeConfig)
        m_ClassesToCodeConfig = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_ClassesToCodeConfig

    End Function

    '<Category("Update Code From Model"), _
    '    Description("If true then regions will be added to the generated code. Regions are required for code synchronization."), _
    '    DisplayName("Include regions"), _
    '    DefaultValue(True)> Public Property IncludeRegions() As Boolean
    '    Get
    '        Return m_ClassesToCodeConfig.IncludeRegions
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_ClassesToCodeConfig.IncludeRegions Then
    '            m_ClassesToCodeConfig.IncludeRegions = Value
    '            RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "IncludeRegions")
    '        End If
    '    End Set
    'End Property



    <Category("Interfaces"), _
        Description("If true then the IInterceptable interface will be implemented by your generated domain model classes."), _
        DisplayName("Implement IInterceptable"), _
        DefaultValue(True)> Public Property ImplementIInterceptable() As Boolean
        Get
            Return m_ClassesToCodeConfig.ImplementIInterceptable
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.ImplementIInterceptable Then
                m_ClassesToCodeConfig.ImplementIInterceptable = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "ImplementIInterceptable")
            End If
        End Set
    End Property

    <Category("Interfaces"), _
        Description("If true then the IObjectHelper interface will be implemented by your generated domain model classes. The IObjectHelper provides faster access to private field variables than offered by reflection. It can also be required if the reflection permissions have been set to disallow private field access."), _
        DisplayName("Implement IObjectHelper"), _
        DefaultValue(False)> Public Property ImplementIObjectHelper() As Boolean
        Get
            Return m_ClassesToCodeConfig.ImplementIObjectHelper
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.ImplementIObjectHelper Then
                m_ClassesToCodeConfig.ImplementIObjectHelper = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "ImplementIObjectHelper")
            End If
        End Set
    End Property

    '<Category("Interfaces"), _
    '    Description("If true then the IObservable interface will be implemented by your generated domain model classes. The IObservable interface will let you add Observers (objects implementing the IObserver interface) in order to apply behavior like logging, security etc to your objects dynamically."), _
    '    DisplayName("Implement IObservable"), _
    '    DefaultValue(False)> Public Property ImplementIObservable() As Boolean
    '    Get
    '        Return m_ClassesToCodeConfig.ImplementIObservable
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_ClassesToCodeConfig.ImplementIObservable Then
    '            m_ClassesToCodeConfig.ImplementIObservable = Value
    '            RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "ImplementIObservable")
    '        End If
    '    End Set
    'End Property

    <Category("Comments"), _
        Description("If true then comments will be included in the generated code"), _
        DisplayName("Include comments"), _
        DefaultValue(True)> Public Property IncludeComments() As Boolean
        Get
            Return m_ClassesToCodeConfig.IncludeComments
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.IncludeComments Then
                m_ClassesToCodeConfig.IncludeComments = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "IncludeComments")
            End If
        End Set
    End Property

    <Category("Comments"), _
        Description("If true then auto-documentation comments will be included in the generated code"), _
        DisplayName("Include autodoc comments"), _
        DefaultValue(True)> Public Property IncludeDocComments() As Boolean
        Get
            Return m_ClassesToCodeConfig.IncludeDocComments
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.IncludeDocComments Then
                m_ClassesToCodeConfig.IncludeDocComments = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "IncludeComments")
            End If
        End Set
    End Property

    <Category("Comments"), _
        Description("If true then information (including mapping information) from the xml model will be included in the auto-documentation comments."), _
        DisplayName("Include model info in autodoc comments"), _
        DefaultValue(True)> Public Property IncludeModelInfoInDocComments() As Boolean
        Get
            Return m_ClassesToCodeConfig.IncludeModelInfoInDocComments
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.IncludeModelInfoInDocComments Then
                m_ClassesToCodeConfig.IncludeModelInfoInDocComments = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "IncludeModelInfoInDocComments")
            End If
        End Set
    End Property

    <Category("Property Access Notification"), _
        Description("If true then property access notification will only be implemented where strictly required by the framework. If you set to false, more generous property access notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that for full fledged interception 'Lightweight notification' should be set to false and 'After property get notification' and 'Before property get notification' should be set to true."), _
        DisplayName("Notify only when required"), _
        DefaultValue(True)> Public Property NotifyOnlyWhenRequired() As Boolean
        Get
            Return m_ClassesToCodeConfig.NotifyOnlyWhenRequired
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.NotifyOnlyWhenRequired Then
                m_ClassesToCodeConfig.NotifyOnlyWhenRequired = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "NotifyOnlyWhenRequired")
            End If
        End Set
    End Property

    <Category("Property Access Notification"), _
        Description("If true then property access notification will be implemented in a lightweight manner. If you set to false, more generous property access notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that for full fledged interception 'Notify only when required' should be set to false and 'After property get notification' and 'Before property get notification' should be set to true."), _
        DisplayName("Lightweight notification"), _
        DefaultValue(True)> Public Property NotifyLightWeight() As Boolean
        Get
            Return m_ClassesToCodeConfig.NotifyLightWeight
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.NotifyLightWeight Then
                m_ClassesToCodeConfig.NotifyLightWeight = Value
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "NotifyLightWeight")
            End If
        End Set
    End Property

    <Category("Property Access Notification"), _
        Description("If true then full property get notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that if 'Lightweight notificatino' is set to true then the value of this setting will be ignored and considered false."), _
        DisplayName("After property get notification"), _
        DefaultValue(True)> Public Property NotifyAfterGet() As Boolean
        Get
            Return m_ClassesToCodeConfig.NotifyAfterGet
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.NotifyAfterGet Then
                m_ClassesToCodeConfig.NotifyAfterGet = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "NotifyAfterGet")
            End If
        End Set
    End Property

    <Category("Property Access Notification"), _
        Description("If true then full property set notification will be implemented allowing for sophisticated interception based security and logging schemes etc. Note that if 'Lightweight notificatino' is set to true then the value of this setting will be ignored and considered false."), _
        DisplayName("After property set notification"), _
        DefaultValue(True)> Public Property NotifyAfterSet() As Boolean
        Get
            Return m_ClassesToCodeConfig.NotifyAfterSet
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.NotifyAfterSet Then
                m_ClassesToCodeConfig.NotifyAfterSet = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "NotifyAfterSet")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
    Description("If true then the shadow properties appearing in the model will be implemented in the generated code as overriding properties. Normally the reason to declare shadowing properties in the model is that there is a need to modify (override) the property mapping settings for the property of a subclass, and there is no need to actually override (shadow) the property in the generated code."), _
    DisplayName("Implement shadow properties"), _
    DefaultValue(True)> Public Property ImplementShadows() As Boolean
        Get
            Return m_ClassesToCodeConfig.ImplementShadows
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.ImplementShadows Then
                m_ClassesToCodeConfig.ImplementShadows = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "ImplementShadows")
            End If
        End Set
    End Property


    <Category("Property Access Notification"), _
    Description("If true then a refactoring will be used in the generated code where protected methods will be generated encapsulating the property notification calls, making the property notification code in the property accessors shorter."), _
    DisplayName("Add property notification methods"), _
    DefaultValue(False)> Public Property AddPropertyNotifyMethods() As Boolean
        Get
            Return m_ClassesToCodeConfig.AddPropertyNotifyMethods
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.AddPropertyNotifyMethods Then
                m_ClassesToCodeConfig.AddPropertyNotifyMethods = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "AddPropertyNotifyMethods")
            End If
        End Set
    End Property

    <Category("POCO"), _
    Description("If true then Plain Old CLR Objects (POCO) will be generated. Note that NPersist doesn't support POCO persistence, but that you may want to generate POCO objects for use with other persistence frameworks that do support POCO (such as NHibernate)."), _
    DisplayName("POCO"), _
    DefaultValue(False)> Public Property GeneratePOCO() As Boolean
        Get
            Return m_ClassesToCodeConfig.GeneratePOCO
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.GeneratePOCO Then
                m_ClassesToCodeConfig.GeneratePOCO = Value
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "GeneratePOCO")
            End If
        End Set
    End Property

    <Category("Target"), _
        Description("The target language that the code for the domain map classes will be generated in."), _
        DisplayName("Target language"), _
        DefaultValue(True)> Public Property TargetLanguage() As TargetLanguageEnum
        Get
            Return m_ClassesToCodeConfig.TargetLanguage
        End Get
        Set(ByVal Value As TargetLanguageEnum)
            If Value <> m_ClassesToCodeConfig.TargetLanguage Then
                m_ClassesToCodeConfig.TargetLanguage = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "TargetLanguage")
            End If
        End Set
    End Property

    <Category("Target"), _
        Description("The target Object/Relational Mapping framework that the generated source will be generated for. If you select POCO, only Plain Old CLR Object class files will be generated (but no meta data files with object/relational mapping information. This means that the POCO option is not targeted at any particular O/R Mapping framework)."), _
        DisplayName("Target mapper"), _
        DefaultValue(True)> Public Property TargetMapper() As TargetMapperEnum
        Get
            Return m_ClassesToCodeConfig.TargetMapper
        End Get
        Set(ByVal Value As TargetMapperEnum)
            If Value <> m_ClassesToCodeConfig.TargetMapper Then
                m_ClassesToCodeConfig.TargetMapper = Value
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "TargetMapper")
            End If
        End Set
    End Property


    <Editor(GetType(FolderNameEditor), GetType(System.Drawing.Design.UITypeEditor)), _
        Category("Target"), _
        Description("The path to the folder where generated files will be placed."), _
        DisplayName("Folder"), _
        DefaultValue("")> Public Property TargetFolder() As String
        Get
            Return m_ClassesToCodeConfig.TargetFolder
        End Get
        Set(ByVal Value As String)
            If Value <> m_ClassesToCodeConfig.TargetFolder Then
                m_ClassesToCodeConfig.TargetFolder = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "TargetFolder")
            End If
        End Set
    End Property

    <Category("Regions"), _
    Description("Set to true if you don't want any regions to be generated. Note that without a 'custom code region' in which you write your custom code, ObjectMapper will not be able to keep your custom code intact during a resynchronization."), _
    DisplayName("No regions"), _
    DefaultValue(False)> Public Property ReallyNoRegions() As Boolean
        Get
            Return m_ClassesToCodeConfig.ReallyNoRegions
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.ReallyNoRegions Then
                m_ClassesToCodeConfig.ReallyNoRegions = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "ReallyNoRegions")
            End If
        End Set
    End Property


    <Category("Collections"), _
    Description("Set to true if you want to generate typed collections."), _
    DisplayName("Typed collections"), _
    DefaultValue(False)> Public Property UseTypedCollections() As Boolean
        Get
            Return m_ClassesToCodeConfig.UseTypedCollections
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.UseTypedCollections Then
                m_ClassesToCodeConfig.UseTypedCollections = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "UseTypedCollections")
            End If
        End Set
    End Property

    <Category("Collections"), _
    Description("Set to true if you want to generate collections that are typed using generics."), _
    DisplayName("Generic collections"), _
    DefaultValue(True)> Public Property UseGenericCollections() As Boolean
        Get
            Return m_ClassesToCodeConfig.UseGenericCollections
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.UseGenericCollections Then
                m_ClassesToCodeConfig.UseGenericCollections = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "UseGenericCollections")
            End If
        End Set
    End Property

    '<Category("Collections"), _
    'Description("Set to true if you want to wrap collections properties with methods."), _
    'DisplayName("Wrap collections"), _
    'DefaultValue(False)> Public Property WrapCollections() As Boolean
    '    Get
    '        Return m_ClassesToCodeConfig.WrapCollections
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If Value <> m_ClassesToCodeConfig.WrapCollections Then
    '            m_ClassesToCodeConfig.WrapCollections = Value
    '            RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "WrapCollections")
    '        End If
    '    End Set
    'End Property

    <Category("Xml"), _
    Description("Set to true if you want to include generated xml files in the generated project as embedded resources."), _
    DisplayName("Embed xml"), _
    DefaultValue(False)> Public Property EmbedXml() As Boolean
        Get
            Return m_ClassesToCodeConfig.EmbedXml
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.EmbedXml Then
                m_ClassesToCodeConfig.EmbedXml = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "EmbedXml")
            End If
        End Set
    End Property


    <Category("Xml"), _
    Description("Set to true if you want to generate a separate xml mapping file for each root class in your domain. Applies only to NHibernate."), _
    DisplayName("Xml file per root class"), _
    DefaultValue(False)> Public Property XmlFilePerClass() As Boolean
        Get
            Return m_ClassesToCodeConfig.XmlFilePerClass
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_ClassesToCodeConfig.XmlFilePerClass Then
                m_ClassesToCodeConfig.XmlFilePerClass = Value
                RaiseEvent AfterPropertySet(m_ClassesToCodeConfig, "XmlFilePerClass")
            End If
        End Set
    End Property



    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Select Case propDesc.Name

            Case "NotifyAfterGet"
                If m_ClassesToCodeConfig.GeneratePOCO Then propDesc.SetReadOnly()
                If m_ClassesToCodeConfig.NotifyLightWeight Then propDesc.SetReadOnly()

            Case "NotifyAfterSet"
                If m_ClassesToCodeConfig.GeneratePOCO Then propDesc.SetReadOnly()
                If m_ClassesToCodeConfig.NotifyLightWeight Then propDesc.SetReadOnly()

            Case "AddPropertyNotifyMethods", "ImplementIInterceptable", "ImplementIObjectHelper", "ImplementIObservable", "NotifyLightWeight", "NotifyOnlyWhenRequired"
                If m_ClassesToCodeConfig.GeneratePOCO Then propDesc.SetReadOnly()

            Case "GeneratePOCO"
                If m_ClassesToCodeConfig.TargetMapper = TargetMapperEnum.NHibernate Then propDesc.SetReadOnly()
                If m_ClassesToCodeConfig.TargetMapper = TargetMapperEnum.POCO Then propDesc.SetReadOnly()

        End Select

        Return propDesc
    End Function

End Class


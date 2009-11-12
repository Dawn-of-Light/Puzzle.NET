Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.ComponentModel


Public Enum NhDefaultCascadeEnum

    Unspecified = 0
    None = 1
    SaveUpdate = 2

End Enum

Public Enum NullableBool

    Unspecified = 0
    [False] = 1
    [True] = 2

End Enum

Public Enum NullableBoolWithAuto

    Unspecified = 0
    [False] = 1
    [True] = 2
    [Auto] = 3

End Enum


Public Class DomainProperties
    Inherits PropertiesBase

    Private m_DomainMap As IDomainMap
    Private m_FilePath As String

    Public Event BeforePropertySet(ByVal mapObject As IDomainMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As IDomainMap, ByVal propertyName As String)

    Public Function GetDomainMap() As IDomainMap
        Return m_DomainMap
    End Function

    Public Sub SetDomainMap(ByVal value As IDomainMap)
        m_DomainMap = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_DomainMap

    End Function

    Public Sub SetFilePath(ByVal filePath As String)
        m_FilePath = filePath
    End Sub

    <Category("Design"), _
    Description("The name of this domain model."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Return m_DomainMap.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainMap.Name Then
                m_DomainMap.Name = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "Name")
            End If
        End Set
    End Property

    <Category("Design"), _
    Description("The name for the assembly that the classes in this domain belong to per default. You can override this setting in individual classes by specifying an assembly name for them. If no assembly name is specified, the name you have given your domain will be used."), _
    DisplayName("Assembly name"), _
    DefaultValue("")> Public Property AssemblyName() As String
        Get
            Return m_DomainMap.AssemblyName
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainMap.AssemblyName Then
                m_DomainMap.AssemblyName = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "AssemblyName")
            End If
        End Set
    End Property

    <Category("Code Generation"), _
    Description("The code language that will be used when you dynamically generate an assembly from your model in order to run it."), _
    DisplayName("Code language"), _
    DefaultValue(CodeLanguage.CSharp)> Public Property TheCodeLanguage() As CodeLanguage
        Get
            Return m_DomainMap.CodeLanguage
        End Get
        Set(ByVal Value As CodeLanguage)
            If Value <> m_DomainMap.CodeLanguage Then
                m_DomainMap.CodeLanguage = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "CodeLanguage")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("Set to True for read-only access to all classes in this domain. If True, objects from this domain can only be fetched, not created, updated or deleted."), _
        DisplayName("Read only"), _
        DefaultValue(False)> Public Property IsReadOnly() As Boolean
        Get
            Return m_DomainMap.IsReadOnly
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_DomainMap.IsReadOnly Then
                m_DomainMap.IsReadOnly = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "IsReadOnly")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("The root namespace for all the classes in this domain model. Leave empty if all classes do not share a common root namespace."), _
        DisplayName("Root namespace"), _
        DefaultValue("")> Public Property RootNamespace() As String
        Get
            Return m_DomainMap.RootNamespace
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainMap.RootNamespace Then
                m_DomainMap.RootNamespace = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "RootNamespace")
            End If
        End Set
    End Property


    <Category("Caching"), _
        Description("The value in seconds that objects may stay cached before going stale and in need of a reload. The default value of -1 means the time to live value of the NPersist context will be used. If that value is also -1 (default) it means there is no limit."), _
        DisplayName("Time to live"), _
        DefaultValue(-1)> Public Property TimeToLive() As Integer
        Get
            Return m_DomainMap.TimeToLive
        End Get
        Set(ByVal Value As Integer)
            If Value <> m_DomainMap.TimeToLive Then
                m_DomainMap.TimeToLive = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "TimeToLive")
            End If
        End Set
    End Property

    <Category("Caching"), _
        Description("The behavior specifying how objects may stay cached before going stale (according to the timespan specified in time to live) and in need of a reload. The default value of means the time to live behavior of the NPersist context will be used. If that value is also default it translates to the time to live behavior 'On'."), _
        DisplayName("Time to live behavior"), _
        DefaultValue(TimeToLiveBehavior.Default)> Public Property TheTimeToLiveBehavior() As TimeToLiveBehavior
        Get
            Return m_DomainMap.TimeToLiveBehavior
        End Get
        Set(ByVal Value As TimeToLiveBehavior)
            If Value <> m_DomainMap.TimeToLiveBehavior Then
                m_DomainMap.TimeToLiveBehavior = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "TimeToLiveBehavior")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("This behavior specifies how objects in this domain are loaded when you load objects by identity. Lazy means that the object will not be filled with values until you read a non-identity property of the object, while eager means the object will be loaded with values from the database right away. The default value of means the loading behavior of the NPersist context will be used."), _
        DisplayName("Loading behavior"), _
        DefaultValue(LoadBehavior.Default)> Public Property TheLoadBehavior() As LoadBehavior
        Get
            Return m_DomainMap.LoadBehavior
        End Get
        Set(ByVal Value As LoadBehavior)
            If Value <> m_DomainMap.LoadBehavior Then
                m_DomainMap.LoadBehavior = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "LoadBehavior")
            End If
        End Set
    End Property

    <Category("Field"), _
        Description("The prefix for the fields that hold the values for the properties in this domain. The prefix will be added before the name of the property to generate a name for the field if no field name is explicitly given."), _
        DisplayName("Default field prefix"), _
        DefaultValue("")> Public Property FieldPrefix() As String
        Get
            Return m_DomainMap.FieldPrefix
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainMap.FieldPrefix Then
                m_DomainMap.FieldPrefix = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "FieldPrefix")
            End If
        End Set
    End Property

    <Category("Field"), _
        Description("The naming strategy applied to the fields that hold the values for the properties in this domain. The strategy will be applied to the name of the property to generate a name for the field if no field name is explicitly given."), _
        DisplayName("Field naming strategy"), _
        DefaultValue(FieldNameStrategyType.None)> Public Property FieldNameStrategy() As FieldNameStrategyType
        Get
            Return m_DomainMap.FieldNameStrategy
        End Get
        Set(ByVal Value As FieldNameStrategyType)
            If Value <> m_DomainMap.FieldNameStrategy Then
                m_DomainMap.FieldNameStrategy = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "FieldNameStrategy")
            End If
        End Set
    End Property

    <Category("Mapping"), _
        TypeConverter(GetType(SourceConverter)), _
        Description("The name of the data source that classes in this domain will be mapping to when nothing else is specified."), _
        DisplayName("Default source name"), _
        DefaultValue("")> Public Property Source() As String
        Get
            Return m_DomainMap.Source
        End Get
        Set(ByVal Value As String)
            If Value <> m_DomainMap.Source Then
                m_DomainMap.Source = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "Source")
            End If
        End Set
    End Property

    <Category("Merging"), _
    Description("The default merging behavior for classes in this domain model. DefaultBehavior means that the merging behavior of the persistence manager will be used."), _
    DisplayName("Merging behavior"), _
    DefaultValue(MergeBehaviorType.DefaultBehavior)> Public Property MergeBehavior() As MergeBehaviorType
        Get
            Return m_DomainMap.MergeBehavior
        End Get
        Set(ByVal Value As MergeBehaviorType)
            If Value <> m_DomainMap.MergeBehavior Then
                m_DomainMap.MergeBehavior = Value
                RaiseEvent AfterPropertySet(m_DomainMap, "MergeBehavior")
            End If
        End Set
    End Property

    <Category("File"), _
        Description("The name of the file contaning the metadata and object/relational mapping information about the domain model."), _
        DisplayName("File path"), _
        DefaultValue("")> Public ReadOnly Property FilePath() As String
        Get
            Return m_FilePath
        End Get

    End Property



    <Category("Code Generation"), _
        Description("By specifying the name of a transient class (that is, not a persistent domain class) here, all classes in the domain that don't inherit from a domain class and that don't have any other super class specified in their 'Inherits transient class' attributes will inherit from this common base class. Only used for code generation."), _
        DisplayName("Inherits transient class"), _
        DefaultValue("")> Public Property InheritsTransientClass() As String
        Get
            Return m_DomainMap.InheritsTransientClass
        End Get
        Set(ByVal Value As String)
            m_DomainMap.InheritsTransientClass = Value
            RaiseEvent AfterPropertySet(m_DomainMap, "InheritsTransientClass")
        End Set
    End Property

    <Category("Code Generation"), _
        Description("Using this property you can let the code generator know which interfaces all the classes in the domain should implement. Only used for code generation."), _
        DisplayName("Implements interfaces")> Public Property ImplementsInterfaces() As String()
        Get
            Return m_DomainMap.ImplementsInterfaces.ToArray(GetType(String))
        End Get
        Set(ByVal Value As String())
            m_DomainMap.ImplementsInterfaces = New ArrayList(Value)
            RaiseEvent AfterPropertySet(m_DomainMap, "ImplementsInterfaces")
        End Set
    End Property


    <Category("Code Generation"), _
        Description("Using this property you can let the code generator know which namespaces all the classes in the domain should import. Only used for code generation."), _
        DisplayName("Imports namespaces")> Public Property ImportsNamespaces() As String()
        Get
            Return m_DomainMap.ImportsNamespaces.ToArray(GetType(String))
        End Get
        Set(ByVal Value As String())
            m_DomainMap.ImportsNamespaces = New ArrayList(Value)
            RaiseEvent AfterPropertySet(m_DomainMap, "ImportsNamespaces")
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to the properties of the classes in this domain when the objects are updated. If a property's update optimistic concurrency behavior is set to DefaultBehavior then the behavior will be inherited from the class map, then from this setting in the domain map and finally from the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting update behavior for the property will become IncludeWhenDirty except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Update optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property UpdateOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Return m_DomainMap.UpdateOptimisticConcurrencyBehavior
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            m_DomainMap.UpdateOptimisticConcurrencyBehavior = Value
            RaiseEvent AfterPropertySet(m_DomainMap, "UpdateOptimisticConcurrencyBehavior")
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to the properties of the classes in this domain when the objects are deleted. If a property's delete optimistic concurrency behavior is set to DefaultBehavior then the behavior will be inherited from the class map, then from this setting in the domain map and finally from the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting delete behavior for the property will become IncludeWhenLoaded except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Delete optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property DeleteOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Return m_DomainMap.DeleteOptimisticConcurrencyBehavior
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            m_DomainMap.DeleteOptimisticConcurrencyBehavior = Value
            RaiseEvent AfterPropertySet(m_DomainMap, "DeleteOptimisticConcurrencyBehavior")
        End Set
    End Property


    '<Category("Target Langauges"), _
    '    Description("By specifying that C# is one of your target languages, your model will be verified against a list of reserved words in this language. ObjectMapper will set this property automatically based on what target language settings you have in your tool configurations."), _
    '    DisplayName("C#"), _
    '    DefaultValue(False)> Public Property VerifyCSharpReservedWords() As Boolean
    '    Get
    '        Return m_DomainMap.VerifyCSharpReservedWords
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        m_DomainMap.VerifyCSharpReservedWords = Value
    '        RaiseEvent AfterPropertySet(m_DomainMap, "VerifyCSharpReservedWords")
    '    End Set
    'End Property

    '<Category("Target Langauges"), _
    '    Description("By specifying that VB.NET is one of your target languages, your model will be verified against a list of reserved words in this language. ObjectMapper will set this property automatically based on what target language settings you have in your tool configurations."), _
    '    DisplayName("VB.NET"), _
    '    DefaultValue(False)> Public Property VerifyVbReservedWords() As Boolean
    '    Get
    '        Return m_DomainMap.VerifyVbReservedWords
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        m_DomainMap.VerifyVbReservedWords = Value
    '        RaiseEvent AfterPropertySet(m_DomainMap, "VerifyVbReservedWords")
    '    End Set
    'End Property

    '<Category("Target Langauges"), _
    '    Description("By specifying that Delphi for .NET is one of your target languages, your model will be verified against a list of reserved words in this language. ObjectMapper will set this property automatically based on what target language settings you have in your tool configurations."), _
    '    DisplayName("Delphi for .NET"), _
    '    DefaultValue(False)> Public Property VerifyDelphiReservedWords() As Boolean
    '    Get
    '        Return m_DomainMap.VerifyDelphiReservedWords
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        m_DomainMap.VerifyDelphiReservedWords = Value
    '        RaiseEvent AfterPropertySet(m_DomainMap, "VerifyDelphiReservedWords")
    '    End Set
    'End Property

    <Category("Serializer"), _
        Description("The serializer you select will be used for serializing and deserializing your domain model to and from XML. Different serializers support different XML schemas."), _
        DisplayName("Serializer"), _
        DefaultValue(False)> Public Property MapSerializer() As MapSerializer
        Get
            Return m_DomainMap.MapSerializer
        End Get
        Set(ByVal Value As MapSerializer)
            m_DomainMap.MapSerializer = Value
            RaiseEvent AfterPropertySet(m_DomainMap, "MapSerializer")
        End Set
    End Property

    '------------------------------------
    'NHibernate
    '------------------------------------

    <Category("NHibernate"), _
        Description("The default cascade style. (NHibernate specific)."), _
        DisplayName("NH default-cascade"), _
        DefaultValue(NhDefaultCascadeEnum.Unspecified)> Public Property NhDefaultCascade() As NhDefaultCascadeEnum
        Get
            Dim meta As String
            Dim key As String = "nh-default-cascade"
            meta = m_DomainMap.GetMetaData(key)
            Select Case LCase(meta)
                Case "none"
                    Return NhDefaultCascadeEnum.None
                Case "save-update"
                    Return NhDefaultCascadeEnum.SaveUpdate
                Case Else
                    Return NhDefaultCascadeEnum.Unspecified
            End Select
        End Get
        Set(ByVal Value As NhDefaultCascadeEnum)
            Dim key As String = "nh-default-cascade"
            Select Case Value
                Case NhDefaultCascadeEnum.Unspecified
                    If m_DomainMap.HasMetaData(key) Then
                        m_DomainMap.RemoveMetaData(key)
                    End If

                Case NhDefaultCascadeEnum.None
                    m_DomainMap.SetMetaData(key, "none")

                Case NhDefaultCascadeEnum.SaveUpdate
                    m_DomainMap.SetMetaData(key, "save-update")
            End Select
            RaiseEvent AfterPropertySet(m_DomainMap, "NhDefaultCascade")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether unqualified class names can be used in the query language. (NHibernate specific)."), _
        DisplayName("NH auto-import"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhAutoImport() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-auto-import"
            meta = m_DomainMap.GetMetaData(key)
            Select Case LCase(meta)
                Case "true"
                    Return NullableBool.True
                Case "false"
                    Return NullableBool.False
                Case Else
                    Return NullableBool.Unspecified
            End Select
        End Get
        Set(ByVal Value As NullableBool)
            Dim key As String = "nh-auto-import"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_DomainMap.HasMetaData(key) Then
                        m_DomainMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_DomainMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_DomainMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_DomainMap, "NhAutoImport")
        End Set
    End Property





    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Select Case propDesc.Name

            Case "VerifyCSharpReservedWords", "VerifyVbReservedWords", "VerifyDelphiReservedWords"
                propDesc.SetReadOnly()

        End Select

        Return propDesc
    End Function



End Class

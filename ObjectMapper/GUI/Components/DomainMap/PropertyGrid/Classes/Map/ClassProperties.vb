Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.ComponentModel

Public Enum NhPolymorphismEnum
    Unspecified = 0
    Implicit = 1
    Explicit = 2
End Enum

Public Class ClassProperties
    Inherits PropertiesBase

    Private m_ClassMap As IClassMap

    Public Function GetClassMap() As IClassMap
        Return m_ClassMap
    End Function

    Public Sub SetClassMap(ByVal value As IClassMap)
        m_ClassMap = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_ClassMap

    End Function


    Public Event BeforePropertySet(ByVal mapObject As IClassMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As IClassMap, ByVal propertyName As String)

    <Category("Design"), _
        Description("The name of this class."), _
        DisplayName("Name"), _
        DefaultValue("")> Public Property Name() As String
        Get
            Dim arrName() As String = Split(m_ClassMap.Name, ".")
            Return arrName(UBound(arrName))
        End Get
        Set(ByVal Value As String)
            Dim arrName() As String = Split(m_ClassMap.Name, ".")
            Dim name As String
            arrName(UBound(arrName)) = Value
            name = Join(arrName, ".")
            If name <> m_ClassMap.Name Then
                RaiseEvent BeforePropertySet(m_ClassMap, "Name", name, m_ClassMap.Name)
                m_ClassMap.UpdateName(name)
                RaiseEvent AfterPropertySet(m_ClassMap, "Name")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("The namepace that this class belongs to."), _
        DisplayName("Namespace"), _
        DefaultValue("")> Public Property NameSpaceName() As String
        Get
            Dim arrName() As String = Split(m_ClassMap.Name, ".")
            If UBound(arrName) > 0 Then
                ReDim Preserve arrName(UBound(arrName) - 1)
                Return Join(arrName, ".")
            End If
        End Get
        Set(ByVal Value As String)
            Dim arrName() As String = Split(m_ClassMap.Name, ".")
            Dim name As String = arrName(UBound(arrName))
            If Len(Value) > 0 Then
                name = Value & "." & name
            End If
            If name <> m_ClassMap.Name Then
                RaiseEvent BeforePropertySet(m_ClassMap, "Namespace", name, m_ClassMap.Name)
                m_ClassMap.UpdateName(name)
                RaiseEvent AfterPropertySet(m_ClassMap, "Namespace")
            End If
        End Set
    End Property


    <Category("Design"), _
    Description("The name of the assembly that this class belongs to. If no setting is specified, the default setting specified in the 'Assembly name' property for the domain will be used."), _
    DisplayName("Assembly name"), _
    DefaultValue("")> Public Property AssemblyName() As String
        Get
            Return m_ClassMap.AssemblyName
        End Get
        Set(ByVal Value As String)
            If Value <> m_ClassMap.AssemblyName Then
                m_ClassMap.AssemblyName = Value
                RaiseEvent AfterPropertySet(m_ClassMap, "AssemblyName")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("Define a load span for this class. A load span corresponds to the select clause of an NPath query. Leave empty for default load span '*'."), _
        DisplayName("Load span"), _
        DefaultValue("")> Public Property LoadSpan() As String
        Get
            Return m_ClassMap.LoadSpan
        End Get
        Set(ByVal Value As String)
            If Value <> m_ClassMap.LoadSpan Then
                m_ClassMap.LoadSpan = Value
                RaiseEvent AfterPropertySet(m_ClassMap, "LoadSpan")
            End If
        End Set
    End Property

    '<Category("Design"), _
    'Description("The name of the validation method on this class. (Must be a method that returns void and takes no parameters or one IList parameter with exceptions)."), _
    'DisplayName("Type"), _
    'DefaultValue(ClassType.Default)> Public Property TheClassType() As ClassType
    '    Get
    '        Return m_ClassMap.ClassType
    '    End Get
    '    Set(ByVal Value As ClassType)
    '        If Value <> m_ClassMap.ClassType Then
    '            m_ClassMap.ClassType = Value
    '            RaiseEvent AfterPropertySet(m_ClassMap, "ClassType")
    '        End If
    '    End Set
    'End Property

    <Category("Validation"), _
    Description("The name of the validation method on this class. (Must be a method that returns void and takes no parameters or one IList parameter with exceptions)."), _
    DisplayName("Validate method"), _
    DefaultValue("")> Public Property ValidateMethod() As String
        Get
            Return m_ClassMap.ValidateMethod
        End Get
        Set(ByVal Value As String)
            If Value <> m_ClassMap.ValidateMethod Then
                m_ClassMap.ValidateMethod = Value
                RaiseEvent AfterPropertySet(m_ClassMap, "ValidateMethod")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("Set to True for read-only access to this class. If True, objects from this class can only be fetched, not created, updated or deleted."), _
        DisplayName("Read only"), _
        DefaultValue(False)> Public Property IsReadOnly() As Boolean
        Get
            Return m_ClassMap.IsReadOnly
        End Get
        Set(ByVal Value As Boolean)
            m_ClassMap.IsReadOnly = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "IsReadOnly")
        End Set
    End Property


    <Category("Caching"), _
        Description("The value in seconds that objects of this class may stay cached before going stale and in need of a reload. The default value of -1 means the time to live value of the domain map will be used."), _
        DisplayName("Time to live"), _
        DefaultValue(-1)> Public Property TimeToLive() As Integer
        Get
            Return m_ClassMap.TimeToLive
        End Get
        Set(ByVal Value As Integer)
            If Value <> m_ClassMap.TimeToLive Then
                m_ClassMap.TimeToLive = Value
                RaiseEvent AfterPropertySet(m_ClassMap, "TimeToLive")
            End If
        End Set
    End Property

    <Category("Caching"), _
        Description("The behavior specifying how objects of this class may stay cached before going stale (according to the timespan specified in time to live) and in need of a reload. The default value of means the time to live behavior of the domain map will be used."), _
        DisplayName("Time to live behavior"), _
        DefaultValue(TimeToLiveBehavior.Default)> Public Property TheTimeToLiveBehavior() As TimeToLiveBehavior
        Get
            Return m_ClassMap.TimeToLiveBehavior
        End Get
        Set(ByVal Value As TimeToLiveBehavior)
            If Value <> m_ClassMap.TimeToLiveBehavior Then
                m_ClassMap.TimeToLiveBehavior = Value
                RaiseEvent AfterPropertySet(m_ClassMap, "TimeToLiveBehavior")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("This behavior specifies how objects of this class are loaded when you load objects by identity. Lazy means that the object will not be filled with values until you read a non-identity property of the object, while eager means the object will be loaded with values from the database right away. The default value of means the loading behavior of the domain map will be used."), _
        DisplayName("Loading behavior"), _
        DefaultValue(LoadBehavior.Default)> Public Property TheLoadBehavior() As LoadBehavior
        Get
            Return m_ClassMap.LoadBehavior
        End Get
        Set(ByVal Value As LoadBehavior)
            If Value <> m_ClassMap.LoadBehavior Then
                m_ClassMap.LoadBehavior = Value
                RaiseEvent AfterPropertySet(m_ClassMap, "LoadBehavior")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
        TypeConverter(GetType(SuperClassConverter)), _
        Description("The name of the superclass that this class inherits from. Leave blank if this class does not inherit from any superclass."), _
        DisplayName("Inherits from class"), _
        DefaultValue("")> Public Property InheritsClass() As String
        Get
            Return m_ClassMap.InheritsClass
        End Get
        Set(ByVal Value As String)
            Dim superClass As IClassMap
            Dim ns As String
            If Len(Value) > 0 Then
                superClass = m_ClassMap.DomainMap.GetClassMap(Value)
                If superClass Is Nothing Then
                    ns = Me.NameSpaceName
                    If Len(ns) > 0 Then
                        superClass = m_ClassMap.DomainMap.GetClassMap(ns & "." & Value)
                    End If
                End If
                If superClass Is Nothing Then
                    MsgBox("Could not find class '" & Value & "'!")
                    Exit Property
                End If
                If m_ClassMap Is superClass Then
                    MsgBox("The class can not inherit from itself!")
                    Exit Property
                End If
                If Not m_ClassMap.IsLegalAsSuperClass(superClass) Then
                    MsgBox("The class '" & Value & "' is not legal as superclass for this class! Since this class is already a superclass to the class '" & Value & "', this would create a cyclic inheritance hierarchy!")
                    Exit Property
                End If
            End If
            m_ClassMap.InheritsClass = Value
            MapServices.SetSuperClass(m_ClassMap)
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ClassMap, "InheritsClass")
        End Set
    End Property

    <Category("Inheritance"), _
        Description("The name of the inheritance patterns that this class uses. Leave blank if this class does not inherit from any superclass."), _
        DisplayName("Type of inheritance"), _
        DefaultValue(InheritanceType.None)> Public Property Inheritance() As InheritanceType
        Get
            Return m_ClassMap.InheritanceType
        End Get
        Set(ByVal Value As InheritanceType)
            m_ClassMap.InheritanceType = Value
            If Value <> InheritanceType.None Then
                If m_ClassMap.TypeValue = "" Then
                    m_ClassMap.TypeValue = UCase(Left(m_ClassMap.GetName, 1))
                End If
            Else
                m_ClassMap.InheritsClass = ""
                m_ClassMap.TypeColumn = ""
                m_ClassMap.TypeValue = ""
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ClassMap, "InheritanceType")
        End Set
    End Property

    <Category("Inheritance"), _
        Description("The name of the type dicriminator column that this specifies the type of each row in the table. Use in conjunction with the Type discriminator value. Applies only to classes that use Single Table Inheritance."), _
        DisplayName("Type discriminator column"), _
        DefaultValue("")> Public Property TypeColumn() As String
        Get
            Return m_ClassMap.TypeColumn
        End Get
        Set(ByVal Value As String)
            m_ClassMap.TypeColumn = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "TypeColumn")
        End Set
    End Property

    <Category("Inheritance"), _
        Description("The value in the type dicriminator column specifies that the row is an object of this class. Use in conjunction with the Type discriminator column. Applies only to classes that use Single Table Inheritance."), _
        DisplayName("Type discriminator value"), _
        DefaultValue("")> Public Property TypeValue() As String
        Get
            Return m_ClassMap.TypeValue
        End Get
        Set(ByVal Value As String)
            m_ClassMap.TypeValue = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "TypeValue")
        End Set
    End Property

    <Category("Inheritance"), _
    Description("By setting to True, this class will become abstract, which means that no instances can be created from it (rather instances should be created from concrete subclasses to this class). For a class with the 'ConcreteTableInheritance' inheritance type, no table will be used for this class. Applies only to classes that participate in an inheritance hierarchy and that has concrete subclasses."), _
    DisplayName("Abstract"), _
    DefaultValue(False)> Public Property IsAbstract() As Boolean
        Get
            Return m_ClassMap.IsAbstract
        End Get
        Set(ByVal Value As Boolean)
            m_ClassMap.IsAbstract = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "IsAbstract")
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(SourceConverter)), _
        Description("The name of the data source that this class is mapping to. Leave blank in order to use the default data source of the domain."), _
        DisplayName("Source name"), _
        DefaultValue("")> Public Property Source() As String
        Get
            Return m_ClassMap.Source
        End Get
        Set(ByVal Value As String)
            m_ClassMap.Source = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "Source")
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(TableConverter)), _
        Description("The name of the table that this class is mapping to."), _
        DisplayName("Table name"), _
        DefaultValue("")> Public Property Table() As String
        Get
            Return m_ClassMap.Table
        End Get
        Set(ByVal Value As String)
            m_ClassMap.Table = Value
            DialogueServices.AskToCreateIfTableNotExist(Value, m_ClassMap.GetSourceMap)
            RaiseEvent AfterPropertySet(m_ClassMap, "Table")
        End Set
    End Property

    <Category("Design"), _
        Description("The string that will be used as separator when concatenating the values of the identity properties into an identity string. Leave blank to use the default separator (""|""). Applies only to classes with composite keys (using more than one identity property)."), _
        DisplayName("Identity separator"), _
        DefaultValue("")> Public Property IdentitySeparator() As String
        Get
            Return m_ClassMap.IdentitySeparator
        End Get
        Set(ByVal Value As String)
            m_ClassMap.IdentitySeparator = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "IdentitySeparator")
        End Set
    End Property


    <Category("Design"), _
        Description("The string that will be used as separator when concatenating the values of the key properties into a key string. Leave blank to use the default separator ("" "")."), _
        DisplayName("Key separator"), _
        DefaultValue("")> Public Property KeySeparator() As String
        Get
            Return m_ClassMap.KeySeparator
        End Get
        Set(ByVal Value As String)
            m_ClassMap.KeySeparator = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "KeySeparator")
        End Set
    End Property


    <Category("O/O Mapping"), _
        TypeConverter(GetType(SourceClassConverter)), _
        Description("The name of the source class that this class is mapping to."), _
        DisplayName("Source class"), _
        DefaultValue("")> Public Property SourceClass() As String
        Get
            Return m_ClassMap.SourceClass
        End Get
        Set(ByVal Value As String)
            m_ClassMap.SourceClass = Value
            'DialogueServices.AskToCreateIfClassNotExist(Value, m_ClassMap.GetSourceClassMap)
            RaiseEvent AfterPropertySet(m_ClassMap, "SourceClass")
        End Set
    End Property



    <Category("Merging"), _
    Description("The default merging behavior for objects of this class. DefaultBehavior means that the merging behavior of the domain map will be used."), _
    DisplayName("Merging behavior"), _
    DefaultValue(MergeBehaviorType.DefaultBehavior)> Public Property MergeBehavior() As MergeBehaviorType
        Get
            Return m_ClassMap.MergeBehavior
        End Get
        Set(ByVal Value As MergeBehaviorType)
            m_ClassMap.MergeBehavior = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "MergeBehavior")
        End Set
    End Property


    <Category("Code Generation"), _
        Description("If the class inherits from a super class which is not another domain class (but perhaps some custom base class you have created) you can enter the fully qualified name of that super class here in order to let the code generator know from which super class this class should inherit. Only used for code generation."), _
        DisplayName("Inherits transient class"), _
        DefaultValue("")> Public Property InheritsTransientClass() As String
        Get
            Return m_ClassMap.InheritsTransientClass
        End Get
        Set(ByVal Value As String)
            m_ClassMap.InheritsTransientClass = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "InheritsTransientClass")
        End Set
    End Property

    <Category("Code Generation"), _
        Description("Using this property you can let the code generator know which interfaces this class implements. Only used for code generation."), _
        DisplayName("Implements interfaces"), _
        DefaultValue("")> Public Property ImplementsInterfaces() As String()
        Get
            Return m_ClassMap.ImplementsInterfaces.ToArray(GetType(String))
        End Get
        Set(ByVal Value As String())
            m_ClassMap.ImplementsInterfaces = New ArrayList(Value)
            RaiseEvent AfterPropertySet(m_ClassMap, "ImplementsInterfaces")
        End Set
    End Property


    <Category("Code Generation"), _
        Description("Using this property you can let the code generator know which namespaces this class imports. Only used for code generation."), _
        DisplayName("Imports namespaces"), _
        DefaultValue("")> Public Property ImportsNamespaces() As String()
        Get
            Return m_ClassMap.ImportsNamespaces.ToArray(GetType(String))
        End Get
        Set(ByVal Value As String())
            m_ClassMap.ImportsNamespaces = New ArrayList(Value)
            RaiseEvent AfterPropertySet(m_ClassMap, "ImportsNamespaces")
        End Set
    End Property



    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to the properties in this class when the object is updated. If a property's update optimistic concurrency behavior is set to DefaultBehavior then the behavior will be inherited from this setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting update behavior for the property will become IncludeWhenDirty except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Update optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property UpdateOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Return m_ClassMap.UpdateOptimisticConcurrencyBehavior
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            m_ClassMap.UpdateOptimisticConcurrencyBehavior = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "UpdateOptimisticConcurrencyBehavior")
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to the properties in this class when the object is deleted. If a property's delete optimistic concurrency behavior is set to DefaultBehavior then the behavior will be inherited from this setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting delete behavior for the property will become IncludeWhenLoaded except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Delete optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property DeleteOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Return m_ClassMap.DeleteOptimisticConcurrencyBehavior
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            m_ClassMap.DeleteOptimisticConcurrencyBehavior = Value
            RaiseEvent AfterPropertySet(m_ClassMap, "DeleteOptimisticConcurrencyBehavior")
        End Set
    End Property




    '------------------------------------
    'NHibernate
    '------------------------------------


    <Category("NHibernate"), _
        Description("Specifies whether objects of this class can be updated and deleted. (NHibernate specific)."), _
        DisplayName("NH mutable"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhMutable() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-mutable"
            meta = m_ClassMap.GetMetaData(key)
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
            Dim key As String = "nh-mutable"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_ClassMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_ClassMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhMutable")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies an interface to use for lazy loading proxies. (NHibernate specific)."), _
        DisplayName("NH proxy"), _
        DefaultValue("")> Public Property NhProxy() As String
        Get
            Dim meta As String
            Dim key As String = "nh-proxy"
            meta = m_ClassMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-proxy"
            Select Case Value
                Case ""
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_ClassMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhProxy")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether Update Sql statements including only dirty columns should be generated at runtime. (NHibernate specific)."), _
        DisplayName("NH dynamic-update"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhDynamicUpdate() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-dynamic-update"
            meta = m_ClassMap.GetMetaData(key)
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
            Dim key As String = "nh-dynamic-update"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_ClassMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_ClassMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhDynamicUpdate")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether Insert Sql statements including only dirty columns should be generated at runtime. (NHibernate specific)."), _
        DisplayName("NH dynamic-insert"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhDynamicInsert() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-dynamic-insert"
            meta = m_ClassMap.GetMetaData(key)
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
            Dim key As String = "nh-dynamic-insert"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_ClassMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_ClassMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhDynamicInsert")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether implicit or explicit query polymorphism is used. (NHibernate specific)."), _
        DisplayName("NH polymorphism"), _
        DefaultValue(NhPolymorphismEnum.Unspecified)> Public Property NhPolymorphism() As NhPolymorphismEnum
        Get
            Dim meta As String
            Dim key As String = "nh-polymorphism"
            meta = m_ClassMap.GetMetaData(key)
            Select Case LCase(meta)
                Case "implicit"
                    Return NhPolymorphismEnum.Implicit
                Case "explicit"
                    Return NhPolymorphismEnum.Explicit
                Case Else
                    Return NhPolymorphismEnum.Unspecified
            End Select
        End Get
        Set(ByVal Value As NhPolymorphismEnum)
            Dim key As String = "nh-polymorphism"
            Select Case Value
                Case NhPolymorphismEnum.Unspecified
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case NhPolymorphismEnum.Implicit
                    m_ClassMap.SetMetaData(key, "implicit")

                Case NhPolymorphismEnum.Explicit
                    m_ClassMap.SetMetaData(key, "explicit")
            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhPolymorphism")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("A custom sql where clause that will be used when fetching objects of this class. (NHibernate specific)."), _
        DisplayName("NH where"), _
        DefaultValue("")> Public Property NhWhere() As String
        Get
            Dim meta As String
            Dim key As String = "nh-where"
            meta = m_ClassMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-where"
            Select Case Value
                Case ""
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_ClassMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhWhere")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("A custom sql where clause that will be used when fetching objects of this class. (NHibernate specific)."), _
        DisplayName("NH persister"), _
        DefaultValue("")> Public Property NhPersister() As String
        Get
            Dim meta As String
            Dim key As String = "nh-persister"
            meta = m_ClassMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-persister"
            Select Case Value
                Case ""
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_ClassMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhPersister")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether NHibernate should be forced to specify allowed discriminator values even when retrieving all instances of the root class. (NHibernate specific)."), _
        DisplayName("NH discriminator force"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhDiscriminatorForce() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-force"
            meta = m_ClassMap.GetMetaData(key)
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
            Dim key As String = "nh-force"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_ClassMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_ClassMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhDiscriminatorForce")
        End Set
    End Property




    <Category("NHibernate"), _
        Description("The name of the property (of component type) that holds the composite identifier. (NHibernate specific)."), _
        DisplayName("NH composite id name"), _
        DefaultValue("")> Public Property NhCompositeName() As String
        Get
            Dim meta As String
            Dim key As String = "nh-composite-name"
            meta = m_ClassMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-composite-name"
            Select Case Value
                Case ""
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_ClassMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhCompositeName")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("The class of the component used as a composite identifier . (NHibernate specific)."), _
        DisplayName("NH composite class"), _
        DefaultValue("")> Public Property NhCompositeClass() As String
        Get
            Dim meta As String
            Dim key As String = "nh-composite-class"
            meta = m_ClassMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-composite-class"
            Select Case Value
                Case ""
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_ClassMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhCompositeClass")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Indicates if set to 'any' that transient instances should be considered newly instantiated. Defaults to 'none'. (NHibernate specific)."), _
        DisplayName("NH composite unsaved-value"), _
        DefaultValue("")> Public Property NhCompositeUnsavedValue() As String
        Get
            Dim meta As String
            Dim key As String = "nh-composite-unsaved-value"
            meta = m_ClassMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-composite-unsaved-value"
            Select Case Value
                Case ""
                    If m_ClassMap.HasMetaData(key) Then
                        m_ClassMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_ClassMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_ClassMap, "NhCompositeUnsavedValue")
        End Set
    End Property


    Private Sub RemoveShadowProperties()

        Dim propertyMap As IPropertyMap
        Dim inheritedNames As New Hashtable
        Dim nonInheritedPropertyMaps As ArrayList
        Dim superClassMap As IClassMap = m_ClassMap.GetInheritedClassMap

        If Not superClassMap Is Nothing Then

            For Each propertyMap In superClassMap.GetAllPropertyMaps

                inheritedNames(LCase(propertyMap.Name)) = True

            Next

            nonInheritedPropertyMaps = m_ClassMap.GetNonInheritedPropertyMaps

            For Each propertyMap In nonInheritedPropertyMaps

                If inheritedNames.ContainsKey(LCase(propertyMap.Name)) Then

                    m_ClassMap.PropertyMaps.Remove(propertyMap)

                End If

            Next

        End If

    End Sub


    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Select Case propDesc.Name

            Case "IsReadOnly"
                If m_ClassMap.DomainMap.IsReadOnly Then propDesc.SetReadOnly()

            Case "TypeColumn"
                If m_ClassMap.InheritanceType = InheritanceType.None Then propDesc.SetReadOnly()

            Case "TypeValue"
                If m_ClassMap.InheritanceType = InheritanceType.None Then propDesc.SetReadOnly()

            Case "IsAbstract"
                If m_ClassMap.InheritanceType = InheritanceType.None Then propDesc.SetReadOnly()

        End Select

        Return propDesc
    End Function



    Public Overrides Function RemoveHiddenProperties(ByVal baseProps As PropertyDescriptorCollection) As PropertyDescriptorCollection

        Dim newProps(baseProps.Count - 1) As PropertyDescriptor
        Dim i As Integer
        Dim j As Integer = 0

        For i = 0 To baseProps.Count - 1

            Dim add As Boolean = True

            If m_ClassMap.ClassType = ClassType.Class Or m_ClassMap.ClassType = ClassType.Default Then

                'we want it all

            Else

                'remove mappings
                Select Case baseProps.Item(i).Name

                    Case "DeleteOptimisticConcurrencyBehavior", "DocClassMapMode", "DocElement", "DocParentProperty", _
                        "DocRoot", "DocSource", "IdentitySeparator", "Inheritance", "InheritsClass", "InheritsTransientClass", _
                        "IsAbstract", "IsReadOnly", "KeySeparator", "MergeBehavior", "RefreshBehavior", "Source", _
                        "SourceClass", "Table", "TypeColumn", "TypeValue", "UpdateOptimisticConcurrencyBehavior", _
                        "ValidateMethod", "ValidationMode", "TimeToLive", "TheTimeToLiveBehavior", "TheLoadBehavior"

                        add = False

                End Select

            End If

            If m_ClassMap.ClassType = ClassType.Class Or m_ClassMap.ClassType = ClassType.Default Or m_ClassMap.ClassType = ClassType.Interface Then

                'we want it all

            Else


                'remove mappings
                Select Case baseProps.Item(i).Name

                    Case "ImplementsInterfaces"

                        add = False

                End Select

            End If


            If add Then

                newProps(j) = baseProps.Item(i)
                j += 1

            End If

        Next

        If j < i Then

            Dim prunedProps(j - 1) As PropertyDescriptor

            For i = 0 To j - 1

                prunedProps(i) = newProps(i)

            Next

            Return New PropertyDescriptorCollection(prunedProps)

        Else

            Return New PropertyDescriptorCollection(newProps)

        End If

    End Function

End Class
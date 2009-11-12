Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.ComponentModel
Imports Puzzle.ObjectMapper.GUI.Uml
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class UmlClassProperties
    Inherits PropertiesBase

    Private m_UmlClass As UmlClass

    Public Event BeforePropertySet(ByVal mapObject As Object, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As Object, ByVal propertyName As String)

    Public Function GetUmlClass() As UmlClass
        Return m_UmlClass
    End Function

    Public Sub SetUmlClass(ByVal value As UmlClass)
        m_UmlClass = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_UmlClass

    End Function

    <Category("Design"), _
    Description("The name of the class represented by this shape."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim arrName() As String = Split(classMap.Name, ".")
                Return arrName(UBound(arrName))
            Else
                Return m_UmlClass.Name
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim arrName() As String = Split(classMap.Name, ".")
                Dim name As String
                arrName(UBound(arrName)) = Value
                name = Join(arrName, ".")
                If name <> classMap.Name Then
                    RaiseEvent BeforePropertySet(classMap, "Name", name, classMap.Name)
                    classMap.UpdateName(name)
                    RaiseEvent AfterPropertySet(classMap, "Name")
                End If
            Else
                RaiseEvent BeforePropertySet(m_UmlClass, "Name", Value, m_UmlClass.Name)
                m_UmlClass.Name = Value
                RaiseEvent AfterPropertySet(m_UmlClass, "Name")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("The namepace that this class belongs to."), _
        DisplayName("Namespace"), _
        DefaultValue("")> Public Property NameSpaceName() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim arrName() As String = Split(classMap.Name, ".")
                If UBound(arrName) > 0 Then
                    ReDim Preserve arrName(UBound(arrName) - 1)
                    Return Join(arrName, ".")
                End If
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim arrName() As String = Split(classMap.Name, ".")
                Dim name As String = arrName(UBound(arrName))
                If Len(Value) > 0 Then
                    name = Value & "." & name
                End If
                If name <> classMap.Name Then
                    RaiseEvent BeforePropertySet(classMap, "Namespace", name, classMap.Name)
                    classMap.UpdateName(name)
                    RaiseEvent AfterPropertySet(classMap, "Namespace")
                End If
            End If
        End Set
    End Property

    <Category("Design"), _
    Description("The name of the assembly that this class belongs to. If no setting is specified, the default setting specified in the 'Assembly name' property for the domain will be used."), _
    DisplayName("Assembly name"), _
    DefaultValue("")> Public Property AssemblyName() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.AssemblyName
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                If Value <> classMap.AssemblyName Then
                    classMap.AssemblyName = Value
                    RaiseEvent AfterPropertySet(classMap, "AssemblyName")
                End If
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("Set to True for read-only access to this class. If True, objects from this class can only be fetched, not created, updated or deleted."), _
        DisplayName("Read only"), _
        DefaultValue(False)> Public Property IsReadOnly() As Boolean
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.IsReadOnly
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.IsReadOnly = Value
                RaiseEvent AfterPropertySet(classMap, "IsReadOnly")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
        TypeConverter(GetType(SuperClassConverter)), _
        Description("The name of the superclass that this class inherits from. Leave blank if this class does not inherit from any superclass."), _
        DisplayName("Inherits from class"), _
        DefaultValue("")> Public Property InheritsClass() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.InheritsClass
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim superClass As IClassMap
                Dim ns As String
                If Len(Value) > 0 Then
                    superClass = classMap.DomainMap.GetClassMap(Value)
                    If superClass Is Nothing Then
                        ns = Me.NameSpaceName
                        If Len(ns) > 0 Then
                            superClass = classMap.DomainMap.GetClassMap(ns & "." & Value)
                        End If
                    End If
                    If superClass Is Nothing Then
                        MsgBox("Could not find class '" & Value & "'!")
                        Exit Property
                    End If
                    If classMap Is superClass Then
                        MsgBox("The class can not inherit from itself!")
                        Exit Property
                    End If
                    If Not classMap.IsLegalAsSuperClass(superClass) Then
                        MsgBox("The class '" & Value & "' is not legal as superclass for this class! Since this class is already a superclass to the class '" & Value & "', this would create a cyclic inheritance hierarchy!")
                        Exit Property
                    End If
                End If
                classMap.InheritsClass = Value
                MapServices.SetSuperClass(classMap)
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(classMap, "InheritsClass")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
        Description("The name of the inheritance patterns that this class uses. Leave blank if this class does not inherit from any superclass."), _
        DisplayName("Type of inheritance"), _
        DefaultValue(InheritanceType.None)> Public Property Inheritance() As InheritanceType
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.InheritanceType
            End If
        End Get
        Set(ByVal Value As InheritanceType)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.InheritanceType = Value
                If Value <> InheritanceType.None Then
                    If classMap.TypeValue = "" Then
                        classMap.TypeValue = UCase(Left(classMap.GetName, 1))
                    End If
                Else
                    classMap.InheritsClass = ""
                    classMap.TypeColumn = ""
                    classMap.TypeValue = ""
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(classMap, "InheritanceType")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
        Description("The name of the type dicriminator column that this specifies the type of each row in the table. Use in conjunction with the Type discriminator value. Applies only to classes that use Single Table Inheritance."), _
        DisplayName("Type discriminator column"), _
        DefaultValue("")> Public Property TypeColumn() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.TypeColumn
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.TypeColumn = Value
                RaiseEvent AfterPropertySet(classMap, "TypeColumn")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
        Description("The value in the type dicriminator column specifies that the row is an object of this class. Use in conjunction with the Type discriminator column. Applies only to classes that use Single Table Inheritance."), _
        DisplayName("Type discriminator value"), _
        DefaultValue("")> Public Property TypeValue() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.TypeValue
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.TypeValue = Value
                RaiseEvent AfterPropertySet(classMap, "TypeValue")
            End If
        End Set
    End Property

    <Category("Inheritance"), _
    Description("By setting to True, this class will become abstract, which means that no instances can be created from it (rather instances should be created from concrete subclasses to this class). For a class with the 'ConcreteTableInheritance' inheritance type, no table will be used for this class. Applies only to classes that participate in an inheritance hierarchy and that has concrete subclasses."), _
    DisplayName("Abstract"), _
    DefaultValue(False)> Public Property IsAbstract() As Boolean
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.IsAbstract
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.IsAbstract = Value
                RaiseEvent AfterPropertySet(classMap, "IsAbstract")
            End If
        End Set
    End Property


    <Category("Caching"), _
        Description("The value in seconds that objects of this class may stay cached before going stale and in need of a reload. The default value of -1 means the time to live value of the domain map will be used."), _
        DisplayName("Time to live"), _
        DefaultValue(-1)> Public Property TimeToLive() As Integer
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.TimeToLive
            End If
        End Get
        Set(ByVal Value As Integer)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                If Value <> classMap.TimeToLive Then
                    classMap.TimeToLive = Value
                    RaiseEvent AfterPropertySet(classMap, "TimeToLive")
                End If
            End If
        End Set
    End Property

    <Category("Caching"), _
        Description("The behavior specifying how objects of this class may stay cached before going stale (according to the timespan specified in time to live) and in need of a reload. The default value of means the time to live behavior of the domain map will be used."), _
        DisplayName("Time to live behavior"), _
        DefaultValue(TimeToLiveBehavior.Default)> Public Property TheTimeToLiveBehavior() As TimeToLiveBehavior
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.TimeToLiveBehavior
            End If
        End Get
        Set(ByVal Value As TimeToLiveBehavior)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                If Value <> classMap.TimeToLiveBehavior Then
                    classMap.TimeToLiveBehavior = Value
                    RaiseEvent AfterPropertySet(classMap, "TimeToLiveBehavior")
                End If
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("This behavior specifies how objects of this class are loaded when you load objects by identity. Lazy means that the object will not be filled with values until you read a non-identity property of the object, while eager means the object will be loaded with values from the database right away. The default value of means the loading behavior of the domain map will be used."), _
        DisplayName("Loading behavior"), _
        DefaultValue(LoadBehavior.Default)> Public Property TheLoadBehavior() As LoadBehavior
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.LoadBehavior
            End If
        End Get
        Set(ByVal Value As LoadBehavior)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                If Value <> classMap.LoadBehavior Then
                    classMap.LoadBehavior = Value
                    RaiseEvent AfterPropertySet(classMap, "LoadBehavior")
                End If
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(SourceConverter)), _
        Description("The name of the data source that this class is mapping to. Leave blank in order to use the default data source of the domain."), _
        DisplayName("Source name"), _
        DefaultValue("")> Public Property Source() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.Source
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.Source = Value
                RaiseEvent AfterPropertySet(classMap, "Source")
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(TableConverter)), _
        Description("The name of the table that this class is mapping to."), _
        DisplayName("Table name"), _
        DefaultValue("")> Public Property Table() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.Table
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.Table = Value
                DialogueServices.AskToCreateIfTableNotExist(Value, classMap.GetSourceMap)
                RaiseEvent AfterPropertySet(classMap, "Table")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("The string that will be used as separator when concatenating the values of the identity properties into an identity string. Leave blank to use the default separator. Applies only to classes with composite keys (using more than one identity property)."), _
        DisplayName("Identity separator"), _
        DefaultValue("")> Public Property IdentitySeparator() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.IdentitySeparator
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.IdentitySeparator = Value
                RaiseEvent AfterPropertySet(classMap, "IdentitySeparator")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("The string that will be used as separator when concatenating the values of the key properties into a key string. Leave blank to use the default separator ("" "")."), _
        DisplayName("Key separator"), _
        DefaultValue("")> Public Property KeySeparator() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.KeySeparator
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.KeySeparator = Value
                RaiseEvent AfterPropertySet(classMap, "KeySeparator")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("Define a load span for this class. A load span corresponds to the select clause of an NPath query. Leave empty for default load span '*'."), _
        DisplayName("Load span"), _
        DefaultValue("")> Public Property LoadSpan() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.LoadSpan
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                If Value <> classMap.LoadSpan Then
                    classMap.LoadSpan = Value
                    RaiseEvent AfterPropertySet(classMap, "LoadSpan")
                End If
            End If
        End Set
    End Property

    '<Category("Design"), _
    ' Description("The name of the validation method on this class. (Must be a method that returns void and takes no parameters or one IList parameter with exceptions)."), _
    ' DisplayName("Type"), _
    ' DefaultValue(ClassType.Default)> Public Property TheClassType() As ClassType
    '    Get
    '        Dim classMap As IClassMap = m_UmlClass.GetClassMap
    '        If Not classMap Is Nothing Then
    '            Return classMap.ClassType
    '        End If
    '    End Get
    '    Set(ByVal Value As ClassType)
    '        Dim classMap As IClassMap = m_UmlClass.GetClassMap
    '        If Not classMap Is Nothing Then
    '            If Value <> classMap.ClassType Then
    '                classMap.ClassType = Value
    '                RaiseEvent AfterPropertySet(classMap, "ClassType")
    '            End If
    '        End If
    '    End Set
    'End Property


    <Category("Validation"), _
    Description("The name of the validation method on this class. (Must be a method that returns void and takes no parameters or one IList parameter with exceptions)."), _
    DisplayName("Validate method"), _
    DefaultValue("")> Public Property ValidateMethod() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.ValidateMethod
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                If Value <> classMap.ValidateMethod Then
                    classMap.ValidateMethod = Value
                    RaiseEvent AfterPropertySet(classMap, "ValidateMethod")
                End If
            End If
        End Set
    End Property

    <Category("O/O Mapping"), _
        TypeConverter(GetType(SourceClassConverter)), _
        Description("The name of the source class that this class is mapping to."), _
        DisplayName("Source class"), _
        DefaultValue("")> Public Property SourceClass() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.SourceClass
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.SourceClass = Value
                'DialogueServices.AskToCreateIfClassNotExist(Value, classMap.GetSourceClassMap)
                RaiseEvent AfterPropertySet(classMap, "SourceClass")
            End If
        End Set
    End Property


    <Category("Merging"), _
    Description("The default merging behavior for objects of this class. DefaultBehavior means that the merging behavior of the domain map will be used."), _
    DisplayName("Merging behavior"), _
    DefaultValue(MergeBehaviorType.DefaultBehavior)> Public Property MergeBehavior() As MergeBehaviorType
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.MergeBehavior
            End If
        End Get
        Set(ByVal Value As MergeBehaviorType)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.MergeBehavior = Value
                RaiseEvent AfterPropertySet(classMap, "MergeBehavior")
            End If
        End Set
    End Property


    <Category("Code Generation"), _
        Description("If the class inherits from a super class which is not another domain class (but perhaps some custom base class you have created) you can enter the fully qualified name of that super class here in order to let the code generator know from which super class this class should inherit. Only used for code generation."), _
        DisplayName("Inherits transient class"), _
        DefaultValue("")> Public Property InheritsTransientClass() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.InheritsTransientClass
            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.InheritsTransientClass = Value
                RaiseEvent AfterPropertySet(classMap, "InheritsTransientClass")
            End If
        End Set
    End Property

    <Category("Code Generation"), _
        Description("Using this property you can let the code generator know which interfaces this class implements. Only used for code generation."), _
        DisplayName("Implements interfaces"), _
        DefaultValue("")> Public Property ImplementsInterfaces() As String()
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.ImplementsInterfaces.ToArray(GetType(String))
            End If
        End Get
        Set(ByVal Value As String())
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.ImplementsInterfaces = New ArrayList(Value)
                RaiseEvent AfterPropertySet(classMap, "ImplementsInterfaces")
            End If
        End Set
    End Property


    <Category("Code Generation"), _
        Description("Using this property you can let the code generator know which namespaces this class imports. Only used for code generation."), _
        DisplayName("Imports namespaces"), _
        DefaultValue("")> Public Property ImportsNamespaces() As String()
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.ImportsNamespaces.ToArray(GetType(String))
            End If
        End Get
        Set(ByVal Value As String())
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.ImportsNamespaces = New ArrayList(Value)
                RaiseEvent AfterPropertySet(classMap, "ImportsNamespaces")
            End If
        End Set
    End Property



    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to the properties in this class when the object is updated. If a property's update optimistic concurrency behavior is set to DefaultBehavior then the behavior will be inherited from this setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting update behavior for the property will become IncludeWhenDirty except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Update optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property UpdateOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.UpdateOptimisticConcurrencyBehavior
            End If
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.UpdateOptimisticConcurrencyBehavior = Value
                RaiseEvent AfterPropertySet(classMap, "UpdateOptimisticConcurrencyBehavior")
            End If
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to the properties in this class when the object is deleted. If a property's delete optimistic concurrency behavior is set to DefaultBehavior then the behavior will be inherited from this setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting delete behavior for the property will become IncludeWhenLoaded except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Delete optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property DeleteOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Return classMap.DeleteOptimisticConcurrencyBehavior
            End If
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                classMap.DeleteOptimisticConcurrencyBehavior = Value
                RaiseEvent AfterPropertySet(classMap, "DeleteOptimisticConcurrencyBehavior")
            End If
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
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-mutable"
                meta = classMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "true"
                        Return NullableBool.True
                    Case "false"
                        Return NullableBool.False
                    Case Else
                        Return NullableBool.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NullableBool)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-mutable"
                Select Case Value
                    Case NullableBool.Unspecified
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case NullableBool.True
                        classMap.SetMetaData(key, "true")

                    Case NullableBool.False
                        classMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(classMap, "NhMutable")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies an interface to use for lazy loading proxies. (NHibernate specific)."), _
        DisplayName("NH proxy"), _
        DefaultValue("")> Public Property NhProxy() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-proxy"
                meta = classMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-proxy"
                Select Case Value
                    Case ""
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case Else
                        classMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(classMap, "NhProxy")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether Update Sql statements including only dirty columns should be generated at runtime. (NHibernate specific)."), _
        DisplayName("NH dynamic-update"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhDynamicUpdate() As NullableBool
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-dynamic-update"
                meta = classMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "true"
                        Return NullableBool.True
                    Case "false"
                        Return NullableBool.False
                    Case Else
                        Return NullableBool.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NullableBool)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-dynamic-update"
                Select Case Value
                    Case NullableBool.Unspecified
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case NullableBool.True
                        classMap.SetMetaData(key, "true")

                    Case NullableBool.False
                        classMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(classMap, "NhDynamicUpdate")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether Insert Sql statements including only dirty columns should be generated at runtime. (NHibernate specific)."), _
        DisplayName("NH dynamic-insert"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhDynamicInsert() As NullableBool
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-dynamic-insert"
                meta = classMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "true"
                        Return NullableBool.True
                    Case "false"
                        Return NullableBool.False
                    Case Else
                        Return NullableBool.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NullableBool)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-dynamic-insert"
                Select Case Value
                    Case NullableBool.Unspecified
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case NullableBool.True
                        classMap.SetMetaData(key, "true")

                    Case NullableBool.False
                        classMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(classMap, "NhDynamicInsert")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether implicit or explicit query polymorphism is used. (NHibernate specific)."), _
        DisplayName("NH polymorphism"), _
        DefaultValue(NhPolymorphismEnum.Unspecified)> Public Property NhPolymorphism() As NhPolymorphismEnum
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-polymorphism"
                meta = classMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "implicit"
                        Return NhPolymorphismEnum.Implicit
                    Case "explicit"
                        Return NhPolymorphismEnum.Explicit
                    Case Else
                        Return NhPolymorphismEnum.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NhPolymorphismEnum)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-polymorphism"
                Select Case Value
                    Case NhPolymorphismEnum.Unspecified
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case NhPolymorphismEnum.Implicit
                        classMap.SetMetaData(key, "implicit")

                    Case NhPolymorphismEnum.Explicit
                        classMap.SetMetaData(key, "explicit")
                End Select
                RaiseEvent AfterPropertySet(classMap, "NhPolymorphism")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("A custom sql where clause that will be used when fetching objects of this class. (NHibernate specific)."), _
        DisplayName("NH where"), _
        DefaultValue("")> Public Property NhWhere() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-where"
                meta = classMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-where"
                Select Case Value
                    Case ""
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case Else
                        classMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(classMap, "NhWhere")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("A custom sql where clause that will be used when fetching objects of this class. (NHibernate specific)."), _
        DisplayName("NH persister"), _
        DefaultValue("")> Public Property NhPersister() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-persister"
                meta = classMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-persister"
                Select Case Value
                    Case ""
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case Else
                        classMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(classMap, "NhPersister")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether NHibernate should be forced to specify allowed discriminator values even when retrieving all instances of the root class. (NHibernate specific)."), _
        DisplayName("NH discriminator force"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhDiscriminatorForce() As NullableBool
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-force"
                meta = classMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "true"
                        Return NullableBool.True
                    Case "false"
                        Return NullableBool.False
                    Case Else
                        Return NullableBool.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NullableBool)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-force"
                Select Case Value
                    Case NullableBool.Unspecified
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case NullableBool.True
                        classMap.SetMetaData(key, "true")

                    Case NullableBool.False
                        classMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(classMap, "NhDiscriminatorForce")

            End If
        End Set
    End Property




    <Category("NHibernate"), _
        Description("The name of the property (of component type) that holds the composite identifier. (NHibernate specific)."), _
        DisplayName("NH composite id name"), _
        DefaultValue("")> Public Property NhCompositeName() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-composite-name"
                meta = classMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-composite-name"
                Select Case Value
                    Case ""
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case Else
                        classMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(classMap, "NhCompositeName")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("The class of the component used as a composite identifier . (NHibernate specific)."), _
        DisplayName("NH composite class"), _
        DefaultValue("")> Public Property NhCompositeClass() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-composite-class"
                meta = classMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-composite-class"
                Select Case Value
                    Case ""
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case Else
                        classMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(classMap, "NhCompositeClass")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Indicates if set to 'any' that transient instances should be considered newly instantiated. Defaults to 'none'. (NHibernate specific)."), _
        DisplayName("NH composite unsaved-value"), _
        DefaultValue("")> Public Property NhCompositeUnsavedValue() As String
        Get
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-composite-unsaved-value"
                meta = classMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim classMap As IClassMap = m_UmlClass.GetClassMap
            If Not classMap Is Nothing Then
                Dim key As String = "nh-composite-unsaved-value"
                Select Case Value
                    Case ""
                        If classMap.HasMetaData(key) Then
                            classMap.RemoveMetaData(key)
                        End If

                    Case Else
                        classMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(classMap, "NhCompositeUnsavedValue")

            End If
        End Set
    End Property





    '------------------------


    <Category("Shape, Layout"), _
        Description("The position of the top-left corner of this class shape on the diagram."), _
        DisplayName("Location"), _
        DefaultValue("")> Public Property Location() As Point
        Get
            Return m_UmlClass.Location
        End Get
        Set(ByVal Value As Point)
            m_UmlClass.Location = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "Location")
        End Set
    End Property

    <Category("Shape, Layout"), _
        Description("The size of this class shape in pixels"), _
        DisplayName("Size"), _
        DefaultValue("")> Public Property Size() As Size
        Get
            Return m_UmlClass.Size
        End Get
        Set(ByVal Value As Size)
            m_UmlClass.Size = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "Size")
        End Set
    End Property

    '<Category("Layout"), _
    '    Description(""), _
    '    DisplayName(""), _
    '    DefaultValue("")> Public Property Selected() As Boolean
    '    Get
    '        Return m_UmlClass.Selected
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        m_UmlClass.Selected = Value
    '        RaiseEvent AfterPropertySet(m_UmlClass, "Selected")
    '    End Set
    'End Property

    <Category("Shape, Behavior"), _
        Description("Indicates whether the class shape will hide or display its members"), _
        DisplayName("Collapsed"), _
        DefaultValue("")> Public Property Collapsed() As Boolean
        Get
            Return m_UmlClass.Collapsed
        End Get
        Set(ByVal Value As Boolean)
            m_UmlClass.Collapsed = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "Collapsed")
        End Set
    End Property



    '<Category("Shape, Behavior"), _
    '    Description("The methods that you want to display in this class shape."), _
    '    DisplayName("Methods"), _
    '    DefaultValue("")> Public Property Methods() As ArrayList
    '    Get
    '        Return m_UmlClass.Methods
    '    End Get
    '    Set(ByVal Value As ArrayList)
    '        m_UmlClass.Methods = Value
    '        RaiseEvent AfterPropertySet(m_UmlClass, "Methods")
    '    End Set
    'End Property

    <Category("Shape, Behavior"), _
        Description("The methods that you want to display in this class shape."), _
        DisplayName("Methods"), _
        DefaultValue("")> Public Property Methods() As String()
        Get
            Return m_UmlClass.Methods
        End Get
        Set(ByVal Value As String())
            m_UmlClass.Methods = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "Methods")
        End Set
    End Property

    <Category("Shape, Behavior"), _
        Description("The number of sockets per side of this class shape that lines can attach to."), _
        DisplayName("Sockets per side"), _
        DefaultValue("")> Public Property SlotsPerSide() As Integer
        Get
            Return m_UmlClass.SlotsPerSide
        End Get
        Set(ByVal Value As Integer)
            m_UmlClass.SlotsPerSide = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "SlotsPerSide")
        End Set
    End Property

    <Category("Shape, Behavior"), _
        Description("Indicates whether appearance settings for this class shape are inherited from the diagram or if the diagram settings should be overridden."), _
        DisplayName("Inherit appearance settings from diagram"), _
        DefaultValue("")> Public Property InheritSettings() As Boolean
        Get
            Return m_UmlClass.InheritSettings
        End Get
        Set(ByVal Value As Boolean)
            m_UmlClass.InheritSettings = Value
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_UmlClass, "InheritSettings")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The color used for drawing the lines in this class shape."), _
        DisplayName("ForeColor"), _
        DefaultValue("")> Public Property ForeColor() As Color
        Get
            Return m_UmlClass.ForeColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlClass.ForeColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlClass, "ForeColor")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The first color used for drawing this class shape"), _
        DisplayName("BackColor1"), _
        DefaultValue("")> Public Property BackColor1() As Color
        Get
            Return m_UmlClass.BackColor1.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlClass.BackColor1.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlClass, "BackColor1")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The second color used for drawing this class shape"), _
        DisplayName("BackColor2"), _
        DefaultValue("")> Public Property BackColor2() As Color
        Get
            Return m_UmlClass.BackColor2.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlClass.BackColor2.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlClass, "BackColor2")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The brush used when draing this class shape."), _
        DisplayName("Brush"), _
        DefaultValue("")> Public Property BgBrushStyle() As BrushEnum
        Get
            Return m_UmlClass.BgBrushStyle
        End Get
        Set(ByVal Value As BrushEnum)
            m_UmlClass.BgBrushStyle = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "BgBrushStyle")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The gradient mode used when drawing this class shape. Applies only if the GradientBrush brush has been selected."), _
        DisplayName("Gradient mode"), _
        DefaultValue("")> Public Property BgGradientMode() As LinearGradientMode
        Get
            Return m_UmlClass.BgGradientMode
        End Get
        Set(ByVal Value As LinearGradientMode)
            m_UmlClass.BgGradientMode = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "BgGradientMode")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The font used for displaying text in this class shape."), _
        DisplayName("Font"), _
        DefaultValue("")> Public Property Font() As Font
        Get
            Return New Font(m_UmlClass.FontFamily, m_UmlClass.FontSize, m_UmlClass.FontStyle)
        End Get
        Set(ByVal Value As Font)
            m_UmlClass.FontFamily = Value.FontFamily.Name
            m_UmlClass.FontSize = Value.Size
            m_UmlClass.FontStyle = Value.Style
            RaiseEvent AfterPropertySet(m_UmlClass, "Font")
        End Set
    End Property


    '<Category("Shape, Appearance"), _
    '    Description(""), _
    '    DisplayName(""), _
    '    DefaultValue("")> Public Property FontSize() As Single
    '    Get
    '        Return m_UmlClass.FontSize
    '    End Get
    '    Set(ByVal Value As Single)
    '        m_UmlClass.FontSize = Value
    '        RaiseEvent AfterPropertySet(m_UmlClass, "FontSize")
    '    End Set
    'End Property

    '<Category("Shape, Appearance"), _
    '    Description(""), _
    '    DisplayName(""), _
    '    DefaultValue("")> Public Property FontFamily() As String
    '    Get
    '        Return m_UmlClass.FontFamily
    '    End Get
    '    Set(ByVal Value As String)
    '        m_UmlClass.FontFamily = Value
    '        RaiseEvent AfterPropertySet(m_UmlClass, "FontFamily")
    '    End Set
    'End Property

    <Category("Shape, Appearance"), _
        Description("The color used for text displayed in this class shape."), _
        DisplayName("Font color"), _
        DefaultValue("")> Public Property FontColor() As Color
        Get
            Return m_UmlClass.FontColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlClass.FontColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlClass, "FontColor")
        End Set
    End Property

    '<Category("Shape, Appearance"), _
    '    Description(""), _
    '    DisplayName(""), _
    '    DefaultValue("")> Public Property FontStyle() As FontStyle
    '    Get
    '        Return m_UmlClass.FontStyle
    '    End Get
    '    Set(ByVal Value As FontStyle)
    '        m_UmlClass.FontStyle = Value
    '        RaiseEvent AfterPropertySet(m_UmlClass, "FontStyle")
    '    End Set
    'End Property

    <Category("Shape, Behavior"), _
    Description("Indicates whether inherited properties are displayed in the class shape"), _
    DisplayName("Display inherited properties"), _
    DefaultValue(False)> Public Property DisplayInheritedProperties() As Boolean
        Get
            Return m_UmlClass.DisplayInheritedProperties
        End Get
        Set(ByVal Value As Boolean)
            m_UmlClass.DisplayInheritedProperties = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "DisplayInheritedProperties")
        End Set
    End Property

    <Category("Shape, Behavior"), _
        Description("Indicates whether reference properties that are displayed with lines in the diagram will also be displayed in the class shape."), _
        DisplayName("Display reference properties"), _
        DefaultValue(False)> Public Property DisplayLineProperties() As Boolean
        Get
            Return m_UmlClass.DisplayLineProperties
        End Get
        Set(ByVal Value As Boolean)
            m_UmlClass.DisplayLineProperties = Value
            RaiseEvent AfterPropertySet(m_UmlClass, "DisplayLineProperties")
        End Set
    End Property

    '----------------------




    Private Sub RemoveShadowProperties()
        Dim classMap As IClassMap = m_UmlClass.GetClassMap
        If Not classMap Is Nothing Then
            Dim propertyMap As IPropertyMap
            Dim inheritedNames As New Hashtable
            Dim nonInheritedPropertyMaps As ArrayList
            Dim superClassMap As IClassMap = classMap.GetInheritedClassMap

            If Not superClassMap Is Nothing Then

                For Each propertyMap In superClassMap.GetAllPropertyMaps

                    inheritedNames(LCase(propertyMap.Name)) = True

                Next

                nonInheritedPropertyMaps = classMap.GetNonInheritedPropertyMaps

                For Each propertyMap In nonInheritedPropertyMaps

                    If inheritedNames.ContainsKey(LCase(propertyMap.Name)) Then

                        classMap.PropertyMaps.Remove(propertyMap)

                    End If

                Next

            End If
        End If

    End Sub


    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Dim classMap As IClassMap = m_UmlClass.GetClassMap
        If Not classMap Is Nothing Then

            Select Case propDesc.Name

                Case "IsReadOnly"
                    If classMap.DomainMap.IsReadOnly Then propDesc.SetReadOnly()

                Case "TypeColumn"
                    If classMap.InheritanceType = InheritanceType.None Then propDesc.SetReadOnly()

                Case "TypeValue"
                    If classMap.InheritanceType = InheritanceType.None Then propDesc.SetReadOnly()

                Case "IsAbstract"
                    If classMap.InheritanceType = InheritanceType.None Then propDesc.SetReadOnly()

            End Select

        Else

            Select Case propDesc.Name

                Case "Name", "NameSpaceName"


                Case Else

                    propDesc.SetReadOnly()

            End Select

        End If

        Select Case propDesc.Name

            Case "ForeColor", "BackColor1", "BackColor2", "BgBrushStyle", "BgGradientMode", "Font", "FontColor", "DisplayInheritedProperties", "DisplayLineProperties"

                If m_UmlClass.InheritSettings Then propDesc.SetReadOnly()

            Case Else

        End Select

        Return propDesc

    End Function


End Class

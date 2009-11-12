Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.ComponentModel
Imports Puzzle.ObjectMapper.GUI.Uml
Imports System.Drawing.Design

Public Class UmlLineEndProperties
    Inherits PropertiesBase

    Private m_UmlLine As UmlLine
    Private m_IsStart As Boolean

    Public Event BeforePropertySet(ByVal mapObject As Object, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As Object, ByVal propertyName As String)

    Public Function GetUmlLine() As UmlLine
        Return m_UmlLine
    End Function

    Public Sub SetUmlLine(ByVal value As UmlLine)
        m_UmlLine = value
    End Sub

    Public Function GetIsStart() As Boolean
        Return m_IsStart
    End Function

    Public Sub SetIsStart(ByVal value As Boolean)
        m_IsStart = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_UmlLine

    End Function

    Public Function GetPropertyMap() As IPropertyMap
        If m_IsStart Then
            Return m_UmlLine.GetEndPropertyMap
        Else
            Return m_UmlLine.GetStartPropertyMap
        End If
    End Function

    <Category("Design"), _
    Description("The name of the property represented by this line."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Dim propertyMap As IPropertyMap = GetpropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.Name
            Else
                If m_IsStart Then
                    Return m_UmlLine.EndProperty
                Else
                    Return m_UmlLine.StartProperty
                End If
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetpropertyMap()
            If Not propertyMap Is Nothing Then
                RaiseEvent BeforePropertySet(propertyMap, "Name", Value, propertyMap.Name)
                propertyMap.UpdateName(Value)
                RaiseEvent AfterPropertySet(propertyMap, "Name")
            Else
                If m_IsStart Then
                    m_UmlLine.EndProperty = Value
                    RaiseEvent AfterPropertySet(m_UmlLine, "EndProperty")
                Else
                    m_UmlLine.StartProperty = Value
                    RaiseEvent AfterPropertySet(m_UmlLine, "StartProperty")
                End If
            End If
        End Set
    End Property


    <Category("Validation"), _
    Description("The name of the validation method for this property. (Must be a method that returns void and takes no parameters or one IList parameter with exceptions)."), _
    DisplayName("Validate method"), _
    DefaultValue("")> Public Property ValidateMethod() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.ValidateMethod
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Value <> propertyMap.ValidateMethod Then
                    propertyMap.ValidateMethod = Value
                    RaiseEvent AfterPropertySet(propertyMap, "ValidateMethod")
                End If
            End If
        End Set
    End Property



    <Category("Design"), _
        Description("The accessibility of this property."), _
        DisplayName("Accessibility"), _
        DefaultValue("")> Public Property Accessibility() As AccessibilityType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.Accessibility
            End If
        End Get
        Set(ByVal Value As AccessibilityType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.Accessibility = Value
                RaiseEvent AfterPropertySet(propertyMap, "Accessibility")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("The modifier that will be used for your property. The default setting of Default will correspond to Virtual for NPersist, while it corresponds to None for NHibernate."), _
        DisplayName("Modifier"), _
        DefaultValue(PropertyModifier.Default)> Public Property Modifier() As PropertyModifier
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.PropertyModifier
            End If
        End Get
        Set(ByVal Value As PropertyModifier)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.PropertyModifier = Value
                RaiseEvent AfterPropertySet(propertyMap, "PropertyModifier")
            End If
        End Set
    End Property

    <Category("Field"), _
        Description("The name of the field that this property is mapping to."), _
        DisplayName("Field name"), _
        DefaultValue("")> Public Property FieldName() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.FieldName
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.FieldName = Value
                RaiseEvent AfterPropertySet(propertyMap, "FieldName")
            End If
        End Set
    End Property

    <Category("Field"), _
    Description("The accessibility of the field that this property is mapping to."), _
    DisplayName("Field accessibility"), _
    DefaultValue("")> Public Property FieldAccessibility() As AccessibilityType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.FieldAccessibility
            End If
        End Get
        Set(ByVal Value As AccessibilityType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.FieldAccessibility = Value
                RaiseEvent AfterPropertySet(propertyMap, "FieldAccessibility")
            End If
        End Set
    End Property

    <Category("Design"), _
        TypeConverter(GetType(DataTypeConverter)), _
        Description("The data type of this property. For collection properties, you should specify the data type of the collection items in the item type setting."), _
        DisplayName("Data type"), _
        DefaultValue("")> Public Property DataType() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.DataType
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.DataType = Value
                If Not Value = "" Then
                    If HasReferenceDataType() Then
                        If propertyMap.ReferenceType = ReferenceType.None Then
                            If propertyMap.IsCollection Then
                                propertyMap.ReferenceType = ReferenceType.ManyToMany
                            Else
                                propertyMap.ReferenceType = ReferenceType.OneToMany
                            End If
                        End If
                        DialogueServices.AskToCreateIfInverseNotExist(propertyMap.Inverse, propertyMap)
                    Else
                        If Not propertyMap.ReferenceType = ReferenceType.None Then
                            propertyMap.ReferenceType = ReferenceType.None
                        End If
                    End If
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "DataType")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("The default value for the private field that holds the value for this property. For a new instance of a non-persistent object (such as an ArrayList for a collection property), just enter 'New' (without the enclosing quote marks)"), _
        DisplayName("Default value"), _
        DefaultValue("")> Public Property DefaultValue() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.DefaultValue
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.DefaultValue = Value
                RaiseEvent AfterPropertySet(propertyMap, "DefaultValue")
            End If
        End Set
    End Property



    <Category("Design"), _
        Description("If you set lazy loading to True, the property will not be loaded with a value from the data source until the first time it is accessed."), _
        DisplayName("Lazy loading"), _
        DefaultValue(False)> Public Property LazyLoad() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.LazyLoad
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.LazyLoad = Value
                RaiseEvent AfterPropertySet(propertyMap, "LazyLoad")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("Set to True for read-only persistence."), _
        DisplayName("Read only"), _
        DefaultValue(False)> Public Property IsReadOnly() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsReadOnly
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.IsReadOnly = Value
                RaiseEvent AfterPropertySet(propertyMap, "IsReadOnly")
            End If
        End Set
    End Property

    <Category("Inverse"), _
        Description("Set to True for an inverse property that should not have its values saved to the database since its inverse property will manage all persistence. Normally, when mapping to a relational database, one of the properties in an inverse property relationship should be set to slave (in a OneMany relationship the list property side should be set to slave)"), _
        DisplayName("Slave"), _
        DefaultValue(False)> Public Property IsSlave() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsSlave
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.IsSlave = Value
                RaiseEvent AfterPropertySet(propertyMap, "IsSlave")
            End If
        End Set
    End Property


    <Category("Caching"), _
        Description("The value in seconds that values of this property may stay cached before going stale and in need of a reload. The default value of -1 means the time to live value of the class map will be used."), _
        DisplayName("Time to live"), _
        DefaultValue(-1)> Public Property TimeToLive() As Integer
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.TimeToLive
            End If
        End Get
        Set(ByVal Value As Integer)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Value <> propertyMap.TimeToLive Then
                    propertyMap.TimeToLive = Value
                    RaiseEvent AfterPropertySet(propertyMap, "TimeToLive")
                End If
            End If
        End Set
    End Property

    <Category("Caching"), _
        Description("The behavior specifying how values of this property may stay cached before going stale (according to the timespan specified in time to live) and in need of a reload. The default value of means the time to live behavior of the class map will be used."), _
        DisplayName("Time to live behavior"), _
        DefaultValue(TimeToLiveBehavior.Default)> Public Property TheTimeToLiveBehavior() As TimeToLiveBehavior
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.TimeToLiveBehavior
            End If
        End Get
        Set(ByVal Value As TimeToLiveBehavior)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Value <> propertyMap.TimeToLiveBehavior Then
                    propertyMap.TimeToLiveBehavior = Value
                    RaiseEvent AfterPropertySet(propertyMap, "TimeToLiveBehavior")
                End If
            End If
        End Set
    End Property

    <Category("Reference property"), _
        Description("The type of reference represented by this property."), _
        DisplayName("Reference type"), _
        DefaultValue(False)> Public Property ReferenceType() As ReferenceType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.ReferenceType
            End If
        End Get
        Set(ByVal Value As ReferenceType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.ReferenceType = Value
                If Not Value = ReferenceType.None Then
                    Select Case Value

                        Case ReferenceType.ManyToMany

                            If Not propertyMap.IsCollection Then
                                propertyMap.IsCollection = True
                            End If

                        Case ReferenceType.ManyToOne

                            If Not propertyMap.IsCollection Then
                                propertyMap.IsCollection = True
                            End If

                            propertyMap.IsSlave = True

                        Case Else

                            If propertyMap.IsCollection Then
                                propertyMap.IsCollection = False
                            End If

                    End Select
                Else
                    propertyMap.CascadingCreate = False
                    propertyMap.CascadingDelete = False
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "ReferenceType")
            End If
        End Set
    End Property


    <Category("Reference property"), _
        Description("When True, a new instance of the class referenced by this property will be created and referenced when a new object of the class this property belongs to is created. Applies only to reference properties."), _
        DisplayName("Cascading create"), _
        DefaultValue(False)> Public Property CascadingCreate() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.CascadingCreate
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.CascadingCreate = Value
                RaiseEvent AfterPropertySet(propertyMap, "CascadingCreate")
            End If
        End Set
    End Property

    <Category("Reference property"), _
        Description("When True, all instances referenced by this property will be deleted when a the object that this property belongs to is deleted. Applies only to reference properties."), _
        DisplayName("Cascading delete"), _
        DefaultValue(False)> Public Property CascadingDelete() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.CascadingDelete
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.CascadingDelete = Value
                RaiseEvent AfterPropertySet(propertyMap, "CascadingDelete")
            End If
        End Set
    End Property

    <Category("Collection property"), _
        Description("Set to true if the property accepts a collection of values."), _
        DisplayName("Collection"), _
        DefaultValue(False)> Public Property IsCollection() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsCollection
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.IsCollection = Value
                If Value Then
                    propertyMap.ItemType = propertyMap.DataType
                    propertyMap.DataType = "System.Collections.ArrayList"
                    'propertyMap.DefaultValue = "New System.Collections.ArrayList()"
                    Select Case propertyMap.ReferenceType
                        Case ReferenceType.OneToMany
                            propertyMap.ReferenceType = ReferenceType.ManyToMany
                        Case ReferenceType.OneToOne
                            propertyMap.ReferenceType = ReferenceType.ManyToOne
                    End Select
                    If propertyMap.IsSlave = False And propertyMap.ReferenceType = ReferenceType.ManyToMany And Len(propertyMap.Inverse) > 0 And propertyMap.NoInverseManagement = False Then
                        propertyMap.DataType = "Puzzle.NPersist.Framework.ManagedList"
                        'propertyMap.DefaultValue = "New Puzzle.NPersist.Framework.ManagedList(Me, """ & propertyMap.Name & """)"
                    End If
                Else
                    propertyMap.DataType = propertyMap.ItemType
                    propertyMap.DefaultValue = ""
                    propertyMap.ItemType = ""
                    Select Case propertyMap.ReferenceType
                        Case ReferenceType.ManyToMany
                            propertyMap.ReferenceType = ReferenceType.OneToMany
                        Case ReferenceType.ManyToOne
                            propertyMap.ReferenceType = ReferenceType.OneToOne
                    End Select
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "IsCollection")
            End If
        End Set
    End Property

    <Category("Collection property"), _
        TypeConverter(GetType(ItemTypeConverter)), _
        Description("The data type of the items in this property. Applies to collection properties only."), _
        DisplayName("Item type"), _
        DefaultValue("")> Public Property ItemType() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.ItemType
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.ItemType = Value
                If Not Value = "" Then
                    If HasReferenceDataType() Then
                        If propertyMap.ReferenceType = ReferenceType.None Then
                            If propertyMap.IsCollection Then
                                propertyMap.ReferenceType = ReferenceType.ManyToMany
                            Else
                                propertyMap.ReferenceType = ReferenceType.OneToMany
                            End If
                        End If
                        DialogueServices.AskToCreateIfInverseNotExist(propertyMap.Inverse, propertyMap)
                    Else
                        If Not propertyMap.ReferenceType = ReferenceType.None Then
                            propertyMap.ReferenceType = ReferenceType.None
                        End If
                    End If
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "ItemType")
            End If
        End Set
    End Property


    <Category("Collection property"), _
        TypeConverter(GetType(OrderByConverter)), _
        Description("The name of the property in the referenced class that referenced in this property should be sorted by."), _
        DisplayName("Order by"), _
        DefaultValue("")> Public Property OrderBy() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.OrderBy
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.OrderBy = Value
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "OrderBy")
            End If
        End Set
    End Property


    <Category("Identity"), _
        Description("Set to true if this is an identity property. An identity property is a property that by itself or together with other identity properties (composite key) can be used to uniquely identify the object."), _
        DisplayName("Identity"), _
        DefaultValue(False)> Public Property IsIdentity() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsIdentity
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.IsIdentity Then
                    If Value Then
                        propertyMap.IdentityIndex = propertyMap.ClassMap.GetIdentityPropertyMaps.Count
                    Else
                        propertyMap.IdentityIndex = 0
                    End If
                    propertyMap.IsIdentity = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "IsIdentity")
            End If
        End Set
    End Property


    <Category("Key"), _
        Description("Set to true if this is a key property. An key property is a property that by itself or together with other key properties can be used to identify the object as uniquely as is required for human identification of the object and can be useful for display purposes (An example would be the FirstName and LastName properties of a Person object)."), _
        DisplayName("Key"), _
        DefaultValue(False)> Public Property IsKey() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsKey
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.IsKey Then
                    If Value Then
                        propertyMap.KeyIndex = propertyMap.ClassMap.GetKeyPropertyMaps.Count
                    Else
                        propertyMap.KeyIndex = 0
                    End If
                    propertyMap.IsKey = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "IsKey")
            End If
        End Set
    End Property


    <Category("Key"), _
        Description("The index of the key property. Key properties will be concatenated into a string object key in the order specified by the key indexes."), _
        DisplayName("Key index"), _
        DefaultValue(0)> Public Property KeyIndex() As Integer
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.KeyIndex
            End If
        End Get
        Set(ByVal Value As Integer)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.KeyIndex = Value
                RaiseEvent AfterPropertySet(propertyMap, "KeyIndex")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("Set to true if this is property may contain null values. (Note that whenever a working mapping to a column has been set up, the nullability is read from the AllowNulls status of that column)."), _
        DisplayName("Nullable"), _
        DefaultValue(False)> Public Property IsNullable() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsNullable
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.IsNullable Then
                    propertyMap.IsNullable = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "IsNullable")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("Set to true if this is property should get its values assigned to it by the data source (for example by an auto increaser column)."), _
        DisplayName("Assigned by source"), _
        DefaultValue(False)> Public Property IsAssignedBySource() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IsAssignedBySource
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.IsAssignedBySource Then
                    propertyMap.IsAssignedBySource = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "IsAssignedBySource")
            End If
        End Set
    End Property

    <Category("Validation"), _
        Description("Specify a maximum length for string values (maximum number of elements for list properties) or leave as -1 for no limit. (Note that whenever a working mapping to a column has been set up for a string property, the length for the property is read from that column)."), _
        DisplayName("Max length"), _
        DefaultValue(-1)> Public Property MaxLength() As Long
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.MaxLength
            End If
        End Get
        Set(ByVal Value As Long)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.MaxLength Then
                    propertyMap.MaxLength = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "MaxLength")
            End If
        End Set
    End Property


    <Category("Validation"), _
        Description("Specify a minimum length for string values (minimum number of elements for list properties) or leave as -1 for no limit."), _
        DisplayName("Min length"), _
        DefaultValue(-1)> Public Property MinLength() As Long
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.MinLength
            End If
        End Get
        Set(ByVal Value As Long)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.MinLength Then
                    propertyMap.MinLength = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "MinLength")
            End If
        End Set
    End Property


    <Category("Validation"), _
        Description("Specify a maximum value for primitive values or leave blank for no limit."), _
        DisplayName("Max value"), _
        DefaultValue("")> Public Property MaxValue() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.MaxValue
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.MaxValue Then
                    propertyMap.MaxValue = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "MaxValue")
            End If
        End Set
    End Property

    <Category("Validation"), _
        Description("Specify a minimum value for primitive values or leave blank for no limit."), _
        DisplayName("Min value"), _
        DefaultValue("")> Public Property MinValue() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.MinValue
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                If Not Value = propertyMap.MinValue Then
                    propertyMap.MinValue = Value
                End If
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "MinValue")
            End If
        End Set
    End Property

    <Category("Identity"), _
        Description("The index of the identity property. Identity properties will be concatenated into a string object identity in the order specified by the identity indexes."), _
        DisplayName("Identity index"), _
        DefaultValue(0)> Public Property IdentityIndex() As Integer
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IdentityIndex
            End If
        End Get
        Set(ByVal Value As Integer)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.IdentityIndex = Value
                RaiseEvent AfterPropertySet(propertyMap, "IdentityIndex")
            End If
        End Set
    End Property


    <Category("Inverse"), _
        Description("The name of this property's inverse property. Use only with bi-directional reference properties. (Example: For an OrderLine.Order property, Order.OrderLines would be the inverse property!)"), _
        DisplayName("Inverse property name"), _
        DefaultValue("")> Public Property Inverse() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.Inverse
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.Inverse = Value
                DialogueServices.AskToCreateIfInverseNotExist(Value, propertyMap)
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "Inverse")
            End If
        End Set
    End Property

    <Category("Inverse"), _
        Description("Set to True for inverse properties with custom inverse management. This will turn off the standard inverse management for the property."), _
        DisplayName("Custom inverse management"), _
        DefaultValue(False)> Public Property NoInverseManagement() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.NoInverseManagement
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.NoInverseManagement = Value
                RaiseEvent AfterPropertySet(propertyMap, "NoInverseManagement")
            End If
        End Set
    End Property

    <Category("Inverse"), _
        Description("Set to True in order for this property to inherit its mapping settings from its inverse property."), _
        DisplayName("Inherit mappings from inverse"), _
        DefaultValue(False)> Public Property InheritInverseMappings() As Boolean
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.InheritInverseMappings
            End If
        End Get
        Set(ByVal Value As Boolean)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim inv As IPropertyMap = propertyMap.GetInversePropertyMap()
                If inv.InheritInverseMappings Then inv.InheritInverseMappings = False
                propertyMap.InheritInverseMappings = Value
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "InheritInverseMappings")
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(SourceConverter)), _
        Description("The name of the data source that this property is mapping to. Leave blank in order to use the data source that the class is mapping to."), _
        DisplayName("Source name"), _
        DefaultValue("")> Public Property Source() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.Source
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.Source = Value
                RaiseEvent AfterPropertySet(propertyMap, "Source")
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(TableConverter)), _
        Description("The name of the table that this property is mapping to. Leave blank to use the table that the class is mapping to."), _
        DisplayName("Table name"), _
        DefaultValue("")> Public Property Table() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.Table
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.Table = Value
                DialogueServices.AskToCreateIfTableNotExist(Value, propertyMap.GetSourceMap)
                SetShouldReloadProperties()
                RaiseEvent AfterPropertySet(propertyMap, "Table")
            End If
        End Set
    End Property


    <Category("O/R Mapping"), _
        Description("The value that will be returned by the property if the value in the column is Null."), _
        DisplayName("Null value substitute"), _
        DefaultValue("")> Public Property NullSubstitute() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.NullSubstitute
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.NullSubstitute = Value
                RaiseEvent AfterPropertySet(propertyMap, "NullSubstitute")
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(ColumnConverter)), _
        Description("The name of the column that this property is mapping to."), _
        DisplayName("Column name"), _
        DefaultValue("")> Public Property Column() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.Column
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.Column = Value
                DialogueServices.AskToCreateIfColumnNotExist(Value, propertyMap.GetTableMap)
                RaiseEvent AfterPropertySet(propertyMap, "Column")
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        Editor(GetType(AdditionalColumnsEditor), GetType(UITypeEditor)), _
        Description("The name of any additional columns that this property is mapping to. Applies only to reference properties."), _
        DisplayName("Additional columns"), _
        DefaultValue("")> Public Property AdditionalColumns() As String()
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.AdditionalColumns.ToArray(GetType(String))
            End If
        End Get
        Set(ByVal Value As String())
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim name As String
                propertyMap.AdditionalColumns.Clear()
                For Each name In Value
                    propertyMap.AdditionalColumns.Add(name)
                Next
                '            DialogueServices.AskToCreateIfColumnNotExist(Value, propertyMap.GetTableMap)
                RaiseEvent AfterPropertySet(propertyMap, "AdditionalColumns")
            End If
        End Set
    End Property


    <Category("O/R Mapping"), _
        TypeConverter(GetType(ColumnConverter)), _
        Description("The name of the id column that this property is mapping to."), _
        DisplayName("Id column name"), _
        DefaultValue("")> Public Property IdColumn() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.IdColumn
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.IdColumn = Value
                DialogueServices.AskToCreateIfColumnNotExist(Value, propertyMap.GetTableMap)
                RaiseEvent AfterPropertySet(propertyMap, "IdColumn")
            End If
        End Set
    End Property

    <Category("O/R Mapping"), _
        Editor(GetType(AdditionalIdColumnsEditor), GetType(UITypeEditor)), _
        Description("The name of any additional columns that this property is mapping to. Applies only to reference properties."), _
        DisplayName("Additional id columns"), _
        DefaultValue("")> Public Property AdditionalIdColumns() As String()
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.AdditionalIdColumns.ToArray(GetType(String))
            End If
        End Get
        Set(ByVal Value As String())
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim name As String
                propertyMap.AdditionalIdColumns.Clear()
                For Each name In Value
                    propertyMap.AdditionalIdColumns.Add(name)
                Next
                '            DialogueServices.AskToCreateIfColumnNotExist(Value, propertyMap.GetTableMap)
                RaiseEvent AfterPropertySet(propertyMap, "AdditionalIdColumns")
            End If
        End Set
    End Property


    <Category("O/O Mapping"), _
        TypeConverter(GetType(SourcePropertyConverter)), _
        Description("The name of the source property that this class is mapping to."), _
        DisplayName("Source property"), _
        DefaultValue("")> Public Property SourceProperty() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.SourceProperty
            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.SourceProperty = Value
                'DialogueServices.AskToCreateIfPropertyNotExist(Value, m_ClassMap.GetSourceClassMap)
                RaiseEvent AfterPropertySet(propertyMap, "SourceProperty")
            End If
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to this property when the object is updated. If set to DefaultBehavior then the behavior will be inherited from the corresponding setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting update behavior for the property will become IncludeWhenDirty except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Update optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property UpdateOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.UpdateOptimisticConcurrencyBehavior
            End If
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.UpdateOptimisticConcurrencyBehavior = Value
                RaiseEvent AfterPropertySet(propertyMap, "UpdateOptimisticConcurrencyBehavior")
            End If
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to this property when the object is deleted. If set to DefaultBehavior then the behavior will be inherited from the corresponding setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting delete behavior for the property will become IncludeWhenLoaded except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Delete optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property DeleteOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.DeleteOptimisticConcurrencyBehavior
            End If
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.DeleteOptimisticConcurrencyBehavior = Value
                RaiseEvent AfterPropertySet(propertyMap, "DeleteOptimisticConcurrencyBehavior")
            End If
        End Set
    End Property

    <Category("Special property behavior"), _
        Description("Select a special behavior that is applied to your property when the object is created. For example, 'SetDateTime' could be used for a 'CreatedAt' property. 'Increase' applies only to properties mapping to numeric columns. 'SetDateTime' applies only to properties mapping to DateTime columns."), _
        DisplayName("Create behavior"), _
        DefaultValue(PropertySpecialBehaviorType.None)> Public Property OnCreateBehavior() As PropertySpecialBehaviorType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.OnCreateBehavior
            End If
        End Get
        Set(ByVal Value As PropertySpecialBehaviorType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.OnCreateBehavior = Value
                RaiseEvent AfterPropertySet(propertyMap, "OnCreateBehavior")
            End If
        End Set
    End Property

    <Category("Special property behavior"), _
        Description("Select a special behavior that is applied to your property when the object is persisted. For example, 'Increase' could be used for a property mapping to a version column while 'SetDateTime' could be used for a 'LastUpdatedAt' property. 'Increase' applies only to properties mapping to numeric columns. 'SetDateTime' applies only to properties mapping to DateTime columns."), _
        DisplayName("Persist behavior"), _
        DefaultValue(PropertySpecialBehaviorType.None)> Public Property OnPersistBehavior() As PropertySpecialBehaviorType
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Return propertyMap.OnPersistBehavior
            End If
        End Get
        Set(ByVal Value As PropertySpecialBehaviorType)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                propertyMap.OnPersistBehavior = Value
                RaiseEvent AfterPropertySet(propertyMap, "OnPersistBehavior")
            End If
        End Set
    End Property


    '------------------------------------
    'NHibernate
    '------------------------------------


    <Category("NHibernate"), _
        Description("An identity property value indicating that an instance is newly instantiated (unsaved), distinguishing it from transient instances that were saved or loaded in a previous session. (NHibernate specific)."), _
        DisplayName("NH unsaved-value"), _
        DefaultValue("")> Public Property NhIdUnsavedValue() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-unsaved-value"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-unsaved-value"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhPersister")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("The name of the class generating the new value for this (identity) property for a newly created object. Applies to identity properties only. (NHibernate specific)."), _
        DisplayName("NH generator class"), _
        DefaultValue("")> Public Property NhGeneratorClass() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-generator-class"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-generator-class"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhGeneratorClass")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("A list of parameters and values sent to the class generating the new value for this (identity) property, using a comma delimited string of name=value pairs (ex: table=uid_table,column=next_hi_value_column). (NHibernate specific)."), _
        DisplayName("NH generator parameters"), _
        DefaultValue("")> Public Property NhGeneratorParams() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-generator-parameters"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-generator-parameters"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhGeneratorParams")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether the mapped columns should be included in Sql Update statements. (NHibernate specific)."), _
        DisplayName("NH update"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhUpdate() As NullableBool
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-update"
                meta = propertyMap.GetMetaData(key)
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
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-update"
                Select Case Value
                    Case NullableBool.Unspecified
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case NullableBool.True
                        propertyMap.SetMetaData(key, "true")

                    Case NullableBool.False
                        propertyMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhUpdate")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether the mapped columns should be included in Sql Insert statements. (NHibernate specific)."), _
        DisplayName("NH insert"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhInsert() As NullableBool
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-insert"
                meta = propertyMap.GetMetaData(key)
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
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-insert"
                Select Case Value
                    Case NullableBool.Unspecified
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case NullableBool.True
                        propertyMap.SetMetaData(key, "true")

                    Case NullableBool.False
                        propertyMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhInsert")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Sql expression defining the value for a computed property. (NHibernate specific)."), _
        DisplayName("NH formula"), _
        DefaultValue("")> Public Property NhFormula() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-formula"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-formula"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhFormula")

            End If
        End Set
    End Property



    <Category("NHibernate"), _
        Description("Specifies which operations should be cascaded from the parent object to the associated object. (NHibernate specific)."), _
        DisplayName("NH cascade"), _
        DefaultValue(NhCascadeEnum.Unspecified)> Public Property NhCascade() As NhCascadeEnum
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-cascade"
                meta = propertyMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "all"
                        Return NhCascadeEnum.All
                    Case "none"
                        Return NhCascadeEnum.None
                    Case "save-update"
                        Return NhCascadeEnum.SaveUpdate
                    Case "delete"
                        Return NhCascadeEnum.Delete
                    Case Else
                        Return NhCascadeEnum.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NhCascadeEnum)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-cascade"
                Select Case Value
                    Case NhCascadeEnum.Unspecified
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case NhCascadeEnum.All
                        propertyMap.SetMetaData(key, "all")

                    Case NhCascadeEnum.None
                        propertyMap.SetMetaData(key, "none")

                    Case NhCascadeEnum.SaveUpdate
                        propertyMap.SetMetaData(key, "save-update")

                    Case NhCascadeEnum.Delete
                        propertyMap.SetMetaData(key, "delete")

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhCascade")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Enables outer-join fetching for this association when use_outer_join is set. (NHibernate specific)."), _
        DisplayName("NH outer-join"), _
        DefaultValue(NullableBoolWithAuto.Unspecified)> Public Property NhOuterJoin() As NullableBoolWithAuto
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-outer-join"
                meta = propertyMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "auto"
                        Return NullableBoolWithAuto.Auto
                    Case "true"
                        Return NullableBoolWithAuto.True
                    Case "false"
                        Return NullableBoolWithAuto.False
                    Case Else
                        Return NullableBoolWithAuto.Unspecified
                End Select

            End If
        End Get
        Set(ByVal Value As NullableBoolWithAuto)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-outer-join"
                Select Case Value
                    Case NullableBoolWithAuto.Unspecified
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case NullableBoolWithAuto.Auto
                        propertyMap.SetMetaData(key, "auto")

                    Case NullableBoolWithAuto.True
                        propertyMap.SetMetaData(key, "true")

                    Case NullableBoolWithAuto.False
                        propertyMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhOuterJoin")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specify a sorted collection with natural sort order, or a given comparator class. Applies only to collection properties. (NHibernate specific)."), _
        DisplayName("NH sort"), _
        DefaultValue("")> Public Property NhSort() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-sort"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-sort"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhSort")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specify a table column (or columns) that define the iteration order of the collection, together with an optional asc or desc. Applies only to collection properties. (NHibernate specific)."), _
        DisplayName("NH order-by"), _
        DefaultValue("")> Public Property NhOrderBy() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-order-by"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-order-by"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhOrderBy")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specify an arbitrary Sql Where condition to be used when retrieving or removing the collection. Applies only to collection properties. (NHibernate specific)."), _
        DisplayName("NH where"), _
        DefaultValue("")> Public Property NhWhere() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-where"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-where"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhWhere")

            End If
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies the maximum length of strings that this property will accept. (NHibernate specific)."), _
        DisplayName("NH length"), _
        DefaultValue("")> Public Property NhLength() As String
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-length"
                meta = propertyMap.GetMetaData(key)
                Return meta

            End If
        End Get
        Set(ByVal Value As String)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-length"
                Select Case Value
                    Case ""
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case Else
                        propertyMap.SetMetaData(key, Value)

                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhLength")

            End If
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether the property should be lazy loaded. If set to auto, the property will be marked as lazy loading if it is a reference collection property. (NHibernate specific)."), _
        DisplayName("NH lazy"), _
        DefaultValue(NullableBoolWithAuto.Unspecified)> Public Property NhLazy() As NullableBoolWithAuto
        Get
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim meta As String
                Dim key As String = "nh-lazy"
                meta = propertyMap.GetMetaData(key)
                Select Case LCase(meta)
                    Case "auto"
                        Return NullableBoolWithAuto.Auto
                    Case "true"
                        Return NullableBoolWithAuto.True
                    Case "false"
                        Return NullableBoolWithAuto.False
                    Case Else
                        Return NullableBoolWithAuto.Unspecified
                End Select
            End If
        End Get
        Set(ByVal Value As NullableBoolWithAuto)
            Dim propertyMap As IPropertyMap = GetPropertyMap()
            If Not propertyMap Is Nothing Then
                Dim key As String = "nh-lazy"
                Select Case Value
                    Case NullableBoolWithAuto.Unspecified
                        If propertyMap.HasMetaData(key) Then
                            propertyMap.RemoveMetaData(key)
                        End If

                    Case NullableBoolWithAuto.Auto
                        propertyMap.SetMetaData(key, "auto")

                    Case NullableBoolWithAuto.True
                        propertyMap.SetMetaData(key, "true")

                    Case NullableBoolWithAuto.False
                        propertyMap.SetMetaData(key, "false")
                End Select
                RaiseEvent AfterPropertySet(propertyMap, "NhLazy")
            End If
        End Set
    End Property




    <Category("Shape, Appearance"), _
        Description("Indicates whether appearance settings for this line are inherited from the diagram or if the diagram settings should be overridden."), _
        DisplayName("Inherit appearance settings from diagram"), _
        DefaultValue("")> Public Property InheritSettings() As Boolean
        Get
            Return m_UmlLine.InheritSettings
        End Get
        Set(ByVal Value As Boolean)
            m_UmlLine.InheritSettings = Value
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_UmlLine, "InheritSettings")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The color used for drawing this line."), _
        DisplayName("ForeColor"), _
        DefaultValue("")> Public Property ForeColor() As Color
        Get
            Return m_UmlLine.ForeColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlLine.ForeColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlLine, "ForeColor")
        End Set
    End Property



    <Category("Shape, Appearance"), _
        Description("The color used for drawing filled arrowheads."), _
        DisplayName("BackColor1"), _
        DefaultValue("")> Public Property BackColor1() As Color
        Get
            Return m_UmlLine.BackColor1.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlLine.BackColor1.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlLine, "BackColor1")
        End Set
    End Property


    <Category("Shape, Appearance"), _
        Description("The font used for text labels displayed with this line."), _
        DisplayName("Font"), _
        DefaultValue("")> Public Property Font() As Font
        Get
            Return New Font(m_UmlLine.FontFamily, m_UmlLine.FontSize, m_UmlLine.FontStyle)
        End Get
        Set(ByVal Value As Font)
            m_UmlLine.FontFamily = Value.FontFamily.Name
            m_UmlLine.FontSize = Value.Size
            m_UmlLine.FontStyle = Value.Style
            RaiseEvent AfterPropertySet(m_UmlLine, "Font")
        End Set
    End Property

    <Category("Shape, Appearance"), _
        Description("The color used for text labels displayed with this line."), _
        DisplayName("Font color"), _
        DefaultValue("")> Public Property FontColor() As Color
        Get
            Return m_UmlLine.FontColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlLine.FontColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlLine, "FontColor")
        End Set
    End Property



    <Category("Shape, Layout"), _
        Description("The class shape socket that this line end is attached to."), _
        DisplayName("Socket"), _
        DefaultValue("")> Public Property Slot() As Integer
        Get
            If m_IsStart Then
                Return m_UmlLine.StartSlot
            Else
                Return m_UmlLine.EndSlot
            End If
        End Get
        Set(ByVal Value As Integer)
            If m_IsStart Then
                m_UmlLine.StartSlot = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "StartSlot")
            Else
                m_UmlLine.EndSlot = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "EndSlot")
            End If
        End Set
    End Property


    <Category("Shape, Layout"), _
        Description("The class shape side that this line end is attached to."), _
        DisplayName("Side"), _
        DefaultValue("")> Public Property Side() As Direction
        Get
            If m_IsStart Then
                Return m_UmlLine.StartSide
            Else
                Return m_UmlLine.EndSide
            End If
        End Get
        Set(ByVal Value As Direction)
            If m_IsStart Then
                m_UmlLine.StartSide = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "StartSide")
            Else
                m_UmlLine.EndSide = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "EndSide")
            End If
        End Set
    End Property


    <Category("Shape, Layout"), _
        Description("Indicates whether this line end is locked to a class side and socket or if the side and socket are dynamically calculated."), _
        DisplayName("Locked"), _
        DefaultValue(True)> Public Property Fixed() As Boolean
        Get
            If m_IsStart Then
                Return m_UmlLine.FixedStart
            Else
                Return m_UmlLine.FixedEnd
            End If
        End Get
        Set(ByVal Value As Boolean)
            If m_IsStart Then
                m_UmlLine.FixedStart = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "FixedStart")
            Else
                m_UmlLine.FixedEnd = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "FixedEnd")
            End If
        End Set
    End Property


    <Category("Shape, Uml"), _
        Description("Select the UML aggregation symbol that you want to display for this line end."), _
        DisplayName("Aggregation"), _
        DefaultValue(AggregationEnum.None)> Public Property Aggregation() As AggregationEnum
        Get
            If m_IsStart Then
                Return m_UmlLine.StartAggregation
            Else
                Return m_UmlLine.EndAggregation
            End If
        End Get
        Set(ByVal Value As AggregationEnum)
            If m_IsStart Then
                m_UmlLine.StartAggregation = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "StartAggregation")
            Else
                m_UmlLine.EndAggregation = Value
                RaiseEvent AfterPropertySet(m_UmlLine, "EndAggregation")
            End If
        End Set
    End Property


    Private Function HasReferenceDataType() As Boolean

        Dim propertyMap As IPropertyMap = GetPropertyMap()
        If Not propertyMap Is Nothing Then
            Dim className As String
            If propertyMap.IsCollection Then
                className = propertyMap.ItemType
            Else
                className = propertyMap.DataType
            End If
            If Len(className) < 1 Then Return False
            Dim ns As String
            Dim classMap As IClassMap = propertyMap.ClassMap.DomainMap.GetClassMap(className)
            If classMap Is Nothing Then
                ns = propertyMap.ClassMap.GetNamespace()
                If Len(ns) > 0 Then classMap = propertyMap.ClassMap.DomainMap.GetClassMap(ns & "." & className)
            End If
            If Not classMap Is Nothing Then
                Return True
            End If
            Return False
        End If

    End Function



    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Dim propertyMap As IPropertyMap = GetPropertyMap()
        If Not propertyMap Is Nothing Then

            Select Case propDesc.Name

                Case "ReferenceType"
                    If Not HasReferenceDataType() Then propDesc.SetReadOnly()
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "DataType"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "IsCollection"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "ItemType"
                    If Not propertyMap.IsCollection Then propDesc.SetReadOnly()
                    'If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "IdentityIndex"
                    If Not propertyMap.IsIdentity Then
                        propDesc.SetReadOnly()
                    Else
                        If Not propertyMap.ClassMap.GetIdentityPropertyMaps.Count > 1 Then propDesc.SetReadOnly()
                    End If

                Case "IsReadOnly"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()
                    If propertyMap.ClassMap.IsReadOnly Then propDesc.SetReadOnly()

                Case "Inverse"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "NoInverseManagement"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()
                    If Len(propertyMap.Inverse) < 1 Then propDesc.SetReadOnly()
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "InheritInverseMappings"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()
                    If Len(propertyMap.Inverse) < 1 Then propDesc.SetReadOnly()

                Case "AdditionalColumns"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

                Case "AdditionalIdColumns"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

                Case "Source"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "Table"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "Column"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "IdColumn"
                    If Not (Len(propertyMap.Table) > 0 Or propertyMap.IsCollection) Then propDesc.SetReadOnly()
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "AdditionalColumns"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "AdditionalIdColumns"
                    If propertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

                Case "CascadingCreate"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

                Case "CascadingDelete"
                    If propertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

                Case "OrderBy"
                    If Not (propertyMap.ReferenceType = ReferenceType.ManyToMany Or propertyMap.ReferenceType = ReferenceType.ManyToOne) Then propDesc.SetReadOnly()

            End Select

        End If

        Return propDesc

    End Function

End Class

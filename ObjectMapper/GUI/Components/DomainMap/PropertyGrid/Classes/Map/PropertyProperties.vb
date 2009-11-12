Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.ComponentModel
Imports System.Drawing.Design

Public Enum NhCascadeEnum

    Unspecified = 0
    All = 1
    None = 2
    SaveUpdate = 3
    Delete = 4
    AllDeleteOrphan = 5

End Enum

Public Class PropertyProperties
    Inherits PropertiesBase

    Private m_PropertyMap As IPropertyMap

    Public Event BeforePropertySet(ByVal mapObject As IPropertyMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As IPropertyMap, ByVal propertyName As String)

    Public Function GetPropertyMap() As IPropertyMap
        Return m_PropertyMap
    End Function

    Public Sub SetPropertyMap(ByVal value As IPropertyMap)
        m_PropertyMap = value
    End Sub

    Public Overrides Function GetMapObject() As IMap

        Return m_PropertyMap

    End Function

    <Category("Design"), _
        Description("The name of this property."), _
        DisplayName("Name"), _
        DefaultValue("")> Public Property Name() As String
        Get
            Return m_PropertyMap.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_PropertyMap.Name Then
                RaiseEvent BeforePropertySet(m_PropertyMap, "Name", Value, m_PropertyMap.Name)
                m_PropertyMap.UpdateName(Value)
                RaiseEvent AfterPropertySet(m_PropertyMap, "Name")
            End If
        End Set
    End Property

    <Category("Design"), _
        Description("The accessibility of this property."), _
        DisplayName("Accessibility"), _
        DefaultValue("")> Public Property Accessibility() As AccessibilityType
        Get
            Return m_PropertyMap.Accessibility
        End Get
        Set(ByVal Value As AccessibilityType)
            m_PropertyMap.Accessibility = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "Accessibility")
        End Set
    End Property

    <Category("Design"), _
        Description("The modifier that will be used for your property. The default setting of Default will correspond to Virtual."), _
        DisplayName("Modifier"), _
        DefaultValue(PropertyModifier.Default)> Public Property Modifier() As PropertyModifier
        Get
            Return m_PropertyMap.PropertyModifier
        End Get
        Set(ByVal Value As PropertyModifier)
            m_PropertyMap.PropertyModifier = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "PropertyModifier")
        End Set
    End Property


    <Category("Validation"), _
    Description("The name of the validation method for this property. (Must be a method that returns void and takes no parameters or one IList parameter with exceptions)."), _
    DisplayName("Validate method"), _
    DefaultValue("")> Public Property ValidateMethod() As String
        Get
            Return m_PropertyMap.ValidateMethod
        End Get
        Set(ByVal Value As String)
            If Value <> m_PropertyMap.ValidateMethod Then
                m_PropertyMap.ValidateMethod = Value
                RaiseEvent AfterPropertySet(m_PropertyMap, "ValidateMethod")
            End If
        End Set
    End Property

    <Category("Field"), _
        Description("The name of the field that this property is mapping to."), _
        DisplayName("Field name"), _
        DefaultValue("")> Public Property FieldName() As String
        Get
            Return m_PropertyMap.FieldName
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.FieldName = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "FieldName")
        End Set
    End Property

    <Category("Field"), _
    Description("The accessibility of the field that this property is mapping to."), _
    DisplayName("Field accessibility"), _
    DefaultValue("")> Public Property FieldAccessibility() As AccessibilityType
        Get
            Return m_PropertyMap.FieldAccessibility
        End Get
        Set(ByVal Value As AccessibilityType)
            m_PropertyMap.FieldAccessibility = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "FieldAccessibility")
        End Set
    End Property

    <Category("Design"), _
        TypeConverter(GetType(DataTypeConverter)), _
        Description("The data type of this property. For collection properties, you should specify the data type of the collection items in the item type setting."), _
        DisplayName("Data type"), _
        DefaultValue("")> Public Property DataType() As String
        Get
            Return m_PropertyMap.DataType
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.DataType = Value
            If Not Value = "" Then
                If HasReferenceDataType() Then
                    If m_PropertyMap.ReferenceType = ReferenceType.None Then
                        If m_PropertyMap.IsCollection Then
                            m_PropertyMap.ReferenceType = ReferenceType.ManyToMany
                        Else
                            m_PropertyMap.ReferenceType = ReferenceType.OneToMany
                        End If
                    End If
                    DialogueServices.AskToCreateIfInverseNotExist(m_PropertyMap.Inverse, m_PropertyMap)
                Else
                    If Not m_PropertyMap.ReferenceType = ReferenceType.None Then
                        m_PropertyMap.ReferenceType = ReferenceType.None
                    End If
                End If
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "DataType")
        End Set
    End Property


    <Category("Design"), _
        Description("The default value for the private field that holds the value for this property. For a new instance of a non-persistent object (such as an ArrayList for a collection property), just enter 'New' (without the enclosing quote marks)"), _
        DisplayName("Default value"), _
        DefaultValue("")> Public Property DefaultValue() As String
        Get
            Return m_PropertyMap.DefaultValue
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.DefaultValue = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "DefaultValue")
        End Set
    End Property



    <Category("Design"), _
        Description("If you set lazy loading to True, the property will not be loaded with a value from the data source until the first time it is accessed."), _
        DisplayName("Lazy loading"), _
        DefaultValue(False)> Public Property LazyLoad() As Boolean
        Get
            Return m_PropertyMap.LazyLoad
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.LazyLoad = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "LazyLoad")
        End Set
    End Property

    <Category("Design"), _
        Description("Set to True for read-only persistence."), _
        DisplayName("Read only"), _
        DefaultValue(False)> Public Property IsReadOnly() As Boolean
        Get
            Return m_PropertyMap.IsReadOnly
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.IsReadOnly = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsReadOnly")
        End Set
    End Property

    <Category("Inverse"), _
        Description("Set to True for an inverse property that should not have its values saved to the database since its inverse property will manage all persistence. Normally, when mapping to a relational database, one of the properties in an inverse property relationship should be set to slave (in a OneMany relationship the list property side should be set to slave)."), _
        DisplayName("Slave"), _
        DefaultValue(False)> Public Property IsSlave() As Boolean
        Get
            Return m_PropertyMap.IsSlave
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.IsSlave = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsSlave")
        End Set
    End Property

    <Category("Caching"), _
        Description("The value in seconds that values of this property may stay cached before going stale and in need of a reload. The default value of -1 means the time to live value of the class map will be used."), _
        DisplayName("Time to live"), _
        DefaultValue(-1)> Public Property TimeToLive() As Integer
        Get
            Return m_PropertyMap.TimeToLive
        End Get
        Set(ByVal Value As Integer)
            If Value <> m_PropertyMap.TimeToLive Then
                m_PropertyMap.TimeToLive = Value
                RaiseEvent AfterPropertySet(m_PropertyMap, "TimeToLive")
            End If
        End Set
    End Property

    <Category("Caching"), _
        Description("The behavior specifying how values of this property may stay cached before going stale (according to the timespan specified in time to live) and in need of a reload. The default value of means the time to live behavior of the class map will be used."), _
        DisplayName("Time to live behavior"), _
        DefaultValue(TimeToLiveBehavior.Default)> Public Property TheTimeToLiveBehavior() As TimeToLiveBehavior
        Get
            Return m_PropertyMap.TimeToLiveBehavior
        End Get
        Set(ByVal Value As TimeToLiveBehavior)
            If Value <> m_PropertyMap.TimeToLiveBehavior Then
                m_PropertyMap.TimeToLiveBehavior = Value
                RaiseEvent AfterPropertySet(m_PropertyMap, "TimeToLiveBehavior")
            End If
        End Set
    End Property


    <Category("Reference property"), _
        Description("The type of reference represented by this property."), _
        DisplayName("Reference type"), _
        DefaultValue(False)> Public Property ReferenceType() As ReferenceType
        Get
            Return m_PropertyMap.ReferenceType
        End Get
        Set(ByVal Value As ReferenceType)
            m_PropertyMap.ReferenceType = Value
            If Not Value = ReferenceType.None Then
                Select Case Value

                    Case ReferenceType.ManyToMany

                        If Not m_PropertyMap.IsCollection Then
                            m_PropertyMap.IsCollection = True
                        End If

                    Case ReferenceType.ManyToOne

                        If Not m_PropertyMap.IsCollection Then
                            m_PropertyMap.IsCollection = True
                        End If

                        m_PropertyMap.IsSlave = True

                    Case Else

                        If m_PropertyMap.IsCollection Then
                            m_PropertyMap.IsCollection = False
                        End If

                End Select
            Else
                m_PropertyMap.CascadingCreate = False
                m_PropertyMap.CascadingDelete = False
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "ReferenceType")
        End Set
    End Property


    <Category("Reference property"), _
        Description("When True, a new instance of the class referenced by this property will be created and referenced when a new object of the class this property belongs to is created. Applies only to reference properties."), _
        DisplayName("Cascading create"), _
        DefaultValue(False)> Public Property CascadingCreate() As Boolean
        Get
            Return m_PropertyMap.CascadingCreate
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.CascadingCreate = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "CascadingCreate")
        End Set
    End Property

    <Category("Reference property"), _
        Description("When True, all instances referenced by this property will be deleted when a the object that this property belongs to is deleted. Applies only to reference properties."), _
        DisplayName("Cascading delete"), _
        DefaultValue(False)> Public Property CascadingDelete() As Boolean
        Get
            Return m_PropertyMap.CascadingDelete
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.CascadingDelete = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "CascadingDelete")
        End Set
    End Property

    <Category("Collection property"), _
        Description("Set to true if the property accepts a collection of values."), _
        DisplayName("Collection"), _
        DefaultValue(False)> Public Property IsCollection() As Boolean
        Get
            Return m_PropertyMap.IsCollection
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.IsCollection = Value
            If Value Then
                m_PropertyMap.ItemType = m_PropertyMap.DataType
                m_PropertyMap.DataType = "System.Collections.ArrayList"
                'm_PropertyMap.DefaultValue = "New System.Collections.ArrayList()"
                Select Case m_PropertyMap.ReferenceType
                    Case ReferenceType.OneToMany
                        m_PropertyMap.ReferenceType = ReferenceType.ManyToMany
                    Case ReferenceType.OneToOne
                        m_PropertyMap.ReferenceType = ReferenceType.ManyToOne
                End Select
                If m_PropertyMap.IsSlave = False And m_PropertyMap.ReferenceType = ReferenceType.ManyToMany And Len(m_PropertyMap.Inverse) > 0 And m_PropertyMap.NoInverseManagement = False Then
                    m_PropertyMap.DataType = "Puzzle.NPersist.Framework.ManagedList"
                    'm_PropertyMap.DefaultValue = "New Puzzle.NPersist.Framework.ManagedList(Me, """ & m_PropertyMap.Name & """)"
                End If
            Else
                m_PropertyMap.DataType = m_PropertyMap.ItemType
                m_PropertyMap.DefaultValue = ""
                m_PropertyMap.ItemType = ""
                Select Case m_PropertyMap.ReferenceType
                    Case ReferenceType.ManyToMany
                        m_PropertyMap.ReferenceType = ReferenceType.OneToMany
                    Case ReferenceType.ManyToOne
                        m_PropertyMap.ReferenceType = ReferenceType.OneToOne
                End Select
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsCollection")
        End Set
    End Property

    <Category("Collection property"), _
        TypeConverter(GetType(ItemTypeConverter)), _
        Description("The data type of the items in this property. Applies to collection properties only."), _
        DisplayName("Item type"), _
        DefaultValue("")> Public Property ItemType() As String
        Get
            Return m_PropertyMap.ItemType
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.ItemType = Value
            If Not Value = "" Then
                If HasReferenceDataType() Then
                    If m_PropertyMap.ReferenceType = ReferenceType.None Then
                        If m_PropertyMap.IsCollection Then
                            m_PropertyMap.ReferenceType = ReferenceType.ManyToMany
                        Else
                            m_PropertyMap.ReferenceType = ReferenceType.OneToMany
                        End If
                    End If
                    DialogueServices.AskToCreateIfInverseNotExist(m_PropertyMap.Inverse, m_PropertyMap)
                Else
                    If Not m_PropertyMap.ReferenceType = ReferenceType.None Then
                        m_PropertyMap.ReferenceType = ReferenceType.None
                    End If
                End If
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "ItemType")
        End Set
    End Property


    <Category("Collection property"), _
        TypeConverter(GetType(OrderByConverter)), _
        Description("The name of the property in the referenced class that referenced in this property should be sorted by."), _
        DisplayName("Order by"), _
        DefaultValue("")> Public Property OrderBy() As String
        Get
            Return m_PropertyMap.OrderBy
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.OrderBy = Value
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "OrderBy")
        End Set
    End Property


    <Category("Design"), _
        Description("Set to true if this is property should get its values assigned to it by the data source (for example by an auto increaser column)."), _
        DisplayName("Assigned by source"), _
        DefaultValue(False)> Public Property IsAssignedBySource() As Boolean
        Get
            Return m_PropertyMap.IsAssignedBySource
        End Get
        Set(ByVal Value As Boolean)
            If Not Value = m_PropertyMap.IsAssignedBySource Then
                m_PropertyMap.IsAssignedBySource = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsAssignedBySource")
        End Set
    End Property


    <Category("Design"), _
        Description("Set to true if this is property may contain null values. (Note that whenever a working mapping to a column has been set up, the nullability is read from the AllowNulls status of that column)."), _
        DisplayName("Nullable"), _
        DefaultValue(False)> Public Property IsNullable() As Boolean
        Get
            Return m_PropertyMap.IsNullable
        End Get
        Set(ByVal Value As Boolean)
            If Not Value = m_PropertyMap.IsNullable Then
                m_PropertyMap.IsNullable = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsNullable")
        End Set
    End Property


    <Category("Validation"), _
        Description("Specify a maximum length for string values (maximum number of elements for list properties) or leave as -1 for no limit. (Note that whenever a working mapping to a column has been set up for a string property, the length for the property is read from that column)."), _
        DisplayName("Max length"), _
        DefaultValue(-1)> Public Property MaxLength() As Long
        Get
            Return m_PropertyMap.MaxLength
        End Get
        Set(ByVal Value As Long)
            If Not Value = m_PropertyMap.MaxLength Then
                m_PropertyMap.MaxLength = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "MaxLength")
        End Set
    End Property

    <Category("Validation"), _
        Description("Specify a minimum length for string values (minimum number of elements for list properties) or leave as -1 for no limit."), _
        DisplayName("Min length"), _
        DefaultValue(-1)> Public Property MinLength() As Long
        Get
            Return m_PropertyMap.MinLength
        End Get
        Set(ByVal Value As Long)
            If Not Value = m_PropertyMap.MinLength Then
                m_PropertyMap.MinLength = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "MinLength")
        End Set
    End Property


    <Category("Validation"), _
        Description("Specify a maximum value for primitive values or leave blank for no limit."), _
        DisplayName("Max value"), _
        DefaultValue("")> Public Property MaxValue() As String
        Get
            Return m_PropertyMap.MaxValue
        End Get
        Set(ByVal Value As String)
            If Not Value = m_PropertyMap.MaxValue Then
                m_PropertyMap.MaxValue = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "MaxValue")
        End Set
    End Property

    <Category("Validation"), _
        Description("Specify a minimum value for primitive values or leave blank for no limit."), _
        DisplayName("Min value"), _
        DefaultValue("")> Public Property MinValue() As String
        Get
            Return m_PropertyMap.MinValue
        End Get
        Set(ByVal Value As String)
            If Not Value = m_PropertyMap.MinValue Then
                m_PropertyMap.MinValue = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "MinValue")
        End Set
    End Property

    <Category("Identity"), _
        Description("Set to true if this is an identity property. An identity property is a property that by itself or together with other identity properties (composite key) can be used to uniquely identify the object."), _
        DisplayName("Identity"), _
        DefaultValue(False)> Public Property IsIdentity() As Boolean
        Get
            Return m_PropertyMap.IsIdentity
        End Get
        Set(ByVal Value As Boolean)
            If Not Value = m_PropertyMap.IsIdentity Then
                If Value Then
                    m_PropertyMap.IdentityIndex = m_PropertyMap.ClassMap.GetIdentityPropertyMaps.Count
                Else
                    m_PropertyMap.IdentityIndex = 0
                End If
                m_PropertyMap.IsIdentity = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsIdentity")
        End Set
    End Property


    <Category("Identity"), _
        Description("The index of the identity property. Identity properties will be concatenated into a string object identity in the order specified by the identity indexes."), _
        DisplayName("Identity index"), _
        DefaultValue(0)> Public Property IdentityIndex() As Integer
        Get
            Return m_PropertyMap.IdentityIndex
        End Get
        Set(ByVal Value As Integer)
            m_PropertyMap.IdentityIndex = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "IdentityIndex")
        End Set
    End Property


    <Category("Key"), _
        Description("Set to true if this is a key property. An key property is a property that by itself or together with other key properties can be used to identify the object as uniquely as is required for human identification of the object and can be useful for display purposes (An example would be the FirstName and LastName properties of a Person object)."), _
        DisplayName("Key"), _
        DefaultValue(False)> Public Property IsKey() As Boolean
        Get
            Return m_PropertyMap.IsKey
        End Get
        Set(ByVal Value As Boolean)
            If Not Value = m_PropertyMap.IsKey Then
                If Value Then
                    m_PropertyMap.KeyIndex = m_PropertyMap.ClassMap.GetKeyPropertyMaps.Count
                Else
                    m_PropertyMap.KeyIndex = 0
                End If
                m_PropertyMap.IsKey = Value
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "IsKey")
        End Set
    End Property


    <Category("Key"), _
        Description("The index of the key property. Key properties will be concatenated into a string object key in the order specified by the key indexes."), _
        DisplayName("Key index"), _
        DefaultValue(0)> Public Property KeyIndex() As Integer
        Get
            Return m_PropertyMap.KeyIndex
        End Get
        Set(ByVal Value As Integer)
            m_PropertyMap.KeyIndex = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "KeyIndex")
        End Set
    End Property


    <Category("Inverse"), _
        Description("The name of this property's inverse property. Use only with bi-directional reference properties. (Example: For an OrderLine.Order property, Order.OrderLines would be the inverse property!)"), _
        DisplayName("Inverse property name"), _
        DefaultValue("")> Public Property Inverse() As String
        Get
            Return m_PropertyMap.Inverse
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.Inverse = Value
            DialogueServices.AskToCreateIfInverseNotExist(Value, m_PropertyMap)
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "Inverse")
        End Set
    End Property

    <Category("Inverse"), _
        Description("Set to True for inverse properties with custom inverse management. This will turn off the standard inverse management for the property."), _
        DisplayName("Custom inverse management"), _
        DefaultValue(False)> Public Property NoInverseManagement() As Boolean
        Get
            Return m_PropertyMap.NoInverseManagement
        End Get
        Set(ByVal Value As Boolean)
            m_PropertyMap.NoInverseManagement = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "NoInverseManagement")
        End Set
    End Property

    <Category("Inverse"), _
        Description("Set to True in order for this property to inherit its mapping settings from its inverse property."), _
        DisplayName("Inherit mappings from inverse"), _
        DefaultValue(False)> Public Property InheritInverseMappings() As Boolean
        Get
            Return m_PropertyMap.InheritInverseMappings
        End Get
        Set(ByVal Value As Boolean)
            Dim inv As IPropertyMap = m_PropertyMap.GetInversePropertyMap()
            If inv.InheritInverseMappings Then inv.InheritInverseMappings = False
            m_PropertyMap.InheritInverseMappings = Value
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "InheritInverseMappings")
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(SourceConverter)), _
        Description("The name of the data source that this property is mapping to. Leave blank in order to use the data source that the class is mapping to."), _
        DisplayName("Source name"), _
        DefaultValue("")> Public Property Source() As String
        Get
            Return m_PropertyMap.Source
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.Source = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "Source")
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(TableConverter)), _
        Description("The name of the table that this property is mapping to. Leave blank to use the table that the class is mapping to."), _
        DisplayName("Table name"), _
        DefaultValue("")> Public Property Table() As String
        Get
            Return m_PropertyMap.Table
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.Table = Value
            DialogueServices.AskToCreateIfTableNotExist(Value, m_PropertyMap.GetSourceMap)
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_PropertyMap, "Table")
        End Set
    End Property


    <Category("O/R Mapping"), _
        Description("The value that will be returned by the property if the value in the column is Null."), _
        DisplayName("Null value substitute"), _
        DefaultValue("")> Public Property NullSubstitute() As String
        Get
            Return m_PropertyMap.NullSubstitute
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.NullSubstitute = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "NullSubstitute")
        End Set
    End Property

    <Category("O/R Mapping"), _
        TypeConverter(GetType(ColumnConverter)), _
        Description("The name of the column that this property is mapping to."), _
        DisplayName("Column name"), _
        DefaultValue("")> Public Property Column() As String
        Get
            Return m_PropertyMap.Column
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.Column = Value
            DialogueServices.AskToCreateIfColumnNotExist(Value, m_PropertyMap.GetTableMap)
            RaiseEvent AfterPropertySet(m_PropertyMap, "Column")
        End Set
    End Property

    <Category("O/R Mapping"), _
        Editor(GetType(AdditionalColumnsEditor), GetType(UITypeEditor)), _
        Description("The name of any additional columns that this property is mapping to. Applies only to reference properties."), _
        DisplayName("Additional columns"), _
        DefaultValue("")> Public Property AdditionalColumns() As String()
        Get
            Return m_PropertyMap.AdditionalColumns.ToArray(GetType(String))
        End Get
        Set(ByVal Value As String())
            Dim name As String
            m_PropertyMap.AdditionalColumns.Clear()
            For Each name In Value
                m_PropertyMap.AdditionalColumns.Add(name)
            Next
            '            DialogueServices.AskToCreateIfColumnNotExist(Value, m_PropertyMap.GetTableMap)
            RaiseEvent AfterPropertySet(m_PropertyMap, "AdditionalColumns")
        End Set
    End Property


    <Category("O/R Mapping"), _
        TypeConverter(GetType(ColumnConverter)), _
        Description("The name of the id column that this property is mapping to."), _
        DisplayName("Id column name"), _
        DefaultValue("")> Public Property IdColumn() As String
        Get
            Return m_PropertyMap.IdColumn
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.IdColumn = Value
            DialogueServices.AskToCreateIfColumnNotExist(Value, m_PropertyMap.GetTableMap)
            RaiseEvent AfterPropertySet(m_PropertyMap, "IdColumn")
        End Set
    End Property

    <Category("O/R Mapping"), _
        Editor(GetType(AdditionalIdColumnsEditor), GetType(UITypeEditor)), _
        Description("The name of any additional columns that this property is mapping to. Applies only to reference properties."), _
        DisplayName("Additional id columns"), _
        DefaultValue("")> Public Property AdditionalIdColumns() As String()
        Get
            Return m_PropertyMap.AdditionalIdColumns.ToArray(GetType(String))
        End Get
        Set(ByVal Value As String())
            Dim name As String
            m_PropertyMap.AdditionalIdColumns.Clear()
            For Each name In Value
                m_PropertyMap.AdditionalIdColumns.Add(name)
            Next
            '            DialogueServices.AskToCreateIfColumnNotExist(Value, m_PropertyMap.GetTableMap)
            RaiseEvent AfterPropertySet(m_PropertyMap, "AdditionalIdColumns")
        End Set
    End Property

    <Category("O/O Mapping"), _
        TypeConverter(GetType(SourcePropertyConverter)), _
        Description("The name of the source property that this class is mapping to."), _
        DisplayName("Source property"), _
        DefaultValue("")> Public Property SourceProperty() As String
        Get
            Return m_PropertyMap.SourceProperty
        End Get
        Set(ByVal Value As String)
            m_PropertyMap.SourceProperty = Value
            'DialogueServices.AskToCreateIfClassNotExist(Value, m_ClassMap.GetSourceClassMap)
            RaiseEvent AfterPropertySet(m_PropertyMap, "SourceProperty")
        End Set
    End Property


    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to this property when the object is updated. If set to DefaultBehavior then the behavior will be inherited from the corresponding setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting update behavior for the property will become IncludeWhenDirty except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Update optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property UpdateOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Return m_PropertyMap.UpdateOptimisticConcurrencyBehavior
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            m_PropertyMap.UpdateOptimisticConcurrencyBehavior = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "UpdateOptimisticConcurrencyBehavior")
        End Set
    End Property

    <Category("Optimistic Concurrency"), _
        Description("The optimistic concurrency behavior applied to this property when the object is deleted. If set to DefaultBehavior then the behavior will be inherited from the corresponding setting in the class map, then the domain map and finally the persistence manager. Assuming all of these are set to DefaultBehavior, the resulting delete behavior for the property will become IncludeWhenLoaded except for properties mapping to large text (Length > 4000) and blob columns, which default to Disabled."), _
        DisplayName("Delete optimistic concurrency behavior"), _
        DefaultValue(OptimisticConcurrencyBehaviorType.DefaultBehavior)> Public Property DeleteOptimisticConcurrencyBehavior() As OptimisticConcurrencyBehaviorType
        Get
            Return m_PropertyMap.DeleteOptimisticConcurrencyBehavior
        End Get
        Set(ByVal Value As OptimisticConcurrencyBehaviorType)
            m_PropertyMap.DeleteOptimisticConcurrencyBehavior = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "DeleteOptimisticConcurrencyBehavior")
        End Set
    End Property

    <Category("Special property behavior"), _
        Description("Select a special behavior that is applied to your property when the object is created. For example, 'SetDateTime' could be used for a 'CreatedAt' property. 'Increase' applies only to properties mapping to numeric columns. 'SetDateTime' applies only to properties mapping to DateTime columns."), _
        DisplayName("Create behavior"), _
        DefaultValue(PropertySpecialBehaviorType.None)> Public Property OnCreateBehavior() As PropertySpecialBehaviorType
        Get
            Return m_PropertyMap.OnCreateBehavior
        End Get
        Set(ByVal Value As PropertySpecialBehaviorType)
            m_PropertyMap.OnCreateBehavior = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "OnCreateBehavior")
        End Set
    End Property

    <Category("Special property behavior"), _
        Description("Select a special behavior that is applied to your property when the object is persisted. For example, 'Increase' could be used for a property mapping to a version column while 'SetDateTime' could be used for a 'LastUpdatedAt' property. 'Increase' applies only to properties mapping to numeric columns. 'SetDateTime' applies only to properties mapping to DateTime columns."), _
        DisplayName("Persist behavior"), _
        DefaultValue(PropertySpecialBehaviorType.None)> Public Property OnPersistBehavior() As PropertySpecialBehaviorType
        Get
            Return m_PropertyMap.OnPersistBehavior
        End Get
        Set(ByVal Value As PropertySpecialBehaviorType)
            m_PropertyMap.OnPersistBehavior = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "OnPersistBehavior")
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
            Dim meta As String
            Dim key As String = "nh-unsaved-value"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-unsaved-value"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhPersister")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("The name of the class generating the new value for this (identity) property for a newly created object. Applies to identity properties only. (NHibernate specific)."), _
        DisplayName("NH generator class"), _
        DefaultValue("")> Public Property NhGeneratorClass() As String
        Get
            Dim meta As String
            Dim key As String = "nh-generator-class"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-generator-class"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhGeneratorClass")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("A list of parameters and values sent to the class generating the new value for this (identity) property, using a comma delimited string of name=value pairs (ex: table=uid_table,column=next_hi_value_column). (NHibernate specific)."), _
        DisplayName("NH generator parameters"), _
        DefaultValue("")> Public Property NhGeneratorParams() As String
        Get
            Dim meta As String
            Dim key As String = "nh-generator-parameters"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-generator-parameters"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhGeneratorParams")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specifies whether the mapped columns should be included in Sql Update statements. (NHibernate specific)."), _
        DisplayName("NH update"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhUpdate() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-update"
            meta = m_PropertyMap.GetMetaData(key)
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
            Dim key As String = "nh-update"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_PropertyMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_PropertyMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhUpdate")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether the mapped columns should be included in Sql Insert statements. (NHibernate specific)."), _
        DisplayName("NH insert"), _
        DefaultValue(NullableBool.Unspecified)> Public Property NhInsert() As NullableBool
        Get
            Dim meta As String
            Dim key As String = "nh-insert"
            meta = m_PropertyMap.GetMetaData(key)
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
            Dim key As String = "nh-insert"
            Select Case Value
                Case NullableBool.Unspecified
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case NullableBool.True
                    m_PropertyMap.SetMetaData(key, "true")

                Case NullableBool.False
                    m_PropertyMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhInsert")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Sql expression defining the value for a computed property. (NHibernate specific)."), _
        DisplayName("NH formula"), _
        DefaultValue("")> Public Property NhFormula() As String
        Get
            Dim meta As String
            Dim key As String = "nh-formula"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-formula"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhFormula")
        End Set
    End Property



    <Category("NHibernate"), _
        Description("Specifies which operations should be cascaded from the parent object to the associated object. (NHibernate specific)."), _
        DisplayName("NH cascade"), _
        DefaultValue(NhCascadeEnum.Unspecified)> Public Property NhCascade() As NhCascadeEnum
        Get
            Dim meta As String
            Dim key As String = "nh-cascade"
            meta = m_PropertyMap.GetMetaData(key)
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
        End Get
        Set(ByVal Value As NhCascadeEnum)
            Dim key As String = "nh-cascade"
            Select Case Value
                Case NhCascadeEnum.Unspecified
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case NhCascadeEnum.All
                    m_PropertyMap.SetMetaData(key, "all")

                Case NhCascadeEnum.None
                    m_PropertyMap.SetMetaData(key, "none")

                Case NhCascadeEnum.SaveUpdate
                    m_PropertyMap.SetMetaData(key, "save-update")

                Case NhCascadeEnum.Delete
                    m_PropertyMap.SetMetaData(key, "delete")

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhCascade")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Enables outer-join fetching for this association when use_outer_join is set. (NHibernate specific)."), _
        DisplayName("NH outer-join"), _
        DefaultValue(NullableBoolWithAuto.Unspecified)> Public Property NhOuterJoin() As NullableBoolWithAuto
        Get
            Dim meta As String
            Dim key As String = "nh-outer-join"
            meta = m_PropertyMap.GetMetaData(key)
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
        End Get
        Set(ByVal Value As NullableBoolWithAuto)
            Dim key As String = "nh-outer-join"
            Select Case Value
                Case NullableBoolWithAuto.Unspecified
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case NullableBoolWithAuto.Auto
                    m_PropertyMap.SetMetaData(key, "auto")

                Case NullableBoolWithAuto.True
                    m_PropertyMap.SetMetaData(key, "true")

                Case NullableBoolWithAuto.False
                    m_PropertyMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhOuterJoin")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specify a sorted collection with natural sort order, or a given comparator class. Applies only to collection properties. (NHibernate specific)."), _
        DisplayName("NH sort"), _
        DefaultValue("")> Public Property NhSort() As String
        Get
            Dim meta As String
            Dim key As String = "nh-sort"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-sort"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhSort")
        End Set
    End Property


    <Category("NHibernate"), _
        Description("Specify a table column (or columns) that define the iteration order of the collection, together with an optional asc or desc. Applies only to collection properties. (NHibernate specific)."), _
        DisplayName("NH order-by"), _
        DefaultValue("")> Public Property NhOrderBy() As String
        Get
            Dim meta As String
            Dim key As String = "nh-order-by"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-order-by"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhOrderBy")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specify an arbitrary Sql Where condition to be used when retrieving or removing the collection. Applies only to collection properties. (NHibernate specific)."), _
        DisplayName("NH where"), _
        DefaultValue("")> Public Property NhWhere() As String
        Get
            Dim meta As String
            Dim key As String = "nh-where"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-where"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhWhere")
        End Set
    End Property

    <Category("NHibernate"), _
    Description("Specifies the maximum length of strings that this property will accept. (NHibernate specific)."), _
    DisplayName("NH length"), _
    DefaultValue("")> Public Property NhLength() As String
        Get
            Dim meta As String
            Dim key As String = "nh-length"
            meta = m_PropertyMap.GetMetaData(key)
            Return meta
        End Get
        Set(ByVal Value As String)
            Dim key As String = "nh-length"
            Select Case Value
                Case ""
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case Else
                    m_PropertyMap.SetMetaData(key, Value)

            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhLength")
        End Set
    End Property

    <Category("NHibernate"), _
        Description("Specifies whether the property should be lazy loaded. If set to auto, the property will be marked as lazy loading if it is a reference collection property. (NHibernate specific)."), _
        DisplayName("NH lazy"), _
        DefaultValue(NullableBoolWithAuto.Unspecified)> Public Property NhLazy() As NullableBoolWithAuto
        Get
            Dim meta As String
            Dim key As String = "nh-lazy"
            meta = m_PropertyMap.GetMetaData(key)
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
        End Get
        Set(ByVal Value As NullableBoolWithAuto)
            Dim key As String = "nh-lazy"
            Select Case Value
                Case NullableBoolWithAuto.Unspecified
                    If m_PropertyMap.HasMetaData(key) Then
                        m_PropertyMap.RemoveMetaData(key)
                    End If

                Case NullableBoolWithAuto.Auto
                    m_PropertyMap.SetMetaData(key, "auto")

                Case NullableBoolWithAuto.True
                    m_PropertyMap.SetMetaData(key, "true")

                Case NullableBoolWithAuto.False
                    m_PropertyMap.SetMetaData(key, "false")
            End Select
            RaiseEvent AfterPropertySet(m_PropertyMap, "NhLazy")
        End Set
    End Property

    <Category("DOL"), _
        Description("Generated method's 'find by' group ID. Disabled if < 0."), _
        DisplayName("FindBy Group"), _
        DefaultValue(-1)> Public Property DOLFindByGroup() As Integer
        Get
            Return m_PropertyMap.DOLFindByGroup
        End Get
        Set(ByVal Value As Integer)
            m_PropertyMap.DOLFindByGroup = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "DOLFindByGroup")
        End Set
    End Property

    <Category("DOL"), _
        Description("Generated method's 'find by' group index"), _
        DisplayName("FindBy GroupIndex"), _
        DefaultValue(-1)> Public Property DOLFindByGroupIndex() As Integer
        Get
            Return m_PropertyMap.DOLFindByGroupIndex
        End Get
        Set(ByVal Value As Integer)
            m_PropertyMap.DOLFindByGroupIndex = Value
            RaiseEvent AfterPropertySet(m_PropertyMap, "DOLFindByGroupIndex")
        End Set
    End Property

    Private Function HasReferenceDataType() As Boolean

        Return frmDomainMapBrowser.HasReferenceDataType(m_PropertyMap)

    End Function



    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Select Case propDesc.Name

            Case "ReferenceType"
                If Not HasReferenceDataType() Then propDesc.SetReadOnly()
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "DataType"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "IsCollection"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "ItemType"
                If Not m_PropertyMap.IsCollection Then propDesc.SetReadOnly()
                'If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "IdentityIndex"
                If Not m_PropertyMap.IsIdentity Then
                    propDesc.SetReadOnly()
                Else
                    If Not m_PropertyMap.ClassMap.GetIdentityPropertyMaps.Count > 1 Then propDesc.SetReadOnly()
                End If

            Case "IsReadOnly"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()
                If m_PropertyMap.ClassMap.IsReadOnly Then propDesc.SetReadOnly()

            Case "Inverse"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "NoInverseManagement"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()
                If Len(m_PropertyMap.Inverse) < 1 Then propDesc.SetReadOnly()
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "InheritInverseMappings"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()
                If Len(m_PropertyMap.Inverse) < 1 Then propDesc.SetReadOnly()

            Case "AdditionalColumns"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

            Case "AdditionalIdColumns"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

            Case "Source"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "Table"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "Column"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "IdColumn"
                If Not (Len(m_PropertyMap.Table) > 0 Or m_PropertyMap.IsCollection) Then propDesc.SetReadOnly()
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "AdditionalColumns"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "AdditionalIdColumns"
                If m_PropertyMap.InheritInverseMappings Then propDesc.SetReadOnly()

            Case "CascadingCreate"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

            Case "CascadingDelete"
                If m_PropertyMap.ReferenceType = ReferenceType.None Then propDesc.SetReadOnly()

            Case "OrderBy"
                If Not (m_PropertyMap.ReferenceType = ReferenceType.ManyToMany Or m_PropertyMap.ReferenceType = ReferenceType.ManyToOne) Then propDesc.SetReadOnly()

        End Select

        Return propDesc
    End Function


End Class

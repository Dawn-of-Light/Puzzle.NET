Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.ComponentModel

Public Class TablesToClassesConfigProperties
    Inherits PropertiesBase

    Private m_TablesToClassesConfig As TablesToClassesConfig

    Public Event BeforePropertySet(ByVal mapObject As TablesToClassesConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As TablesToClassesConfig, ByVal propertyName As String)

    Public Function GetTablesToClassesConfig() As TablesToClassesConfig
        Return m_TablesToClassesConfig
    End Function

    Public Sub SetTablesToClassesConfig(ByVal value As TablesToClassesConfig)
        m_TablesToClassesConfig = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_TablesToClassesConfig

    End Function


    <Category("Converters"), _
        TypeConverter(GetType(ConverterMethodConverter)), _
        Description("A custom converter method accepting a string with the name of a table and returning a string with the name of a class."), _
        DisplayName("Table to class name converter"), _
        DefaultValue(True)> Public Property TableToClassNameConverter() As String
        Get
            Return m_TablesToClassesConfig.TableToClassNameConverter
        End Get
        Set(ByVal Value As String)
            If Value <> m_TablesToClassesConfig.TableToClassNameConverter Then
                m_TablesToClassesConfig.TableToClassNameConverter = Value
                RaiseEvent AfterPropertySet(m_TablesToClassesConfig, "TableToClassNameConverter")
            End If
        End Set
    End Property

    <Category("Converters"), _
        TypeConverter(GetType(ConverterMethodConverter)), _
        Description("A custom converter method accepting an object implementing the Puzzle.NPersist.Framework.Mapping.IColumnMap interface representing the column and returning a string with the name of a property."), _
        DisplayName("Column to property name converter"), _
        DefaultValue(True)> Public Property ColumnToPropertyNameConverter() As String
        Get
            Return m_TablesToClassesConfig.ColumnToPropertyNameConverter
        End Get
        Set(ByVal Value As String)
            If Value <> m_TablesToClassesConfig.ColumnToPropertyNameConverter Then
                m_TablesToClassesConfig.ColumnToPropertyNameConverter = Value
                RaiseEvent AfterPropertySet(m_TablesToClassesConfig, "ColumnToPropertyNameConverter")
            End If
        End Set
    End Property

    <Category("Converters"), _
        TypeConverter(GetType(ConverterMethodConverter)), _
        Description("A custom converter method accepting an object implementing the Puzzle.NPersist.Framework.Mapping.IPropertyMap interface representing the property and returning a string with the name of an inverse property."), _
        DisplayName("Property to inverse name converter"), _
        DefaultValue(True)> Public Property PropertyToInverseNameConverter() As String
        Get
            Return m_TablesToClassesConfig.PropertyToInverseNameConverter
        End Get
        Set(ByVal Value As String)
            If Value <> m_TablesToClassesConfig.PropertyToInverseNameConverter Then
                m_TablesToClassesConfig.PropertyToInverseNameConverter = Value
                RaiseEvent AfterPropertySet(m_TablesToClassesConfig, "PropertyToInverseNameConverter")
            End If
        End Set
    End Property


    <Category("Interfaces"), _
        Description("If true, inverse properties will be generated for all reference properties, creating bi-directional property relationships. An example would be an Order.Customer property which would get a Customer.Orders property created for it as its inverse property."), _
        DisplayName("Generate inverse properties"), _
        DefaultValue(True)> Public Property GenerateInverseProperties() As Boolean
        Get
            Return m_TablesToClassesConfig.GenerateInverseProperties
        End Get
        Set(ByVal Value As Boolean)
            If Value <> m_TablesToClassesConfig.GenerateInverseProperties Then
                m_TablesToClassesConfig.GenerateInverseProperties = Value
                RaiseEvent AfterPropertySet(m_TablesToClassesConfig, "GenerateInverseProperties")
            End If
        End Set
    End Property

End Class


Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class SourceConverter
    Inherits StringConverter


    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listSourceNames As New ArrayList()
        Dim domainMap As IDomainMap
        Dim sourceMap As ISourceMap
        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim umlClass As umlClass

        If context.Instance.GetType Is GetType(ClassProperties) Then

            classMap = CType(context.Instance, ClassProperties).GetClassMap
            domainMap = classMap.DomainMap

        ElseIf context.Instance.GetType Is GetType(PropertyProperties) Then

            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap
            domainMap = propertyMap.ClassMap.DomainMap

        ElseIf context.Instance.GetType Is GetType(DomainProperties) Then

            domainMap = CType(context.Instance, DomainProperties).GetDomainMap

        ElseIf context.Instance.GetType Is GetType(UmlClassProperties) Then

            umlClass = CType(context.Instance, UmlClassProperties).GetUmlClass
            classMap = umlClass.GetClassMap

            If Not classMap Is Nothing Then

                domainMap = classMap.DomainMap

            End If

        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then

            propertyMap = CType(context.Instance, UmlLineEndProperties).GetpropertyMap
            If Not propertyMap Is Nothing Then

                domainMap = propertyMap.ClassMap.DomainMap

            End If

        End If

        If Not domainMap Is Nothing Then

            For Each sourceMap In domainMap.SourceMaps

                listSourceNames.Add(sourceMap.Name)

            Next

        End If

        listSourceNames.Sort()

        listSourceNames.Insert(0, "")

        Return New StandardValuesCollection(listSourceNames)

    End Function

End Class

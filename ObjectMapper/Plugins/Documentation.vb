Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports Puzzle.ObjectMapper.Tools
Imports Puzzle.ObjectMapper.Plugin
Imports System.Text

<PluginClass("Puzzle")> Public Class Documentation

    Public tab As String = "    "

    Public Function GetTabs(ByVal index As Integer) As String

        Dim i As Long
        Dim str As String

        For i = 0 To index

            str += tab

        Next

        Return str

    End Function

    <PluginMethod(GetType(IProject), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal project As IProject) As String

        Dim strBuilder As New StringBuilder

        Dim resource As IResource
        Dim domainMap As IDomainMap

        strBuilder.Append("Project " & project.Name & vbCrLf)
        strBuilder.Append(vbCrLf)

        For Each resource In project.Resources

            If resource.ResourceType = ResourceTypeEnum.XmlDomainMap Then

                domainMap = resource.GetResource

                strBuilder.Append(ModelOverview(domainMap))

            End If

        Next

        strBuilder.Append("End Project" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function


    <PluginMethod(GetType(IDomainMap), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal domainMap As IDomainMap) As String

        Dim strBuilder As New StringBuilder

        Dim classMap As IClassMap
        Dim sourceMap As ISourceMap

        strBuilder.Append("Domain " & domainMap.Name & vbCrLf)
        strBuilder.Append(vbCrLf)

        For Each classMap In domainMap.ClassMaps

            strBuilder.Append(ModelOverview(classMap))

        Next

        For Each sourceMap In domainMap.SourceMaps

            strBuilder.Append(ModelOverview(sourceMap))

        Next

        strBuilder.Append("End Domain" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function


    <PluginMethod(GetType(IClassMap), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal classMap As IClassMap) As String

        Dim strBuilder As New StringBuilder
        Dim propertyMap As IPropertyMap

        strBuilder.Append(GetTabs(0) & "Class " & classMap.Name & vbCrLf)
        strBuilder.Append(vbCrLf)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            strBuilder.Append(ModelOverview(propertyMap))

        Next

        strBuilder.Append(vbCrLf)
        strBuilder.Append(GetTabs(0) & "End Class" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function

    <PluginMethod(GetType(IPropertyMap), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal propertyMap As IPropertyMap) As String

        Dim strBuilder As New StringBuilder

        strBuilder.Append(GetTabs(1) & "Property " & propertyMap.Name & vbCrLf)

        Return strBuilder.ToString

    End Function

    <PluginMethod(GetType(ISourceMap), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal sourceMap As ISourceMap) As String

        Dim strBuilder As New StringBuilder
        Dim tableMap As ITableMap

        strBuilder.Append(GetTabs(0) & "Source " & sourceMap.Name & vbCrLf)
        strBuilder.Append(vbCrLf)

        For Each tableMap In sourceMap.TableMaps

            strBuilder.Append(ModelOverview(tableMap))

        Next

        strBuilder.Append(GetTabs(0) & "End Source" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function



    <PluginMethod(GetType(ITableMap), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal tableMap As ITableMap) As String

        Dim strBuilder As New StringBuilder
        Dim columnMap As IColumnMap

        strBuilder.Append(GetTabs(1) & "Table " & tableMap.Name & vbCrLf)
        strBuilder.Append(vbCrLf)

        For Each columnMap In tableMap.ColumnMaps

            strBuilder.Append(ModelOverview(columnMap))

        Next

        strBuilder.Append(vbCrLf)
        strBuilder.Append(GetTabs(1) & "End Table" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function

    <PluginMethod(GetType(IColumnMap), GetType(String), "Model Overview")> Public Overloads Function ModelOverview(ByVal columnMap As IColumnMap) As String

        Dim strBuilder As New StringBuilder

        strBuilder.Append(GetTabs(2) & "Column " & columnMap.Name & vbCrLf)

        Return strBuilder.ToString

    End Function





    <PluginMethod(GetType(IProject), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal project As IProject) As String

        Dim strBuilder As New StringBuilder

        Dim resource As IResource
        Dim domainMap As IDomainMap

        strBuilder.Append("Project " & project.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        For Each resource In project.Resources

            If resource.ResourceType = ResourceTypeEnum.XmlDomainMap Then

                domainMap = resource.GetResource

                strBuilder.Append(ModelDescription(domainMap))

            End If

        Next

        strBuilder.Append("End Project" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function


    <PluginMethod(GetType(IDomainMap), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal domainMap As IDomainMap) As String

        Dim classesToCode As New ClassesToCodeBase

        Dim strBuilder As New StringBuilder

        Dim classMap As IClassMap
        Dim sourceMap As ISourceMap

        strBuilder.Append("Domain " & domainMap.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        strBuilder.Append(classesToCode.ModelDescription(domainMap, GetTabs(0), ""))

        strBuilder.Append(vbCrLf)

        For Each classMap In domainMap.ClassMaps

            strBuilder.Append(ModelDescription(classMap))

        Next

        For Each sourceMap In domainMap.SourceMaps

            strBuilder.Append(ModelDescription(sourceMap))

        Next

        strBuilder.Append("End Domain" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function


    <PluginMethod(GetType(IClassMap), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal classMap As IClassMap) As String

        Dim classesToCode As New ClassesToCodeBase

        Dim strBuilder As New StringBuilder
        Dim propertyMap As IPropertyMap
        Dim superClassMap As IClassMap = classMap.GetInheritedClassMap

        strBuilder.Append(GetTabs(0) & "Class " & classMap.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        strBuilder.Append(classesToCode.ModelDescription(classMap, GetTabs(1), ""))

        strBuilder.Append(vbCrLf)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            strBuilder.Append(ModelDescription(propertyMap))

        Next

        strBuilder.Append(GetTabs(0) & "End Class" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function

    <PluginMethod(GetType(IPropertyMap), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal propertyMap As IPropertyMap) As String

        Dim classesToCode As New ClassesToCodeBase

        Dim strBuilder As New StringBuilder
        Dim column As String
        Dim columns As String
        Dim hasMulti As Boolean
        Dim dataType As String

        strBuilder.Append(GetTabs(1) & "Property " & propertyMap.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        strBuilder.Append(classesToCode.ModelDescription(propertyMap, GetTabs(2), ""))

        strBuilder.Append(vbCrLf)

        strBuilder.Append(GetTabs(1) & "End Property" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function

    <PluginMethod(GetType(ISourceMap), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal sourceMap As ISourceMap) As String

        Dim classesToCode As New ClassesToCodeBase

        Dim strBuilder As New StringBuilder
        Dim tableMap As ITableMap

        strBuilder.Append(GetTabs(0) & "Source " & sourceMap.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        strBuilder.Append(classesToCode.ModelDescription(sourceMap, GetTabs(1), ""))

        strBuilder.Append(vbCrLf)

        For Each tableMap In sourceMap.TableMaps

            strBuilder.Append(ModelDescription(tableMap))

        Next

        strBuilder.Append(GetTabs(0) & "End Source" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function



    <PluginMethod(GetType(ITableMap), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal tableMap As ITableMap) As String

        Dim classesToCode As New ClassesToCodeBase

        Dim strBuilder As New StringBuilder
        Dim columnMap As IColumnMap

        strBuilder.Append(GetTabs(1) & "Table " & tableMap.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        'strBuilder.Append(classesToCode.ModelDescription(tableMap, GetTabs(2), ""))

        'strBuilder.Append(vbCrLf)

        For Each columnMap In tableMap.ColumnMaps

            strBuilder.Append(ModelDescription(columnMap))

        Next

        strBuilder.Append(GetTabs(1) & "End Table" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function

    <PluginMethod(GetType(IColumnMap), GetType(String), "Model Description")> Public Overloads Function ModelDescription(ByVal columnMap As IColumnMap) As String

        Dim classesToCode As New ClassesToCodeBase

        Dim strBuilder As New StringBuilder

        strBuilder.Append(GetTabs(2) & "Column " & columnMap.Name & vbCrLf)

        strBuilder.Append(vbCrLf)

        strBuilder.Append(classesToCode.ModelDescription(columnMap, GetTabs(3), ""))

        strBuilder.Append(vbCrLf)
        strBuilder.Append(GetTabs(2) & "End Column" & vbCrLf)
        strBuilder.Append(vbCrLf)

        Return strBuilder.ToString

    End Function

End Class

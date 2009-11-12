Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports Puzzle.ObjectMapper.Tools

Public Class DialogueServices

    Public Shared Function AskToCreateIfTableNotExist(ByVal name As String, ByVal sourceMap As ISourceMap) As ITableMap

        If Len(name) < 1 Then Return Nothing

        If sourceMap Is Nothing Then Return Nothing

        If Not sourceMap.GetTableMap(name) Is Nothing Then Return sourceMap.GetTableMap(name)

        If MsgBox("There is currently no table named '" & name & "' in the data source '" & sourceMap.Name & "'. Would you like to create the table now?", MsgBoxStyle.YesNo, "Create Table") = MsgBoxResult.No Then Return Nothing

        Return MapServices.CreateTableMap(name, sourceMap)

    End Function

    Public Shared Function AskToCreateIfColumnNotExist(ByVal name As String, ByVal tableMap As ITableMap) As IColumnMap

        If Len(name) < 1 Then Return Nothing

        If tableMap Is Nothing Then Return Nothing

        If Not tableMap.GetColumnMap(name) Is Nothing Then Return tableMap.GetColumnMap(name)

        If MsgBox("There is currently no column named '" & name & "' in the table '" & tableMap.Name & "'. Would you like to create the column now?", MsgBoxStyle.YesNo, "Create Column") = MsgBoxResult.No Then Return Nothing

        Return MapServices.CreateColumnMap(name, tableMap)

    End Function


    Public Shared Function AskToCreateIfInverseNotExist(ByVal name As String, ByVal propertyMap As IPropertyMap) As IPropertyMap

        'If Len(name) < 1 Then Return Nothing

        If propertyMap Is Nothing Then Return Nothing
        Dim invPropertyMap As IPropertyMap

        Dim refClassMap As IClassMap = propertyMap.GetReferencedClassMap

        Dim invColl As Boolean

        If refClassMap Is Nothing Then Exit Function

        If Len(name) > 0 AndAlso Not refClassMap.GetPropertyMap(name) Is Nothing Then

            invPropertyMap = refClassMap.GetPropertyMap(name)

            Return invPropertyMap

        Else

            If Len(name) > 0 Then

                If MsgBox("There is currently no property named '" & name & "' in the class '" & refClassMap.Name & "'. Would you like to create the inverse property now?", MsgBoxStyle.YesNo, "Create Inverse") = MsgBoxResult.No Then Return Nothing

                Select Case MsgBox("Would you like the new inverse property '" & name & "' in the class '" & refClassMap.Name & "' to be a collection property?", MsgBoxStyle.YesNoCancel, "Create Inverse")

                    Case MsgBoxResult.Yes

                        invColl = True

                    Case MsgBoxResult.No

                        invColl = False

                    Case MsgBoxResult.Cancel

                        Return Nothing

                End Select

            Else

                If MsgBox("There is currently no inverse property in the class '" & refClassMap.Name & "' for the property '" & propertyMap.ClassMap.Name & "." & propertyMap.Name & "'. Would you like to create the inverse property now?", MsgBoxStyle.YesNo, "Create Inverse") = MsgBoxResult.No Then Return Nothing

                Select Case MsgBox("Would you like the new inverse property in the class '" & refClassMap.Name & "' to be a collection property?", MsgBoxStyle.YesNoCancel, "Create Inverse")

                    Case MsgBoxResult.Yes

                        invColl = True

                    Case MsgBoxResult.No

                        invColl = False

                    Case MsgBoxResult.Cancel

                        Return Nothing

                End Select

                Dim tbltocls As New TablesToClasses

                If invColl Then

                    name = tbltocls.MakePluralName(propertyMap.ClassMap.Name)

                Else

                    name = propertyMap.ClassMap.Name

                End If

                name = InputBox("Please enter the name of your new inverse property.", "Create Inverse Property", name)

                If Len(name) < 1 Then Exit Function

            End If

            If propertyMap.IsCollection Then

                If invColl Then

                    propertyMap.ReferenceType = ReferenceType.ManyToMany

                Else

                    propertyMap.ReferenceType = ReferenceType.ManyToOne

                End If

            Else

                If invColl Then

                    propertyMap.ReferenceType = ReferenceType.OneToMany

                Else

                    propertyMap.ReferenceType = ReferenceType.OneToOne

                End If

            End If


            invPropertyMap = MapServices.CreatePropertyMap(name, refClassMap)


            propertyMap.Inverse = name
            invPropertyMap.Inverse = propertyMap.Name

            Select Case propertyMap.ReferenceType

                Case ReferenceType.ManyToMany

                    invPropertyMap.ReferenceType = ReferenceType.ManyToMany

                Case ReferenceType.ManyToOne

                    invPropertyMap.ReferenceType = ReferenceType.OneToMany

                Case ReferenceType.OneToMany

                    invPropertyMap.ReferenceType = ReferenceType.ManyToOne

                Case ReferenceType.OneToOne

                    invPropertyMap.ReferenceType = ReferenceType.OneToOne

            End Select

            Select Case propertyMap.ReferenceType

                Case ReferenceType.ManyToMany, ReferenceType.OneToMany

                    invPropertyMap.IsCollection = True
                    invPropertyMap.DataType = "System.Collections.IList"
                    'invPropertyMap.DefaultValue = "New System.Collections.ArrayList()"
                    invPropertyMap.ItemType = propertyMap.ClassMap.Name
                    invPropertyMap.IsSlave = True
                    invPropertyMap.InheritInverseMappings = True

                Case ReferenceType.ManyToOne

                    invPropertyMap.DataType = propertyMap.ClassMap.Name
                    propertyMap.IsSlave = True
                    propertyMap.InheritInverseMappings = True

                Case ReferenceType.OneToOne

                    invPropertyMap.DataType = propertyMap.ClassMap.Name
                    invPropertyMap.IsSlave = True
                    invPropertyMap.InheritInverseMappings = True

            End Select


            Return invPropertyMap

        End If


    End Function

End Class

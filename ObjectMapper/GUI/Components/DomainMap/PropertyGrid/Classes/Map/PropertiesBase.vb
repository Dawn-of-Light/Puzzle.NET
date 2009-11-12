Imports System.ComponentModel
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public MustInherit Class PropertiesBase
    Implements ICustomTypeDescriptor

    Private m_ShouldReloadProperties As Boolean

    Public MustOverride Function GetMapObject() As IMap

    Public Function ShouldReloadProperties() As Boolean

        Dim blnShouldReloadProperties As Boolean

        blnShouldReloadProperties = m_ShouldReloadProperties

        m_ShouldReloadProperties = False

        Return blnShouldReloadProperties

    End Function

    Public Sub SetShouldReloadProperties()

        m_ShouldReloadProperties = True

    End Sub

    Public Function GetAttributes() As System.ComponentModel.AttributeCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetAttributes
        Return TypeDescriptor.GetAttributes(Me, True)
    End Function

    Public Function GetClassName() As String Implements System.ComponentModel.ICustomTypeDescriptor.GetClassName
        Return TypeDescriptor.GetClassName(Me, True)
    End Function

    Public Function GetComponentName() As String Implements System.ComponentModel.ICustomTypeDescriptor.GetComponentName
        Return TypeDescriptor.GetComponentName(Me, True)
    End Function

    Public Function GetConverter() As System.ComponentModel.TypeConverter Implements System.ComponentModel.ICustomTypeDescriptor.GetConverter
        Return TypeDescriptor.GetConverter(Me, True)
    End Function

    Public Function GetDefaultEvent() As System.ComponentModel.EventDescriptor Implements System.ComponentModel.ICustomTypeDescriptor.GetDefaultEvent
        Return TypeDescriptor.GetDefaultEvent(Me, True)
    End Function

    Public Function GetDefaultProperty() As System.ComponentModel.PropertyDescriptor Implements System.ComponentModel.ICustomTypeDescriptor.GetDefaultProperty
        Return TypeDescriptor.GetDefaultProperty(Me, True)
    End Function

    Public Function GetEditor(ByVal editorBaseType As System.Type) As Object Implements System.ComponentModel.ICustomTypeDescriptor.GetEditor
        Return TypeDescriptor.GetEditor(Me, editorBaseType, True)
    End Function

    Public Overloads Function GetEvents(ByVal attributes() As System.Attribute) As System.ComponentModel.EventDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, attributes, True)
    End Function

    Public Overloads Function GetEvents() As System.ComponentModel.EventDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, True)
    End Function

    Public Overloads Function GetProperties(ByVal attributes() As System.Attribute) As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetProperties
        'Return TypeDescriptor.GetProperties(Me, attributes, True)
        Dim baseProps As PropertyDescriptorCollection = TypeDescriptor.GetProperties(Me, attributes, True)

        baseProps = RemoveHiddenProperties(baseProps)

        Dim metaDataDescriptors As IList = GetMetaDataDescriptors(attributes)

        'Dim newProps(baseProps.Count - 1) As PropertyDescriptor
        Dim newProps(baseProps.Count + metaDataDescriptors.Count - 1) As PropertyDescriptor
        Dim i As Integer
        Dim hideCategories As ArrayList = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings.HidePropGridCategories

        For i = 0 To baseProps.Count - 1

            If Not hideCategories.Contains(baseProps.Item(i).Category) Then

                newProps(i) = CustomizeProperty(New DisplayNamePropertyDescriptor(baseProps.Item(i), attributes))

            End If

        Next

        For Each metaDataDescriptor As MetaDataPropertyDescriptor In metaDataDescriptors

            newProps(i) = metaDataDescriptor
            i += 1

        Next

        Return New PropertyDescriptorCollection(newProps)

    End Function

    Private Function GetMetaDataDescriptors(ByVal attributes() As Attribute) As IList

        Dim list As IList = New ArrayList

        Dim configs As IList = frmDomainMapBrowser.GetCustomMetaDataConfigs
        Dim config As CustomMetaDataConfig
        Dim metadata As CustomMetaData

        Dim map As IMap = GetMapObject()
        If Not map Is Nothing Then

            Dim mapType As Type = CType(map, Object).GetType

            For Each config In configs

                If config.IsActive Then

                    For Each metadata In config.CustomMetaDataList

                        Dim use As Boolean = False

                        If metadata.Targets.Length > 0 Then

                            Dim targets() As String = Split(metadata.Targets, ",")

                            For Each target As String In targets

                                target = Trim(LCase(target))

                                If target.Length > 0 Then

                                    If target = "all" Or target = "" Then

                                        use = True

                                    ElseIf target = "domain" Or target = "domainmap" Then

                                        If Not mapType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

                                            use = True

                                        End If

                                    ElseIf target = "class" Or target = "classmap" Then

                                        If Not mapType.GetInterface(GetType(IClassMap).ToString) Is Nothing Then

                                            use = True

                                        End If

                                    ElseIf target = "property" Or target = "propertymap" Then

                                        If Not mapType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing Then

                                            use = True

                                        End If

                                    ElseIf target = "source" Or target = "sourcemap" Then

                                        If Not mapType.GetInterface(GetType(ISourceMap).ToString) Is Nothing Then

                                            use = True

                                        End If

                                    ElseIf target = "table" Or target = "tablemap" Then

                                        If Not mapType.GetInterface(GetType(ITableMap).ToString) Is Nothing Then

                                            use = True

                                        End If

                                    ElseIf target = "column" Or target = "columnmap" Then

                                        If Not mapType.GetInterface(GetType(IColumnMap).ToString) Is Nothing Then

                                            use = True

                                        End If

                                    End If


                                End If

                                If use Then Exit For

                            Next

                        Else

                            use = True

                        End If

                        If use Then

                            Dim category As String = config.DisplayName & ", " & metadata.Category
                            Dim key As String = config.Name & "." & metadata.Name
                            Dim metaDataDescriptor = New MetaDataPropertyDescriptor(metadata.Name, key, metadata.DisplayName, metadata.Description, category, metadata.DefaultValue, metadata.Type, attributes)
                            list.Add(metaDataDescriptor)

                        End If

                    Next

                End If

            Next

        End If

        Return list

    End Function

    Public Overloads Function GetProperties() As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetProperties
        Return TypeDescriptor.GetProperties(Me, True)
    End Function

    Public Function GetPropertyOwner(ByVal pd As System.ComponentModel.PropertyDescriptor) As Object Implements System.ComponentModel.ICustomTypeDescriptor.GetPropertyOwner
        Return Me
    End Function

    Public Overridable Function RemoveHiddenProperties(ByVal baseProps As PropertyDescriptorCollection) As PropertyDescriptorCollection
        Return baseProps
    End Function

    Public Overridable Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase
        Return propDesc
    End Function


End Class

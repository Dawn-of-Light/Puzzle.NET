Imports Puzzle.NPersist.Framework.Mapping

Public Class MapPropertyGrid
    Inherits System.Windows.Forms.UserControl

    Private WithEvents m_ProjectProperties As New ProjectProperties
    Private WithEvents m_DomainProperties As New DomainProperties
    Private WithEvents m_ClassProperties As New ClassProperties()
    Private WithEvents m_PropertyProperties As New PropertyProperties()
    Private WithEvents m_EnumValueProperties As New EnumValueProperties
    Private WithEvents m_SourceProperties As New SourceProperties
    Private WithEvents m_TableProperties As New TableProperties()
    Private WithEvents m_ColumnProperties As New ColumnProperties()

    Private WithEvents m_DomainConfigProperties As New DomainConfigProperties
    Private WithEvents m_ClassesToCodeConfigProperties As New ClassesToCodeConfigProperties
    Private WithEvents m_ClassesToTablesConfigProperties As New ClassesToTablesConfigProperties
    Private WithEvents m_TablesToClassesConfigProperties As New TablesToClassesConfigProperties
    Private WithEvents m_SourceCodeFileProperties As New SourceCodeFileProperties

    Private WithEvents m_UmlDiagramProperties As New UmlDiagramProperties
    Private WithEvents m_UmlClassProperties As New UmlClassProperties
    Private WithEvents m_UmlLineEndProperties As New UmlLineEndProperties
    Private WithEvents m_UmlLinePointProperties As New UmlLinePointProperties

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents innerGrid As System.Windows.Forms.PropertyGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.innerGrid = New System.Windows.Forms.PropertyGrid()
        Me.SuspendLayout()
        '
        'innerGrid
        '
        Me.innerGrid.CommandsVisibleIfAvailable = True
        Me.innerGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.innerGrid.LargeButtons = False
        Me.innerGrid.LineColor = System.Drawing.SystemColors.ScrollBar
        Me.innerGrid.Name = "innerGrid"
        Me.innerGrid.Size = New System.Drawing.Size(150, 150)
        Me.innerGrid.TabIndex = 0
        Me.innerGrid.Text = "PropertyGrid1"
        Me.innerGrid.ViewBackColor = System.Drawing.SystemColors.Window
        Me.innerGrid.ViewForeColor = System.Drawing.SystemColors.WindowText
        '
        'MapPropertyGrid
        '
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.innerGrid})
        Me.Name = "MapPropertyGrid"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public Event BeforePropertySet(ByVal mapObject As Object, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As Object, ByVal propertyName As String)

    Public Event PropertySortChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Property TheGrid() As PropertyGrid
        Get
            Return innerGrid
        End Get
        Set(ByVal Value As PropertyGrid)
            innerGrid = Value
        End Set
    End Property

    Public Sub RefreshProperties()

        If Not innerGrid.SelectedObject Is Nothing Then
            innerGrid.SelectedObject = innerGrid.SelectedObject
        End If

    End Sub

    Public Sub SelectMapObject(ByVal mapObject As Object, ByVal project As ProjectModel.IProject, Optional ByVal isStart As Boolean = False)

        If mapObject Is Nothing Then Exit Sub
        Dim resource As ProjectModel.IResource
        Dim filePath As String


        If Not mapObject.GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

            m_DomainProperties.SetDomainMap(mapObject)

            resource = project.GetResource(CType(mapObject, IDomainMap).Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

            If Not resource Is Nothing Then

                filePath = resource.FilePath
                m_DomainProperties.SetFilePath(filePath)

            Else

                m_DomainProperties.SetFilePath("")

            End If

            innerGrid.SelectedObject = m_DomainProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(IClassMap).ToString) Is Nothing Then

            m_ClassProperties.SetClassMap(mapObject)
            innerGrid.SelectedObject = m_ClassProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing Then

            m_PropertyProperties.SetPropertyMap(mapObject)
            innerGrid.SelectedObject = m_PropertyProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(IEnumValueMap).ToString) Is Nothing Then

            m_EnumValueProperties.SetEnumValueMap(mapObject)
            innerGrid.SelectedObject = m_EnumValueProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(ISourceMap).ToString) Is Nothing Then

            m_SourceProperties.SetSourceMap(mapObject)
            innerGrid.SelectedObject = m_SourceProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(ITableMap).ToString) Is Nothing Then

            m_TableProperties.SetTableMap(mapObject)
            innerGrid.SelectedObject = m_TableProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(IColumnMap).ToString) Is Nothing Then

            m_ColumnProperties.SetColumnMap(mapObject)
            innerGrid.SelectedObject = m_ColumnProperties

        ElseIf Not mapObject.GetType.GetInterface(GetType(ProjectModel.IProject).ToString) Is Nothing Then

            m_ProjectProperties.SetProject(mapObject)
            innerGrid.SelectedObject = m_ProjectProperties

        ElseIf mapObject.GetType Is GetType(ProjectModel.DomainConfig) Then

            m_DomainConfigProperties.SetDomainConfig(mapObject)
            innerGrid.SelectedObject = m_DomainConfigProperties


        ElseIf mapObject.GetType Is GetType(ProjectModel.ClassesToCodeConfig) Then

            m_ClassesToCodeConfigProperties.SetClassesToCodeConfig(mapObject)
            innerGrid.SelectedObject = m_ClassesToCodeConfigProperties

        ElseIf mapObject.GetType Is GetType(ProjectModel.ClassesToTablesConfig) Then

            m_ClassesToTablesConfigProperties.SetClassesToTablesConfig(mapObject)
            innerGrid.SelectedObject = m_ClassesToTablesConfigProperties

        ElseIf mapObject.GetType Is GetType(ProjectModel.TablesToClassesConfig) Then

            m_TablesToClassesConfigProperties.SetTablesToClassesConfig(mapObject)
            innerGrid.SelectedObject = m_TablesToClassesConfigProperties

        ElseIf mapObject.GetType Is GetType(ProjectModel.SourceCodeFile) Then

            m_SourceCodeFileProperties.SetSourceCodeFile(mapObject)
            innerGrid.SelectedObject = m_SourceCodeFileProperties

        ElseIf mapObject.GetType Is GetType(Uml.UmlDiagram) Then

            m_UmlDiagramProperties.SetUmlDiagram(mapObject)
            innerGrid.SelectedObject = m_UmlDiagramProperties

        ElseIf mapObject.GetType Is GetType(Uml.UmlClass) Then

            m_UmlClassProperties.SetUmlClass(mapObject)
            innerGrid.SelectedObject = m_UmlClassProperties

        ElseIf mapObject.GetType Is GetType(Uml.UmlLine) Then

            m_UmlLineEndProperties.SetUmlLine(mapObject)
            m_UmlLineEndProperties.SetIsStart(isStart)
            innerGrid.SelectedObject = m_UmlLineEndProperties

        ElseIf mapObject.GetType Is GetType(Uml.UmlLinePoint) Then

            m_UmlLinePointProperties.SetUmlLinePoint(mapObject)
            innerGrid.SelectedObject = m_UmlLinePointProperties

        Else

            innerGrid.SelectedObject = mapObject

        End If

        If Not innerGrid.SelectedObject Is Nothing Then

            If Not LogServices.mainForm Is Nothing Then

                If Not LogServices.mainForm.m_ApplicationSettings Is Nothing Then

                    If LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings.AlphabeticPropertyGrid Then

                        innerGrid.PropertySort = PropertySort.Alphabetical

                    Else

                        innerGrid.PropertySort = PropertySort.CategorizedAlphabetical

                    End If

                End If

            End If

        End If

    End Sub

    Public Sub OnObjectDelete(ByVal obj As Object, Optional ByVal force As Boolean = False)

        Try

            If Not innerGrid.SelectedObject Is Nothing Then

                If Not CType(innerGrid.SelectedObject, PropertiesBase).GetMapObject Is Nothing Then

                    If force OrElse obj Is CType(innerGrid.SelectedObject, PropertiesBase).GetMapObject Then

                        innerGrid.SelectedObject = Nothing

                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub m_DomainProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal propertyName As String) Handles m_DomainProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_DomainProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_DomainProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_ClassProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IClassMap, ByVal propertyName As String) Handles m_ClassProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_ClassProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IClassMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_ClassProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_PropertyProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IPropertyMap, ByVal propertyName As String) Handles m_PropertyProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_PropertyProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IPropertyMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_PropertyProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_EnumValueProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IEnumValueMap, ByVal propertyName As String) Handles m_EnumValueProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_EnumValueProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IEnumValueMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_EnumValueProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_SourceProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal propertyName As String) Handles m_SourceProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_SourceProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_SourceProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub


    Private Sub m_TableProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal propertyName As String) Handles m_TableProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_TableProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_TableProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_ColumnProperties_AfterPropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IColumnMap, ByVal propertyName As String) Handles m_ColumnProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_ColumnProperties_BeforePropertySet(ByVal mapObject As Puzzle.NPersist.Framework.Mapping.IColumnMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_ColumnProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub


    Private Sub m_ProjectProperties_AfterPropertySet(ByVal mapObject As ProjectModel.IProject, ByVal propertyName As String) Handles m_ProjectProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_ProjectProperties_BeforePropertySet(ByVal mapObject As ProjectModel.IProject, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_ProjectProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub innerGrid_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles innerGrid.PropertyValueChanged

        'OBS!
        'UUugglyyy! Only while experimenting in order to be able to show objects that are not yet inheriting PropertiesBase
        Try

            If CType(innerGrid.SelectedObject, PropertiesBase).ShouldReloadProperties Then
                innerGrid.SelectedObject = innerGrid.SelectedObject
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub m_DomainConfigProperties_AfterPropertySet(ByVal mapObject As ProjectModel.DomainConfig, ByVal propertyName As String) Handles m_DomainConfigProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_DomainConfigProperties_BeforePropertySet(ByVal mapObject As ProjectModel.DomainConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_DomainConfigProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_ClassesToCodeConfigProperties_AfterPropertySet(ByVal mapObject As ProjectModel.ClassesToCodeConfig, ByVal propertyName As String) Handles m_ClassesToCodeConfigProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_ClassesToCodeConfigProperties_BeforePropertySet(ByVal mapObject As ProjectModel.ClassesToCodeConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_ClassesToCodeConfigProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_ClassesToTablesConfigProperties_AfterPropertySet(ByVal mapObject As ProjectModel.ClassesToTablesConfig, ByVal propertyName As String) Handles m_ClassesToTablesConfigProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_ClassesToTablesConfigProperties_BeforePropertySet(ByVal mapObject As ProjectModel.ClassesToTablesConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_ClassesToTablesConfigProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub


    Private Sub m_TablesToClassesConfigProperties_AfterPropertySet(ByVal mapObject As ProjectModel.TablesToClassesConfig, ByVal propertyName As String) Handles m_TablesToClassesConfigProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_TablesToClassesConfigProperties_BeforePropertySet(ByVal mapObject As ProjectModel.TablesToClassesConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_TablesToClassesConfigProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub


    Private Sub m_UmlClassProperties_AfterPropertySet(ByVal mapObject As Object, ByVal propertyName As String) Handles m_UmlClassProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_UmlClassProperties_BeforePropertySet(ByVal mapObject As Object, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_UmlClassProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_UmlDiagramProperties_AfterPropertySet(ByVal mapObject As Uml.UmlDiagram, ByVal propertyName As String) Handles m_UmlDiagramProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_UmlDiagramProperties_BeforePropertySet(ByVal mapObject As Uml.UmlDiagram, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_UmlDiagramProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_UmlLineEndProperties_AfterPropertySet(ByVal mapObject As Object, ByVal propertyName As String) Handles m_UmlLineEndProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_UmlLineEndProperties_BeforePropertySet(ByVal mapObject As Object, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_UmlLineEndProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub m_UmlLinePointProperties_AfterPropertySet(ByVal mapObject As Uml.UmlLinePoint, ByVal propertyName As String) Handles m_UmlLinePointProperties.AfterPropertySet

        RaiseEvent AfterPropertySet(mapObject, propertyName)

    End Sub

    Private Sub m_UmlLinePointProperties_BeforePropertySet(ByVal mapObject As Uml.UmlLinePoint, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles m_UmlLinePointProperties.BeforePropertySet

        RaiseEvent BeforePropertySet(mapObject, propertyName, value, oldValue)

    End Sub

    Private Sub innerGrid_PropertySortChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles innerGrid.PropertySortChanged

        RaiseEvent PropertySortChanged(sender, e)

    End Sub

End Class

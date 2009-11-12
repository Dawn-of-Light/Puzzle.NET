Imports Puzzle.NPersist.Framework.Mapping
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Public Class AdditionalColumnsEditor
    Inherits UITypeEditor

    Private m_edSvcs As IWindowsFormsEditorService
    Private m_Editor As frmColumnListEditor

    Public Overloads Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        If Not context Is Nothing Then
            If Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.Modal
            End If
        End If
        Return MyBase.GetEditStyle(context)
    End Function


    Public Overloads Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object
        Dim propertyMap As IPropertyMap
        If Not context Is Nothing Then
            If Not context.Instance Is Nothing Then
                If Not provider Is Nothing Then
                    m_edSvcs = provider.GetService(GetType(IWindowsFormsEditorService))

                    If Not m_edSvcs Is Nothing Then
                        If m_Editor Is Nothing Then
                            m_Editor = New frmColumnListEditor()
                        End If
                        If context.Instance.GetType Is GetType(PropertyProperties) Then
                            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap
                        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then
                            propertyMap = CType(context.Instance, UmlLineEndProperties).GetpropertyMap
                        End If
                        If Not propertyMap Is Nothing Then
                            m_Editor.Setup(propertyMap, False)
                            m_Editor.ShowDialog(context.Container)
                        End If
                        Return m_Editor.GetColumns
                    End If
                End If
            End If
        End If
    End Function
End Class

Imports System.ComponentModel
Imports Puzzle.NPersist.Framework.Mapping

Public Class MetaDataPropertyDescriptor
    Inherits PropertyDescriptorBase

    Private m_Key As String = ""
    Private m_DisplayName As String = ""
    Private m_Description As String = ""
    Private m_DefaultValue As String = ""
    Private m_Category As String = ""
    Private m_Type As String = ""

    Public Sub New(ByVal name As String, ByVal key As String, ByVal displayName As String, ByVal description As String, ByVal category As String, ByVal defaultValue As String, ByVal type As String, ByVal filter As Attribute())
        MyBase.New(name, filter)

        m_Key = key
        m_DisplayName = displayName
        m_Description = description
        m_DefaultValue = defaultValue
        m_Category = category
        m_Type = type

        If m_DefaultValue.Length > 0 Then


        End If

        Dim attributes(2) As Attribute
        attributes(0) = New CategoryAttribute(m_Category)
        attributes(1) = New DescriptionAttribute(m_Description)
        attributes(2) = New DefaultValueAttribute(m_DefaultValue)

        m_AttributeCollection = New AttributeCollection(attributes)

    End Sub

    Public ReadOnly Property Key() As String
        Get
            If m_Key.Length < 1 Then
                Return Me.Name
            End If
            Return m_Key
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayName() As String
        Get
            If m_DisplayName.Length > 0 Then
                Return m_DisplayName
            Else
                Return Me.Name
            End If
        End Get
    End Property

    Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
        Return True
        'Return baseProp.CanResetValue(component)
    End Function

    Public Overrides ReadOnly Property ComponentType() As System.Type
        Get
            'Return baseProp.ComponentType
        End Get
    End Property

    Public Overrides Function GetValue(ByVal component As Object) As Object
        Dim map As IMap = GetMap(component)
        If Not map Is Nothing Then
            Dim value As String = map.GetMetaData(Me.Key)
            If value Is Nothing Then value = ""
            If value.Length < 1 Then
                Return m_DefaultValue
            End If
            Return value
        End If
    End Function

    Public Overrides ReadOnly Property PropertyType() As System.Type
        Get
            Return GetType(String)
            'Return baseProp.PropertyType
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)
        Dim map As IMap = GetMap(component)
        If Not map Is Nothing Then
            map.RemoveMetaData(Me.Key)
        End If
    End Sub

    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
        Dim map As IMap = GetMap(component)
        If Not map Is Nothing Then
            Dim str As String = ""
            If Not value Is Nothing Then
                str = value.ToString()
            End If
            If str.Length < 1 OrElse str = m_DefaultValue Then
                map.RemoveMetaData(Me.Key)
            Else
                map.SetMetaData(Me.Key, str)
            End If
        End If
    End Sub

    Private Function GetMap(ByVal component As Object) As IMap
        If Not component Is Nothing Then
            If GetType(PropertiesBase).IsAssignableFrom(component.GetType()) Then
                Dim properties As PropertiesBase = component
                Dim map As IMap = properties.GetMapObject
                Return map
            End If
        End If
        Return Nothing
    End Function

    Dim m_AttributeCollection As AttributeCollection

    Public Overrides ReadOnly Property Attributes() As AttributeCollection
        Get
            Return m_AttributeCollection
        End Get
    End Property

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return True
    End Function

End Class

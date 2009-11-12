Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO

Namespace ProjectModel

    Public Class CustomMetaDataConfig

        Private m_FileName As String


        Public Shared Function Load(ByVal fileName As String) As CustomMetaDataConfig
            Dim reader As StreamReader = Nothing
            Dim xml As String = ""
            Try
                reader = File.OpenText(fileName)
                xml = reader.ReadToEnd()
                reader.Close()
                reader = Nothing
            Catch ex As Exception
                If Not reader Is Nothing Then
                    Try
                        reader.Close()
                    Catch
                    End Try
                End If
                Throw New IOException("Could not load custom metadata config xml mapping file: " + fileName, ex)
            End Try
            Return Deserialize(xml)

        End Function

        Public Shared Function Deserialize(ByVal xml As String) As CustomMetaDataConfig

            Dim xmlDoc As XmlDocument = New XmlDocument
            Dim xmlConfig As XmlNode
            xmlDoc.LoadXml(xml)
            xmlConfig = xmlDoc.SelectSingleNode("config")
            Return DeserializeConfig(xmlConfig)

        End Function

        Public Shared Function DeserializeConfig(ByVal xmlConfig As XmlNode) As CustomMetaDataConfig

            Dim config As CustomMetaDataConfig = New CustomMetaDataConfig

            If Not xmlConfig.Attributes("name") Is Nothing Then
                config.Name = xmlConfig.Attributes("name").Value
            End If

            If Not xmlConfig.Attributes("display-name") Is Nothing Then
                config.DisplayName = xmlConfig.Attributes("display-name").Value
            End If

            Dim xmlMetaDataList As XmlNodeList = xmlConfig.SelectNodes("metadata")
            For Each xmlMetaData As XmlNode In xmlMetaDataList
                Dim metadata As CustomMetaData = DeserializeMetaData(xmlMetaData)
                config.CustomMetaDataList.Add(metadata)
            Next

            Return config

        End Function


        Public Shared Function DeserializeMetaData(ByVal xmlMetaData As XmlNode) As CustomMetaData

            Dim metadata As CustomMetaData = New CustomMetaData

            If Not xmlMetaData.Attributes("name") Is Nothing Then
                metadata.Name = xmlMetaData.Attributes("name").Value
            End If

            If Not xmlMetaData.Attributes("display-name") Is Nothing Then
                metadata.DisplayName = xmlMetaData.Attributes("display-name").Value
            End If

            If Not xmlMetaData.Attributes("category") Is Nothing Then
                metadata.Category = xmlMetaData.Attributes("category").Value
            End If

            If Not xmlMetaData.Attributes("default") Is Nothing Then
                metadata.DefaultValue = xmlMetaData.Attributes("default").Value
            End If

            If Not xmlMetaData.Attributes("description") Is Nothing Then
                metadata.Description = xmlMetaData.Attributes("description").Value
            End If

            If Not xmlMetaData.Attributes("targets") Is Nothing Then
                metadata.Targets = xmlMetaData.Attributes("targets").Value
            End If

            If Not xmlMetaData.Attributes("type") Is Nothing Then
                metadata.Type = xmlMetaData.Attributes("type").Value
            End If

            Return metadata

        End Function

        Public Sub Save(ByVal path As String)

            Try

                If path = "" Then path = m_FileName

                If path = "" Then Exit Sub

                Dim mySerializer As XmlSerializer = New XmlSerializer(Me.GetType)
                ' To write to a file, create a StreamWriter object.
                Dim myWriter As StreamWriter = New StreamWriter(path)
                mySerializer.Serialize(myWriter, Me)
                myWriter.Close()

                m_FileName = path

            Catch ex As Exception

            End Try

        End Sub

        Private m_name As String = ""

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal Value As String)
                m_name = Value
            End Set
        End Property

        Private m_DisplayName As String = ""

        Public Property DisplayName() As String
            Get
                Return m_DisplayName
            End Get
            Set(ByVal Value As String)
                m_DisplayName = Value
            End Set
        End Property


        Private m_CustomMetaData As ArrayList = New ArrayList

        <XmlArrayItem(GetType(CustomMetaData))> Public Property CustomMetaDataList() As ArrayList
            Get
                Return m_CustomMetaData
            End Get
            Set(ByVal Value As ArrayList)
                m_CustomMetaData = Value
            End Set
        End Property

        Private m_isActive As Boolean = True

        Public Property IsActive() As Boolean
            Get
                Return m_isActive
            End Get
            Set(ByVal Value As Boolean)
                m_isActive = Value
            End Set
        End Property

    End Class

End Namespace

Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports Puzzle.ObjectMapper.Tools
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.IO
Imports System.Xml.Serialization

Public Class WrapDbWizardService


    Public Function ExecuteWizard( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMapName As String, _
        ByVal sourceTypeName As String, _
        ByVal providerTypeName As String, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal copyOriginal As Boolean, _
        ByVal copyResult As Boolean) As String

        Dim domainMap As IDomainMap
        Dim sourceType As SourceType
        Dim providerType As ProviderType

        Dim resource As ProjectModel.IResource

        For Each resource In project.Resources

            If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                If resource.Name = domainMapName Then

                    domainMap = resource.GetResource

                    Exit For

                End If

            End If

        Next

        Select Case sourceTypeName

            Case "Microsoft SQL Server/MSDE"

                sourceType = SourceType.MSSqlServer

            Case "Microsoft Access 4.0"

                sourceType = SourceType.MSAccess

            Case "Borland InterBase"

                sourceType = SourceType.Interbase

        End Select

        Select Case providerTypeName

            Case "System.Data.SqlClient"

                providerType = ProviderType.SqlClient

            Case "System.Data.OleDb"

                providerType = ProviderType.OleDb

            Case "System.Data.Odbc"

                providerType = ProviderType.SqlClient

            Case "Borland.Data.Provider"

                providerType = ProviderType.Bdp

        End Select

        Select Case targetLanguage

            Case "Microsoft C#"

                targetLanguage = "cs"

            Case "Microsoft Visual Basic.NET"

                targetLanguage = "vb"

            Case "Borland Delphi for .NET"

                targetLanguage = "pas"

        End Select

        Return DoExecuteWizard( _
            project, _
            domainMap, _
            sourceType, _
            providerType, _
            connectionString, _
            targetLanguage, _
            targetDirectory, _
            copyOriginal, _
            copyResult)

    End Function

    Private Function DoExecuteWizard( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal copyOriginal As Boolean, _
        ByVal copyResult As Boolean) As String

        Dim retStr As String

        Dim resource As ProjectModel.IResource

        Dim newDomainMap As IDomainMap

        Dim cfg As DomainConfig

        Try

            'Create the folder to hold the source code
            targetDirectory = CreateTargetDirectory( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory)

        Catch ex As Exception

            Throw New Exception("Failed creating output directory! Reason:" & vbCrLf & ex.Message)

        End Try

        If copyOriginal Then

            'Create a copy of the unmodified domain map
            CopyDomain( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory, _
                "Old version of ")

        End If


        'Begin by adding a new source map to the domain map 
        'and setting it as default
        Dim sourceMap As ISourceMap = CreateSourceMap( _
            project, _
            domainMap, _
            sourceType, _
            providerType, _
            connectionString, _
            targetLanguage, _
            targetDirectory)

        Try

            'Connect to and analyze the database,
            'generating the table model
            newDomainMap = DoSourceToTables( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory, _
                sourceMap)

            'Merge the models
            MapServices.MargeDomains(domainMap, newDomainMap)

        Catch ex As Exception

            Throw New Exception("Failed extracting table model from database! Reason:" & vbCrLf & ex.Message)

        End Try

        Try

            'Generate the class model and O/R Mappings
            newDomainMap = DoTablesToClasses( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory, _
                sourceMap)

            'Merge the models
            MapServices.MargeDomains(domainMap, newDomainMap)

        Catch ex As Exception

            Throw New Exception("Failed generating class model from table model! Reason:" & vbCrLf & ex.Message)

        End Try


        'Create a new tool config and set as current
        cfg = CreateToolConfig( _
            project, _
            domainMap, _
            sourceType, _
            providerType, _
            connectionString, _
            targetLanguage, _
            targetDirectory, _
            sourceMap)

        retStr = cfg.Name


        Try

            'Generate the source code
            DoClassesToCode( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory, _
                sourceMap, _
                cfg)

        Catch ex As Exception

            Throw New Exception("Failed generating the source code for the class model! Reason:" & vbCrLf & ex.Message)

        End Try


        If copyResult Then

            'Create a copy of the updated domain map
            CopyDomain( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory, _
                "")

        End If

        Return retStr

    End Function

    Private Function DoSourceToTables( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal sourceMap As ISourceMap) As IDomainMap

        Dim tool As ISourceToTables



        Select Case sourceType

            Case SourceType.MSSqlServer

                Select Case providerType

                    Case ProviderType.SqlClient

                        tool = New SourceToTablesMSSqlServer

                    Case ProviderType.Bdp

                        'tool = New SourceToTablesBdpSqlServer

                    Case Else

                End Select

            Case SourceType.MSAccess

                Select Case providerType

                    Case ProviderType.OleDb

                        tool = New SourceToTablesMSAccess

                    Case Else

                End Select

            Case SourceType.Interbase

                Select Case providerType

                    Case ProviderType.Bdp

                        'tool = New SourceToTablesBdpInterbase

                    Case Else

                        Throw New Exception("Database + provider combination not supported by this wizard!")

                End Select

            Case Else

                Throw New Exception("Database not supported by this wizard!")

        End Select


        Dim targetDomainMap As IDomainMap = New domainMap

        Dim newSourceMap As ISourceMap

        domainMap.Copy(targetDomainMap)

        newSourceMap = New sourceMap

        sourceMap.Copy(newSourceMap)

        newSourceMap.DomainMap = targetDomainMap

        Dim hashDiff As New Hashtable

        tool.SourceToTables(sourceMap, targetDomainMap, hashDiff)

        Return targetDomainMap

    End Function


    Private Function DoTablesToClasses( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal sourceMap As ISourceMap) As IDomainMap

        Dim tool As ITablesToClasses = New TablesToClasses

        Dim targetDomainMap As IDomainMap = New domainMap

        domainMap.Copy(targetDomainMap)

        tool.GenerateClassesForTables(sourceMap, targetDomainMap, True)

        Return targetDomainMap

    End Function


    Private Function CreateToolConfig( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal sourceMap As ISourceMap) As DomainConfig

        Dim name As String = GetNewConfigName( _
                project, _
                domainMap, _
                sourceType, _
                providerType, _
                connectionString, _
                targetLanguage, _
                targetDirectory, _
                sourceMap)

        Dim resource As ProjectModel.IResource = project.GetResource(domainMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        Dim cfg As ProjectModel.DomainConfig

        For Each cfg In resource.Configs

            cfg.IsActive = False

        Next

        cfg = New ProjectModel.DomainConfig

        cfg.Name = name
        cfg.IsActive = True

        Select Case targetLanguage

            Case "cs"

                cfg.ClassesToCodeConfig.TargetLanguage = ProjectModel.SourceCodeFileTypeEnum.CSharp

            Case "vb"

                cfg.ClassesToCodeConfig.TargetLanguage = ProjectModel.SourceCodeFileTypeEnum.VB

            Case "pas"

                cfg.ClassesToCodeConfig.TargetLanguage = ProjectModel.SourceCodeFileTypeEnum.Delphi

        End Select

        cfg.Description = ""

        cfg.ClassesToCodeConfig.IncludeComments = True
        cfg.ClassesToCodeConfig.IncludeDocComments = True
        cfg.ClassesToCodeConfig.IncludeModelInfoInDocComments = False
        cfg.ClassesToCodeConfig.IncludeRegions = False
        cfg.ClassesToCodeConfig.NotifyAfterGet = True
        cfg.ClassesToCodeConfig.NotifyAfterSet = True
        cfg.ClassesToCodeConfig.NotifyLightWeight = False
        cfg.ClassesToCodeConfig.NotifyOnlyWhenRequired = False

        resource.Configs.Add(cfg)

        Return cfg

    End Function

    Private Function GetNewConfigName( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal sourceMap As ISourceMap) As String

        Dim newName As String

        Dim baseName As String = "Wrap Database Wizard Config"

        Dim i As Long

        Dim cfg As DomainConfig

        Dim resource As ProjectModel.IResource = project.GetResource(domainMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        newName = baseName

        While ConfigNameExists(newName, resource)

            i += 1

            newName = baseName & " " & i

        End While

        Return newName

    End Function

    Private Function ConfigNameExists(ByVal name As String, ByVal resource As IResource) As Boolean

        Dim cfg As DomainConfig

        For Each cfg In resource.Configs

            If LCase(cfg.Name) = LCase(name) Then

                Return True

            End If

        Next


    End Function


    Private Function CreateTargetDirectory( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String) As String

        Dim path As String = targetDirectory & "\" & domainMap.Name

        If Not Directory.Exists(path) Then

            Directory.CreateDirectory(path)

        End If

        Return path

    End Function

    Private Function CopyDomain( _
    ByVal project As ProjectModel.IProject, _
    ByVal domainMap As IDomainMap, _
    ByVal sourceType As SourceType, _
    ByVal providerType As ProviderType, _
    ByVal connectionString As String, _
    ByVal targetLanguage As String, _
    ByVal targetDirectory As String, _
    ByVal fileNameSuffix As String) As String

        Dim path As String = targetDirectory & "\" & fileNameSuffix & domainMap.Name & ".npersist"

        Try

            Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(domainMap))
            Dim myWriter As StreamWriter = New StreamWriter(path)
            mySerializer.Serialize(myWriter, domainMap)
            myWriter.Close()

        Catch ex As Exception

            Throw New Exception("Could not serialize Domain Map! " & ex.Message, ex)

        End Try


    End Function

    Private Sub DoClassesToCode( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal sourceMap As ISourceMap, _
        ByVal cfg As DomainConfig)

        Dim domainMaps As ArrayList
        Dim useCfg As ClassesToCodeConfig = cfg.ClassesToCodeConfig
        Dim src As SourceCodeFile
        Dim removeSrc As New ArrayList
        Dim fileInfo As fileInfo
        Dim newDomainMap As IDomainMap = New domainMap
        Dim fileName As String
        Dim code As String
        Dim classMap As IClassMap
        Dim fileWriter As StreamWriter
        Dim noWrite As Boolean
        Dim classMapsAndFiles As New Hashtable
        Dim ext As String
        Dim isUnSynched As Boolean
        Dim customCode As String

        Dim resource As IResource

        Dim classesToCode As IClassesToCode

        Select Case targetLanguage

            Case "cs"

                classesToCode = New ClassesToCodeCs

            Case "vb"

                classesToCode = New ClassesToCodeVb

            Case "pas"

                classesToCode = New ClassesToCodeDelphi

        End Select

        resource = project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

        'Prune source files

        For Each src In useCfg.SourceCodeFiles

            If Not File.Exists(src.FilePath) Then

                removeSrc.Add(src)

            End If

        Next

        For Each src In removeSrc

            useCfg.SourceCodeFiles.Remove(src)

        Next


        'Generate classes

        For Each classMap In domainMap.ClassMaps

            noWrite = False

            customCode = ""

            src = useCfg.GetSourceCodeFile(classMap)

            If src Is Nothing Then

                fileName = targetDirectory & "\" & classMap.GetName

                Select Case useCfg.TargetLanguage

                    Case SourceCodeFileTypeEnum.CSharp

                        fileName += ".cs"

                    Case SourceCodeFileTypeEnum.VB

                        fileName += ".vb"

                    Case SourceCodeFileTypeEnum.Delphi

                        fileName += ".pas"

                End Select

                src = useCfg.SetSourceCodeFile(classMap, fileName)

                src.FileType = useCfg.TargetLanguage

            Else

                fileName = src.FilePath

                customCode = GetCustomCode(src, classMap)

            End If

            code = classesToCode.ClassToCode(classMap, False, True, customCode)

            fileWriter = File.CreateText(fileName)

            fileWriter.Write(code)

            fileWriter.Close()

            classMapsAndFiles(classMap) = fileName

            src.LastWrittenTo = File.GetLastWriteTime(fileName)

        Next



        'Generate project

        src = useCfg.GetSourceCodeFile(domainMap)

        If src Is Nothing Then

            fileName = targetDirectory & "\" & domainMap.Name

            Select Case useCfg.TargetLanguage

                Case SourceCodeFileTypeEnum.CSharp

                    fileName += ".csproj"

                Case SourceCodeFileTypeEnum.VB

                    fileName += ".vbproj"

                Case SourceCodeFileTypeEnum.Delphi

                    fileName += ".bdsproj"

            End Select

            src = useCfg.SetSourceCodeFile(domainMap, fileName)

            src.FileType = useCfg.TargetLanguage

        Else

            fileName = src.FilePath

        End If

        code = classesToCode.DomainToProject(domainMap, targetDirectory, classMapsAndFiles, New ArrayList)

        fileWriter = File.CreateText(fileName)

        fileWriter.Write(code)

        fileWriter.Close()

        src.LastWrittenTo = File.GetLastWriteTime(fileName)



        'AssemblyInfo


        code = classesToCode.DomainToAssemblyInfo(domainMap)

        Select Case useCfg.TargetLanguage

            Case SourceCodeFileTypeEnum.CSharp

                ext = ".cs"

            Case SourceCodeFileTypeEnum.VB

                ext = ".vb"

            Case SourceCodeFileTypeEnum.VB

                ext = ".dpr"

        End Select

        src = useCfg.GetSourceCodeFile("AssemblyInfo")

        If src Is Nothing Then

            fileName = targetDirectory & "\AssemblyInfo" & ext

            src = useCfg.SetSourceCodeFile("AssemblyInfo", fileName)

            src.FileType = useCfg.TargetLanguage

        Else

            fileName = src.FilePath

        End If


        fileWriter = File.CreateText(fileName)

        fileWriter.Write(code)

        fileWriter.Close()

        src.LastWrittenTo = File.GetLastWriteTime(fileName)


        'Framework dll

        If Not Directory.Exists(targetDirectory & "\bin") Then

            Directory.CreateDirectory(targetDirectory & "\bin")

        End If



        fileName = targetDirectory & "\bin\Puzzle.NPersist.Framework.dll"

        src = useCfg.GetSourceCodeFile("Puzzle.NPersist.Framework.dll")

        If File.Exists(Application.StartupPath & "\Puzzle.NPersist.Framework.dll") Then

            Try

                File.Copy(Application.StartupPath & "\Puzzle.NPersist.Framework.dll", fileName, True)

            Catch ex As Exception


            End Try

        End If


        If src Is Nothing Then

            src = useCfg.SetSourceCodeFile("Puzzle.NPersist.Framework.dll", fileName)

            src.FileType = SourceCodeFileTypeEnum.Other

        Else

            src.FilePath = fileName

        End If

        src.LastWrittenTo = File.GetLastWriteTime(fileName)

    End Sub

    Private Function GetCustomCode(ByVal src As SourceCodeFile, ByVal classMap As IClassMap) As String

        Dim fullcode As String
        Dim code As String
        Dim pos1 As Integer
        Dim pos2 As Integer
        Dim tag1 As String
        Dim tag2 As String
        Dim fileReader As StreamReader

        Dim fileInfo As New fileInfo(src.FilePath)

        If fileInfo.Exists() Then

            fileReader = fileInfo.OpenText

            fullcode = fileReader.ReadToEnd

            fileReader.Close()
            tag1 = "#Region "" Unsynchronized Custom Code Region """ & vbCrLf
            tag2 = "#End Region 'Unsynchronized Custom Code Region" & vbCrLf

            Select Case src.FileType

                Case SourceCodeFileTypeEnum.CSharp
                    tag1 = "#region "" Unsynchronized Custom Code Region """ & vbCrLf
                    tag2 = "#endregion //Unsynchronized Custom Code Region" & vbCrLf

                Case SourceCodeFileTypeEnum.VB

                Case SourceCodeFileTypeEnum.Delphi
                    tag1 = "{$REGION  ' Unsynchronized Custom Code Region '}" & vbCrLf
                    tag2 = "{$ENDREGION} //Unsynchronized Custom Code Region" & vbCrLf

            End Select


            If Len(fullcode) > Len(tag1 & tag2) Then

                pos1 = fullcode.IndexOf(tag1)

                If pos1 > -1 Then

                    pos2 = fullcode.IndexOf(tag2, pos1 + 1)

                    If pos2 > pos1 Then

                        code = fullcode.Substring(pos1 + Len(tag1), pos2 - pos1 - Len(tag1))

                    End If

                End If

            End If

            Return code


        End If

    End Function


    Private Sub DoTablesToSource( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String, _
        ByVal sourceMap As ISourceMap)

        Dim tool As ITablesToSource

        Select Case sourceType

            Case SourceType.MSSqlServer

                tool = New TablesToSourceMSSqlServer

            Case SourceType.MSAccess

                tool = New TablesToSourceMSAccess

        End Select

        tool.CommitTablesToSource(sourceMap)

    End Sub

    Private Function CreateSourceMap( _
        ByVal project As ProjectModel.IProject, _
        ByVal domainMap As IDomainMap, _
        ByVal sourceType As SourceType, _
        ByVal providerType As ProviderType, _
        ByVal connectionString As String, _
        ByVal targetLanguage As String, _
        ByVal targetDirectory As String) As ISourceMap

        Dim sourceMap As ISourceMap

        For Each sourceMap In domainMap.SourceMaps

            If sourceMap.Name = domainMap.Source Then

                If sourceMap.ConnectionString = "" Or _
                    sourceMap.ConnectionString = connectionString Then

                    sourceMap.SourceType = sourceType

                    sourceMap.ProviderType = providerType

                    sourceMap.ConnectionString = connectionString

                    Return sourceMap

                Else

                    'TODO: Update all classes and properties
                    'mapping to the current default source so that the
                    'references become explicit! (Set thier 'Source' properties..)

                End If

            End If

        Next

        sourceMap = New sourceMap

        sourceMap.Name = domainMap.Name

        sourceMap.DomainMap = domainMap

        domainMap.Source = sourceMap.Name

        sourceMap.SourceType = sourceType

        sourceMap.ProviderType = providerType

        sourceMap.ConnectionString = connectionString

        Return sourceMap

    End Function



End Class

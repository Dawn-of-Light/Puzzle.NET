Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports Puzzle.NPersist.Framework.Mapping.Serialization
Imports Puzzle.ObjectMapper.Plugin
Imports Puzzle.ObjectMapper.Tools
Imports System.Xml.Serialization
Imports System.IO
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports Puzzle.ObjectMapper.GUI.Uml
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Windows.Forms.Cursor
Imports System.Diagnostics
Imports Puzzle.Windows.Forms

Public Class frmDomainMapBrowser
    Inherits System.Windows.Forms.Form

#Region " Private fields "

    Public Shared m_CustomMetaDataConfigs As IList = New ArrayList

    Public Shared m_Beta As Boolean = False ' True
    Public Shared m_BetaNr As Integer = 0

    Public Shared PRODUCT_NAME As String = "Puzzle ObjectMapper"
    Public Shared OLD_PRODUCT_NAME As String = "ObjectMapper 2004"

    Public Shared Converters As New ArrayList

    Private PropGridWithTreeView As Boolean '= True

    Public m_ApplicationSettings As New ApplicationSettings

    Public m_Project As ProjectModel.IProject


    Public WithEvents m_PropTreeSplitter As Splitter

    Private m_DoLoadCustomMetaDataConfigs As Boolean = False


    Private m_started As Boolean = False

    Private m_RunPreviewDTD As Boolean '= True

    Private m_EcoSupported As Boolean = True

    Private m_settingUp As Boolean = True


    Private m_PluginTable As New Hashtable

    Private m_TimerCnt As Integer

    Private m_ProjectLoadedFromPath As String
    Private m_ProjectSavedToPath As String
    Public m_ProjectDirty As Boolean

    Private m_CurrSaveObject As Object

    Private m_CurrentCopyTarget As Object
    Private m_SelectedCopySource As Object
    Private m_CutFlag As Boolean


    Private m_Updating As Boolean
    Private m_IsVerifying As Boolean

    Private m_ActionType As ActionTypeEnum = ActionTypeEnum.None
    Private m_SynchMode As SynchModeEnum = SynchModeEnum.None

    Private m_hashSynchDiff As Hashtable = New Hashtable

    Private m_DynamicCreateIdProperty As Boolean = True
    Private m_DynamicCreateSource As Boolean '= True

    Private m_contextMenuNode As MapNode
    Private m_contextMenuMapObject As Object
    Private m_contextMenuNamespace As String
    Private m_contextMenuDomainMap As IDomainMap
    Private m_contextMenuParentMap As Object
    Private m_contextMenuIsStart As Boolean

    Private m_ClassesToTables As IClassesToTables = New ClassesToTables
    Private m_TablesToClasses As ITablesToClasses = New TablesToClasses

    Private m_ClassesToCodeVb As IClassesToCode = New ClassesToCodeVb
    Private m_ClassesToCodeCs As IClassesToCode = New ClassesToCodeCs
    Private m_ClassesToCodeDelphi As IClassesToCode = New ClassesToCodeDelphi

    Private m_TablesToSourceSql As ITablesToSource = New TablesToSourceMSSqlServer
    Private m_TablesToSourceMdb As ITablesToSource = New TablesToSourceMSAccess

    Private m_SourceToTablesSql As ISourceToTables = New SourceToTablesMSSqlServer
    Private m_SourceToTablesMdb As ISourceToTables = New SourceToTablesMSAccess
    Private m_SourceToTablesOleDb As ISourceToTables = New SourceToTablesOleDbGeneric
    'Private m_SourceToTablesBdpSql As ISourceToTables = New SourceToTablesBdpSqlServer
    'Private m_SourceToTablesBdpAccess As ISourceToTables = New SourceToTablesBdpAccess
    'Private m_SourceToTablesBdpInterbase As ISourceToTables = New SourceToTablesBdpInterbase

    Private m_CodeToClasses As ICodeToClasses = New CodeToClasses


    'Private m_NPersistToEco As New NPersistToEco
    'Private m_EcoToNPersist As New EcoToNPersist
    'Private m_EcoToXml As New EcoToXml
    'Private m_XmlToEco As New XmlToEco

    'Private m_NPersistToEcoCodeCs As New NPersistToEcoCodeCs
    'Private m_NPersistToEcoCodeDelphi As New NPersistToEcoCodeDelphi

    Private m_NPersistToNHibernate As New NPersistToNHibernate

    Private m_frmImportDtbStatus As New frmImportDtbStatus

    Private m_frmSearchReplace As frmSearchReplace

    Private m_frmExportToNHibernate As New frmExportToNHibernate

    Private m_stackStatus As New ArrayList

    Private m_TablesToSourceWorking As Boolean

    Private m_WizardStatus As WizardResultStatusEnum
    Private m_WizardResultMsgs As New ArrayList

    Private m_TreeDragHilited As MapNode

    Private m_NoRefreshXmlBehind As Boolean
    Private m_NoRefreshUmlDoc As Boolean

    Dim m_MousePointerStack() As Cursor

    Private m_ClassContextMenuWasFromDiagram As Boolean = False

#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
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
    Friend WithEvents panelLeft As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents imageListSmall As System.Windows.Forms.ImageList
    Friend WithEvents panelStatus As System.Windows.Forms.Panel
    Friend WithEvents panelBottom As System.Windows.Forms.Panel
    Friend WithEvents panelRight As System.Windows.Forms.Panel
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents panelMain As System.Windows.Forms.Panel
    Friend WithEvents Splitter4 As System.Windows.Forms.Splitter
    Friend WithEvents Splitter5 As System.Windows.Forms.Splitter
    Friend WithEvents panelSecondTree As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomain As System.Windows.Forms.ContextMenu
    Friend WithEvents menuClassList As System.Windows.Forms.ContextMenu
    Friend WithEvents menuClassListAddClass As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespace As System.Windows.Forms.ContextMenu
    Friend WithEvents menuDomainSave As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainSaveAs As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainRemove As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespaceAddClass As System.Windows.Forms.MenuItem
    Friend WithEvents menuClass As System.Windows.Forms.ContextMenu
    Friend WithEvents menuProperty As System.Windows.Forms.ContextMenu
    Friend WithEvents menuClassAddProperty As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassDelete As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyDelete As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainAdd As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceList As System.Windows.Forms.ContextMenu
    Friend WithEvents menuSourceListAddSource As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainAddClass As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainAddSource As System.Windows.Forms.MenuItem
    Friend WithEvents menuSource As System.Windows.Forms.ContextMenu
    Friend WithEvents menuTable As System.Windows.Forms.ContextMenu
    Friend WithEvents menuColumn As System.Windows.Forms.ContextMenu
    Friend WithEvents menuSourceAddTable As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableAddColumn As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableDelete As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnDelete As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceDelete As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespaceDelete As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyCodeVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyCodeCs As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem15 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeCs As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem17 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem18 As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespaceCodeVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespaceCodeCs As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem20 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeCs As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem21 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem22 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeCs As System.Windows.Forms.MenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MenuItem23 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem24 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceSynchModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceSynchSource As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem25 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceCodeDTD As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem27 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem26 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem28 As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableCodeDTD As System.Windows.Forms.MenuItem
    Friend WithEvents menuNew As System.Windows.Forms.ContextMenu
    Friend WithEvents menuNewItem As System.Windows.Forms.ContextMenu
    Friend WithEvents menuNewDomain As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewProject As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemClass As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemProperty As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemNewItem As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemExistingItem As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemSource As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemTable As System.Windows.Forms.MenuItem
    Friend WithEvents menuNewItemColumn As System.Windows.Forms.MenuItem
    Friend WithEvents tabControlTools As System.Windows.Forms.TabControl
    Friend WithEvents menuSynch As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem29 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchMarkAllChecked As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchMarkAllUnchecked As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchCommit As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchDiscard As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem32 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem30 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem31 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSyncRemoveUnchecked As System.Windows.Forms.MenuItem
    Friend WithEvents menuSyncRemoveChecked As System.Windows.Forms.MenuItem
    Friend WithEvents statusMain As System.Windows.Forms.StatusBarPanel
    Friend WithEvents statusMessage As System.Windows.Forms.StatusBarPanel
    Friend WithEvents mapTreeViewPreview As Puzzle.ObjectMapper.GUI.MapTreeView
    Friend WithEvents menuSynchTools As System.Windows.Forms.ContextMenu
    Friend WithEvents menuSynchToolsSncDM As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncCM As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncTM As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncTMToModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSnc As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncToSource As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem41 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem43 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynch As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchDM As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchCM As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchTM As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncCMToModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncCMToCode As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncDMToCls As System.Windows.Forms.MenuItem
    Friend WithEvents menuSynchToolsSncDMToTbl As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchTMToModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchTMToSource As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchCMToModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchCMToCode As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchDMToTbl As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsSynchDMToCls As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem33 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem35 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListSynchDMToTbl As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListSynchDMToCls As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem37 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsVerifyVerify As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsVerify As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsVerifyAuto As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem45 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsVerifyMappings As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem36 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsOptions As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileSaveAs As System.Windows.Forms.MenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents menuDomainCodeDTD As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeXml As System.Windows.Forms.MenuItem
    Friend WithEvents panelDocuments As System.Windows.Forms.Panel
    Friend WithEvents tabControlDocuments As System.Windows.Forms.TabControl
    Friend WithEvents menuFile As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileSaveAll As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListSynchCMToModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListSynchCMToCode As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileOpenModel As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileOpenFile As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileOpenProject As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem34 As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileCloseProject As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileNewMap As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileNewProject As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileNewFile As System.Windows.Forms.MenuItem
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents menuConfigList As System.Windows.Forms.ContextMenu
    Friend WithEvents menuConfigListAddConfig As System.Windows.Forms.MenuItem
    Friend WithEvents menuConfig As System.Windows.Forms.ContextMenu
    Friend WithEvents menuConfigDelete As System.Windows.Forms.MenuItem
    Friend WithEvents menuConfigSetActive As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem44 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDiagramList As System.Windows.Forms.ContextMenu
    Friend WithEvents menuDiagramListAddDiagram As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceCodeFile As System.Windows.Forms.ContextMenu
    Friend WithEvents menuSourceCodeFileRemove As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceCodeFileDelete As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem38 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem39 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem46 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem47 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem48 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem49 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem50 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem51 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem52 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem53 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem54 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem55 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem56 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSrcListSynchTMToDB As System.Windows.Forms.MenuItem
    Friend WithEvents menuSrcListSynchTMToModel As System.Windows.Forms.MenuItem
    Friend WithEvents panelDocTitle As System.Windows.Forms.Panel
    Friend WithEvents labelUserDocTitle As System.Windows.Forms.Label
    Friend WithEvents buttonCloseUserDoc As System.Windows.Forms.Button
    Friend WithEvents tabControlUserDoc As System.Windows.Forms.TabControl
    Friend WithEvents panelPreviewDocTitle As System.Windows.Forms.Panel
    Friend WithEvents labelPreviewDocTitle As System.Windows.Forms.Label
    Friend WithEvents buttonClosePreviewDoc As System.Windows.Forms.Button
    Friend WithEvents tabControlPreviewDoc As System.Windows.Forms.TabControl
    Friend WithEvents panelPropGridMap As System.Windows.Forms.Panel
    Friend WithEvents mapTreeView As Puzzle.ObjectMapper.GUI.MapTreeView
    Friend WithEvents panelTreeMap As System.Windows.Forms.Panel
    Friend WithEvents mapPropertyGrid As Puzzle.ObjectMapper.GUI.MapPropertyGrid
    Friend WithEvents MenuItem57 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsPluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsPlugins As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileTools As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainPluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainPlugins As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem42 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassPlugins As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassPluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyPlugins As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyPluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourcePlugins As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourcePluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuTablePlugins As System.Windows.Forms.MenuItem
    Friend WithEvents menuTablePluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnPlugins As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnPluginsSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem59 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem60 As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileExit As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileImpExpSep As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileExport As System.Windows.Forms.MenuItem
    Friend WithEvents menuViewProjectExplorer As System.Windows.Forms.MenuItem
    Friend WithEvents menuViewMain As System.Windows.Forms.MenuItem
    Friend WithEvents menuViewTools As System.Windows.Forms.MenuItem
    Friend WithEvents menuViewMsgList As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem67 As System.Windows.Forms.MenuItem
    Friend WithEvents menuViewStatusBar As System.Windows.Forms.MenuItem
    Friend WithEvents menuViewToolBar As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem62 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem65 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceSynchClsToTbl As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceSynchTblToCls As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem68 As System.Windows.Forms.MenuItem
    Friend WithEvents listViewExceptions As System.Windows.Forms.ListView
    Friend WithEvents listViewPreviewMsgs As System.Windows.Forms.ListView
    Friend WithEvents listViewMsgs As System.Windows.Forms.ListView
    Friend WithEvents tabControlMessages As System.Windows.Forms.TabControl
    Friend WithEvents menuToolsWiz As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsWizWrapDbWiz As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsWizGenDomWiz As System.Windows.Forms.MenuItem
    Friend WithEvents menuWizards As System.Windows.Forms.ContextMenu
    Friend WithEvents menuWizardGenDom As System.Windows.Forms.MenuItem
    Friend WithEvents menuWizardWrapDb As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem66 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem70 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSrcListPreviewDDL As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem71 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem73 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem74 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem75 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem76 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem58 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem77 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem78 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem79 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem80 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeDDL As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeXml As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem83 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem84 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem85 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem86 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem87 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem88 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem89 As System.Windows.Forms.MenuItem
    Friend WithEvents menuEditCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuEditPaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuEditDelete As System.Windows.Forms.MenuItem
    Friend WithEvents menuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassPaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourcePaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespaceRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuTablePaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuConfigRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainPaste As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem40 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListPaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespacePaste As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem91 As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceListPaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuProject As System.Windows.Forms.ContextMenu
    Friend WithEvents menuProjectClose As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectRename As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectAdd As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectAddNewDomain As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectAddExistingDomain As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem92 As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectSave As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectSaveAs As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectSaveAll As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem96 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem94 As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectPaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceTestConnection As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem95 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents menuNamespaceCodeDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyCodeDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem97 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassAddShadow As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassRemoveShadow As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem99 As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyAddShadow As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyRemoveShadow As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileClose As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassTransform As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassShadow As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyTransform As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyShadow As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents menuEditSelectAll As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem98 As System.Windows.Forms.MenuItem
    Friend WithEvents menuEditFindAndReplace As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainViewXML As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem100 As System.Windows.Forms.MenuItem
    Friend WithEvents panelXmlBehindTitle As System.Windows.Forms.Panel
    Friend WithEvents labelXmlBehindTitle As System.Windows.Forms.Label
    Friend WithEvents buttonCloseXmlBehind As System.Windows.Forms.Button
    Friend WithEvents tabControlXmlBehind As System.Windows.Forms.TabControl
    Friend WithEvents buttonDiscardXmlBehind As System.Windows.Forms.Button
    Friend WithEvents buttonApplyXmlBehind As System.Windows.Forms.Button
    Friend WithEvents menuXmlBehind As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem93 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsXml As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsXmlOpen As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsXmlClose As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem105 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsXmlApply As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsXmlDiscard As System.Windows.Forms.MenuItem
    Friend WithEvents menuXmlOpen As System.Windows.Forms.MenuItem
    Friend WithEvents menuXmlClose As System.Windows.Forms.MenuItem
    Friend WithEvents menuXmlApply As System.Windows.Forms.MenuItem
    Friend WithEvents menuXmlDiscard As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainXml As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainXmlClose As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem101 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainXmlApply As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainXmlDiscard As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem102 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem104 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem106 As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyProperties As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem108 As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem109 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem107 As System.Windows.Forms.MenuItem
    Friend WithEvents menuColumnProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem110 As System.Windows.Forms.MenuItem
    Friend WithEvents menuConfigProperties As System.Windows.Forms.MenuItem
    Friend WithEvents menuSourceCodeFileProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem111 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem112 As System.Windows.Forms.MenuItem
    Friend WithEvents menuProjectProperties As System.Windows.Forms.MenuItem
    Friend WithEvents treePreviewClassesToCode As Puzzle.ObjectMapper.GUI.MapTreeView
    Friend WithEvents panelMainTitle As System.Windows.Forms.Panel
    Friend WithEvents buttonCloseMain As System.Windows.Forms.Button
    Friend WithEvents panelToolsTitle As System.Windows.Forms.Panel
    Friend WithEvents buttonCloseTools As System.Windows.Forms.Button
    Friend WithEvents panelListTitle As System.Windows.Forms.Panel
    Friend WithEvents labelListTitle As System.Windows.Forms.Label
    Friend WithEvents buttonCloseList As System.Windows.Forms.Button
    Friend WithEvents imageListSmallButtons As System.Windows.Forms.ImageList
    Friend WithEvents mapListView As Puzzle.ObjectMapper.GUI.MapListView
    Friend WithEvents buttonNextPreviewDoc As System.Windows.Forms.Button
    Friend WithEvents buttonPrevPreviewDoc As System.Windows.Forms.Button
    Friend WithEvents buttonPrevUserDoc As System.Windows.Forms.Button
    Friend WithEvents buttonNextUserDoc As System.Windows.Forms.Button
    Friend WithEvents buttonPrevXmlBehind As System.Windows.Forms.Button
    Friend WithEvents buttonNextXmlBehind As System.Windows.Forms.Button
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents toolBarButtonBack As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonForward As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonUp As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonNew As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonNewItem As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonOpen As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonSave As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonSaveAll As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonFind As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonCut As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonCopy As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonPaste As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonSynch As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonRun As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonWizards As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonExplorer As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonProperties As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonXmlBehind As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonList As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonTools As System.Windows.Forms.ToolBarButton
    Friend WithEvents menuFileRecentProjects As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem114 As System.Windows.Forms.MenuItem
    Friend WithEvents buttonSwapPosMain As System.Windows.Forms.Button
    Friend WithEvents buttonSwapPosTools As System.Windows.Forms.Button
    Friend WithEvents buttonSwapPosList As System.Windows.Forms.Button
    Friend WithEvents menuUserDoc As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem115 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem119 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocUndo As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocCut As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocCopy As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocPaste As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocSelectAll As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocDelete As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlDiagram As System.Windows.Forms.ContextMenu
    Friend WithEvents menuUmlView As System.Windows.Forms.MenuItem
    Friend WithEvents tabToolsPreview As System.Windows.Forms.TabPage
    Friend WithEvents menuClassUml As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem116 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassUmlAddToCurr As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassUmlAddToNew As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButton5 As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonUml As System.Windows.Forms.ToolBarButton
    Friend WithEvents menuPropertyUml As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem118 As System.Windows.Forms.MenuItem
    Friend WithEvents menuPropertyUmlShowPropLine As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassUmlShowAssocLines As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassUmlRemove As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassUmlAddSeparator As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEnd As System.Windows.Forms.ContextMenu
    Friend WithEvents menuUmlLinePoint As System.Windows.Forms.ContextMenu
    Friend WithEvents menuPropertyUmlRemovePropLine As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassUmlRemoveAllLines As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem113 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddLines As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlRemoveLines As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlDeleteDiagram As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem120 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem121 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem117 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddClass As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddClassNew As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddClassExisting As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddClassExistingAll As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem126 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlFit As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEndRemoveLine As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem124 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEndAddPoint As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem127 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEndProperties As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem122 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEndLock As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEndUnLock As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmSelectAll As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem128 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem129 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem130 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLinePointRemove As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLinePointAddPoint As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLinePointProperties As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlLineEndSelectLine As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem131 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem123 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom400 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom200 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom150 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom100 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom75 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoom50 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem138 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoomIn As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlZoomOut As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem140 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem125 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeNH As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeNHCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeNHVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeNHDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem135 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeNHXml As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddClassExistingSelect As System.Windows.Forms.MenuItem
    Friend WithEvents menuFileExportNHib As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem132 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeNhCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeNhVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeNhDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem134 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainExportNh As System.Windows.Forms.MenuItem
    Friend WithEvents toolBarButtonClassesToCode As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonCodeToClasses As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonTablesToClasses As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonClassesToTables As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonTablesToSource As System.Windows.Forms.ToolBarButton
    Friend WithEvents toolBarButtonSourceToTables As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem136 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeNhXml As System.Windows.Forms.MenuItem
    Friend WithEvents tabPageMainCustom As System.Windows.Forms.TabPage
    Friend WithEvents tabPageMainPreview As System.Windows.Forms.TabPage
    Friend WithEvents tabPageMainXmlBehind As System.Windows.Forms.TabPage
    Friend WithEvents panelProjectTitle As System.Windows.Forms.Panel
    Friend WithEvents labelExplorer As System.Windows.Forms.Label
    Friend WithEvents buttonSwapPosExplorer As System.Windows.Forms.Button
    Friend WithEvents buttonCloseExplorer As System.Windows.Forms.Button
    Friend WithEvents tabPageMainUml As System.Windows.Forms.TabPage
    Friend WithEvents panelProperties As System.Windows.Forms.Panel
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents panelUmlTitle As System.Windows.Forms.Panel
    Friend WithEvents labelUmlTitle As System.Windows.Forms.Label
    Friend WithEvents buttonPrevUmlDoc As System.Windows.Forms.Button
    Friend WithEvents buttonNextUmlDoc As System.Windows.Forms.Button
    Friend WithEvents buttonCloseUmlDoc As System.Windows.Forms.Button
    Friend WithEvents tabControlUmlDoc As System.Windows.Forms.TabControl
    Friend WithEvents panelPropertiesTitle As System.Windows.Forms.Panel
    Friend WithEvents buttonSwapPosProperties As System.Windows.Forms.Button
    Friend WithEvents buttonCloseProperties As System.Windows.Forms.Button
    Friend WithEvents menuViewProperties As System.Windows.Forms.MenuItem
    Friend WithEvents menuFilePrint As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem139 As System.Windows.Forms.MenuItem
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents toolBarButtonPrint As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton7 As System.Windows.Forms.ToolBarButton
    Friend WithEvents SourceCodePrintDocument1 As Puzzle.SourceCode.SourceCodePrintDocument
    Friend WithEvents PrintPreviewDialog1 As System.Windows.Forms.PrintPreviewDialog
    Friend WithEvents PageSetupDialog1 As System.Windows.Forms.PageSetupDialog
    Friend WithEvents menuFilePrintPreview As System.Windows.Forms.MenuItem
    Friend WithEvents menuFilePageSetup As System.Windows.Forms.MenuItem
    Friend WithEvents toolBarButtonPrintPreview As System.Windows.Forms.ToolBarButton
    Friend WithEvents menuUmlSaveAs As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem141 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem69 As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlAddClassExistingAllWithLines As System.Windows.Forms.MenuItem
    Friend WithEvents menuUmlSaveViewAs As System.Windows.Forms.MenuItem
    Friend WithEvents menuUserDocRedo As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem72 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem103 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem137 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeNhCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeNhVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeNhDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListCodeNhXml As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem146 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents MenuItem142 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem143 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeNhCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeNhVb As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem148 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsCodeNhXml As System.Windows.Forms.MenuItem
    Friend WithEvents toolBarButtonDiscard As System.Windows.Forms.ToolBarButton
    Friend WithEvents tabPageErrors As System.Windows.Forms.TabPage
    Friend WithEvents tabPagePreviewList As System.Windows.Forms.TabPage
    Friend WithEvents tabPageMessageList As System.Windows.Forms.TabPage
    Friend WithEvents tabPageListView As System.Windows.Forms.TabPage
    Friend WithEvents menuToolsNPersist As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem61 As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsNPersistDomainExplorer As System.Windows.Forms.MenuItem
    Friend WithEvents menuToolsNPersistQueryAnalyzer As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem81 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainTestInDomainExplorer As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButton6 As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem64 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem63 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeMapCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeMapVb As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem133 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassCodeMapDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents tabPageMainCodeMap As System.Windows.Forms.TabPage
    Friend WithEvents tabControlCodeMapDoc As System.Windows.Forms.TabControl
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents labelCodeMapDocTitle As System.Windows.Forms.Label
    Friend WithEvents buttonPrevCodeMapDoc As System.Windows.Forms.Button
    Friend WithEvents buttonNextCodeMapDoc As System.Windows.Forms.Button
    Friend WithEvents buttonCloseCodeMapDoc As System.Windows.Forms.Button
    Friend WithEvents MenuItem82 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem90 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCleanupAllOrMappings As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCleanupAllTables As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCleanupAllCode As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCleanupAllClasses As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem144 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem145 As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeMapCs As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeMapVb As System.Windows.Forms.MenuItem
    Friend WithEvents menuDomainCodeMapDelphi As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassAddEnumValue As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem147 As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListAddInterface As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListAddStruct As System.Windows.Forms.MenuItem
    Friend WithEvents menuClassListAddEnum As System.Windows.Forms.MenuItem
    Friend WithEvents enumValueMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents deleteEnumValueMenuItem As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDomainMapBrowser))
        Me.panelStatus = New System.Windows.Forms.Panel
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.statusMain = New System.Windows.Forms.StatusBarPanel
        Me.statusMessage = New System.Windows.Forms.StatusBarPanel
        Me.menuNew = New System.Windows.Forms.ContextMenu
        Me.menuNewDomain = New System.Windows.Forms.MenuItem
        Me.menuNewProject = New System.Windows.Forms.MenuItem
        Me.menuNewItem = New System.Windows.Forms.ContextMenu
        Me.menuNewItemNewItem = New System.Windows.Forms.MenuItem
        Me.menuNewItemExistingItem = New System.Windows.Forms.MenuItem
        Me.MenuItem59 = New System.Windows.Forms.MenuItem
        Me.menuNewItemClass = New System.Windows.Forms.MenuItem
        Me.menuNewItemProperty = New System.Windows.Forms.MenuItem
        Me.MenuItem60 = New System.Windows.Forms.MenuItem
        Me.menuNewItemSource = New System.Windows.Forms.MenuItem
        Me.menuNewItemTable = New System.Windows.Forms.MenuItem
        Me.menuNewItemColumn = New System.Windows.Forms.MenuItem
        Me.menuSynchTools = New System.Windows.Forms.ContextMenu
        Me.menuSynchToolsSnc = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncDM = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncDMToCls = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncDMToTbl = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncCM = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncCMToCode = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncCMToModel = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncTM = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncToSource = New System.Windows.Forms.MenuItem
        Me.menuSynchToolsSncTMToModel = New System.Windows.Forms.MenuItem
        Me.menuSynch = New System.Windows.Forms.ContextMenu
        Me.menuSynchCommit = New System.Windows.Forms.MenuItem
        Me.menuSynchDiscard = New System.Windows.Forms.MenuItem
        Me.MenuItem32 = New System.Windows.Forms.MenuItem
        Me.MenuItem29 = New System.Windows.Forms.MenuItem
        Me.menuSynchMarkAllChecked = New System.Windows.Forms.MenuItem
        Me.menuSynchMarkAllUnchecked = New System.Windows.Forms.MenuItem
        Me.MenuItem30 = New System.Windows.Forms.MenuItem
        Me.MenuItem31 = New System.Windows.Forms.MenuItem
        Me.menuSyncRemoveUnchecked = New System.Windows.Forms.MenuItem
        Me.menuSyncRemoveChecked = New System.Windows.Forms.MenuItem
        Me.menuWizards = New System.Windows.Forms.ContextMenu
        Me.menuWizardGenDom = New System.Windows.Forms.MenuItem
        Me.menuWizardWrapDb = New System.Windows.Forms.MenuItem
        Me.menuXmlBehind = New System.Windows.Forms.ContextMenu
        Me.menuXmlOpen = New System.Windows.Forms.MenuItem
        Me.menuXmlClose = New System.Windows.Forms.MenuItem
        Me.MenuItem93 = New System.Windows.Forms.MenuItem
        Me.menuXmlApply = New System.Windows.Forms.MenuItem
        Me.menuXmlDiscard = New System.Windows.Forms.MenuItem
        Me.imageListSmall = New System.Windows.Forms.ImageList(Me.components)
        Me.panelLeft = New System.Windows.Forms.Panel
        Me.panelPropGridMap = New System.Windows.Forms.Panel
        Me.panelProjectTitle = New System.Windows.Forms.Panel
        Me.labelExplorer = New System.Windows.Forms.Label
        Me.buttonSwapPosExplorer = New System.Windows.Forms.Button
        Me.imageListSmallButtons = New System.Windows.Forms.ImageList(Me.components)
        Me.buttonCloseExplorer = New System.Windows.Forms.Button
        Me.panelBottom = New System.Windows.Forms.Panel
        Me.tabControlMessages = New System.Windows.Forms.TabControl
        Me.tabPageListView = New System.Windows.Forms.TabPage
        Me.tabPageErrors = New System.Windows.Forms.TabPage
        Me.listViewExceptions = New System.Windows.Forms.ListView
        Me.tabPagePreviewList = New System.Windows.Forms.TabPage
        Me.listViewPreviewMsgs = New System.Windows.Forms.ListView
        Me.tabPageMessageList = New System.Windows.Forms.TabPage
        Me.listViewMsgs = New System.Windows.Forms.ListView
        Me.panelListTitle = New System.Windows.Forms.Panel
        Me.labelListTitle = New System.Windows.Forms.Label
        Me.buttonSwapPosList = New System.Windows.Forms.Button
        Me.buttonCloseList = New System.Windows.Forms.Button
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.panelRight = New System.Windows.Forms.Panel
        Me.panelSecondTree = New System.Windows.Forms.Panel
        Me.tabControlTools = New System.Windows.Forms.TabControl
        Me.tabToolsPreview = New System.Windows.Forms.TabPage
        Me.panelToolsTitle = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.buttonSwapPosTools = New System.Windows.Forms.Button
        Me.buttonCloseTools = New System.Windows.Forms.Button
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.menuFile = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.menuFileNewMap = New System.Windows.Forms.MenuItem
        Me.menuFileNewProject = New System.Windows.Forms.MenuItem
        Me.menuFileNewFile = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.menuFileOpenModel = New System.Windows.Forms.MenuItem
        Me.menuFileOpenFile = New System.Windows.Forms.MenuItem
        Me.menuFileClose = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.menuFileOpenProject = New System.Windows.Forms.MenuItem
        Me.menuFileCloseProject = New System.Windows.Forms.MenuItem
        Me.MenuItem34 = New System.Windows.Forms.MenuItem
        Me.menuFileSave = New System.Windows.Forms.MenuItem
        Me.menuFileSaveAs = New System.Windows.Forms.MenuItem
        Me.menuFileSaveAll = New System.Windows.Forms.MenuItem
        Me.menuFileImpExpSep = New System.Windows.Forms.MenuItem
        Me.menuFileExport = New System.Windows.Forms.MenuItem
        Me.menuFileExportNHib = New System.Windows.Forms.MenuItem
        Me.MenuItem139 = New System.Windows.Forms.MenuItem
        Me.menuFilePageSetup = New System.Windows.Forms.MenuItem
        Me.menuFilePrintPreview = New System.Windows.Forms.MenuItem
        Me.menuFilePrint = New System.Windows.Forms.MenuItem
        Me.MenuItem68 = New System.Windows.Forms.MenuItem
        Me.menuFileRecentProjects = New System.Windows.Forms.MenuItem
        Me.MenuItem114 = New System.Windows.Forms.MenuItem
        Me.menuFileExit = New System.Windows.Forms.MenuItem
        Me.menuEdit = New System.Windows.Forms.MenuItem
        Me.menuEditCut = New System.Windows.Forms.MenuItem
        Me.menuEditCopy = New System.Windows.Forms.MenuItem
        Me.menuEditPaste = New System.Windows.Forms.MenuItem
        Me.menuEditDelete = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.menuEditSelectAll = New System.Windows.Forms.MenuItem
        Me.MenuItem98 = New System.Windows.Forms.MenuItem
        Me.menuEditFindAndReplace = New System.Windows.Forms.MenuItem
        Me.MenuItem41 = New System.Windows.Forms.MenuItem
        Me.menuViewProjectExplorer = New System.Windows.Forms.MenuItem
        Me.menuViewProperties = New System.Windows.Forms.MenuItem
        Me.menuViewMain = New System.Windows.Forms.MenuItem
        Me.menuViewTools = New System.Windows.Forms.MenuItem
        Me.menuViewMsgList = New System.Windows.Forms.MenuItem
        Me.MenuItem67 = New System.Windows.Forms.MenuItem
        Me.menuViewToolBar = New System.Windows.Forms.MenuItem
        Me.menuViewStatusBar = New System.Windows.Forms.MenuItem
        Me.menuFileTools = New System.Windows.Forms.MenuItem
        Me.menuToolsVerify = New System.Windows.Forms.MenuItem
        Me.menuToolsVerifyVerify = New System.Windows.Forms.MenuItem
        Me.MenuItem45 = New System.Windows.Forms.MenuItem
        Me.menuToolsVerifyAuto = New System.Windows.Forms.MenuItem
        Me.menuToolsVerifyMappings = New System.Windows.Forms.MenuItem
        Me.MenuItem37 = New System.Windows.Forms.MenuItem
        Me.menuToolsXml = New System.Windows.Forms.MenuItem
        Me.menuToolsXmlOpen = New System.Windows.Forms.MenuItem
        Me.menuToolsXmlClose = New System.Windows.Forms.MenuItem
        Me.MenuItem105 = New System.Windows.Forms.MenuItem
        Me.menuToolsXmlApply = New System.Windows.Forms.MenuItem
        Me.menuToolsXmlDiscard = New System.Windows.Forms.MenuItem
        Me.MenuItem69 = New System.Windows.Forms.MenuItem
        Me.menuToolsWiz = New System.Windows.Forms.MenuItem
        Me.menuToolsWizGenDomWiz = New System.Windows.Forms.MenuItem
        Me.menuToolsWizWrapDbWiz = New System.Windows.Forms.MenuItem
        Me.MenuItem61 = New System.Windows.Forms.MenuItem
        Me.menuToolsNPersist = New System.Windows.Forms.MenuItem
        Me.menuToolsNPersistDomainExplorer = New System.Windows.Forms.MenuItem
        Me.menuToolsNPersistQueryAnalyzer = New System.Windows.Forms.MenuItem
        Me.MenuItem36 = New System.Windows.Forms.MenuItem
        Me.menuToolsPlugins = New System.Windows.Forms.MenuItem
        Me.menuToolsPluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuToolsOptions = New System.Windows.Forms.MenuItem
        Me.menuToolsSynch = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchDM = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchDMToTbl = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchDMToCls = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchCM = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchCMToCode = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchCMToModel = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchTM = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchTMToSource = New System.Windows.Forms.MenuItem
        Me.menuToolsSynchTMToModel = New System.Windows.Forms.MenuItem
        Me.MenuItem71 = New System.Windows.Forms.MenuItem
        Me.MenuItem58 = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeCs = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeVb = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem77 = New System.Windows.Forms.MenuItem
        Me.MenuItem78 = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeDDL = New System.Windows.Forms.MenuItem
        Me.MenuItem79 = New System.Windows.Forms.MenuItem
        Me.MenuItem80 = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeXml = New System.Windows.Forms.MenuItem
        Me.MenuItem142 = New System.Windows.Forms.MenuItem
        Me.MenuItem143 = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeNhCs = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeNhVb = New System.Windows.Forms.MenuItem
        Me.MenuItem148 = New System.Windows.Forms.MenuItem
        Me.menuToolsCodeNhXml = New System.Windows.Forms.MenuItem
        Me.MenuItem43 = New System.Windows.Forms.MenuItem
        Me.MenuItem42 = New System.Windows.Forms.MenuItem
        Me.panelMain = New System.Windows.Forms.Panel
        Me.panelDocuments = New System.Windows.Forms.Panel
        Me.tabControlDocuments = New System.Windows.Forms.TabControl
        Me.tabPageMainUml = New System.Windows.Forms.TabPage
        Me.panelTreeMap = New System.Windows.Forms.Panel
        Me.tabControlUmlDoc = New System.Windows.Forms.TabControl
        Me.panelUmlTitle = New System.Windows.Forms.Panel
        Me.labelUmlTitle = New System.Windows.Forms.Label
        Me.buttonPrevUmlDoc = New System.Windows.Forms.Button
        Me.buttonNextUmlDoc = New System.Windows.Forms.Button
        Me.buttonCloseUmlDoc = New System.Windows.Forms.Button
        Me.tabPageMainCustom = New System.Windows.Forms.TabPage
        Me.tabControlUserDoc = New System.Windows.Forms.TabControl
        Me.panelDocTitle = New System.Windows.Forms.Panel
        Me.labelUserDocTitle = New System.Windows.Forms.Label
        Me.buttonPrevUserDoc = New System.Windows.Forms.Button
        Me.buttonNextUserDoc = New System.Windows.Forms.Button
        Me.buttonCloseUserDoc = New System.Windows.Forms.Button
        Me.tabPageMainPreview = New System.Windows.Forms.TabPage
        Me.tabControlPreviewDoc = New System.Windows.Forms.TabControl
        Me.panelPreviewDocTitle = New System.Windows.Forms.Panel
        Me.labelPreviewDocTitle = New System.Windows.Forms.Label
        Me.buttonPrevPreviewDoc = New System.Windows.Forms.Button
        Me.buttonNextPreviewDoc = New System.Windows.Forms.Button
        Me.buttonClosePreviewDoc = New System.Windows.Forms.Button
        Me.tabPageMainXmlBehind = New System.Windows.Forms.TabPage
        Me.tabControlXmlBehind = New System.Windows.Forms.TabControl
        Me.panelXmlBehindTitle = New System.Windows.Forms.Panel
        Me.labelXmlBehindTitle = New System.Windows.Forms.Label
        Me.buttonApplyXmlBehind = New System.Windows.Forms.Button
        Me.buttonDiscardXmlBehind = New System.Windows.Forms.Button
        Me.buttonPrevXmlBehind = New System.Windows.Forms.Button
        Me.buttonNextXmlBehind = New System.Windows.Forms.Button
        Me.buttonCloseXmlBehind = New System.Windows.Forms.Button
        Me.tabPageMainCodeMap = New System.Windows.Forms.TabPage
        Me.tabControlCodeMapDoc = New System.Windows.Forms.TabControl
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.labelCodeMapDocTitle = New System.Windows.Forms.Label
        Me.buttonPrevCodeMapDoc = New System.Windows.Forms.Button
        Me.buttonNextCodeMapDoc = New System.Windows.Forms.Button
        Me.buttonCloseCodeMapDoc = New System.Windows.Forms.Button
        Me.panelMainTitle = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.buttonSwapPosMain = New System.Windows.Forms.Button
        Me.buttonCloseMain = New System.Windows.Forms.Button
        Me.Splitter4 = New System.Windows.Forms.Splitter
        Me.Splitter5 = New System.Windows.Forms.Splitter
        Me.menuDomain = New System.Windows.Forms.ContextMenu
        Me.menuDomainAdd = New System.Windows.Forms.MenuItem
        Me.menuDomainAddClass = New System.Windows.Forms.MenuItem
        Me.menuDomainAddSource = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.menuDomainCopy = New System.Windows.Forms.MenuItem
        Me.menuDomainPaste = New System.Windows.Forms.MenuItem
        Me.menuDomainRemove = New System.Windows.Forms.MenuItem
        Me.menuDomainRename = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.menuDomainSave = New System.Windows.Forms.MenuItem
        Me.menuDomainSaveAs = New System.Windows.Forms.MenuItem
        Me.MenuItem19 = New System.Windows.Forms.MenuItem
        Me.menuDomainTestInDomainExplorer = New System.Windows.Forms.MenuItem
        Me.MenuItem145 = New System.Windows.Forms.MenuItem
        Me.MenuItem144 = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeMapCs = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeMapVb = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeMapDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem64 = New System.Windows.Forms.MenuItem
        Me.MenuItem39 = New System.Windows.Forms.MenuItem
        Me.MenuItem46 = New System.Windows.Forms.MenuItem
        Me.MenuItem49 = New System.Windows.Forms.MenuItem
        Me.MenuItem50 = New System.Windows.Forms.MenuItem
        Me.MenuItem47 = New System.Windows.Forms.MenuItem
        Me.MenuItem51 = New System.Windows.Forms.MenuItem
        Me.MenuItem52 = New System.Windows.Forms.MenuItem
        Me.MenuItem48 = New System.Windows.Forms.MenuItem
        Me.MenuItem53 = New System.Windows.Forms.MenuItem
        Me.MenuItem54 = New System.Windows.Forms.MenuItem
        Me.MenuItem100 = New System.Windows.Forms.MenuItem
        Me.MenuItem20 = New System.Windows.Forms.MenuItem
        Me.MenuItem73 = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeCs = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeVb = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem57 = New System.Windows.Forms.MenuItem
        Me.MenuItem74 = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeDTD = New System.Windows.Forms.MenuItem
        Me.MenuItem75 = New System.Windows.Forms.MenuItem
        Me.MenuItem76 = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeXml = New System.Windows.Forms.MenuItem
        Me.MenuItem125 = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeNH = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeNHCs = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeNHVb = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeNHDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem135 = New System.Windows.Forms.MenuItem
        Me.menuDomainCodeNHXml = New System.Windows.Forms.MenuItem
        Me.MenuItem38 = New System.Windows.Forms.MenuItem
        Me.menuDomainXml = New System.Windows.Forms.MenuItem
        Me.menuDomainViewXML = New System.Windows.Forms.MenuItem
        Me.menuDomainXmlClose = New System.Windows.Forms.MenuItem
        Me.MenuItem101 = New System.Windows.Forms.MenuItem
        Me.menuDomainXmlApply = New System.Windows.Forms.MenuItem
        Me.menuDomainXmlDiscard = New System.Windows.Forms.MenuItem
        Me.MenuItem90 = New System.Windows.Forms.MenuItem
        Me.MenuItem82 = New System.Windows.Forms.MenuItem
        Me.menuDomainCleanupAllCode = New System.Windows.Forms.MenuItem
        Me.menuDomainCleanupAllOrMappings = New System.Windows.Forms.MenuItem
        Me.menuDomainCleanupAllClasses = New System.Windows.Forms.MenuItem
        Me.menuDomainCleanupAllTables = New System.Windows.Forms.MenuItem
        Me.MenuItem81 = New System.Windows.Forms.MenuItem
        Me.MenuItem134 = New System.Windows.Forms.MenuItem
        Me.menuDomainExportNh = New System.Windows.Forms.MenuItem
        Me.menuDomainPluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuDomainPlugins = New System.Windows.Forms.MenuItem
        Me.MenuItem102 = New System.Windows.Forms.MenuItem
        Me.menuDomainProperties = New System.Windows.Forms.MenuItem
        Me.menuClassList = New System.Windows.Forms.ContextMenu
        Me.MenuItem147 = New System.Windows.Forms.MenuItem
        Me.menuClassListAddClass = New System.Windows.Forms.MenuItem
        Me.menuClassListAddInterface = New System.Windows.Forms.MenuItem
        Me.menuClassListAddStruct = New System.Windows.Forms.MenuItem
        Me.menuClassListAddEnum = New System.Windows.Forms.MenuItem
        Me.MenuItem40 = New System.Windows.Forms.MenuItem
        Me.menuClassListPaste = New System.Windows.Forms.MenuItem
        Me.MenuItem21 = New System.Windows.Forms.MenuItem
        Me.MenuItem22 = New System.Windows.Forms.MenuItem
        Me.MenuItem83 = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeCs = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeVb = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem103 = New System.Windows.Forms.MenuItem
        Me.MenuItem137 = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeNhCs = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeNhVb = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeNhDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem146 = New System.Windows.Forms.MenuItem
        Me.menuClassListCodeNhXml = New System.Windows.Forms.MenuItem
        Me.MenuItem33 = New System.Windows.Forms.MenuItem
        Me.MenuItem35 = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.menuClassListSynchDMToTbl = New System.Windows.Forms.MenuItem
        Me.menuClassListSynchDMToCls = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.menuClassListSynchCMToCode = New System.Windows.Forms.MenuItem
        Me.menuClassListSynchCMToModel = New System.Windows.Forms.MenuItem
        Me.menuNamespace = New System.Windows.Forms.ContextMenu
        Me.menuNamespaceAddClass = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.menuNamespacePaste = New System.Windows.Forms.MenuItem
        Me.menuNamespaceDelete = New System.Windows.Forms.MenuItem
        Me.menuNamespaceRename = New System.Windows.Forms.MenuItem
        Me.MenuItem17 = New System.Windows.Forms.MenuItem
        Me.MenuItem18 = New System.Windows.Forms.MenuItem
        Me.MenuItem84 = New System.Windows.Forms.MenuItem
        Me.menuNamespaceCodeCs = New System.Windows.Forms.MenuItem
        Me.menuNamespaceCodeVb = New System.Windows.Forms.MenuItem
        Me.menuNamespaceCodeDelphi = New System.Windows.Forms.MenuItem
        Me.menuClass = New System.Windows.Forms.ContextMenu
        Me.menuClassAddProperty = New System.Windows.Forms.MenuItem
        Me.menuClassAddEnumValue = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.menuClassCut = New System.Windows.Forms.MenuItem
        Me.menuClassCopy = New System.Windows.Forms.MenuItem
        Me.menuClassPaste = New System.Windows.Forms.MenuItem
        Me.menuClassDelete = New System.Windows.Forms.MenuItem
        Me.menuClassRename = New System.Windows.Forms.MenuItem
        Me.MenuItem133 = New System.Windows.Forms.MenuItem
        Me.MenuItem63 = New System.Windows.Forms.MenuItem
        Me.menuClassCodeMapCs = New System.Windows.Forms.MenuItem
        Me.menuClassCodeMapVb = New System.Windows.Forms.MenuItem
        Me.menuClassCodeMapDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem97 = New System.Windows.Forms.MenuItem
        Me.menuClassTransform = New System.Windows.Forms.MenuItem
        Me.menuClassShadow = New System.Windows.Forms.MenuItem
        Me.menuClassAddShadow = New System.Windows.Forms.MenuItem
        Me.menuClassRemoveShadow = New System.Windows.Forms.MenuItem
        Me.MenuItem15 = New System.Windows.Forms.MenuItem
        Me.menuClassUml = New System.Windows.Forms.MenuItem
        Me.menuClassUmlAddToCurr = New System.Windows.Forms.MenuItem
        Me.menuClassUmlAddToNew = New System.Windows.Forms.MenuItem
        Me.menuClassUmlRemove = New System.Windows.Forms.MenuItem
        Me.menuClassUmlAddSeparator = New System.Windows.Forms.MenuItem
        Me.menuClassUmlShowAssocLines = New System.Windows.Forms.MenuItem
        Me.menuClassUmlRemoveAllLines = New System.Windows.Forms.MenuItem
        Me.MenuItem116 = New System.Windows.Forms.MenuItem
        Me.MenuItem16 = New System.Windows.Forms.MenuItem
        Me.MenuItem85 = New System.Windows.Forms.MenuItem
        Me.menuClassCodeCs = New System.Windows.Forms.MenuItem
        Me.menuClassCodeVb = New System.Windows.Forms.MenuItem
        Me.menuClassCodeDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem72 = New System.Windows.Forms.MenuItem
        Me.MenuItem132 = New System.Windows.Forms.MenuItem
        Me.menuClassCodeNhCs = New System.Windows.Forms.MenuItem
        Me.menuClassCodeNhVb = New System.Windows.Forms.MenuItem
        Me.menuClassCodeNhDelphi = New System.Windows.Forms.MenuItem
        Me.MenuItem136 = New System.Windows.Forms.MenuItem
        Me.menuClassCodeNhXml = New System.Windows.Forms.MenuItem
        Me.menuClassPluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuClassPlugins = New System.Windows.Forms.MenuItem
        Me.MenuItem104 = New System.Windows.Forms.MenuItem
        Me.menuClassProperties = New System.Windows.Forms.MenuItem
        Me.menuProperty = New System.Windows.Forms.ContextMenu
        Me.menuPropertyCut = New System.Windows.Forms.MenuItem
        Me.menuPropertyCopy = New System.Windows.Forms.MenuItem
        Me.menuPropertyDelete = New System.Windows.Forms.MenuItem
        Me.menuPropertyRename = New System.Windows.Forms.MenuItem
        Me.MenuItem99 = New System.Windows.Forms.MenuItem
        Me.menuPropertyTransform = New System.Windows.Forms.MenuItem
        Me.menuPropertyShadow = New System.Windows.Forms.MenuItem
        Me.menuPropertyAddShadow = New System.Windows.Forms.MenuItem
        Me.menuPropertyRemoveShadow = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.menuPropertyUml = New System.Windows.Forms.MenuItem
        Me.menuPropertyUmlShowPropLine = New System.Windows.Forms.MenuItem
        Me.menuPropertyUmlRemovePropLine = New System.Windows.Forms.MenuItem
        Me.MenuItem118 = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.MenuItem86 = New System.Windows.Forms.MenuItem
        Me.menuPropertyCodeCs = New System.Windows.Forms.MenuItem
        Me.menuPropertyCodeVb = New System.Windows.Forms.MenuItem
        Me.menuPropertyCodeDelphi = New System.Windows.Forms.MenuItem
        Me.menuPropertyPluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuPropertyPlugins = New System.Windows.Forms.MenuItem
        Me.MenuItem106 = New System.Windows.Forms.MenuItem
        Me.menuPropertyProperties = New System.Windows.Forms.MenuItem
        Me.menuSourceList = New System.Windows.Forms.ContextMenu
        Me.menuSourceListAddSource = New System.Windows.Forms.MenuItem
        Me.MenuItem91 = New System.Windows.Forms.MenuItem
        Me.menuSourceListPaste = New System.Windows.Forms.MenuItem
        Me.MenuItem55 = New System.Windows.Forms.MenuItem
        Me.MenuItem66 = New System.Windows.Forms.MenuItem
        Me.MenuItem87 = New System.Windows.Forms.MenuItem
        Me.menuSrcListPreviewDDL = New System.Windows.Forms.MenuItem
        Me.MenuItem70 = New System.Windows.Forms.MenuItem
        Me.MenuItem56 = New System.Windows.Forms.MenuItem
        Me.menuSrcListSynchTMToDB = New System.Windows.Forms.MenuItem
        Me.menuSrcListSynchTMToModel = New System.Windows.Forms.MenuItem
        Me.menuSource = New System.Windows.Forms.ContextMenu
        Me.menuSourceAddTable = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.menuSourceCut = New System.Windows.Forms.MenuItem
        Me.menuSourceCopy = New System.Windows.Forms.MenuItem
        Me.menuSourcePaste = New System.Windows.Forms.MenuItem
        Me.menuSourceDelete = New System.Windows.Forms.MenuItem
        Me.menuSourceRename = New System.Windows.Forms.MenuItem
        Me.MenuItem23 = New System.Windows.Forms.MenuItem
        Me.menuSourceTestConnection = New System.Windows.Forms.MenuItem
        Me.MenuItem95 = New System.Windows.Forms.MenuItem
        Me.MenuItem25 = New System.Windows.Forms.MenuItem
        Me.MenuItem88 = New System.Windows.Forms.MenuItem
        Me.menuSourceCodeDTD = New System.Windows.Forms.MenuItem
        Me.MenuItem27 = New System.Windows.Forms.MenuItem
        Me.MenuItem24 = New System.Windows.Forms.MenuItem
        Me.MenuItem65 = New System.Windows.Forms.MenuItem
        Me.menuSourceSynchClsToTbl = New System.Windows.Forms.MenuItem
        Me.menuSourceSynchTblToCls = New System.Windows.Forms.MenuItem
        Me.MenuItem62 = New System.Windows.Forms.MenuItem
        Me.menuSourceSynchSource = New System.Windows.Forms.MenuItem
        Me.menuSourceSynchModel = New System.Windows.Forms.MenuItem
        Me.menuSourcePluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuSourcePlugins = New System.Windows.Forms.MenuItem
        Me.MenuItem108 = New System.Windows.Forms.MenuItem
        Me.menuSourceProperties = New System.Windows.Forms.MenuItem
        Me.menuTable = New System.Windows.Forms.ContextMenu
        Me.menuTableAddColumn = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.menuTableCut = New System.Windows.Forms.MenuItem
        Me.menuTableCopy = New System.Windows.Forms.MenuItem
        Me.menuTablePaste = New System.Windows.Forms.MenuItem
        Me.menuTableDelete = New System.Windows.Forms.MenuItem
        Me.menuTableRename = New System.Windows.Forms.MenuItem
        Me.MenuItem26 = New System.Windows.Forms.MenuItem
        Me.MenuItem28 = New System.Windows.Forms.MenuItem
        Me.MenuItem89 = New System.Windows.Forms.MenuItem
        Me.menuTableCodeDTD = New System.Windows.Forms.MenuItem
        Me.menuTablePluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuTablePlugins = New System.Windows.Forms.MenuItem
        Me.MenuItem109 = New System.Windows.Forms.MenuItem
        Me.menuTableProperties = New System.Windows.Forms.MenuItem
        Me.menuColumn = New System.Windows.Forms.ContextMenu
        Me.menuColumnCut = New System.Windows.Forms.MenuItem
        Me.menuColumnCopy = New System.Windows.Forms.MenuItem
        Me.menuColumnDelete = New System.Windows.Forms.MenuItem
        Me.menuColumnRename = New System.Windows.Forms.MenuItem
        Me.menuColumnPluginsSeparator = New System.Windows.Forms.MenuItem
        Me.menuColumnPlugins = New System.Windows.Forms.MenuItem
        Me.MenuItem107 = New System.Windows.Forms.MenuItem
        Me.menuColumnProperties = New System.Windows.Forms.MenuItem
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.menuConfigList = New System.Windows.Forms.ContextMenu
        Me.menuConfigListAddConfig = New System.Windows.Forms.MenuItem
        Me.menuConfig = New System.Windows.Forms.ContextMenu
        Me.menuConfigSetActive = New System.Windows.Forms.MenuItem
        Me.MenuItem44 = New System.Windows.Forms.MenuItem
        Me.menuConfigDelete = New System.Windows.Forms.MenuItem
        Me.menuConfigRename = New System.Windows.Forms.MenuItem
        Me.MenuItem110 = New System.Windows.Forms.MenuItem
        Me.menuConfigProperties = New System.Windows.Forms.MenuItem
        Me.menuDiagramList = New System.Windows.Forms.ContextMenu
        Me.menuDiagramListAddDiagram = New System.Windows.Forms.MenuItem
        Me.menuSourceCodeFile = New System.Windows.Forms.ContextMenu
        Me.menuSourceCodeFileRemove = New System.Windows.Forms.MenuItem
        Me.menuSourceCodeFileDelete = New System.Windows.Forms.MenuItem
        Me.MenuItem111 = New System.Windows.Forms.MenuItem
        Me.menuSourceCodeFileProperties = New System.Windows.Forms.MenuItem
        Me.menuProject = New System.Windows.Forms.ContextMenu
        Me.menuProjectAdd = New System.Windows.Forms.MenuItem
        Me.menuProjectAddNewDomain = New System.Windows.Forms.MenuItem
        Me.menuProjectAddExistingDomain = New System.Windows.Forms.MenuItem
        Me.MenuItem92 = New System.Windows.Forms.MenuItem
        Me.menuProjectSave = New System.Windows.Forms.MenuItem
        Me.menuProjectSaveAs = New System.Windows.Forms.MenuItem
        Me.menuProjectSaveAll = New System.Windows.Forms.MenuItem
        Me.MenuItem96 = New System.Windows.Forms.MenuItem
        Me.menuProjectPaste = New System.Windows.Forms.MenuItem
        Me.menuProjectRename = New System.Windows.Forms.MenuItem
        Me.MenuItem94 = New System.Windows.Forms.MenuItem
        Me.menuProjectClose = New System.Windows.Forms.MenuItem
        Me.MenuItem112 = New System.Windows.Forms.MenuItem
        Me.menuProjectProperties = New System.Windows.Forms.MenuItem
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.toolBarButtonBack = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonForward = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonUp = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton4 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonNew = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonNewItem = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonOpen = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonSave = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonSaveAll = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonPrint = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonPrintPreview = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton7 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonCut = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonCopy = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonPaste = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonFind = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonRun = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton6 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonSynch = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonDiscard = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonClassesToCode = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonCodeToClasses = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonTablesToClasses = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonClassesToTables = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonTablesToSource = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonSourceToTables = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonExplorer = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonProperties = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonTools = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonList = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonXmlBehind = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonUml = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton5 = New System.Windows.Forms.ToolBarButton
        Me.toolBarButtonWizards = New System.Windows.Forms.ToolBarButton
        Me.menuUserDoc = New System.Windows.Forms.ContextMenu
        Me.menuUserDocUndo = New System.Windows.Forms.MenuItem
        Me.menuUserDocRedo = New System.Windows.Forms.MenuItem
        Me.MenuItem115 = New System.Windows.Forms.MenuItem
        Me.menuUserDocCut = New System.Windows.Forms.MenuItem
        Me.menuUserDocCopy = New System.Windows.Forms.MenuItem
        Me.menuUserDocPaste = New System.Windows.Forms.MenuItem
        Me.menuUserDocDelete = New System.Windows.Forms.MenuItem
        Me.MenuItem119 = New System.Windows.Forms.MenuItem
        Me.menuUserDocSelectAll = New System.Windows.Forms.MenuItem
        Me.menuUmlDiagram = New System.Windows.Forms.ContextMenu
        Me.menuUmlView = New System.Windows.Forms.MenuItem
        Me.MenuItem121 = New System.Windows.Forms.MenuItem
        Me.menuUmlAddClass = New System.Windows.Forms.MenuItem
        Me.menuUmlAddClassNew = New System.Windows.Forms.MenuItem
        Me.menuUmlAddClassExisting = New System.Windows.Forms.MenuItem
        Me.menuUmlAddClassExistingAll = New System.Windows.Forms.MenuItem
        Me.menuUmlAddClassExistingAllWithLines = New System.Windows.Forms.MenuItem
        Me.MenuItem126 = New System.Windows.Forms.MenuItem
        Me.menuUmlAddClassExistingSelect = New System.Windows.Forms.MenuItem
        Me.menuUmlAddLines = New System.Windows.Forms.MenuItem
        Me.MenuItem117 = New System.Windows.Forms.MenuItem
        Me.menuUmlRemoveLines = New System.Windows.Forms.MenuItem
        Me.MenuItem113 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom = New System.Windows.Forms.MenuItem
        Me.menuUmlZoomIn = New System.Windows.Forms.MenuItem
        Me.menuUmlZoomOut = New System.Windows.Forms.MenuItem
        Me.MenuItem140 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom400 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom200 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom150 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom100 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom75 = New System.Windows.Forms.MenuItem
        Me.menuUmlZoom50 = New System.Windows.Forms.MenuItem
        Me.MenuItem138 = New System.Windows.Forms.MenuItem
        Me.menuUmlFit = New System.Windows.Forms.MenuItem
        Me.MenuItem123 = New System.Windows.Forms.MenuItem
        Me.menuUmlDeleteDiagram = New System.Windows.Forms.MenuItem
        Me.MenuItem128 = New System.Windows.Forms.MenuItem
        Me.menuUmSelectAll = New System.Windows.Forms.MenuItem
        Me.MenuItem141 = New System.Windows.Forms.MenuItem
        Me.menuUmlSaveViewAs = New System.Windows.Forms.MenuItem
        Me.menuUmlSaveAs = New System.Windows.Forms.MenuItem
        Me.MenuItem120 = New System.Windows.Forms.MenuItem
        Me.menuUmlProperties = New System.Windows.Forms.MenuItem
        Me.menuUmlLineEnd = New System.Windows.Forms.ContextMenu
        Me.menuUmlLineEndAddPoint = New System.Windows.Forms.MenuItem
        Me.MenuItem124 = New System.Windows.Forms.MenuItem
        Me.menuUmlLineEndLock = New System.Windows.Forms.MenuItem
        Me.menuUmlLineEndUnLock = New System.Windows.Forms.MenuItem
        Me.MenuItem131 = New System.Windows.Forms.MenuItem
        Me.menuUmlLineEndSelectLine = New System.Windows.Forms.MenuItem
        Me.MenuItem122 = New System.Windows.Forms.MenuItem
        Me.menuUmlLineEndRemoveLine = New System.Windows.Forms.MenuItem
        Me.MenuItem127 = New System.Windows.Forms.MenuItem
        Me.menuUmlLineEndProperties = New System.Windows.Forms.MenuItem
        Me.menuUmlLinePoint = New System.Windows.Forms.ContextMenu
        Me.menuUmlLinePointAddPoint = New System.Windows.Forms.MenuItem
        Me.MenuItem129 = New System.Windows.Forms.MenuItem
        Me.menuUmlLinePointRemove = New System.Windows.Forms.MenuItem
        Me.MenuItem130 = New System.Windows.Forms.MenuItem
        Me.menuUmlLinePointProperties = New System.Windows.Forms.MenuItem
        Me.panelProperties = New System.Windows.Forms.Panel
        Me.panelPropertiesTitle = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.buttonSwapPosProperties = New System.Windows.Forms.Button
        Me.buttonCloseProperties = New System.Windows.Forms.Button
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.SourceCodePrintDocument1 = New Puzzle.SourceCode.SourceCodePrintDocument
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog
        Me.PageSetupDialog1 = New System.Windows.Forms.PageSetupDialog
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.enumValueMenu = New System.Windows.Forms.ContextMenu
        Me.deleteEnumValueMenuItem = New System.Windows.Forms.MenuItem
        Me.treePreviewClassesToCode = New Puzzle.ObjectMapper.GUI.MapTreeView
        Me.mapTreeViewPreview = New Puzzle.ObjectMapper.GUI.MapTreeView
        Me.mapPropertyGrid = New Puzzle.ObjectMapper.GUI.MapPropertyGrid
        Me.mapListView = New Puzzle.ObjectMapper.GUI.MapListView
        Me.mapTreeView = New Puzzle.ObjectMapper.GUI.MapTreeView
        Me.panelStatus.SuspendLayout()
        CType(Me.statusMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusMessage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelLeft.SuspendLayout()
        Me.panelPropGridMap.SuspendLayout()
        Me.panelProjectTitle.SuspendLayout()
        Me.panelBottom.SuspendLayout()
        Me.tabControlMessages.SuspendLayout()
        Me.tabPageListView.SuspendLayout()
        Me.tabPageErrors.SuspendLayout()
        Me.tabPagePreviewList.SuspendLayout()
        Me.tabPageMessageList.SuspendLayout()
        Me.panelListTitle.SuspendLayout()
        Me.panelRight.SuspendLayout()
        Me.panelSecondTree.SuspendLayout()
        Me.tabControlTools.SuspendLayout()
        Me.tabToolsPreview.SuspendLayout()
        Me.panelToolsTitle.SuspendLayout()
        Me.panelMain.SuspendLayout()
        Me.panelDocuments.SuspendLayout()
        Me.tabControlDocuments.SuspendLayout()
        Me.tabPageMainUml.SuspendLayout()
        Me.panelTreeMap.SuspendLayout()
        Me.panelUmlTitle.SuspendLayout()
        Me.tabPageMainCustom.SuspendLayout()
        Me.panelDocTitle.SuspendLayout()
        Me.tabPageMainPreview.SuspendLayout()
        Me.panelPreviewDocTitle.SuspendLayout()
        Me.tabPageMainXmlBehind.SuspendLayout()
        Me.panelXmlBehindTitle.SuspendLayout()
        Me.tabPageMainCodeMap.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.panelMainTitle.SuspendLayout()
        Me.panelProperties.SuspendLayout()
        Me.panelPropertiesTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelStatus
        '
        Me.panelStatus.Controls.Add(Me.StatusBar1)
        Me.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelStatus.Location = New System.Drawing.Point(0, 379)
        Me.panelStatus.Name = "panelStatus"
        Me.panelStatus.Size = New System.Drawing.Size(679, 24)
        Me.panelStatus.TabIndex = 0
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 2)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.statusMain, Me.statusMessage})
        Me.StatusBar1.ShowPanels = True
        Me.StatusBar1.Size = New System.Drawing.Size(679, 22)
        Me.StatusBar1.TabIndex = 0
        Me.StatusBar1.Text = "StatusBar1"
        '
        'statusMain
        '
        Me.statusMain.Name = "statusMain"
        Me.statusMain.Text = "Ready"
        Me.statusMain.Width = 250
        '
        'statusMessage
        '
        Me.statusMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.statusMessage.Name = "statusMessage"
        Me.statusMessage.Width = 413
        '
        'menuNew
        '
        Me.menuNew.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuNewDomain, Me.menuNewProject})
        '
        'menuNewDomain
        '
        Me.menuNewDomain.DefaultItem = True
        Me.menuNewDomain.Index = 0
        Me.menuNewDomain.Text = "New Domain Model"
        '
        'menuNewProject
        '
        Me.menuNewProject.Index = 1
        Me.menuNewProject.Text = "New Project"
        '
        'menuNewItem
        '
        Me.menuNewItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuNewItemNewItem, Me.menuNewItemExistingItem, Me.MenuItem59, Me.menuNewItemClass, Me.menuNewItemProperty, Me.MenuItem60, Me.menuNewItemSource, Me.menuNewItemTable, Me.menuNewItemColumn})
        '
        'menuNewItemNewItem
        '
        Me.menuNewItemNewItem.DefaultItem = True
        Me.menuNewItemNewItem.Index = 0
        Me.menuNewItemNewItem.Text = "Add New Item..."
        '
        'menuNewItemExistingItem
        '
        Me.menuNewItemExistingItem.Index = 1
        Me.menuNewItemExistingItem.Text = "Add Existing Item..."
        '
        'MenuItem59
        '
        Me.MenuItem59.Index = 2
        Me.MenuItem59.Text = "-"
        Me.MenuItem59.Visible = False
        '
        'menuNewItemClass
        '
        Me.menuNewItemClass.Index = 3
        Me.menuNewItemClass.Text = "Add Class"
        Me.menuNewItemClass.Visible = False
        '
        'menuNewItemProperty
        '
        Me.menuNewItemProperty.Index = 4
        Me.menuNewItemProperty.Text = "Add Property"
        Me.menuNewItemProperty.Visible = False
        '
        'MenuItem60
        '
        Me.MenuItem60.Index = 5
        Me.MenuItem60.Text = "-"
        Me.MenuItem60.Visible = False
        '
        'menuNewItemSource
        '
        Me.menuNewItemSource.Index = 6
        Me.menuNewItemSource.Text = "Add Data Source"
        Me.menuNewItemSource.Visible = False
        '
        'menuNewItemTable
        '
        Me.menuNewItemTable.Index = 7
        Me.menuNewItemTable.Text = "Add Table"
        Me.menuNewItemTable.Visible = False
        '
        'menuNewItemColumn
        '
        Me.menuNewItemColumn.Index = 8
        Me.menuNewItemColumn.Text = "Add Column"
        Me.menuNewItemColumn.Visible = False
        '
        'menuSynchTools
        '
        Me.menuSynchTools.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchToolsSnc})
        '
        'menuSynchToolsSnc
        '
        Me.menuSynchToolsSnc.Index = 0
        Me.menuSynchToolsSnc.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchToolsSncDM, Me.menuSynchToolsSncCM, Me.menuSynchToolsSncTM})
        Me.menuSynchToolsSnc.Text = "Synchronize"
        '
        'menuSynchToolsSncDM
        '
        Me.menuSynchToolsSncDM.Index = 0
        Me.menuSynchToolsSncDM.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchToolsSncDMToCls, Me.menuSynchToolsSncDMToTbl})
        Me.menuSynchToolsSncDM.Text = "Domain Model"
        '
        'menuSynchToolsSncDMToCls
        '
        Me.menuSynchToolsSncDMToCls.Index = 0
        Me.menuSynchToolsSncDMToCls.Text = "From Classes To Tables"
        '
        'menuSynchToolsSncDMToTbl
        '
        Me.menuSynchToolsSncDMToTbl.Index = 1
        Me.menuSynchToolsSncDMToTbl.Text = "From Tables To Classes "
        '
        'menuSynchToolsSncCM
        '
        Me.menuSynchToolsSncCM.Index = 1
        Me.menuSynchToolsSncCM.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchToolsSncCMToCode, Me.menuSynchToolsSncCMToModel})
        Me.menuSynchToolsSncCM.Text = "Class Model"
        '
        'menuSynchToolsSncCMToCode
        '
        Me.menuSynchToolsSncCMToCode.Index = 0
        Me.menuSynchToolsSncCMToCode.Text = "From Model To Code"
        '
        'menuSynchToolsSncCMToModel
        '
        Me.menuSynchToolsSncCMToModel.Index = 1
        Me.menuSynchToolsSncCMToModel.Text = "From Code To Model"
        '
        'menuSynchToolsSncTM
        '
        Me.menuSynchToolsSncTM.Index = 2
        Me.menuSynchToolsSncTM.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchToolsSncToSource, Me.menuSynchToolsSncTMToModel})
        Me.menuSynchToolsSncTM.Text = "Table Model"
        '
        'menuSynchToolsSncToSource
        '
        Me.menuSynchToolsSncToSource.Index = 0
        Me.menuSynchToolsSncToSource.Text = "From Model To Data Sources"
        '
        'menuSynchToolsSncTMToModel
        '
        Me.menuSynchToolsSncTMToModel.Index = 1
        Me.menuSynchToolsSncTMToModel.Text = "From Data Sources To Model"
        '
        'menuSynch
        '
        Me.menuSynch.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchCommit, Me.menuSynchDiscard, Me.MenuItem32, Me.MenuItem29, Me.MenuItem30, Me.MenuItem31})
        '
        'menuSynchCommit
        '
        Me.menuSynchCommit.DefaultItem = True
        Me.menuSynchCommit.Index = 0
        Me.menuSynchCommit.Text = "Commit"
        '
        'menuSynchDiscard
        '
        Me.menuSynchDiscard.Index = 1
        Me.menuSynchDiscard.Text = "Discard"
        '
        'MenuItem32
        '
        Me.MenuItem32.Index = 2
        Me.MenuItem32.Text = "-"
        '
        'MenuItem29
        '
        Me.MenuItem29.Index = 3
        Me.MenuItem29.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSynchMarkAllChecked, Me.menuSynchMarkAllUnchecked})
        Me.MenuItem29.Text = "Mark All"
        '
        'menuSynchMarkAllChecked
        '
        Me.menuSynchMarkAllChecked.Index = 0
        Me.menuSynchMarkAllChecked.Text = "Checked"
        '
        'menuSynchMarkAllUnchecked
        '
        Me.menuSynchMarkAllUnchecked.Index = 1
        Me.menuSynchMarkAllUnchecked.Text = "Unchecked"
        '
        'MenuItem30
        '
        Me.MenuItem30.Index = 4
        Me.MenuItem30.Text = "-"
        '
        'MenuItem31
        '
        Me.MenuItem31.Index = 5
        Me.MenuItem31.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSyncRemoveUnchecked, Me.menuSyncRemoveChecked})
        Me.MenuItem31.Text = "Remove"
        '
        'menuSyncRemoveUnchecked
        '
        Me.menuSyncRemoveUnchecked.Index = 0
        Me.menuSyncRemoveUnchecked.Text = "Unchecked"
        '
        'menuSyncRemoveChecked
        '
        Me.menuSyncRemoveChecked.Index = 1
        Me.menuSyncRemoveChecked.Text = "Checked"
        '
        'menuWizards
        '
        Me.menuWizards.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuWizardGenDom, Me.menuWizardWrapDb})
        '
        'menuWizardGenDom
        '
        Me.menuWizardGenDom.DefaultItem = True
        Me.menuWizardGenDom.Index = 0
        Me.menuWizardGenDom.Text = "Implement Class Model Wizard"
        '
        'menuWizardWrapDb
        '
        Me.menuWizardWrapDb.Index = 1
        Me.menuWizardWrapDb.Text = "Wrap Database Wizard"
        '
        'menuXmlBehind
        '
        Me.menuXmlBehind.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuXmlOpen, Me.menuXmlClose, Me.MenuItem93, Me.menuXmlApply, Me.menuXmlDiscard})
        '
        'menuXmlOpen
        '
        Me.menuXmlOpen.DefaultItem = True
        Me.menuXmlOpen.Index = 0
        Me.menuXmlOpen.Text = "Open"
        '
        'menuXmlClose
        '
        Me.menuXmlClose.Index = 1
        Me.menuXmlClose.Text = "Close"
        '
        'MenuItem93
        '
        Me.MenuItem93.Index = 2
        Me.MenuItem93.Text = "-"
        '
        'menuXmlApply
        '
        Me.menuXmlApply.Index = 3
        Me.menuXmlApply.Text = "Apply Changes"
        '
        'menuXmlDiscard
        '
        Me.menuXmlDiscard.Index = 4
        Me.menuXmlDiscard.Text = "Discard Changes"
        '
        'imageListSmall
        '
        Me.imageListSmall.ImageStream = CType(resources.GetObject("imageListSmall.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageListSmall.TransparentColor = System.Drawing.Color.Transparent
        Me.imageListSmall.Images.SetKeyName(0, "")
        Me.imageListSmall.Images.SetKeyName(1, "")
        Me.imageListSmall.Images.SetKeyName(2, "")
        Me.imageListSmall.Images.SetKeyName(3, "")
        Me.imageListSmall.Images.SetKeyName(4, "")
        Me.imageListSmall.Images.SetKeyName(5, "")
        Me.imageListSmall.Images.SetKeyName(6, "")
        Me.imageListSmall.Images.SetKeyName(7, "")
        Me.imageListSmall.Images.SetKeyName(8, "")
        Me.imageListSmall.Images.SetKeyName(9, "")
        Me.imageListSmall.Images.SetKeyName(10, "")
        Me.imageListSmall.Images.SetKeyName(11, "")
        Me.imageListSmall.Images.SetKeyName(12, "")
        Me.imageListSmall.Images.SetKeyName(13, "")
        Me.imageListSmall.Images.SetKeyName(14, "")
        Me.imageListSmall.Images.SetKeyName(15, "")
        Me.imageListSmall.Images.SetKeyName(16, "")
        Me.imageListSmall.Images.SetKeyName(17, "")
        Me.imageListSmall.Images.SetKeyName(18, "")
        Me.imageListSmall.Images.SetKeyName(19, "")
        Me.imageListSmall.Images.SetKeyName(20, "")
        Me.imageListSmall.Images.SetKeyName(21, "")
        Me.imageListSmall.Images.SetKeyName(22, "")
        Me.imageListSmall.Images.SetKeyName(23, "")
        Me.imageListSmall.Images.SetKeyName(24, "")
        Me.imageListSmall.Images.SetKeyName(25, "")
        Me.imageListSmall.Images.SetKeyName(26, "")
        Me.imageListSmall.Images.SetKeyName(27, "")
        Me.imageListSmall.Images.SetKeyName(28, "")
        Me.imageListSmall.Images.SetKeyName(29, "")
        Me.imageListSmall.Images.SetKeyName(30, "")
        Me.imageListSmall.Images.SetKeyName(31, "")
        Me.imageListSmall.Images.SetKeyName(32, "")
        Me.imageListSmall.Images.SetKeyName(33, "")
        Me.imageListSmall.Images.SetKeyName(34, "")
        Me.imageListSmall.Images.SetKeyName(35, "")
        Me.imageListSmall.Images.SetKeyName(36, "")
        Me.imageListSmall.Images.SetKeyName(37, "")
        Me.imageListSmall.Images.SetKeyName(38, "")
        Me.imageListSmall.Images.SetKeyName(39, "")
        Me.imageListSmall.Images.SetKeyName(40, "")
        Me.imageListSmall.Images.SetKeyName(41, "")
        Me.imageListSmall.Images.SetKeyName(42, "")
        Me.imageListSmall.Images.SetKeyName(43, "")
        Me.imageListSmall.Images.SetKeyName(44, "")
        Me.imageListSmall.Images.SetKeyName(45, "")
        Me.imageListSmall.Images.SetKeyName(46, "")
        Me.imageListSmall.Images.SetKeyName(47, "")
        Me.imageListSmall.Images.SetKeyName(48, "")
        Me.imageListSmall.Images.SetKeyName(49, "")
        Me.imageListSmall.Images.SetKeyName(50, "")
        Me.imageListSmall.Images.SetKeyName(51, "")
        Me.imageListSmall.Images.SetKeyName(52, "")
        Me.imageListSmall.Images.SetKeyName(53, "")
        Me.imageListSmall.Images.SetKeyName(54, "")
        Me.imageListSmall.Images.SetKeyName(55, "")
        Me.imageListSmall.Images.SetKeyName(56, "")
        Me.imageListSmall.Images.SetKeyName(57, "")
        Me.imageListSmall.Images.SetKeyName(58, "")
        Me.imageListSmall.Images.SetKeyName(59, "")
        Me.imageListSmall.Images.SetKeyName(60, "")
        Me.imageListSmall.Images.SetKeyName(61, "")
        Me.imageListSmall.Images.SetKeyName(62, "")
        Me.imageListSmall.Images.SetKeyName(63, "")
        Me.imageListSmall.Images.SetKeyName(64, "")
        Me.imageListSmall.Images.SetKeyName(65, "")
        Me.imageListSmall.Images.SetKeyName(66, "")
        Me.imageListSmall.Images.SetKeyName(67, "")
        Me.imageListSmall.Images.SetKeyName(68, "")
        Me.imageListSmall.Images.SetKeyName(69, "")
        Me.imageListSmall.Images.SetKeyName(70, "")
        Me.imageListSmall.Images.SetKeyName(71, "")
        Me.imageListSmall.Images.SetKeyName(72, "")
        Me.imageListSmall.Images.SetKeyName(73, "")
        Me.imageListSmall.Images.SetKeyName(74, "")
        Me.imageListSmall.Images.SetKeyName(75, "")
        Me.imageListSmall.Images.SetKeyName(76, "")
        Me.imageListSmall.Images.SetKeyName(77, "")
        Me.imageListSmall.Images.SetKeyName(78, "")
        Me.imageListSmall.Images.SetKeyName(79, "")
        Me.imageListSmall.Images.SetKeyName(80, "")
        Me.imageListSmall.Images.SetKeyName(81, "")
        Me.imageListSmall.Images.SetKeyName(82, "")
        Me.imageListSmall.Images.SetKeyName(83, "")
        Me.imageListSmall.Images.SetKeyName(84, "")
        Me.imageListSmall.Images.SetKeyName(85, "")
        Me.imageListSmall.Images.SetKeyName(86, "")
        Me.imageListSmall.Images.SetKeyName(87, "")
        Me.imageListSmall.Images.SetKeyName(88, "")
        Me.imageListSmall.Images.SetKeyName(89, "")
        Me.imageListSmall.Images.SetKeyName(90, "")
        Me.imageListSmall.Images.SetKeyName(91, "")
        Me.imageListSmall.Images.SetKeyName(92, "")
        Me.imageListSmall.Images.SetKeyName(93, "")
        Me.imageListSmall.Images.SetKeyName(94, "")
        Me.imageListSmall.Images.SetKeyName(95, "")
        Me.imageListSmall.Images.SetKeyName(96, "")
        Me.imageListSmall.Images.SetKeyName(97, "")
        Me.imageListSmall.Images.SetKeyName(98, "")
        Me.imageListSmall.Images.SetKeyName(99, "")
        Me.imageListSmall.Images.SetKeyName(100, "")
        Me.imageListSmall.Images.SetKeyName(101, "")
        Me.imageListSmall.Images.SetKeyName(102, "")
        Me.imageListSmall.Images.SetKeyName(103, "")
        Me.imageListSmall.Images.SetKeyName(104, "")
        Me.imageListSmall.Images.SetKeyName(105, "")
        Me.imageListSmall.Images.SetKeyName(106, "")
        Me.imageListSmall.Images.SetKeyName(107, "")
        Me.imageListSmall.Images.SetKeyName(108, "")
        Me.imageListSmall.Images.SetKeyName(109, "")
        Me.imageListSmall.Images.SetKeyName(110, "")
        Me.imageListSmall.Images.SetKeyName(111, "")
        Me.imageListSmall.Images.SetKeyName(112, "")
        Me.imageListSmall.Images.SetKeyName(113, "")
        Me.imageListSmall.Images.SetKeyName(114, "")
        Me.imageListSmall.Images.SetKeyName(115, "")
        Me.imageListSmall.Images.SetKeyName(116, "")
        Me.imageListSmall.Images.SetKeyName(117, "")
        Me.imageListSmall.Images.SetKeyName(118, "")
        '
        'panelLeft
        '
        Me.panelLeft.Controls.Add(Me.panelPropGridMap)
        Me.panelLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelLeft.Location = New System.Drawing.Point(0, 72)
        Me.panelLeft.Name = "panelLeft"
        Me.panelLeft.Size = New System.Drawing.Size(160, 307)
        Me.panelLeft.TabIndex = 2
        '
        'panelPropGridMap
        '
        Me.panelPropGridMap.Controls.Add(Me.mapTreeView)
        Me.panelPropGridMap.Controls.Add(Me.panelProjectTitle)
        Me.panelPropGridMap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelPropGridMap.Location = New System.Drawing.Point(0, 0)
        Me.panelPropGridMap.Name = "panelPropGridMap"
        Me.panelPropGridMap.Size = New System.Drawing.Size(160, 307)
        Me.panelPropGridMap.TabIndex = 2
        '
        'panelProjectTitle
        '
        Me.panelProjectTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelProjectTitle.Controls.Add(Me.labelExplorer)
        Me.panelProjectTitle.Controls.Add(Me.buttonSwapPosExplorer)
        Me.panelProjectTitle.Controls.Add(Me.buttonCloseExplorer)
        Me.panelProjectTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelProjectTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelProjectTitle.Name = "panelProjectTitle"
        Me.panelProjectTitle.Size = New System.Drawing.Size(160, 16)
        Me.panelProjectTitle.TabIndex = 4
        '
        'labelExplorer
        '
        Me.labelExplorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelExplorer.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.labelExplorer.Location = New System.Drawing.Point(0, 0)
        Me.labelExplorer.Name = "labelExplorer"
        Me.labelExplorer.Size = New System.Drawing.Size(126, 14)
        Me.labelExplorer.TabIndex = 0
        Me.labelExplorer.Text = "Project Explorer"
        '
        'buttonSwapPosExplorer
        '
        Me.buttonSwapPosExplorer.BackColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosExplorer.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonSwapPosExplorer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonSwapPosExplorer.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosExplorer.ImageIndex = 4
        Me.buttonSwapPosExplorer.ImageList = Me.imageListSmallButtons
        Me.buttonSwapPosExplorer.Location = New System.Drawing.Point(126, 0)
        Me.buttonSwapPosExplorer.Name = "buttonSwapPosExplorer"
        Me.buttonSwapPosExplorer.Size = New System.Drawing.Size(16, 14)
        Me.buttonSwapPosExplorer.TabIndex = 3
        Me.buttonSwapPosExplorer.UseVisualStyleBackColor = False
        '
        'imageListSmallButtons
        '
        Me.imageListSmallButtons.ImageStream = CType(resources.GetObject("imageListSmallButtons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageListSmallButtons.TransparentColor = System.Drawing.Color.Transparent
        Me.imageListSmallButtons.Images.SetKeyName(0, "")
        Me.imageListSmallButtons.Images.SetKeyName(1, "")
        Me.imageListSmallButtons.Images.SetKeyName(2, "")
        Me.imageListSmallButtons.Images.SetKeyName(3, "")
        Me.imageListSmallButtons.Images.SetKeyName(4, "")
        Me.imageListSmallButtons.Images.SetKeyName(5, "")
        Me.imageListSmallButtons.Images.SetKeyName(6, "")
        Me.imageListSmallButtons.Images.SetKeyName(7, "")
        Me.imageListSmallButtons.Images.SetKeyName(8, "")
        Me.imageListSmallButtons.Images.SetKeyName(9, "")
        Me.imageListSmallButtons.Images.SetKeyName(10, "")
        Me.imageListSmallButtons.Images.SetKeyName(11, "")
        '
        'buttonCloseExplorer
        '
        Me.buttonCloseExplorer.BackColor = System.Drawing.SystemColors.Control
        Me.buttonCloseExplorer.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseExplorer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseExplorer.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonCloseExplorer.ImageIndex = 0
        Me.buttonCloseExplorer.ImageList = Me.imageListSmallButtons
        Me.buttonCloseExplorer.Location = New System.Drawing.Point(142, 0)
        Me.buttonCloseExplorer.Name = "buttonCloseExplorer"
        Me.buttonCloseExplorer.Size = New System.Drawing.Size(16, 14)
        Me.buttonCloseExplorer.TabIndex = 2
        Me.buttonCloseExplorer.UseVisualStyleBackColor = False
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.tabControlMessages)
        Me.panelBottom.Controls.Add(Me.panelListTitle)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(163, 203)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(516, 176)
        Me.panelBottom.TabIndex = 3
        '
        'tabControlMessages
        '
        Me.tabControlMessages.Controls.Add(Me.tabPageListView)
        Me.tabControlMessages.Controls.Add(Me.tabPageErrors)
        Me.tabControlMessages.Controls.Add(Me.tabPagePreviewList)
        Me.tabControlMessages.Controls.Add(Me.tabPageMessageList)
        Me.tabControlMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlMessages.Location = New System.Drawing.Point(0, 16)
        Me.tabControlMessages.Name = "tabControlMessages"
        Me.tabControlMessages.SelectedIndex = 0
        Me.tabControlMessages.Size = New System.Drawing.Size(516, 160)
        Me.tabControlMessages.TabIndex = 4
        '
        'tabPageListView
        '
        Me.tabPageListView.Controls.Add(Me.mapListView)
        Me.tabPageListView.Location = New System.Drawing.Point(4, 22)
        Me.tabPageListView.Name = "tabPageListView"
        Me.tabPageListView.Size = New System.Drawing.Size(508, 134)
        Me.tabPageListView.TabIndex = 3
        Me.tabPageListView.Text = "List View"
        '
        'tabPageErrors
        '
        Me.tabPageErrors.Controls.Add(Me.listViewExceptions)
        Me.tabPageErrors.Location = New System.Drawing.Point(4, 22)
        Me.tabPageErrors.Name = "tabPageErrors"
        Me.tabPageErrors.Size = New System.Drawing.Size(508, 134)
        Me.tabPageErrors.TabIndex = 0
        Me.tabPageErrors.Text = "Errors"
        '
        'listViewExceptions
        '
        Me.listViewExceptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listViewExceptions.HideSelection = False
        Me.listViewExceptions.Location = New System.Drawing.Point(0, 0)
        Me.listViewExceptions.Name = "listViewExceptions"
        Me.listViewExceptions.Size = New System.Drawing.Size(508, 134)
        Me.listViewExceptions.SmallImageList = Me.imageListSmall
        Me.listViewExceptions.TabIndex = 4
        Me.listViewExceptions.UseCompatibleStateImageBehavior = False
        Me.listViewExceptions.View = System.Windows.Forms.View.Details
        '
        'tabPagePreviewList
        '
        Me.tabPagePreviewList.Controls.Add(Me.listViewPreviewMsgs)
        Me.tabPagePreviewList.Location = New System.Drawing.Point(4, 22)
        Me.tabPagePreviewList.Name = "tabPagePreviewList"
        Me.tabPagePreviewList.Size = New System.Drawing.Size(508, 134)
        Me.tabPagePreviewList.TabIndex = 1
        Me.tabPagePreviewList.Text = "Preview"
        '
        'listViewPreviewMsgs
        '
        Me.listViewPreviewMsgs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listViewPreviewMsgs.HideSelection = False
        Me.listViewPreviewMsgs.Location = New System.Drawing.Point(0, 0)
        Me.listViewPreviewMsgs.Name = "listViewPreviewMsgs"
        Me.listViewPreviewMsgs.Size = New System.Drawing.Size(508, 134)
        Me.listViewPreviewMsgs.SmallImageList = Me.imageListSmall
        Me.listViewPreviewMsgs.TabIndex = 6
        Me.listViewPreviewMsgs.UseCompatibleStateImageBehavior = False
        Me.listViewPreviewMsgs.View = System.Windows.Forms.View.Details
        '
        'tabPageMessageList
        '
        Me.tabPageMessageList.Controls.Add(Me.listViewMsgs)
        Me.tabPageMessageList.Location = New System.Drawing.Point(4, 22)
        Me.tabPageMessageList.Name = "tabPageMessageList"
        Me.tabPageMessageList.Size = New System.Drawing.Size(508, 134)
        Me.tabPageMessageList.TabIndex = 2
        Me.tabPageMessageList.Text = "Messages"
        '
        'listViewMsgs
        '
        Me.listViewMsgs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listViewMsgs.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listViewMsgs.HideSelection = False
        Me.listViewMsgs.Location = New System.Drawing.Point(0, 0)
        Me.listViewMsgs.Name = "listViewMsgs"
        Me.listViewMsgs.Size = New System.Drawing.Size(508, 134)
        Me.listViewMsgs.TabIndex = 6
        Me.listViewMsgs.UseCompatibleStateImageBehavior = False
        Me.listViewMsgs.View = System.Windows.Forms.View.Details
        '
        'panelListTitle
        '
        Me.panelListTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelListTitle.Controls.Add(Me.labelListTitle)
        Me.panelListTitle.Controls.Add(Me.buttonSwapPosList)
        Me.panelListTitle.Controls.Add(Me.buttonCloseList)
        Me.panelListTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelListTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelListTitle.Name = "panelListTitle"
        Me.panelListTitle.Size = New System.Drawing.Size(516, 16)
        Me.panelListTitle.TabIndex = 5
        '
        'labelListTitle
        '
        Me.labelListTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelListTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelListTitle.Name = "labelListTitle"
        Me.labelListTitle.Size = New System.Drawing.Size(482, 14)
        Me.labelListTitle.TabIndex = 6
        Me.labelListTitle.Text = "List Pane"
        '
        'buttonSwapPosList
        '
        Me.buttonSwapPosList.BackColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosList.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonSwapPosList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonSwapPosList.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosList.ImageIndex = 10
        Me.buttonSwapPosList.ImageList = Me.imageListSmallButtons
        Me.buttonSwapPosList.Location = New System.Drawing.Point(482, 0)
        Me.buttonSwapPosList.Name = "buttonSwapPosList"
        Me.buttonSwapPosList.Size = New System.Drawing.Size(16, 14)
        Me.buttonSwapPosList.TabIndex = 7
        Me.buttonSwapPosList.UseVisualStyleBackColor = False
        '
        'buttonCloseList
        '
        Me.buttonCloseList.BackColor = System.Drawing.SystemColors.Control
        Me.buttonCloseList.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseList.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonCloseList.ImageIndex = 0
        Me.buttonCloseList.ImageList = Me.imageListSmallButtons
        Me.buttonCloseList.Location = New System.Drawing.Point(498, 0)
        Me.buttonCloseList.Name = "buttonCloseList"
        Me.buttonCloseList.Size = New System.Drawing.Size(16, 14)
        Me.buttonCloseList.TabIndex = 5
        Me.buttonCloseList.UseVisualStyleBackColor = False
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(160, 72)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 307)
        Me.Splitter1.TabIndex = 4
        Me.Splitter1.TabStop = False
        '
        'panelRight
        '
        Me.panelRight.Controls.Add(Me.panelSecondTree)
        Me.panelRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.panelRight.Location = New System.Drawing.Point(535, 72)
        Me.panelRight.Name = "panelRight"
        Me.panelRight.Size = New System.Drawing.Size(144, 128)
        Me.panelRight.TabIndex = 6
        '
        'panelSecondTree
        '
        Me.panelSecondTree.Controls.Add(Me.tabControlTools)
        Me.panelSecondTree.Controls.Add(Me.panelToolsTitle)
        Me.panelSecondTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSecondTree.Location = New System.Drawing.Point(0, 0)
        Me.panelSecondTree.Name = "panelSecondTree"
        Me.panelSecondTree.Size = New System.Drawing.Size(144, 128)
        Me.panelSecondTree.TabIndex = 2
        '
        'tabControlTools
        '
        Me.tabControlTools.AllowDrop = True
        Me.tabControlTools.Controls.Add(Me.tabToolsPreview)
        Me.tabControlTools.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlTools.Location = New System.Drawing.Point(0, 16)
        Me.tabControlTools.Name = "tabControlTools"
        Me.tabControlTools.SelectedIndex = 0
        Me.tabControlTools.Size = New System.Drawing.Size(144, 112)
        Me.tabControlTools.TabIndex = 2
        '
        'tabToolsPreview
        '
        Me.tabToolsPreview.Controls.Add(Me.treePreviewClassesToCode)
        Me.tabToolsPreview.Controls.Add(Me.mapTreeViewPreview)
        Me.tabToolsPreview.Location = New System.Drawing.Point(4, 22)
        Me.tabToolsPreview.Name = "tabToolsPreview"
        Me.tabToolsPreview.Size = New System.Drawing.Size(136, 86)
        Me.tabToolsPreview.TabIndex = 1
        Me.tabToolsPreview.Text = "Preview"
        '
        'panelToolsTitle
        '
        Me.panelToolsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelToolsTitle.Controls.Add(Me.Label3)
        Me.panelToolsTitle.Controls.Add(Me.buttonSwapPosTools)
        Me.panelToolsTitle.Controls.Add(Me.buttonCloseTools)
        Me.panelToolsTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelToolsTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelToolsTitle.Name = "panelToolsTitle"
        Me.panelToolsTitle.Size = New System.Drawing.Size(144, 16)
        Me.panelToolsTitle.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Tools"
        '
        'buttonSwapPosTools
        '
        Me.buttonSwapPosTools.BackColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosTools.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonSwapPosTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonSwapPosTools.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosTools.ImageIndex = 2
        Me.buttonSwapPosTools.ImageList = Me.imageListSmallButtons
        Me.buttonSwapPosTools.Location = New System.Drawing.Point(111, 0)
        Me.buttonSwapPosTools.Name = "buttonSwapPosTools"
        Me.buttonSwapPosTools.Size = New System.Drawing.Size(16, 14)
        Me.buttonSwapPosTools.TabIndex = 5
        Me.buttonSwapPosTools.UseVisualStyleBackColor = False
        '
        'buttonCloseTools
        '
        Me.buttonCloseTools.BackColor = System.Drawing.SystemColors.Control
        Me.buttonCloseTools.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseTools.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonCloseTools.ImageIndex = 0
        Me.buttonCloseTools.ImageList = Me.imageListSmallButtons
        Me.buttonCloseTools.Location = New System.Drawing.Point(127, 0)
        Me.buttonCloseTools.Name = "buttonCloseTools"
        Me.buttonCloseTools.Size = New System.Drawing.Size(15, 14)
        Me.buttonCloseTools.TabIndex = 4
        Me.buttonCloseTools.UseVisualStyleBackColor = False
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFile, Me.menuEdit, Me.MenuItem41, Me.menuFileTools, Me.menuToolsSynch, Me.MenuItem71, Me.MenuItem43})
        '
        'menuFile
        '
        Me.menuFile.Index = 0
        Me.menuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem6, Me.MenuItem3, Me.menuFileClose, Me.MenuItem5, Me.menuFileOpenProject, Me.menuFileCloseProject, Me.MenuItem34, Me.menuFileSave, Me.menuFileSaveAs, Me.menuFileSaveAll, Me.menuFileImpExpSep, Me.menuFileExport, Me.MenuItem139, Me.menuFilePageSetup, Me.menuFilePrintPreview, Me.menuFilePrint, Me.MenuItem68, Me.menuFileRecentProjects, Me.MenuItem114, Me.menuFileExit})
        Me.menuFile.Text = "File"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 0
        Me.MenuItem6.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFileNewMap, Me.menuFileNewProject, Me.menuFileNewFile})
        Me.MenuItem6.Text = "New"
        '
        'menuFileNewMap
        '
        Me.menuFileNewMap.Index = 0
        Me.menuFileNewMap.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.menuFileNewMap.Text = "Domain Model"
        '
        'menuFileNewProject
        '
        Me.menuFileNewProject.Index = 1
        Me.menuFileNewProject.Text = "Project"
        '
        'menuFileNewFile
        '
        Me.menuFileNewFile.Index = 2
        Me.menuFileNewFile.Text = "File"
        Me.menuFileNewFile.Visible = False
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFileOpenModel, Me.menuFileOpenFile})
        Me.MenuItem3.Text = "Open"
        '
        'menuFileOpenModel
        '
        Me.menuFileOpenModel.Index = 0
        Me.menuFileOpenModel.Text = "Domain Model"
        '
        'menuFileOpenFile
        '
        Me.menuFileOpenFile.Index = 1
        Me.menuFileOpenFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO
        Me.menuFileOpenFile.Text = "File"
        '
        'menuFileClose
        '
        Me.menuFileClose.Index = 2
        Me.menuFileClose.Text = "Close"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 3
        Me.MenuItem5.Text = "-"
        '
        'menuFileOpenProject
        '
        Me.menuFileOpenProject.Index = 4
        Me.menuFileOpenProject.Text = "Open Project"
        '
        'menuFileCloseProject
        '
        Me.menuFileCloseProject.Index = 5
        Me.menuFileCloseProject.Text = "Close Project"
        '
        'MenuItem34
        '
        Me.MenuItem34.Index = 6
        Me.MenuItem34.Text = "-"
        '
        'menuFileSave
        '
        Me.menuFileSave.Index = 7
        Me.menuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.menuFileSave.Text = "Save"
        '
        'menuFileSaveAs
        '
        Me.menuFileSaveAs.Index = 8
        Me.menuFileSaveAs.Text = "Save As..."
        '
        'menuFileSaveAll
        '
        Me.menuFileSaveAll.Index = 9
        Me.menuFileSaveAll.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS
        Me.menuFileSaveAll.Text = "Save All"
        '
        'menuFileImpExpSep
        '
        Me.menuFileImpExpSep.Index = 10
        Me.menuFileImpExpSep.Text = "-"
        '
        'menuFileExport
        '
        Me.menuFileExport.Index = 11
        Me.menuFileExport.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFileExportNHib})
        Me.menuFileExport.Text = "Export"
        '
        'menuFileExportNHib
        '
        Me.menuFileExportNHib.Index = 0
        Me.menuFileExportNHib.Text = "NHibernate"
        '
        'MenuItem139
        '
        Me.MenuItem139.Index = 12
        Me.MenuItem139.Text = "-"
        '
        'menuFilePageSetup
        '
        Me.menuFilePageSetup.Index = 13
        Me.menuFilePageSetup.Text = "Page Setup"
        '
        'menuFilePrintPreview
        '
        Me.menuFilePrintPreview.Index = 14
        Me.menuFilePrintPreview.Text = "Print Preview"
        '
        'menuFilePrint
        '
        Me.menuFilePrint.Index = 15
        Me.menuFilePrint.Text = "Print"
        '
        'MenuItem68
        '
        Me.MenuItem68.Index = 16
        Me.MenuItem68.Text = "-"
        '
        'menuFileRecentProjects
        '
        Me.menuFileRecentProjects.Index = 17
        Me.menuFileRecentProjects.Text = "Recent Projects"
        '
        'MenuItem114
        '
        Me.MenuItem114.Index = 18
        Me.MenuItem114.Text = "-"
        '
        'menuFileExit
        '
        Me.menuFileExit.Index = 19
        Me.menuFileExit.Text = "Exit"
        '
        'menuEdit
        '
        Me.menuEdit.Index = 1
        Me.menuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuEditCut, Me.menuEditCopy, Me.menuEditPaste, Me.menuEditDelete, Me.MenuItem4, Me.menuEditSelectAll, Me.MenuItem98, Me.menuEditFindAndReplace})
        Me.menuEdit.Text = "Edit"
        '
        'menuEditCut
        '
        Me.menuEditCut.Index = 0
        Me.menuEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX
        Me.menuEditCut.Text = "Cut"
        '
        'menuEditCopy
        '
        Me.menuEditCopy.Index = 1
        Me.menuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.menuEditCopy.Text = "Copy"
        '
        'menuEditPaste
        '
        Me.menuEditPaste.Index = 2
        Me.menuEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV
        Me.menuEditPaste.Text = "Paste"
        '
        'menuEditDelete
        '
        Me.menuEditDelete.Index = 3
        Me.menuEditDelete.Text = "Delete"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 4
        Me.MenuItem4.Text = "-"
        '
        'menuEditSelectAll
        '
        Me.menuEditSelectAll.Index = 5
        Me.menuEditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA
        Me.menuEditSelectAll.Text = "Select All"
        '
        'MenuItem98
        '
        Me.MenuItem98.Index = 6
        Me.MenuItem98.Text = "-"
        '
        'menuEditFindAndReplace
        '
        Me.menuEditFindAndReplace.Index = 7
        Me.menuEditFindAndReplace.Shortcut = System.Windows.Forms.Shortcut.CtrlF
        Me.menuEditFindAndReplace.Text = "Find and Replace"
        '
        'MenuItem41
        '
        Me.MenuItem41.Index = 2
        Me.MenuItem41.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuViewProjectExplorer, Me.menuViewProperties, Me.menuViewMain, Me.menuViewTools, Me.menuViewMsgList, Me.MenuItem67, Me.menuViewToolBar, Me.menuViewStatusBar})
        Me.MenuItem41.Text = "View"
        '
        'menuViewProjectExplorer
        '
        Me.menuViewProjectExplorer.Index = 0
        Me.menuViewProjectExplorer.Text = "Project Explorer"
        '
        'menuViewProperties
        '
        Me.menuViewProperties.Index = 1
        Me.menuViewProperties.Text = "Properties"
        '
        'menuViewMain
        '
        Me.menuViewMain.Index = 2
        Me.menuViewMain.Text = "Documents"
        '
        'menuViewTools
        '
        Me.menuViewTools.Index = 3
        Me.menuViewTools.Text = "Tools"
        '
        'menuViewMsgList
        '
        Me.menuViewMsgList.Index = 4
        Me.menuViewMsgList.Text = "List Pane"
        '
        'MenuItem67
        '
        Me.MenuItem67.Index = 5
        Me.MenuItem67.Text = "-"
        '
        'menuViewToolBar
        '
        Me.menuViewToolBar.Index = 6
        Me.menuViewToolBar.Text = "Tool Bar"
        '
        'menuViewStatusBar
        '
        Me.menuViewStatusBar.Index = 7
        Me.menuViewStatusBar.Text = "Status Bar"
        '
        'menuFileTools
        '
        Me.menuFileTools.Index = 3
        Me.menuFileTools.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsVerify, Me.MenuItem37, Me.menuToolsXml, Me.MenuItem69, Me.menuToolsWiz, Me.MenuItem61, Me.menuToolsNPersist, Me.MenuItem36, Me.menuToolsPlugins, Me.menuToolsPluginsSeparator, Me.menuToolsOptions})
        Me.menuFileTools.Text = "Tools"
        '
        'menuToolsVerify
        '
        Me.menuToolsVerify.Index = 0
        Me.menuToolsVerify.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsVerifyVerify, Me.MenuItem45, Me.menuToolsVerifyAuto, Me.menuToolsVerifyMappings})
        Me.menuToolsVerify.Text = "Verification"
        '
        'menuToolsVerifyVerify
        '
        Me.menuToolsVerifyVerify.Index = 0
        Me.menuToolsVerifyVerify.Text = "Verify Model"
        '
        'MenuItem45
        '
        Me.MenuItem45.Index = 1
        Me.MenuItem45.Text = "-"
        '
        'menuToolsVerifyAuto
        '
        Me.menuToolsVerifyAuto.Index = 2
        Me.menuToolsVerifyAuto.Text = "Auto Verify Model"
        '
        'menuToolsVerifyMappings
        '
        Me.menuToolsVerifyMappings.Index = 3
        Me.menuToolsVerifyMappings.Text = "Verify O/R Mappings"
        '
        'MenuItem37
        '
        Me.MenuItem37.Index = 1
        Me.MenuItem37.Text = "-"
        '
        'menuToolsXml
        '
        Me.menuToolsXml.Index = 2
        Me.menuToolsXml.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsXmlOpen, Me.menuToolsXmlClose, Me.MenuItem105, Me.menuToolsXmlApply, Me.menuToolsXmlDiscard})
        Me.menuToolsXml.Text = "Xml Behind"
        '
        'menuToolsXmlOpen
        '
        Me.menuToolsXmlOpen.Index = 0
        Me.menuToolsXmlOpen.Text = "Open"
        '
        'menuToolsXmlClose
        '
        Me.menuToolsXmlClose.Index = 1
        Me.menuToolsXmlClose.Text = "Close"
        '
        'MenuItem105
        '
        Me.MenuItem105.Index = 2
        Me.MenuItem105.Text = "-"
        '
        'menuToolsXmlApply
        '
        Me.menuToolsXmlApply.Index = 3
        Me.menuToolsXmlApply.Text = "Apply Changes"
        '
        'menuToolsXmlDiscard
        '
        Me.menuToolsXmlDiscard.Index = 4
        Me.menuToolsXmlDiscard.Text = "Discard Changes"
        '
        'MenuItem69
        '
        Me.MenuItem69.Index = 3
        Me.MenuItem69.Text = "-"
        '
        'menuToolsWiz
        '
        Me.menuToolsWiz.Index = 4
        Me.menuToolsWiz.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsWizGenDomWiz, Me.menuToolsWizWrapDbWiz})
        Me.menuToolsWiz.Text = "Wizards"
        '
        'menuToolsWizGenDomWiz
        '
        Me.menuToolsWizGenDomWiz.Index = 0
        Me.menuToolsWizGenDomWiz.Text = "Implement Class Model Wizard"
        '
        'menuToolsWizWrapDbWiz
        '
        Me.menuToolsWizWrapDbWiz.Index = 1
        Me.menuToolsWizWrapDbWiz.Text = "Wrap Database Wizard"
        '
        'MenuItem61
        '
        Me.MenuItem61.Index = 5
        Me.MenuItem61.Text = "-"
        '
        'menuToolsNPersist
        '
        Me.menuToolsNPersist.Index = 6
        Me.menuToolsNPersist.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsNPersistDomainExplorer, Me.menuToolsNPersistQueryAnalyzer})
        Me.menuToolsNPersist.Text = "NPersist"
        '
        'menuToolsNPersistDomainExplorer
        '
        Me.menuToolsNPersistDomainExplorer.Index = 0
        Me.menuToolsNPersistDomainExplorer.Text = "Domain Explorer"
        '
        'menuToolsNPersistQueryAnalyzer
        '
        Me.menuToolsNPersistQueryAnalyzer.Index = 1
        Me.menuToolsNPersistQueryAnalyzer.Text = "Query Analyzer"
        '
        'MenuItem36
        '
        Me.MenuItem36.Index = 7
        Me.MenuItem36.Text = "-"
        '
        'menuToolsPlugins
        '
        Me.menuToolsPlugins.Index = 8
        Me.menuToolsPlugins.Text = "Plugins"
        '
        'menuToolsPluginsSeparator
        '
        Me.menuToolsPluginsSeparator.Index = 9
        Me.menuToolsPluginsSeparator.Text = "-"
        '
        'menuToolsOptions
        '
        Me.menuToolsOptions.Index = 10
        Me.menuToolsOptions.Text = "Options..."
        '
        'menuToolsSynch
        '
        Me.menuToolsSynch.Index = 4
        Me.menuToolsSynch.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsSynchDM, Me.menuToolsSynchCM, Me.menuToolsSynchTM})
        Me.menuToolsSynch.Text = "Synchronization"
        '
        'menuToolsSynchDM
        '
        Me.menuToolsSynchDM.Index = 0
        Me.menuToolsSynchDM.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsSynchDMToTbl, Me.menuToolsSynchDMToCls})
        Me.menuToolsSynchDM.Text = "Domain Model"
        '
        'menuToolsSynchDMToTbl
        '
        Me.menuToolsSynchDMToTbl.Index = 0
        Me.menuToolsSynchDMToTbl.Text = "From Classes To Tables"
        '
        'menuToolsSynchDMToCls
        '
        Me.menuToolsSynchDMToCls.Index = 1
        Me.menuToolsSynchDMToCls.Text = "From Tables To Classes"
        '
        'menuToolsSynchCM
        '
        Me.menuToolsSynchCM.Index = 1
        Me.menuToolsSynchCM.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsSynchCMToCode, Me.menuToolsSynchCMToModel})
        Me.menuToolsSynchCM.Text = "Class Model"
        '
        'menuToolsSynchCMToCode
        '
        Me.menuToolsSynchCMToCode.Index = 0
        Me.menuToolsSynchCMToCode.Text = "From Model To Code"
        '
        'menuToolsSynchCMToModel
        '
        Me.menuToolsSynchCMToModel.Index = 1
        Me.menuToolsSynchCMToModel.Text = "From Code To Model"
        '
        'menuToolsSynchTM
        '
        Me.menuToolsSynchTM.Index = 2
        Me.menuToolsSynchTM.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsSynchTMToSource, Me.menuToolsSynchTMToModel})
        Me.menuToolsSynchTM.Text = "Table Model"
        '
        'menuToolsSynchTMToSource
        '
        Me.menuToolsSynchTMToSource.Index = 0
        Me.menuToolsSynchTMToSource.Text = "From Model To Data Sources"
        '
        'menuToolsSynchTMToModel
        '
        Me.menuToolsSynchTMToModel.Index = 1
        Me.menuToolsSynchTMToModel.Text = "From Data Sources To Model"
        '
        'MenuItem71
        '
        Me.MenuItem71.Index = 5
        Me.MenuItem71.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem58, Me.MenuItem77, Me.MenuItem78, Me.MenuItem79, Me.MenuItem80, Me.MenuItem142, Me.MenuItem143})
        Me.MenuItem71.Text = "Preview"
        '
        'MenuItem58
        '
        Me.MenuItem58.Index = 0
        Me.MenuItem58.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsCodeCs, Me.menuToolsCodeVb, Me.menuToolsCodeDelphi})
        Me.MenuItem58.Text = "Source Code"
        '
        'menuToolsCodeCs
        '
        Me.menuToolsCodeCs.Index = 0
        Me.menuToolsCodeCs.Text = "C#"
        '
        'menuToolsCodeVb
        '
        Me.menuToolsCodeVb.Index = 1
        Me.menuToolsCodeVb.Text = "VB.NET"
        '
        'menuToolsCodeDelphi
        '
        Me.menuToolsCodeDelphi.Index = 2
        Me.menuToolsCodeDelphi.Text = "Delphi"
        '
        'MenuItem77
        '
        Me.MenuItem77.Index = 1
        Me.MenuItem77.Text = "-"
        '
        'MenuItem78
        '
        Me.MenuItem78.Index = 2
        Me.MenuItem78.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsCodeDDL})
        Me.MenuItem78.Text = "Database"
        '
        'menuToolsCodeDDL
        '
        Me.menuToolsCodeDDL.Index = 0
        Me.menuToolsCodeDDL.Text = "DDL"
        '
        'MenuItem79
        '
        Me.MenuItem79.Index = 3
        Me.MenuItem79.Text = "-"
        '
        'MenuItem80
        '
        Me.MenuItem80.Index = 4
        Me.MenuItem80.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsCodeXml})
        Me.MenuItem80.Text = "Mapping File"
        '
        'menuToolsCodeXml
        '
        Me.menuToolsCodeXml.Index = 0
        Me.menuToolsCodeXml.Text = "Xml"
        '
        'MenuItem142
        '
        Me.MenuItem142.Index = 5
        Me.MenuItem142.Text = "-"
        '
        'MenuItem143
        '
        Me.MenuItem143.Index = 6
        Me.MenuItem143.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuToolsCodeNhCs, Me.menuToolsCodeNhVb, Me.MenuItem148, Me.menuToolsCodeNhXml})
        Me.MenuItem143.Text = "NHibernate"
        '
        'menuToolsCodeNhCs
        '
        Me.menuToolsCodeNhCs.Index = 0
        Me.menuToolsCodeNhCs.Text = "C#"
        '
        'menuToolsCodeNhVb
        '
        Me.menuToolsCodeNhVb.Index = 1
        Me.menuToolsCodeNhVb.Text = "VB.Net"
        '
        'MenuItem148
        '
        Me.MenuItem148.Index = 2
        Me.MenuItem148.Text = "-"
        '
        'menuToolsCodeNhXml
        '
        Me.menuToolsCodeNhXml.Index = 3
        Me.menuToolsCodeNhXml.Text = "Xml"
        '
        'MenuItem43
        '
        Me.MenuItem43.Index = 6
        Me.MenuItem43.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem42})
        Me.MenuItem43.Text = "Help"
        '
        'MenuItem42
        '
        Me.MenuItem42.Index = 0
        Me.MenuItem42.Text = "About"
        '
        'panelMain
        '
        Me.panelMain.Controls.Add(Me.panelDocuments)
        Me.panelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelMain.Location = New System.Drawing.Point(322, 72)
        Me.panelMain.Name = "panelMain"
        Me.panelMain.Size = New System.Drawing.Size(210, 128)
        Me.panelMain.TabIndex = 7
        '
        'panelDocuments
        '
        Me.panelDocuments.Controls.Add(Me.tabControlDocuments)
        Me.panelDocuments.Controls.Add(Me.panelMainTitle)
        Me.panelDocuments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDocuments.Location = New System.Drawing.Point(0, 0)
        Me.panelDocuments.Name = "panelDocuments"
        Me.panelDocuments.Size = New System.Drawing.Size(210, 128)
        Me.panelDocuments.TabIndex = 2
        '
        'tabControlDocuments
        '
        Me.tabControlDocuments.Controls.Add(Me.tabPageMainUml)
        Me.tabControlDocuments.Controls.Add(Me.tabPageMainCustom)
        Me.tabControlDocuments.Controls.Add(Me.tabPageMainPreview)
        Me.tabControlDocuments.Controls.Add(Me.tabPageMainXmlBehind)
        Me.tabControlDocuments.Controls.Add(Me.tabPageMainCodeMap)
        Me.tabControlDocuments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlDocuments.ItemSize = New System.Drawing.Size(96, 18)
        Me.tabControlDocuments.Location = New System.Drawing.Point(0, 16)
        Me.tabControlDocuments.Multiline = True
        Me.tabControlDocuments.Name = "tabControlDocuments"
        Me.tabControlDocuments.SelectedIndex = 0
        Me.tabControlDocuments.Size = New System.Drawing.Size(210, 112)
        Me.tabControlDocuments.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.tabControlDocuments, resources.GetString("tabControlDocuments.ToolTip"))
        '
        'tabPageMainUml
        '
        Me.tabPageMainUml.Controls.Add(Me.panelTreeMap)
        Me.tabPageMainUml.Location = New System.Drawing.Point(4, 40)
        Me.tabPageMainUml.Name = "tabPageMainUml"
        Me.tabPageMainUml.Size = New System.Drawing.Size(202, 68)
        Me.tabPageMainUml.TabIndex = 0
        Me.tabPageMainUml.Text = "Uml"
        '
        'panelTreeMap
        '
        Me.panelTreeMap.Controls.Add(Me.tabControlUmlDoc)
        Me.panelTreeMap.Controls.Add(Me.panelUmlTitle)
        Me.panelTreeMap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelTreeMap.Location = New System.Drawing.Point(0, 0)
        Me.panelTreeMap.Name = "panelTreeMap"
        Me.panelTreeMap.Size = New System.Drawing.Size(202, 68)
        Me.panelTreeMap.TabIndex = 1
        '
        'tabControlUmlDoc
        '
        Me.tabControlUmlDoc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlUmlDoc.ItemSize = New System.Drawing.Size(96, 18)
        Me.tabControlUmlDoc.Location = New System.Drawing.Point(0, 16)
        Me.tabControlUmlDoc.Name = "tabControlUmlDoc"
        Me.tabControlUmlDoc.SelectedIndex = 0
        Me.tabControlUmlDoc.Size = New System.Drawing.Size(202, 52)
        Me.tabControlUmlDoc.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabControlUmlDoc.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.tabControlUmlDoc, "To create a UML document, mark your domain in the tree view and then press the re" & _
                "d UML button in the toolbar")
        '
        'panelUmlTitle
        '
        Me.panelUmlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelUmlTitle.Controls.Add(Me.labelUmlTitle)
        Me.panelUmlTitle.Controls.Add(Me.buttonPrevUmlDoc)
        Me.panelUmlTitle.Controls.Add(Me.buttonNextUmlDoc)
        Me.panelUmlTitle.Controls.Add(Me.buttonCloseUmlDoc)
        Me.panelUmlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelUmlTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelUmlTitle.Name = "panelUmlTitle"
        Me.panelUmlTitle.Size = New System.Drawing.Size(202, 16)
        Me.panelUmlTitle.TabIndex = 9
        '
        'labelUmlTitle
        '
        Me.labelUmlTitle.BackColor = System.Drawing.SystemColors.Info
        Me.labelUmlTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelUmlTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelUmlTitle.Name = "labelUmlTitle"
        Me.labelUmlTitle.Size = New System.Drawing.Size(152, 14)
        Me.labelUmlTitle.TabIndex = 0
        '
        'buttonPrevUmlDoc
        '
        Me.buttonPrevUmlDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonPrevUmlDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonPrevUmlDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonPrevUmlDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonPrevUmlDoc.ImageIndex = 2
        Me.buttonPrevUmlDoc.ImageList = Me.imageListSmallButtons
        Me.buttonPrevUmlDoc.Location = New System.Drawing.Point(152, 0)
        Me.buttonPrevUmlDoc.Name = "buttonPrevUmlDoc"
        Me.buttonPrevUmlDoc.Size = New System.Drawing.Size(16, 14)
        Me.buttonPrevUmlDoc.TabIndex = 3
        Me.buttonPrevUmlDoc.UseVisualStyleBackColor = False
        '
        'buttonNextUmlDoc
        '
        Me.buttonNextUmlDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonNextUmlDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonNextUmlDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonNextUmlDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonNextUmlDoc.ImageIndex = 4
        Me.buttonNextUmlDoc.ImageList = Me.imageListSmallButtons
        Me.buttonNextUmlDoc.Location = New System.Drawing.Point(168, 0)
        Me.buttonNextUmlDoc.Name = "buttonNextUmlDoc"
        Me.buttonNextUmlDoc.Size = New System.Drawing.Size(17, 14)
        Me.buttonNextUmlDoc.TabIndex = 2
        Me.buttonNextUmlDoc.UseVisualStyleBackColor = False
        '
        'buttonCloseUmlDoc
        '
        Me.buttonCloseUmlDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonCloseUmlDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseUmlDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseUmlDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonCloseUmlDoc.Image = CType(resources.GetObject("buttonCloseUmlDoc.Image"), System.Drawing.Image)
        Me.buttonCloseUmlDoc.Location = New System.Drawing.Point(185, 0)
        Me.buttonCloseUmlDoc.Name = "buttonCloseUmlDoc"
        Me.buttonCloseUmlDoc.Size = New System.Drawing.Size(15, 14)
        Me.buttonCloseUmlDoc.TabIndex = 1
        Me.buttonCloseUmlDoc.UseVisualStyleBackColor = False
        '
        'tabPageMainCustom
        '
        Me.tabPageMainCustom.Controls.Add(Me.tabControlUserDoc)
        Me.tabPageMainCustom.Controls.Add(Me.panelDocTitle)
        Me.tabPageMainCustom.Location = New System.Drawing.Point(4, 22)
        Me.tabPageMainCustom.Name = "tabPageMainCustom"
        Me.tabPageMainCustom.Size = New System.Drawing.Size(202, 106)
        Me.tabPageMainCustom.TabIndex = 1
        Me.tabPageMainCustom.Text = "Custom"
        '
        'tabControlUserDoc
        '
        Me.tabControlUserDoc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlUserDoc.ItemSize = New System.Drawing.Size(96, 18)
        Me.tabControlUserDoc.Location = New System.Drawing.Point(0, 16)
        Me.tabControlUserDoc.Name = "tabControlUserDoc"
        Me.tabControlUserDoc.SelectedIndex = 0
        Me.tabControlUserDoc.Size = New System.Drawing.Size(202, 90)
        Me.tabControlUserDoc.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabControlUserDoc.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.tabControlUserDoc, resources.GetString("tabControlUserDoc.ToolTip"))
        '
        'panelDocTitle
        '
        Me.panelDocTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelDocTitle.Controls.Add(Me.labelUserDocTitle)
        Me.panelDocTitle.Controls.Add(Me.buttonPrevUserDoc)
        Me.panelDocTitle.Controls.Add(Me.buttonNextUserDoc)
        Me.panelDocTitle.Controls.Add(Me.buttonCloseUserDoc)
        Me.panelDocTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelDocTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelDocTitle.Name = "panelDocTitle"
        Me.panelDocTitle.Size = New System.Drawing.Size(202, 16)
        Me.panelDocTitle.TabIndex = 2
        '
        'labelUserDocTitle
        '
        Me.labelUserDocTitle.BackColor = System.Drawing.SystemColors.Info
        Me.labelUserDocTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelUserDocTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelUserDocTitle.Name = "labelUserDocTitle"
        Me.labelUserDocTitle.Size = New System.Drawing.Size(152, 14)
        Me.labelUserDocTitle.TabIndex = 0
        '
        'buttonPrevUserDoc
        '
        Me.buttonPrevUserDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonPrevUserDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonPrevUserDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonPrevUserDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonPrevUserDoc.ImageIndex = 2
        Me.buttonPrevUserDoc.ImageList = Me.imageListSmallButtons
        Me.buttonPrevUserDoc.Location = New System.Drawing.Point(152, 0)
        Me.buttonPrevUserDoc.Name = "buttonPrevUserDoc"
        Me.buttonPrevUserDoc.Size = New System.Drawing.Size(16, 14)
        Me.buttonPrevUserDoc.TabIndex = 5
        Me.buttonPrevUserDoc.UseVisualStyleBackColor = False
        '
        'buttonNextUserDoc
        '
        Me.buttonNextUserDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonNextUserDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonNextUserDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonNextUserDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonNextUserDoc.ImageIndex = 4
        Me.buttonNextUserDoc.ImageList = Me.imageListSmallButtons
        Me.buttonNextUserDoc.Location = New System.Drawing.Point(168, 0)
        Me.buttonNextUserDoc.Name = "buttonNextUserDoc"
        Me.buttonNextUserDoc.Size = New System.Drawing.Size(17, 14)
        Me.buttonNextUserDoc.TabIndex = 4
        Me.buttonNextUserDoc.UseVisualStyleBackColor = False
        '
        'buttonCloseUserDoc
        '
        Me.buttonCloseUserDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonCloseUserDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseUserDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseUserDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonCloseUserDoc.Image = CType(resources.GetObject("buttonCloseUserDoc.Image"), System.Drawing.Image)
        Me.buttonCloseUserDoc.Location = New System.Drawing.Point(185, 0)
        Me.buttonCloseUserDoc.Name = "buttonCloseUserDoc"
        Me.buttonCloseUserDoc.Size = New System.Drawing.Size(15, 14)
        Me.buttonCloseUserDoc.TabIndex = 1
        Me.buttonCloseUserDoc.UseVisualStyleBackColor = False
        '
        'tabPageMainPreview
        '
        Me.tabPageMainPreview.Controls.Add(Me.tabControlPreviewDoc)
        Me.tabPageMainPreview.Controls.Add(Me.panelPreviewDocTitle)
        Me.tabPageMainPreview.Location = New System.Drawing.Point(4, 22)
        Me.tabPageMainPreview.Name = "tabPageMainPreview"
        Me.tabPageMainPreview.Size = New System.Drawing.Size(202, 106)
        Me.tabPageMainPreview.TabIndex = 2
        Me.tabPageMainPreview.Text = "Preview"
        '
        'tabControlPreviewDoc
        '
        Me.tabControlPreviewDoc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlPreviewDoc.ItemSize = New System.Drawing.Size(96, 18)
        Me.tabControlPreviewDoc.Location = New System.Drawing.Point(0, 16)
        Me.tabControlPreviewDoc.Name = "tabControlPreviewDoc"
        Me.tabControlPreviewDoc.SelectedIndex = 0
        Me.tabControlPreviewDoc.Size = New System.Drawing.Size(202, 90)
        Me.tabControlPreviewDoc.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabControlPreviewDoc.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.tabControlPreviewDoc, "You can preview the source code and xml that ObjectMapper would generate for you " & _
                "with the current settings by selecting an option from the Preview menu in the me" & _
                "nubar")
        '
        'panelPreviewDocTitle
        '
        Me.panelPreviewDocTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelPreviewDocTitle.Controls.Add(Me.labelPreviewDocTitle)
        Me.panelPreviewDocTitle.Controls.Add(Me.buttonPrevPreviewDoc)
        Me.panelPreviewDocTitle.Controls.Add(Me.buttonNextPreviewDoc)
        Me.panelPreviewDocTitle.Controls.Add(Me.buttonClosePreviewDoc)
        Me.panelPreviewDocTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelPreviewDocTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelPreviewDocTitle.Name = "panelPreviewDocTitle"
        Me.panelPreviewDocTitle.Size = New System.Drawing.Size(202, 16)
        Me.panelPreviewDocTitle.TabIndex = 4
        '
        'labelPreviewDocTitle
        '
        Me.labelPreviewDocTitle.BackColor = System.Drawing.SystemColors.Info
        Me.labelPreviewDocTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelPreviewDocTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelPreviewDocTitle.Name = "labelPreviewDocTitle"
        Me.labelPreviewDocTitle.Size = New System.Drawing.Size(152, 14)
        Me.labelPreviewDocTitle.TabIndex = 0
        '
        'buttonPrevPreviewDoc
        '
        Me.buttonPrevPreviewDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonPrevPreviewDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonPrevPreviewDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonPrevPreviewDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonPrevPreviewDoc.ImageIndex = 2
        Me.buttonPrevPreviewDoc.ImageList = Me.imageListSmallButtons
        Me.buttonPrevPreviewDoc.Location = New System.Drawing.Point(152, 0)
        Me.buttonPrevPreviewDoc.Name = "buttonPrevPreviewDoc"
        Me.buttonPrevPreviewDoc.Size = New System.Drawing.Size(16, 14)
        Me.buttonPrevPreviewDoc.TabIndex = 3
        Me.buttonPrevPreviewDoc.UseVisualStyleBackColor = False
        '
        'buttonNextPreviewDoc
        '
        Me.buttonNextPreviewDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonNextPreviewDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonNextPreviewDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonNextPreviewDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonNextPreviewDoc.ImageIndex = 4
        Me.buttonNextPreviewDoc.ImageList = Me.imageListSmallButtons
        Me.buttonNextPreviewDoc.Location = New System.Drawing.Point(168, 0)
        Me.buttonNextPreviewDoc.Name = "buttonNextPreviewDoc"
        Me.buttonNextPreviewDoc.Size = New System.Drawing.Size(17, 14)
        Me.buttonNextPreviewDoc.TabIndex = 2
        Me.buttonNextPreviewDoc.UseVisualStyleBackColor = False
        '
        'buttonClosePreviewDoc
        '
        Me.buttonClosePreviewDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonClosePreviewDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonClosePreviewDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonClosePreviewDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonClosePreviewDoc.Image = CType(resources.GetObject("buttonClosePreviewDoc.Image"), System.Drawing.Image)
        Me.buttonClosePreviewDoc.Location = New System.Drawing.Point(185, 0)
        Me.buttonClosePreviewDoc.Name = "buttonClosePreviewDoc"
        Me.buttonClosePreviewDoc.Size = New System.Drawing.Size(15, 14)
        Me.buttonClosePreviewDoc.TabIndex = 1
        Me.buttonClosePreviewDoc.UseVisualStyleBackColor = False
        '
        'tabPageMainXmlBehind
        '
        Me.tabPageMainXmlBehind.Controls.Add(Me.tabControlXmlBehind)
        Me.tabPageMainXmlBehind.Controls.Add(Me.panelXmlBehindTitle)
        Me.tabPageMainXmlBehind.Location = New System.Drawing.Point(4, 22)
        Me.tabPageMainXmlBehind.Name = "tabPageMainXmlBehind"
        Me.tabPageMainXmlBehind.Size = New System.Drawing.Size(202, 106)
        Me.tabPageMainXmlBehind.TabIndex = 3
        Me.tabPageMainXmlBehind.Text = "Xml Behind"
        '
        'tabControlXmlBehind
        '
        Me.tabControlXmlBehind.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlXmlBehind.ItemSize = New System.Drawing.Size(96, 18)
        Me.tabControlXmlBehind.Location = New System.Drawing.Point(0, 16)
        Me.tabControlXmlBehind.Name = "tabControlXmlBehind"
        Me.tabControlXmlBehind.SelectedIndex = 0
        Me.tabControlXmlBehind.Size = New System.Drawing.Size(202, 90)
        Me.tabControlXmlBehind.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabControlXmlBehind.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.tabControlXmlBehind, "To edit the XML behind for a domain model, mark your domain in the tree view and " & _
                "then press the blue XML button in the toolbar")
        '
        'panelXmlBehindTitle
        '
        Me.panelXmlBehindTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelXmlBehindTitle.Controls.Add(Me.labelXmlBehindTitle)
        Me.panelXmlBehindTitle.Controls.Add(Me.buttonApplyXmlBehind)
        Me.panelXmlBehindTitle.Controls.Add(Me.buttonDiscardXmlBehind)
        Me.panelXmlBehindTitle.Controls.Add(Me.buttonPrevXmlBehind)
        Me.panelXmlBehindTitle.Controls.Add(Me.buttonNextXmlBehind)
        Me.panelXmlBehindTitle.Controls.Add(Me.buttonCloseXmlBehind)
        Me.panelXmlBehindTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelXmlBehindTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelXmlBehindTitle.Name = "panelXmlBehindTitle"
        Me.panelXmlBehindTitle.Size = New System.Drawing.Size(202, 16)
        Me.panelXmlBehindTitle.TabIndex = 3
        '
        'labelXmlBehindTitle
        '
        Me.labelXmlBehindTitle.BackColor = System.Drawing.SystemColors.Info
        Me.labelXmlBehindTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelXmlBehindTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelXmlBehindTitle.Name = "labelXmlBehindTitle"
        Me.labelXmlBehindTitle.Size = New System.Drawing.Size(120, 14)
        Me.labelXmlBehindTitle.TabIndex = 0
        '
        'buttonApplyXmlBehind
        '
        Me.buttonApplyXmlBehind.BackColor = System.Drawing.SystemColors.Info
        Me.buttonApplyXmlBehind.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonApplyXmlBehind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonApplyXmlBehind.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonApplyXmlBehind.Image = CType(resources.GetObject("buttonApplyXmlBehind.Image"), System.Drawing.Image)
        Me.buttonApplyXmlBehind.Location = New System.Drawing.Point(120, 0)
        Me.buttonApplyXmlBehind.Name = "buttonApplyXmlBehind"
        Me.buttonApplyXmlBehind.Size = New System.Drawing.Size(16, 14)
        Me.buttonApplyXmlBehind.TabIndex = 3
        Me.buttonApplyXmlBehind.UseVisualStyleBackColor = False
        '
        'buttonDiscardXmlBehind
        '
        Me.buttonDiscardXmlBehind.BackColor = System.Drawing.SystemColors.Info
        Me.buttonDiscardXmlBehind.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonDiscardXmlBehind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonDiscardXmlBehind.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonDiscardXmlBehind.Image = CType(resources.GetObject("buttonDiscardXmlBehind.Image"), System.Drawing.Image)
        Me.buttonDiscardXmlBehind.Location = New System.Drawing.Point(136, 0)
        Me.buttonDiscardXmlBehind.Name = "buttonDiscardXmlBehind"
        Me.buttonDiscardXmlBehind.Size = New System.Drawing.Size(16, 14)
        Me.buttonDiscardXmlBehind.TabIndex = 2
        Me.buttonDiscardXmlBehind.UseVisualStyleBackColor = False
        '
        'buttonPrevXmlBehind
        '
        Me.buttonPrevXmlBehind.BackColor = System.Drawing.SystemColors.Info
        Me.buttonPrevXmlBehind.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonPrevXmlBehind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonPrevXmlBehind.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonPrevXmlBehind.ImageIndex = 2
        Me.buttonPrevXmlBehind.ImageList = Me.imageListSmallButtons
        Me.buttonPrevXmlBehind.Location = New System.Drawing.Point(152, 0)
        Me.buttonPrevXmlBehind.Name = "buttonPrevXmlBehind"
        Me.buttonPrevXmlBehind.Size = New System.Drawing.Size(16, 14)
        Me.buttonPrevXmlBehind.TabIndex = 5
        Me.buttonPrevXmlBehind.UseVisualStyleBackColor = False
        '
        'buttonNextXmlBehind
        '
        Me.buttonNextXmlBehind.BackColor = System.Drawing.SystemColors.Info
        Me.buttonNextXmlBehind.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonNextXmlBehind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonNextXmlBehind.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonNextXmlBehind.ImageIndex = 4
        Me.buttonNextXmlBehind.ImageList = Me.imageListSmallButtons
        Me.buttonNextXmlBehind.Location = New System.Drawing.Point(168, 0)
        Me.buttonNextXmlBehind.Name = "buttonNextXmlBehind"
        Me.buttonNextXmlBehind.Size = New System.Drawing.Size(17, 14)
        Me.buttonNextXmlBehind.TabIndex = 4
        Me.buttonNextXmlBehind.UseVisualStyleBackColor = False
        '
        'buttonCloseXmlBehind
        '
        Me.buttonCloseXmlBehind.BackColor = System.Drawing.SystemColors.Info
        Me.buttonCloseXmlBehind.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseXmlBehind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseXmlBehind.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonCloseXmlBehind.Image = CType(resources.GetObject("buttonCloseXmlBehind.Image"), System.Drawing.Image)
        Me.buttonCloseXmlBehind.Location = New System.Drawing.Point(185, 0)
        Me.buttonCloseXmlBehind.Name = "buttonCloseXmlBehind"
        Me.buttonCloseXmlBehind.Size = New System.Drawing.Size(15, 14)
        Me.buttonCloseXmlBehind.TabIndex = 1
        Me.buttonCloseXmlBehind.UseVisualStyleBackColor = False
        '
        'tabPageMainCodeMap
        '
        Me.tabPageMainCodeMap.Controls.Add(Me.tabControlCodeMapDoc)
        Me.tabPageMainCodeMap.Controls.Add(Me.Panel1)
        Me.tabPageMainCodeMap.Location = New System.Drawing.Point(4, 40)
        Me.tabPageMainCodeMap.Name = "tabPageMainCodeMap"
        Me.tabPageMainCodeMap.Size = New System.Drawing.Size(202, 88)
        Me.tabPageMainCodeMap.TabIndex = 4
        Me.tabPageMainCodeMap.Text = "Code"
        '
        'tabControlCodeMapDoc
        '
        Me.tabControlCodeMapDoc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlCodeMapDoc.ItemSize = New System.Drawing.Size(96, 18)
        Me.tabControlCodeMapDoc.Location = New System.Drawing.Point(0, 16)
        Me.tabControlCodeMapDoc.Name = "tabControlCodeMapDoc"
        Me.tabControlCodeMapDoc.SelectedIndex = 0
        Me.tabControlCodeMapDoc.Size = New System.Drawing.Size(202, 72)
        Me.tabControlCodeMapDoc.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabControlCodeMapDoc.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.tabControlCodeMapDoc, "You can preview the source code and xml that ObjectMapper would generate for you " & _
                "with the current settings by selecting an option from the Preview menu in the me" & _
                "nubar")
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.labelCodeMapDocTitle)
        Me.Panel1.Controls.Add(Me.buttonPrevCodeMapDoc)
        Me.Panel1.Controls.Add(Me.buttonNextCodeMapDoc)
        Me.Panel1.Controls.Add(Me.buttonCloseCodeMapDoc)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(202, 16)
        Me.Panel1.TabIndex = 6
        '
        'labelCodeMapDocTitle
        '
        Me.labelCodeMapDocTitle.BackColor = System.Drawing.SystemColors.Info
        Me.labelCodeMapDocTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.labelCodeMapDocTitle.Location = New System.Drawing.Point(0, 0)
        Me.labelCodeMapDocTitle.Name = "labelCodeMapDocTitle"
        Me.labelCodeMapDocTitle.Size = New System.Drawing.Size(152, 14)
        Me.labelCodeMapDocTitle.TabIndex = 0
        '
        'buttonPrevCodeMapDoc
        '
        Me.buttonPrevCodeMapDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonPrevCodeMapDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonPrevCodeMapDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonPrevCodeMapDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonPrevCodeMapDoc.ImageIndex = 2
        Me.buttonPrevCodeMapDoc.ImageList = Me.imageListSmallButtons
        Me.buttonPrevCodeMapDoc.Location = New System.Drawing.Point(152, 0)
        Me.buttonPrevCodeMapDoc.Name = "buttonPrevCodeMapDoc"
        Me.buttonPrevCodeMapDoc.Size = New System.Drawing.Size(16, 14)
        Me.buttonPrevCodeMapDoc.TabIndex = 3
        Me.buttonPrevCodeMapDoc.UseVisualStyleBackColor = False
        '
        'buttonNextCodeMapDoc
        '
        Me.buttonNextCodeMapDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonNextCodeMapDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonNextCodeMapDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonNextCodeMapDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonNextCodeMapDoc.ImageIndex = 4
        Me.buttonNextCodeMapDoc.ImageList = Me.imageListSmallButtons
        Me.buttonNextCodeMapDoc.Location = New System.Drawing.Point(168, 0)
        Me.buttonNextCodeMapDoc.Name = "buttonNextCodeMapDoc"
        Me.buttonNextCodeMapDoc.Size = New System.Drawing.Size(17, 14)
        Me.buttonNextCodeMapDoc.TabIndex = 2
        Me.buttonNextCodeMapDoc.UseVisualStyleBackColor = False
        '
        'buttonCloseCodeMapDoc
        '
        Me.buttonCloseCodeMapDoc.BackColor = System.Drawing.SystemColors.Info
        Me.buttonCloseCodeMapDoc.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseCodeMapDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseCodeMapDoc.ForeColor = System.Drawing.SystemColors.Info
        Me.buttonCloseCodeMapDoc.Image = CType(resources.GetObject("buttonCloseCodeMapDoc.Image"), System.Drawing.Image)
        Me.buttonCloseCodeMapDoc.Location = New System.Drawing.Point(185, 0)
        Me.buttonCloseCodeMapDoc.Name = "buttonCloseCodeMapDoc"
        Me.buttonCloseCodeMapDoc.Size = New System.Drawing.Size(15, 14)
        Me.buttonCloseCodeMapDoc.TabIndex = 1
        Me.buttonCloseCodeMapDoc.UseVisualStyleBackColor = False
        '
        'panelMainTitle
        '
        Me.panelMainTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelMainTitle.Controls.Add(Me.Label5)
        Me.panelMainTitle.Controls.Add(Me.buttonSwapPosMain)
        Me.panelMainTitle.Controls.Add(Me.buttonCloseMain)
        Me.panelMainTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelMainTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelMainTitle.Name = "panelMainTitle"
        Me.panelMainTitle.Size = New System.Drawing.Size(210, 16)
        Me.panelMainTitle.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(176, 14)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Documents"
        '
        'buttonSwapPosMain
        '
        Me.buttonSwapPosMain.BackColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosMain.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonSwapPosMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonSwapPosMain.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosMain.ImageIndex = 4
        Me.buttonSwapPosMain.ImageList = Me.imageListSmallButtons
        Me.buttonSwapPosMain.Location = New System.Drawing.Point(176, 0)
        Me.buttonSwapPosMain.Name = "buttonSwapPosMain"
        Me.buttonSwapPosMain.Size = New System.Drawing.Size(16, 14)
        Me.buttonSwapPosMain.TabIndex = 4
        Me.buttonSwapPosMain.UseVisualStyleBackColor = False
        '
        'buttonCloseMain
        '
        Me.buttonCloseMain.BackColor = System.Drawing.SystemColors.Control
        Me.buttonCloseMain.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseMain.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonCloseMain.ImageIndex = 0
        Me.buttonCloseMain.ImageList = Me.imageListSmallButtons
        Me.buttonCloseMain.Location = New System.Drawing.Point(192, 0)
        Me.buttonCloseMain.Name = "buttonCloseMain"
        Me.buttonCloseMain.Size = New System.Drawing.Size(16, 14)
        Me.buttonCloseMain.TabIndex = 3
        Me.buttonCloseMain.UseVisualStyleBackColor = False
        '
        'Splitter4
        '
        Me.Splitter4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Splitter4.Location = New System.Drawing.Point(163, 200)
        Me.Splitter4.Name = "Splitter4"
        Me.Splitter4.Size = New System.Drawing.Size(516, 3)
        Me.Splitter4.TabIndex = 8
        Me.Splitter4.TabStop = False
        '
        'Splitter5
        '
        Me.Splitter5.Dock = System.Windows.Forms.DockStyle.Right
        Me.Splitter5.Location = New System.Drawing.Point(532, 72)
        Me.Splitter5.Name = "Splitter5"
        Me.Splitter5.Size = New System.Drawing.Size(3, 128)
        Me.Splitter5.TabIndex = 9
        Me.Splitter5.TabStop = False
        '
        'menuDomain
        '
        Me.menuDomain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainAdd, Me.MenuItem8, Me.menuDomainCopy, Me.menuDomainPaste, Me.menuDomainRemove, Me.menuDomainRename, Me.MenuItem11, Me.menuDomainSave, Me.menuDomainSaveAs, Me.MenuItem19, Me.menuDomainTestInDomainExplorer, Me.MenuItem145, Me.MenuItem144, Me.MenuItem64, Me.MenuItem39, Me.MenuItem100, Me.MenuItem20, Me.MenuItem38, Me.menuDomainXml, Me.MenuItem90, Me.MenuItem82, Me.MenuItem81, Me.MenuItem134, Me.menuDomainPluginsSeparator, Me.menuDomainPlugins, Me.MenuItem102, Me.menuDomainProperties})
        '
        'menuDomainAdd
        '
        Me.menuDomainAdd.Index = 0
        Me.menuDomainAdd.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainAddClass, Me.menuDomainAddSource})
        Me.menuDomainAdd.Text = "Add"
        '
        'menuDomainAddClass
        '
        Me.menuDomainAddClass.Index = 0
        Me.menuDomainAddClass.Text = "Class"
        '
        'menuDomainAddSource
        '
        Me.menuDomainAddSource.Index = 1
        Me.menuDomainAddSource.Text = "Data Source"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 1
        Me.MenuItem8.Text = "-"
        '
        'menuDomainCopy
        '
        Me.menuDomainCopy.Index = 2
        Me.menuDomainCopy.Text = "Copy"
        '
        'menuDomainPaste
        '
        Me.menuDomainPaste.Index = 3
        Me.menuDomainPaste.Text = "Paste"
        '
        'menuDomainRemove
        '
        Me.menuDomainRemove.Index = 4
        Me.menuDomainRemove.Text = "Remove"
        '
        'menuDomainRename
        '
        Me.menuDomainRename.Index = 5
        Me.menuDomainRename.Text = "Rename"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 6
        Me.MenuItem11.Text = "-"
        '
        'menuDomainSave
        '
        Me.menuDomainSave.Index = 7
        Me.menuDomainSave.Text = "Save"
        '
        'menuDomainSaveAs
        '
        Me.menuDomainSaveAs.Index = 8
        Me.menuDomainSaveAs.Text = "Save As..."
        '
        'MenuItem19
        '
        Me.MenuItem19.Index = 9
        Me.MenuItem19.Text = "-"
        '
        'menuDomainTestInDomainExplorer
        '
        Me.menuDomainTestInDomainExplorer.Index = 10
        Me.menuDomainTestInDomainExplorer.Text = "Run"
        '
        'MenuItem145
        '
        Me.MenuItem145.Index = 11
        Me.MenuItem145.Text = "-"
        '
        'MenuItem144
        '
        Me.MenuItem144.Index = 12
        Me.MenuItem144.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainCodeMapCs, Me.menuDomainCodeMapVb, Me.menuDomainCodeMapDelphi})
        Me.MenuItem144.Text = "Custom Code"
        '
        'menuDomainCodeMapCs
        '
        Me.menuDomainCodeMapCs.Index = 0
        Me.menuDomainCodeMapCs.Text = "C#"
        '
        'menuDomainCodeMapVb
        '
        Me.menuDomainCodeMapVb.Index = 1
        Me.menuDomainCodeMapVb.Text = "VB.Net"
        '
        'menuDomainCodeMapDelphi
        '
        Me.menuDomainCodeMapDelphi.Index = 2
        Me.menuDomainCodeMapDelphi.Text = "Delphi"
        '
        'MenuItem64
        '
        Me.MenuItem64.Index = 13
        Me.MenuItem64.Text = "-"
        '
        'MenuItem39
        '
        Me.MenuItem39.Index = 14
        Me.MenuItem39.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem46, Me.MenuItem47, Me.MenuItem48})
        Me.MenuItem39.Text = "Synchronize"
        '
        'MenuItem46
        '
        Me.MenuItem46.Index = 0
        Me.MenuItem46.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem49, Me.MenuItem50})
        Me.MenuItem46.Text = "Domain Model"
        '
        'MenuItem49
        '
        Me.MenuItem49.Index = 0
        Me.MenuItem49.Text = "From Classes To Tables"
        '
        'MenuItem50
        '
        Me.MenuItem50.Index = 1
        Me.MenuItem50.Text = "From Tables To Classes"
        '
        'MenuItem47
        '
        Me.MenuItem47.Index = 1
        Me.MenuItem47.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem51, Me.MenuItem52})
        Me.MenuItem47.Text = "Class Model"
        '
        'MenuItem51
        '
        Me.MenuItem51.Index = 0
        Me.MenuItem51.Text = "From Model To Code"
        '
        'MenuItem52
        '
        Me.MenuItem52.Index = 1
        Me.MenuItem52.Text = "From Code To Model"
        '
        'MenuItem48
        '
        Me.MenuItem48.Index = 2
        Me.MenuItem48.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem53, Me.MenuItem54})
        Me.MenuItem48.Text = "Table Model"
        '
        'MenuItem53
        '
        Me.MenuItem53.Index = 0
        Me.MenuItem53.Text = "From Model To Data Sources"
        '
        'MenuItem54
        '
        Me.MenuItem54.Index = 1
        Me.MenuItem54.Text = "From Data Sources To Model"
        '
        'MenuItem100
        '
        Me.MenuItem100.Index = 15
        Me.MenuItem100.Text = "-"
        '
        'MenuItem20
        '
        Me.MenuItem20.Index = 16
        Me.MenuItem20.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem73, Me.MenuItem57, Me.MenuItem74, Me.MenuItem75, Me.MenuItem76, Me.MenuItem125, Me.menuDomainCodeNH})
        Me.MenuItem20.Text = "Preview"
        '
        'MenuItem73
        '
        Me.MenuItem73.Index = 0
        Me.MenuItem73.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainCodeCs, Me.menuDomainCodeVb, Me.menuDomainCodeDelphi})
        Me.MenuItem73.Text = "Source Code"
        '
        'menuDomainCodeCs
        '
        Me.menuDomainCodeCs.Index = 0
        Me.menuDomainCodeCs.Text = "C#"
        '
        'menuDomainCodeVb
        '
        Me.menuDomainCodeVb.Index = 1
        Me.menuDomainCodeVb.Text = "VB.Net"
        '
        'menuDomainCodeDelphi
        '
        Me.menuDomainCodeDelphi.Index = 2
        Me.menuDomainCodeDelphi.Text = "Delphi"
        '
        'MenuItem57
        '
        Me.MenuItem57.Index = 1
        Me.MenuItem57.Text = "-"
        '
        'MenuItem74
        '
        Me.MenuItem74.Index = 2
        Me.MenuItem74.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainCodeDTD})
        Me.MenuItem74.Text = "Database"
        '
        'menuDomainCodeDTD
        '
        Me.menuDomainCodeDTD.Index = 0
        Me.menuDomainCodeDTD.Text = "DDL"
        '
        'MenuItem75
        '
        Me.MenuItem75.Index = 3
        Me.MenuItem75.Text = "-"
        '
        'MenuItem76
        '
        Me.MenuItem76.Index = 4
        Me.MenuItem76.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainCodeXml})
        Me.MenuItem76.Text = "Mapping File"
        '
        'menuDomainCodeXml
        '
        Me.menuDomainCodeXml.Index = 0
        Me.menuDomainCodeXml.Text = "Xml"
        '
        'MenuItem125
        '
        Me.MenuItem125.Index = 5
        Me.MenuItem125.Text = "-"
        '
        'menuDomainCodeNH
        '
        Me.menuDomainCodeNH.Index = 6
        Me.menuDomainCodeNH.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainCodeNHCs, Me.menuDomainCodeNHVb, Me.menuDomainCodeNHDelphi, Me.MenuItem135, Me.menuDomainCodeNHXml})
        Me.menuDomainCodeNH.Text = "NHibernate"
        '
        'menuDomainCodeNHCs
        '
        Me.menuDomainCodeNHCs.Index = 0
        Me.menuDomainCodeNHCs.Text = "C#"
        '
        'menuDomainCodeNHVb
        '
        Me.menuDomainCodeNHVb.Index = 1
        Me.menuDomainCodeNHVb.Text = "VB.Net"
        '
        'menuDomainCodeNHDelphi
        '
        Me.menuDomainCodeNHDelphi.Index = 2
        Me.menuDomainCodeNHDelphi.Text = "Delphi"
        '
        'MenuItem135
        '
        Me.MenuItem135.Index = 3
        Me.MenuItem135.Text = "-"
        '
        'menuDomainCodeNHXml
        '
        Me.menuDomainCodeNHXml.Index = 4
        Me.menuDomainCodeNHXml.Text = "Xml"
        '
        'MenuItem38
        '
        Me.MenuItem38.Index = 17
        Me.MenuItem38.Text = "-"
        '
        'menuDomainXml
        '
        Me.menuDomainXml.Index = 18
        Me.menuDomainXml.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainViewXML, Me.menuDomainXmlClose, Me.MenuItem101, Me.menuDomainXmlApply, Me.menuDomainXmlDiscard})
        Me.menuDomainXml.Text = "Xml Behind"
        '
        'menuDomainViewXML
        '
        Me.menuDomainViewXML.Index = 0
        Me.menuDomainViewXML.Text = "Open"
        '
        'menuDomainXmlClose
        '
        Me.menuDomainXmlClose.Index = 1
        Me.menuDomainXmlClose.Text = "Close"
        '
        'MenuItem101
        '
        Me.MenuItem101.Index = 2
        Me.MenuItem101.Text = "-"
        '
        'menuDomainXmlApply
        '
        Me.menuDomainXmlApply.Index = 3
        Me.menuDomainXmlApply.Text = "Apply Changes"
        '
        'menuDomainXmlDiscard
        '
        Me.menuDomainXmlDiscard.Index = 4
        Me.menuDomainXmlDiscard.Text = "Discard Changes"
        '
        'MenuItem90
        '
        Me.MenuItem90.Index = 19
        Me.MenuItem90.Text = "-"
        '
        'MenuItem82
        '
        Me.MenuItem82.Index = 20
        Me.MenuItem82.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainCleanupAllCode, Me.menuDomainCleanupAllOrMappings, Me.menuDomainCleanupAllClasses, Me.menuDomainCleanupAllTables})
        Me.MenuItem82.Text = "Clean Up"
        '
        'menuDomainCleanupAllCode
        '
        Me.menuDomainCleanupAllCode.Index = 0
        Me.menuDomainCleanupAllCode.Text = "Remove All Custom Code"
        '
        'menuDomainCleanupAllOrMappings
        '
        Me.menuDomainCleanupAllOrMappings.Index = 1
        Me.menuDomainCleanupAllOrMappings.Text = "Remove All O/R Mappings"
        '
        'menuDomainCleanupAllClasses
        '
        Me.menuDomainCleanupAllClasses.Index = 2
        Me.menuDomainCleanupAllClasses.Text = "Remove All Classes"
        '
        'menuDomainCleanupAllTables
        '
        Me.menuDomainCleanupAllTables.Index = 3
        Me.menuDomainCleanupAllTables.Text = "Remove All Tables"
        '
        'MenuItem81
        '
        Me.MenuItem81.Index = 21
        Me.MenuItem81.Text = "-"
        '
        'MenuItem134
        '
        Me.MenuItem134.Index = 22
        Me.MenuItem134.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDomainExportNh})
        Me.MenuItem134.Text = "Export"
        '
        'menuDomainExportNh
        '
        Me.menuDomainExportNh.Index = 0
        Me.menuDomainExportNh.Text = "NHibernate"
        '
        'menuDomainPluginsSeparator
        '
        Me.menuDomainPluginsSeparator.Index = 23
        Me.menuDomainPluginsSeparator.Text = "-"
        '
        'menuDomainPlugins
        '
        Me.menuDomainPlugins.Index = 24
        Me.menuDomainPlugins.Text = "Plugins"
        '
        'MenuItem102
        '
        Me.MenuItem102.Index = 25
        Me.MenuItem102.Text = "-"
        '
        'menuDomainProperties
        '
        Me.menuDomainProperties.Index = 26
        Me.menuDomainProperties.Text = "Properties"
        '
        'menuClassList
        '
        Me.menuClassList.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem147, Me.MenuItem40, Me.menuClassListPaste, Me.MenuItem21, Me.MenuItem22, Me.MenuItem33, Me.MenuItem35})
        '
        'MenuItem147
        '
        Me.MenuItem147.Index = 0
        Me.MenuItem147.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassListAddClass, Me.menuClassListAddInterface, Me.menuClassListAddStruct, Me.menuClassListAddEnum})
        Me.MenuItem147.Text = "Add"
        '
        'menuClassListAddClass
        '
        Me.menuClassListAddClass.Index = 0
        Me.menuClassListAddClass.Text = "Class"
        '
        'menuClassListAddInterface
        '
        Me.menuClassListAddInterface.Index = 1
        Me.menuClassListAddInterface.Text = "Interface"
        '
        'menuClassListAddStruct
        '
        Me.menuClassListAddStruct.Index = 2
        Me.menuClassListAddStruct.Text = "Struct"
        '
        'menuClassListAddEnum
        '
        Me.menuClassListAddEnum.Index = 3
        Me.menuClassListAddEnum.Text = "Enumeration"
        '
        'MenuItem40
        '
        Me.MenuItem40.Index = 1
        Me.MenuItem40.Text = "-"
        '
        'menuClassListPaste
        '
        Me.menuClassListPaste.Index = 2
        Me.menuClassListPaste.Text = "Paste"
        '
        'MenuItem21
        '
        Me.MenuItem21.Index = 3
        Me.MenuItem21.Text = "-"
        '
        'MenuItem22
        '
        Me.MenuItem22.Index = 4
        Me.MenuItem22.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem83, Me.MenuItem103, Me.MenuItem137})
        Me.MenuItem22.Text = "Preview"
        '
        'MenuItem83
        '
        Me.MenuItem83.Index = 0
        Me.MenuItem83.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassListCodeCs, Me.menuClassListCodeVb, Me.menuClassListCodeDelphi})
        Me.MenuItem83.Text = "Source Code"
        '
        'menuClassListCodeCs
        '
        Me.menuClassListCodeCs.Index = 0
        Me.menuClassListCodeCs.Text = "C#"
        '
        'menuClassListCodeVb
        '
        Me.menuClassListCodeVb.Index = 1
        Me.menuClassListCodeVb.Text = "VB.Net"
        '
        'menuClassListCodeDelphi
        '
        Me.menuClassListCodeDelphi.Index = 2
        Me.menuClassListCodeDelphi.Text = "Delphi"
        '
        'MenuItem103
        '
        Me.MenuItem103.Index = 1
        Me.MenuItem103.Text = "-"
        '
        'MenuItem137
        '
        Me.MenuItem137.Index = 2
        Me.MenuItem137.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassListCodeNhCs, Me.menuClassListCodeNhVb, Me.menuClassListCodeNhDelphi, Me.MenuItem146, Me.menuClassListCodeNhXml})
        Me.MenuItem137.Text = "NHibernate"
        '
        'menuClassListCodeNhCs
        '
        Me.menuClassListCodeNhCs.Index = 0
        Me.menuClassListCodeNhCs.Text = "C#"
        '
        'menuClassListCodeNhVb
        '
        Me.menuClassListCodeNhVb.Index = 1
        Me.menuClassListCodeNhVb.Text = "VB.Net"
        '
        'menuClassListCodeNhDelphi
        '
        Me.menuClassListCodeNhDelphi.Index = 2
        Me.menuClassListCodeNhDelphi.Text = "Delphi"
        '
        'MenuItem146
        '
        Me.MenuItem146.Index = 3
        Me.MenuItem146.Text = "-"
        '
        'menuClassListCodeNhXml
        '
        Me.menuClassListCodeNhXml.Index = 4
        Me.menuClassListCodeNhXml.Text = "Xml"
        '
        'MenuItem33
        '
        Me.MenuItem33.Index = 5
        Me.MenuItem33.Text = "-"
        '
        'MenuItem35
        '
        Me.MenuItem35.Index = 6
        Me.MenuItem35.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem2})
        Me.MenuItem35.Text = "Synchronize"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassListSynchDMToTbl, Me.menuClassListSynchDMToCls})
        Me.MenuItem1.Text = "Domain Model"
        '
        'menuClassListSynchDMToTbl
        '
        Me.menuClassListSynchDMToTbl.Index = 0
        Me.menuClassListSynchDMToTbl.Text = "From Classes To Tables"
        '
        'menuClassListSynchDMToCls
        '
        Me.menuClassListSynchDMToCls.Index = 1
        Me.menuClassListSynchDMToCls.Text = "From Tables To Classes"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassListSynchCMToCode, Me.menuClassListSynchCMToModel})
        Me.MenuItem2.Text = "Class Model"
        '
        'menuClassListSynchCMToCode
        '
        Me.menuClassListSynchCMToCode.Index = 0
        Me.menuClassListSynchCMToCode.Text = "From Model To Code"
        '
        'menuClassListSynchCMToModel
        '
        Me.menuClassListSynchCMToModel.Index = 1
        Me.menuClassListSynchCMToModel.Text = "From Code To Model"
        '
        'menuNamespace
        '
        Me.menuNamespace.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuNamespaceAddClass, Me.MenuItem13, Me.menuNamespacePaste, Me.menuNamespaceDelete, Me.menuNamespaceRename, Me.MenuItem17, Me.MenuItem18})
        '
        'menuNamespaceAddClass
        '
        Me.menuNamespaceAddClass.Index = 0
        Me.menuNamespaceAddClass.Text = "Add Class"
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 1
        Me.MenuItem13.Text = "-"
        '
        'menuNamespacePaste
        '
        Me.menuNamespacePaste.Index = 2
        Me.menuNamespacePaste.Text = "Paste"
        '
        'menuNamespaceDelete
        '
        Me.menuNamespaceDelete.Index = 3
        Me.menuNamespaceDelete.Text = "Delete"
        '
        'menuNamespaceRename
        '
        Me.menuNamespaceRename.Index = 4
        Me.menuNamespaceRename.Text = "Rename"
        '
        'MenuItem17
        '
        Me.MenuItem17.Index = 5
        Me.MenuItem17.Text = "-"
        '
        'MenuItem18
        '
        Me.MenuItem18.Index = 6
        Me.MenuItem18.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem84})
        Me.MenuItem18.Text = "Preview"
        '
        'MenuItem84
        '
        Me.MenuItem84.Index = 0
        Me.MenuItem84.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuNamespaceCodeCs, Me.menuNamespaceCodeVb, Me.menuNamespaceCodeDelphi})
        Me.MenuItem84.Text = "Source Code"
        '
        'menuNamespaceCodeCs
        '
        Me.menuNamespaceCodeCs.Index = 0
        Me.menuNamespaceCodeCs.Text = "C#"
        '
        'menuNamespaceCodeVb
        '
        Me.menuNamespaceCodeVb.Index = 1
        Me.menuNamespaceCodeVb.Text = "VB.Net"
        '
        'menuNamespaceCodeDelphi
        '
        Me.menuNamespaceCodeDelphi.Index = 2
        Me.menuNamespaceCodeDelphi.Text = "Delphi"
        '
        'menuClass
        '
        Me.menuClass.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassAddProperty, Me.menuClassAddEnumValue, Me.MenuItem9, Me.menuClassCut, Me.menuClassCopy, Me.menuClassPaste, Me.menuClassDelete, Me.menuClassRename, Me.MenuItem133, Me.MenuItem63, Me.MenuItem97, Me.menuClassTransform, Me.MenuItem15, Me.menuClassUml, Me.MenuItem116, Me.MenuItem16, Me.menuClassPluginsSeparator, Me.menuClassPlugins, Me.MenuItem104, Me.menuClassProperties})
        '
        'menuClassAddProperty
        '
        Me.menuClassAddProperty.Index = 0
        Me.menuClassAddProperty.Text = "Add Property"
        '
        'menuClassAddEnumValue
        '
        Me.menuClassAddEnumValue.Index = 1
        Me.menuClassAddEnumValue.Text = "Add Enum Value"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 2
        Me.MenuItem9.Text = "-"
        '
        'menuClassCut
        '
        Me.menuClassCut.Index = 3
        Me.menuClassCut.Text = "Cut"
        '
        'menuClassCopy
        '
        Me.menuClassCopy.Index = 4
        Me.menuClassCopy.Text = "Copy"
        '
        'menuClassPaste
        '
        Me.menuClassPaste.Index = 5
        Me.menuClassPaste.Text = "Paste"
        '
        'menuClassDelete
        '
        Me.menuClassDelete.Index = 6
        Me.menuClassDelete.Text = "Delete"
        '
        'menuClassRename
        '
        Me.menuClassRename.Index = 7
        Me.menuClassRename.Text = "Rename"
        '
        'MenuItem133
        '
        Me.MenuItem133.Index = 8
        Me.MenuItem133.Text = "-"
        '
        'MenuItem63
        '
        Me.MenuItem63.Index = 9
        Me.MenuItem63.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassCodeMapCs, Me.menuClassCodeMapVb, Me.menuClassCodeMapDelphi})
        Me.MenuItem63.Text = "Custom Code"
        '
        'menuClassCodeMapCs
        '
        Me.menuClassCodeMapCs.Index = 0
        Me.menuClassCodeMapCs.Text = "C#"
        '
        'menuClassCodeMapVb
        '
        Me.menuClassCodeMapVb.Index = 1
        Me.menuClassCodeMapVb.Text = "VB.Net"
        '
        'menuClassCodeMapDelphi
        '
        Me.menuClassCodeMapDelphi.Index = 2
        Me.menuClassCodeMapDelphi.Text = "Delphi"
        '
        'MenuItem97
        '
        Me.MenuItem97.Index = 10
        Me.MenuItem97.Text = "-"
        '
        'menuClassTransform
        '
        Me.menuClassTransform.Index = 11
        Me.menuClassTransform.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassShadow})
        Me.menuClassTransform.Text = "Refactor"
        '
        'menuClassShadow
        '
        Me.menuClassShadow.Index = 0
        Me.menuClassShadow.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassAddShadow, Me.menuClassRemoveShadow})
        Me.menuClassShadow.Text = "Shadow Properties"
        '
        'menuClassAddShadow
        '
        Me.menuClassAddShadow.Index = 0
        Me.menuClassAddShadow.Text = "Add"
        '
        'menuClassRemoveShadow
        '
        Me.menuClassRemoveShadow.Index = 1
        Me.menuClassRemoveShadow.Text = "Remove"
        '
        'MenuItem15
        '
        Me.MenuItem15.Index = 12
        Me.MenuItem15.Text = "-"
        '
        'menuClassUml
        '
        Me.menuClassUml.Index = 13
        Me.menuClassUml.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassUmlAddToCurr, Me.menuClassUmlAddToNew, Me.menuClassUmlRemove, Me.menuClassUmlAddSeparator, Me.menuClassUmlShowAssocLines, Me.menuClassUmlRemoveAllLines})
        Me.menuClassUml.Text = "Uml"
        '
        'menuClassUmlAddToCurr
        '
        Me.menuClassUmlAddToCurr.Index = 0
        Me.menuClassUmlAddToCurr.Text = "Add To Diagram"
        '
        'menuClassUmlAddToNew
        '
        Me.menuClassUmlAddToNew.Index = 1
        Me.menuClassUmlAddToNew.Text = "Add To New Diagram"
        '
        'menuClassUmlRemove
        '
        Me.menuClassUmlRemove.Index = 2
        Me.menuClassUmlRemove.Text = "Remove From Diagram"
        '
        'menuClassUmlAddSeparator
        '
        Me.menuClassUmlAddSeparator.Index = 3
        Me.menuClassUmlAddSeparator.Text = "-"
        '
        'menuClassUmlShowAssocLines
        '
        Me.menuClassUmlShowAssocLines.Index = 4
        Me.menuClassUmlShowAssocLines.Text = "Add Lines To Diagram"
        '
        'menuClassUmlRemoveAllLines
        '
        Me.menuClassUmlRemoveAllLines.Index = 5
        Me.menuClassUmlRemoveAllLines.Text = "Remove Lines From Diagram"
        '
        'MenuItem116
        '
        Me.MenuItem116.Index = 14
        Me.MenuItem116.Text = "-"
        '
        'MenuItem16
        '
        Me.MenuItem16.Index = 15
        Me.MenuItem16.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem85, Me.MenuItem72, Me.MenuItem132})
        Me.MenuItem16.Text = "Preview"
        '
        'MenuItem85
        '
        Me.MenuItem85.Index = 0
        Me.MenuItem85.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassCodeCs, Me.menuClassCodeVb, Me.menuClassCodeDelphi})
        Me.MenuItem85.Text = "Source Code"
        '
        'menuClassCodeCs
        '
        Me.menuClassCodeCs.Index = 0
        Me.menuClassCodeCs.Text = "C#"
        '
        'menuClassCodeVb
        '
        Me.menuClassCodeVb.Index = 1
        Me.menuClassCodeVb.Text = "VB.Net"
        '
        'menuClassCodeDelphi
        '
        Me.menuClassCodeDelphi.Index = 2
        Me.menuClassCodeDelphi.Text = "Delphi"
        '
        'MenuItem72
        '
        Me.MenuItem72.Index = 1
        Me.MenuItem72.Text = "-"
        '
        'MenuItem132
        '
        Me.MenuItem132.Index = 2
        Me.MenuItem132.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClassCodeNhCs, Me.menuClassCodeNhVb, Me.menuClassCodeNhDelphi, Me.MenuItem136, Me.menuClassCodeNhXml})
        Me.MenuItem132.Text = "NHibernate"
        '
        'menuClassCodeNhCs
        '
        Me.menuClassCodeNhCs.Index = 0
        Me.menuClassCodeNhCs.Text = "C#"
        '
        'menuClassCodeNhVb
        '
        Me.menuClassCodeNhVb.Index = 1
        Me.menuClassCodeNhVb.Text = "VB.NET"
        '
        'menuClassCodeNhDelphi
        '
        Me.menuClassCodeNhDelphi.Index = 2
        Me.menuClassCodeNhDelphi.Text = "Delphi"
        '
        'MenuItem136
        '
        Me.MenuItem136.Index = 3
        Me.MenuItem136.Text = "-"
        '
        'menuClassCodeNhXml
        '
        Me.menuClassCodeNhXml.Index = 4
        Me.menuClassCodeNhXml.Text = "Xml"
        '
        'menuClassPluginsSeparator
        '
        Me.menuClassPluginsSeparator.Index = 16
        Me.menuClassPluginsSeparator.Text = "-"
        '
        'menuClassPlugins
        '
        Me.menuClassPlugins.Index = 17
        Me.menuClassPlugins.Text = "Plugins"
        '
        'MenuItem104
        '
        Me.MenuItem104.Index = 18
        Me.MenuItem104.Text = "-"
        '
        'menuClassProperties
        '
        Me.menuClassProperties.Index = 19
        Me.menuClassProperties.Text = "Properties"
        '
        'menuProperty
        '
        Me.menuProperty.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuPropertyCut, Me.menuPropertyCopy, Me.menuPropertyDelete, Me.menuPropertyRename, Me.MenuItem99, Me.menuPropertyTransform, Me.MenuItem12, Me.menuPropertyUml, Me.MenuItem118, Me.MenuItem14, Me.menuPropertyPluginsSeparator, Me.menuPropertyPlugins, Me.MenuItem106, Me.menuPropertyProperties})
        '
        'menuPropertyCut
        '
        Me.menuPropertyCut.Index = 0
        Me.menuPropertyCut.Text = "Cut"
        '
        'menuPropertyCopy
        '
        Me.menuPropertyCopy.Index = 1
        Me.menuPropertyCopy.Text = "Copy"
        '
        'menuPropertyDelete
        '
        Me.menuPropertyDelete.Index = 2
        Me.menuPropertyDelete.Text = "Delete"
        '
        'menuPropertyRename
        '
        Me.menuPropertyRename.Index = 3
        Me.menuPropertyRename.Text = "Rename"
        '
        'MenuItem99
        '
        Me.MenuItem99.Index = 4
        Me.MenuItem99.Text = "-"
        '
        'menuPropertyTransform
        '
        Me.menuPropertyTransform.Index = 5
        Me.menuPropertyTransform.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuPropertyShadow})
        Me.menuPropertyTransform.Text = "Refactor"
        '
        'menuPropertyShadow
        '
        Me.menuPropertyShadow.Index = 0
        Me.menuPropertyShadow.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuPropertyAddShadow, Me.menuPropertyRemoveShadow})
        Me.menuPropertyShadow.Text = "Shadow Property"
        '
        'menuPropertyAddShadow
        '
        Me.menuPropertyAddShadow.Index = 0
        Me.menuPropertyAddShadow.Text = "Add"
        '
        'menuPropertyRemoveShadow
        '
        Me.menuPropertyRemoveShadow.Index = 1
        Me.menuPropertyRemoveShadow.Text = "Remove"
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 6
        Me.MenuItem12.Text = "-"
        '
        'menuPropertyUml
        '
        Me.menuPropertyUml.Index = 7
        Me.menuPropertyUml.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuPropertyUmlShowPropLine, Me.menuPropertyUmlRemovePropLine})
        Me.menuPropertyUml.Text = "Uml"
        '
        'menuPropertyUmlShowPropLine
        '
        Me.menuPropertyUmlShowPropLine.Index = 0
        Me.menuPropertyUmlShowPropLine.Text = "Add To Diagram As Line"
        '
        'menuPropertyUmlRemovePropLine
        '
        Me.menuPropertyUmlRemovePropLine.Index = 1
        Me.menuPropertyUmlRemovePropLine.Text = "Remove Line From Diagram"
        '
        'MenuItem118
        '
        Me.MenuItem118.Index = 8
        Me.MenuItem118.Text = "-"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 9
        Me.MenuItem14.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem86})
        Me.MenuItem14.Text = "Preview"
        '
        'MenuItem86
        '
        Me.MenuItem86.Index = 0
        Me.MenuItem86.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuPropertyCodeCs, Me.menuPropertyCodeVb, Me.menuPropertyCodeDelphi})
        Me.MenuItem86.Text = "Source Code"
        '
        'menuPropertyCodeCs
        '
        Me.menuPropertyCodeCs.Index = 0
        Me.menuPropertyCodeCs.Text = "C#"
        '
        'menuPropertyCodeVb
        '
        Me.menuPropertyCodeVb.Index = 1
        Me.menuPropertyCodeVb.Text = "VB.Net"
        '
        'menuPropertyCodeDelphi
        '
        Me.menuPropertyCodeDelphi.Index = 2
        Me.menuPropertyCodeDelphi.Text = "Delphi"
        '
        'menuPropertyPluginsSeparator
        '
        Me.menuPropertyPluginsSeparator.Index = 10
        Me.menuPropertyPluginsSeparator.Text = "-"
        '
        'menuPropertyPlugins
        '
        Me.menuPropertyPlugins.Index = 11
        Me.menuPropertyPlugins.Text = "Plugins"
        '
        'MenuItem106
        '
        Me.MenuItem106.Index = 12
        Me.MenuItem106.Text = "-"
        '
        'menuPropertyProperties
        '
        Me.menuPropertyProperties.Index = 13
        Me.menuPropertyProperties.Text = "Properties"
        '
        'menuSourceList
        '
        Me.menuSourceList.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSourceListAddSource, Me.MenuItem91, Me.menuSourceListPaste, Me.MenuItem55, Me.MenuItem66, Me.MenuItem70, Me.MenuItem56})
        '
        'menuSourceListAddSource
        '
        Me.menuSourceListAddSource.Index = 0
        Me.menuSourceListAddSource.Text = "Add Data Source"
        '
        'MenuItem91
        '
        Me.MenuItem91.Index = 1
        Me.MenuItem91.Text = "-"
        '
        'menuSourceListPaste
        '
        Me.menuSourceListPaste.Index = 2
        Me.menuSourceListPaste.Text = "Paste"
        '
        'MenuItem55
        '
        Me.MenuItem55.Index = 3
        Me.MenuItem55.Text = "-"
        '
        'MenuItem66
        '
        Me.MenuItem66.Index = 4
        Me.MenuItem66.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem87})
        Me.MenuItem66.Text = "Preview"
        '
        'MenuItem87
        '
        Me.MenuItem87.Index = 0
        Me.MenuItem87.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSrcListPreviewDDL})
        Me.MenuItem87.Text = "Database"
        '
        'menuSrcListPreviewDDL
        '
        Me.menuSrcListPreviewDDL.Index = 0
        Me.menuSrcListPreviewDDL.Text = "DDL"
        '
        'MenuItem70
        '
        Me.MenuItem70.Index = 5
        Me.MenuItem70.Text = "-"
        '
        'MenuItem56
        '
        Me.MenuItem56.Index = 6
        Me.MenuItem56.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSrcListSynchTMToDB, Me.menuSrcListSynchTMToModel})
        Me.MenuItem56.Text = "Synchronize"
        '
        'menuSrcListSynchTMToDB
        '
        Me.menuSrcListSynchTMToDB.Index = 0
        Me.menuSrcListSynchTMToDB.Text = "From Model To Data Sources"
        '
        'menuSrcListSynchTMToModel
        '
        Me.menuSrcListSynchTMToModel.Index = 1
        Me.menuSrcListSynchTMToModel.Text = "From Data Sources To Model"
        '
        'menuSource
        '
        Me.menuSource.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSourceAddTable, Me.MenuItem7, Me.menuSourceCut, Me.menuSourceCopy, Me.menuSourcePaste, Me.menuSourceDelete, Me.menuSourceRename, Me.MenuItem23, Me.menuSourceTestConnection, Me.MenuItem95, Me.MenuItem25, Me.MenuItem27, Me.MenuItem24, Me.menuSourcePluginsSeparator, Me.menuSourcePlugins, Me.MenuItem108, Me.menuSourceProperties})
        '
        'menuSourceAddTable
        '
        Me.menuSourceAddTable.Index = 0
        Me.menuSourceAddTable.Text = "Add Table"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 1
        Me.MenuItem7.Text = "-"
        '
        'menuSourceCut
        '
        Me.menuSourceCut.Index = 2
        Me.menuSourceCut.Text = "Cut"
        '
        'menuSourceCopy
        '
        Me.menuSourceCopy.Index = 3
        Me.menuSourceCopy.Text = "Copy"
        '
        'menuSourcePaste
        '
        Me.menuSourcePaste.Index = 4
        Me.menuSourcePaste.Text = "Paste"
        '
        'menuSourceDelete
        '
        Me.menuSourceDelete.Index = 5
        Me.menuSourceDelete.Text = "Delete"
        '
        'menuSourceRename
        '
        Me.menuSourceRename.Index = 6
        Me.menuSourceRename.Text = "Rename"
        '
        'MenuItem23
        '
        Me.MenuItem23.Index = 7
        Me.MenuItem23.Text = "-"
        '
        'menuSourceTestConnection
        '
        Me.menuSourceTestConnection.Index = 8
        Me.menuSourceTestConnection.Text = "Test Connection"
        '
        'MenuItem95
        '
        Me.MenuItem95.Index = 9
        Me.MenuItem95.Text = "-"
        '
        'MenuItem25
        '
        Me.MenuItem25.Index = 10
        Me.MenuItem25.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem88})
        Me.MenuItem25.Text = "Preview"
        '
        'MenuItem88
        '
        Me.MenuItem88.Index = 0
        Me.MenuItem88.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSourceCodeDTD})
        Me.MenuItem88.Text = "Database"
        '
        'menuSourceCodeDTD
        '
        Me.menuSourceCodeDTD.Index = 0
        Me.menuSourceCodeDTD.Text = "DDL"
        '
        'MenuItem27
        '
        Me.MenuItem27.Index = 11
        Me.MenuItem27.Text = "-"
        '
        'MenuItem24
        '
        Me.MenuItem24.Index = 12
        Me.MenuItem24.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem65, Me.MenuItem62})
        Me.MenuItem24.Text = "Synchronize"
        '
        'MenuItem65
        '
        Me.MenuItem65.Index = 0
        Me.MenuItem65.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSourceSynchClsToTbl, Me.menuSourceSynchTblToCls})
        Me.MenuItem65.Text = "Domain Model"
        '
        'menuSourceSynchClsToTbl
        '
        Me.menuSourceSynchClsToTbl.Index = 0
        Me.menuSourceSynchClsToTbl.Text = "From Classes To Tables"
        '
        'menuSourceSynchTblToCls
        '
        Me.menuSourceSynchTblToCls.Index = 1
        Me.menuSourceSynchTblToCls.Text = "From Tables To Classes"
        '
        'MenuItem62
        '
        Me.MenuItem62.Index = 1
        Me.MenuItem62.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSourceSynchSource, Me.menuSourceSynchModel})
        Me.MenuItem62.Text = "Table Model"
        '
        'menuSourceSynchSource
        '
        Me.menuSourceSynchSource.Index = 0
        Me.menuSourceSynchSource.Text = "From Model To Data Source"
        '
        'menuSourceSynchModel
        '
        Me.menuSourceSynchModel.Index = 1
        Me.menuSourceSynchModel.Text = "From Data Source To Model "
        '
        'menuSourcePluginsSeparator
        '
        Me.menuSourcePluginsSeparator.Index = 13
        Me.menuSourcePluginsSeparator.Text = "-"
        '
        'menuSourcePlugins
        '
        Me.menuSourcePlugins.Index = 14
        Me.menuSourcePlugins.Text = "Plugins"
        '
        'MenuItem108
        '
        Me.MenuItem108.Index = 15
        Me.MenuItem108.Text = "-"
        '
        'menuSourceProperties
        '
        Me.menuSourceProperties.Index = 16
        Me.menuSourceProperties.Text = "Properties"
        '
        'menuTable
        '
        Me.menuTable.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuTableAddColumn, Me.MenuItem10, Me.menuTableCut, Me.menuTableCopy, Me.menuTablePaste, Me.menuTableDelete, Me.menuTableRename, Me.MenuItem26, Me.MenuItem28, Me.menuTablePluginsSeparator, Me.menuTablePlugins, Me.MenuItem109, Me.menuTableProperties})
        '
        'menuTableAddColumn
        '
        Me.menuTableAddColumn.Index = 0
        Me.menuTableAddColumn.Text = "Add Column"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 1
        Me.MenuItem10.Text = "-"
        '
        'menuTableCut
        '
        Me.menuTableCut.Index = 2
        Me.menuTableCut.Text = "Cut"
        '
        'menuTableCopy
        '
        Me.menuTableCopy.Index = 3
        Me.menuTableCopy.Text = "Copy"
        '
        'menuTablePaste
        '
        Me.menuTablePaste.Index = 4
        Me.menuTablePaste.Text = "Paste"
        '
        'menuTableDelete
        '
        Me.menuTableDelete.Index = 5
        Me.menuTableDelete.Text = "Delete"
        '
        'menuTableRename
        '
        Me.menuTableRename.Index = 6
        Me.menuTableRename.Text = "Rename"
        '
        'MenuItem26
        '
        Me.MenuItem26.Index = 7
        Me.MenuItem26.Text = "-"
        '
        'MenuItem28
        '
        Me.MenuItem28.Index = 8
        Me.MenuItem28.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem89})
        Me.MenuItem28.Text = "Preview"
        '
        'MenuItem89
        '
        Me.MenuItem89.Index = 0
        Me.MenuItem89.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuTableCodeDTD})
        Me.MenuItem89.Text = "Database"
        '
        'menuTableCodeDTD
        '
        Me.menuTableCodeDTD.Index = 0
        Me.menuTableCodeDTD.Text = "DDL"
        '
        'menuTablePluginsSeparator
        '
        Me.menuTablePluginsSeparator.Index = 9
        Me.menuTablePluginsSeparator.Text = "-"
        '
        'menuTablePlugins
        '
        Me.menuTablePlugins.Index = 10
        Me.menuTablePlugins.Text = "Plugins"
        '
        'MenuItem109
        '
        Me.MenuItem109.Index = 11
        Me.MenuItem109.Text = "-"
        '
        'menuTableProperties
        '
        Me.menuTableProperties.Index = 12
        Me.menuTableProperties.Text = "Properties"
        '
        'menuColumn
        '
        Me.menuColumn.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuColumnCut, Me.menuColumnCopy, Me.menuColumnDelete, Me.menuColumnRename, Me.menuColumnPluginsSeparator, Me.menuColumnPlugins, Me.MenuItem107, Me.menuColumnProperties})
        '
        'menuColumnCut
        '
        Me.menuColumnCut.Index = 0
        Me.menuColumnCut.Text = "Cut"
        '
        'menuColumnCopy
        '
        Me.menuColumnCopy.Index = 1
        Me.menuColumnCopy.Text = "Copy"
        '
        'menuColumnDelete
        '
        Me.menuColumnDelete.Index = 2
        Me.menuColumnDelete.Text = "Delete"
        '
        'menuColumnRename
        '
        Me.menuColumnRename.Index = 3
        Me.menuColumnRename.Text = "Rename"
        '
        'menuColumnPluginsSeparator
        '
        Me.menuColumnPluginsSeparator.Index = 4
        Me.menuColumnPluginsSeparator.Text = "-"
        '
        'menuColumnPlugins
        '
        Me.menuColumnPlugins.Index = 5
        Me.menuColumnPlugins.Text = "Plugins"
        '
        'MenuItem107
        '
        Me.MenuItem107.Index = 6
        Me.MenuItem107.Text = "-"
        '
        'menuColumnProperties
        '
        Me.menuColumnProperties.Index = 7
        Me.menuColumnProperties.Text = "Properties"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "doc1"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'menuConfigList
        '
        Me.menuConfigList.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuConfigListAddConfig})
        '
        'menuConfigListAddConfig
        '
        Me.menuConfigListAddConfig.Index = 0
        Me.menuConfigListAddConfig.Text = "Add Tool Configuration"
        '
        'menuConfig
        '
        Me.menuConfig.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuConfigSetActive, Me.MenuItem44, Me.menuConfigDelete, Me.menuConfigRename, Me.MenuItem110, Me.menuConfigProperties})
        '
        'menuConfigSetActive
        '
        Me.menuConfigSetActive.Index = 0
        Me.menuConfigSetActive.Text = "Set As Currently Active Configuration"
        '
        'MenuItem44
        '
        Me.MenuItem44.Index = 1
        Me.MenuItem44.Text = "-"
        '
        'menuConfigDelete
        '
        Me.menuConfigDelete.Index = 2
        Me.menuConfigDelete.Text = "Delete"
        '
        'menuConfigRename
        '
        Me.menuConfigRename.Index = 3
        Me.menuConfigRename.Text = "Rename"
        '
        'MenuItem110
        '
        Me.MenuItem110.Index = 4
        Me.MenuItem110.Text = "-"
        '
        'menuConfigProperties
        '
        Me.menuConfigProperties.Index = 5
        Me.menuConfigProperties.Text = "Properties"
        '
        'menuDiagramList
        '
        Me.menuDiagramList.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDiagramListAddDiagram})
        '
        'menuDiagramListAddDiagram
        '
        Me.menuDiagramListAddDiagram.Index = 0
        Me.menuDiagramListAddDiagram.Text = "Add Uml Diagram"
        '
        'menuSourceCodeFile
        '
        Me.menuSourceCodeFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSourceCodeFileRemove, Me.menuSourceCodeFileDelete, Me.MenuItem111, Me.menuSourceCodeFileProperties})
        '
        'menuSourceCodeFileRemove
        '
        Me.menuSourceCodeFileRemove.Index = 0
        Me.menuSourceCodeFileRemove.Text = "Remove"
        '
        'menuSourceCodeFileDelete
        '
        Me.menuSourceCodeFileDelete.Index = 1
        Me.menuSourceCodeFileDelete.Text = "Delete"
        '
        'MenuItem111
        '
        Me.MenuItem111.Index = 2
        Me.MenuItem111.Text = "-"
        '
        'menuSourceCodeFileProperties
        '
        Me.menuSourceCodeFileProperties.Index = 3
        Me.menuSourceCodeFileProperties.Text = "Properties"
        '
        'menuProject
        '
        Me.menuProject.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuProjectAdd, Me.MenuItem92, Me.menuProjectSave, Me.menuProjectSaveAs, Me.menuProjectSaveAll, Me.MenuItem96, Me.menuProjectPaste, Me.menuProjectRename, Me.MenuItem94, Me.menuProjectClose, Me.MenuItem112, Me.menuProjectProperties})
        '
        'menuProjectAdd
        '
        Me.menuProjectAdd.Index = 0
        Me.menuProjectAdd.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuProjectAddNewDomain, Me.menuProjectAddExistingDomain})
        Me.menuProjectAdd.Text = "Add"
        '
        'menuProjectAddNewDomain
        '
        Me.menuProjectAddNewDomain.Index = 0
        Me.menuProjectAddNewDomain.Text = "New Domain Model..."
        '
        'menuProjectAddExistingDomain
        '
        Me.menuProjectAddExistingDomain.Index = 1
        Me.menuProjectAddExistingDomain.Text = "Existing Domain Model..."
        '
        'MenuItem92
        '
        Me.MenuItem92.Index = 1
        Me.MenuItem92.Text = "-"
        '
        'menuProjectSave
        '
        Me.menuProjectSave.Index = 2
        Me.menuProjectSave.Text = "Save"
        '
        'menuProjectSaveAs
        '
        Me.menuProjectSaveAs.Index = 3
        Me.menuProjectSaveAs.Text = "Save As..."
        '
        'menuProjectSaveAll
        '
        Me.menuProjectSaveAll.Index = 4
        Me.menuProjectSaveAll.Text = "Save All"
        '
        'MenuItem96
        '
        Me.MenuItem96.Index = 5
        Me.MenuItem96.Text = "-"
        '
        'menuProjectPaste
        '
        Me.menuProjectPaste.Index = 6
        Me.menuProjectPaste.Text = "Paste"
        '
        'menuProjectRename
        '
        Me.menuProjectRename.Index = 7
        Me.menuProjectRename.Text = "Rename"
        '
        'MenuItem94
        '
        Me.MenuItem94.Index = 8
        Me.MenuItem94.Text = "-"
        '
        'menuProjectClose
        '
        Me.menuProjectClose.Index = 9
        Me.menuProjectClose.Text = "Close"
        '
        'MenuItem112
        '
        Me.MenuItem112.Index = 10
        Me.MenuItem112.Text = "-"
        '
        'menuProjectProperties
        '
        Me.menuProjectProperties.Index = 11
        Me.menuProjectProperties.Text = "Properties"
        '
        'ToolBar1
        '
        Me.ToolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.toolBarButtonBack, Me.toolBarButtonForward, Me.toolBarButtonUp, Me.ToolBarButton4, Me.toolBarButtonNew, Me.toolBarButtonNewItem, Me.toolBarButtonOpen, Me.toolBarButtonSave, Me.toolBarButtonSaveAll, Me.ToolBarButton1, Me.toolBarButtonPrint, Me.toolBarButtonPrintPreview, Me.ToolBarButton7, Me.toolBarButtonCut, Me.toolBarButtonCopy, Me.toolBarButtonPaste, Me.toolBarButtonFind, Me.ToolBarButton2, Me.toolBarButtonRun, Me.ToolBarButton6, Me.toolBarButtonSynch, Me.toolBarButtonDiscard, Me.toolBarButtonClassesToCode, Me.toolBarButtonCodeToClasses, Me.toolBarButtonTablesToClasses, Me.toolBarButtonClassesToTables, Me.toolBarButtonTablesToSource, Me.toolBarButtonSourceToTables, Me.ToolBarButton3, Me.toolBarButtonExplorer, Me.toolBarButtonProperties, Me.toolBarButtonTools, Me.toolBarButtonList, Me.toolBarButtonXmlBehind, Me.toolBarButtonUml, Me.ToolBarButton5, Me.toolBarButtonWizards})
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.ImageList = Me.imageListSmall
        Me.ToolBar1.Location = New System.Drawing.Point(0, 0)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(679, 72)
        Me.ToolBar1.TabIndex = 10
        '
        'toolBarButtonBack
        '
        Me.toolBarButtonBack.Enabled = False
        Me.toolBarButtonBack.ImageIndex = 92
        Me.toolBarButtonBack.Name = "toolBarButtonBack"
        Me.toolBarButtonBack.Tag = "Back"
        Me.toolBarButtonBack.ToolTipText = "Back"
        '
        'toolBarButtonForward
        '
        Me.toolBarButtonForward.Enabled = False
        Me.toolBarButtonForward.ImageIndex = 93
        Me.toolBarButtonForward.Name = "toolBarButtonForward"
        Me.toolBarButtonForward.Tag = "Forward"
        Me.toolBarButtonForward.ToolTipText = "Forward"
        '
        'toolBarButtonUp
        '
        Me.toolBarButtonUp.Enabled = False
        Me.toolBarButtonUp.ImageIndex = 94
        Me.toolBarButtonUp.Name = "toolBarButtonUp"
        Me.toolBarButtonUp.Tag = "Up"
        Me.toolBarButtonUp.ToolTipText = "Up"
        '
        'ToolBarButton4
        '
        Me.ToolBarButton4.Name = "ToolBarButton4"
        Me.ToolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonNew
        '
        Me.toolBarButtonNew.DropDownMenu = Me.menuNew
        Me.toolBarButtonNew.ImageIndex = 28
        Me.toolBarButtonNew.Name = "toolBarButtonNew"
        Me.toolBarButtonNew.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.toolBarButtonNew.Tag = "New"
        Me.toolBarButtonNew.ToolTipText = "New Domain Model"
        '
        'toolBarButtonNewItem
        '
        Me.toolBarButtonNewItem.DropDownMenu = Me.menuNewItem
        Me.toolBarButtonNewItem.Enabled = False
        Me.toolBarButtonNewItem.ImageIndex = 36
        Me.toolBarButtonNewItem.Name = "toolBarButtonNewItem"
        Me.toolBarButtonNewItem.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.toolBarButtonNewItem.Tag = "NewItem"
        Me.toolBarButtonNewItem.ToolTipText = "Add New Item"
        '
        'toolBarButtonOpen
        '
        Me.toolBarButtonOpen.ImageIndex = 29
        Me.toolBarButtonOpen.Name = "toolBarButtonOpen"
        Me.toolBarButtonOpen.Tag = "Open"
        Me.toolBarButtonOpen.ToolTipText = "Open File"
        '
        'toolBarButtonSave
        '
        Me.toolBarButtonSave.Enabled = False
        Me.toolBarButtonSave.ImageIndex = 30
        Me.toolBarButtonSave.Name = "toolBarButtonSave"
        Me.toolBarButtonSave.Tag = "Save"
        Me.toolBarButtonSave.ToolTipText = "Save"
        '
        'toolBarButtonSaveAll
        '
        Me.toolBarButtonSaveAll.Enabled = False
        Me.toolBarButtonSaveAll.ImageIndex = 31
        Me.toolBarButtonSaveAll.Name = "toolBarButtonSaveAll"
        Me.toolBarButtonSaveAll.Tag = "SaveAll"
        Me.toolBarButtonSaveAll.ToolTipText = "Save All"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonPrint
        '
        Me.toolBarButtonPrint.Enabled = False
        Me.toolBarButtonPrint.ImageIndex = 110
        Me.toolBarButtonPrint.Name = "toolBarButtonPrint"
        Me.toolBarButtonPrint.Tag = "Print"
        Me.toolBarButtonPrint.ToolTipText = "Print"
        '
        'toolBarButtonPrintPreview
        '
        Me.toolBarButtonPrintPreview.Enabled = False
        Me.toolBarButtonPrintPreview.ImageIndex = 111
        Me.toolBarButtonPrintPreview.Name = "toolBarButtonPrintPreview"
        Me.toolBarButtonPrintPreview.Tag = "PrintPreview"
        Me.toolBarButtonPrintPreview.ToolTipText = "Print Preview"
        '
        'ToolBarButton7
        '
        Me.ToolBarButton7.Name = "ToolBarButton7"
        Me.ToolBarButton7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonCut
        '
        Me.toolBarButtonCut.Enabled = False
        Me.toolBarButtonCut.ImageIndex = 89
        Me.toolBarButtonCut.Name = "toolBarButtonCut"
        Me.toolBarButtonCut.Tag = "Cut"
        Me.toolBarButtonCut.ToolTipText = "Cut"
        '
        'toolBarButtonCopy
        '
        Me.toolBarButtonCopy.Enabled = False
        Me.toolBarButtonCopy.ImageIndex = 90
        Me.toolBarButtonCopy.Name = "toolBarButtonCopy"
        Me.toolBarButtonCopy.Tag = "Copy"
        Me.toolBarButtonCopy.ToolTipText = "Copy"
        '
        'toolBarButtonPaste
        '
        Me.toolBarButtonPaste.Enabled = False
        Me.toolBarButtonPaste.ImageIndex = 91
        Me.toolBarButtonPaste.Name = "toolBarButtonPaste"
        Me.toolBarButtonPaste.Tag = "Paste"
        Me.toolBarButtonPaste.ToolTipText = "Paste"
        '
        'toolBarButtonFind
        '
        Me.toolBarButtonFind.Enabled = False
        Me.toolBarButtonFind.ImageIndex = 95
        Me.toolBarButtonFind.Name = "toolBarButtonFind"
        Me.toolBarButtonFind.Tag = "Find"
        Me.toolBarButtonFind.ToolTipText = "Find"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Name = "ToolBarButton2"
        Me.ToolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonRun
        '
        Me.toolBarButtonRun.Enabled = False
        Me.toolBarButtonRun.ImageIndex = 38
        Me.toolBarButtonRun.Name = "toolBarButtonRun"
        Me.toolBarButtonRun.Tag = "Run"
        Me.toolBarButtonRun.ToolTipText = "Run Model in Domain Explorer"
        '
        'ToolBarButton6
        '
        Me.ToolBarButton6.Name = "ToolBarButton6"
        Me.ToolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonSynch
        '
        Me.toolBarButtonSynch.DropDownMenu = Me.menuSynch
        Me.toolBarButtonSynch.Enabled = False
        Me.toolBarButtonSynch.ImageIndex = 37
        Me.toolBarButtonSynch.Name = "toolBarButtonSynch"
        Me.toolBarButtonSynch.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.toolBarButtonSynch.Tag = "Commit"
        Me.toolBarButtonSynch.ToolTipText = "Commit Synchronization"
        '
        'toolBarButtonDiscard
        '
        Me.toolBarButtonDiscard.Enabled = False
        Me.toolBarButtonDiscard.ImageIndex = 112
        Me.toolBarButtonDiscard.Name = "toolBarButtonDiscard"
        Me.toolBarButtonDiscard.Tag = "Discard"
        Me.toolBarButtonDiscard.ToolTipText = "Discard Synchronization"
        '
        'toolBarButtonClassesToCode
        '
        Me.toolBarButtonClassesToCode.Enabled = False
        Me.toolBarButtonClassesToCode.ImageIndex = 108
        Me.toolBarButtonClassesToCode.Name = "toolBarButtonClassesToCode"
        Me.toolBarButtonClassesToCode.Tag = "ClassesToCode"
        Me.toolBarButtonClassesToCode.ToolTipText = "From Model To Code Synchronization"
        '
        'toolBarButtonCodeToClasses
        '
        Me.toolBarButtonCodeToClasses.Enabled = False
        Me.toolBarButtonCodeToClasses.ImageIndex = 109
        Me.toolBarButtonCodeToClasses.Name = "toolBarButtonCodeToClasses"
        Me.toolBarButtonCodeToClasses.Tag = "CodeToClasses"
        Me.toolBarButtonCodeToClasses.ToolTipText = "From Code To Model Synchronization"
        '
        'toolBarButtonTablesToClasses
        '
        Me.toolBarButtonTablesToClasses.Enabled = False
        Me.toolBarButtonTablesToClasses.ImageIndex = 107
        Me.toolBarButtonTablesToClasses.Name = "toolBarButtonTablesToClasses"
        Me.toolBarButtonTablesToClasses.Tag = "TablesToClasses"
        Me.toolBarButtonTablesToClasses.ToolTipText = "From Tables To Classes Synchronization"
        '
        'toolBarButtonClassesToTables
        '
        Me.toolBarButtonClassesToTables.Enabled = False
        Me.toolBarButtonClassesToTables.ImageIndex = 106
        Me.toolBarButtonClassesToTables.Name = "toolBarButtonClassesToTables"
        Me.toolBarButtonClassesToTables.Tag = "ClassesToTables"
        Me.toolBarButtonClassesToTables.ToolTipText = "From Classes To Tables Synchronization"
        '
        'toolBarButtonTablesToSource
        '
        Me.toolBarButtonTablesToSource.Enabled = False
        Me.toolBarButtonTablesToSource.ImageIndex = 104
        Me.toolBarButtonTablesToSource.Name = "toolBarButtonTablesToSource"
        Me.toolBarButtonTablesToSource.Tag = "TablesToSource"
        Me.toolBarButtonTablesToSource.ToolTipText = "From Model To Data Source Synchronization"
        '
        'toolBarButtonSourceToTables
        '
        Me.toolBarButtonSourceToTables.Enabled = False
        Me.toolBarButtonSourceToTables.ImageIndex = 105
        Me.toolBarButtonSourceToTables.Name = "toolBarButtonSourceToTables"
        Me.toolBarButtonSourceToTables.Tag = "SourceToTables"
        Me.toolBarButtonSourceToTables.ToolTipText = "From Data Source To Model Synchronization"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.Name = "ToolBarButton3"
        Me.ToolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonExplorer
        '
        Me.toolBarButtonExplorer.Enabled = False
        Me.toolBarButtonExplorer.ImageIndex = 46
        Me.toolBarButtonExplorer.Name = "toolBarButtonExplorer"
        Me.toolBarButtonExplorer.Tag = "Explorer"
        Me.toolBarButtonExplorer.ToolTipText = "Project Explorer"
        '
        'toolBarButtonProperties
        '
        Me.toolBarButtonProperties.Enabled = False
        Me.toolBarButtonProperties.ImageIndex = 88
        Me.toolBarButtonProperties.Name = "toolBarButtonProperties"
        Me.toolBarButtonProperties.Tag = "Properties"
        Me.toolBarButtonProperties.ToolTipText = "Properties"
        '
        'toolBarButtonTools
        '
        Me.toolBarButtonTools.Enabled = False
        Me.toolBarButtonTools.ImageIndex = 97
        Me.toolBarButtonTools.Name = "toolBarButtonTools"
        Me.toolBarButtonTools.Tag = "Tools"
        Me.toolBarButtonTools.ToolTipText = "Tools"
        '
        'toolBarButtonList
        '
        Me.toolBarButtonList.Enabled = False
        Me.toolBarButtonList.ImageIndex = 98
        Me.toolBarButtonList.Name = "toolBarButtonList"
        Me.toolBarButtonList.Tag = "List"
        Me.toolBarButtonList.ToolTipText = "List View"
        '
        'toolBarButtonXmlBehind
        '
        Me.toolBarButtonXmlBehind.DropDownMenu = Me.menuXmlBehind
        Me.toolBarButtonXmlBehind.Enabled = False
        Me.toolBarButtonXmlBehind.ImageIndex = 86
        Me.toolBarButtonXmlBehind.Name = "toolBarButtonXmlBehind"
        Me.toolBarButtonXmlBehind.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.toolBarButtonXmlBehind.Tag = "Xml"
        Me.toolBarButtonXmlBehind.ToolTipText = "Xml Behind"
        '
        'toolBarButtonUml
        '
        Me.toolBarButtonUml.Enabled = False
        Me.toolBarButtonUml.ImageIndex = 99
        Me.toolBarButtonUml.Name = "toolBarButtonUml"
        Me.toolBarButtonUml.Tag = "Uml"
        Me.toolBarButtonUml.ToolTipText = "Uml Diagram"
        '
        'ToolBarButton5
        '
        Me.ToolBarButton5.Name = "ToolBarButton5"
        Me.ToolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'toolBarButtonWizards
        '
        Me.toolBarButtonWizards.DropDownMenu = Me.menuWizards
        Me.toolBarButtonWizards.ImageIndex = 84
        Me.toolBarButtonWizards.Name = "toolBarButtonWizards"
        Me.toolBarButtonWizards.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.toolBarButtonWizards.Tag = "Wizard"
        Me.toolBarButtonWizards.ToolTipText = "Wizards"
        '
        'menuUserDoc
        '
        Me.menuUserDoc.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUserDocUndo, Me.menuUserDocRedo, Me.MenuItem115, Me.menuUserDocCut, Me.menuUserDocCopy, Me.menuUserDocPaste, Me.menuUserDocDelete, Me.MenuItem119, Me.menuUserDocSelectAll})
        '
        'menuUserDocUndo
        '
        Me.menuUserDocUndo.Index = 0
        Me.menuUserDocUndo.Text = "&Undo"
        '
        'menuUserDocRedo
        '
        Me.menuUserDocRedo.Index = 1
        Me.menuUserDocRedo.Text = "Redo"
        '
        'MenuItem115
        '
        Me.MenuItem115.Index = 2
        Me.MenuItem115.Text = "-"
        '
        'menuUserDocCut
        '
        Me.menuUserDocCut.Index = 3
        Me.menuUserDocCut.Text = "Cu&t"
        '
        'menuUserDocCopy
        '
        Me.menuUserDocCopy.Index = 4
        Me.menuUserDocCopy.Text = "&Copy"
        '
        'menuUserDocPaste
        '
        Me.menuUserDocPaste.Index = 5
        Me.menuUserDocPaste.Text = "&Paste"
        '
        'menuUserDocDelete
        '
        Me.menuUserDocDelete.Index = 6
        Me.menuUserDocDelete.Text = "&Delete"
        '
        'MenuItem119
        '
        Me.MenuItem119.Index = 7
        Me.MenuItem119.Text = "-"
        '
        'menuUserDocSelectAll
        '
        Me.menuUserDocSelectAll.Index = 8
        Me.menuUserDocSelectAll.Text = "Select &All"
        '
        'menuUmlDiagram
        '
        Me.menuUmlDiagram.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUmlView, Me.MenuItem121, Me.menuUmlAddClass, Me.menuUmlAddLines, Me.MenuItem117, Me.menuUmlRemoveLines, Me.MenuItem113, Me.menuUmlZoom, Me.MenuItem123, Me.menuUmlDeleteDiagram, Me.MenuItem128, Me.menuUmSelectAll, Me.MenuItem141, Me.menuUmlSaveViewAs, Me.menuUmlSaveAs, Me.MenuItem120, Me.menuUmlProperties})
        '
        'menuUmlView
        '
        Me.menuUmlView.Index = 0
        Me.menuUmlView.Text = "View"
        '
        'MenuItem121
        '
        Me.MenuItem121.Index = 1
        Me.MenuItem121.Text = "-"
        '
        'menuUmlAddClass
        '
        Me.menuUmlAddClass.Index = 2
        Me.menuUmlAddClass.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUmlAddClassNew, Me.menuUmlAddClassExisting})
        Me.menuUmlAddClass.Text = "Add Class"
        '
        'menuUmlAddClassNew
        '
        Me.menuUmlAddClassNew.Index = 0
        Me.menuUmlAddClassNew.Text = "New"
        '
        'menuUmlAddClassExisting
        '
        Me.menuUmlAddClassExisting.Index = 1
        Me.menuUmlAddClassExisting.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUmlAddClassExistingAll, Me.menuUmlAddClassExistingAllWithLines, Me.MenuItem126, Me.menuUmlAddClassExistingSelect})
        Me.menuUmlAddClassExisting.Text = "Existing"
        '
        'menuUmlAddClassExistingAll
        '
        Me.menuUmlAddClassExistingAll.Index = 0
        Me.menuUmlAddClassExistingAll.Text = "All Classes"
        '
        'menuUmlAddClassExistingAllWithLines
        '
        Me.menuUmlAddClassExistingAllWithLines.Index = 1
        Me.menuUmlAddClassExistingAllWithLines.Text = "All Classes And Lines"
        '
        'MenuItem126
        '
        Me.MenuItem126.Index = 2
        Me.MenuItem126.Text = "-"
        Me.MenuItem126.Visible = False
        '
        'menuUmlAddClassExistingSelect
        '
        Me.menuUmlAddClassExistingSelect.Index = 3
        Me.menuUmlAddClassExistingSelect.Text = "Select..."
        Me.menuUmlAddClassExistingSelect.Visible = False
        '
        'menuUmlAddLines
        '
        Me.menuUmlAddLines.Index = 3
        Me.menuUmlAddLines.Text = "Add Lines"
        '
        'MenuItem117
        '
        Me.MenuItem117.Index = 4
        Me.MenuItem117.Text = "-"
        '
        'menuUmlRemoveLines
        '
        Me.menuUmlRemoveLines.Index = 5
        Me.menuUmlRemoveLines.Text = "Remove Lines"
        '
        'MenuItem113
        '
        Me.MenuItem113.Index = 6
        Me.MenuItem113.Text = "-"
        '
        'menuUmlZoom
        '
        Me.menuUmlZoom.Index = 7
        Me.menuUmlZoom.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUmlZoomIn, Me.menuUmlZoomOut, Me.MenuItem140, Me.menuUmlZoom400, Me.menuUmlZoom200, Me.menuUmlZoom150, Me.menuUmlZoom100, Me.menuUmlZoom75, Me.menuUmlZoom50, Me.MenuItem138, Me.menuUmlFit})
        Me.menuUmlZoom.Text = "Zoom"
        '
        'menuUmlZoomIn
        '
        Me.menuUmlZoomIn.Index = 0
        Me.menuUmlZoomIn.Text = "Zoom In"
        '
        'menuUmlZoomOut
        '
        Me.menuUmlZoomOut.Index = 1
        Me.menuUmlZoomOut.Text = "Zoom Out"
        '
        'MenuItem140
        '
        Me.MenuItem140.Index = 2
        Me.MenuItem140.Text = "-"
        '
        'menuUmlZoom400
        '
        Me.menuUmlZoom400.Index = 3
        Me.menuUmlZoom400.Text = "400%"
        '
        'menuUmlZoom200
        '
        Me.menuUmlZoom200.Index = 4
        Me.menuUmlZoom200.Text = "200%"
        '
        'menuUmlZoom150
        '
        Me.menuUmlZoom150.Index = 5
        Me.menuUmlZoom150.Text = "150%"
        '
        'menuUmlZoom100
        '
        Me.menuUmlZoom100.Index = 6
        Me.menuUmlZoom100.Text = "100%"
        '
        'menuUmlZoom75
        '
        Me.menuUmlZoom75.Index = 7
        Me.menuUmlZoom75.Text = "75%"
        '
        'menuUmlZoom50
        '
        Me.menuUmlZoom50.Index = 8
        Me.menuUmlZoom50.Text = "50%"
        '
        'MenuItem138
        '
        Me.MenuItem138.Index = 9
        Me.MenuItem138.Text = "-"
        '
        'menuUmlFit
        '
        Me.menuUmlFit.Index = 10
        Me.menuUmlFit.Text = "Fit"
        '
        'MenuItem123
        '
        Me.MenuItem123.Index = 8
        Me.MenuItem123.Text = "-"
        '
        'menuUmlDeleteDiagram
        '
        Me.menuUmlDeleteDiagram.Index = 9
        Me.menuUmlDeleteDiagram.Text = "Delete"
        '
        'MenuItem128
        '
        Me.MenuItem128.Index = 10
        Me.MenuItem128.Text = "-"
        '
        'menuUmSelectAll
        '
        Me.menuUmSelectAll.Index = 11
        Me.menuUmSelectAll.Text = "Select All"
        '
        'MenuItem141
        '
        Me.MenuItem141.Index = 12
        Me.MenuItem141.Text = "-"
        '
        'menuUmlSaveViewAs
        '
        Me.menuUmlSaveViewAs.Index = 13
        Me.menuUmlSaveViewAs.Text = "Save View As..."
        '
        'menuUmlSaveAs
        '
        Me.menuUmlSaveAs.Index = 14
        Me.menuUmlSaveAs.Text = "Save Diagram As..."
        '
        'MenuItem120
        '
        Me.MenuItem120.Index = 15
        Me.MenuItem120.Text = "-"
        '
        'menuUmlProperties
        '
        Me.menuUmlProperties.Index = 16
        Me.menuUmlProperties.Text = "Properties"
        '
        'menuUmlLineEnd
        '
        Me.menuUmlLineEnd.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUmlLineEndAddPoint, Me.MenuItem124, Me.menuUmlLineEndLock, Me.menuUmlLineEndUnLock, Me.MenuItem131, Me.menuUmlLineEndSelectLine, Me.MenuItem122, Me.menuUmlLineEndRemoveLine, Me.MenuItem127, Me.menuUmlLineEndProperties})
        '
        'menuUmlLineEndAddPoint
        '
        Me.menuUmlLineEndAddPoint.Index = 0
        Me.menuUmlLineEndAddPoint.Text = "Add New Point To Line"
        '
        'MenuItem124
        '
        Me.MenuItem124.Index = 1
        Me.MenuItem124.Text = "-"
        '
        'menuUmlLineEndLock
        '
        Me.menuUmlLineEndLock.Index = 2
        Me.menuUmlLineEndLock.Text = "Lock"
        '
        'menuUmlLineEndUnLock
        '
        Me.menuUmlLineEndUnLock.Index = 3
        Me.menuUmlLineEndUnLock.Text = "UnLock"
        '
        'MenuItem131
        '
        Me.MenuItem131.Index = 4
        Me.MenuItem131.Text = "-"
        '
        'menuUmlLineEndSelectLine
        '
        Me.menuUmlLineEndSelectLine.Index = 5
        Me.menuUmlLineEndSelectLine.Text = "Select Line"
        '
        'MenuItem122
        '
        Me.MenuItem122.Index = 6
        Me.MenuItem122.Text = "-"
        '
        'menuUmlLineEndRemoveLine
        '
        Me.menuUmlLineEndRemoveLine.Index = 7
        Me.menuUmlLineEndRemoveLine.Text = "Remove Line From Diagram"
        '
        'MenuItem127
        '
        Me.MenuItem127.Index = 8
        Me.MenuItem127.Text = "-"
        '
        'menuUmlLineEndProperties
        '
        Me.menuUmlLineEndProperties.Index = 9
        Me.menuUmlLineEndProperties.Text = "Properties"
        '
        'menuUmlLinePoint
        '
        Me.menuUmlLinePoint.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuUmlLinePointAddPoint, Me.MenuItem129, Me.menuUmlLinePointRemove, Me.MenuItem130, Me.menuUmlLinePointProperties})
        '
        'menuUmlLinePointAddPoint
        '
        Me.menuUmlLinePointAddPoint.Index = 0
        Me.menuUmlLinePointAddPoint.Text = "Add New Point To Line"
        '
        'MenuItem129
        '
        Me.MenuItem129.Index = 1
        Me.MenuItem129.Text = "-"
        '
        'menuUmlLinePointRemove
        '
        Me.menuUmlLinePointRemove.Index = 2
        Me.menuUmlLinePointRemove.Text = "Remove Point From Line"
        '
        'MenuItem130
        '
        Me.MenuItem130.Index = 3
        Me.MenuItem130.Text = "-"
        '
        'menuUmlLinePointProperties
        '
        Me.menuUmlLinePointProperties.Index = 4
        Me.menuUmlLinePointProperties.Text = "Properties"
        '
        'panelProperties
        '
        Me.panelProperties.Controls.Add(Me.mapPropertyGrid)
        Me.panelProperties.Controls.Add(Me.panelPropertiesTitle)
        Me.panelProperties.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelProperties.Location = New System.Drawing.Point(163, 72)
        Me.panelProperties.Name = "panelProperties"
        Me.panelProperties.Size = New System.Drawing.Size(157, 128)
        Me.panelProperties.TabIndex = 11
        '
        'panelPropertiesTitle
        '
        Me.panelPropertiesTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelPropertiesTitle.Controls.Add(Me.Label1)
        Me.panelPropertiesTitle.Controls.Add(Me.buttonSwapPosProperties)
        Me.panelPropertiesTitle.Controls.Add(Me.buttonCloseProperties)
        Me.panelPropertiesTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelPropertiesTitle.Location = New System.Drawing.Point(0, 0)
        Me.panelPropertiesTitle.Name = "panelPropertiesTitle"
        Me.panelPropertiesTitle.Size = New System.Drawing.Size(157, 16)
        Me.panelPropertiesTitle.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(123, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Properties"
        '
        'buttonSwapPosProperties
        '
        Me.buttonSwapPosProperties.BackColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosProperties.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonSwapPosProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonSwapPosProperties.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonSwapPosProperties.ImageIndex = 4
        Me.buttonSwapPosProperties.ImageList = Me.imageListSmallButtons
        Me.buttonSwapPosProperties.Location = New System.Drawing.Point(123, 0)
        Me.buttonSwapPosProperties.Name = "buttonSwapPosProperties"
        Me.buttonSwapPosProperties.Size = New System.Drawing.Size(16, 14)
        Me.buttonSwapPosProperties.TabIndex = 4
        Me.buttonSwapPosProperties.UseVisualStyleBackColor = False
        '
        'buttonCloseProperties
        '
        Me.buttonCloseProperties.BackColor = System.Drawing.SystemColors.Control
        Me.buttonCloseProperties.Dock = System.Windows.Forms.DockStyle.Right
        Me.buttonCloseProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.buttonCloseProperties.ForeColor = System.Drawing.SystemColors.Control
        Me.buttonCloseProperties.ImageIndex = 0
        Me.buttonCloseProperties.ImageList = Me.imageListSmallButtons
        Me.buttonCloseProperties.Location = New System.Drawing.Point(139, 0)
        Me.buttonCloseProperties.Name = "buttonCloseProperties"
        Me.buttonCloseProperties.Size = New System.Drawing.Size(16, 14)
        Me.buttonCloseProperties.TabIndex = 3
        Me.buttonCloseProperties.UseVisualStyleBackColor = False
        '
        'Splitter2
        '
        Me.Splitter2.Location = New System.Drawing.Point(320, 72)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(2, 128)
        Me.Splitter2.TabIndex = 12
        Me.Splitter2.TabStop = False
        '
        'SourceCodePrintDocument1
        '
        Me.SourceCodePrintDocument1.Document = Nothing
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'enumValueMenu
        '
        Me.enumValueMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.deleteEnumValueMenuItem})
        '
        'deleteEnumValueMenuItem
        '
        Me.deleteEnumValueMenuItem.Index = 0
        Me.deleteEnumValueMenuItem.Text = "Delete"
        '
        'treePreviewClassesToCode
        '
        Me.treePreviewClassesToCode.AutoCollapseVerifyNodes = False
        Me.treePreviewClassesToCode.CheckBoxes = True
        Me.treePreviewClassesToCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treePreviewClassesToCode.HideSelection = False
        Me.treePreviewClassesToCode.ImageIndex = 0
        Me.treePreviewClassesToCode.ImageList = Me.imageListSmall
        Me.treePreviewClassesToCode.IsPreviewTree = False
        Me.treePreviewClassesToCode.IsVerifyTree = False
        Me.treePreviewClassesToCode.ListExceptions = CType(resources.GetObject("treePreviewClassesToCode.ListExceptions"), System.Collections.ArrayList)
        Me.treePreviewClassesToCode.Location = New System.Drawing.Point(0, 0)
        Me.treePreviewClassesToCode.Name = "treePreviewClassesToCode"
        Me.treePreviewClassesToCode.SelectedImageIndex = 0
        Me.treePreviewClassesToCode.Size = New System.Drawing.Size(136, 86)
        Me.treePreviewClassesToCode.TabIndex = 1
        Me.treePreviewClassesToCode.Visible = False
        '
        'mapTreeViewPreview
        '
        Me.mapTreeViewPreview.AutoCollapseVerifyNodes = True
        Me.mapTreeViewPreview.CheckBoxes = True
        Me.mapTreeViewPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapTreeViewPreview.HideSelection = False
        Me.mapTreeViewPreview.ImageIndex = 0
        Me.mapTreeViewPreview.ImageList = Me.imageListSmall
        Me.mapTreeViewPreview.IsPreviewTree = True
        Me.mapTreeViewPreview.IsVerifyTree = False
        Me.mapTreeViewPreview.ListExceptions = CType(resources.GetObject("mapTreeViewPreview.ListExceptions"), System.Collections.ArrayList)
        Me.mapTreeViewPreview.Location = New System.Drawing.Point(0, 0)
        Me.mapTreeViewPreview.Name = "mapTreeViewPreview"
        Me.mapTreeViewPreview.SelectedImageIndex = 0
        Me.mapTreeViewPreview.Size = New System.Drawing.Size(136, 86)
        Me.mapTreeViewPreview.TabIndex = 0
        '
        'mapPropertyGrid
        '
        Me.mapPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapPropertyGrid.Location = New System.Drawing.Point(0, 16)
        Me.mapPropertyGrid.Name = "mapPropertyGrid"
        Me.mapPropertyGrid.Size = New System.Drawing.Size(157, 112)
        Me.mapPropertyGrid.TabIndex = 1
        '
        'mapListView
        '
        Me.mapListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapListView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mapListView.frmDomainMapBrowser = Nothing
        Me.mapListView.HideSelection = False
        Me.mapListView.Location = New System.Drawing.Point(0, 0)
        Me.mapListView.Name = "mapListView"
        Me.mapListView.ParentNode = Nothing
        Me.mapListView.Size = New System.Drawing.Size(508, 134)
        Me.mapListView.SmallImageList = Me.imageListSmall
        Me.mapListView.TabIndex = 0
        Me.mapListView.UseCompatibleStateImageBehavior = False
        Me.mapListView.View = System.Windows.Forms.View.Details
        '
        'mapTreeView
        '
        Me.mapTreeView.AllowDrop = True
        Me.mapTreeView.AutoCollapseVerifyNodes = True
        Me.mapTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapTreeView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mapTreeView.HideSelection = False
        Me.mapTreeView.ImageIndex = 0
        Me.mapTreeView.ImageList = Me.imageListSmall
        Me.mapTreeView.IsPreviewTree = False
        Me.mapTreeView.IsVerifyTree = False
        Me.mapTreeView.LabelEdit = True
        Me.mapTreeView.ListExceptions = CType(resources.GetObject("mapTreeView.ListExceptions"), System.Collections.ArrayList)
        Me.mapTreeView.Location = New System.Drawing.Point(0, 16)
        Me.mapTreeView.Name = "mapTreeView"
        Me.mapTreeView.SelectedImageIndex = 0
        Me.mapTreeView.ShowRootLines = False
        Me.mapTreeView.Size = New System.Drawing.Size(160, 291)
        Me.mapTreeView.TabIndex = 1
        '
        'frmDomainMapBrowser
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(679, 403)
        Me.Controls.Add(Me.panelMain)
        Me.Controls.Add(Me.Splitter5)
        Me.Controls.Add(Me.panelRight)
        Me.Controls.Add(Me.Splitter2)
        Me.Controls.Add(Me.panelProperties)
        Me.Controls.Add(Me.Splitter4)
        Me.Controls.Add(Me.panelBottom)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.panelLeft)
        Me.Controls.Add(Me.panelStatus)
        Me.Controls.Add(Me.ToolBar1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmDomainMapBrowser"
        Me.Text = "  Puzzle ObjectMapper"
        Me.panelStatus.ResumeLayout(False)
        CType(Me.statusMain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusMessage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelLeft.ResumeLayout(False)
        Me.panelPropGridMap.ResumeLayout(False)
        Me.panelProjectTitle.ResumeLayout(False)
        Me.panelBottom.ResumeLayout(False)
        Me.tabControlMessages.ResumeLayout(False)
        Me.tabPageListView.ResumeLayout(False)
        Me.tabPageErrors.ResumeLayout(False)
        Me.tabPagePreviewList.ResumeLayout(False)
        Me.tabPageMessageList.ResumeLayout(False)
        Me.panelListTitle.ResumeLayout(False)
        Me.panelRight.ResumeLayout(False)
        Me.panelSecondTree.ResumeLayout(False)
        Me.tabControlTools.ResumeLayout(False)
        Me.tabToolsPreview.ResumeLayout(False)
        Me.panelToolsTitle.ResumeLayout(False)
        Me.panelMain.ResumeLayout(False)
        Me.panelDocuments.ResumeLayout(False)
        Me.tabControlDocuments.ResumeLayout(False)
        Me.tabPageMainUml.ResumeLayout(False)
        Me.panelTreeMap.ResumeLayout(False)
        Me.panelUmlTitle.ResumeLayout(False)
        Me.tabPageMainCustom.ResumeLayout(False)
        Me.panelDocTitle.ResumeLayout(False)
        Me.tabPageMainPreview.ResumeLayout(False)
        Me.panelPreviewDocTitle.ResumeLayout(False)
        Me.tabPageMainXmlBehind.ResumeLayout(False)
        Me.panelXmlBehindTitle.ResumeLayout(False)
        Me.tabPageMainCodeMap.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.panelMainTitle.ResumeLayout(False)
        Me.panelProperties.ResumeLayout(False)
        Me.panelPropertiesTitle.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region " Enumerations "

    Public Enum ActionTypeEnum
        None = 0
        SynchAction = 1
    End Enum

    Public Enum SynchModeEnum
        None = 0
        SourceToTables = 1
        TablesToSource = 2
        CodeToClasses = 3
        ClassesToCode = 4
        ClassesToTables = 5
        TablesToClasses = 6
    End Enum

    Public Enum MainDocumentType
        Text = 0
        CodeVbNet = 1
        CodeCSharp = 2
        CodeDelphi = 3
        DTD = 3
        XML = 4
        NPersist = 5
    End Enum

#End Region

#Region " Exported methods "

    Public Sub SetProjectDirty()

        m_ProjectDirty = True

    End Sub

    Public Function GetCurrentProject() As ProjectModel.IProject

        Return m_Project

    End Function

#End Region

#Region " Commands "

    Private Sub OpenProperties()

        If Not panelProperties.Visible Then

            TogglePropertiesVisibility()

            SaveVisibilitySettings()

        End If

        mapPropertyGrid.Focus()

    End Sub

    Private Sub SetCutCopyPasteEnabled()

        Dim txt As SyntaxBoxControl

        If Not m_CurrentCopyTarget Is Nothing Then

            If m_CurrentCopyTarget.GetType Is GetType(SyntaxBoxControl) Then

                txt = CType(m_CurrentCopyTarget, SyntaxBoxControl)

                If txt.Selection.Text = "" Then

                    menuEditCut.Enabled = False
                    menuEditCopy.Enabled = False
                    toolBarButtonCut.Enabled = False
                    toolBarButtonCopy.Enabled = False

                Else

                    menuEditCut.Enabled = True
                    menuEditCopy.Enabled = True
                    toolBarButtonCut.Enabled = True
                    toolBarButtonCopy.Enabled = True

                End If

                If Clipboard.GetDataObject.GetDataPresent(GetType(String)) Then

                    menuEditPaste.Enabled = True
                    toolBarButtonPaste.Enabled = True

                Else

                    menuEditPaste.Enabled = False
                    toolBarButtonPaste.Enabled = False

                End If

                Exit Sub

            End If

        End If



        If m_CurrentCopyTarget Is Nothing Then

            toolBarButtonCut.Enabled = False
            toolBarButtonCopy.Enabled = False

        Else

            toolBarButtonCut.Enabled = True
            toolBarButtonCopy.Enabled = True

        End If

        If m_SelectedCopySource Is Nothing Then

            toolBarButtonPaste.Enabled = False

        Else

            toolBarButtonPaste.Enabled = MayPaste()

        End If

    End Sub


    Private Sub AddShadowPropertiesForSubClass(ByVal classMap As IClassMap)

        MapServices.AddShadowProperties(classMap, "Would you like to add the following shadow properties to the class '" & classMap.Name & "'?" & vbCrLf & vbCrLf)

        RefreshAll()

    End Sub


    Private Sub RemoveShadowPropertiesFromSubClass(ByVal classMap As IClassMap)

        MapServices.RemoveShadowProperties(classMap, False, "Would you like to remove the following shadow properties from the class '" & classMap.Name & "'?" & vbCrLf & vbCrLf)

        RefreshAll()

    End Sub

    Private Sub AddShadowPropertyForSubClass(ByVal classMap As IClassMap, ByVal propertyMap As IPropertyMap)

        MapServices.AddShadowProperties(classMap, "Would you like to add the following shadow properties to the class '" & classMap.Name & "'?" & vbCrLf & vbCrLf, propertyMap)

        RefreshAll()

    End Sub


    Private Sub RemoveShadowPropertyFromSubClass(ByVal classMap As IClassMap, ByVal propertyMap As IPropertyMap)

        MapServices.RemoveShadowProperties(classMap, False, "Would you like to remove the following shadow properties from the class '" & classMap.Name & "'?" & vbCrLf & vbCrLf, propertyMap)

        RefreshAll()

    End Sub


    Public Function TestConnection(ByVal sourceMap As ISourceMap, Optional ByVal quietFailure As Boolean = False, Optional ByVal quietSuccess As Boolean = False, Optional ByRef errMsg As String = "") As Boolean

        Dim addErrMsg As String

        If Not ConnectionServices.TestConnection(sourceMap.ConnectionString, sourceMap.ProviderType, errMsg) Then

            If Not quietFailure Then

                Try

                    FormattingServices.VerifyConnectionParameters(sourceMap.ConnectionString, sourceMap.ProviderType, sourceMap.SourceType)

                Catch ex As Exception

                    addErrMsg = ex.Message

                End Try

                If Len(addErrMsg) > 0 Then errMsg = addErrMsg & vbCrLf & vbCrLf & errMsg


                MsgBox("Error! Database could not be contacted! Please inspect your connection string!" & vbCrLf & vbCrLf & "Error: " & errMsg)

            End If

            Return False

        Else

            If Not quietSuccess Then

                MsgBox("Database Connection OK!")

            End If

            Return True

        End If

    End Function


    Private Sub Cut()

        If Not m_CurrentCopyTarget Is Nothing Then

            If m_CurrentCopyTarget.GetType Is GetType(SyntaxBoxControl) Then

                Clipboard.SetDataObject(CType(m_CurrentCopyTarget, SyntaxBoxControl).Selection.Text, False)

                CType(m_CurrentCopyTarget, SyntaxBoxControl).Selection.Text = ""

                SetCutCopyPasteEnabled()

                Exit Sub

            End If

        End If

        m_SelectedCopySource = m_CurrentCopyTarget
        m_CutFlag = True

        SetCutCopyPasteEnabled()

    End Sub


    Private Sub Copy()

        If Not m_CurrentCopyTarget Is Nothing Then

            If m_CurrentCopyTarget.GetType Is GetType(SyntaxBoxControl) Then

                Clipboard.SetDataObject(CType(m_CurrentCopyTarget, SyntaxBoxControl).Selection.Text, True)

                SetCutCopyPasteEnabled()

                Exit Sub

            End If

        End If

        m_SelectedCopySource = m_CurrentCopyTarget
        m_CutFlag = False

        SetCutCopyPasteEnabled()

    End Sub


    Private Sub Paste()

        If Not m_CurrentCopyTarget Is Nothing Then

            If m_CurrentCopyTarget.GetType Is GetType(SyntaxBoxControl) Then

                CType(m_CurrentCopyTarget, SyntaxBoxControl).Selection.Text = Clipboard.GetDataObject.GetData(GetType(String))

                SetCutCopyPasteEnabled()

                Exit Sub

            End If

        End If

        If Not m_SelectedCopySource Is Nothing Then

            PerformPaste()

        End If

        If m_CutFlag Then

            m_SelectedCopySource = Nothing

        End If

        m_CutFlag = False

        SetCutCopyPasteEnabled()

    End Sub

    Private Sub PerformPaste(Optional ByVal sourceObject As Object = Nothing, _
                                Optional ByVal targetObject As Object = Nothing)

        If sourceObject Is Nothing Then sourceObject = m_SelectedCopySource
        If targetObject Is Nothing Then targetObject = m_CurrentCopyTarget

        If sourceObject Is Nothing Then Exit Sub
        If targetObject Is Nothing Then Exit Sub

        If m_CutFlag Then

            PerformCut(sourceObject, targetObject)

        Else

            PerformCopy(sourceObject, targetObject)

        End If
    End Sub


    Private Sub PerformCut(Optional ByVal sourceObject As Object = Nothing, _
                    Optional ByVal targetObject As Object = Nothing)

        If sourceObject Is Nothing Then sourceObject = m_SelectedCopySource
        If targetObject Is Nothing Then targetObject = m_CurrentCopyTarget

        If sourceObject Is Nothing Then Exit Sub
        If targetObject Is Nothing Then Exit Sub

        Dim copy As IMap

        If sourceObject.GetType Is GetType(DomainMap) Then

            If targetObject.GetType Is GetType(ProjectModel.Project) Then

                'copy = CType(sourceObject, IMap).DeepClone

                'Todo: Add the new domain map

            End If

        ElseIf sourceObject.GetType Is GetType(ClassMap) Then

            If targetObject.GetType Is GetType(DomainMap) Then

                If CType(sourceObject, ClassMap).DomainMap Is targetObject Then Exit Sub

                CType(sourceObject, IMap).Name = GetCopyName(sourceObject, targetObject)

                CType(sourceObject, ClassMap).DomainMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(PropertyMap) Then

            If targetObject.GetType Is GetType(ClassMap) Then

                If CType(sourceObject, PropertyMap).ClassMap Is targetObject Then Exit Sub

                CType(sourceObject, IMap).Name = GetCopyName(sourceObject, targetObject)

                CType(sourceObject, PropertyMap).ClassMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(SourceMap) Then

            If targetObject.GetType Is GetType(DomainMap) Then

                If CType(sourceObject, SourceMap).DomainMap Is targetObject Then Exit Sub

                CType(sourceObject, IMap).Name = GetCopyName(sourceObject, targetObject)

                CType(sourceObject, SourceMap).DomainMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(TableMap) Then

            If targetObject.GetType Is GetType(SourceMap) Then

                If CType(sourceObject, TableMap).SourceMap Is targetObject Then Exit Sub

                CType(sourceObject, IMap).Name = GetCopyName(sourceObject, targetObject)

                CType(sourceObject, TableMap).SourceMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(ColumnMap) Then

            If targetObject.GetType Is GetType(TableMap) Then

                If CType(sourceObject, ColumnMap).TableMap Is targetObject Then Exit Sub

                CType(sourceObject, IMap).Name = GetCopyName(sourceObject, targetObject)

                CType(sourceObject, ColumnMap).TableMap = targetObject

            End If

        End If

        RefreshAll()

    End Sub

    Private Sub PerformCopy(Optional ByVal sourceObject As Object = Nothing, _
                    Optional ByVal targetObject As Object = Nothing)

        If sourceObject Is Nothing Then sourceObject = m_SelectedCopySource
        If targetObject Is Nothing Then targetObject = m_CurrentCopyTarget

        If sourceObject Is Nothing Then Exit Sub
        If targetObject Is Nothing Then Exit Sub

        Dim copy As IMap

        If sourceObject.GetType Is GetType(DomainMap) Then

            If targetObject.GetType Is GetType(ProjectModel.Project) Then

                'copy = CType(sourceObject, IMap).DeepClone

                'Todo: Add the new domain map

            End If

        ElseIf sourceObject.GetType Is GetType(ClassMap) Then

            If targetObject.GetType Is GetType(DomainMap) Then

                copy = CType(sourceObject, IMap).DeepClone

                copy.Name = GetCopyName(sourceObject, targetObject)

                CType(copy, ClassMap).DomainMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(PropertyMap) Then

            If targetObject.GetType Is GetType(ClassMap) Then

                copy = CType(sourceObject, IMap).DeepClone

                copy.Name = GetCopyName(sourceObject, targetObject)

                CType(copy, PropertyMap).ClassMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(SourceMap) Then

            If targetObject.GetType Is GetType(DomainMap) Then

                copy = CType(sourceObject, IMap).DeepClone

                copy.Name = GetCopyName(sourceObject, targetObject)

                CType(copy, SourceMap).DomainMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(TableMap) Then

            If targetObject.GetType Is GetType(SourceMap) Then

                copy = CType(sourceObject, IMap).DeepClone

                copy.Name = GetCopyName(sourceObject, targetObject)

                CType(copy, TableMap).SourceMap = targetObject

            End If

        ElseIf sourceObject.GetType Is GetType(ColumnMap) Then

            If targetObject.GetType Is GetType(TableMap) Then

                copy = CType(sourceObject, IMap).DeepClone

                copy.Name = GetCopyName(sourceObject, targetObject)

                CType(copy, ColumnMap).TableMap = targetObject

            End If

        End If

        RefreshAll()

    End Sub

    Private Function GetCopyName(ByVal sourceObject As Object, _
                    ByVal targetObject As Object) As String

        Dim existing As ArrayList
        Dim test As IMap
        Dim i As Long = 1
        Dim testName As String
        Dim baseName As String = CType(sourceObject, IMap).Name
        Dim namePrefix1 As String = "Copy "
        Dim namePrefix2 As String = "of "

        testName = baseName

        Dim foundName As Boolean = True

        Try

            If sourceObject.GetType Is GetType(DomainMap) Then

                'TODO:

            ElseIf sourceObject.GetType Is GetType(ClassMap) Then

                existing = CType(targetObject, DomainMap).ClassMaps

            ElseIf sourceObject.GetType Is GetType(PropertyMap) Then

                existing = CType(targetObject, ClassMap).GetAllPropertyMaps

            ElseIf sourceObject.GetType Is GetType(SourceMap) Then

                existing = CType(targetObject, DomainMap).SourceMaps

            ElseIf sourceObject.GetType Is GetType(TableMap) Then

                existing = CType(targetObject, SourceMap).TableMaps

            ElseIf sourceObject.GetType Is GetType(ColumnMap) Then

                existing = CType(targetObject, TableMap).ColumnMaps

            End If

            While foundName

                foundName = False

                For Each test In existing

                    If test.Name = testName Then

                        foundName = True

                        Exit For

                    End If

                Next

                If foundName Then

                    If i < 2 Then

                        testName = namePrefix1 & namePrefix2 & baseName

                    Else

                        testName = namePrefix1 & "(" & i & ") " & namePrefix2 & baseName

                    End If

                    i += 1

                End If

            End While

        Catch ex As Exception

        End Try

        Return testName

    End Function


    Private Sub Delete()

        If m_CurrentCopyTarget Is Nothing Then Exit Sub

        If m_CurrentCopyTarget.GetType Is GetType(ProjectModel.Project) Then

            CloseProject()

        ElseIf m_CurrentCopyTarget.GetType Is GetType(DomainMap) Then

            RemoveDomainFromProject(m_CurrentCopyTarget)

        ElseIf m_CurrentCopyTarget.GetType Is GetType(ClassMap) Then

            DeleteClass(m_CurrentCopyTarget)

        ElseIf m_CurrentCopyTarget.GetType Is GetType(PropertyMap) Then

            DeleteProperty(m_CurrentCopyTarget)

        ElseIf m_CurrentCopyTarget.GetType Is GetType(SourceMap) Then

            DeleteSource(m_CurrentCopyTarget)

        ElseIf m_CurrentCopyTarget.GetType Is GetType(TableMap) Then

            DeleteTable(m_CurrentCopyTarget)

        ElseIf m_CurrentCopyTarget.GetType Is GetType(ColumnMap) Then

            DeleteColumn(m_CurrentCopyTarget)

        ElseIf m_CurrentCopyTarget.GetType Is GetType(UmlDiagram) Then

            DeleteUmlDiagram(m_CurrentCopyTarget)

        End If

    End Sub

    Private Sub MakeSureAProjectExists()

        If m_Project Is Nothing Then

            NewProject("New Project")

        End If

    End Sub

    Private Sub OpenProject(Optional ByVal path As String = "")

        Dim resource As ProjectModel.IResource
        Dim domainMap As IDomainMap
        Dim projFolder As String
        Dim domPath As String

        Dim cnt As Integer

        If path = "" Then

            OpenFileDialog1.Multiselect = False
            OpenFileDialog1.Filter = "Project Files (*.omproj)|*.omproj|All files (*.*)|*.*"
            If OpenFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                Exit Sub

            End If

            path = OpenFileDialog1.FileName

        End If


        If Not path = "" Then

            If Not CloseProject() Then

                Exit Sub

            End If

            PushStatus("Opening project file...")

            Try

                m_Project = ProjectModel.Project.Load(path)

                m_ProjectLoadedFromPath = path
                m_Project.FilePath = path

                projFolder = New FileInfo(path).Directory.FullName

                mapTreeView.LoadProject(m_Project)

                If Not m_ApplicationSettings Is Nothing Then

                    m_ApplicationSettings.LastProjectPath = path

                End If


                For Each resource In m_Project.Resources

                    Select Case resource.ResourceType

                        Case ProjectModel.ResourceTypeEnum.XmlDomainMap

                            domPath = resource.FilePath

                            If Not File.Exists(domPath) Then

                                domPath = projFolder & "\" & resource.HintPath

                                If File.Exists(domPath) Then

                                    resource.FilePath = New FileInfo(domPath).FullName

                                    m_ProjectDirty = True

                                Else

                                    MsgBox("Could not find domain model file: " & resource.FilePath)

                                    domPath = ""

                                End If

                            End If

                            If Not domPath = "" Then


                                domainMap = OpenMap(domPath, True)

                                resource.SetResource(domainMap)


                                cnt += 1

                            End If

                    End Select

                Next

                labelExplorer.Text = m_Project.Name
                Me.Text = "  " & m_Project.Name

                EnableToolbar(True)
                SetCurrSaveObject(m_Project)

                mapTreeView.ExpandProject()

                m_ApplicationSettings.AddToLatestFiles(path)
                SaveAppSettings()


            Catch ex As Exception

                MsgBox("Could not load project!")

            End Try

            PullStatus()

        End If



        RefreshAll()

        VerifyMap()

    End Sub

    Private Sub NewProject(Optional ByVal name As String = "")

        If name = "" Then

            name = InputBox("Please enter a name for your new project.", "New Project")

        End If

        If name = "" Then Exit Sub

        If Not CloseProject() Then Exit Sub

        m_Project = New ProjectModel.Project

        m_Project.Name = name

        mapTreeView.LoadProject(m_Project)

        labelExplorer.Text = m_Project.Name
        Me.Text = "  " & m_Project.Name

        m_ProjectDirty = True
        m_ProjectLoadedFromPath = ""
        m_ProjectSavedToPath = ""

        EnableToolbar(True)
        SetCurrSaveObject(m_Project)

        RefreshAll()

    End Sub

    Private Sub NewFile()

    End Sub

    Private Function SaveProject(Optional ByVal justDirty As Boolean = False, Optional ByVal saveAs As Boolean = False) As Boolean

        Dim path As String
        Dim ok As Boolean

        If Not m_Project Is Nothing Then

            ok = True

            If justDirty Then

                If m_ProjectDirty Then

                    Select Case MsgBox("The project '" & GetProjectFileName() & "' contains unsaved changes!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.No

                            ok = False

                    End Select

                End If

            End If

            If ok Then

                If justDirty = False OrElse m_ProjectDirty Then

                    If Not saveAs Then

                        path = m_ProjectSavedToPath
                        If path = "" Then path = m_ProjectLoadedFromPath

                    End If

                    If path = "" Then

                        SaveFileDialog1.FileName = m_Project.Name & ".omproj"
                        SaveFileDialog1.Filter = "Project Files (*.omproj)|*.omproj|All files (*.*)|*.*"
                        If SaveFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                            Exit Function

                        End If

                        path = SaveFileDialog1.FileName

                    End If


                    If Not path = "" Then

                        Try

                            m_Project.Save(path)

                            m_ProjectSavedToPath = path
                            m_Project.FilePath = path

                            m_ProjectDirty = False


                            m_ApplicationSettings.AddToLatestFiles(path)
                            SaveAppSettings()

                        Catch ex As Exception

                            MsgBox("Could not save project!")

                            Return False

                        End Try

                    End If

                End If

            End If

        End If


        Return True

    End Function

    Private Function AddDomainToProject(ByVal domainMap As IDomainMap, Optional ByVal loadedFromPath As String = "") As Boolean

        MakeSureAProjectExists()

        Dim resource As ProjectModel.IResource
        Dim domainConfig As ProjectModel.DomainConfig

        If Not m_Project.GetDomainMap(domainMap.Name) Is Nothing Then

            MsgBox("A domain map with this name is already open in this project!")

            Exit Function

        End If

        resource = New ProjectModel.Resource(domainMap, loadedFromPath)

        domainConfig = New ProjectModel.DomainConfig
        domainConfig.Name = "Default configuration"
        domainConfig.IsActive = True

        domainConfig.ClassesToCodeConfig.TargetLanguage = m_ApplicationSettings.OptionSettings.SynchronizationSettings.DefaultTargetLanguage
        domainConfig.ClassesToCodeConfig.TargetLanguage = m_ApplicationSettings.OptionSettings.SynchronizationSettings.DefaultTargetMapper

        resource.Configs.Add(domainConfig)

        m_Project.Resources.Add(resource)

        m_ProjectDirty = True

        Return True

    End Function


    Private Function RemoveDomainFromProject(ByVal domainMap As IDomainMap) As Boolean

        Dim resource As ProjectModel.IResource
        Dim domainConfig As ProjectModel.DomainConfig

        Dim removeResources As New ArrayList

        If MsgBox("Are you sure that you want to remove the domain model '" & domainMap.Name & "' from this project?", MsgBoxStyle.YesNo, "Remove Domain Model") = MsgBoxResult.No Then

            Exit Function

        End If

        If domainMap.Dirty Then

            Select Case MsgBox("The content in the domain map '" & GetDomainMapFileName(domainMap) & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                Case MsgBoxResult.Cancel

                    Return False

                Case MsgBoxResult.Yes

                    SaveDomainMap(domainMap)

            End Select

        End If

        If Not m_Project Is Nothing Then

            If m_Project.GetDomainMap(domainMap.Name) Is Nothing Then

                MsgBox("No domain map with this name is open in this project!")

                Exit Function

            End If

            CloseXmlBehind(domainMap)

            CloseUmlDoc(domainMap)

            For Each resource In m_Project.Resources

                Select Case resource.ResourceType

                    Case ResourceTypeEnum.XmlDomainMap

                        If resource.Name = domainMap.Name Then

                            removeResources.Add(resource)

                        End If

                End Select

            Next

            For Each resource In removeResources

                m_Project.Resources.Remove(resource)

            Next


            mapPropertyGrid.OnObjectDelete(domainMap)

            mapListView.Items.Clear()

            m_ProjectDirty = True

            RefreshAll()

            VerifyMap()

            Return True

        End If

    End Function


    Private Function CloseProject() As Boolean

        If Not m_ActionType = ActionTypeEnum.None Then

            If Not m_SynchMode = SynchModeEnum.CodeToClasses Then

                DiscardSynch()

            End If

        End If

        If Not SaveAll(True) Then

            Return False

        End If

        If Not CloseAllDocs(True) Then

            Return False

        End If

        mapPropertyGrid.OnObjectDelete(m_Project, True)

        mapListView.Items.Clear()
        mapListView.ParentNode = Nothing


        m_Project = Nothing
        m_ProjectDirty = False
        m_ProjectLoadedFromPath = ""
        m_ProjectSavedToPath = ""

        mapTreeView.ClearAll()
        mapTreeViewPreview.ClearAll()

        m_CurrSaveObject = Nothing
        m_contextMenuDomainMap = Nothing
        m_contextMenuMapObject = Nothing
        m_contextMenuNamespace = Nothing
        m_contextMenuNode = Nothing
        m_contextMenuParentMap = Nothing
        m_CurrentCopyTarget = Nothing

        DisableToolbar()

        RefreshAll()

        Me.Text = PRODUCT_NAME
        Return True

    End Function

    Private Sub RefreshAll(Optional ByVal resetIcons As Boolean = False)

        mapTreeView.RefreshAllNodes(True)
        mapTreeViewPreview.RefreshAllNodes(True)
        mapPropertyGrid.RefreshProperties()

        RefreshListView()

        RefreshToolBar()

        VerifyMap()

        RefreshUml()

        Select Case m_SynchMode

            Case SynchModeEnum.TablesToSource

                RefreshSynchTablesToSourcePreviewDTD()

        End Select



    End Sub

    Private Sub RefreshToolBar()

        If m_Project Is Nothing Then

            toolBarButtonBack.Enabled = False
            toolBarButtonForward.Enabled = False
            toolBarButtonUp.Enabled = False

            toolBarButtonFind.Enabled = False

            toolBarButtonProperties.Enabled = False
            toolBarButtonXmlBehind.Enabled = False
            toolBarButtonRun.Enabled = False
            toolBarButtonUml.Enabled = False

            toolBarButtonExplorer.Enabled = False
            toolBarButtonTools.Enabled = False
            toolBarButtonList.Enabled = False

        Else

            toolBarButtonBack.Enabled = mapListView.CanGoBack
            toolBarButtonForward.Enabled = mapListView.CanGoForward
            toolBarButtonUp.Enabled = mapListView.CanGoUp


            If GetAllOpenDocs.Count < 1 Then

                toolBarButtonFind.Enabled = False

            Else

                toolBarButtonFind.Enabled = True

            End If

            toolBarButtonProperties.Enabled = True


            toolBarButtonExplorer.Enabled = True
            toolBarButtonTools.Enabled = True
            toolBarButtonList.Enabled = True

            toolBarButtonXmlBehind.Enabled = False
            toolBarButtonRun.Enabled = False
            toolBarButtonUml.Enabled = False

            If Not m_CurrSaveObject Is Nothing Then

                If m_CurrSaveObject.GetType Is GetType(UmlDiagram) Then

                    toolBarButtonXmlBehind.Enabled = True
                    toolBarButtonRun.Enabled = True
                    toolBarButtonUml.Enabled = True

                ElseIf m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                    toolBarButtonXmlBehind.Enabled = True
                    toolBarButtonRun.Enabled = True
                    toolBarButtonUml.Enabled = True

                End If

            End If

            'If Not m_contextMenuMapObject Is Nothing Then

            '    If m_contextMenuMapObject.GetType Is GetType(UmlDiagram) Then

            '        toolBarButtonXmlBehind.Enabled = True
            '        toolBarButtonUml.Enabled = True

            '        'ElseIf m_contextMenuMapObject.GetType Is GetType(DomainMap) Then

            '        '    toolBarButtonXmlBehind.Enabled = True
            '        '    toolBarButtonUml.Enabled = True

            '    End If

            'End If

            If Not m_CurrentCopyTarget Is Nothing Then

                If m_CurrentCopyTarget.GetType Is GetType(UmlDiagram) Then

                    toolBarButtonXmlBehind.Enabled = True
                    toolBarButtonRun.Enabled = True
                    toolBarButtonUml.Enabled = True

                    'ElseIf m_contextMenuMapObject.GetType Is GetType(DomainMap) Then

                    '    toolBarButtonXmlBehind.Enabled = True
                    '    toolBarButtonUml.Enabled = True

                End If

            End If

        End If

        SetCutCopyPasteEnabled()

        If MayPrint() Then

            toolBarButtonPrint.Enabled = True
            toolBarButtonPrintPreview.Enabled = True

        Else

            toolBarButtonPrint.Enabled = False
            toolBarButtonPrintPreview.Enabled = False

        End If

    End Sub

    Private Sub AddImportedDomainToProject(ByVal domainMap As IDomainMap)

        If AddDomainToProject(domainMap) Then

            domainMap = mapTreeView.LoadDomain(domainMap)

            If Not domainMap Is Nothing Then

                domainMap.Dirty = True

            End If

            EnableToolbar()
            SetCurrSaveObject(domainMap)

            RefreshAll()

            VerifyMap()

        End If

    End Sub

    Public Function NewMap(Optional ByVal name As String = "") As IDomainMap

        Dim domainMap As IDomainMap
        Dim sourceMap As ISourceMap

        If Len(name) < 1 Then

            name = InputBox("Please enter a name for your new domain model.", "New Domain")

        End If

        If Len(name) > 0 Then

            Try

                domainMap = New domainMap(name)

                If AddDomainToProject(domainMap) Then

                    domainMap = mapTreeView.LoadDomain(domainMap)

                    If Not domainMap Is Nothing Then

                        'mapTreeViewVerify.LoadDomain(domainMap)

                        'If m_DynamicCreateSource Then

                        'sourceMap = New sourceMap(name & "Source")
                        sourceMap = New sourceMap(name)

                        sourceMap.DomainMap = domainMap

                        domainMap.Source = sourceMap.Name

                        Dim newDiagram As UmlDiagram = AddNewDiagram(domainMap, name)

                        OpenUmlDoc()

                        'End If
                        domainMap.Dirty = True

                    End If

                    EnableToolbar()
                    SetCurrSaveObject(domainMap)

                End If

            Catch ex As Exception

                MsgBox("Could not create new domain map!")

            End Try

            RefreshAll()

            VerifyMap()

            Return domainMap

        End If

    End Function


    Public Function OpenMap(Optional ByVal fileName As String = "", Optional ByVal addedToProject As Boolean = False) As IDomainMap

        Dim domainMap As IDomainMap
        Dim fileNames As New ArrayList
        Dim ok As Boolean

        If fileName = "" Then

            OpenFileDialog1.Multiselect = True
            OpenFileDialog1.Filter = "NPersist files (*.npersist)|*.npersist|All files (*.*)|*.*"
            If OpenFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                Exit Function

            End If

            For Each fileName In OpenFileDialog1.FileNames

                fileNames.Add(fileName)

            Next

        Else

            fileNames.Add(fileName)

        End If

        PushStatus("Opening domain model file(s)...")

        Application.DoEvents()

        If Not fileNames.Count < 1 Then

            MakeSureAProjectExists()

        End If

        For Each fileName In fileNames

            Try

                domainMap = mapTreeView.LoadDomain(fileName, True)

                If Not domainMap Is Nothing Then

                    ok = True

                    If Not addedToProject Then

                        ok = AddDomainToProject(domainMap, fileName)

                    End If

                    If ok Then

                        mapTreeView.LoadDomain(domainMap)

                    End If

                End If

                EnableToolbar()
                SetCurrSaveObject(domainMap)

            Catch ex As Exception

                MsgBox("Could not load domain map from path " & fileName & "!")

            End Try

        Next

        PullStatus()

        RefreshAll()

        VerifyMap()

        Return domainMap

    End Function

    Public Function Open(Optional ByVal fileNames As ArrayList = Nothing) As IDomainMap

        Dim domainMap As IDomainMap
        Dim fileName As String
        Dim fileInfo As System.IO.FileInfo
        Dim cntProj As Integer

        Dim msg As String
        Dim cnt As Long


        If fileNames Is Nothing Then

            fileNames = New ArrayList

            OpenFileDialog1.Multiselect = True
            OpenFileDialog1.Filter = "NPersist files (*.npersist)|*.npersist|Project Files (*.omproj)|*.omproj|All files (*.*)|*.*"
            If OpenFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                Exit Function

            End If

            For Each fileName In OpenFileDialog1.FileNames

                fileNames.Add(fileName)

            Next

        End If

        PushStatus("Opening selected files...")

        Application.DoEvents()

        For Each fileName In fileNames

            fileInfo = New fileInfo(fileName)

            If fileInfo.Exists Then

                Select Case LCase(fileInfo.Extension)

                    Case ".omproj"

                        cntProj += 1

                        If cntProj > 1 Then

                            MsgBox("Can't open more than one project!")

                        End If

                    Case ".npersist", ".npe"

                        cnt += 1

                    Case Else

                        MsgBox("Can't open " & fileInfo.Extension & " files, only .omproj and .npersist files!")

                End Select

            End If

        Next

        'Open eventual project
        For Each fileName In fileNames

            fileInfo = New fileInfo(fileName)

            If fileInfo.Exists Then

                Select Case fileInfo.Extension

                    Case ".omproj"

                        OpenProject(fileName)

                End Select

            End If

        Next

        'Open domain maps and other files
        For Each fileName In fileNames

            fileInfo = New fileInfo(fileName)

            If fileInfo.Exists Then

                Select Case LCase(fileInfo.Extension)

                    Case ".npersist", ".npe"

                        OpenMap(fileName)

                    Case Else

                End Select

            End If

        Next

        PullStatus()

        VerifyMap()

        Return domainMap

    End Function

    Private Sub CloseSelectedFile()

        If Not m_CurrSaveObject Is Nothing Then

            If m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                'RemoveDomainFromProject(m_CurrSaveObject)

            ElseIf m_CurrSaveObject.GetType Is GetType(ProjectModel.Project) Then

                'CloseProject()

            ElseIf m_CurrSaveObject.GetType Is GetType(UserDocTabPage) Then

                CloseUserDoc(m_CurrSaveObject)

            Else

            End If

        End If

    End Sub


    Private Sub Save()

        If Not m_CurrSaveObject Is Nothing Then

            If m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                SaveDomainMap(m_CurrSaveObject)

            ElseIf m_CurrSaveObject.GetType Is GetType(UserDocTabPage) Then

                SaveUserDoc(m_CurrSaveObject)

            Else

            End If

        End If

    End Sub

    Public Function SaveAll(Optional ByVal justDirty As Boolean = False) As Boolean

        If Not SaveAllDomainMaps(justDirty) Then Return False

        If Not SaveAllUserDocs(justDirty) Then Return False

        If Not SaveAllPreviewDocs(justDirty) Then Return False

        If Not SaveProject(justDirty) Then Return False

        SaveAppSettings()

        Return True

    End Function


    Private Sub VerifyMap(Optional ByVal closeErrorNodes As Boolean = False, Optional ByVal closeOKNodes As Boolean = True, Optional ByVal forceVerify As Boolean = False)

        m_IsVerifying = True

        If m_ApplicationSettings.OptionSettings.VerificationSettings.AutoVerify Or forceVerify Then

            ClearErrorMsgs()

            If mapTreeView.VerifyNew(m_Project, m_ApplicationSettings.OptionSettings.VerificationSettings.VerifyMappings) Then

                '

            End If

            mapTreeView.RefreshAllNodes(True)

        End If

        m_IsVerifying = False

    End Sub


    Private Sub OldVerifyMap(Optional ByVal closeErrorNodes As Boolean = False, Optional ByVal closeOKNodes As Boolean = True, Optional ByVal forceVerify As Boolean = False)

        'm_IsVerifying = True

        'textHideVerify.Visible = True

        'If forceVerify Then

        '    If Not tabControlTools.SelectedIndex = 0 Then

        '        tabControlTools.SelectedIndex = 0

        '    End If

        'End If

        'If tabControlTools.SelectedIndex = 0 Then

        '    If m_ApplicationSettings.OptionSettings.VerificationSettings.AutoVerify Or forceVerify Then

        '        ClearErrorMsgs()

        '        If mapTreeView.VerifyNew(m_Project, m_ApplicationSettings.OptionSettings.VerificationSettings.VerifyMappings) Then

        '            '

        '        End If

        '        mapTreeView.RefreshAllNodes(True)

        '    End If

        'End If

        'textHideVerify.Visible = False

        'm_IsVerifying = False

    End Sub



    Private Sub SaveAs()

        If Not m_CurrSaveObject Is Nothing Then

            If m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                SaveDomainMap(m_CurrSaveObject, True)

            ElseIf m_CurrSaveObject.GetType Is GetType(UserDocTabPage) Then

                SaveUserDoc(m_CurrSaveObject, True)

            Else

            End If

        End If

    End Sub

    Private Sub OpenOptions()

        Dim frm As New frmOptions

        frm.LoadOptions(m_ApplicationSettings)

        frm.ShowDialog(Me)

    End Sub

    Private Sub AddNewConfig(ByVal domainMap As IDomainMap)

        Dim name As String = "New Configuration"
        Dim resource As IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

        Dim cfg As New DomainConfig

        cfg.Name = name

        If resource.Configs.Count > 0 Then

            cfg.IsActive = False

        Else

            cfg.IsActive = True

        End If

        resource.Configs.Add(cfg)

        m_ProjectDirty = True

        RefreshAll()

    End Sub


    Private Sub SetActiveConfig(ByVal cfg As DomainConfig, ByVal domainMap As IDomainMap)

        Dim name As String = "New Configuration"
        Dim resource As IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

        resource.SetActiveConfig(cfg)

        m_ProjectDirty = True

        RefreshAll()

    End Sub

    Private Sub DeleteConfig(ByVal cfg As DomainConfig, ByVal domainMap As IDomainMap)

        If MsgBox("Do you really want to delete the  configuration '" & cfg.Name & "'?", MsgBoxStyle.YesNo, "Delete Configuration") = MsgBoxResult.No Then Exit Sub

        Dim resource As IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

        resource.Configs.Remove(cfg)

        mapPropertyGrid.OnObjectDelete(cfg)

        m_ProjectDirty = True

        RefreshAll()

    End Sub

    Private Function AddNewDiagram(ByVal domainMap As IDomainMap, Optional ByVal name As String = "") As UmlDiagram

        If name.Length < 1 Then

            name = InputBox("Please enter the name for your new diagram.", "Add Diagram")

        End If

        If Len(name) < 1 Then Exit Function

        Dim resource As IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

        Dim diagram As New UmlDiagram

        diagram.Name = name

        diagram.OwnerResource = resource

        m_ProjectDirty = True

        ViewUmlDiagram(diagram)

        RefreshAll()

        Return diagram

    End Function

    Private Sub DeleteSourceCodeFile(ByVal src As SourceCodeFile, ByVal cfg As ClassesToCodeConfig, Optional ByVal noAsk As Boolean = False)

        If MsgBox("Are you sure that you want to remove the file '" & src.FilePath & "' from this configuration (physical file will not be deleted!)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then

            Exit Sub

        End If

        Dim fileInfo As New fileInfo(src.FilePath)

        If fileInfo.Exists Then

            fileInfo.Delete()

        End If

        mapPropertyGrid.OnObjectDelete(src)

        RemoveSourceCodeFile(src, cfg, True)

    End Sub

    Private Sub RemoveSourceCodeFile(ByVal src As SourceCodeFile, ByVal cfg As ClassesToCodeConfig, Optional ByVal noAsk As Boolean = False)

        If Not noAsk Then

            If MsgBox("Are you sure that you want to remove the file '" & src.FilePath & "' from this configuration (physical file will not be deleted!)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then

                Exit Sub

            End If

        End If

        cfg.SourceCodeFiles.Remove(src)

        mapPropertyGrid.OnObjectDelete(src)

        RefreshAll()

    End Sub

    Private Function MayPaste(Optional ByVal sourceObject As Object = Nothing, _
                            Optional ByVal targetObject As Object = Nothing) As Boolean

        If targetObject Is Nothing Then targetObject = m_CurrentCopyTarget

        If targetObject Is Nothing Then Return False

        If targetObject.GetType Is GetType(SyntaxBoxControl) Then

            If Clipboard.GetDataObject.GetDataPresent(GetType(String)) Then

                Return True

            End If

        End If

        If sourceObject Is Nothing Then sourceObject = m_SelectedCopySource
        If sourceObject Is Nothing Then Return False

        If sourceObject.GetType Is GetType(DomainMap) Then

            If targetObject.GetType Is GetType(ProjectModel.Project) Then

                Return True

            End If

        ElseIf sourceObject.GetType Is GetType(ClassMap) Then

            If targetObject.GetType Is GetType(DomainMap) Then

                Return True

            End If

        ElseIf sourceObject.GetType Is GetType(PropertyMap) Then

            If targetObject.GetType Is GetType(ClassMap) Then

                Return True

            End If

        ElseIf sourceObject.GetType Is GetType(SourceMap) Then

            If targetObject.GetType Is GetType(DomainMap) Then

                Return True

            End If

        ElseIf sourceObject.GetType Is GetType(TableMap) Then

            If targetObject.GetType Is GetType(SourceMap) Then

                Return True

            End If

        ElseIf sourceObject.GetType Is GetType(ColumnMap) Then

            If targetObject.GetType Is GetType(TableMap) Then

                Return True

            End If

        End If

        Return False

    End Function

    Private Sub SelectAll()

        Dim doc As UserDocTabPage = GetCurrentOpenDoc()

        If doc Is Nothing Then Exit Sub

        doc.TextBox.SelectAll()

    End Sub

    Private Sub FindAndReplace()

        If m_frmSearchReplace Is Nothing Then

            m_frmSearchReplace = New frmSearchReplace

        End If

        m_frmSearchReplace.OpenForm(Me)

        If Not m_frmSearchReplace.Visible Then

            m_frmSearchReplace.Show()

        End If

    End Sub

#End Region

#Region " Design and visibility "


    Public Sub ShowExplorer()

        If Not panelLeft.Visible Then

            ToggleProjectExplorerVisibility()

            SaveVisibilitySettings()

        End If

        mapTreeView.Focus()

    End Sub

    Public Sub ShowTools()

        If Not panelRight.Visible Then

            ToggleToolsVisibility()

            SaveVisibilitySettings()

        End If


        tabControlTools.Focus()

    End Sub

    Public Sub ShowMain()

        If Not panelMain.Visible Then

            ToggleMainVisibility()

            SaveVisibilitySettings()

        End If


        tabControlDocuments.Focus()

    End Sub


    Public Sub ShowList()

        If Not panelBottom.Visible Then

            ToggleMsgListVisibility()

            SaveVisibilitySettings()

        End If

        'tabControlMessages.SelectedIndex = 3
        tabControlMessages.SelectedTab = tabPageMessageList

        mapListView.Focus()

    End Sub



    Public Sub ShowSynchTree()

        ShowTools()

        tabControlTools.SelectedTab = tabToolsPreview

    End Sub

    Public Sub ShowUmlDocs()

        ShowMain()

        tabControlDocuments.SelectedTab = tabPageMainUml

    End Sub

#End Region

#Region " Adding And Deleting "


    Public Sub DeleteUmlDiagram(ByVal umlDiagram As UmlDiagram)

        If MsgBox("Do you really want to delete the diagram '" & umlDiagram.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Diagram") = MsgBoxResult.Cancel Then Exit Sub

        'mapPropertyGrid.OnObjectDelete(umlDiagram)
        mapPropertyGrid.OnObjectDelete(umlDiagram, True)

        CloseUmlDoc(umlDiagram)

        MapServices.DeleteUmlDiagram(umlDiagram)

        RefreshAll()

    End Sub



    Private Sub DeleteNamespace(ByVal domainMap As IDomainMap, ByVal namespaceToDelete As String)

        If MsgBox("Do you really want to delete the namespace '" & namespaceToDelete & "'?.", MsgBoxStyle.OKCancel, "Delete Class") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeleteNamespace(domainMap, namespaceToDelete)

        m_ProjectDirty = True

        RefreshAll()

    End Sub

    Private Sub AddNewClassToDiagram(ByVal diagram As UmlDiagram, ByVal doc As UserDocTabPage)

        If doc Is Nothing Then Exit Sub

        Dim domainMap As IDomainMap = diagram.GetDomainMap

        Dim classMap As IClassMap

        If Not domainMap Is Nothing Then

            classMap = AddClass(domainMap)

            If Not classMap Is Nothing Then

                AddClassToUmlDoc(classMap, Point.Empty, doc)

            End If

        End If

    End Sub

    Private Function AddClass(ByVal domainMap As IDomainMap, Optional ByVal addToNamespace As String = "") As IClassMap

        Dim name As String = InputBox("Please enter the name for your new class.", "Add Class")
        Dim tableName As String
        Dim propertyMap As IPropertyMap
        Dim columnMap As IColumnMap

        If Len(name) < 1 Then Exit Function

        If Len(addToNamespace) > 0 Then name = addToNamespace & "." & name

        Dim classMap As IClassMap = MapServices.CreateClassMap(name, domainMap)


        If m_DynamicCreateSource Then

            If Not classMap.GetSourceMap Is Nothing Then

                SetupClassesToTables(domainMap)

                tableName = m_ClassesToTables.GetTableNameForClass(classMap)
                MapServices.CreateTableMap(tableName, classMap.GetSourceMap)
                classMap.Table = tableName

            End If

        End If

        If m_DynamicCreateIdProperty Then

            propertyMap = AddProperty(classMap, "Id", "System.Int32")

            If Not propertyMap Is Nothing Then

                propertyMap.IsIdentity = True
                propertyMap.IsNullable = False
                propertyMap.DataType = "System.Int32"
                propertyMap.IsAssignedBySource = True

                If m_DynamicCreateSource Then

                    columnMap = propertyMap.GetColumnMap

                    If Not columnMap Is Nothing Then

                        columnMap.IsAutoIncrease = True
                        columnMap.Seed = 1
                        columnMap.Increment = 1
                        columnMap.IsPrimaryKey = True
                        columnMap.AllowNulls = False
                        columnMap.DataType = DbType.Int32
                        columnMap.Length = "4"

                    End If

                End If

            End If

        End If


        RefreshAll()

        Return classMap

    End Function

    Private Function AddInterface(ByVal domainMap As IDomainMap, Optional ByVal addToNamespace As String = "") As IClassMap

        Dim name As String = InputBox("Please enter the name for your new interface.", "Add Interface")

        If Len(name) < 1 Then Exit Function

        If Len(addToNamespace) > 0 Then name = addToNamespace & "." & name

        Dim classMap As IClassMap = MapServices.CreateClassMap(name, domainMap)

        classMap.ClassType = ClassType.Interface

        RefreshAll()

        Return classMap

    End Function

    Private Function AddStruct(ByVal domainMap As IDomainMap, Optional ByVal addToNamespace As String = "") As IClassMap

        Dim name As String = InputBox("Please enter the name for your new struct.", "Add Struct")

        If Len(name) < 1 Then Exit Function

        If Len(addToNamespace) > 0 Then name = addToNamespace & "." & name

        Dim classMap As IClassMap = MapServices.CreateClassMap(name, domainMap)

        classMap.ClassType = ClassType.Struct

        RefreshAll()

        Return classMap

    End Function

    Private Function AddEnum(ByVal domainMap As IDomainMap, Optional ByVal addToNamespace As String = "") As IClassMap

        Dim name As String = InputBox("Please enter the name for your new enumeration.", "Add Enumeration")

        If Len(name) < 1 Then Exit Function

        If Len(addToNamespace) > 0 Then name = addToNamespace & "." & name

        Dim classMap As IClassMap = MapServices.CreateClassMap(name, domainMap)

        classMap.ClassType = ClassType.Enum

        RefreshAll()

        Return classMap

    End Function


    Private Sub DeleteClass(ByVal classMap As IClassMap)

        If MsgBox("Do you really want to delete the class '" & classMap.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Class") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeleteClassMap(classMap)

        mapPropertyGrid.OnObjectDelete(classMap)

        RefreshAll()

    End Sub

    Private Function AddProperty(ByVal classMap As IClassMap, Optional ByVal name As String = "", Optional ByVal dataType As String = "") As IPropertyMap

        Dim columnName As String
        Dim columnMap As IColumnMap

        If Len(name) < 1 Then

            name = InputBox("Please enter the name for your new property.", "Add Property")

        End If

        If Len(name) < 1 Then Exit Function

        If InStr(name, ":") > 0 Then

            Dim arr() As String = Split(name, ":", 2)

            name = Trim(arr(0))
            dataType = Trim(arr(1))

        End If

        Dim isCollection As Boolean = False
        Dim maxLength As String = ""
        Dim minLength As String = ""
        Dim isNullable = False

        If dataType.EndsWith("?") Then

            dataType = dataType.Substring(0, Len(dataType) - 1)

            isNullable = True

        End If

        If dataType.EndsWith("]") Then

            If Not dataType = "System.Byte[]" Then

                dataType = dataType.Substring(0, Len(dataType) - 1)

                Dim arr() As String = Split(dataType, "[")

                dataType = arr(0)
                Dim inside As String = Trim(arr(1))

                If (Len(inside)) > 0 Then

                    If InStr(inside, "-") > 0 Then

                        Dim arr2() As String = Split(inside, "-")
                        Dim val As String = Trim(arr2(0))
                        If IsNumeric(val) Then
                            minLength = val
                        End If

                        val = Trim(arr2(1))
                        If IsNumeric(val) Then
                            maxLength = val
                        End If

                    Else

                        If IsNumeric(inside) Then
                            minLength = inside
                            maxLength = inside
                        End If

                    End If

                End If

                isCollection = True

            End If

        End If


        If dataType.EndsWith(")") Then

            If InStr(dataType, "(") > 0 Then

                dataType = dataType.Substring(0, Len(dataType) - 1)

                Dim arr() As String = Split(dataType, "(")

                dataType = arr(0)
                maxLength = arr(1)

            End If

        End If


        Select Case LCase(dataType)

            Case "int16"
                dataType = "System.Int16"

            Case "int", "int32", "integer"
                dataType = "System.Int32"

            Case "int64"
                dataType = "System.Int64"

            Case "bool", "boolean"
                dataType = "System.Boolean"

            Case "string", ""
                dataType = "System.String"

            Case "datetime"
                dataType = "System.DateTime"

            Case "guid"
                dataType = "System.Guid"

            Case "byte"
                dataType = "System.Byte"

            Case "byte[]", "byte()", "system.byte()"
                dataType = "System.Byte[]"

            Case "char"
                dataType = "System.Char"
            Case "decimal", "dec"
                dataType = "System.Decimal"
            Case "single", "sng"
                dataType = "System.Single"
            Case "double", "dbl"
                dataType = "System.Double"

        End Select

        If Len(dataType) < 1 Then

            dataType = "System.String"

            'dataType = InputBox("Please enter the data type for your new property.", "Add Property")

        End If

        If Len(dataType) < 1 Then Exit Function



        Dim propertyMap As IPropertyMap = MapServices.CreatePropertyMap(name, classMap)

        propertyMap.IsCollection = isCollection

        If Not propertyMap.IsCollection Then

            propertyMap.DataType = dataType

        Else

            propertyMap.ItemType = dataType

        End If

        If maxLength.Length > 0 Then

            propertyMap.MaxLength = maxLength

        End If

        If minLength.Length > 0 Then

            propertyMap.MinLength = minLength

        End If

        propertyMap.IsNullable = isNullable

        If HasReferenceDataType(propertyMap) Then
            propertyMap.IsNullable = True
            If propertyMap.ReferenceType = ReferenceType.None Then
                If propertyMap.IsCollection Then
                    propertyMap.ReferenceType = ReferenceType.ManyToMany
                Else
                    propertyMap.ReferenceType = ReferenceType.OneToMany
                End If
            End If
            DialogueServices.AskToCreateIfInverseNotExist(propertyMap.Inverse, propertyMap)
        Else
            If Not propertyMap.ReferenceType = ReferenceType.None Then
                propertyMap.ReferenceType = ReferenceType.None
            End If
        End If

        If m_DynamicCreateSource Then

            If Not classMap.GetTableMap Is Nothing Then

                SetupClassesToTables(classMap.DomainMap)

                columnName = m_ClassesToTables.GetColumnNameForProperty(propertyMap)
                columnMap = MapServices.CreateColumnMap(columnName, classMap.GetTableMap)
                propertyMap.Column = columnName

                columnMap.DataType = m_ClassesToTables.GetColumnTypeForProperty(propertyMap)
                columnMap.Length = m_ClassesToTables.GetColumnLengthForProperty(propertyMap)

            End If

        End If

        RefreshAll()

        Return propertyMap

    End Function


    Private Function AddEnumValue(ByVal classMap As IClassMap, Optional ByVal name As String = "") As IEnumValueMap

        Dim columnName As String
        Dim columnMap As IColumnMap

        If Len(name) < 1 Then

            name = InputBox("Please enter the name for your new enumeration value.", "Add Enumeration Value")

        End If

        If Len(name) < 1 Then Exit Function

        Dim enumValueMap As IEnumValueMap = MapServices.CreateEnumValueMap(name, classMap)

        RefreshAll()

        Return enumValueMap

    End Function



    Private Sub DeleteProperty(ByVal propertyMap As IPropertyMap)

        If MsgBox("Do you really want to delete the property '" & propertyMap.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Property") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeletePropertyMap(propertyMap)

        mapPropertyGrid.OnObjectDelete(propertyMap)

        RefreshAll()

    End Sub

    Private Sub DeleteEnumValue(ByVal enumValueMap As IEnumValueMap)

        If MsgBox("Do you really want to delete the enumeration value '" & enumValueMap.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Enumeration Value") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeleteEnumValueMap(enumValueMap)

        mapPropertyGrid.OnObjectDelete(enumValueMap)

        RefreshAll()

    End Sub

    Private Sub AddSource(ByVal domainMap As IDomainMap)


        Dim name As String = InputBox("Please enter the name for your new data source.", "Add Source")

        If Len(name) < 1 Then Exit Sub

        MapServices.CreateSourceMap(name, domainMap)

        RefreshAll()

    End Sub

    Private Sub DeleteSource(ByVal sourceMap As ISourceMap)

        If MsgBox("Do you really want to delete the data source '" & sourceMap.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Source") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeleteSourceMap(sourceMap)

        mapPropertyGrid.OnObjectDelete(sourceMap)

        RefreshAll()

    End Sub

    Private Sub AddTable(ByVal sourceMap As ISourceMap)

        Dim name As String = InputBox("Please enter the name for your new table.", "Add Table")

        If Len(name) < 1 Then Exit Sub

        MapServices.CreateTableMap(name, sourceMap)

        RefreshAll()

    End Sub

    Private Sub DeleteTable(ByVal tableMap As ITableMap)

        If MsgBox("Do you really want to delete the table '" & tableMap.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Table") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeleteTableMap(tableMap)

        mapPropertyGrid.OnObjectDelete(tableMap)

        RefreshAll()

    End Sub

    Private Sub AddColumn(ByVal tableMap As ITableMap)

        Dim name As String = InputBox("Please enter the name for your new column.", "Add Column")

        If Len(name) < 1 Then Exit Sub

        MapServices.CreateColumnMap(name, tableMap)

        RefreshAll()

    End Sub

    Private Sub DeleteColumn(ByVal columnMap As IColumnMap)

        If MsgBox("Do you really want to delete the column '" & columnMap.Name & "'?.", MsgBoxStyle.OKCancel, "Delete Column") = MsgBoxResult.Cancel Then Exit Sub

        MapServices.DeleteColumnMap(columnMap)

        mapPropertyGrid.OnObjectDelete(columnMap)

        RefreshAll()

    End Sub

#End Region

#Region " Code generation "

#Region " Setups "


    Private Sub SetupPreviewCode(ByVal domainMap As IDomainMap)

        Dim resource As ProjectModel.IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)
        Dim cfg As ProjectModel.DomainConfig

        If Not resource Is Nothing Then

            For Each cfg In resource.Configs

                If cfg.IsActive Then

                    With cfg.ClassesToCodeConfig

                        m_ClassesToCodeCs.ImplementIInterceptable = .ImplementIInterceptable
                        m_ClassesToCodeVb.ImplementIInterceptable = .ImplementIInterceptable
                        m_ClassesToCodeDelphi.ImplementIInterceptable = .ImplementIInterceptable

                        m_ClassesToCodeCs.ImplementIObjectHelper = .ImplementIObjectHelper
                        m_ClassesToCodeVb.ImplementIObjectHelper = .ImplementIObjectHelper
                        m_ClassesToCodeDelphi.ImplementIObjectHelper = .ImplementIObjectHelper

                        m_ClassesToCodeCs.ImplementIObservable = .ImplementIObservable
                        m_ClassesToCodeVb.ImplementIObservable = .ImplementIObservable
                        m_ClassesToCodeDelphi.ImplementIObservable = .ImplementIObservable

                        m_ClassesToCodeCs.IncludeComments = .IncludeComments
                        m_ClassesToCodeVb.IncludeComments = .IncludeComments
                        m_ClassesToCodeDelphi.IncludeComments = .IncludeComments

                        m_ClassesToCodeCs.IncludeDocComments = .IncludeDocComments
                        m_ClassesToCodeVb.IncludeDocComments = .IncludeDocComments
                        m_ClassesToCodeDelphi.IncludeDocComments = .IncludeDocComments

                        m_ClassesToCodeCs.IncludeModelInfoInDocComments = .IncludeModelInfoInDocComments
                        m_ClassesToCodeVb.IncludeModelInfoInDocComments = .IncludeModelInfoInDocComments
                        m_ClassesToCodeDelphi.IncludeModelInfoInDocComments = .IncludeModelInfoInDocComments

                        m_ClassesToCodeCs.IncludeRegions = .IncludeRegions
                        m_ClassesToCodeVb.IncludeRegions = .IncludeRegions
                        m_ClassesToCodeDelphi.IncludeRegions = .IncludeRegions

                        m_ClassesToCodeCs.NotifyAfterGet = .NotifyAfterGet
                        m_ClassesToCodeVb.NotifyAfterGet = .NotifyAfterGet
                        m_ClassesToCodeDelphi.NotifyAfterGet = .NotifyAfterGet

                        m_ClassesToCodeCs.NotifyAfterSet = .NotifyAfterSet
                        m_ClassesToCodeVb.NotifyAfterSet = .NotifyAfterSet
                        m_ClassesToCodeDelphi.NotifyAfterSet = .NotifyAfterSet

                        m_ClassesToCodeCs.NotifyLightWeight = .NotifyLightWeight
                        m_ClassesToCodeVb.NotifyLightWeight = .NotifyLightWeight
                        m_ClassesToCodeDelphi.NotifyLightWeight = .NotifyLightWeight

                        m_ClassesToCodeCs.NotifyOnlyWhenRequired = .NotifyOnlyWhenRequired
                        m_ClassesToCodeVb.NotifyOnlyWhenRequired = .NotifyOnlyWhenRequired
                        m_ClassesToCodeDelphi.NotifyOnlyWhenRequired = .NotifyOnlyWhenRequired

                        m_ClassesToCodeCs.ImplementShadows = .ImplementShadows
                        m_ClassesToCodeVb.ImplementShadows = .ImplementShadows
                        m_ClassesToCodeDelphi.ImplementShadows = .ImplementShadows

                        m_ClassesToCodeCs.AddPropertyNotifyMethods = .AddPropertyNotifyMethods
                        m_ClassesToCodeVb.AddPropertyNotifyMethods = .AddPropertyNotifyMethods
                        m_ClassesToCodeDelphi.AddPropertyNotifyMethods = .AddPropertyNotifyMethods

                        m_ClassesToCodeCs.GeneratePOCO = .GeneratePOCO
                        m_ClassesToCodeVb.GeneratePOCO = .GeneratePOCO
                        m_ClassesToCodeDelphi.GeneratePOCO = .GeneratePOCO

                        m_ClassesToCodeCs.ReallyNoRegions = .ReallyNoRegions
                        m_ClassesToCodeVb.ReallyNoRegions = .ReallyNoRegions
                        m_ClassesToCodeDelphi.ReallyNoRegions = .ReallyNoRegions

                        m_ClassesToCodeCs.UseTypedCollections = .UseTypedCollections
                        m_ClassesToCodeVb.UseTypedCollections = .UseTypedCollections
                        m_ClassesToCodeDelphi.UseTypedCollections = .UseTypedCollections

                        m_ClassesToCodeCs.WrapCollections = .WrapCollections
                        m_ClassesToCodeVb.WrapCollections = .WrapCollections
                        m_ClassesToCodeDelphi.WrapCollections = .WrapCollections

                        m_ClassesToCodeCs.XmlFilePerClass = .XmlFilePerClass
                        m_ClassesToCodeVb.XmlFilePerClass = .XmlFilePerClass
                        m_ClassesToCodeDelphi.XmlFilePerClass = .XmlFilePerClass

                        m_ClassesToCodeCs.UseGenericCollections = .UseGenericCollections
                        m_ClassesToCodeVb.UseGenericCollections = .UseGenericCollections
                        m_ClassesToCodeDelphi.UseGenericCollections = .UseGenericCollections

                        m_ClassesToCodeCs.EmbedXml = .EmbedXml
                        m_ClassesToCodeVb.EmbedXml = .EmbedXml
                        m_ClassesToCodeDelphi.EmbedXml = .EmbedXml

                        Select Case .TargetMapper

                            Case TargetMapperEnum.POCO

                                m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.POCO
                                m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.POCO
                                m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.POCO

                            Case TargetMapperEnum.NPersist

                                m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.NPersist
                                m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.NPersist
                                m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.NPersist

                            Case TargetMapperEnum.NHibernate

                                m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.NHibernate
                                m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.NHibernate
                                m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.NHibernate

                                'Case TargetMapperEnum.ECO2

                                '    m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.ECO2
                                '    m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.ECO2
                                '    m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.ECO2


                                'Case TargetMapperEnum.WilsonORMapper

                                '    m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.WilsonORMapper
                                '    m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.WilsonORMapper
                                '    m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.WilsonORMapper

                        End Select


                    End With

                    Exit For

                End If

            Next

        End If

    End Sub

    Private Sub SetupGenerateCode(ByVal domainMap As IDomainMap)

        Dim resource As ProjectModel.IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)
        Dim cfg As ProjectModel.DomainConfig

        If Not resource Is Nothing Then

            For Each cfg In resource.Configs

                If cfg.IsActive Then

                    With cfg.ClassesToCodeConfig

                        m_ClassesToCodeCs.ImplementIInterceptable = .ImplementIInterceptable
                        m_ClassesToCodeVb.ImplementIInterceptable = .ImplementIInterceptable
                        m_ClassesToCodeDelphi.ImplementIInterceptable = .ImplementIInterceptable

                        m_ClassesToCodeCs.ImplementIObjectHelper = .ImplementIObjectHelper
                        m_ClassesToCodeVb.ImplementIObjectHelper = .ImplementIObjectHelper
                        m_ClassesToCodeDelphi.ImplementIObjectHelper = .ImplementIObjectHelper

                        m_ClassesToCodeCs.ImplementIObservable = .ImplementIObservable
                        m_ClassesToCodeVb.ImplementIObservable = .ImplementIObservable
                        m_ClassesToCodeDelphi.ImplementIObservable = .ImplementIObservable

                        m_ClassesToCodeCs.IncludeComments = .IncludeComments
                        m_ClassesToCodeVb.IncludeComments = .IncludeComments
                        m_ClassesToCodeDelphi.IncludeComments = .IncludeComments

                        m_ClassesToCodeCs.IncludeDocComments = .IncludeDocComments
                        m_ClassesToCodeVb.IncludeDocComments = .IncludeDocComments
                        m_ClassesToCodeDelphi.IncludeDocComments = .IncludeDocComments

                        m_ClassesToCodeCs.IncludeModelInfoInDocComments = .IncludeModelInfoInDocComments
                        m_ClassesToCodeVb.IncludeModelInfoInDocComments = .IncludeModelInfoInDocComments
                        m_ClassesToCodeDelphi.IncludeModelInfoInDocComments = .IncludeModelInfoInDocComments

                        m_ClassesToCodeCs.IncludeRegions = .IncludeRegions
                        m_ClassesToCodeVb.IncludeRegions = .IncludeRegions
                        m_ClassesToCodeDelphi.IncludeRegions = .IncludeRegions

                        m_ClassesToCodeCs.NotifyAfterGet = .NotifyAfterGet
                        m_ClassesToCodeVb.NotifyAfterGet = .NotifyAfterGet
                        m_ClassesToCodeDelphi.NotifyAfterGet = .NotifyAfterGet

                        m_ClassesToCodeCs.NotifyAfterSet = .NotifyAfterSet
                        m_ClassesToCodeVb.NotifyAfterSet = .NotifyAfterSet
                        m_ClassesToCodeDelphi.NotifyAfterSet = .NotifyAfterSet

                        m_ClassesToCodeCs.NotifyLightWeight = .NotifyLightWeight
                        m_ClassesToCodeVb.NotifyLightWeight = .NotifyLightWeight
                        m_ClassesToCodeDelphi.NotifyLightWeight = .NotifyLightWeight

                        m_ClassesToCodeCs.NotifyOnlyWhenRequired = .NotifyOnlyWhenRequired
                        m_ClassesToCodeVb.NotifyOnlyWhenRequired = .NotifyOnlyWhenRequired
                        m_ClassesToCodeDelphi.NotifyOnlyWhenRequired = .NotifyOnlyWhenRequired

                        m_ClassesToCodeCs.ImplementShadows = .ImplementShadows
                        m_ClassesToCodeVb.ImplementShadows = .ImplementShadows
                        m_ClassesToCodeDelphi.ImplementShadows = .ImplementShadows

                        m_ClassesToCodeCs.AddPropertyNotifyMethods = .AddPropertyNotifyMethods
                        m_ClassesToCodeVb.AddPropertyNotifyMethods = .AddPropertyNotifyMethods
                        m_ClassesToCodeDelphi.AddPropertyNotifyMethods = .AddPropertyNotifyMethods

                        m_ClassesToCodeCs.GeneratePOCO = .GeneratePOCO
                        m_ClassesToCodeVb.GeneratePOCO = .GeneratePOCO
                        m_ClassesToCodeDelphi.GeneratePOCO = .GeneratePOCO

                        m_ClassesToCodeCs.ReallyNoRegions = .ReallyNoRegions
                        m_ClassesToCodeVb.ReallyNoRegions = .ReallyNoRegions
                        m_ClassesToCodeDelphi.ReallyNoRegions = .ReallyNoRegions

                        m_ClassesToCodeCs.UseTypedCollections = .UseTypedCollections
                        m_ClassesToCodeVb.UseTypedCollections = .UseTypedCollections
                        m_ClassesToCodeDelphi.UseTypedCollections = .UseTypedCollections

                        m_ClassesToCodeCs.WrapCollections = .WrapCollections
                        m_ClassesToCodeVb.WrapCollections = .WrapCollections
                        m_ClassesToCodeDelphi.WrapCollections = .WrapCollections

                        m_ClassesToCodeCs.UseGenericCollections = .UseGenericCollections
                        m_ClassesToCodeVb.UseGenericCollections = .UseGenericCollections
                        m_ClassesToCodeDelphi.UseGenericCollections = .UseGenericCollections

                        m_ClassesToCodeCs.EmbedXml = .EmbedXml
                        m_ClassesToCodeVb.EmbedXml = .EmbedXml
                        m_ClassesToCodeDelphi.EmbedXml = .EmbedXml

                        Select Case .TargetMapper

                            Case TargetMapperEnum.POCO

                                m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.POCO
                                m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.POCO
                                m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.POCO

                            Case TargetMapperEnum.NPersist

                                m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.NPersist
                                m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.NPersist
                                m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.NPersist

                            Case TargetMapperEnum.NHibernate

                                m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.NHibernate
                                m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.NHibernate
                                m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.NHibernate

                                'Case TargetMapperEnum.ECO2

                                '    m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.ECO2
                                '    m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.ECO2
                                '    m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.ECO2

                                'Case TargetMapperEnum.WilsonORMapper

                                '    m_ClassesToCodeCs.TargetPlatform = TargetPlatformEnum.WilsonORMapper
                                '    m_ClassesToCodeVb.TargetPlatform = TargetPlatformEnum.WilsonORMapper
                                '    m_ClassesToCodeDelphi.TargetPlatform = TargetPlatformEnum.WilsonORMapper

                        End Select

                    End With

                    Exit For

                End If

            Next

        End If

    End Sub

    Private Sub SetupClassesToTables(ByVal domainMap As IDomainMap)

        Dim resource As ProjectModel.IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)
        Dim cfg As ProjectModel.DomainConfig

        If Not resource Is Nothing Then

            For Each cfg In resource.Configs

                If cfg.IsActive Then

                    With cfg.ClassesToTablesConfig

                        m_ClassesToTables.TablePrefix = .TablePrefix
                        m_ClassesToTables.TableSuffix = .TableSuffix

                    End With

                    Exit For

                End If

            Next

        End If

    End Sub


    Private Sub SetupTablesToClasses(ByVal domainMap As IDomainMap)

        Dim resource As ProjectModel.IResource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)
        Dim cfg As ProjectModel.DomainConfig

        Dim convObj As Object
        Dim convMeth As MethodInfo

        If Not resource Is Nothing Then

            For Each cfg In resource.Configs

                If cfg.IsActive Then

                    With cfg.TablesToClassesConfig

                        m_TablesToClasses.GenerateInverseProperties = .GenerateInverseProperties

                        GetConverterObjectAndMethod(.TableToClassNameConverter, convObj, convMeth)

                        m_TablesToClasses.GetClassNameForTableConverter = convObj
                        m_TablesToClasses.GetClassNameForTableConverterMethod = convMeth

                        GetConverterObjectAndMethod(.ColumnToPropertyNameConverter, convObj, convMeth)

                        m_TablesToClasses.GetPropertyNameForColumnConverter = convObj
                        m_TablesToClasses.GetPropertyNameForColumnConverterMethod = convMeth

                        GetConverterObjectAndMethod(.PropertyToInverseNameConverter, convObj, convMeth)

                        m_TablesToClasses.GetInverseNameForPropertyConverter = convObj
                        m_TablesToClasses.GetInverseNameForPropertyConverterMethod = convMeth

                    End With

                    Exit For

                End If

            Next

        End If

    End Sub

    Private Sub GetConverterObjectAndMethod(ByVal converterName As String, ByRef convObj As Object, ByRef convMeth As MethodInfo)

        Dim testConvObj As ConverterClass
        Dim testConvMeth As ConverterMethod

        convObj = Nothing
        convMeth = Nothing

        For Each testConvObj In Converters

            For Each testConvMeth In testConvObj.ConverterMethods

                If testConvObj.Name & " : " & testConvMeth.MethodName = converterName Then

                    convObj = testConvObj.Converter
                    convMeth = testConvMeth.MethodInfo

                    Exit Sub

                End If

            Next

        Next

    End Sub



    Private Sub SetupPreviewDTD()

        m_ClassesToCodeCs.IncludeRegions = False
        m_ClassesToCodeVb.IncludeRegions = False
        m_ClassesToCodeDelphi.IncludeRegions = False

        m_ClassesToCodeCs.IncludeComments = True
        m_ClassesToCodeVb.IncludeComments = True
        m_ClassesToCodeDelphi.IncludeComments = True

    End Sub

#End Region

#Region " CodeMaps "

    Private Sub OpenDomainCodeMap(ByVal domainMap As IDomainMap, ByVal codeLanguage As CodeLanguage)

        Dim codeMap As ICodeMap = domainMap.EnsuredGetCodeMap(codeLanguage)
        Dim code As String = codeMap.Code

        Select Case codeLanguage

            Case codeLanguage.CSharp

                OpenCodeMapDocument(domainMap.Name & ".cs", "C# custom source code for class '" & domainMap.Name & "'", MainDocumentType.CodeCSharp, domainMap, codeMap)

            Case codeLanguage.VB

                OpenCodeMapDocument(domainMap.Name & ".vb", "VB.Net custom source code for class '" & domainMap.Name & "'", MainDocumentType.CodeVbNet, domainMap, codeMap)

            Case codeLanguage.Delphi

                OpenCodeMapDocument(domainMap.Name & ".pas", "Delphi for .Net custom source code for class '" & domainMap.Name & "'", MainDocumentType.CodeVbNet, domainMap, codeMap)

        End Select

    End Sub


    Private Sub OpenClassCodeMap(ByVal classMap As IClassMap, ByVal codeLanguage As CodeLanguage)

        Dim codeMap As ICodeMap = classMap.EnsuredGetCodeMap(codeLanguage)
        Dim code As String = codeMap.Code

        Select Case codeLanguage

            Case codeLanguage.CSharp

                OpenCodeMapDocument(classMap.Name & ".cs", "C# custom source code for class '" & classMap.Name & "'", MainDocumentType.CodeCSharp, classMap, codeMap)

            Case codeLanguage.VB

                OpenCodeMapDocument(classMap.Name & ".vb", "VB.Net custom source code for class '" & classMap.Name & "'", MainDocumentType.CodeVbNet, classMap, codeMap)

            Case codeLanguage.Delphi

                OpenCodeMapDocument(classMap.Name & ".pas", "Delphi for .Net custom source code for class '" & classMap.Name & "'", MainDocumentType.CodeVbNet, classMap, codeMap)

        End Select

    End Sub

#End Region


#Region " Previews "


    Private Sub ShowDomainXml(ByVal domainMap As IDomainMap)

        Dim xml As String

        xml = GetDomainXml(domainMap)

        AddNewPreviewDocument(xml, domainMap.Name & ".npersist", "Xml for domain map '" & domainMap.Name & "'", MainDocumentType.NPersist)

    End Sub

    Public Function GetDomainXml(ByVal domainMap As IDomainMap) As String

        Dim xml As String

        Select Case domainMap.MapSerializer

            Case MapSerializer.DefaultSerializer

                Dim mapSerializer As New DefaultMapSerializer

                xml = mapSerializer.Serialize(domainMap)

            Case MapSerializer.DotNetSerializer


                Try

                    Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(DomainMap))
                    ' To write to a file, create a StreamWriter object.
                    Dim myWriter As StringWriter = New StringWriter
                    mySerializer.Serialize(myWriter, domainMap)
                    xml = myWriter.ToString
                    myWriter.Close()

                Catch ex As Exception

                    Throw New Exception("Could not serialize Domain Map! " & ex.Message, ex)

                End Try

            Case MapSerializer.CustomSerializer

                MsgBox("Can't serialize using custom serializer! Please select a different serialiser for you domain map!")

                Exit Function

        End Select

        Return xml

    End Function


    Private Sub ShowDomainDtd(ByVal domainMap As IDomainMap)

        Dim dtd As String
        Dim title As String

        Dim sourceMap As ISourceMap

        For Each sourceMap In domainMap.SourceMaps


            Select Case sourceMap.SourceType

                Case SourceType.MSAccess

                    dtd = m_TablesToSourceMdb.SourceToDTDEvolve(sourceMap)

                Case SourceType.MSSqlServer

                    dtd = m_TablesToSourceSql.SourceToDTDEvolve(sourceMap)

            End Select


            title = "DDL For updating data source '" & sourceMap.ConnectionString & "' from model data source '" & domainMap.Name & ":" & sourceMap.Name & "'."

            AddNewPreviewDocument(dtd, sourceMap.Name & ".sql", title, MainDocumentType.DTD)

        Next

    End Sub





    Private Sub DomainToCode(ByVal domainMap As IDomainMap, ByVal codeLanguage As IClassesToCode.CodeLanguageEnum)

        Dim code As String

        SetupPreviewCode(domainMap)

        Select Case codeLanguage

            Case Tools.IClassesToCode.CodeLanguageEnum.CSharp

                code = m_ClassesToCodeCs.DomainToCode(domainMap, False)
                AddNewPreviewDocument(code, domainMap.Name & ".cs", "C# source code for domain '" & domainMap.Name & "'", MainDocumentType.CodeCSharp)

            Case Tools.IClassesToCode.CodeLanguageEnum.VbNet

                code = m_ClassesToCodeVb.DomainToCode(domainMap, False)
                AddNewPreviewDocument(code, domainMap.Name & ".vb", "VB.Net source code for domain '" & domainMap.Name & "'", MainDocumentType.CodeVbNet)

            Case Tools.IClassesToCode.CodeLanguageEnum.Delphi

                code = m_ClassesToCodeDelphi.DomainToCode(domainMap, False)
                AddNewPreviewDocument(code, domainMap.Name & ".pas", "Delphi for .Net source code for domain '" & domainMap.Name & "'", MainDocumentType.CodeDelphi)

        End Select


    End Sub


    Private Sub NamespaceToCode(ByVal domainMap As IDomainMap, ByVal name As String, ByVal codeLanguage As IClassesToCode.CodeLanguageEnum)

        Dim code As String

        SetupPreviewCode(domainMap)

        Select Case codeLanguage

            Case Tools.IClassesToCode.CodeLanguageEnum.CSharp

                code = m_ClassesToCodeCs.NamespaceToCode(domainMap, name, False)
                AddNewPreviewDocument(code, name & ".cs", "C# source code for namespace '" & name & "'", MainDocumentType.CodeCSharp)

            Case Tools.IClassesToCode.CodeLanguageEnum.VbNet

                code = m_ClassesToCodeVb.NamespaceToCode(domainMap, name, False)
                AddNewPreviewDocument(code, name & ".vb", "VB.Net source code for namespace '" & name & "'", MainDocumentType.CodeVbNet)


            Case Tools.IClassesToCode.CodeLanguageEnum.Delphi

                code = m_ClassesToCodeDelphi.NamespaceToCode(domainMap, name, False)
                AddNewPreviewDocument(code, name & ".pas", "Delphi for .Net source code for namespace '" & name & "'", MainDocumentType.CodeVbNet)

        End Select

    End Sub


    Private Sub ClassToCode(ByVal classMap As IClassMap, ByVal codeLanguage As IClassesToCode.CodeLanguageEnum)

        Dim code As String

        SetupPreviewCode(classMap.DomainMap)

        Select Case codeLanguage

            Case Tools.IClassesToCode.CodeLanguageEnum.CSharp

                code = m_ClassesToCodeCs.ClassToCode(classMap)
                AddNewPreviewDocument(code, classMap.Name & ".cs", "C# source code for class '" & classMap.Name & "'", MainDocumentType.CodeCSharp)

            Case Tools.IClassesToCode.CodeLanguageEnum.VbNet

                code = m_ClassesToCodeVb.ClassToCode(classMap)
                AddNewPreviewDocument(code, classMap.Name & ".vb", "VB.Net source code for class '" & classMap.Name & "'", MainDocumentType.CodeVbNet)

            Case Tools.IClassesToCode.CodeLanguageEnum.Delphi

                code = m_ClassesToCodeDelphi.ClassToCode(classMap)
                AddNewPreviewDocument(code, classMap.Name & ".pas", "Delphi for .Net source code for class '" & classMap.Name & "'", MainDocumentType.CodeVbNet)

        End Select

    End Sub

    Private Sub PropertyToCode(ByVal propertyMap As IPropertyMap, ByVal codeLanguage As IClassesToCode.CodeLanguageEnum)

        Dim code As String

        SetupPreviewCode(propertyMap.ClassMap.DomainMap)

        Select Case codeLanguage

            Case Tools.IClassesToCode.CodeLanguageEnum.CSharp

                code = m_ClassesToCodeCs.PropertyToCode(propertyMap)
                AddNewPreviewDocument(code, propertyMap.Name & ".cs", "C# source code for property '" & propertyMap.Name & "'", MainDocumentType.CodeCSharp)

            Case Tools.IClassesToCode.CodeLanguageEnum.VbNet

                code = m_ClassesToCodeVb.PropertyToCode(propertyMap)
                AddNewPreviewDocument(code, propertyMap.Name & ".vb", "VB.Net source code for property '" & propertyMap.Name & "'", MainDocumentType.CodeVbNet)

            Case Tools.IClassesToCode.CodeLanguageEnum.Delphi

                code = m_ClassesToCodeDelphi.PropertyToCode(propertyMap)
                AddNewPreviewDocument(code, propertyMap.Name & ".pas", "Delphi for .Net source code for property '" & propertyMap.Name & "'", MainDocumentType.CodeVbNet)

        End Select

    End Sub

    Private Sub SourceToDTD(ByVal sourceMap As ISourceMap)

        Dim dtd As String

        SetupPreviewDTD()

        Select Case sourceMap.SourceType

            Case SourceType.MSSqlServer

                dtd = m_TablesToSourceSql.SourceToDTD(sourceMap, True)

            Case SourceType.MSAccess

                dtd = m_TablesToSourceMdb.SourceToDTD(sourceMap, True)

        End Select

        AddNewPreviewDocument(dtd, sourceMap.Name & ".sql", "DDL for data source '" & sourceMap.Name & "'", MainDocumentType.DTD)

    End Sub



    Private Sub TableToDTD(ByVal tableMap As ITableMap)

        Dim dtd As String

        SetupPreviewDTD()

        Select Case tableMap.SourceMap.SourceType

            Case SourceType.MSSqlServer

                dtd = m_TablesToSourceSql.TableToDTD(tableMap, True, True)

            Case SourceType.MSAccess

                dtd = m_TablesToSourceMdb.TableToDTD(tableMap, True, True)

        End Select

        AddNewPreviewDocument(dtd, tableMap.Name & ".sql", "DDL for table '" & tableMap.Name & "'", MainDocumentType.DTD)

    End Sub


#End Region

#Region " NHibernate "

    Private Sub DomainToNhXml(ByVal domainMap As IDomainMap)

        Dim xml As String

        xml = m_NPersistToNHibernate.Serialize(domainMap)

        AddNewPreviewDocument(xml, domainMap.Name & ".hbm.xml", "NHibernate Xml mapping file for domain map '" & domainMap.Name & "'", MainDocumentType.XML)

    End Sub

    Private Sub DomainToNhCodeCs(ByVal domainMap As IDomainMap)

        Dim code As String

        code = m_NPersistToNHibernate.ToCSharp(domainMap)

        AddNewPreviewDocument(code, domainMap.Name & ".cs", "NHibernate C# source code for classes in domain map '" & domainMap.Name & "'", MainDocumentType.CodeCSharp)

    End Sub

    Private Sub DomainToNhCodeVb(ByVal domainMap As IDomainMap)

        Dim code As String

        code = m_NPersistToNHibernate.ToVb(domainMap)

        AddNewPreviewDocument(code, domainMap.Name & ".vb", "NHibernate Visual Basic.Net source code for classes in domain map '" & domainMap.Name & "'", MainDocumentType.CodeVbNet)

    End Sub

    Private Sub DomainToNhCodeDelphi(ByVal domainMap As IDomainMap)

        Dim code As String

        code = m_NPersistToNHibernate.ToDelphi(domainMap)

        AddNewPreviewDocument(code, domainMap.Name & ".pas", "NHibernate Delphi source code for classes in domain map '" & domainMap.Name & "'", MainDocumentType.CodeDelphi)

    End Sub


    Private Sub ClassToNhXml(ByVal classMap As IClassMap)

        Dim xml As String
        Dim superClassMap As IClassMap

        If Not m_NPersistToNHibernate.IsRootClass(classMap) Then

            superClassMap = classMap.GetInheritedClassMap

            While Not superClassMap Is Nothing

                If m_NPersistToNHibernate.IsRootClass(superClassMap) Then

                    classMap = superClassMap

                    superClassMap = Nothing

                Else

                    superClassMap = superClassMap.GetInheritedClassMap

                End If

            End While

        End If

        If Not classMap Is Nothing Then

            If m_NPersistToNHibernate.IsRootClass(classMap) Then

                xml = m_NPersistToNHibernate.Serialize(classMap)

                AddNewPreviewDocument(xml, classMap.Name & ".hbm.xml", "NHibernate Xml mapping file for class map '" & classMap.Name & "'", MainDocumentType.XML)

            End If

        End If

    End Sub


    Private Sub ClassToCodeNh(ByVal classMap As IClassMap, ByVal codeLanguage As IClassesToCode.CodeLanguageEnum)

        Dim code As String

        Select Case codeLanguage

            Case Tools.IClassesToCode.CodeLanguageEnum.CSharp

                code = m_NPersistToNHibernate.ToCSharp(classMap)
                AddNewPreviewDocument(code, classMap.Name & ".cs", "NHibernate C# source code for class '" & classMap.Name & "'", MainDocumentType.CodeCSharp)

            Case Tools.IClassesToCode.CodeLanguageEnum.VbNet

                code = m_NPersistToNHibernate.ToVb(classMap)
                AddNewPreviewDocument(code, classMap.Name & ".vb", "NHibernate VB.Net source code for class '" & classMap.Name & "'", MainDocumentType.CodeVbNet)

            Case Tools.IClassesToCode.CodeLanguageEnum.Delphi

                code = m_NPersistToNHibernate.ToDelphi(classMap)
                AddNewPreviewDocument(code, classMap.Name & ".pas", "NHibernate Delphi for .Net source code for class '" & classMap.Name & "'", MainDocumentType.CodeVbNet)

        End Select

    End Sub


#End Region

#Region " Eco "

    Private Sub DomainToEcoXml(ByVal domainMap As IDomainMap)

        'Dim xml As String
        'Dim ecoClassList As ecoClassList
        'Dim takenNames As New Hashtable

        'ecoClassList = m_NPersistToEco.NPersistToEco(domainMap, takenNames)

        'xml = m_EcoToXml.EcoToXml(ecoClassList)

        'AddNewPreviewDocument(xml, domainMap.Name & ".xml", "Xml in Borland ECO format for domain map '" & domainMap.Name & "'", MainDocumentType.XML)

    End Sub

    Private Sub DomainToEcoCodeCs(ByVal domainMap As IDomainMap)

        'Dim code As String
        'Dim takenNames As New Hashtable

        'code = m_NPersistToEcoCodeCs.NPersistToEcoCode(domainMap, takenNames)

        'AddNewPreviewDocument(code, domainMap.Name & ".cs", "C# code for Borland ECO classes for domain map '" & domainMap.Name & "'", MainDocumentType.CodeCSharp)

    End Sub

    Private Sub DomainToEcoCodeDelphi(ByVal domainMap As IDomainMap)

        'Dim code As String
        'Dim takenNames As New Hashtable

        'code = m_NPersistToEcoCodeDelphi.NPersistToEcoCode(domainMap, takenNames)

        'AddNewPreviewDocument(code, domainMap.Name & ".pas", "Borland Delphi code for Borland ECO classes for domain map '" & domainMap.Name & "'", MainDocumentType.CodeCSharp)

    End Sub

#End Region

#End Region

#Region " Synchronization "

#Region " Synchronization Previews "


    Private Overloads Sub SourceToTables()

        SourceToTables(mapTreeView.GetAllDomainMaps)

    End Sub

    Private Overloads Sub SourceToTables(ByVal sourceMap As ISourceMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(sourceMap.DomainMap)

        SourceToTables(domainMaps, sourceMap)

    End Sub

    Private Overloads Sub SourceToTables(ByVal domainMap As IDomainMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(domainMap)

        SourceToTables(domainMaps)

    End Sub


    Private Overloads Sub SourceToTables(ByVal domainMaps As ArrayList, Optional ByVal justThisSourceMap As ISourceMap = Nothing)

        Dim domainMap As IDomainMap
        Dim sourceMap As ISourceMap
        Dim newDomainMap As IDomainMap
        Dim newSourceMap As ISourceMap
        Dim newDomainMaps As New ArrayList
        Dim ok As Boolean


        Dim errMsg As String
        Dim addErrInfo As String = ""

        PushStatus("Calculating preview...")

        Try

            ClearMsgs()
            AddMsg("'From Data Source To Model' Synchronizer:")
            AddMsg("Generating Preview...")

            Cursor.Current = MouseSet(Cursors.WaitCursor)

            Application.DoEvents()

            DiscardSynch()

            m_SynchMode = SynchModeEnum.SourceToTables
            SetActionType(ActionTypeEnum.SynchAction)

            For Each domainMap In domainMaps

                ok = True

                If Not justThisSourceMap Is Nothing Then

                    ok = False

                    For Each sourceMap In domainMap.SourceMaps

                        If sourceMap Is justThisSourceMap Then

                            ok = True

                            Exit For

                        End If

                    Next

                End If

                If ok Then

                    newDomainMap = New domainMap
                    domainMap.Copy(newDomainMap)

                    For Each sourceMap In domainMap.SourceMaps

                        ok = True

                        If Not justThisSourceMap Is Nothing Then

                            ok = False

                            If sourceMap Is justThisSourceMap Then

                                ok = True

                            End If

                        End If

                        If ok Then

                            errMsg = "Database of type '" & sourceMap.SourceType.ToString & "' not supported in combination with provider '" & sourceMap.ProviderType.ToString() & "'!"

                            newSourceMap = New sourceMap

                            sourceMap.Copy(newSourceMap)

                            newSourceMap.DomainMap = newDomainMap

                            PushStatus("Contacting data source...")


                            If Not TestConnection(sourceMap, True, True, addErrInfo) Then

                                newDomainMap = Nothing

                                Try

                                    FormattingServices.VerifyConnectionParameters(sourceMap.ConnectionString, sourceMap.ProviderType, sourceMap.SourceType)

                                Catch ex As Exception

                                    addErrInfo = ex.Message & vbCrLf & vbCrLf & addErrInfo

                                End Try

                                If Len(addErrInfo) > 0 Then addErrInfo = vbCrLf & vbCrLf & "Error: " & addErrInfo

                                MsgBox("Source '" & sourceMap.DomainMap.Name & "." & sourceMap.Name & "' could not be contacted! Skipping domain map '" & sourceMap.DomainMap.Name & "'!" & addErrInfo)

                            End If

                            If Not newDomainMap Is Nothing Then

                                Select Case sourceMap.ProviderType

                                    Case ProviderType.SqlClient

                                        Select Case sourceMap.SourceType

                                            Case SourceType.MSSqlServer

                                                m_SourceToTablesSql.SourceToTables(sourceMap, newDomainMap, m_hashSynchDiff)

                                            Case Else

                                                newDomainMap = Nothing
                                                MsgBox(errMsg)

                                        End Select

                                    Case ProviderType.OleDb

                                        Select Case sourceMap.SourceType

                                            Case SourceType.MSAccess

                                                m_SourceToTablesMdb.SourceToTables(sourceMap, newDomainMap, m_hashSynchDiff)

                                            Case Else

                                                newDomainMap = Nothing
                                                MsgBox(errMsg)

                                        End Select

                                    Case ProviderType.Odbc

                                        'Select Case sourceMap.SourceType

                                        '    Case SourceType.MSSqlServer

                                        '    Case Else

                                        newDomainMap = Nothing
                                        MsgBox(errMsg)

                                        'End Select

                                    Case ProviderType.Bdp

                                        Select Case sourceMap.SourceType

                                            'Case SourceType.MSSqlServer

                                            '    m_SourceToTablesBdpSql.SourceToTables(sourceMap, newDomainMap, m_hashSynchDiff)

                                            'Case SourceType.MSAccess

                                            '    m_SourceToTablesBdpAccess.SourceToTables(sourceMap, newDomainMap, m_hashSynchDiff)

                                            'Case SourceType.Interbase

                                            '    m_SourceToTablesBdpInterbase.SourceToTables(sourceMap, newDomainMap, m_hashSynchDiff)

                                            Case Else

                                                newDomainMap = Nothing
                                                MsgBox(errMsg)

                                        End Select

                                    Case ProviderType.Other

                                        newDomainMap = Nothing
                                        MsgBox(errMsg)

                                    Case Else

                                        newDomainMap = Nothing
                                        MsgBox(errMsg)

                                End Select

                            End If

                            PullStatus()

                        End If

                    Next

                    If Not newDomainMap Is Nothing Then

                        newDomainMaps.Add(newDomainMap)

                    End If

                End If

            Next


            mapTreeViewPreview.Nodes.Clear()

            For Each newDomainMap In newDomainMaps

                mapTreeViewPreview.LoadDomain(newDomainMap)

            Next

            Cursor.Current = MouseSet()

            ShowSynchTree()

            mapTreeViewPreview.MarkAllNodes(True)

            AddMsg("Preview Generated!")

            If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

                CommitSynch()

            End If

        Catch ex As Exception

            PullStatus()

        End Try

    End Sub

    Private Overloads Sub TablesToSource()

        TablesToSource(mapTreeView.GetAllDomainMaps)

    End Sub

    Private Overloads Sub TablesToSource(ByVal sourceMap As ISourceMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(sourceMap.DomainMap)

        TablesToSource(domainMaps, sourceMap)

    End Sub

    Private Overloads Sub TablesToSource(ByVal domainMap As IDomainMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(domainMap)

        TablesToSource(domainMaps)

    End Sub

    Private Overloads Sub TablesToSource(ByVal domainMaps As ArrayList, Optional ByVal justThisSourceMap As ISourceMap = Nothing)

        Dim domainMap As IDomainMap
        Dim sourceMap As ISourceMap
        Dim newDomainMap As IDomainMap
        Dim newSourceMap As ISourceMap
        Dim newDomainMaps As New ArrayList
        Dim ok As Boolean

        Dim addErrInfo As String = ""

        m_TablesToSourceWorking = True

        PushStatus("Calculating preview...")

        ClearMsgs()
        AddMsg("'From Model To Data Source' Synchronizer:")
        AddMsg("Generating Preview...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        DiscardSynch()

        m_SynchMode = SynchModeEnum.TablesToSource
        SetActionType(ActionTypeEnum.SynchAction)

        For Each domainMap In domainMaps

            ok = True

            If Not justThisSourceMap Is Nothing Then

                ok = False

                For Each sourceMap In domainMap.SourceMaps

                    If sourceMap Is justThisSourceMap Then

                        ok = True

                        Exit For

                    End If

                Next

            End If

            If ok Then

                newDomainMap = New domainMap
                domainMap.Copy(newDomainMap)

                For Each sourceMap In domainMap.SourceMaps

                    ok = True

                    If Not justThisSourceMap Is Nothing Then

                        ok = False

                        If sourceMap Is justThisSourceMap Then

                            ok = True

                        End If

                    End If

                    If ok Then

                        newSourceMap = New sourceMap

                        sourceMap.Copy(newSourceMap)

                        newSourceMap.DomainMap = newDomainMap

                        PushStatus("Contacting data source...")

                        If Not TestConnection(sourceMap, True, True, addErrInfo) Then

                            newDomainMap = Nothing

                            Try

                                FormattingServices.VerifyConnectionParameters(sourceMap.ConnectionString, sourceMap.ProviderType, sourceMap.SourceType)

                            Catch ex As Exception

                                addErrInfo = ex.Message & vbCrLf & vbCrLf & addErrInfo

                            End Try

                            If Len(addErrInfo) > 0 Then addErrInfo = vbCrLf & vbCrLf & "Error: " & addErrInfo

                            MsgBox("Source '" & sourceMap.DomainMap.Name & "." & sourceMap.Name & "' could not be contacted! Skipping domain map '" & sourceMap.DomainMap.Name & "'!" & addErrInfo)

                        End If

                        If Not newDomainMap Is Nothing Then

                            Select Case sourceMap.SourceType

                                Case SourceType.MSSqlServer

                                    m_TablesToSourceSql.TablesToSource(sourceMap, newDomainMap, m_hashSynchDiff)

                                Case SourceType.MSAccess

                                    m_TablesToSourceMdb.TablesToSource(sourceMap, newDomainMap, m_hashSynchDiff)

                                Case Else

                                    MsgBox("Database type '" & sourceMap.SourceType.ToString & "' not supported by this operation!")

                            End Select

                        End If

                        PullStatus()

                    End If

                Next

                If Not newDomainMap Is Nothing Then

                    newDomainMaps.Add(newDomainMap)

                End If

            End If

        Next


        mapTreeViewPreview.Nodes.Clear()

        For Each newDomainMap In newDomainMaps

            mapTreeViewPreview.LoadDomain(newDomainMap)

        Next

        ShowSynchTree()

        mapTreeViewPreview.MarkAllNodes(True)

        Cursor.Current = MouseSet()

        PullStatus()

        RefreshSynchTablesToSourcePreviewDTD()

        m_TablesToSourceWorking = False

        AddMsg("Preview Generated!")

        If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

            CommitSynch()

        End If

    End Sub



    Private Overloads Sub ClassesToTables()

        ClassesToTables(mapTreeView.GetAllDomainMaps)

    End Sub

    Private Overloads Sub ClassesToTables(ByVal domainMap As IDomainMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(domainMap)

        ClassesToTables(domainMaps)

    End Sub

    Private Overloads Sub ClassesToTables(ByVal domainMaps As ArrayList)

        Dim domainMap As IDomainMap
        Dim newDomainMap As IDomainMap
        Dim newDomainMaps As New ArrayList

        PushStatus("Calculating preview...")

        ClearMsgs()
        AddMsg("'From Classes To Tables' Synchronizer:")
        AddMsg("Generating preview...")

        DiscardSynch()

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        m_SynchMode = SynchModeEnum.ClassesToTables
        SetActionType(ActionTypeEnum.SynchAction)

        For Each domainMap In domainMaps

            'newDomainMap = New domainMap

            'domainMap.Copy(newDomainMap)

            newDomainMap = domainMap.DeepClone

            PushStatus("Generating tables from classes...")

            'Try

            SetupClassesToTables(domainMap)

            m_ClassesToTables.GenerateTablesForClasses(domainMap, newDomainMap, True)

            'Catch ex As Exception

            'MsgBox("Could not generate preview! Reason: " & ex.Message)

            'End Try

            newDomainMaps.Add(newDomainMap)

        Next

        PullStatus()

        mapTreeViewPreview.Nodes.Clear()

        For Each newDomainMap In newDomainMaps

            mapTreeViewPreview.LoadDomain(newDomainMap)

        Next

        Cursor.Current = MouseSet()

        ShowSynchTree()

        mapTreeViewPreview.MarkAllNodes(True)

        PullStatus()

        AddMsg("Preview Generated!")

        If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

            CommitSynch()

        End If

    End Sub


    Private Overloads Sub TablesToClasses()

        TablesToClasses(mapTreeView.GetAllDomainMaps)

    End Sub

    Private Overloads Sub TablesToClasses(ByVal domainMap As IDomainMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(domainMap)

        TablesToClasses(domainMaps)

    End Sub

    Private Overloads Sub TablesToClasses(ByVal domainMaps As ArrayList)

        m_TablesToClasses.CheckReservedNamesDelphi = False

        Dim domainMap As IDomainMap
        Dim newDomainMap As IDomainMap
        Dim newDomainMaps As New ArrayList

        PushStatus("Calculating preview...")

        ClearMsgs()
        AddMsg("'From Tables To Classes' Synchronizer:")
        AddMsg("Generating preview...")

        DiscardSynch()

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        m_SynchMode = SynchModeEnum.TablesToClasses

        SetActionType(ActionTypeEnum.SynchAction)


        PushStatus("Generating classes from tables...")

        For Each domainMap In domainMaps

            'newDomainMap = New domainMap

            'domainMap.Copy(newDomainMap)

            newDomainMap = domainMap.DeepClone

            SetupTablesToClasses(domainMap)

            m_TablesToClasses.GenerateClassesForSources(domainMap, newDomainMap, True)

            m_TablesToClasses.MakeSureNamesAreLegal(newDomainMap)

            newDomainMaps.Add(newDomainMap)

        Next

        PullStatus()

        mapTreeViewPreview.Nodes.Clear()

        For Each newDomainMap In newDomainMaps

            mapTreeViewPreview.LoadDomain(newDomainMap)

        Next

        Cursor.Current = MouseSet()

        ShowSynchTree()

        mapTreeViewPreview.MarkAllNodes(True)

        PullStatus()

        AddMsg("Preview Generated!")

        If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

            CommitSynch()

        End If

    End Sub


    Private Sub CodeToClassesFromDll(Optional ByVal domainMap As IDomainMap = Nothing)

        Dim newDomainMap As IDomainMap = New domainMap

        Dim asm As System.Reflection.Assembly
        Dim path As String 
        Dim name As String

        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.Filter = "Assembly files (*.dll)|*.dll|All files (*.*)|*.*"
        If OpenFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

            Exit Sub

        End If

        path = OpenFileDialog1.FileName

        If Len(path) < 4 Then Exit Sub

        If Not LCase(path).EndsWith("dll") Then Exit Sub


        asm = System.Reflection.Assembly.LoadFile(path)


        DiscardSynch()

        m_SynchMode = SynchModeEnum.CodeToClasses

        SetActionType(ActionTypeEnum.SynchAction)

        If domainMap Is Nothing Then

            Select Case MsgBox("Would you like to create a new domain model for the classes extracted from the assembly?", MsgBoxStyle.YesNoCancel, "New Domain Model")

                Case MsgBoxResult.Yes

                    name = asm.GetName.Name

                    domainMap = NewMap(name)

                Case MsgBoxResult.No

                    MsgBox("Please select this synchrinization action from the domain model that you would like to fill with the classes extracted from the selected dll!")

                    Exit Sub

                    'TODO: Select domain model dialogue

                Case MsgBoxResult.Cancel

                    Exit Sub

            End Select

        End If


        PushStatus("Calculating preview...")

        ClearMsgs()
        AddMsg("'From Code To Model' Synchronizer:")
        AddMsg("Generating Preview...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        domainMap.Copy(newDomainMap)

        PushStatus("Generating classes from tables...")


        m_CodeToClasses.AssemblyCodeToClasses(asm, domainMap, newDomainMap)


        PullStatus()

        mapTreeViewPreview.Nodes.Clear()

        mapTreeViewPreview.LoadDomain(newDomainMap)

        Cursor.Current = MouseSet()

        ShowSynchTree()

        mapTreeViewPreview.MarkAllNodes(True)

        PullStatus()

        AddMsg("Preview Generated!")


        If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

            CommitSynch()

        End If

    End Sub

    Private Overloads Sub ClassesToCode()

        ClassesToCode(mapTreeView.GetAllDomainMaps)

    End Sub

    Private Overloads Sub ClassesToCode(ByVal domainMap As IDomainMap)

        Dim domainMaps As New ArrayList

        domainMaps.Add(domainMap)

        ClassesToCode(domainMaps)

    End Sub

    Private Overloads Sub ClassesToCode(ByVal domainMaps As ArrayList)

        Dim domainMap As IDomainMap
        Dim classMap As IClassMap
        Dim config As DomainConfig

        Dim resource As IResource
        Dim cfg As DomainConfig

        Dim clsExt As String
        Dim projExt As String

        Dim folderNode As PreviewFolderNode
        Dim fileNode As PreviewFileNode

        Dim projImgIndex As Integer
        Dim projSelImgIndex As Integer
        Dim clsImgIndex As Integer
        Dim clsSelImgIndex As Integer
        Dim dllImgIndex As Integer = 64
        Dim dllSelImgIndex As Integer = 65

        Dim path As String

        Dim src As SourceCodeFile
        Dim useCfg As ClassesToCodeConfig

        Dim fileType As SourceCodeFileTypeEnum

        Dim fileName As String

        PushStatus("Calculating preview...")

        ClearMsgs()
        AddMsg("'From Model To Code' Synchronizer:")
        AddMsg("Generating preview...")

        DiscardSynch()

        treePreviewClassesToCode.Visible = True

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        m_SynchMode = SynchModeEnum.ClassesToCode
        SetActionType(ActionTypeEnum.SynchAction)

        For Each domainMap In domainMaps

            resource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

            For Each cfg In resource.Configs

                If cfg.IsActive Then

                    config = cfg

                    Exit For

                End If

            Next

            If config Is Nothing Then

                MsgBox("No tool configuration is selected as currently active for domain '" & domainMap.Name & "'! Please mark a tool configuration as active first!")

            Else

                useCfg = config.ClassesToCodeConfig

                mapTreeView.ExtractToConfig(config, domainMap, True)

                OpenProperties()

                If Len(useCfg.TargetFolder) < 1 Then

                    ''src = useCfg.GetSourceCodeFile(domainMap)

                    ''If Not src Is Nothing Then

                    ''    If File.Exists(src.FilePath) Then

                    ''        useCfg.TargetFolder = New FileInfo(src.FilePath).Directory.FullName

                    ''    End If

                    ''End If

                    If Len(useCfg.TargetFolder) < 1 Then

                        useCfg.TargetFolder = New DirectoryInfo(Application.UserAppDataPath & "\..").FullName & "\Projects\" & m_Project.Name & "\" & domainMap.Name

                    End If


                End If



                Select Case useCfg.TargetLanguage

                    Case TargetLanguageEnum.CSharp

                        fileType = SourceCodeFileTypeEnum.CSharp
                        clsExt = "cs"
                        projExt = "csproj"

                    Case TargetLanguageEnum.VB

                        fileType = SourceCodeFileTypeEnum.VB
                        clsExt = "vb"
                        projExt = "vbproj"

                    Case TargetLanguageEnum.Delphi

                        fileType = SourceCodeFileTypeEnum.Delphi
                        clsExt = "pas"
                        projExt = "bdsproj"

                End Select


                folderNode = New PreviewFolderNode(config, domainMap)

                folderNode.Checked = True

                folderNode.ImageIndex = 54
                folderNode.SelectedImageIndex = 55

                path = useCfg.TargetFolder

                folderNode.Text = path

                treePreviewClassesToCode.Nodes.Add(folderNode)

                'Project File
                fileName = path & "\" & domainMap.Name & "." & projExt

                fileNode = New PreviewFileNode

                fileNode.FileType = fileType
                fileNode.DomainMap = domainMap

                fileNode.Checked = True

                fileNode.Text = domainMap.Name & "." & projExt


                src = useCfg.GetSourceCodeFile(domainMap)

                ''If src Is Nothing Then

                ''    src = useCfg.SetSourceCodeFile(domainMap, fileName)

                ''    src.FileType = useCfg.TargetLanguage

                ''End If

                fileNode.Src = src

                folderNode.Nodes.Add(fileNode)

                fileNode.Refresh()

                'AssemblyInfo
                fileName = path & "\AssemblyInfo." & clsExt

                fileNode = New PreviewFileNode

                fileNode.FileType = fileType
                fileNode.TagName = "AssemblyInfo"

                fileNode.Checked = True

                fileNode.ImageIndex = clsImgIndex
                fileNode.SelectedImageIndex = clsSelImgIndex

                fileNode.Text = "AssemblyInfo." & clsExt


                src = useCfg.GetSourceCodeFile("AssemblyInfo")

                ''If src Is Nothing Then

                ''    src = useCfg.SetSourceCodeFile("AssemblyInfo", fileName)

                ''    src.FileType = useCfg.TargetLanguage

                ''End If

                fileNode.Src = src


                folderNode.Nodes.Add(fileNode)

                fileNode.Refresh()

                If useCfg.UseTypedCollections Then

                    For Each classMap In domainMap.ClassMaps

                        'Typed collection file
                        'fileName = path & "\" & classMap.GetName & "." & clsExt
                        fileName = path & "\" & classMap.Name & "Collection." & clsExt

                        fileNode = New PreviewFileNode

                        fileNode.FileType = fileType
                        'fileNode.ClassMap = classMap

                        fileNode.Checked = True

                        fileNode.ImageIndex = clsImgIndex
                        fileNode.SelectedImageIndex = clsSelImgIndex

                        fileNode.Text = classMap.Name & "Collection." & clsExt

                        src = useCfg.GetSourceCodeFile(classMap.Name & "Collection")

                        ''If src Is Nothing Then

                        ''    src = useCfg.SetSourceCodeFile(classMap, fileName)

                        ''    src.FileType = useCfg.TargetLanguage

                        ''End If

                        fileNode.Src = src

                        folderNode.Nodes.Add(fileNode)

                        fileNode.Refresh()

                    Next

                End If


                For Each classMap In domainMap.ClassMaps

                    'Class file
                    'fileName = path & "\" & classMap.GetName & "." & clsExt
                    fileName = path & "\" & classMap.Name & "." & clsExt

                    fileNode = New PreviewFileNode

                    fileNode.FileType = fileType
                    fileNode.ClassMap = classMap

                    fileNode.Checked = True

                    fileNode.ImageIndex = clsImgIndex
                    fileNode.SelectedImageIndex = clsSelImgIndex

                    fileNode.Text = classMap.Name & "." & clsExt

                    src = useCfg.GetSourceCodeFile(classMap)

                    ''If src Is Nothing Then

                    ''    src = useCfg.SetSourceCodeFile(classMap, fileName)

                    ''    src.FileType = useCfg.TargetLanguage

                    ''End If

                    fileNode.Src = src

                    folderNode.Nodes.Add(fileNode)

                    fileNode.Refresh()

                Next

                Select Case useCfg.TargetMapper

                    Case TargetMapperEnum.NPersist

                        'Mapping File
                        fileName = path & "\" & domainMap.Name & ".npersist"

                        fileNode = New PreviewFileNode

                        fileNode.FileType = SourceCodeFileTypeEnum.NPersist
                        fileNode.TagName = "MapFile"

                        fileNode.Checked = True

                        fileNode.ImageIndex = 100
                        fileNode.SelectedImageIndex = 101

                        src = useCfg.GetSourceCodeFile("MapFile")

                        fileNode.Src = src

                        fileNode.Text = domainMap.Name & ".npersist"

                        folderNode.Nodes.Add(fileNode)

                        fileNode.Refresh()


                    Case TargetMapperEnum.NHibernate

                        'Mapping File
                        If useCfg.XmlFilePerClass Then

                            For Each classMap In m_NPersistToNHibernate.GetRootClasses(domainMap)

                                'fileName = path & "\" & classMap.Name & ".hbm.xml"
                                fileName = path & "\" & classMap.GetFullName & ".hbm.xml"

                                fileNode = New PreviewFileNode

                                fileNode.FileType = SourceCodeFileTypeEnum.Xml
                                fileNode.TagName = "MapFile." & classMap.Name

                                fileNode.Checked = True

                                fileNode.ImageIndex = 100
                                fileNode.SelectedImageIndex = 101

                                src = useCfg.GetSourceCodeFile("MapFile." & classMap.Name)

                                fileNode.Src = src

                                fileNode.Text = classMap.Name & ".hbm.xml"

                                folderNode.Nodes.Add(fileNode)

                                fileNode.Refresh()

                            Next

                        Else

                            fileName = path & "\" & domainMap.Name & ".hbm.xml"

                            fileNode = New PreviewFileNode

                            fileNode.FileType = SourceCodeFileTypeEnum.Xml
                            fileNode.TagName = "MapFile"

                            fileNode.Checked = True

                            fileNode.ImageIndex = 100
                            fileNode.SelectedImageIndex = 101

                            src = useCfg.GetSourceCodeFile("MapFile")

                            fileNode.Src = src

                            fileNode.Text = domainMap.Name & ".hbm.xml"

                            folderNode.Nodes.Add(fileNode)

                            fileNode.Refresh()

                        End If


                End Select


                If useCfg.TargetMapper = TargetMapperEnum.NPersist Then

                    If Not useCfg.GeneratePOCO Then

                        'NPersist framework dll
                        fileName = path & "\bin\Puzzle.NPersist.Framework.dll"

                        fileNode = New PreviewFileNode

                        fileNode.FileType = SourceCodeFileTypeEnum.Other
                        fileNode.TagName = "Puzzle.NPersist.Framework.dll"

                        fileNode.Checked = True

                        fileNode.ImageIndex = dllImgIndex
                        fileNode.SelectedImageIndex = dllSelImgIndex

                        fileNode.Text = "Puzzle.NPersist.Framework.dll"

                        src = useCfg.GetSourceCodeFile("Puzzle.NPersist.Framework.dll")

                        ''If src Is Nothing Then

                        ''    src = useCfg.SetSourceCodeFile("Puzzle.NPersist.Framework.dll", fileName)

                        ''    src.FileType = SourceCodeFileTypeEnum.Other

                        ''End If

                        fileNode.Src = src

                        folderNode.Nodes.Add(fileNode)

                        fileNode.Refresh()

                    End If

                End If


                folderNode.Expand()


            End If

        Next




        Cursor.Current = MouseSet()

        ShowSynchTree()

        mapTreeViewPreview.MarkAllNodes(True)

        PullStatus()


        If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

            CommitSynch()

        End If
    End Sub

    Private Overloads Sub ClassesToCodeX(ByVal domainMaps As ArrayList)

        Dim newDomainMaps As New ArrayList
        Dim domainMap As IDomainMap
        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim newDomainMap As IDomainMap
        Dim newClassMap As IClassMap
        Dim newPropertyMap As IPropertyMap

        PushStatus("Calculating preview...")

        ClearMsgs()
        AddMsg("'From Model To Code' Synchronizer:")
        AddMsg("Generating preview...")

        DiscardSynch()

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        m_SynchMode = SynchModeEnum.ClassesToCode
        SetActionType(ActionTypeEnum.SynchAction)


        Try

            For Each domainMap In domainMaps

                'newDomainMap = New domainMap

                newDomainMap = domainMap.DeepClone()

                'For Each classMap In domainMap.ClassMaps

                '    newClassMap = New classMap

                '    classMap.Copy(newClassMap)

                '    newClassMap.DomainMap = newDomainMap

                '    For Each propertyMap In classMap.PropertyMaps

                '        newPropertyMap = New propertyMap

                '        propertyMap.Copy(newPropertyMap)

                '        newPropertyMap.ClassMap = newClassMap

                '    Next

                'Next

                newDomainMaps.Add(newDomainMap)

            Next

            AddMsg("Preview Generated!")

        Catch ex As Exception

            AddMsg("'Error! Failed Generating Preview! Reason:" & ex.Message)

            MsgBox("Could not generate preview! Reason: " & ex.Message)

        End Try


        mapTreeViewPreview.Nodes.Clear()

        For Each newDomainMap In newDomainMaps

            mapTreeViewPreview.LoadDomain(newDomainMap)

        Next

        Cursor.Current = MouseSet()

        ShowSynchTree()

        mapTreeViewPreview.MarkAllNodes(True)

        PullStatus()


        If m_ApplicationSettings.OptionSettings.SynchronizationSettings.AutoAcceptPreview Then

            CommitSynch()

        End If

    End Sub

#End Region

#Region " Synchronization Commit "

    Private Sub CommitSynch()

        Select Case m_SynchMode

            Case SynchModeEnum.SourceToTables

                CommitSynchSourceToTables()

            Case SynchModeEnum.TablesToSource

                CommitSynchTablesToSource()

            Case SynchModeEnum.ClassesToTables

                CommitSynchClassesToTables()

            Case SynchModeEnum.TablesToClasses

                CommitSynchTablesToClasses()

            Case SynchModeEnum.CodeToClasses

                CommitSynchCodeToClasses()

            Case SynchModeEnum.ClassesToCode

                CommitSynchClassesToCode()

        End Select

    End Sub

    Private Sub DiscardSynch()

        treePreviewClassesToCode.Nodes.Clear()

        treePreviewClassesToCode.Visible = False

        mapTreeViewPreview.Nodes.Clear()

        mapTreeViewPreview.ClearUnCheckedNodesMemory()

        ClearSynch()

    End Sub


    Private Sub CommitSynchSourceToTables()

        Dim sourceDomainMaps As ArrayList
        Dim sourceDomainMap As IDomainMap
        Dim targetDomainMap As IDomainMap

        PushStatus("Committing synchronization...")

        'AddMsg("'From Data Source To Model' Synchronizer:")
        AddMsg("Committing Synchronization...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        sourceDomainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        For Each sourceDomainMap In sourceDomainMaps

            targetDomainMap = mapTreeView.GetDomainMap(sourceDomainMap.Name)

            If Not targetDomainMap Is Nothing Then

                MapServices.MargeDomains(targetDomainMap, sourceDomainMap)

                targetDomainMap.Dirty = True

            End If

        Next

        Cursor.Current = MouseSet()

        DiscardSynch()

        PullStatus()

        AddMsg("Synchronization complete!")

        'tabControlTools.SelectedIndex = 0

        VerifyMap()

        RefreshAll()

    End Sub

    Private Sub RefreshSynchTablesToSourcePreviewDTD()

        Dim domainMaps As ArrayList
        Dim sourceDomainMap As IDomainMap
        Dim sourceMap As ISourceMap
        Dim userDoc As UserDocTabPage

        'sourceDomainMap = mapTreeViewPreview.CloneMarkedNodes(True)
        domainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        Dim dtd As String
        Dim title As String
        Dim ok As Boolean

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        For Each sourceDomainMap In domainMaps

            For Each sourceMap In sourceDomainMap.SourceMaps

                Select Case sourceMap.SourceType

                    Case SourceType.MSAccess

                        dtd = m_TablesToSourceMdb.SourceToDTDEvolve(sourceMap)

                    Case SourceType.MSSqlServer

                        dtd = m_TablesToSourceSql.SourceToDTDEvolve(sourceMap)

                End Select

                title = "DDL For updating data source '" & sourceMap.ConnectionString & "' from model data source '" & sourceDomainMap.Name & ":" & sourceMap.Name & "'."

                ok = True

                For Each userDoc In tabControlPreviewDoc.TabPages

                    If userDoc.DocumentType = MainDocumentType.DTD Then

                        If userDoc.Title = title Then

                            userDoc.DocumentText = dtd

                            ok = False

                        End If

                    End If

                Next

                If ok Then

                    AddNewPreviewDocument(dtd, sourceMap.Name & ".sql", title, MainDocumentType.DTD)

                End If

            Next

        Next

        Cursor.Current = MouseSet()

    End Sub


    Private Sub CommitSynchTablesToSource()

        Dim sourceDomainMaps As ArrayList
        Dim sourceDomainMap As IDomainMap
        Dim sourceMap As ISourceMap
        Dim dtd As String

        Dim addErrInfo As String
        Dim ok As Boolean

        PushStatus("Committing synchronization...")

        'AddMsg("'From Model To Data Source' Synchronizer:")
        AddMsg("Committing Synchronization...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        sourceDomainMap = mapTreeViewPreview.CloneMarkedNodes(True)

        sourceDomainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)


        For Each sourceDomainMap In sourceDomainMaps

            For Each sourceMap In sourceDomainMap.SourceMaps

                ok = True

                If Not TestConnection(sourceMap, True, True, addErrInfo) Then

                    ok = False

                    Try

                        FormattingServices.VerifyConnectionParameters(sourceMap.ConnectionString, sourceMap.ProviderType, sourceMap.SourceType)

                    Catch ex As Exception

                        addErrInfo = ex.Message & vbCrLf & vbCrLf & addErrInfo

                    End Try

                    If Len(addErrInfo) > 0 Then addErrInfo = vbCrLf & vbCrLf & "Error: " & addErrInfo

                    MsgBox("Source '" & sourceMap.DomainMap.Name & "." & sourceMap.Name & "' could not be contacted! Skipping domain map '" & sourceMap.DomainMap.Name & "'!" & addErrInfo)

                End If

                If ok Then

                    Try

                        Select Case sourceMap.SourceType

                            Case SourceType.MSSqlServer

                                m_TablesToSourceSql.CommitTablesToSource(sourceMap)

                            Case SourceType.MSAccess

                                If m_RunPreviewDTD Then

                                    m_TablesToSourceMdb.CommitTablesToSource(sourceMap, dtd)

                                Else

                                    m_TablesToSourceMdb.CommitTablesToSource(sourceMap)

                                End If

                        End Select

                    Catch ex As Exception

                        MsgBox("Error! Exception occurred when communicating with source '" & sourceMap.DomainMap.Name & "." & sourceMap.Name & "':" & vbCrLf & vbCrLf & ex.Message)

                    End Try

                End If


            Next

        Next

        Cursor.Current = MouseSet()

        DiscardSynch()

        PullStatus()

        AddMsg("Synchronization complete!")

        'tabControlTools.SelectedIndex = 0

        VerifyMap()

        RefreshAll()

    End Sub

    Private Sub CommitSynchClassesToTables()

        Dim sourceDomainMaps As ArrayList
        Dim sourceDomainMap As IDomainMap
        Dim targetDomainMap As IDomainMap

        PushStatus("Committing synchronization...")

        'AddMsg("'From Classes To Tables' Synchronizer:")
        AddMsg("Committing Synchronization...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        sourceDomainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        For Each sourceDomainMap In sourceDomainMaps

            targetDomainMap = mapTreeView.GetDomainMap(sourceDomainMap.Name)

            If Not targetDomainMap Is Nothing Then

                MapServices.MargeDomains(targetDomainMap, sourceDomainMap)

                targetDomainMap.Dirty = True

            End If

        Next

        Cursor.Current = MouseSet()

        DiscardSynch()

        PullStatus()

        AddMsg("Synchronization complete!")

        'tabControlTools.SelectedIndex = 0

        VerifyMap()

        RefreshAll()

    End Sub


    Private Sub CommitSynchTablesToClasses()

        Dim sourceDomainMaps As ArrayList
        Dim sourceDomainMap As IDomainMap
        Dim targetDomainMap As IDomainMap

        PushStatus("Committing synchronization...")

        'AddMsg("'From Tables To Classes' Synchronizer:")
        AddMsg("Committing Synchronization...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        'sourceDomainMap = mapTreeViewPreview.CloneMarkedNodes(True)
        sourceDomainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        For Each sourceDomainMap In sourceDomainMaps

            targetDomainMap = mapTreeView.GetDomainMap(sourceDomainMap.Name)

            If Not targetDomainMap Is Nothing Then

                MapServices.MargeDomains(targetDomainMap, sourceDomainMap)

                targetDomainMap.Dirty = True

            End If

        Next

        Cursor.Current = MouseSet()

        DiscardSynch()

        PullStatus()

        AddMsg("Synchronization complete!")

        'tabControlTools.SelectedIndex = 0

        VerifyMap()

        RefreshAll()

    End Sub



    Private Sub CommitSynchCodeToClasses()

        Dim sourceDomainMaps As ArrayList
        Dim sourceDomainMap As IDomainMap
        Dim targetDomainMap As IDomainMap

        PushStatus("Committing synchronization...")

        'AddMsg("'From Code To Model' Synchronizer:")
        AddMsg("Committing Synchronization...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        sourceDomainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        For Each sourceDomainMap In sourceDomainMaps

            targetDomainMap = mapTreeView.GetDomainMap(sourceDomainMap.Name)

            If Not targetDomainMap Is Nothing Then

                MapServices.MargeDomains(targetDomainMap, sourceDomainMap)

                targetDomainMap.Dirty = True

            End If

        Next

        Cursor.Current = MouseSet()

        DiscardSynch()

        PullStatus()

        AddMsg("Synchronization complete!")

        'tabControlTools.SelectedIndex = 0

        VerifyMap()

        RefreshAll()

    End Sub

    Private Sub RemoveMarkedSynchNodes(ByVal value As Boolean)

        Dim domainMap As IDomainMap

        domainMap = mapTreeViewPreview.CloneMarkedNodes(Not value)

        mapTreeViewPreview.Nodes.Clear()

        If Not domainMap Is Nothing Then

            mapTreeViewPreview.LoadDomain(domainMap)

            mapTreeViewPreview.MarkAllNodes(True)

            Select Case m_SynchMode

                Case SynchModeEnum.TablesToSource

                    RefreshSynchTablesToSourcePreviewDTD()

            End Select

        Else

            DiscardSynch()

        End If

    End Sub

    Private Sub CommitSynchClassesToCode()

        Dim domainMap As IDomainMap
        Dim domainMaps As ArrayList
        Dim useCfg As ClassesToCodeConfig
        Dim projectFolderPath As String
        Dim folderPath As String
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
        Dim embeddedFiles As New ArrayList
        Dim ext As String
        Dim isUnSynched As Boolean
        Dim customCode As String

        Dim resource As IResource

        Dim preFileNode As PreviewFileNode
        Dim preFolderNode As PreviewFolderNode

        Dim folder As DirectoryInfo
        Dim logFile As fileInfo

        Dim fileType As SourceCodeFileTypeEnum

        Dim ok As Boolean

        'domainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        domainMaps = mapTreeView.GetAllDomainMaps

        PushStatus("Generating code from classes...")

        'AddMsg("'From Model To Code' Synchronizer:")
        AddMsg("Committing Synchronization...")

        Cursor.Current = MouseSet(Cursors.WaitCursor)

        Application.DoEvents()

        For Each domainMap In domainMaps

            preFolderNode = treePreviewClassesToCode.GetPreviewFolderNode(domainMap)

            ok = False

            If Not preFolderNode Is Nothing Then

                ok = preFolderNode.Checked

            End If

            If ok Then

                resource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

                Dim cfg As DomainConfig

                For Each cfg In resource.Configs

                    If cfg.IsActive Then

                        useCfg = cfg.ClassesToCodeConfig

                        Exit For

                    End If

                Next

                If useCfg Is Nothing Then

                    MsgBox("No tool configuration is selected as currently active for domain '" & domainMap.Name & "'! Please mark a tool configuration as active first!")

                    folderPath = ""

                Else


                    folderPath = useCfg.TargetFolder

                    'Make sure target folder exists

                    folderPath = ReplaceVars(folderPath)

                    If Len(folderPath) > 0 Then

                        Try

                            folder = New DirectoryInfo(folderPath)

                        Catch ex As Exception

                            folderPath = ""

                        End Try

                    End If

                    If Len(folderPath) > 0 Then

                        If Not folder.Exists() Then

                            Select Case MsgBox("The folder '" & folderPath & "' does not exist. Do you want to create this folder?", MsgBoxStyle.YesNoCancel)

                                Case MsgBoxResult.Yes

                                    folder.Create()

                                    LogMsg("Folder '" & folderPath & "' created.", TraceLevel.Info)

                                Case MsgBoxResult.No

                                    'Here we could offer the user to select a different folder...
                                    folderPath = ""

                                Case MsgBoxResult.Cancel

                                    folderPath = ""

                            End Select

                        End If

                    End If

                End If

                If Not folderPath = "" Then

                    'Prune source files
                    For Each src In useCfg.SourceCodeFiles

                        If Not File.Exists(src.FilePath) Then

                            removeSrc.Add(src)

                        End If

                    Next

                    For Each src In removeSrc

                        useCfg.SourceCodeFiles.Remove(src)

                    Next


                    '4 extra files: The project file, the AssemblyInfo file, the framework dll and the mappingfile
                    'If Len(folderPath) > 0 Or useCfg.SourceCodeFiles.Count = domainMap.ClassMaps.Count + 4 Then
                    If Len(folderPath) > 0 Or useCfg.SourceCodeFiles.Count = domainMap.ClassMaps.Count + 4 Then


                        SetupGenerateCode(domainMap)

                        'Generate typed collections

                        If useCfg.UseTypedCollections Then

                            For Each classMap In domainMap.ClassMaps

                                noWrite = False

                                src = useCfg.GetSourceCodeFile(classMap.Name & "Collection")

                                'If src Is Nothing Then

                                '    'fileName = folderPath & "\" & classMap.GetName
                                fileName = folderPath & "\" & classMap.Name & "Collection"

                                'Else

                                '    'fileName = New fileInfo(src.FilePath).DirectoryName & "\" & classMap.GetName
                                '    fileName = New fileInfo(src.FilePath).DirectoryName & "\" & classMap.Name

                                'End If

                                Select Case useCfg.TargetLanguage

                                    Case TargetLanguageEnum.CSharp

                                        fileName += ".cs"

                                    Case TargetLanguageEnum.VB

                                        fileName += ".vb"

                                    Case TargetLanguageEnum.Delphi

                                        fileName += ".pas"

                                End Select

                                preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                                If Not preFileNode Is Nothing Then

                                    If preFileNode.Checked Then

                                        If preFileNode.Parent.Checked Then

                                            If src Is Nothing Then

                                                src = useCfg.SetSourceCodeFile(classMap.Name & "Collection", fileName)

                                                Select Case useCfg.TargetLanguage

                                                    Case TargetLanguageEnum.CSharp

                                                        src.FileType = SourceCodeFileTypeEnum.CSharp

                                                    Case TargetLanguageEnum.Delphi

                                                        src.FileType = SourceCodeFileTypeEnum.Delphi

                                                    Case TargetLanguageEnum.VB

                                                        src.FileType = SourceCodeFileTypeEnum.VB

                                                End Select

                                            Else

                                                If Not CmpSourceCodeAndTargetLanguageEnums(src.FileType, useCfg.TargetLanguage) Then

                                                    MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

                                                    noWrite = True

                                                Else

                                                    If Not src.IsSynched Then

                                                        If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                            noWrite = True

                                                        End If

                                                    End If

                                                End If

                                                If Not fileName = src.FilePath Then

                                                    If MsgBox("Due to changes made in your model, it is suggested that you change the name of the file '" & src.FilePath & "' to '" & fileName & "'. Do you want to make this change?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                        Try

                                                            File.Move(src.FilePath, fileName)

                                                            src.FilePath = fileName

                                                        Catch ex As Exception

                                                        End Try

                                                    End If

                                                End If

                                                fileName = src.FilePath

                                            End If

                                            If Not noWrite Then

                                                Select Case useCfg.TargetLanguage

                                                    Case TargetLanguageEnum.CSharp

                                                        code = m_ClassesToCodeCs.ClassToTypedCollection(classMap, False, False)

                                                    Case TargetLanguageEnum.VB

                                                        code = m_ClassesToCodeVb.ClassToTypedCollection(classMap, False, False)

                                                    Case TargetLanguageEnum.Delphi

                                                        code = m_ClassesToCodeDelphi.ClassToTypedCollection(classMap, False, False)

                                                End Select

                                                fileWriter = File.CreateText(fileName)

                                                fileWriter.Write(code)

                                                fileWriter.Close()

                                                logFile = New fileInfo(fileName)

                                                LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                                classMapsAndFiles(classMap.Name & "Collection") = fileName

                                                src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                            End If


                                        End If

                                    End If

                                End If

                            Next

                        End If


                        'Generate classes

                        For Each classMap In domainMap.ClassMaps

                            noWrite = False

                            customCode = ""

                            src = useCfg.GetSourceCodeFile(classMap)

                            'If src Is Nothing Then

                            '    'fileName = folderPath & "\" & classMap.GetName
                            fileName = folderPath & "\" & classMap.Name

                            'Else

                            '    'fileName = New fileInfo(src.FilePath).DirectoryName & "\" & classMap.GetName
                            '    fileName = New fileInfo(src.FilePath).DirectoryName & "\" & classMap.Name

                            'End If

                            Select Case useCfg.TargetLanguage

                                Case TargetLanguageEnum.CSharp

                                    fileName += ".cs"

                                Case TargetLanguageEnum.VB

                                    fileName += ".vb"

                                Case TargetLanguageEnum.Delphi

                                    fileName += ".pas"

                            End Select

                            preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                            If Not preFileNode Is Nothing Then

                                If preFileNode.Checked Then

                                    If preFileNode.Parent.Checked Then

                                        If src Is Nothing Then

                                            src = useCfg.SetSourceCodeFile(classMap, fileName)

                                            Select Case useCfg.TargetLanguage

                                                Case TargetLanguageEnum.CSharp

                                                    src.FileType = SourceCodeFileTypeEnum.CSharp

                                                Case TargetLanguageEnum.Delphi

                                                    src.FileType = SourceCodeFileTypeEnum.Delphi

                                                Case TargetLanguageEnum.VB

                                                    src.FileType = SourceCodeFileTypeEnum.VB

                                            End Select

                                        Else

                                            If Not CmpSourceCodeAndTargetLanguageEnums(src.FileType, useCfg.TargetLanguage) Then

                                                MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

                                                noWrite = True

                                            Else

                                                If Not src.IsSynched Then

                                                    If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                        noWrite = True

                                                    End If

                                                End If

                                            End If

                                            customCode = GetCustomCode(src, classMap)

                                            If Not fileName = src.FilePath Then

                                                If MsgBox("Due to changes made in your model, it is suggested that you change the name of the file '" & src.FilePath & "' to '" & fileName & "'. Do you want to make this change?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                    Try

                                                        File.Move(src.FilePath, fileName)

                                                        src.FilePath = fileName

                                                    Catch ex As Exception

                                                    End Try

                                                End If

                                            End If

                                            fileName = src.FilePath

                                        End If

                                        If Not noWrite Then

                                            Select Case useCfg.TargetLanguage

                                                Case TargetLanguageEnum.CSharp

                                                    code = m_ClassesToCodeCs.ClassToCode(classMap, False, False, customCode)

                                                Case TargetLanguageEnum.VB

                                                    code = m_ClassesToCodeVb.ClassToCode(classMap, False, False, customCode)

                                                Case TargetLanguageEnum.Delphi

                                                    code = m_ClassesToCodeDelphi.ClassToCode(classMap, False, False, customCode)

                                            End Select

                                            fileWriter = File.CreateText(fileName)

                                            fileWriter.Write(code)

                                            fileWriter.Close()

                                            logFile = New fileInfo(fileName)

                                            LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                            classMapsAndFiles(classMap) = fileName

                                            src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                        End If


                                    End If

                                End If

                            End If

                        Next




                        'AssemblyInfo

                        noWrite = False

                        Select Case useCfg.TargetLanguage

                            Case TargetLanguageEnum.CSharp

                                code = m_ClassesToCodeCs.DomainToAssemblyInfo(domainMap)
                                ext = ".cs"

                            Case TargetLanguageEnum.VB

                                code = m_ClassesToCodeVb.DomainToAssemblyInfo(domainMap)
                                ext = ".vb"

                            Case TargetLanguageEnum.Delphi

                                code = m_ClassesToCodeDelphi.DomainToAssemblyInfo(domainMap)
                                ext = ".dpr"

                        End Select

                        src = useCfg.GetSourceCodeFile("AssemblyInfo")

                        'If src Is Nothing Then

                        '    'fileName = projectFolderPath & "\AssemblyInfo" & ext
                        fileName = folderPath & "\AssemblyInfo" & ext

                        'Else

                        '    fileName = New fileInfo(src.FilePath).DirectoryName & "\AssemblyInfo" & ext

                        'End If

                        preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                        If Not preFileNode Is Nothing Then

                            If preFileNode.Checked Then

                                If preFileNode.Parent.Checked Then

                                    If src Is Nothing Then

                                        src = useCfg.SetSourceCodeFile("AssemblyInfo", fileName)

                                        Select Case useCfg.TargetLanguage

                                            Case TargetLanguageEnum.CSharp

                                                src.FileType = SourceCodeFileTypeEnum.CSharp

                                            Case TargetLanguageEnum.VB

                                                src.FileType = SourceCodeFileTypeEnum.VB

                                            Case TargetLanguageEnum.Delphi

                                                src.FileType = SourceCodeFileTypeEnum.Delphi

                                        End Select

                                    Else

                                        If Not CmpSourceCodeAndTargetLanguageEnums(src.FileType, useCfg.TargetLanguage) Then

                                            MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

                                            noWrite = True

                                        Else

                                            If Not src.IsSynched Then

                                                If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                    noWrite = True

                                                End If

                                            End If

                                        End If

                                        fileName = src.FilePath

                                    End If

                                    If Not noWrite Then

                                        fileWriter = File.CreateText(fileName)

                                        fileWriter.Write(code)

                                        fileWriter.Close()

                                        logFile = New fileInfo(fileName)

                                        LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                        src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                    End If

                                End If

                            End If

                        End If



                        'Mapping file

                        noWrite = False

                        Select Case useCfg.TargetMapper

                            Case TargetMapperEnum.NPersist

                                code = GetDomainXml(domainMap)
                                ext = ".npersist"
                                fileType = SourceCodeFileTypeEnum.NPersist

                            Case TargetMapperEnum.NHibernate

                                If useCfg.XmlFilePerClass Then

                                    For Each classMap In m_NPersistToNHibernate.GetRootClasses(domainMap)

                                        code = m_NPersistToNHibernate.Serialize(classMap)
                                        ext = ".hbm.xml"
                                        fileType = SourceCodeFileTypeEnum.Xml

                                        src = useCfg.GetSourceCodeFile("MapFile." & classMap.Name)

                                        'fileName = folderPath & "\" & classMap.Name & ext
                                        fileName = folderPath & "\" & classMap.GetFullName & ext

                                        preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                                        If Not preFileNode Is Nothing Then

                                            If preFileNode.Checked Then

                                                If preFileNode.Parent.Checked Then

                                                    If src Is Nothing Then

                                                        src = useCfg.SetSourceCodeFile("MapFile." & classMap.Name, fileName)

                                                        src.FileType = fileType

                                                    Else

                                                        If Not src.FileType = fileType Then

                                                            MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

                                                            noWrite = True

                                                        Else

                                                            If Not src.IsSynched Then

                                                                If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                                    noWrite = True

                                                                End If

                                                            End If

                                                        End If

                                                        fileName = src.FilePath

                                                    End If

                                                    If Not noWrite Then

                                                        fileWriter = File.CreateText(fileName)

                                                        fileWriter.Write(code)

                                                        fileWriter.Close()

                                                        logFile = New fileInfo(fileName)

                                                        LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                                        src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                                        embeddedFiles.Add(fileName)

                                                    End If

                                                End If

                                            End If

                                        End If

                                    Next

                                    code = ""
                                    noWrite = True

                                Else

                                    code = m_NPersistToNHibernate.Serialize(domainMap)
                                    ext = ".hbm.xml"
                                    fileType = SourceCodeFileTypeEnum.Xml

                                End If

                            Case Else

                                code = ""
                                noWrite = True

                        End Select

                        If Len(code) > 0 Then

                            src = useCfg.GetSourceCodeFile("MapFile")
                            fileName = folderPath & "\" & domainMap.Name & ext

                            preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                            If Not preFileNode Is Nothing Then

                                If preFileNode.Checked Then

                                    If preFileNode.Parent.Checked Then

                                        If src Is Nothing Then

                                            src = useCfg.SetSourceCodeFile("MapFile", fileName)

                                            src.FileType = fileType

                                        Else

                                            If Not src.FileType = fileType Then

                                                MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

                                                noWrite = True

                                            Else

                                                If Not src.IsSynched Then

                                                    If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                        noWrite = True

                                                    End If

                                                End If

                                            End If

                                            fileName = src.FilePath

                                        End If

                                        If Not noWrite Then

                                            fileWriter = File.CreateText(fileName)

                                            fileWriter.Write(code)

                                            fileWriter.Close()

                                            logFile = New fileInfo(fileName)

                                            LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                            src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                            embeddedFiles.Add(fileName)

                                        End If

                                    End If

                                End If

                            End If

                        End If




                        'Generate project

                        noWrite = False

                        src = useCfg.GetSourceCodeFile(domainMap)

                        'If src Is Nothing Then

                        fileName = folderPath & "\" & domainMap.Name

                        'Else

                        '    fileName = New fileInfo(src.FilePath).DirectoryName & "\" & domainMap.Name

                        'End If

                        Select Case useCfg.TargetLanguage

                            Case TargetLanguageEnum.CSharp

                                fileName += ".csproj"

                            Case TargetLanguageEnum.VB

                                fileName += ".vbproj"

                            Case TargetLanguageEnum.Delphi

                                fileName += ".bdsproj"

                        End Select

                        preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                        If Not preFileNode Is Nothing Then

                            If preFileNode.Checked Then

                                If preFileNode.Parent.Checked Then

                                    If src Is Nothing Then

                                        projectFolderPath = folderPath

                                        src = useCfg.SetSourceCodeFile(domainMap, fileName)

                                        Select Case useCfg.TargetLanguage

                                            Case TargetLanguageEnum.CSharp

                                                src.FileType = SourceCodeFileTypeEnum.CSharp

                                            Case TargetLanguageEnum.VB

                                                src.FileType = SourceCodeFileTypeEnum.VB

                                            Case TargetLanguageEnum.Delphi

                                                src.FileType = SourceCodeFileTypeEnum.Delphi

                                        End Select


                                    Else

                                        If Not CmpSourceCodeAndTargetLanguageEnums(src.FileType, useCfg.TargetLanguage) Then

                                            MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

                                            noWrite = True

                                        Else

                                            If Not src.IsSynched Then

                                                If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                                    noWrite = True

                                                End If

                                            End If

                                        End If

                                        fileName = src.FilePath

                                        projectFolderPath = New fileInfo(fileName).DirectoryName

                                    End If

                                    If Not noWrite Then

                                        Select Case useCfg.TargetLanguage

                                            Case TargetLanguageEnum.CSharp

                                                'code = m_ClassesToCodeCs.DomainToProject(domainMap, projectFolderPath, classMapsAndFiles)
                                                code = m_ClassesToCodeCs.DomainToProject(domainMap, folderPath, classMapsAndFiles, embeddedFiles)

                                            Case TargetLanguageEnum.VB

                                                'code = m_ClassesToCodeVb.DomainToProject(domainMap, projectFolderPath, classMapsAndFiles)
                                                code = m_ClassesToCodeVb.DomainToProject(domainMap, folderPath, classMapsAndFiles, embeddedFiles)

                                            Case TargetLanguageEnum.Delphi

                                                'code = m_ClassesToCodeDelphi.DomainToProject(domainMap, projectFolderPath, classMapsAndFiles)
                                                code = m_ClassesToCodeDelphi.DomainToProject(domainMap, folderPath, classMapsAndFiles, embeddedFiles)

                                        End Select

                                        fileWriter = File.CreateText(fileName)

                                        fileWriter.Write(code)

                                        fileWriter.Close()

                                        logFile = New fileInfo(fileName)

                                        LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                        src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                    End If

                                End If

                            End If

                        End If





                        If useCfg.TargetMapper = TargetMapperEnum.NPersist Then

                            If Not useCfg.GeneratePOCO Then


                                'Framework dll

                                'If Not Directory.Exists(projectFolderPath & "\bin") Then

                                '    Directory.CreateDirectory(projectFolderPath & "\bin")

                                'End If

                                If Not Directory.Exists(folderPath & "\bin") Then

                                    Directory.CreateDirectory(folderPath & "\bin")

                                End If

                                noWrite = False

                                src = useCfg.GetSourceCodeFile("Puzzle.NPersist.Framework.dll")

                                'If src Is Nothing Then

                                fileName = folderPath & "\bin\Puzzle.NPersist.framework.dll"

                                'Else

                                '    fileName = New fileInfo(src.FilePath).DirectoryName & "\Puzzle.NPersist.framework.dll"

                                'End If

                                preFileNode = treePreviewClassesToCode.GetPreviewFileNode(fileName)

                                If Not preFileNode Is Nothing Then

                                    If preFileNode.Checked Then

                                        If preFileNode.Parent.Checked Then

                                            isUnSynched = False

                                            If Not src Is Nothing Then

                                                If Not src.IsSynched Then

                                                    isUnSynched = True

                                                End If

                                            Else

                                                isUnSynched = True

                                            End If


                                            'If Not File.Exists(fileName) Or isUnSynched Then

                                            If File.Exists(Application.StartupPath & "\Puzzle.NPersist.Framework.dll") Then

                                                Try

                                                    File.Copy(Application.StartupPath & "\Puzzle.NPersist.Framework.dll", fileName, True)

                                                Catch ex As Exception

                                                    MsgBox("Could not copy Puzzle.NPersist.Framework Assembly to target location! Reason: " & ex.Message)

                                                    noWrite = True

                                                End Try

                                            Else

                                                MsgBox("Could not find file 'Puzzle.NPersist.Framework.dll' in the startup folder of this application and could therefor not make a copy of that file to the target folder! Please copy the file manually to the bin folder under the target folder for your output project file. You should probably want to put a copy of the dll in the startup folder of this application for your next synchronization!")

                                                noWrite = True

                                            End If

                                            'End If

                                            If Not noWrite Then

                                                If src Is Nothing Then

                                                    src = useCfg.SetSourceCodeFile("Puzzle.NPersist.Framework.dll", fileName)

                                                    src.FileType = SourceCodeFileTypeEnum.Other

                                                Else

                                                    If Not src.FileType = SourceCodeFileTypeEnum.Other Then

                                                        MsgBox("Incorrect file type for NPersist framework assembly! Will not write to file " & src.FilePath)

                                                        noWrite = True

                                                    Else

                                                        src.FilePath = fileName

                                                    End If


                                                End If

                                                If Not noWrite Then

                                                    logFile = New fileInfo(fileName)

                                                    LogMsg("File '" & logFile.Name & "' created in folder '" & logFile.Directory.FullName & "'.", TraceLevel.Info)

                                                    src.LastWrittenTo = File.GetLastWriteTime(fileName)

                                                End If

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If




                    End If

                End If


            End If

        Next

        m_ProjectDirty = True

        Cursor.Current = MouseSet()

        DiscardSynch()

        PullStatus()

        AddMsg("Synchronization complete!")

        'tabControlTools.SelectedIndex = 0

        VerifyMap()

        RefreshAll()

    End Sub

    Private Function CmpSourceCodeAndTargetLanguageEnums(ByVal sourceCodeFileType As SourceCodeFileTypeEnum, ByVal targetLanguage As TargetLanguageEnum) As Boolean

        Select Case targetLanguage

            Case TargetLanguageEnum.CSharp

                If sourceCodeFileType = SourceCodeFileTypeEnum.CSharp Then Return True

            Case TargetLanguageEnum.VB

                If sourceCodeFileType = SourceCodeFileTypeEnum.VB Then Return True

            Case TargetLanguageEnum.Delphi

                If sourceCodeFileType = SourceCodeFileTypeEnum.Delphi Then Return True

        End Select

        Return False

    End Function

    Private Sub CommitSynchClassesToCodeX()

        '        Dim domainMap As IDomainMap
        '        Dim domainMaps As ArrayList
        '        Dim useCfg As ClassesToCodeConfig
        '        Dim projectFolderPath As String
        '        Dim folderPath As String
        '        Dim src As SourceCodeFile
        '        Dim removeSrc As New ArrayList
        '        Dim fileInfo As fileInfo
        '        Dim newDomainMap As IDomainMap = New domainMap
        '        Dim fileName As String
        '        Dim code As String
        '        Dim classMap As IClassMap
        '        Dim fileWriter As StreamWriter
        '        Dim noWrite As Boolean
        '        Dim classMapsAndFiles As New Hashtable
        '        Dim ext As String
        '        Dim isUnSynched As Boolean
        '        Dim customCode As String

        '        Dim resource As IResource

        '        domainMaps = mapTreeViewPreview.GetDomainMapsForMarkedNodes(True)

        '        PushStatus("Generating code from classes...")

        '        'AddMsg("'From Model To Code' Synchronizer:")
        '        AddMsg("Committing Synchronization...")

        '        Cursor.Current = MouseSet(Cursors.WaitCursor)

        '        Application.DoEvents()

        '        For Each domainMap In domainMaps

        '            resource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

        '            Dim cfg As DomainConfig

        '            For Each cfg In resource.Configs

        '                If cfg.IsActive Then

        '                    useCfg = cfg.ClassesToCodeConfig

        '                    Exit For

        '                End If

        '            Next

        '            If useCfg Is Nothing Then

        '                MsgBox("No tool configuration is selected as currently active for domain '" & domainMap.Name & "'! Please mark a tool configuration as active first!")

        '                folderPath = ""

        '            End If

        '            'Prune source files

        '            For Each src In useCfg.SourceCodeFiles

        '                If Not File.Exists(src.FilePath) Then

        '                    removeSrc.Add(src)

        '                End If

        '            Next

        '            For Each src In removeSrc

        '                useCfg.SourceCodeFiles.Remove(src)

        '            Next

        '            '4 extra files: The project file, the AssemblyInfo file, the framework dll and the norm dll
        '            'If useCfg.SourceCodeFiles.Count < domainMap.ClassMaps.Count + 4 Then
        '            If useCfg.SourceCodeFiles.Count < domainMap.ClassMaps.Count + 3 Then

        '                If useCfg.SourceCodeFiles.Count > 0 Then

        '                    For Each src In useCfg.SourceCodeFiles

        '                        fileInfo = New fileInfo(src.FilePath)

        '                        FolderBrowserDialog1.SelectedPath = fileInfo.DirectoryName

        '                        Exit For

        '                    Next

        '                End If

        '                '*THIS* Is when you should use GOTO
        'Loopy:

        '                FolderBrowserDialog1.Description = "Please select a target folder for putting new source code files in"

        '                folderPath = ""

        '                If Not FolderBrowserDialog1.ShowDialog(Me) = DialogResult.Cancel Then

        '                    folderPath = FolderBrowserDialog1.SelectedPath

        '                End If

        '                If Not folderPath = "" Then

        '                    Select Case MsgBox("Do you want to place new files in the folder '" & folderPath & "'?", MsgBoxStyle.YesNoCancel, "Select target folder for new files")

        '                        Case MsgBoxResult.Yes

        '                        Case MsgBoxResult.No

        '                            GoTo Loopy

        '                        Case MsgBoxResult.Cancel

        '                            folderPath = ""

        '                    End Select

        '                End If

        '            End If

        '            '4 extra files: The project file, the AssemblyInfo file, the framework dll and the norm dll
        '            'If Len(folderPath) > 0 Or useCfg.SourceCodeFiles.Count = domainMap.ClassMaps.Count + 4 Then
        '            If Len(folderPath) > 0 Or useCfg.SourceCodeFiles.Count = domainMap.ClassMaps.Count + 3 Then


        '                SetupGenerateCode(domainMap)


        '                'Generate classes

        '                For Each classMap In domainMap.ClassMaps

        '                    noWrite = False

        '                    customCode = ""

        '                    src = useCfg.GetSourceCodeFile(classMap)

        '                    If src Is Nothing Then

        '                        fileName = folderPath & "\" & classMap.GetName

        '                    Else

        '                        fileName = New fileInfo(src.FilePath).DirectoryName & "\" & classMap.GetName

        '                    End If

        '                    Select Case useCfg.TargetLanguage

        '                        Case TargetLanguageEnum.CSharp

        '                            fileName += ".cs"

        '                        Case TargetLanguageEnum.VB

        '                            fileName += ".vb"

        '                        Case TargetLanguageEnum.Delphi

        '                            fileName += ".pas"

        '                    End Select

        '                    If src Is Nothing Then

        '                        src = useCfg.SetSourceCodeFile(classMap, fileName)

        '                        Select Case useCfg.TargetLanguage

        '                            Case TargetLanguageEnum.CSharp

        '                                src.FileType = SourceCodeFileTypeEnum.CSharp

        '                            Case TargetLanguageEnum.VB

        '                                src.FileType = SourceCodeFileTypeEnum.VB

        '                            Case TargetLanguageEnum.Delphi

        '                                src.FileType = SourceCodeFileTypeEnum.Delphi

        '                        End Select

        '                    Else

        '                        If Not CmpSourceCodeAndTargetLanguageEnums(src.FileType, useCfg.TargetLanguage) Then

        '                            MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

        '                            noWrite = True

        '                        Else

        '                            If Not src.IsSynched Then

        '                                If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

        '                                    noWrite = True

        '                                End If

        '                            End If

        '                        End If

        '                        customCode = GetCustomCode(src, classMap)

        '                        If Not fileName = src.FilePath Then

        '                            If MsgBox("Due to changes made in your model, it is suggested that you change the name of the file '" & src.FilePath & "' to '" & fileName & "'. Do you want to make this change?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

        '                                Try

        '                                    File.Move(src.FilePath, fileName)

        '                                    src.FilePath = fileName

        '                                Catch ex As Exception

        '                                End Try

        '                            End If

        '                        End If

        '                        fileName = src.FilePath

        '                    End If

        '                    If Not noWrite Then

        '                        Select Case useCfg.TargetLanguage

        '                            Case TargetLanguageEnum.CSharp

        '                                code = m_ClassesToCodeCs.ClassToCode(classMap, False, False, customCode)

        '                            Case TargetLanguageEnum.VB

        '                                code = m_ClassesToCodeVb.ClassToCode(classMap, False, False, customCode)

        '                            Case TargetLanguageEnum.Delphi

        '                                code = m_ClassesToCodeDelphi.ClassToCode(classMap, False, False, customCode)

        '                        End Select

        '                        fileWriter = File.CreateText(fileName)

        '                        fileWriter.Write(code)

        '                        fileWriter.Close()

        '                        classMapsAndFiles(classMap) = fileName

        '                        src.LastWrittenTo = File.GetLastWriteTime(fileName)

        '                    End If

        '                Next



        '                'Generate project

        '                noWrite = False

        '                src = useCfg.GetSourceCodeFile(domainMap)

        '                If src Is Nothing Then

        '                    fileName = folderPath & "\" & domainMap.Name

        '                    projectFolderPath = folderPath

        '                    Select Case useCfg.TargetLanguage

        '                        Case TargetLanguageEnum.CSharp

        '                            fileName += ".csproj"

        '                        Case TargetLanguageEnum.VB

        '                            fileName += ".vbproj"

        '                        Case TargetLanguageEnum.Delphi

        '                            fileName += ".bdsproj"

        '                    End Select

        '                    src = useCfg.SetSourceCodeFile(domainMap, fileName)

        '                    Select Case useCfg.TargetLanguage

        '                        Case TargetLanguageEnum.CSharp

        '                            src.FileType = SourceCodeFileTypeEnum.CSharp

        '                        Case TargetLanguageEnum.VB

        '                            src.FileType = SourceCodeFileTypeEnum.VB

        '                        Case TargetLanguageEnum.Delphi

        '                            src.FileType = SourceCodeFileTypeEnum.Delphi

        '                    End Select

        '                Else

        '                    If Not CmpSourceCodeAndTargetLanguageEnums src.FileType = useCfg.TargetLanguage Then

        '                        MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

        '                        noWrite = True

        '                    Else

        '                        If Not src.IsSynched Then

        '                            If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

        '                                noWrite = True

        '                            End If

        '                        End If

        '                    End If

        '                    fileName = src.FilePath

        '                    projectFolderPath = New fileInfo(fileName).DirectoryName

        '                End If

        '                If Not noWrite Then

        '                    Select Case useCfg.TargetLanguage

        '                        Case SourceCodeFileTypeEnum.CSharp

        '                            code = m_ClassesToCodeCs.DomainToProject(domainMap, projectFolderPath, classMapsAndFiles)

        '                        Case SourceCodeFileTypeEnum.VB

        '                            code = m_ClassesToCodeVb.DomainToProject(domainMap, projectFolderPath, classMapsAndFiles)

        '                        Case SourceCodeFileTypeEnum.Delphi

        '                            code = m_ClassesToCodeDelphi.DomainToProject(domainMap, projectFolderPath, classMapsAndFiles)

        '                    End Select

        '                    fileWriter = File.CreateText(fileName)

        '                    fileWriter.Write(code)

        '                    fileWriter.Close()

        '                    src.LastWrittenTo = File.GetLastWriteTime(fileName)

        '                End If



        '                'AssemblyInfo

        '                noWrite = False

        '                Select Case useCfg.TargetLanguage

        '                    Case SourceCodeFileTypeEnum.CSharp

        '                        code = m_ClassesToCodeCs.DomainToAssemblyInfo(domainMap)
        '                        ext = ".cs"

        '                    Case SourceCodeFileTypeEnum.VB

        '                        code = m_ClassesToCodeVb.DomainToAssemblyInfo(domainMap)
        '                        ext = ".vb"

        '                    Case SourceCodeFileTypeEnum.Delphi

        '                        code = m_ClassesToCodeDelphi.DomainToAssemblyInfo(domainMap)
        '                        ext = ".dpr"

        '                End Select

        '                src = useCfg.GetSourceCodeFile("AssemblyInfo")

        '                If src Is Nothing Then

        '                    fileName = projectFolderPath & "\AssemblyInfo" & ext

        '                    src = useCfg.SetSourceCodeFile("AssemblyInfo", fileName)

        '                    src.FileType = useCfg.TargetLanguage

        '                Else

        '                    If Not src.FileType = useCfg.TargetLanguage Then

        '                        MsgBox("Source code language does not match target file type! Will not write to file " & src.FilePath)

        '                        noWrite = True

        '                    Else

        '                        If Not src.IsSynched Then

        '                            If Not MsgBox("Source code file '" & src.FilePath & "' has been modified since it was last written to! Do you want to overwrite this file now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

        '                                noWrite = True

        '                            End If

        '                        End If

        '                    End If

        '                    fileName = src.FilePath

        '                End If

        '                If Not noWrite Then

        '                    fileWriter = File.CreateText(fileName)

        '                    fileWriter.Write(code)

        '                    fileWriter.Close()

        '                    src.LastWrittenTo = File.GetLastWriteTime(fileName)

        '                End If


        '                'Framework dll

        '                If Not Directory.Exists(projectFolderPath & "\bin") Then

        '                    Directory.CreateDirectory(projectFolderPath & "\bin")

        '                End If

        '                noWrite = False

        '                fileName = projectFolderPath & "\bin\Puzzle.NPersist.framework.dll"

        '                src = useCfg.GetSourceCodeFile("Puzzle.NPersist.Framework.dll")

        '                isUnSynched = False

        '                If Not src Is Nothing Then

        '                    If Not src.IsSynched Then

        '                        isUnSynched = True

        '                    End If

        '                Else

        '                    isUnSynched = True

        '                End If


        '                If Not File.Exists(fileName) Or isUnSynched Then

        '                    If File.Exists(Application.StartupPath & "\Puzzle.NPersist.framework.dll") Then

        '                        Try

        '                            File.Copy(Application.StartupPath & "\Puzzle.NPersist.framework.dll", fileName, True)

        '                        Catch ex As Exception

        '                            MsgBox("Could not copy Puzzle.NPersist.Framework Assembly to target location! Reason: " & ex.Message)

        '                            noWrite = True

        '                        End Try

        '                    Else

        '                        MsgBox("Could not find file 'Puzzle.NPersist.framework.dll' in the startup folder of this application and could therefor not make a copy of that file to the target folder! Please copy the file manually to the bin folder under the target folder for your output project file. You should probably want to put a copy of the dll in the startup folder of this application for your next synchronization!")

        '                        noWrite = True

        '                    End If

        '                End If

        '                If Not noWrite Then

        '                    If src Is Nothing Then

        '                        src = useCfg.SetSourceCodeFile("Puzzle.NPersist.Framework.dll", fileName)

        '                        src.FileType = SourceCodeFileTypeEnum.Other

        '                    Else

        '                        If Not src.FileType = SourceCodeFileTypeEnum.Other Then

        '                            MsgBox("Incorrect file type for NPersist framework assembly! Will not write to file " & src.FilePath)

        '                            noWrite = True

        '                        Else

        '                            src.FilePath = fileName

        '                        End If


        '                    End If

        '                    If Not noWrite Then

        '                        src.LastWrittenTo = File.GetLastWriteTime(fileName)

        '                    End If

        '                End If



        '                'Norm dll

        '                'If Not Directory.Exists(projectFolderPath & "\bin") Then

        '                '    Directory.CreateDirectory(projectFolderPath & "\bin")

        '                'End If

        '                'noWrite = False

        '                'fileName = projectFolderPath & "\bin\norm.specification.dll"

        '                'src = useCfg.GetSourceCodeFile("Norm.Specification.dll")

        '                'isUnSynched = False

        '                'If Not src Is Nothing Then

        '                '    If Not src.IsSynched Then

        '                '        isUnSynched = True

        '                '    End If

        '                'Else

        '                '    isUnSynched = True

        '                'End If


        '                'If Not File.Exists(fileName) Or isUnSynched Then

        '                '    If File.Exists(Application.StartuNPath & "\norm.specification.dll") Then

        '                '        File.Copy(Application.StartuNPath & "\norm.specification.dll", fileName, True)

        '                '    Else

        '                '        MsgBox("Could not find file 'norm.specification.dll' in the startup folder of this application and could therefor not make a copy of that file to the target folder! Please copy the file manually to the bin folder under the target folder for your output project file. You should probably want to put a copy of the dll in the startup folder of this application for your next synchronization!")

        '                '        noWrite = True

        '                '    End If

        '                'End If

        '                'If Not noWrite Then

        '                '    If src Is Nothing Then

        '                '        src = useCfg.SetSourceCodeFile("Norm.Specification.dll", fileName)

        '                '        src.FileType = SourceCodeFileTypeEnum.Other

        '                '    Else

        '                '        If Not src.FileType = SourceCodeFileTypeEnum.Other Then

        '                '            MsgBox("Incorrect file type for norm specification assembly! Will not write to file " & src.FilePath)

        '                '            noWrite = True

        '                '        Else

        '                '            src.FilePath = fileName

        '                '        End If


        '                '    End If

        '                '    If Not noWrite Then

        '                '        src.LastWrittenTo = File.GetLastWriteTime(fileName)

        '                '    End If

        '                'End If


        '            End If

        '        Next

        '        m_ProjectDirty = True

        '        Cursor.Current = MouseSet()

        '        DiscardSynch()

        '        PullStatus()

        '        AddMsg("Synchronization complete!")

        '        tabControlTools.SelectedIndex = 0

        '        VerifyMap()

        '        RefreshAll()

    End Sub

#End Region

#End Region

#Region " Handle updates "


    Private Sub HandleUpdate(ByVal mapObject As Object, ByVal propertyName As String)

        If Not mapObject.GetType.GetInterface(GetType(ProjectModel.IProject).ToString) Is Nothing Then

            HandleProjectUpdate(mapObject, propertyName)

        ElseIf Not mapObject.GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

            HandleDomainUpdate(mapObject, propertyName)

        ElseIf Not mapObject.GetType.GetInterface(GetType(IClassMap).ToString) Is Nothing Then

            HandleClassUpdate(mapObject, propertyName)

        ElseIf Not mapObject.GetType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing Then

            HandlePropertyUpdate(mapObject, propertyName)

        ElseIf Not mapObject.GetType.GetInterface(GetType(ITableMap).ToString) Is Nothing Then

            HandleTableUpdate(mapObject, propertyName)

        ElseIf Not mapObject.GetType.GetInterface(GetType(IColumnMap).ToString) Is Nothing Then

            'HandleColumnUpdate(mapObject, propertyName)

        ElseIf mapObject.GetType Is GetType(DomainConfig) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(ClassesToCodeConfig) Then

            m_ProjectDirty = True

        End If

    End Sub

    Private Sub HandleProjectUpdate(ByVal project As IProject, ByVal propertyName As String)

        Select Case propertyName

            Case "Name"

                HandleProjectNameUpdate(project)

        End Select

    End Sub

    Private Sub HandleProjectNameUpdate(ByVal project As IProject)

        Me.Text = project.Name

    End Sub


    Private Sub HandleDomainUpdate(ByVal domainMap As IDomainMap, ByVal propertyName As String)

        Select Case propertyName

            Case "Name"

                HandleDomainNameUpdate(domainMap)

        End Select

    End Sub

    Private Sub HandleDomainNameUpdate(ByVal domainMap As IDomainMap)

        SetXmlBehindTitle()

    End Sub


    Private Sub HandleClassUpdate(ByVal classMap As IClassMap, ByVal propertyName As String)

        Select Case propertyName

            Case "Name", "Namespace"

                HandleClassNameUpdate(classMap)

        End Select

    End Sub

    Private Sub HandleClassNameUpdate(ByVal classMap As IClassMap)

        Dim tableMap As ITableMap

        If m_DynamicCreateSource Then

            SetupClassesToTables(classMap.DomainMap)

            Dim suggestedName As String = m_ClassesToTables.GetTableNameForClass(classMap)

            If Not suggestedName = classMap.Table Then

                tableMap = classMap.GetTableMap

                If Not tableMap Is Nothing Then

                    tableMap.Name = suggestedName
                    classMap.Table = suggestedName

                End If

            End If

        End If


    End Sub


    Private Sub HandlePropertyUpdate(ByVal propertyMap As IPropertyMap, ByVal propertyName As String)

        Select Case propertyName

            Case "Name"

                HandlePropertyNameUpdate(propertyMap)

            Case "DataType"

                HandlePropertyTypeUpdate(propertyMap)

        End Select

    End Sub

    Private Sub HandlePropertyNameUpdate(ByVal propertyMap As IPropertyMap)

        Dim columnMap As IColumnMap

        If m_DynamicCreateSource Then

            SetupClassesToTables(propertyMap.ClassMap.DomainMap)

            Dim suggestedName As String = m_ClassesToTables.GetColumnNameForProperty(propertyMap)

            If Not suggestedName = propertyMap.Column Then

                columnMap = propertyMap.GetColumnMap

                If Not columnMap Is Nothing Then

                    columnMap.Name = suggestedName
                    propertyMap.Column = suggestedName

                End If

            End If

        End If

    End Sub


    Private Sub HandlePropertyTypeUpdate(ByVal propertyMap As IPropertyMap)

        Dim columnMap As IColumnMap

        If m_DynamicCreateSource Then

            columnMap = propertyMap.GetColumnMap

            If Not columnMap Is Nothing Then

                SetupClassesToTables(propertyMap.ClassMap.DomainMap)

                Dim suggestedType As System.Data.DbType = m_ClassesToTables.GetColumnTypeForProperty(propertyMap)
                Dim suggestedLength As Integer = m_ClassesToTables.GetColumnLengthForProperty(propertyMap)

                If Not suggestedType = columnMap.DataType Then

                    columnMap.DataType = suggestedType

                End If

                If Not suggestedType = columnMap.DataType Then

                    columnMap.Length = suggestedLength

                End If

            End If

        End If

    End Sub


    Private Sub HandleTableUpdate(ByVal tableMap As ITableMap, ByVal propertyName As String)

        Select Case propertyName

            Case "Name"

                HandleTableNameUpdate(tableMap)

        End Select

    End Sub

    Private Sub HandleTableNameUpdate(ByVal tableMap As ITableMap)

        Dim columnMap As IColumnMap


    End Sub



#End Region

#Region " Treeview and property grid events "

#Region " Event Handlers "

    Private Sub treePreviewClassesToCode_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treePreviewClassesToCode.AfterSelect

        Dim folderNode As PreviewFolderNode

        If Not treePreviewClassesToCode.SelectedNode Is Nothing Then

            If treePreviewClassesToCode.SelectedNode.GetType Is GetType(PreviewFolderNode) Then

                folderNode = treePreviewClassesToCode.SelectedNode

                mapTreeView.ExtractToConfig(folderNode.Config, folderNode.DomainMap, True)

            ElseIf treePreviewClassesToCode.SelectedNode.GetType Is GetType(PreviewFileNode) Then

                folderNode = treePreviewClassesToCode.SelectedNode.Parent

                mapTreeView.ExtractToConfig(folderNode.Config, folderNode.DomainMap, True)

            End If

        End If

    End Sub


    Private Sub mapTreeView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles mapTreeView.DragDrop

        TreeDragDrop(e)

    End Sub

    Private Sub mapTreeView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles mapTreeView.DragEnter

        TreeDragEnter(e)

    End Sub

    Private Sub mapTreeView_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mapTreeView.DragLeave

        TurnOffTreeDragHilite()

    End Sub


    Private Sub mapTreeView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles mapTreeView.DragOver

        TreeDragOver(e)

    End Sub

    Private Sub mapTreeView_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles mapTreeView.ItemDrag

        TreeItemDrag(e)

    End Sub


    Private Sub mapTreeView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles mapTreeView.KeyDown

        Select Case e.KeyCode

            Case Keys.Delete

                Delete()

        End Select

    End Sub

    Private Sub mapTreeView_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mapTreeView.DoubleClick

        If Not mapTreeView.SelectedNode Is Nothing Then

            If mapTreeView.SelectedNode.GetType Is GetType(SourceCodeFileNode) Then

                OpenSourceCodeFile(CType(mapTreeView.SelectedNode, SourceCodeFileNode).Map)

            ElseIf mapTreeView.SelectedNode.GetType Is GetType(UmlDiagramNode) Then

                ViewUmlDiagram(CType(mapTreeView.SelectedNode, UmlDiagramNode).Map)

            End If

        End If

    End Sub


    Private Sub mapTreeView_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles mapTreeView.BeforeLabelEdit

        CheckOkLabelEdit(e)

    End Sub

    Private Sub mapTreeView_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles mapTreeView.AfterLabelEdit

        If e.CancelEdit Then Exit Sub

        If e.Label = "" Then

            e.CancelEdit = True

            Exit Sub

        End If

        UpdateNodeObjectName(e)

    End Sub

    Private Sub mapTreeView_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles mapTreeView.AfterSelect

        If m_Updating Then Exit Sub
        Dim domainMap As IDomainMap

        Dim mapObject As IMap = CType(e.Node, MapNode).GetMap

        SelectPropertiesForMapObject(CType(e.Node, MapNode).Map)
        'mapPropertyGrid.SelectMapObject(CType(e.Node, MapNode).Map, m_Project)

        If e.Node.GetType Is GetType(DomainMapNode) Then

            SetCurrSaveObject(CType(e.Node, DomainMapNode).Map)

        ElseIf e.Node.GetType Is GetType(ProjectNode) Then

        Else

            If Not mapObject Is Nothing Then

                domainMap = GetDomainMap(mapObject)

                If Not domainMap Is Nothing Then

                    SetCurrSaveObject(domainMap)

                End If

            End If

        End If

        m_CurrentCopyTarget = mapObject

        RefreshToolBar()

        toolBarButtonProperties.Enabled = True

        SetListParent()

        ListErrors(e.Node)

    End Sub

    Private Sub ListErrors(ByVal mapNode As MapNode)

        ClearErrorMsgs()

        Dim listExceptions As IList = mapTreeView.GetMapNodeExceptions(mapNode, True)
        Dim mapException As MappingException

        For Each mapException In listExceptions

            AddErrorMsg(mapException)

        Next

    End Sub

    Private Sub mapTreeView_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapTreeView.MouseUp

        TreeViewMouseUp(mapTreeView, sender, e)

    End Sub


    Private Sub mapTreeViewPreview_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles mapTreeViewPreview.AfterSelect

        If m_Updating Then Exit Sub

        SelectPropertiesForMapObject(CType(e.Node, MapNode).Map)
        'mapPropertyGrid.SelectMapObject(CType(e.Node, MapNode).Map, m_Project)

        SelectSynchObject(CType(e.Node, MapNode).Map)

    End Sub


    Private Sub mapTreeViewPreview_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapTreeViewPreview.MouseUp

        TreeViewMouseUp(mapTreeViewPreview, sender, e)

    End Sub


    Private Sub mapTreeViewPreview_BeforeCollapse(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles mapTreeViewPreview.BeforeCollapse

        'e.Cancel = True

    End Sub


    Private Sub mapTreeViewPreview_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles mapTreeViewPreview.AfterCheck

        Select Case m_SynchMode

            Case SynchModeEnum.TablesToSource

                If Not m_TablesToSourceWorking Then

                    RefreshSynchTablesToSourcePreviewDTD()

                End If

        End Select

    End Sub


    Private Sub mapPropertyGrid_AfterPropertySet(ByVal mapObject As Object, ByVal propertyName As String) Handles MapPropertyGrid.AfterPropertySet

        HandleUpdate(mapObject, propertyName)

        m_Updating = True

        mapTreeView.RefreshAllNodes(False)
        mapTreeViewPreview.RefreshAllNodes(False)

        RefreshToolBar()

        RefreshListView()

        RefreshPreviewClassesToCode()

        m_Updating = False

        SetDomainMapDirty(mapObject)

        VerifyMap()

        RefreshUml()

        Select Case m_SynchMode

            Case SynchModeEnum.TablesToSource

                RefreshSynchTablesToSourcePreviewDTD()

            Case SynchModeEnum.ClassesToCode

                If propertyName = "XmlFilePerClass" Then

                    MsgBox("Please discard the current preview and start a new 'From Model To Code' synchronization after changing the value of this setting!")
                    'ClassesToCode(

                End If


        End Select

    End Sub

    Private Sub mapPropertyGrid_BeforePropertySet(ByVal mapObject As Object, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object) Handles MapPropertyGrid.BeforePropertySet

        If Not (mapObject.GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing _
            AndAlso mapObject.GetType.GetInterface(GetType(IClassMap).ToString) Is Nothing _
            AndAlso mapObject.GetType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing) Then

            If propertyName = "Name" Or propertyName = "Namespace" Then

                m_Project.UpdateMapObjectName(mapObject, value)

            End If

        End If

    End Sub

#End Region

#Region " Event helper methods "


    Private Sub RefreshPreviewClassesToCode()

        Dim folderNode As PreviewFolderNode
        Dim fileNode As PreviewFileNode

        For Each folderNode In treePreviewClassesToCode.Nodes

            folderNode.Refresh()

            For Each fileNode In folderNode.Nodes

                fileNode.Refresh()

            Next

        Next

    End Sub

    Public Sub TreeViewMouseUp(ByVal treeView As mapTreeView, ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim onNode As TreeNode = treeView.GetNodeAt(New Point(e.X, e.Y))

        If Not onNode Is Nothing Then

            If Not treeView.SelectedNode Is onNode Then

                treeView.SelectedNode = onNode

            End If

        End If

        If e.Button = MouseButtons.Right Then

            m_contextMenuNode = treeView.SelectedNode
            m_contextMenuMapObject = Nothing
            m_contextMenuNamespace = ""

            If treeView.SelectedNode Is Nothing Then

            Else

                If treeView.SelectedNode.GetType Is GetType(ProjectNode) Then

                    m_contextMenuMapObject = CType(treeView.SelectedNode, ProjectNode).GetMap

                    m_contextMenuDomainMap = Nothing

                    menuProject.Show(treeView, New Point(e.X, e.Y))

                ElseIf treeView.SelectedNode.GetType Is GetType(ConfigNode) Then

                    m_contextMenuMapObject = CType(treeView.SelectedNode, ConfigNode).GetMap

                    m_contextMenuDomainMap = CType(treeView.SelectedNode.Parent, ConfigListNode).GetMap

                    menuConfig.Show(treeView, New Point(e.X, e.Y))

                ElseIf treeView.SelectedNode.GetType Is GetType(SourceCodeFileNode) Then

                    m_contextMenuMapObject = CType(treeView.SelectedNode, SourceCodeFileNode).GetMap

                    m_contextMenuParentMap = CType(treeView.SelectedNode.Parent.Parent, ClassesToCodeConfigNode).GetMap

                    menuSourceCodeFile.Show(treeView, New Point(e.X, e.Y))

                ElseIf treeView.SelectedNode.GetType Is GetType(UmlDiagramNode) Then

                    m_contextMenuMapObject = CType(treeView.SelectedNode, UmlDiagramNode).GetMap

                    m_contextMenuDomainMap = CType(treeView.SelectedNode.Parent, UmlDiagramListNode).GetMap

                    menuUmlDiagram.Show(treeView, New Point(e.X, e.Y))

                Else

                    If CType(treeView.SelectedNode, MapNode).Map Is Nothing Then

                        If treeView.SelectedNode.GetType Is GetType(ClassListMapNode) Then

                            m_contextMenuMapObject = CType(treeView.SelectedNode, ClassListMapNode).GetMap

                            menuClassList.Show(treeView, New Point(e.X, e.Y))

                        ElseIf treeView.SelectedNode.GetType Is GetType(SourceListMapNode) Then

                            m_contextMenuMapObject = CType(treeView.SelectedNode, SourceListMapNode).GetMap

                            menuSourceList.Show(treeView, New Point(e.X, e.Y))

                        ElseIf treeView.SelectedNode.GetType Is GetType(NamespaceMapNode) Then

                            m_contextMenuNamespace = CType(treeView.SelectedNode, NamespaceMapNode).GetPath

                            m_contextMenuMapObject = CType(treeView.SelectedNode, NamespaceMapNode).GetMap

                            menuNamespace.Show(treeView, New Point(e.X, e.Y))

                        ElseIf treeView.SelectedNode.GetType Is GetType(ConfigListNode) Then

                            m_contextMenuMapObject = CType(treeView.SelectedNode, ConfigListNode).GetMap

                            menuConfigList.Show(treeView, New Point(e.X, e.Y))

                        ElseIf treeView.SelectedNode.GetType Is GetType(UmlDiagramListNode) Then

                            m_contextMenuMapObject = CType(treeView.SelectedNode, UmlDiagramListNode).GetMap

                            menuDiagramList.Show(treeView, New Point(e.X, e.Y))

                        ElseIf treeView.SelectedNode.GetType Is GetType(MapNode) Then

                        End If

                    Else

                        'ShowMapObjectContextMenu(treeView, CType(treeView.SelectedNode, MapNode).Map, e.X, e.Y, treeView)
                        ShowMapObjectContextMenu(CType(treeView.SelectedNode, MapNode).Map, e.X, e.Y, treeView)

                    End If


                End If


            End If

        End If


    End Sub



    Public Sub ShowMapObjectContextMenu(ByVal mapObject As Object, ByVal X As Integer, ByVal y As Integer, ByVal eventFromControl As Control, Optional ByVal fromDiagram As Boolean = False, Optional ByVal isStart As Boolean = False)

        m_contextMenuMapObject = mapObject
        m_contextMenuIsStart = isStart

        Dim umlLine As umlLine
        Dim fixed As Boolean

        If Not mapObject.GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

            menuDomain.Show(eventFromControl, New Point(X, y))

        ElseIf Not mapObject.GetType.GetInterface(GetType(IClassMap).ToString) Is Nothing Then

            Dim classMap As IClassMap = mapObject

            m_ClassContextMenuWasFromDiagram = fromDiagram

            If fromDiagram Then

                menuClassRename.Visible = False
                menuClassUmlAddToCurr.Visible = False

            Else

                menuClassRename.Visible = True
                menuClassUmlAddToCurr.Visible = True

            End If

            If classMap.ClassType = ClassType.Enum Then

                menuClassAddProperty.Visible = False
                menuClassAddEnumValue.Visible = True

            Else

                menuClassAddProperty.Visible = True
                menuClassAddEnumValue.Visible = False

            End If

            menuClass.Show(eventFromControl, New Point(X, y))

        ElseIf Not mapObject.GetType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing Then

            m_contextMenuParentMap = CType(mapObject, IPropertyMap).ClassMap

            If fromDiagram Then

                menuPropertyRename.Visible = False

            Else

                menuPropertyRename.Visible = True

            End If

            menuProperty.Show(eventFromControl, New Point(X, y))

        ElseIf Not mapObject.GetType.GetInterface(GetType(IEnumValueMap).ToString) Is Nothing Then

            enumValueMenu.Show(eventFromControl, New Point(X, y))

        ElseIf Not mapObject.GetType.GetInterface(GetType(ISourceMap).ToString) Is Nothing Then

            menuSource.Show(eventFromControl, New Point(X, y))

        ElseIf Not mapObject.GetType.GetInterface(GetType(ITableMap).ToString) Is Nothing Then

            menuTable.Show(eventFromControl, New Point(X, y))

        ElseIf Not mapObject.GetType.GetInterface(GetType(IColumnMap).ToString) Is Nothing Then

            menuColumn.Show(eventFromControl, New Point(X, y))

        ElseIf mapObject.GetType Is GetType(UmlDiagram) Then

            menuUmlDiagram.Show(eventFromControl, New Point(X, y))

        ElseIf mapObject.GetType Is GetType(umlLine) Then

            umlLine = mapObject

            If isStart Then

                fixed = umlLine.FixedStart

            Else

                fixed = umlLine.FixedEnd

            End If

            If fixed Then

                menuUmlLineEndLock.Enabled = False
                menuUmlLineEndUnLock.Enabled = True

            Else

                menuUmlLineEndLock.Enabled = True
                menuUmlLineEndUnLock.Enabled = False

            End If

            menuUmlLineEnd.Show(eventFromControl, New Point(X, y))

        ElseIf mapObject.GetType Is GetType(UmlLinePoint) Then

            menuUmlLinePoint.Show(eventFromControl, New Point(X, y))

        End If

    End Sub

    'Private Sub ShowMapObjectContextMenu(ByVal treeView As mapTreeView, ByVal mapObject As Object, ByVal X As Integer, ByVal y As Integer, ByVal eventFromControl As Control)

    '    m_contextMenuMapObject = mapObject

    '    If Not mapObject.GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

    '        menuDomain.Show(eventFromControl, New Point(X, y))

    '    ElseIf Not mapObject.GetType.GetInterface(GetType(IClassMap).ToString) Is Nothing Then

    '        menuClass.Show(eventFromControl, New Point(X, y))

    '    ElseIf Not mapObject.GetType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing Then

    '        m_contextMenuParentMap = CType(treeView.SelectedNode.Parent, ClassMapNode).GetMap

    '        menuProperty.Show(eventFromControl, New Point(X, y))

    '    ElseIf Not mapObject.GetType.GetInterface(GetType(ISourceMap).ToString) Is Nothing Then

    '        menuSource.Show(eventFromControl, New Point(X, y))

    '    ElseIf Not mapObject.GetType.GetInterface(GetType(ITableMap).ToString) Is Nothing Then

    '        menuTable.Show(eventFromControl, New Point(X, y))

    '    ElseIf Not mapObject.GetType.GetInterface(GetType(IColumnMap).ToString) Is Nothing Then

    '        menuColumn.Show(eventFromControl, New Point(X, y))


    '    End If

    'End Sub


    Private Sub CheckOkLabelEdit(ByVal e As System.Windows.Forms.NodeLabelEditEventArgs)

        Dim ok As Boolean = True

        If e.Node Is Nothing Then

            Exit Sub

        End If

        If (e.Node.GetType) Is GetType(ClassListMapNode) Then

            ok = False

        ElseIf (e.Node.GetType) Is GetType(SourceListMapNode) Then

            ok = False

        ElseIf (e.Node.GetType) Is GetType(ClassesToCodeConfigNode) Then

            ok = False

        ElseIf (e.Node.GetType) Is GetType(ConfigListNode) Then

            ok = False

        ElseIf (e.Node.GetType) Is GetType(SourceCodeFileListNode) Then

            ok = False

        ElseIf (e.Node.GetType) Is GetType(SourceCodeFileNode) Then

            ok = False

        End If

        If Not ok Then

            e.CancelEdit = True

        End If

    End Sub

    Private Sub UpdateNodeObjectName(ByVal e As System.Windows.Forms.NodeLabelEditEventArgs)

        Dim mapObject As IMap

        If e.Node Is Nothing Then Exit Sub

        mapObject = CType(e.Node, MapNode).GetMap

        If CType(mapObject, Object).GetType Is GetType(ClassMap) Then

            CType(mapObject, IClassMap).UpdateName(e.Label)

        ElseIf CType(mapObject, Object).GetType Is GetType(PropertyMap) Then

            CType(mapObject, IPropertyMap).UpdateName(e.Label)

        ElseIf CType(mapObject, Object).GetType Is GetType(SourceMap) Then

            CType(mapObject, ISourceMap).UpdateName(e.Label)

        ElseIf CType(mapObject, Object).GetType Is GetType(TableMap) Then

            CType(mapObject, ITableMap).UpdateName(e.Label)

        ElseIf CType(mapObject, Object).GetType Is GetType(ColumnMap) Then

            CType(mapObject, IColumnMap).UpdateName(e.Label)

        ElseIf CType(mapObject, Object).GetType Is GetType(ProjectModel.Project) Then

        Else

            mapObject.Name = e.Label

        End If

        HandleUpdate(mapObject, "Name")

        m_Updating = True

        mapTreeView.RefreshAllNodes(False)
        mapTreeViewPreview.RefreshAllNodes(False)
        mapPropertyGrid.RefreshProperties()

        RefreshToolBar()

        RefreshListView()

        m_Updating = False

        SetDomainMapDirty(mapObject)

        VerifyMap()

        RefreshUml()

        Select Case m_SynchMode

            Case SynchModeEnum.TablesToSource

                RefreshSynchTablesToSourcePreviewDTD()

        End Select

    End Sub


#End Region

#End Region

#Region " Toolbar click handler "

    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick

        Select Case e.Button.Tag

            Case "Back"

                GoBack()

            Case "Forward"

                GoForward()

            Case "Up"

                GoUp()



            Case "New"

                NewMap()

            Case "Open"

                Open()

            Case "Save"

                Save()

            Case "SaveAll"

                SaveAll()

            Case "Find"

                FindAndReplace()

            Case "Cut"

                Cut()

            Case "Copy"

                Copy()

            Case "Paste"

                Paste()

            Case "ClassesToCode"

                ClassesToCode()

            Case "CodeToClasses"

                CodeToClassesFromDll()

            Case "ClassesToTables"

                ClassesToTables()

            Case "TablesToClasses"

                TablesToClasses()

            Case "SourceToTables"

                SourceToTables()

            Case "TablesToSource"

                TablesToSource()

            Case "Run"

                RunInDomainExplorer()

            Case "Commit"

                CommitSynch()

            Case "Discard"

                DiscardSynch()

                AddMsg("Synchronization discarded!")

            Case "Wizard"

                OpenGenDomWizard()

            Case "Explorer"

                ShowExplorer()

            Case "Properties"

                OpenProperties()

            Case "Tools"

                ShowTools()

            Case "List"

                ShowList()

            Case "Xml"

                OpenXmlBehind()


            Case "Uml"

                OpenUml()

            Case "Print"

                Print()

            Case "PrintPreview"

                Print(True)

        End Select

    End Sub

#End Region

#Region " Context menu click event handlers "


    Private Sub menuDomainExportNh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainExportNh.Click

        ExportToNHibernate(m_contextMenuMapObject)

    End Sub


    Private Sub menuClassProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuPropertyProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuSourceProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuTableProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuColumnProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuColumnProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuConfigProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuConfigProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuSourceCodeFileProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceCodeFileProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuProjectProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectProperties.Click

        OpenProperties()

    End Sub



    Private Sub menuDomainProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainProperties.Click

        OpenProperties()

    End Sub


    Private Sub menuXmlBehind_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuXmlBehind.Popup

        menuXmlOpen.Enabled = False
        menuXmlOpen.Text = "Open"

        If Not m_CurrSaveObject Is Nothing Then

            If m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                menuXmlOpen.Enabled = True
                menuXmlOpen.Text = "Open " & CType(m_CurrSaveObject, IDomainMap).Name

            End If

        End If

        Dim doc As UserDocTabPage = GetCurrentOpenDoc()

        menuXmlClose.Text = "Close"
        menuXmlApply.Text = "Apply Changes"
        menuXmlDiscard.Text = "Discard Changes"

        menuXmlClose.Enabled = False
        menuXmlApply.Enabled = False
        menuXmlDiscard.Enabled = False

        If Not doc Is Nothing Then

            If Not doc.DomainMap Is Nothing Then

                menuXmlClose.Text = "Close " & doc.DomainMap.Name
                menuXmlClose.Enabled = True

                menuXmlApply.Text = "Apply Changes In " & doc.DomainMap.Name
                menuXmlDiscard.Text = "Discard Changes In " & doc.DomainMap.Name

                menuXmlApply.Enabled = doc.Dirty
                menuXmlDiscard.Enabled = doc.Dirty

            End If

        End If

    End Sub

    Private Sub menuFileTools_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuFileTools.Popup

        menuToolsXmlOpen.Enabled = False
        menuToolsXmlOpen.Text = "Open"

        If Not m_CurrSaveObject Is Nothing Then

            If m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                menuToolsXmlOpen.Enabled = True
                menuToolsXmlOpen.Text = "Open " & CType(m_CurrSaveObject, IDomainMap).Name

            End If

        End If

        Dim doc As UserDocTabPage = GetCurrentOpenDoc()

        menuToolsXmlClose.Text = "Close"
        menuToolsXmlApply.Text = "Apply Changes"
        menuToolsXmlDiscard.Text = "Discard Changes"

        menuToolsXmlClose.Enabled = False
        menuToolsXmlApply.Enabled = False
        menuToolsXmlDiscard.Enabled = False

        If Not doc Is Nothing Then

            If Not doc.DomainMap Is Nothing Then

                menuToolsXmlClose.Text = "Close " & doc.DomainMap.Name
                menuToolsXmlClose.Enabled = True

                menuToolsXmlApply.Text = "Apply Changes In " & doc.DomainMap.Name
                menuToolsXmlDiscard.Text = "Discard Changes In " & doc.DomainMap.Name

                menuToolsXmlApply.Enabled = doc.Dirty
                menuToolsXmlDiscard.Enabled = doc.Dirty

            End If

        End If


    End Sub

    Private Sub menuXmlOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuXmlOpen.Click

        OpenXmlBehind()

    End Sub

    Private Sub menuXmlClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuXmlClose.Click

        CloseXmlBehind()

    End Sub

    Private Sub menuXmlApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuXmlApply.Click

        ApplyXmlBehind()

    End Sub

    Private Sub menuXmlDiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuXmlDiscard.Click

        DiscardXmlBehind()

    End Sub

    Private Sub menuToolsXmlOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsXmlOpen.Click

        OpenXmlBehind()

    End Sub

    Private Sub menuToolsXmlClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsXmlClose.Click

        CloseXmlBehind()

    End Sub

    Private Sub menuToolsXmlApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsXmlApply.Click

        ApplyXmlBehind()

    End Sub

    Private Sub menuToolsXmlDiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsXmlDiscard.Click

        DiscardXmlBehind()

    End Sub



    Private Sub menuDomainXmlClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainXmlClose.Click

        CloseXmlBehind(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainXmlApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainXmlApply.Click

        ApplyXmlBehind(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainXmlDiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainXmlDiscard.Click

        ApplyXmlBehind(m_contextMenuMapObject)

    End Sub



    Private Sub menuEditSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEditSelectAll.Click

        SelectAll()

    End Sub

    Private Sub menuEditFindAndReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEditFindAndReplace.Click

        FindAndReplace()

    End Sub

    Private Sub menuDomainViewXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainViewXML.Click

        ViewDomainXML(m_contextMenuMapObject)

    End Sub


    Private Sub menuClassAddShadow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassAddShadow.Click

        AddShadowPropertiesForSubClass(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassRemoveShadow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassRemoveShadow.Click

        RemoveShadowPropertiesFromSubClass(m_contextMenuMapObject)

    End Sub

    Private Sub menuPropertyAddShadow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyAddShadow.Click

        AddShadowPropertyForSubClass(m_contextMenuParentMap, m_contextMenuMapObject)

    End Sub

    Private Sub menuPropertyRemoveShadow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyRemoveShadow.Click

        RemoveShadowPropertyFromSubClass(m_contextMenuParentMap, m_contextMenuMapObject)

    End Sub



    Private Sub menuSourceTestConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceTestConnection.Click

        TestConnection(m_contextMenuMapObject)

    End Sub

    Private Sub menuFileClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileClose.Click
        'File | Close

        CloseSelectedFile()

    End Sub


    Private Sub menuProjectPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectPaste.Click

        Paste()

    End Sub

    Private Sub menuProjectAddNewDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectAddNewDomain.Click

        NewMap()

    End Sub

    Private Sub menuProjectAddExistingDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectAddExistingDomain.Click

        OpenMap()

    End Sub

    Private Sub menuProjectSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectSave.Click

        SaveProject()

    End Sub

    Private Sub menuProjectSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectSaveAs.Click

        SaveProject(False, True)

    End Sub

    Private Sub menuProjectSaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectSaveAll.Click

        SaveAll()

    End Sub

    Private Sub menuProjectClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectClose.Click

        CloseProject()

    End Sub

    Private Sub menuProjectRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuProjectRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuClassListPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListPaste.Click

        Paste()

    End Sub


    Private Sub menuSourceListPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceListPaste.Click

        Paste()

    End Sub

    Private Sub menuEditCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEditCut.Click

        Cut()

    End Sub

    Private Sub menuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEditCopy.Click

        Copy()

    End Sub

    Private Sub menuEditPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEditPaste.Click

        Paste()

    End Sub

    Private Sub menuEditDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuEditDelete.Click

        Delete()

    End Sub


    Private Sub menuDomainAddClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainAddClass.Click

        AddClass(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainAddSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainAddSource.Click

        AddSource(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListAddClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListAddClass.Click

        AddClass(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListAddInterface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListAddInterface.Click

        AddInterface(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListAddStruct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListAddStruct.Click

        AddStruct(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListAddEnum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListAddEnum.Click

        AddEnum(m_contextMenuMapObject)

    End Sub


    Private Sub menuNamespaceAddClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespaceAddClass.Click

        AddClass(m_contextMenuMapObject, m_contextMenuNamespace)

    End Sub

    Private Sub menuNamespaceDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespaceDelete.Click

        DeleteNamespace(m_contextMenuMapObject, m_contextMenuNamespace)

    End Sub

    Private Sub menuClassAddProperty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassAddProperty.Click

        Dim propertyMap As IPropertyMap = AddProperty(m_contextMenuMapObject)

        If Not propertyMap Is Nothing Then

            If m_ClassContextMenuWasFromDiagram Then

                Dim umlDoc As UserDocTabPage = GetCurrentUmlDoc()

                If Not umlDoc Is Nothing Then

                    AddPropertyAssociationLineToUmlDoc(propertyMap, umlDoc, True)

                End If

            End If

        End If

    End Sub


    Private Sub menuClassAddEnumValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassAddEnumValue.Click

        Dim enumValueMap As IEnumValueMap = AddEnumValue(m_contextMenuMapObject)

    End Sub


    Private Sub menuClassDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassDelete.Click

        DeleteClass(m_contextMenuMapObject)

    End Sub

    Private Sub menuPropertyDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyDelete.Click

        DeleteProperty(m_contextMenuMapObject)

    End Sub


    Private Sub deleteEnumValueMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteEnumValueMenuItem.Click

        DeleteEnumValue(m_contextMenuMapObject)

    End Sub

    Private Sub menuSourceListAddSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceListAddSource.Click

        AddSource(m_contextMenuMapObject)

    End Sub

    Private Sub menuSourceAddTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceAddTable.Click

        AddTable(m_contextMenuMapObject)

    End Sub

    Private Sub menuSourceDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceDelete.Click

        DeleteSource(m_contextMenuMapObject)

    End Sub

    Private Sub menuTableAddColumn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableAddColumn.Click

        AddColumn(m_contextMenuMapObject)

    End Sub

    Private Sub menuTableDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableDelete.Click

        DeleteTable(m_contextMenuMapObject)

    End Sub

    Private Sub menuColumnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuColumnDelete.Click

        DeleteColumn(m_contextMenuMapObject)

    End Sub





    Private Sub menuDomainCodeVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeVb.Click

        DomainToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

    End Sub

    Private Sub menuDomainCodeCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeCs.Click

        DomainToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

    End Sub


    Private Sub menuDomainCodeDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeDelphi.Click

        DomainToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

    End Sub

    Private Sub menuDomainCodeDTD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeDTD.Click

        ShowDomainDtd(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeXml.Click

        ShowDomainXml(m_contextMenuMapObject)

    End Sub


    Private Sub menuClassListCodeVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListCodeVb.Click

        DomainToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

    End Sub

    Private Sub menuClassListCodeCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListCodeCs.Click

        DomainToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

    End Sub

    Private Sub menuClassListCodeDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListCodeDelphi.Click

        DomainToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

    End Sub


    Private Sub menuClassListCodeEcoXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DomainToEcoXml(m_contextMenuMapObject)

    End Sub


    Private Sub menuClassListCodeEcoCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DomainToEcoCodeCs(m_contextMenuMapObject)

    End Sub

    Private Sub menuNamespaceCodeVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespaceCodeVb.Click

        NamespaceToCode(m_contextMenuMapObject, m_contextMenuNamespace, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

    End Sub


    Private Sub menuClassListCodeNhCs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassListCodeNhCs.Click

        DomainToNhCodeCs(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListCodeNhDelphi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassListCodeNhDelphi.Click

        DomainToNhCodeDelphi(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListCodeNhVb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassListCodeNhVb.Click

        DomainToNhCodeVb(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassListCodeNhXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassListCodeNhXml.Click

        DomainToNhXml(m_contextMenuMapObject)

    End Sub



    Private Sub menuNamespaceCodeCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespaceCodeCs.Click

        NamespaceToCode(m_contextMenuMapObject, m_contextMenuNamespace, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

    End Sub


    Private Sub menuNamespaceCodeDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespaceCodeDelphi.Click

        NamespaceToCode(m_contextMenuMapObject, m_contextMenuNamespace, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

    End Sub

    Private Sub menuClassCodeVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCodeVb.Click

        ClassToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

    End Sub

    Private Sub menuClassCodeCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCodeCs.Click

        ClassToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

    End Sub


    Private Sub menuClassCodeDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCodeDelphi.Click

        ClassToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

    End Sub



    Private Sub menuClassCodeNhCs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassCodeNhCs.Click

        ClassToCodeNh(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

    End Sub

    Private Sub menuClassCodeNhDelphi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassCodeNhDelphi.Click

        ClassToCodeNh(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

    End Sub

    Private Sub menuClassCodeNhVb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassCodeNhVb.Click

        ClassToCodeNh(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

    End Sub


    Private Sub menuClassCodeNhXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassCodeNhXml.Click

        ClassToNhXml(m_contextMenuMapObject)

    End Sub


    Private Sub menuPropertyCodeVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyCodeVb.Click

        PropertyToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

    End Sub

    Private Sub menuPropertyCodeCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyCodeCs.Click

        PropertyToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

    End Sub


    Private Sub menuPropertyCodeDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyCodeDelphi.Click

        PropertyToCode(m_contextMenuMapObject, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

    End Sub


    Private Sub menuSourceCodeDTD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceCodeDTD.Click

        SourceToDTD(m_contextMenuMapObject)

    End Sub

    Private Sub menuTableCodeDTD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableCodeDTD.Click

        TableToDTD(m_contextMenuMapObject)

    End Sub


    Private Sub menuSourceSynchModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceSynchModel.Click

        SourceToTables(CType(m_contextMenuMapObject, ISourceMap))

    End Sub


    Private Sub menuSourceSynchSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceSynchSource.Click

        TablesToSource(CType(m_contextMenuMapObject, ISourceMap))

    End Sub


    Private Sub menuClassListSynchDMToTbl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListSynchDMToTbl.Click

        ClassesToTables(CType(m_contextMenuMapObject, IDomainMap))

    End Sub


    Private Sub menuClassListSynchDMToCls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListSynchDMToCls.Click

        TablesToClasses(CType(m_contextMenuMapObject, IDomainMap))

    End Sub


    Private Sub menuClassListSynchCMToModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListSynchCMToModel.Click

        CodeToClassesFromDll(CType(m_contextMenuMapObject, IDomainMap))

    End Sub


    Private Sub menuClassListSynchCMToCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassListSynchCMToCode.Click

        ClassesToCode(CType(m_contextMenuMapObject, IDomainMap))

    End Sub




    Private Sub menuSynchToolsSncCMToCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchToolsSncCMToCode.Click

        ClassesToCode()

    End Sub


    Private Sub menuSynchMarkAllChecked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchMarkAllChecked.Click

        mapTreeViewPreview.MarkAllNodes(True)

    End Sub

    Private Sub menuSynchMarkAllUnchecked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchMarkAllUnchecked.Click

        mapTreeViewPreview.MarkAllNodes(False)

    End Sub

    Private Sub menuSynchCommit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchCommit.Click

        CommitSynch()

    End Sub

    Private Sub menuSynchDiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchDiscard.Click

        DiscardSynch()

        AddMsg("Synchronization discarded!")

    End Sub

    Private Sub menuSyncRemoveUnchecked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSyncRemoveUnchecked.Click

        RemoveMarkedSynchNodes(False)

    End Sub

    Private Sub menuSyncRemoveChecked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSyncRemoveChecked.Click

        RemoveMarkedSynchNodes(True)

    End Sub


    Private Sub menuToolsVerifyVerify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsVerifyVerify.Click

        VerifyMap(False, True, True)

    End Sub


    Private Sub menuToolsVerifyAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsVerifyAuto.Click

        m_ApplicationSettings.OptionSettings.VerificationSettings.AutoVerify = Not m_ApplicationSettings.OptionSettings.VerificationSettings.AutoVerify

        VerifyMap()

    End Sub

    Private Sub menuToolsVerifyMappings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsVerifyMappings.Click

        m_ApplicationSettings.OptionSettings.VerificationSettings.VerifyMappings = Not m_ApplicationSettings.OptionSettings.VerificationSettings.VerifyMappings

        VerifyMap()

    End Sub


    Private Sub menuToolsOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsOptions.Click

        OpenOptions()

    End Sub



    Private Sub menuNewDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNewDomain.Click

        NewMap()

    End Sub


    Private Sub menuNewProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNewProject.Click

        NewProject()

    End Sub



    Private Sub menuFileNewMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileNewMap.Click

        NewMap()

    End Sub

    Private Sub menuFileNewProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileNewProject.Click

        NewProject()

    End Sub

    Private Sub menuFileNewFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileNewFile.Click

        NewFile()

    End Sub

    Private Sub menuFileOpenModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileOpenModel.Click

        OpenMap()

    End Sub

    Private Sub menuFileOpenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileOpenFile.Click

        Open()

    End Sub


    Private Sub menuFileOpenProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileOpenProject.Click

        OpenProject()

    End Sub

    Private Sub menuFileCloseProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileCloseProject.Click

        CloseProject()

    End Sub

    Private Sub menuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileSave.Click

        Save()

    End Sub


    Private Sub menuFileSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileSaveAs.Click

        SaveAs()

    End Sub



    Private Sub menuConfigListAddConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuConfigListAddConfig.Click

        AddNewConfig(m_contextMenuMapObject)

    End Sub


    Private Sub menuConfigSetActive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuConfigSetActive.Click

        SetActiveConfig(m_contextMenuMapObject, m_contextMenuDomainMap)

    End Sub

    Private Sub menuConfigDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuConfigDelete.Click

        DeleteConfig(m_contextMenuMapObject, m_contextMenuDomainMap)

    End Sub



    Private Sub menuDiagramListAddDiagram_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDiagramListAddDiagram.Click

        AddNewDiagram(m_contextMenuMapObject)

    End Sub



    Private Sub menuSourceCodeFileRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceCodeFileRemove.Click

        RemoveSourceCodeFile(m_contextMenuMapObject, m_contextMenuParentMap)

    End Sub

    Private Sub menuSourceCodeFileDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceCodeFileDelete.Click

        DeleteSourceCodeFile(m_contextMenuMapObject, m_contextMenuParentMap)

    End Sub



    Private Sub menuToolsSynchDMToTbl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsSynchDMToTbl.Click

        ClassesToTables()

    End Sub

    Private Sub menuToolsSynchDMToCls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsSynchDMToCls.Click

        TablesToClasses()

    End Sub

    Private Sub menuToolsSynchCMToCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsSynchCMToCode.Click

        ClassesToCode()

    End Sub

    Private Sub menuToolsSynchCMToModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsSynchCMToModel.Click

        CodeToClassesFromDll()

    End Sub

    Private Sub menuToolsSynchTMToSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsSynchTMToSource.Click

        TablesToSource()

    End Sub

    Private Sub menuToolsSynchTMToModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsSynchTMToModel.Click

        SourceToTables()

    End Sub

    Private Sub menuSrcListSynchTMToDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSrcListSynchTMToDB.Click

        TablesToSource(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub menuSrcListSynchTMToModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSrcListSynchTMToModel.Click

        SourceToTables(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub menuSynchToolsSncDMToCls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchToolsSncDMToCls.Click

        ClassesToTables()

    End Sub

    Private Sub menuSynchToolsSncDMToTbl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchToolsSncDMToTbl.Click

        TablesToClasses()

    End Sub

    Private Sub menuSynchToolsSncCMToModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchToolsSncCMToModel.Click

        CodeToClassesFromDll()

    End Sub

    Private Sub menuSynchToolsSncToSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchToolsSncToSource.Click

        TablesToSource()

    End Sub

    Private Sub MenuItem49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem49.Click

        ClassesToTables(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub MenuItem50_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem50.Click

        TablesToClasses(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub MenuItem51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem51.Click

        ClassesToCode(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub MenuItem52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem52.Click

        CodeToClassesFromDll(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub MenuItem53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem53.Click

        TablesToSource(CType(m_contextMenuMapObject, IDomainMap))

    End Sub

    Private Sub MenuItem54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem54.Click

        SourceToTables(CType(m_contextMenuMapObject, IDomainMap))

    End Sub


    Private Sub MenuItem42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem42.Click

        Dim frm As New frmAbout

        frm.ShowDialog(Me)

    End Sub

    Private Sub menuFileExportEcoXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ExportToEco()

    End Sub


    Private Sub menuFileImportEcoXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ImportFromEcoXml()

    End Sub


    Private Sub menuFileExportNHib_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileExportNHib.Click

        ExportToNHibernate()

    End Sub



    Private Sub menuFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileExit.Click

        If Quit() Then

            End

        End If

    End Sub



    Private Sub menuViewProjectExplorer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewProjectExplorer.Click

        ToggleProjectExplorerVisibility()

        SaveVisibilitySettings()

    End Sub


    Private Sub menuViewProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewProperties.Click

        TogglePropertiesVisibility()

        SaveVisibilitySettings()

    End Sub

    Private Sub menuViewMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewMain.Click

        ToggleMainVisibility()

        SaveVisibilitySettings()

    End Sub

    Private Sub menuViewTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewTools.Click

        ToggleToolsVisibility()

        SaveVisibilitySettings()

    End Sub

    Private Sub menuViewMsgList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewMsgList.Click

        ToggleMsgListVisibility()

        SaveVisibilitySettings()

    End Sub


    Private Sub menuViewToolBar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewToolBar.Click

        ToggleToolBarVisibility()

        SaveVisibilitySettings()

    End Sub

    Private Sub menuViewStatusBar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuViewStatusBar.Click

        ToggleStatusBarVisibility()

        SaveVisibilitySettings()

    End Sub


    Private Sub menuSourceSynchClsToTbl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceSynchClsToTbl.Click

        ClassesToTables(CType(m_contextMenuMapObject, ISourceMap).DomainMap)

    End Sub

    Private Sub menuSourceSynchTblToCls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceSynchTblToCls.Click

        TablesToClasses(CType(m_contextMenuMapObject, ISourceMap).DomainMap)

    End Sub


    Private Sub menuFileImportDTB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ImportDtb()

    End Sub

    Private Sub menuToolsWizGenDomWiz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsWizGenDomWiz.Click

        OpenGenDomWizard()

    End Sub


    Private Sub menuToolsWizWrapDbWiz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsWizWrapDbWiz.Click

        OpenWrapDbWizard()

    End Sub

    Private Sub menuWizardGenDom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuWizardGenDom.Click

        OpenGenDomWizard()

    End Sub

    Private Sub menuWizardWrapDb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuWizardWrapDb.Click

        OpenWrapDbWizard()

    End Sub


    Private Sub menuDomainRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainRemove.Click

        RemoveDomainFromProject(m_contextMenuMapObject)

    End Sub


    Private Sub menuDomainSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainSave.Click

        SaveDomainMap(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainSaveAs.Click

        SaveDomainMap(m_contextMenuMapObject, True)

    End Sub

    Private Sub menuDomainCodeEcoXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DomainToEcoXml(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeEcoCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DomainToEcoCodeCs(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeNHCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeNHCs.Click

        DomainToNhCodeCs(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeNHDelphi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuDomainCodeNHDelphi.Click

        DomainToNhCodeDelphi(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeNHVb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuDomainCodeNHVb.Click

        DomainToNhCodeVb(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeNHXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuDomainCodeNHXml.Click

        DomainToNhXml(m_contextMenuMapObject)

    End Sub


    Private Sub menuClassListCodeEcoDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DomainToEcoCodeDelphi(m_contextMenuMapObject)

    End Sub

    Private Sub menuDomainCodeEcoDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DomainToEcoCodeDelphi(m_contextMenuMapObject)

    End Sub

    Private Sub menuSrcListPreviewDDL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSrcListPreviewDDL.Click

        ShowDomainDtd(m_contextMenuMapObject)

    End Sub


    Private Sub menuToolsCodeCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsCodeCs.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToCode(domainMap, Tools.IClassesToCode.CodeLanguageEnum.CSharp)

        Next

    End Sub

    Private Sub menuToolsCodeVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsCodeVb.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToCode(domainMap, Tools.IClassesToCode.CodeLanguageEnum.VbNet)

        Next

    End Sub

    Private Sub menuToolsCodeDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsCodeDelphi.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToCode(domainMap, Tools.IClassesToCode.CodeLanguageEnum.Delphi)

        Next

    End Sub

    Private Sub menuToolsCodeDDL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsCodeDDL.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            ShowDomainDtd(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsCodeXml.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            ShowDomainXml(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeEcoCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToEcoCodeCs(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeEcoDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToEcoCodeDelphi(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeEcoXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToEcoXml(domainMap)

        Next

    End Sub



    Private Sub menuToolsCodeNhCs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuToolsCodeNhCs.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToNhCodeCs(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeNhDelphi_Disposed(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToNhCodeDelphi(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeNhVb_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuToolsCodeNhVb.Disposed

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToNhCodeVb(domainMap)

        Next

    End Sub

    Private Sub menuToolsCodeNhXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuToolsCodeNhXml.Click

        Dim domainMap As IDomainMap

        For Each domainMap In mapTreeView.GetAllDomainMaps()

            DomainToNhXml(domainMap)

        Next

    End Sub

    Private Sub menuClassCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCut.Click

        Cut()

    End Sub

    Private Sub menuClassCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCopy.Click

        Copy()

    End Sub

    Private Sub menuClassPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassPaste.Click

        Paste()

    End Sub

    Private Sub menuClassRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuPropertyCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyCut.Click

        Cut()

    End Sub

    Private Sub menuPropertyCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyCopy.Click

        Copy()

    End Sub

    Private Sub menuPropertyRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuSourceCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceCut.Click

        Cut()

    End Sub

    Private Sub menuSourceCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceCopy.Click

        Copy()

    End Sub

    Private Sub menuSourcePaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourcePaste.Click

        Paste()

    End Sub

    Private Sub menuSourceRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSourceRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub


    Private Sub menuDomainCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCopy.Click

        Copy()

    End Sub

    Private Sub menuDomainRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuNamespaceRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespaceRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuTableCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableCut.Click

        Cut()

    End Sub

    Private Sub menuTableCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableCopy.Click

        Copy()

    End Sub

    Private Sub menuTablePaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTablePaste.Click

        Paste()

    End Sub

    Private Sub menuTableRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTableRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuColumnCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuColumnCut.Click

        Cut()

    End Sub

    Private Sub menuColumnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuColumnCopy.Click

        Copy()

    End Sub

    Private Sub menuColumnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuColumnRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub

    Private Sub menuConfigRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuConfigRename.Click

        m_contextMenuNode.BeginEdit()

    End Sub


    Private Sub menuDomainPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainPaste.Click

        Paste()

    End Sub

    Private Sub buttonPrevPreviewDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPrevPreviewDoc.Click

        GoPrevPreviewDoc()

    End Sub

    Private Sub buttonNextPreviewDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNextPreviewDoc.Click

        GoNextPreviewDoc()

    End Sub

    Private Sub buttonPrevUserDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPrevUserDoc.Click

        GoPrevUserDoc()

    End Sub

    Private Sub buttonNextUserDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNextUserDoc.Click

        GoNextUserDoc()

    End Sub

    Private Sub buttonPrevXmlBehind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPrevXmlBehind.Click

        GoPrevXmlBehind()

    End Sub

    Private Sub buttonNextXmlBehind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNextXmlBehind.Click

        GoNextXmlBehind()

    End Sub


    Private Sub buttonPrevUmlDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPrevUmlDoc.Click

        GoPrevUmlDoc()

    End Sub

    Private Sub buttonNextUmlDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNextUmlDoc.Click

        GoNextUmlDoc()

    End Sub

    Private Sub buttonCloseUmlDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseUmlDoc.Click

        CloseUmlDoc()

    End Sub



#End Region

#Region " Context menu popup handlers "

    Private Sub menuDiagramUml_HandlePopup()


        Dim diagram As UmlDiagram
        Dim domainMap As IDomainMap
        Dim classMap As IClassMap
        Dim item As MenuItem

        Try

            Dim doc As UserDocTabPage = GetCurrentUmlDoc()

            menuUmlAddLines.Enabled = False
            menuUmlRemoveLines.Enabled = False
            menuUmlAddClass.Enabled = False

            If Not doc Is Nothing Then

                menuUmlAddLines.Enabled = True
                menuUmlRemoveLines.Enabled = True
                menuUmlAddClass.Enabled = True

                diagram = doc.UmlDiagram

                If Not diagram Is Nothing Then

                    If diagram Is m_contextMenuMapObject Then

                        domainMap = diagram.GetDomainMap

                        ''menuUmlAddClassExisting.MenuItems.Clear()

                        'menuUmlAddClassExisting.MenuItems.Add("All", AddressOf menuUmlAddClassExistingAll_Click)

                        'menuUmlAddClassExisting.MenuItems.Add("-")

                        'For Each classMap In domainMap.ClassMaps

                        '    If Not doc.DisplaysClassMap(classMap) Then

                        '        menuUmlAddClassExisting.MenuItems.Add(classMap.Name, AddressOf menuUmlAddClassExistingClass_Click)

                        '    End If

                        'Next

                    End If

                End If

            End If


            If Not classMap Is Nothing Then



            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub menuClassUml_HandlePopup()

        Try

            Dim classMap As IClassMap = m_contextMenuMapObject

            If Not classMap Is Nothing Then

                Dim doc As UserDocTabPage = GetCurrentUmlDoc()

                If doc Is Nothing Then

                    menuClassUmlAddToCurr.Enabled = False
                    menuClassUmlRemoveAllLines.Enabled = False
                    menuClassUmlShowAssocLines.Enabled = False
                    menuClassUmlRemove.Enabled = False

                Else

                    menuClassUmlAddToCurr.Enabled = Not doc.DisplaysClassMap(classMap)
                    menuClassUmlShowAssocLines.Enabled = True
                    menuClassUmlRemoveAllLines.Enabled = True
                    menuClassUmlRemove.Enabled = doc.DisplaysClassMap(classMap)

                End If


            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub menuPropertyUml_HandlePopup()

        Try

            Dim propertyMap As IPropertyMap = m_contextMenuMapObject
            Dim classMap As IClassMap = m_contextMenuParentMap

            If Not propertyMap Is Nothing And Not classMap Is Nothing Then

                Dim doc As UserDocTabPage = GetCurrentUmlDoc()

                If doc Is Nothing Then

                    menuPropertyUmlShowPropLine.Enabled = False
                    menuPropertyUmlRemovePropLine.Enabled = False

                Else

                    menuPropertyUmlShowPropLine.Enabled = Not doc.DisplaysPropertyMap(propertyMap)
                    menuPropertyUmlRemovePropLine.Enabled = doc.DisplaysPropertyMap(propertyMap)

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub menuClassTransform_HandlePopup()

        Try

            Dim classMap As IClassMap = m_contextMenuMapObject

            If Not classMap Is Nothing Then

                If classMap.InheritanceType = InheritanceType.None Then

                    menuClassShadow.Enabled = False

                Else

                    If classMap.GetInheritedClassMap Is Nothing Then

                        menuClassShadow.Enabled = False

                    Else

                        menuClassShadow.Enabled = True

                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub menuPropertyTransform_HandlePopup()

        Try

            Dim classMap As IClassMap = m_contextMenuParentMap

            If Not classMap Is Nothing Then

                If classMap.InheritanceType = InheritanceType.None Then

                    menuPropertyShadow.Enabled = False

                Else

                    If classMap.GetInheritedClassMap Is Nothing Then

                        menuPropertyShadow.Enabled = False

                    Else

                        menuPropertyShadow.Enabled = True

                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub menuPropertyShadow_HandlePopup()

        Try

            Dim propertyMap As IPropertyMap = m_contextMenuMapObject
            Dim classMap As IClassMap = m_contextMenuParentMap

            If Not propertyMap Is Nothing And Not classMap Is Nothing Then

                If classMap.IsShadowingProperty(propertyMap) Then

                    menuPropertyAddShadow.Enabled = False
                    menuPropertyRemoveShadow.Enabled = True

                Else

                    menuPropertyRemoveShadow.Enabled = False

                    If classMap.IsInheritedProperty(propertyMap) Then

                        menuPropertyAddShadow.Enabled = True

                    Else

                        menuPropertyAddShadow.Enabled = False
                        menuPropertyShadow.Enabled = False

                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub menuProperty_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuProperty.Popup

        menuPropertyUml_HandlePopup()

        menuPropertyTransform_HandlePopup()

        menuPropertyShadow_HandlePopup()

    End Sub



    Private Sub menuFile_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuFile.Popup

        menuFileSave.Enabled = True
        menuFileSaveAs.Enabled = True
        menuFileSave.Text = "Save"
        menuFileSaveAs.Text = "Save As..."

        If m_Project Is Nothing Then

            menuFileCloseProject.Enabled = False

            'perhaps user docs are open?
            'menuFileSaveAll.Enabled = False

        Else

            menuFileCloseProject.Enabled = True
            'menuFileSaveAll.Enabled = True

        End If

        If Not m_CurrSaveObject Is Nothing Then

            If m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                menuFileSave.Text = "Save " & GetDomainMapFileName(m_CurrSaveObject)
                menuFileSaveAs.Text = "Save " & GetDomainMapFileName(m_CurrSaveObject) & " As..."
                menuFileClose.Enabled = False

            ElseIf m_CurrSaveObject.GetType Is GetType(UserDocTabPage) Then

                menuFileSave.Text = "Save " & CType(m_CurrSaveObject, UserDocTabPage).GetFileName
                menuFileSaveAs.Text = "Save " & CType(m_CurrSaveObject, UserDocTabPage).GetFileName & " As..."
                menuFileClose.Enabled = True

            Else

                menuFileSave.Enabled = False
                menuFileSaveAs.Enabled = False
                menuFileClose.Enabled = False

            End If

        Else

            menuFileSave.Enabled = False
            menuFileSaveAs.Enabled = False

            menuFileClose.Enabled = False

        End If

        Dim path As String
        Dim newMenuItem As MenuItem

        menuFileRecentProjects.MenuItems.Clear()

        For Each path In m_ApplicationSettings.LatestFiles

            newMenuItem = New MenuItem(path)

            AddHandler newMenuItem.Click, AddressOf RecentProjectClickHandler

            menuFileRecentProjects.MenuItems.Add(newMenuItem)

        Next

        If MayPrint() Then

            menuFilePrint.Enabled = True
            menuFilePrintPreview.Enabled = True
            menuFilePageSetup.Enabled = True

        Else

            menuFilePrint.Enabled = False
            menuFilePrintPreview.Enabled = False
            menuFilePageSetup.Enabled = False

        End If

    End Sub

    Private Sub menuEdit_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuEdit.Popup


        'Yes, this looks backwards, but it does make sense....
        'The target is what we have just clicked on (but not cut/copied).
        'By copying it, it becomes the new source.
        If m_CurrentCopyTarget Is Nothing Then

            menuEditCut.Enabled = False
            menuEditCopy.Enabled = False
            menuEditDelete.Enabled = False

        Else

            menuEditCut.Enabled = True
            menuEditCopy.Enabled = True
            menuEditDelete.Enabled = True

        End If

        menuEditPaste.Enabled = MayPaste()

        'If m_SelectedCopySource Is Nothing Then

        '    menuEditPaste.Enabled = False

        'Else

        '    menuEditPaste.Enabled = MayPaste()

        'End If


        Dim doc As UserDocTabPage = GetCurrentOpenDoc()

        If doc Is Nothing Then

            menuEditSelectAll.Enabled = False

        Else

            menuEditSelectAll.Enabled = True

        End If

        If GetAllOpenDocs.Count < 1 Then

            menuEditFindAndReplace.Enabled = False

        Else

            menuEditFindAndReplace.Enabled = True

        End If

    End Sub



    Private Sub menuToolsVerify_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuToolsVerify.Popup

        menuToolsVerifyAuto.Checked = m_ApplicationSettings.OptionSettings.VerificationSettings.AutoVerify
        menuToolsVerifyMappings.Checked = m_ApplicationSettings.OptionSettings.VerificationSettings.VerifyMappings

    End Sub

    Private Sub MenuItem41_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuItem41.Popup

        menuViewProjectExplorer.Checked = panelLeft.Visible
        menuViewMain.Checked = panelMain.Visible
        menuViewProperties.Checked = panelProperties.Visible
        menuViewTools.Checked = panelRight.Visible
        menuViewMsgList.Checked = panelBottom.Visible
        menuViewToolBar.Checked = ToolBar1.Visible
        menuViewStatusBar.Checked = StatusBar1.Visible

    End Sub

    Private Sub menuProject_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuProject.Popup

        If m_SelectedCopySource Is Nothing Then

            menuProjectPaste.Enabled = False

        Else

            menuProjectPaste.Enabled = MayPaste()

        End If

    End Sub

    Private Sub menuDomain_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuDomain.Popup

        If m_SelectedCopySource Is Nothing Then

            menuDomainPaste.Enabled = False

        Else

            menuDomainPaste.Enabled = MayPaste()

        End If

        menuDomainXmlClose.Enabled = HasOpenXmlBehind(m_contextMenuMapObject)

        menuDomainXmlApply.Enabled = HasDirtyXmlBehind(m_contextMenuMapObject)
        menuDomainXmlDiscard.Enabled = HasDirtyXmlBehind(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassList_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClassList.Popup

        If m_SelectedCopySource Is Nothing Then

            menuClassListPaste.Enabled = False

        Else

            menuClassListPaste.Enabled = MayPaste()

        End If

    End Sub


    Private Sub menuNamespace_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuNamespace.Popup

        If m_SelectedCopySource Is Nothing Then

            menuNamespacePaste.Enabled = False

        Else

            menuNamespacePaste.Enabled = MayPaste()

        End If

    End Sub

    Private Sub menuClass_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClass.Popup

        If m_SelectedCopySource Is Nothing Then

            menuClassPaste.Enabled = False

        Else

            menuClassPaste.Enabled = MayPaste()

        End If

        menuClassTransform_HandlePopup()

        menuClassUml_HandlePopup()

    End Sub

    Private Sub menuSourceList_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuSourceList.Popup

        If m_SelectedCopySource Is Nothing Then

            menuSourceListPaste.Enabled = False

        Else

            menuSourceListPaste.Enabled = MayPaste()

        End If

    End Sub

    Private Sub menuSource_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuSource.Popup

        If m_SelectedCopySource Is Nothing Then

            menuSourcePaste.Enabled = False

        Else

            menuSourcePaste.Enabled = MayPaste()

        End If

    End Sub

    Private Sub menuTable_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuTable.Popup

        If m_SelectedCopySource Is Nothing Then

            menuTablePaste.Enabled = False

        Else

            menuTablePaste.Enabled = MayPaste()

        End If

    End Sub

#End Region

#Region " Misc. event handlers "

    Private Sub tabControlTools_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlTools.SelectedIndexChanged


    End Sub




    Private Sub menuSynchToolsSncTMToModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSynchToolsSncTMToModel.Click

        SourceToTables()

    End Sub

    Private Sub tabControlUserDoc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlUserDoc.SelectedIndexChanged

        SetUserDocumentTitle()

    End Sub

    Private Sub buttonCloseUserDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseUserDoc.Click

        CloseUserDoc()

    End Sub

    Private Sub buttonClosePreviewDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonClosePreviewDoc.Click

        ClosePreviewDoc()

    End Sub

    Private Sub buttonCloseXmlBehind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseXmlBehind.Click

        CloseXmlBehind()

    End Sub

    Private Sub buttonApplyXmlBehind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonApplyXmlBehind.Click

        ApplyXmlBehind()

    End Sub

    Private Sub buttonDiscardXmlBehind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonDiscardXmlBehind.Click

        DiscardXmlBehind()

    End Sub

    Private Sub tabControlPreviewDoc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlPreviewDoc.SelectedIndexChanged

        SetPreviewDocumentTitle()

    End Sub

    Private Sub tabControlXmlBehind_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlXmlBehind.SelectedIndexChanged

        SetXmlBehindTitle()

    End Sub

    Private Sub tabControlUmlDoc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlUmlDoc.SelectedIndexChanged

        SetUmlDocumentTitle()

    End Sub

    Private Sub frmDomainMapBrowser_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If Not Quit() Then

            e.Cancel = True

        End If

    End Sub

    Public Function Quit() As Boolean

        If Not SaveAll(True) Then

            Return False

        End If

        Return True

    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Timer1.Enabled = False

        RefreshAllDocuments()

        m_TimerCnt += 1

        If m_TimerCnt > 1 Then

            m_TimerCnt = 0

            mapTreeView.RefreshAllFileNodes()

        End If

        Timer1.Enabled = True

    End Sub


#End Region

#Region " Misc functions "



    Private Sub DisableToolbar(Optional ByVal domainOnly As Boolean = False)

        If Not domainOnly Then

            toolBarButtonNewItem.Enabled = False
            toolBarButtonSynch.Enabled = False
            toolBarButtonClassesToCode.Enabled = False
            toolBarButtonCodeToClasses.Enabled = False
            toolBarButtonClassesToTables.Enabled = False
            toolBarButtonTablesToClasses.Enabled = False
            toolBarButtonTablesToSource.Enabled = False
            toolBarButtonSourceToTables.Enabled = False

            'Else


            '    toolBarButtonNewItem.Enabled = False
            '    toolBarButtonSynch.Enabled = False
            '    toolBarButtonClassesToCode.Enabled = False
            '    toolBarButtonCodeToClasses.Enabled = False
            '    toolBarButtonClassesToTables.Enabled = False
            '    toolBarButtonTablesToClasses.Enabled = False
            '    toolBarButtonTablesToSource.Enabled = False
            '    toolBarButtonSourceToTables.Enabled = False

        End If

        toolBarButtonSave.Enabled = False
        toolBarButtonSaveAll.Enabled = False

    End Sub

    Private Sub EnableToolbar(Optional ByVal projectOnly As Boolean = False)

        If Not projectOnly Then

            toolBarButtonNewItem.Enabled = True
            toolBarButtonSynch.Enabled = True
            toolBarButtonClassesToCode.Enabled = True
            toolBarButtonCodeToClasses.Enabled = True
            toolBarButtonClassesToTables.Enabled = True
            toolBarButtonTablesToClasses.Enabled = True
            toolBarButtonTablesToSource.Enabled = True
            toolBarButtonSourceToTables.Enabled = True

            'Else


            '    toolBarButtonNewItem.Enabled = False
            '    toolBarButtonSynch.Enabled = False
            '    toolBarButtonClassesToCode.Enabled = False
            '    toolBarButtonCodeToClasses.Enabled = False
            '    toolBarButtonClassesToTables.Enabled = False
            '    toolBarButtonTablesToClasses.Enabled = False
            '    toolBarButtonTablesToSource.Enabled = False
            '    toolBarButtonSourceToTables.Enabled = False

        End If

        toolBarButtonSave.Enabled = True
        toolBarButtonSaveAll.Enabled = True

    End Sub

    Private Sub SetDomainMapDirty(ByVal mapObject As Object)

        If mapObject.GetType Is GetType(UmlDiagram) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(UmlClass) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(UmlLine) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(UmlLinePoint) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(ClassesToCodeConfig) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(ClassesToTablesConfig) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(DomainConfig) Then

            m_ProjectDirty = True

        ElseIf mapObject.GetType Is GetType(SourceCodeFile) Then

            m_ProjectDirty = True

        Else

            Dim domainMap As IDomainMap = GetDomainMap(mapObject)

            If Not domainMap Is Nothing Then

                domainMap.Dirty = True

            End If

        End If

    End Sub

    Private Function GetDomainMap(ByVal mapObject As Object) As IDomainMap

        If mapObject.GetType Is GetType(DomainMap) Then

            Return mapObject

        ElseIf mapObject.GetType Is GetType(ClassMap) Then

            Return CType(mapObject, ClassMap).DomainMap

        ElseIf mapObject.GetType Is GetType(PropertyMap) Then

            Return CType(mapObject, PropertyMap).ClassMap.DomainMap

        ElseIf mapObject.GetType Is GetType(SourceMap) Then

            Return CType(mapObject, SourceMap).DomainMap

        ElseIf mapObject.GetType Is GetType(TableMap) Then

            Return CType(mapObject, TableMap).SourceMap.DomainMap

        ElseIf mapObject.GetType Is GetType(ColumnMap) Then

            Return CType(mapObject, ColumnMap).TableMap.SourceMap.DomainMap

        End If

    End Function

    Private Sub ClearSynch()

        m_SynchMode = SynchModeEnum.None
        SetActionType(ActionTypeEnum.None)
        m_hashSynchDiff.Clear()

    End Sub

    Private Sub SetActionType(ByVal at As ActionTypeEnum)

        m_ActionType = at

        If m_ActionType = ActionTypeEnum.None Then
            toolBarButtonSynch.Enabled = False
            toolBarButtonDiscard.Enabled = False
        Else
            toolBarButtonSynch.Enabled = True
            toolBarButtonDiscard.Enabled = True
        End If


    End Sub


    Public Sub RegisterExtensions()

        If Not m_ApplicationSettings.FileAssociationsOffered Then

            m_ApplicationSettings.FileAssociationsOffered = True

            SaveAppSettings()

            Dim frm As New frmAssociateFiles

            frm.ShowDialog()

        End If

    End Sub


    Private Sub frmDomainMapBrowser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        mapListView.frmDomainMapBrowser = Me

        LogServices.mainForm = Me


        'LoadSettings()

        RegisterExtensions()


        SetActionType(ActionTypeEnum.None)


        Cursor.Current = MouseSet(Cursors.WaitCursor)

        LoadPluginsAndConverters()

        BeginLife()

        AddWelcomeMsg()

        Cursor.Current = MouseSet()


        LoadWinSettings()

        SetLayout()

        LoadPanelSizes()

        SetCutCopyPasteEnabled()

        m_settingUp = False

        If PropGridWithTreeView Then

            AddPropGridToTreeView()

        End If

    End Sub



    Private Sub frmDomainMapBrowser_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        SaveWinSettings()

    End Sub

    Private Sub frmDomainMapBrowser_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Move

        SaveWinSettings()

    End Sub



    Private Sub AddWelcomeMsg()

        AddMsg("Welcome to Puzzle ObjectMapper,")
        AddMsg("version " & Application.ProductVersion.ToString & "!")
        AddMsg("Developed by Mats Helander and Roger Johansson.")

        AddMsg("")

    End Sub



    Private Sub SelectSynchObject(ByVal mapObject As IMap)

        If mapObject Is Nothing Then Exit Sub

        Dim diffInfos As ArrayList = GetDiffInfo(mapObject)
        Dim diffInfo As IDiffInfo

        ClearPreviewMsgs()

        For Each diffInfo In diffInfos

            AddPreviewMsg(diffInfo)

        Next

    End Sub

    Private Function GetDiffInfo(ByVal mapObject As IMap) As ArrayList

        If mapObject Is Nothing Then Return Nothing
        Dim diffInfo As IDiffInfo
        Dim key As String = mapObject.GetKey

        If m_hashSynchDiff.ContainsKey(key) Then

            Return m_hashSynchDiff(key)

        End If

        Return New ArrayList

    End Function

    Private Sub SetStatusMessage(ByVal message As String)

        statusMessage.Text = message

    End Sub

    Private Sub ClearStatus()

        m_stackStatus.Clear()
        statusMain.Text = "Ready"

    End Sub

    Private Sub PushStatus(ByVal message As String)

        m_stackStatus.Add(statusMain.Text)
        statusMain.Text = message

    End Sub


    Private Sub PullStatus()

        statusMain.Text = m_stackStatus(m_stackStatus.Count - 1)
        m_stackStatus.RemoveAt(m_stackStatus.Count - 1)

    End Sub

    Private Overloads Sub AddNewMainDocument(ByVal text As String, ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType)

        AddNewMainDocument(text, tabName, docTitle, docType, "")

    End Sub

    Private Overloads Sub AddNewMainDocument(ByVal text As String, ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType, ByVal loadedFromPath As String)

        Dim newTabPage As New UserDocTabPage(tabName, docTitle, text, docType, loadedFromPath)

        AddHandler newTabPage.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

        'newTabPage.TextBox.ContextMenu = menuUserDoc

        AddHandler newTabPage.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp

        tabControlUserDoc.TabPages.Add(newTabPage)

        'tabControlDocuments.SelectedIndex = 1
        tabControlDocuments.SelectedTab = tabPageMainCustom

        tabControlUserDoc.SelectedIndex = tabControlUserDoc.TabPages.Count - 1

        SetUserDocumentTitle()

    End Sub

    Private Sub AddNewPreviewDocument(ByVal text As String, ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType)

        Dim newTabPage As New UserDocTabPage(tabName, docTitle, text, docType)

        AddHandler newTabPage.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

        'newTabPage.TextBox.ContextMenu = menuUserDoc

        AddHandler newTabPage.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp

        tabControlPreviewDoc.TabPages.Add(newTabPage)

        'tabControlDocuments.SelectedIndex = 2
        tabControlDocuments.SelectedTab = tabPageMainPreview

        tabControlPreviewDoc.SelectedIndex = tabControlPreviewDoc.TabPages.Count - 1

        SetPreviewDocumentTitle()

    End Sub




    Public Sub HandleUserDocTextBoxEnter(ByVal sender As Object)

        m_CurrentCopyTarget = sender

    End Sub

    Public Sub HandleUserDocTextBoxMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If e.Button = MouseButtons.Right Then

            menuUserDoc.Show(sender, New Point(e.X, e.Y))

        End If

    End Sub


    Private Function CloseUserDoc(Optional ByVal tp As UserDocTabPage = Nothing, Optional ByVal noSave As Boolean = False) As Boolean

        Dim path As String

        If tp Is Nothing Then

            If tabControlUserDoc.TabPages.Count > 0 Then

                tp = CType(tabControlUserDoc.SelectedTab, UserDocTabPage)

            End If

        End If

        If Not tp Is Nothing Then

            If Not noSave Then

                If tp.Dirty Then

                    Select Case MsgBox("The content in the document '" & tp.Text & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.Yes

                            SaveUserDoc(tp)

                    End Select

                End If


            End If

            If tabControlUserDoc.TabPages.Contains(tp) Then

                tabControlUserDoc.TabPages.Remove(tp)

            ElseIf tabControlPreviewDoc.TabPages.Contains(tp) Then

                tabControlPreviewDoc.TabPages.Remove(tp)

            End If

            SetUserDocumentTitle()

        End If

        Return True

    End Function

    Private Function CloseCodeMapDoc(Optional ByVal noSave As Boolean = False) As Boolean

        Dim path As String
        Dim tp As UserDocTabPage

        If tabControlCodeMapDoc.TabPages.Count > 0 Then

            tp = CType(tabControlCodeMapDoc.SelectedTab, UserDocTabPage)

            If Not noSave Then

                If tp.Dirty Then

                    Select Case MsgBox("The content in the document '" & tabControlCodeMapDoc.SelectedTab.Text & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.Yes

                            SaveUserDoc(tp)

                    End Select

                End If

            End If

            tabControlCodeMapDoc.TabPages.Remove(tabControlCodeMapDoc.SelectedTab)

            SetCodeMapDocumentTitle()

        End If

        Return True

    End Function


    Private Function ClosePreviewDoc(Optional ByVal noSave As Boolean = False) As Boolean

        Dim path As String
        Dim tp As UserDocTabPage

        If tabControlPreviewDoc.TabPages.Count > 0 Then

            tp = CType(tabControlPreviewDoc.SelectedTab, UserDocTabPage)

            If Not noSave Then

                If tp.Dirty Then

                    Select Case MsgBox("The content in the document '" & tabControlPreviewDoc.SelectedTab.Text & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.Yes

                            SaveUserDoc(tp)

                    End Select

                End If

            End If

            tabControlPreviewDoc.TabPages.Remove(tabControlPreviewDoc.SelectedTab)

            SetPreviewDocumentTitle()

        End If

        Return True

    End Function

    Private Sub OpenUml()

        If m_CurrSaveObject Is Nothing Then Exit Sub

        ShowMain()

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()
        Dim checkDomainMap As IDomainMap
        Dim diagram As UmlDiagram
        Dim resource As IResource
        Dim domainMap As IDomainMap

        If Not doc Is Nothing Then

            If Not doc.UmlDiagram Is Nothing Then

                If m_CurrentCopyTarget.GetType Is GetType(UmlDiagram) Then

                    diagram = m_CurrentCopyTarget

                    If doc.UmlDiagram Is diagram Then

                        If Not panelMain.Visible Then

                            ToggleMainVisibility()

                            SaveVisibilitySettings()

                        End If

                        tabControlDocuments.SelectedTab = tabPageMainUml

                        Exit Sub

                    End If


                ElseIf m_CurrSaveObject.GetType Is GetType(domainMap) Then

                    domainMap = m_CurrSaveObject

                    checkDomainMap = doc.UmlDiagram.GetDomainMap

                    If checkDomainMap Is domainMap Then

                        If Not panelMain.Visible Then

                            ToggleMainVisibility()

                            SaveVisibilitySettings()

                        End If

                        tabControlDocuments.SelectedTab = tabPageMainUml

                        Exit Sub

                    End If

                End If

            End If

        End If

        doc = OpenUmlDoc()

        If doc Is Nothing Then

            If Not m_CurrSaveObject.GetType Is GetType(domainMap) Then Exit Sub

            domainMap = m_CurrSaveObject



            doc = ViewDomainUmlDiagram(domainMap)

            If Not doc Is Nothing Then Exit Sub

            resource = m_Project.GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

            If Not resource Is Nothing Then

                For Each diagram In resource.Diagrams

                    ViewUmlDiagram(diagram)

                    Exit Sub

                Next

            End If

            diagram = AddNewDiagram(m_CurrSaveObject)

            If Not diagram Is Nothing Then

                ViewUmlDiagram(diagram)

            End If

        End If

    End Sub

    Private Sub OpenXmlBehind()

        If m_CurrSaveObject Is Nothing Then Exit Sub

        If Not m_CurrSaveObject.GetType Is GetType(DomainMap) Then Exit Sub

        ViewDomainXML(m_CurrSaveObject)

    End Sub


    Private Overloads Function CloseXmlBehind(Optional ByVal noSave As Boolean = False) As Boolean

        Dim tp As UserDocTabPage

        If tabControlXmlBehind.TabPages.Count > 0 Then

            tp = CType(tabControlXmlBehind.SelectedTab, UserDocTabPage)

            If Not noSave Then

                If tp.Dirty Then

                    Select Case MsgBox("The xml behind '" & tp.Title & "' is changed but the changes have not been applied! Do you want to apply changes now?", MsgBoxStyle.YesNoCancel, "Apply Changes")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.Yes

                            tp.ApplyChanges()

                            If tp.Dirty Then

                                Return False

                            End If

                    End Select

                End If

            End If

            tabControlXmlBehind.TabPages.Remove(tabControlXmlBehind.SelectedTab)

            SetXmlBehindTitle()

        End If

        Return True

    End Function

    Private Overloads Sub ApplyXmlBehind()

        Dim tp As UserDocTabPage

        If tabControlXmlBehind.TabPages.Count > 0 Then

            tp = CType(tabControlXmlBehind.SelectedTab, UserDocTabPage)

            tp.ApplyChanges()

            SetXmlBehindTitle()

        End If

    End Sub

    Private Overloads Sub DiscardXmlBehind()

        Dim tp As UserDocTabPage

        If tabControlXmlBehind.TabPages.Count > 0 Then

            tp = CType(tabControlXmlBehind.SelectedTab, UserDocTabPage)

            tp.DiscardChanges()

            SetXmlBehindTitle()

        End If

    End Sub

    Private Overloads Function CloseXmlBehind(ByVal domainMap As IDomainMap) As Boolean

        Dim tp As UserDocTabPage

        Dim remove As New ArrayList

        If tabControlXmlBehind.TabPages.Count > 0 Then

            For Each tp In tabControlXmlBehind.TabPages

                If tp.DomainMap Is domainMap Then

                    remove.Add(tp)

                End If
            Next

            For Each tp In remove

                tabControlXmlBehind.TabPages.Remove(tp)

            Next

            SetXmlBehindTitle()


        End If

    End Function


    Private Overloads Sub ApplyXmlBehind(ByVal domainMap As IDomainMap)

        Dim tp As UserDocTabPage

        If tabControlXmlBehind.TabPages.Count > 0 Then

            For Each tp In tabControlXmlBehind.TabPages

                If tp.DomainMap Is domainMap Then

                    tp.ApplyChanges()

                    SetXmlBehindTitle()

                    Exit For

                End If
            Next

        End If

    End Sub

    Private Overloads Sub DiscardXmlBehind(ByVal domainMap As IDomainMap)

        Dim tp As UserDocTabPage

        If tabControlXmlBehind.TabPages.Count > 0 Then

            For Each tp In tabControlXmlBehind.TabPages

                If tp.DomainMap Is domainMap Then

                    tp.DiscardChanges()

                    SetXmlBehindTitle()

                    Exit For

                End If
            Next

        End If

    End Sub


    Private Function OpenUmlDoc() As UserDocTabPage

        If m_CurrSaveObject Is Nothing Then Exit Function

        ShowMain()

        If m_CurrSaveObject.GetType Is GetType(UmlDiagram) Then

            Return ViewUmlDiagram(m_CurrSaveObject)

        Else

            If Not m_CurrentCopyTarget Is Nothing Then

                If Not m_CurrentCopyTarget.GetType Is GetType(UmlDiagram) Then

                    Exit Function

                Else

                    Return ViewUmlDiagram(m_CurrentCopyTarget)

                End If

            Else

                'If Not m_contextMenuMapObject Is Nothing Then

                '    If Not m_contextMenuMapObject.GetType Is GetType(UmlDiagram) Then

                '        Exit Function

                '    Else

                '        Return ViewUmlDiagram(m_contextMenuMapObject)

                '    End If

                'End If

            End If


        End If

    End Function



    Private Overloads Function CloseUmlDoc() As Boolean

        Dim tp As UserDocTabPage

        If tabControlUmlDoc.TabPages.Count > 0 Then

            tp = CType(tabControlUmlDoc.SelectedTab, UserDocTabPage)

            tabControlUmlDoc.TabPages.Remove(tabControlUmlDoc.SelectedTab)

            SetUmlDocumentTitle()

        End If

        Return True

    End Function



    Private Overloads Function CloseUmlDoc(ByVal diagram As UmlDiagram) As Boolean

        Dim tp As UserDocTabPage

        Dim remove As New ArrayList

        If tabControlUmlDoc.TabPages.Count > 0 Then

            For Each tp In tabControlUmlDoc.TabPages

                If tp.UmlDiagram Is diagram Then

                    remove.Add(tp)

                End If
            Next

            For Each tp In remove

                tabControlUmlDoc.TabPages.Remove(tp)

            Next

            SetUmlDocumentTitle()

        End If

    End Function

    Private Overloads Function CloseUmlDoc(ByVal domainMap As IDomainMap) As Boolean

        Dim tp As UserDocTabPage

        Dim remove As New ArrayList
        Dim checkDomainMap As IDomainMap

        If tabControlUmlDoc.TabPages.Count > 0 Then

            For Each tp In tabControlUmlDoc.TabPages

                checkDomainMap = tp.UmlDiagram.GetDomainMap

                If Not checkDomainMap Is Nothing Then

                    If checkDomainMap Is domainMap Then

                        remove.Add(tp)

                    End If

                End If

            Next

            For Each tp In remove

                tabControlUmlDoc.TabPages.Remove(tp)

            Next

            SetUmlDocumentTitle()

        End If

    End Function

    Private Function SaveAllDomainMaps(Optional ByVal justDirty As Boolean = False) As Boolean

        Dim domainMap As IDomainMap

        Dim ok As Boolean

        For Each domainMap In mapTreeView.GetAllDomainMaps

            ok = True

            If justDirty Then

                If domainMap.Dirty Then

                    Select Case MsgBox("The content in the domain map '" & GetDomainMapFileName(domainMap) & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.No

                            ok = False

                    End Select

                End If

            End If

            If ok Then

                If justDirty = False OrElse domainMap.Dirty Then

                    SaveDomainMap(domainMap)

                End If

            End If

        Next

        Return True


    End Function

    Private Function SaveAllUserDocs(Optional ByVal justDirty As Boolean = False) As Boolean

        Dim tp As UserDocTabPage
        Dim ok As Boolean

        For Each tp In tabControlUserDoc.TabPages

            ok = True

            If justDirty Then

                If tp.Dirty Then

                    Select Case MsgBox("The content in the document '" & tp.GetFileName & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.No

                            ok = False

                    End Select

                End If

            End If

            If ok Then

                If justDirty = False OrElse tp.Dirty Then

                    SaveUserDoc(tp)

                End If

            End If

        Next

        Return True

    End Function

    Public Function GetCurrentOpenDoc() As UserDocTabPage

        Try

            Dim doc As UserDocTabPage

            If tabControlDocuments.SelectedTab Is tabPageMainUml Then

            ElseIf tabControlDocuments.SelectedTab Is tabPageMainCustom Then

                If tabControlUserDoc.SelectedIndex < 0 Then Return Nothing

                Return tabControlUserDoc.SelectedTab

            ElseIf tabControlDocuments.SelectedTab Is tabPageMainPreview Then

                If tabControlPreviewDoc.SelectedIndex < 0 Then Return Nothing

                Return tabControlPreviewDoc.SelectedTab

            ElseIf tabControlDocuments.SelectedTab Is tabPageMainXmlBehind Then

                If tabControlXmlBehind.SelectedIndex < 0 Then Return Nothing

                Return tabControlXmlBehind.SelectedTab

            End If

        Catch ex As Exception

        End Try

    End Function


    Public Function GetCurrentUmlDoc() As UserDocTabPage

        Try

            Dim doc As UserDocTabPage

            If tabControlDocuments.SelectedTab Is tabPageMainUml Then

                If tabControlUmlDoc.SelectedIndex < 0 Then Return Nothing

                Return tabControlUmlDoc.SelectedTab

            End If

        Catch ex As Exception

        End Try

    End Function


    Public Sub SetCurrentOpenDoc(ByVal currDoc As UserDocTabPage)

        Dim doc As UserDocTabPage
        Dim docs As New ArrayList

        For Each doc In tabControlPreviewDoc.TabPages

            If doc Is currDoc Then

                tabControlDocuments.SelectedTab = tabPageMainCustom

                tabControlPreviewDoc.SelectedTab = doc

                Exit Sub

            End If

        Next

        For Each doc In tabControlUserDoc.TabPages

            If doc Is currDoc Then

                tabControlDocuments.SelectedTab = tabPageMainPreview

                tabControlUserDoc.SelectedTab = doc

                Exit Sub

            End If

        Next

        For Each doc In tabControlXmlBehind.TabPages

            If doc Is currDoc Then

                tabControlDocuments.SelectedTab = tabPageMainXmlBehind

                tabControlXmlBehind.SelectedTab = doc

                Exit Sub

            End If

        Next

    End Sub

    Public Function GetAllOpenDocs() As ArrayList

        Dim doc As UserDocTabPage
        Dim docs As New ArrayList

        For Each doc In tabControlPreviewDoc.TabPages

            docs.Add(doc)

        Next

        For Each doc In tabControlUserDoc.TabPages

            docs.Add(doc)

        Next

        For Each doc In tabControlXmlBehind.TabPages

            docs.Add(doc)

        Next

        Return docs

    End Function


    Private Function SaveAllPreviewDocs(Optional ByVal justDirty As Boolean = False) As Boolean

        Dim tp As UserDocTabPage

        Dim ok As Boolean

        For Each tp In tabControlPreviewDoc.TabPages

            ok = True

            If justDirty Then

                If tp.Dirty Then

                    Select Case MsgBox("The content in the document '" & tp.GetFileName & "' has changed!" & vbCrLf & vbCrLf & "Do you want to save the changes?", MsgBoxStyle.YesNoCancel, "")

                        Case MsgBoxResult.Cancel

                            Return False

                        Case MsgBoxResult.No

                            ok = False

                    End Select

                End If

            End If

            If ok Then

                If justDirty = False OrElse tp.Dirty Then

                    SaveUserDoc(tp)

                End If

            End If

        Next

        Return True

    End Function

    Private Sub SaveUserDoc(ByVal tp As UserDocTabPage, Optional ByVal saveAs As Boolean = False)

        Dim path As String
        Dim fileInfo As fileInfo

        If Not saveAs Then

            path = tp.SavedToPath
            If path = "" Then path = tp.LoadedFromPath

        End If

        If path = "" Then

            Select Case tp.DocumentType

                Case MainDocumentType.CodeCSharp

                    SaveFileDialog1.Filter = "C# Code Files (*.cs)|*.cs|All Files (*.*)|*.*"

                Case MainDocumentType.CodeVbNet

                    SaveFileDialog1.Filter = "VB.NET Code Files (*.vb)|*.vb|All Files (*.*)|*.*"

                Case MainDocumentType.CodeDelphi

                    SaveFileDialog1.Filter = "Delpi Code Files (*.pas)|*.pas|All Files (*.*)|*.*"

                Case MainDocumentType.DTD

                    SaveFileDialog1.Filter = "DDL Files (*.sql)|*.sql|All Files (*.*)|*.*"

                Case MainDocumentType.XML

                    SaveFileDialog1.Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*"

                Case MainDocumentType.NPersist

                    SaveFileDialog1.Filter = "NPersist Files (*.npersist)|*.npersist|All Files (*.*)|*.*"

                Case MainDocumentType.Text

                    SaveFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

                Case Else

                    SaveFileDialog1.Filter = "All Files (*.*)|*.*"

            End Select

            SaveFileDialog1.FileName = tp.GetFullText

            If SaveFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                Exit Sub

            End If

            path = SaveFileDialog1.FileName

        End If

        If Not path = "" Then

            tp.Save(path)

            If File.Exists(path) Then

                fileInfo = New fileInfo(path)

                tp.FileUpdated = fileInfo.LastWriteTime

            End If

        End If


    End Sub

    Private Function GetDomainMapFileName(ByVal domainMap As IDomainMap) As String

        Dim name As String
        Dim resource As ProjectModel.IResource

        resource = GetResourceForDomainMap(domainMap)

        If Not resource Is Nothing Then

            name = resource.FilePath
            If name = "" Then name = domainMap.Name & ".npersist"

        End If

        Return name

    End Function

    Private Function GetProjectFileName() As String

        Dim name As String = m_ProjectSavedToPath
        If name = "" Then name = m_ProjectLoadedFromPath
        If name = "" Then name = m_Project.Name & ".omproj"

        Return name

    End Function

    Private Sub SaveDomainMap(ByVal domainMap As IDomainMap, Optional ByVal saveAs As Boolean = False)

        Dim path As String
        Dim resource As ProjectModel.IResource
        Dim mapSerializer As IMapSerializer

        resource = GetResourceForDomainMap(domainMap)

        If Not saveAs Then

            If Not resource Is Nothing Then

                path = resource.FilePath

            End If

        End If

        If path = "" Then

            Try

                SaveFileDialog1.Filter = "NPersist files (*.npersist)|*.npersist|All Files (*.*)|*.*"

                SaveFileDialog1.FileName = domainMap.Name & ".npersist"

                If SaveFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                    Exit Sub

                End If

                path = SaveFileDialog1.FileName

            Catch ex As Exception

                path = ""

            End Try

        End If

        If Not path = "" Then

            Try

                Select Case domainMap.MapSerializer

                    Case Puzzle.NPersist.Framework.Enumerations.MapSerializer.DefaultSerializer

                        mapSerializer = New DefaultMapSerializer

                    Case Puzzle.NPersist.Framework.Enumerations.MapSerializer.DotNetSerializer

                        mapSerializer = Nothing

                    Case Puzzle.NPersist.Framework.Enumerations.MapSerializer.CustomSerializer

                        MsgBox("Can't serialize to custom serializer formats. Will serialize to default format!")

                End Select

                domainMap.Save(path, mapSerializer)

                If Not resource Is Nothing Then

                    resource.FilePath = path

                End If

            Catch ex As Exception

                MsgBox("Could not save domain map to file '" & path & "'!")

            End Try

        End If


    End Sub




    Private Sub SetUserDocumentTitle()

        Dim tp As UserDocTabPage

        If Not tabControlUserDoc.TabPages.Count < 1 Then

            tp = tabControlUserDoc.SelectedTab

            If Not tp Is Nothing Then

                labelUserDocTitle.Text = tp.Title

                If Not buttonCloseUserDoc.Visible Then

                    buttonCloseUserDoc.Visible = True
                    buttonPrevUserDoc.Visible = True
                    buttonNextUserDoc.Visible = True

                End If

                If Not CanGoPrevUserDoc() Then

                    buttonPrevUserDoc.ImageIndex = 3
                    buttonPrevUserDoc.Enabled = False

                Else

                    buttonPrevUserDoc.ImageIndex = 2
                    buttonPrevUserDoc.Enabled = True

                End If

                If Not CanGoNextUserDoc() Then

                    buttonNextUserDoc.ImageIndex = 5
                    buttonNextUserDoc.Enabled = False

                Else

                    buttonNextUserDoc.ImageIndex = 4
                    buttonNextUserDoc.Enabled = True

                End If

                SetCurrSaveObject(tp)

            End If

        Else

            labelUserDocTitle.Text = ""
            buttonCloseUserDoc.Visible = False
            buttonNextUserDoc.Visible = False
            buttonPrevUserDoc.Visible = False

            SetCurrSaveObject(Nothing)

        End If

    End Sub

    Private Sub SetCodeMapDocumentTitle()

        Dim tp As UserDocTabPage

        If Not tabControlCodeMapDoc.TabPages.Count < 1 Then

            tp = tabControlCodeMapDoc.SelectedTab

            If Not tp Is Nothing Then

                labelCodeMapDocTitle.Text = tp.Title

                If Not buttonCloseCodeMapDoc.Visible Then

                    buttonCloseCodeMapDoc.Visible = True
                    buttonPrevCodeMapDoc.Visible = True
                    buttonNextCodeMapDoc.Visible = True

                End If

                If Not CanGoPrevCodeMapDoc() Then

                    buttonPrevCodeMapDoc.ImageIndex = 3
                    buttonPrevCodeMapDoc.Enabled = False

                Else

                    buttonPrevCodeMapDoc.ImageIndex = 2
                    buttonPrevCodeMapDoc.Enabled = True

                End If

                If Not CanGoNextCodeMapDoc() Then

                    buttonNextCodeMapDoc.ImageIndex = 5
                    buttonNextCodeMapDoc.Enabled = False

                Else

                    buttonNextCodeMapDoc.ImageIndex = 4
                    buttonNextCodeMapDoc.Enabled = True

                End If

                SetCurrSaveObject(tp)

            End If

        Else

            labelCodeMapDocTitle.Text = ""
            buttonCloseCodeMapDoc.Visible = False
            buttonNextCodeMapDoc.Visible = False
            buttonPrevCodeMapDoc.Visible = False

            SetCurrSaveObject(Nothing)

        End If

    End Sub

    Private Sub SetPreviewDocumentTitle()

        Dim tp As UserDocTabPage

        If Not tabControlPreviewDoc.TabPages.Count < 1 Then

            tp = tabControlPreviewDoc.SelectedTab

            If Not tp Is Nothing Then

                labelPreviewDocTitle.Text = tp.Title

                If Not buttonClosePreviewDoc.Visible Then

                    buttonClosePreviewDoc.Visible = True
                    buttonPrevPreviewDoc.Visible = True
                    buttonNextPreviewDoc.Visible = True

                End If

                If Not CanGoPrevPreviewDoc() Then

                    buttonPrevPreviewDoc.ImageIndex = 3
                    buttonPrevPreviewDoc.Enabled = False

                Else

                    buttonPrevPreviewDoc.ImageIndex = 2
                    buttonPrevPreviewDoc.Enabled = True

                End If

                If Not CanGoNextPreviewDoc() Then

                    buttonNextPreviewDoc.ImageIndex = 5
                    buttonNextPreviewDoc.Enabled = False

                Else

                    buttonNextPreviewDoc.ImageIndex = 4
                    buttonNextPreviewDoc.Enabled = True

                End If

                SetCurrSaveObject(tp)

            End If

        Else

            labelPreviewDocTitle.Text = ""
            buttonClosePreviewDoc.Visible = False
            buttonNextPreviewDoc.Visible = False
            buttonPrevPreviewDoc.Visible = False

            SetCurrSaveObject(Nothing)

        End If

    End Sub



    Private Sub SetXmlBehindTitle()

        Dim tp As UserDocTabPage
        Dim doIt As Boolean

        If Not tabControlXmlBehind.TabPages.Count < 1 Then

            tp = tabControlXmlBehind.SelectedTab

            If Not tp Is Nothing Then

                labelXmlBehindTitle.Text = "Xml Behind for domain model '" & tp.Title & "'"

                If Not buttonCloseXmlBehind.Visible Then

                    buttonCloseXmlBehind.Visible = True
                    buttonPrevXmlBehind.Visible = True
                    buttonNextXmlBehind.Visible = True

                End If


                If Not CanGoPrevXmlBehind() Then

                    buttonPrevXmlBehind.ImageIndex = 3
                    buttonPrevXmlBehind.Enabled = False

                Else

                    buttonPrevXmlBehind.ImageIndex = 2
                    buttonPrevXmlBehind.Enabled = True

                End If

                If Not CanGoNextXmlBehind() Then

                    buttonNextXmlBehind.ImageIndex = 5
                    buttonNextXmlBehind.Enabled = False

                Else

                    buttonNextXmlBehind.ImageIndex = 4
                    buttonNextXmlBehind.Enabled = True

                End If


                If tp.Dirty Then

                    labelXmlBehindTitle.Text += " [Modified]"

                    If Not buttonApplyXmlBehind.Visible Then

                        buttonApplyXmlBehind.Visible = True
                        buttonDiscardXmlBehind.Visible = True

                        buttonPrevXmlBehind.SendToBack()
                        buttonNextXmlBehind.SendToBack()
                        buttonCloseXmlBehind.SendToBack()

                    End If

                Else

                    buttonApplyXmlBehind.Visible = False
                    buttonDiscardXmlBehind.Visible = False

                End If

            End If

            For Each tp In tabControlXmlBehind.TabPages

                tp.Text = tp.Title

                If tp.Dirty Then

                    tp.Text = "*" & tp.Text

                End If

            Next

        Else

            labelXmlBehindTitle.Text = ""

            buttonCloseXmlBehind.Visible = False
            buttonNextXmlBehind.Visible = False
            buttonPrevXmlBehind.Visible = False
            buttonApplyXmlBehind.Visible = False
            buttonDiscardXmlBehind.Visible = False

            SetCurrSaveObject(Nothing)

        End If

    End Sub


    Private Sub SetUmlDocumentTitle()

        Dim tp As UserDocTabPage

        If Not tabControlUmlDoc.TabPages.Count < 1 Then

            tp = tabControlUmlDoc.SelectedTab

            If Not tp Is Nothing Then

                labelUmlTitle.Text = tp.Title

                If Not buttonCloseUmlDoc.Visible Then

                    buttonCloseUmlDoc.Visible = True
                    buttonPrevUmlDoc.Visible = True
                    buttonNextUmlDoc.Visible = True

                End If

                If Not CanGoPrevUmlDoc() Then

                    buttonPrevUmlDoc.ImageIndex = 3
                    buttonPrevUmlDoc.Enabled = False

                Else

                    buttonPrevUmlDoc.ImageIndex = 2
                    buttonPrevUmlDoc.Enabled = True

                End If

                If Not CanGoNextUmlDoc() Then

                    buttonNextUmlDoc.ImageIndex = 5
                    buttonNextUmlDoc.Enabled = False

                Else

                    buttonNextUmlDoc.ImageIndex = 4
                    buttonNextUmlDoc.Enabled = True

                End If

                SetCurrSaveObject(tp)

            End If

            For Each tp In tabControlUmlDoc.TabPages

                tp.Text = tp.Title

                'If tp.Dirty Then

                '    tp.Text = "*" & tp.Text

                'End If

            Next

        Else

            labelUmlTitle.Text = ""
            buttonCloseUmlDoc.Visible = False
            buttonNextUmlDoc.Visible = False
            buttonPrevUmlDoc.Visible = False

            SetCurrSaveObject(Nothing)

        End If

    End Sub



    Private Sub SetCurrSaveObject(ByVal value As Object)

        m_CurrSaveObject = value

        If m_CurrSaveObject Is Nothing Then

            toolBarButtonSave.Enabled = False

        Else

            toolBarButtonSave.Enabled = True

        End If

        RefreshToolBar()

    End Sub


    Private Function GetResourceForDomainMap(ByVal domainMap As IDomainMap) As ProjectModel.IResource

        Dim resource As ProjectModel.IResource

        If m_Project Is Nothing Then Exit Function

        For Each resource In m_Project.Resources

            If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                If resource.GetResource Is domainMap Then

                    Return resource

                End If

            End If

        Next

    End Function


    Private Sub OpenSourceCodeFile(ByVal src As SourceCodeFile)

        Dim tabPage As UserDocTabPage
        Dim text As String
        Dim fileReader As StreamReader
        Dim docType As MainDocumentType

        tabControlDocuments.SelectedTab = tabPageMainCustom

        For Each tabPage In tabControlUserDoc.TabPages

            If tabPage.GetFilePath = src.FilePath Then

                tabControlUserDoc.SelectedIndex = tabPage.TabIndex
                Exit Sub

            End If

        Next

        fileReader = File.OpenText(src.FilePath)

        text = fileReader.ReadToEnd

        fileReader.Close()

        Select Case src.FileType

            Case SourceCodeFileTypeEnum.CSharp

                docType = MainDocumentType.CodeCSharp

            Case SourceCodeFileTypeEnum.VB

                docType = MainDocumentType.CodeVbNet

            Case SourceCodeFileTypeEnum.Delphi

                docType = MainDocumentType.CodeDelphi

            Case SourceCodeFileTypeEnum.NPersist

                docType = MainDocumentType.NPersist

            Case SourceCodeFileTypeEnum.Xml

                docType = MainDocumentType.XML


        End Select


        'AddNewMainDocument(text, src.FilePath, "Code for '" & src.MapObjectName & "'", docType, src.FilePath)
        AddNewMainDocument(text, src.FilePath, src.FilePath, docType, src.FilePath)

    End Sub

    Public Sub RefreshAllDocuments()

        Dim tabPage As UserDocTabPage
        Dim text As String
        Dim fileInfo As fileInfo
        Dim fileReader As StreamReader
        Dim docType As MainDocumentType


        RefreshXmlBehind()

        For Each tabPage In tabControlUserDoc.TabPages

            If Len(tabPage.GetFilePath) > 0 Then

                If File.Exists(tabPage.GetFilePath) Then

                    fileInfo = New fileInfo(tabPage.GetFilePath)

                    If fileInfo.LastWriteTime > tabPage.FileUpdated Then

                        If MsgBox("The file '" & tabPage.GetFilePath & "' has been modified outside this designer! Do you want to refresh the file in the designer?", MsgBoxStyle.YesNo, "Modified File") = MsgBoxResult.Yes Then

                            fileReader = File.OpenText(tabPage.GetFilePath)

                            text = fileReader.ReadToEnd

                            fileReader.Close()

                            tabPage.DocumentText = text

                            tabPage.FileUpdated = fileInfo.LastWriteTime

                        End If

                    End If

                End If

            End If

        Next

    End Sub

    Private Shared Function ReplaceVars(ByVal text As String) As String

        Dim dict As IDictionary = Environment.GetEnvironmentVariables()
		for each variable as string in dict.Keys

            Dim pattern As String = "%" + variable.ToLower() + "%"
            Dim patternIndex As Integer = text.ToLower().IndexOf(pattern)
            If patternIndex >= 0 Then
                Dim left As String = text.Substring(0, patternIndex)
                Dim right As String = text.Substring(patternIndex + pattern.Length)

                text = left + Environment.GetEnvironmentVariable(variable) + right
            End If
        Next

        Return text

    End Function


    Private Function GetCustomCode(ByVal src As SourceCodeFile, ByVal classMap As IClassMap) As String

        Return GetCustomCode(src, ClassMap, True)

    End Function

    Private Function GetCustomCode(ByVal src As SourceCodeFile, ByVal classMap As IClassMap, ByVal first As Boolean) As String

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

            If first Then

                tag1 = "#Region "" Unsynchronized Custom Code Region """ & vbCrLf
                tag2 = "#End Region 'Unsynchronized Custom Code Region" & vbCrLf

            Else

                tag1 = "#Region "" MatsSoft maintained code : Custom Code Region """ & vbCrLf
                tag2 = "#End Region 'MatsSoft maintained code : Custom Code Region" & vbCrLf

            End If


            Select Case src.FileType

                Case SourceCodeFileTypeEnum.CSharp

                    If first Then

                        tag1 = "#region "" Unsynchronized Custom Code Region """ & vbCrLf
                        tag2 = "#endregion //Unsynchronized Custom Code Region" & vbCrLf

                    Else

                        tag1 = "#region "" MatsSoft maintained code : Custom Code Region """ & vbCrLf
                        tag2 = "#endregion //MatsSoft maintained code : Custom Code Region" & vbCrLf

                    End If



                Case SourceCodeFileTypeEnum.VB


                Case SourceCodeFileTypeEnum.Delphi

                    If first Then

                        tag1 = "{$REGION  ' Unsynchronized Custom Code Region '}" & vbCrLf
                        tag2 = "{$ENDREGION} //Unsynchronized Custom Code Region" & vbCrLf

                    Else

                        tag1 = "{$REGION  ' MatsSoft maintained code : Custom Code Region '}" & vbCrLf
                        tag2 = "{$ENDREGION} //MatsSoft maintained code : Custom Code Region" & vbCrLf

                    End If


            End Select

            Dim found As Boolean = False

            If Len(fullcode) > Len(tag1 & tag2) Then

                pos1 = fullcode.IndexOf(tag1)

                If pos1 > -1 Then

                    pos2 = fullcode.IndexOf(tag2, pos1 + 1)

                    If pos2 > pos1 Then

                        found = True

                        code = fullcode.Substring(pos1 + Len(tag1), pos2 - pos1 - Len(tag1))

                    End If

                End If

            End If

            If Not found Then

                If first = True Then

                    code = GetCustomCode(src, classMap, False)

                End If

            End If

            Return code


        End If

    End Function




    Public Sub LoadPluginsAndConverters()

        Dim path As String = Application.LocalUserAppDataPath & "\..\custom\plugins"
        LoadPluginsAndConverters(path)

        path = Application.StartupPath & "\plugins"
        LoadPluginsAndConverters(path)

    End Sub

    Public Sub LoadPluginsAndConverters(ByVal path As String)

        Dim file As String
        Dim fileInfo As fileInfo
        Dim asmType As Type
        Dim methInfo As MethodInfo
        Dim plugClassAttr As PluginClassAttribute
        Dim plugMethodAttr As PluginMethodAttribute
        Dim convClassAttr As ConverterClassAttribute
        Dim convMethodAttr As ConverterMethodAttribute

        Dim pluginClassMenu As MenuItem

        Dim domainPluginClassMenu As MenuItem
        Dim classPluginClassMenu As MenuItem
        Dim propPluginClassMenu As MenuItem
        Dim srcPluginClassMenu As MenuItem
        Dim tblPluginClassMenu As MenuItem
        Dim colPluginClassMenu As MenuItem

        Dim asm As Reflection.Assembly
        Dim add As Boolean

        Dim convClass As ConverterClass
        Dim convMeth As ConverterMethod

        If Not Directory.Exists(path) Then

            Exit Sub

        End If

        PushStatus("Loading plugins...")

        For Each file In Directory.GetFiles(path)

            fileInfo = New fileInfo(file)

            If fileInfo.Extension = ".dll" Then

                Try

                    asm = Reflection.Assembly.LoadFrom(file)

                    For Each asmType In asm.GetTypes()

                        For Each convClassAttr In asmType.GetCustomAttributes(GetType(ConverterClassAttribute), True)

                            Try

                                convClass = New ConverterClass

                                convClass.Name = convClassAttr.DisplayName
                                convClass.ConverterAssembly = asm
                                convClass.Converter = asm.CreateInstance(asmType.ToString)

                                Converters.Add(convClass)

                                For Each methInfo In asmType.GetMethods

                                    For Each convMethodAttr In methInfo.GetCustomAttributes(GetType(ConverterMethodAttribute), True)

                                        convMeth = New ConverterMethod

                                        convMeth.ConverterClass = convClass
                                        convMeth.MethodName = convMethodAttr.DisplayName
                                        convMeth.MethodInfo = methInfo

                                        Exit For

                                    Next

                                Next

                            Catch ex As Exception

                            End Try

                            Exit For

                        Next

                        For Each plugClassAttr In asmType.GetCustomAttributes(GetType(Puzzle.ObjectMapper.Plugin.PluginClassAttribute), True)

                            Try

                                pluginClassMenu = Nothing

                                domainPluginClassMenu = Nothing
                                classPluginClassMenu = Nothing
                                propPluginClassMenu = Nothing
                                srcPluginClassMenu = Nothing
                                tblPluginClassMenu = Nothing
                                colPluginClassMenu = Nothing

                                For Each methInfo In asmType.GetMethods

                                    For Each plugMethodAttr In methInfo.GetCustomAttributes(GetType(PluginMethodAttribute), True)

                                        add = False

                                        If plugMethodAttr.ReturnsType Is Nothing Then

                                            add = True

                                        Else

                                            add = False

                                            Select Case plugMethodAttr.ReturnsType.ToString

                                                Case GetType(String).ToString

                                                    add = True

                                            End Select

                                        End If


                                        If add Then

                                            add = False

                                            If plugMethodAttr.AcceptsType Is Nothing Then

                                                add = True

                                            Else

                                                Select Case plugMethodAttr.AcceptsType.ToString

                                                    Case GetType(Project).ToString

                                                        add = True

                                                    Case Else


                                                        If plugMethodAttr.AcceptsType Is GetType(IProject) Or plugMethodAttr.AcceptsType Is GetType(Project) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(Project)) Then

                                                            AddPlugin(menuToolsPlugins, asm, asmType, plugClassAttr, plugMethodAttr, pluginClassMenu, methInfo)
                                                            'AddPlugin(menuDomainPlugins, asm, asmType, plugClassAttr, plugMethodAttr, domainPluginClassMenu, methInfo)

                                                        ElseIf plugMethodAttr.AcceptsType Is GetType(IDomainMap) Or plugMethodAttr.AcceptsType Is GetType(DomainMap) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(DomainMap)) Then

                                                            AddPlugin(menuDomainPlugins, asm, asmType, plugClassAttr, plugMethodAttr, domainPluginClassMenu, methInfo)

                                                        ElseIf plugMethodAttr.AcceptsType Is GetType(IClassMap) Or plugMethodAttr.AcceptsType Is GetType(ClassMap) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(ClassMap)) Then

                                                            AddPlugin(menuClassPlugins, asm, asmType, plugClassAttr, plugMethodAttr, classPluginClassMenu, methInfo)

                                                        ElseIf plugMethodAttr.AcceptsType Is GetType(IPropertyMap) Or plugMethodAttr.AcceptsType Is GetType(PropertyMap) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(PropertyMap)) Then

                                                            AddPlugin(menuPropertyPlugins, asm, asmType, plugClassAttr, plugMethodAttr, propPluginClassMenu, methInfo)

                                                        ElseIf plugMethodAttr.AcceptsType Is GetType(ISourceMap) Or plugMethodAttr.AcceptsType Is GetType(SourceMap) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(SourceMap)) Then

                                                            AddPlugin(menuSourcePlugins, asm, asmType, plugClassAttr, plugMethodAttr, srcPluginClassMenu, methInfo)

                                                        ElseIf plugMethodAttr.AcceptsType Is GetType(ITableMap) Or plugMethodAttr.AcceptsType Is GetType(TableMap) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(TableMap)) Then

                                                            AddPlugin(menuTablePlugins, asm, asmType, plugClassAttr, plugMethodAttr, tblPluginClassMenu, methInfo)

                                                        ElseIf plugMethodAttr.AcceptsType Is GetType(IColumnMap) Or plugMethodAttr.AcceptsType Is GetType(ColumnMap) Or plugMethodAttr.AcceptsType.IsSubclassOf(GetType(ColumnMap)) Then

                                                            AddPlugin(menuColumnPlugins, asm, asmType, plugClassAttr, plugMethodAttr, colPluginClassMenu, methInfo)

                                                        End If

                                                End Select

                                            End If

                                        End If

                                        If add Then

                                            AddPlugin(menuToolsPlugins, asm, asmType, plugClassAttr, plugMethodAttr, pluginClassMenu, methInfo)

                                        End If

                                        Exit For

                                    Next

                                Next

                                Exit For

                            Catch ex As Exception

                            End Try

                        Next

                    Next

                Catch ex As Exception

                    Dim str As String = ex.Message

                End Try

            End If

        Next

        If menuToolsPlugins.MenuItems.Count < 1 Then

            menuToolsPlugins.Visible = False
            menuToolsPluginsSeparator.Visible = False

        End If

        If menuDomainPlugins.MenuItems.Count < 1 Then

            menuDomainPlugins.Visible = False
            menuDomainPluginsSeparator.Visible = False

        End If

        If menuClassPlugins.MenuItems.Count < 1 Then

            menuClassPlugins.Visible = False
            menuClassPluginsSeparator.Visible = False

        End If

        If menuPropertyPlugins.MenuItems.Count < 1 Then

            menuPropertyPlugins.Visible = False
            menuPropertyPluginsSeparator.Visible = False

        End If

        If menuSourcePlugins.MenuItems.Count < 1 Then

            menuSourcePlugins.Visible = False
            menuSourcePluginsSeparator.Visible = False

        End If

        If menuTablePlugins.MenuItems.Count < 1 Then

            menuTablePlugins.Visible = False
            menuTablePluginsSeparator.Visible = False

        End If

        If menuColumnPlugins.MenuItems.Count < 1 Then

            menuColumnPlugins.Visible = False
            menuColumnPluginsSeparator.Visible = False

        End If
        PullStatus()

    End Sub

    Private Sub AddPlugin(ByVal targetMenu As MenuItem, ByVal asm As Reflection.Assembly, ByVal asmType As Type, ByVal plugClassAttr As PluginClassAttribute, ByVal plugMethodAttr As PluginMethodAttribute, ByRef pluginClassMenu As MenuItem, ByVal methInfo As MethodInfo)

        Dim plugName As String
        Dim name As String
        Dim pluginMethodMenu As MenuItem
        Dim plugMeth As PluginMethod

        plugName = plugClassAttr.DisplayName

        If plugName = "" Then plugName = asmType.Name

        If pluginClassMenu Is Nothing Then

            pluginClassMenu = targetMenu.MenuItems.Add(plugName)

        End If

        name = plugMethodAttr.DisplayName

        If name = "" Then name = methInfo.Name

        pluginMethodMenu = pluginClassMenu.MenuItems.Add(name, AddressOf HandlePluginMenuItemClick)

        plugMeth = New PluginMethod

        plugMeth.AcceptsType = plugMethodAttr.AcceptsType
        plugMeth.ReturnsType = plugMethodAttr.ReturnsType
        plugMeth.PluginName = plugName
        plugMeth.MethodName = name
        plugMeth.MethodInfo = methInfo
        plugMeth.Plugin = asm.CreateInstance(asmType.ToString)
        plugMeth.PluginAssembly = asm

        m_PluginTable(pluginMethodMenu) = plugMeth

    End Sub

    Public Sub HandlePluginMenuItemClick(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim menuItem As menuItem = sender
        Dim plugMeth As PluginMethod
        Dim result As Object
        Dim obj As Object
        Dim docTitle As String

        If Not m_PluginTable.ContainsKey(menuItem) Then Exit Sub

        plugMeth = m_PluginTable(menuItem)

        If Not plugMeth.AcceptsType Is Nothing Then

            If plugMeth.AcceptsType Is GetType(IProject) Or plugMeth.AcceptsType Is GetType(Project) Or plugMeth.AcceptsType.IsSubclassOf(GetType(Project)) Then

                obj = m_Project

            Else

                obj = m_contextMenuMapObject

            End If

        End If

        If plugMeth.ReturnsType Is Nothing Then

            Try

                plugMeth.Execute(obj)

            Catch ex As Exception

                MsgBox("Failed Executing Plugin! : " & ex.ToString)

            End Try

        Else

            Try

                result = plugMeth.Execute(obj)


                Select Case plugMeth.ReturnsType.ToString

                    Case GetType(String).ToString

                        docTitle = "Output from plugin '" & plugMeth.PluginName & "." & plugMeth.MethodName & "'"
                        If Not obj Is Nothing Then
                            If Not obj.GetType.GetInterface(GetType(IMap).ToString) Is Nothing Then
                                docTitle += " for '" & CType(obj, IMap).Name & "'"
                            End If
                        End If
                        AddNewMainDocument(result, plugMeth.PluginName & "." & plugMeth.MethodName & ".txt", docTitle, MainDocumentType.Text)

                End Select

            Catch ex As Exception

                MsgBox("Failed Executing Plugin!")

            End Try


        End If

    End Sub


    Private Sub GoPrevCodeMapDoc()

        If Not CanGoPrevCodeMapDoc() Then Exit Sub

        If tabControlCodeMapDoc.SelectedIndex > 0 Then

            tabControlCodeMapDoc.SelectedIndex -= 1

        End If

        SetCodeMapDocumentTitle()

    End Sub

    Private Sub GoNextCodeMapDoc()

        If Not CanGoNextCodeMapDoc() Then Exit Sub

        If tabControlCodeMapDoc.SelectedIndex < tabControlCodeMapDoc.TabPages.Count - 1 Then

            tabControlCodeMapDoc.SelectedIndex += 1

        End If

        SetCodeMapDocumentTitle()

    End Sub

    Private Sub GoPrevPreviewDoc()

        If Not CanGoPrevPreviewDoc() Then Exit Sub

        If tabControlPreviewDoc.SelectedIndex > 0 Then

            tabControlPreviewDoc.SelectedIndex -= 1

        End If

        SetPreviewDocumentTitle()

    End Sub

    Private Sub GoNextPreviewDoc()

        If Not CanGoNextPreviewDoc() Then Exit Sub

        If tabControlPreviewDoc.SelectedIndex < tabControlPreviewDoc.TabPages.Count - 1 Then

            tabControlPreviewDoc.SelectedIndex += 1

        End If

        SetPreviewDocumentTitle()

    End Sub

    Private Sub GoPrevUserDoc()

        If Not CanGoPrevUserDoc() Then Exit Sub

        If tabControlUserDoc.SelectedIndex > 0 Then

            tabControlUserDoc.SelectedIndex -= 1

        End If

        SetUserDocumentTitle()

    End Sub

    Private Sub GoNextUserDoc()

        If Not CanGoNextUserDoc() Then Exit Sub

        If tabControlUserDoc.SelectedIndex < tabControlUserDoc.TabPages.Count - 1 Then

            tabControlUserDoc.SelectedIndex += 1

        End If

        SetUserDocumentTitle()

    End Sub

    Private Sub GoPrevXmlBehind()

        If Not CanGoPrevXmlBehind() Then Exit Sub

        If tabControlXmlBehind.SelectedIndex > 0 Then

            tabControlXmlBehind.SelectedIndex -= 1

        End If

        SetXmlBehindTitle()

    End Sub

    Private Sub GoNextXmlBehind()

        If Not CanGoNextXmlBehind() Then Exit Sub

        If tabControlXmlBehind.SelectedIndex < tabControlXmlBehind.TabPages.Count - 1 Then

            tabControlXmlBehind.SelectedIndex += 1

        End If

        SetXmlBehindTitle()

    End Sub



    Private Sub GoPrevUmlDoc()

        If Not CanGoPrevUmlDoc() Then Exit Sub

        If tabControlUmlDoc.SelectedIndex > 0 Then

            tabControlUmlDoc.SelectedIndex -= 1

        End If

        SetUmlDocumentTitle()

    End Sub

    Private Sub GoNextUmlDoc()

        If Not CanGoNextUmlDoc() Then Exit Sub

        If tabControlUmlDoc.SelectedIndex < tabControlUmlDoc.TabPages.Count - 1 Then

            tabControlUmlDoc.SelectedIndex += 1

        End If

        SetUmlDocumentTitle()

    End Sub


    Private Function CanGoPrevCodeMapDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlCodeMapDoc.TabPages.Count < 1 Then

            tp = tabControlCodeMapDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlCodeMapDoc.TabPages(0) Then

                    Return True

                End If

            End If

        End If

    End Function


    Private Function CanGoNextCodeMapDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlCodeMapDoc.TabPages.Count < 1 Then

            tp = tabControlCodeMapDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlCodeMapDoc.TabPages(tabControlCodeMapDoc.TabPages.Count - 1) Then

                    Return True

                End If

            End If

        End If

    End Function


    Private Function CanGoPrevPreviewDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlPreviewDoc.TabPages.Count < 1 Then

            tp = tabControlPreviewDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlPreviewDoc.TabPages(0) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoNextPreviewDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlPreviewDoc.TabPages.Count < 1 Then

            tp = tabControlPreviewDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlPreviewDoc.TabPages(tabControlPreviewDoc.TabPages.Count - 1) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoPrevUserDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlUserDoc.TabPages.Count < 1 Then

            tp = tabControlUserDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlUserDoc.TabPages(0) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoNextUserDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlUserDoc.TabPages.Count < 1 Then

            tp = tabControlUserDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlUserDoc.TabPages(tabControlUserDoc.TabPages.Count - 1) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoPrevXmlBehind() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlXmlBehind.TabPages.Count < 1 Then

            tp = tabControlXmlBehind.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlXmlBehind.TabPages(0) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoNextXmlBehind() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlXmlBehind.TabPages.Count < 1 Then

            tp = tabControlXmlBehind.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlXmlBehind.TabPages(tabControlXmlBehind.TabPages.Count - 1) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoPrevUmlDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlUmlDoc.TabPages.Count < 1 Then

            tp = tabControlUmlDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlUmlDoc.TabPages(0) Then

                    Return True

                End If

            End If

        End If

    End Function

    Private Function CanGoNextUmlDoc() As Boolean

        Dim tp As UserDocTabPage

        If Not tabControlUmlDoc.TabPages.Count < 1 Then

            tp = tabControlUmlDoc.SelectedTab

            If Not tp Is Nothing Then

                If Not tp Is tabControlUmlDoc.TabPages(tabControlUmlDoc.TabPages.Count - 1) Then

                    Return True

                End If

            End If

        End If

    End Function

#End Region

#Region " Other "


    Private Function SetMainTitle()

        Me.Text = PRODUCT_NAME

        If m_Beta Then Me.Text = Me.Text & " Beta " & m_BetaNr

    End Function

    Public Function Starting()

        BeginLife()

    End Function

    Private Function BeginLife()

        If m_started Then Exit Function

        m_started = True

        LoadSettings()

        LoadCustomMetaDataConfigs()

        AddErrorIcons()

        SetMainTitle()

    End Function

#End Region

#Region " Import/Export (I'm in the import/export business!) "

    Public Sub ExportToEco()

        '        Dim xml As String
        '        Dim code As String
        '        Dim ecoClassList As ecoClassList

        '        Dim domainMap As IDomainMap
        '        Dim folderPath As String
        '        Dim fileName As String

        '        Dim fileWriter As StreamWriter
        '        Dim takenNames As Hashtable


        '        FolderBrowserDialog1.Description = "Please select the target directory for your generated file(s)"

        '        If FolderBrowserDialog1.ShowDialog(Me) = DialogResult.Cancel Then

        '            Exit Sub

        '        End If

        '        folderPath = FolderBrowserDialog1.SelectedPath

        'Loopy:

        '        Select Case MsgBox("Do you want to save generated file(s) to the folder '" & folderPath & "'?" & vbCrLf & vbCrLf & "Warning! Existing files with conflicting names will be overwritten!", MsgBoxStyle.YesNoCancel, "Target Folder")

        '            Case MsgBoxResult.Yes

        '            Case MsgBoxResult.No

        '                GoTo Loopy

        '            Case MsgBoxResult.Cancel

        '                Exit Sub

        '        End Select

        '        For Each domainMap In mapTreeView.GetAllDomainMaps

        '            PushStatus("Exporting domain '" & domainMap.Name & "' to C# and xml files in Borland ECO format...")

        '            takenNames = New Hashtable

        '            ecoClassList = m_NPersistToEco.NPersistToEco(domainMap, takenNames)

        '            xml = m_EcoToXml.EcoToXml(ecoClassList)

        '            fileName = domainMap.Name & ".xml"

        '            fileWriter = File.CreateText(folderPath & "\" & fileName)

        '            fileWriter.Write(xml)

        '            fileWriter.Close()

        '            code = m_NPersistToEcoCodeCs.NPersistToEcoCode(domainMap, takenNames)

        '            fileName = domainMap.Name & ".cs"

        '            fileWriter = File.CreateText(folderPath & "\" & fileName)

        '            fileWriter.Write(code)

        '            fileWriter.Close()


        '            PullStatus()

        '        Next


    End Sub

    Public Sub ExportToNHibernate(Optional ByVal onlyDomainMap As IDomainMap = Nothing)

        Dim domainMap As IDomainMap

        If Not onlyDomainMap Is Nothing Then

            m_frmExportToNHibernate.Setup(Me, onlyDomainMap)

            m_frmExportToNHibernate.ShowDialog()

        Else

            For Each domainMap In mapTreeView.GetAllDomainMaps

                m_frmExportToNHibernate.Setup(Me, domainMap)

                m_frmExportToNHibernate.ShowDialog()

            Next

        End If

    End Sub



    Public Sub ImportFromEcoXml()

        'Dim path As String
        'Dim dllPath As String
        'Dim domainMap As IDomainMap
        'Dim ecoClassList As IList
        'Dim name As String
        'Dim rootNs As String
        'Dim arr() As String


        'Dim asm As Reflection.Assembly

        'Dim frmImportEco As New frmImportEco

        'frmImportEco.ShowDialog(Me)

        'If frmImportEco.IsCanceled Then Exit Sub


        'Try

        '    asm = Nothing

        '    path = frmImportEco.GetXmlPath
        '    dllPath = frmImportEco.GetDllPath
        '    rootNs = frmImportEco.GetRootNamespace

        '    Try

        '        asm = Reflection.Assembly.LoadFrom(dllPath)

        '    Catch ex As Exception

        '        MsgBox("Could not load assembly file '" & dllPath & "'!", MsgBoxStyle.OKOnly, "ECO Import")

        '        Exit Sub

        '    End Try

        '    ecoClassList = m_XmlToEco.XmlToEco(path)

        '    If Not ecoClassList Is Nothing Then

        '        arr = Split(path, "\")
        '        name = arr(UBound(arr))

        '        If Len(name) > 4 Then
        '            If LCase(name.Substring(Len(name) - 4)) = ".xml" Then name = name.Substring(0, Len(name) - 4)
        '        End If


        '        domainMap = m_EcoToNPersist.EcoToNPersist(asm, rootNs, ecoClassList, name)

        '        If Not domainMap Is Nothing Then

        '            AddImportedDomainToProject(domainMap)

        '        End If

        '    End If

        'Catch ex As Exception

        '    MsgBox("Could not import from file '" & path & "'! Reason: " & ex.Message, MsgBoxStyle.OKOnly)

        'End Try


    End Sub




    Public Sub ImportDtb()

    End Sub


#End Region

#Region " Messages "

    Public Sub ClearErrorMsgs()

        listViewExceptions.Items.Clear()

    End Sub

    Public Sub AddErrorMsg(ByVal mapException As MappingException)

        Dim item As MapExceptionItem
        Dim imgIndex As Integer = 26
        Dim selImgIndex As Integer = 26


        If listViewExceptions.Columns.Count < 1 Then

            listViewExceptions.Columns.Add("Object", 150, HorizontalAlignment.Left)
            listViewExceptions.Columns.Add("Setting", 150, HorizontalAlignment.Left)
            listViewExceptions.Columns.Add("Description", 250, HorizontalAlignment.Left)

        End If

        Dim name As String = ""
        If Not mapException.MapObject Is Nothing Then
            name = mapException.MapObject.Name
            IconManager.GetIconIndexesForMap(mapException.MapObject, imgIndex, selImgIndex)
            imgIndex += (imageListSmall.Images.Count / 2)
        End If
        item = New MapExceptionItem(mapException)
        item.SubItems.Add(mapException.Setting)
        item.SubItems.Add(mapException.Message)


        item.ImageIndex = imgIndex

        listViewExceptions.Items.Add(item)

        Application.DoEvents()

    End Sub

    Public Sub ClearPreviewMsgs()

        listViewPreviewMsgs.Items.Clear()

        'tabControlMessages.SelectedIndex = 1

    End Sub

    Public Sub AddPreviewMsg(ByVal diffInfo As IDiffInfo)

        Dim item As ListViewItem

        If listViewPreviewMsgs.Columns.Count < 1 Then

            listViewPreviewMsgs.Columns.Add("Description", 250, HorizontalAlignment.Left)
            listViewPreviewMsgs.Columns.Add("Setting", 150, HorizontalAlignment.Left)

        End If

        item = New ListViewItem(diffInfo.Message)
        item.SubItems.Add(diffInfo.Setting)

        item.ImageIndex = 39

        listViewPreviewMsgs.Items.Add(item)

        tabControlMessages.SelectedTab = tabPagePreviewList

        Application.DoEvents()

    End Sub


    Public Sub ClearMsgs()

        listViewMsgs.Items.Clear()

        'tabControlMessages.SelectedIndex = 2

    End Sub

    Public Sub AddMsg(ByVal message As String, Optional ByVal ShowPane As Boolean = True)

        Dim item As ListViewItem

        If listViewMsgs.Columns.Count < 1 Then

            listViewMsgs.Columns.Add("Message", 350, HorizontalAlignment.Left)

        End If

        item = New ListViewItem(message)

        item.ImageIndex = 39

        listViewMsgs.Items.Add(item)

        If ShowPane Then

            tabControlMessages.SelectedTab = tabPageMessageList

        End If

        listViewMsgs.EnsureVisible(listViewMsgs.Items.Count - 1)

        Application.DoEvents()

    End Sub

#End Region

#Region " Wizards "

    Public Enum WizardResultStatusEnum

        Canceled = 0
        OK = 1
        ErrorOccurred = 2

    End Enum

    Public Sub ReturnWizardStatus(ByVal status As WizardResultStatusEnum, ByVal msgs As ArrayList)

        Dim msg As String

        m_WizardStatus = status
        m_WizardResultMsgs.Clear()

        For Each msg In msgs
            m_WizardResultMsgs.Add(msg)
        Next

    End Sub

    Private Sub OpenGenDomWizard()

        'If Not m_Project Is Nothing Then

        Dim frmWiz As New frmGenDomWizard

        Dim msg As String

        frmWiz.Setup(Me, m_Project)

        frmWiz.ShowDialog(Me)

        RefreshAll()

        VerifyMap()

        Select Case m_WizardStatus

            Case WizardResultStatusEnum.Canceled

            Case WizardResultStatusEnum.OK

                ClearMsgs()
                AddMsg("Implement Class Model Wizard:")
                AddMsg("Class model implemented!")

                For Each msg In m_WizardResultMsgs

                    AddMsg(msg)

                Next

            Case WizardResultStatusEnum.ErrorOccurred

                ClearMsgs()
                AddMsg("Implement Class Model Wizard:")
                AddMsg("An exception occurred when implementing the class model:")

                For Each msg In m_WizardResultMsgs

                    AddMsg(msg)

                Next

        End Select

        'Else

        'MsgBox("You must first have a project with a domain model open first!")

        'End If

    End Sub

    Private Sub OpenWrapDbWizard()

        'If Not m_Project Is Nothing Then

        Dim frmWiz As New frmWrapDbWizard

        Dim msg As String

        frmWiz.Setup(Me, m_Project)

        frmWiz.ShowDialog(Me)

        RefreshAll()

        VerifyMap()


        Select Case m_WizardStatus

            Case WizardResultStatusEnum.Canceled

            Case WizardResultStatusEnum.OK

                ClearMsgs()
                AddMsg("Wrap Database Wizard:")
                AddMsg("Database wrapped!")

                For Each msg In m_WizardResultMsgs

                    AddMsg(msg)

                Next

            Case WizardResultStatusEnum.ErrorOccurred

                ClearMsgs()
                AddMsg("Implement Class Model Wizard:")
                AddMsg("An exception occurred when wrapping the database:")

                For Each msg In m_WizardResultMsgs

                    AddMsg(msg)

                Next

        End Select



        'Else

        'MsgBox("You must first have a project with a domain model open first!")

        'End If

    End Sub

#End Region

#Region "Mouse Pointer / Cursor Control "

    Private Function MouseFunction(Optional ByVal NewMouse As Cursor = Nothing) As Cursor

        Dim ub As Short

        InitializeArrayIfNeeded()

        ub = UBound(m_MousePointerStack)

        Try
            If IsNothing(NewMouse) Then
                MouseFunction = m_MousePointerStack(ub)
                ReDim Preserve m_MousePointerStack(ub - 1)
            Else
                ReDim Preserve m_MousePointerStack(ub + 1)
                m_MousePointerStack(ub + 1) = NewMouse
                Return NewMouse
            End If
        Catch ex As Exception
            Return Cursors.Default
        End Try
    End Function

    Public Function MouseReset() As Cursor
        MouseReset = MouseFunction()
    End Function

    Public Function MouseSet(Optional ByVal NewMouse As Cursor = Nothing) As Cursor
        If IsNothing(NewMouse) Then
            MouseSet = MouseFunction()
        Else
            MouseSet = MouseFunction(NewMouse)
        End If
    End Function

    Public Sub MouseClearStack()
        Erase m_MousePointerStack
    End Sub

    Private Sub InitializeArrayIfNeeded()
        Dim lb As Short
        Try
            lb = UBound(m_MousePointerStack)
        Catch ex As Exception
            ReDim m_MousePointerStack(0)
        End Try
    End Sub


#End Region

#Region " TODO: Implement these "

    Private Sub menuNamespacePaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNamespacePaste.Click

        'TODO: Move class to new namespace!
        Paste()

    End Sub

#End Region

#Region " Drag and drop "


    Private Sub TurnOnTreeDragHilite(ByVal node As TreeNode)

        If Not node Is m_TreeDragHilited Then

            TurnOffTreeDragHilite()

        End If

        m_TreeDragHilited = node

        m_TreeDragHilited.BackColor = Color.DarkBlue
        m_TreeDragHilited.ForeColor = Color.White

    End Sub

    Private Sub TurnOffTreeDragHilite()

        If Not m_TreeDragHilited Is Nothing Then

            m_TreeDragHilited.BackColor = Color.White
            m_TreeDragHilited.ForeColor = Color.Black
            m_TreeDragHilited = Nothing

        End If

    End Sub

    Private Function GetDropDataType(ByVal data As String) As String

        Dim arr() As String

        arr = Split(data, "|")

        Return arr(0)

    End Function



    Private Sub TreeDragEnter(ByVal e As System.Windows.Forms.DragEventArgs)

        Dim data As String
        Dim dataType As String

        Dim arr() As String
        Dim arrFiles() As String

        Dim ok As Boolean

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist", "omproject"

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then

                        e.Effect = DragDropEffects.Copy

                    End If

                    Exit Sub

                End If

            End If



            dataType = GetDropDataType(data)

            Select Case LCase(dataType)

                Case "domainmap", "classmap", "propertymap", "sourcemap", "tablemap", "columnmap"

                    e.Effect = DragDropEffects.Copy

            End Select

        Catch ex As Exception

        End Try

    End Sub


    Private Sub TreeDragOver(ByVal e As System.Windows.Forms.DragEventArgs)

        Dim data As String
        Dim dataType As String
        Dim overNode As MapNode
        Dim X As Long
        Dim Y As Long

        Dim DoHilite As Boolean

        Dim arr() As String
        Dim arrFiles() As String

        Dim ok As Boolean

        Dim sourceDomainMap As IDomainMap
        Dim sourceObject As Object

        Dim key As String
        Dim domName As String

        Dim classMap As IClassMap
        Dim superClassMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim invPropertyMap As IPropertyMap
        Dim sourceMap As ISourceMap
        Dim tableMap As ITableMap
        Dim columnMap As IColumnMap
        Dim forColumnMap As IColumnMap

        e.Effect = DragDropEffects.None


        'X = e.X - Me.Location.X
        'Y = e.Y - Me.Location.Y - ToolBar1.Height - labelExplorer.Height - (Me.Height - Me.ClientSize.Height)

        Dim pt As Point = mapTreeView.PointToClient(New Point(e.X, e.Y))

        X = pt.X
        Y = pt.Y

        overNode = mapTreeView.GetNodeAt(New Point(X, Y))

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist"

                                If Not overNode Is Nothing Then

                                    If Not overNode.GetType Is GetType(ProjectNode) Then

                                        ok = False

                                        Exit For

                                    End If

                                End If

                            Case "omproject"

                                If Not overNode Is Nothing Then

                                    ok = False

                                    Exit For

                                End If

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then
                        e.Effect = DragDropEffects.Copy
                        If overNode Is Nothing Then
                            TurnOffTreeDragHilite()
                        Else
                            TurnOnTreeDragHilite(overNode)
                        End If
                    Else
                        TurnOffTreeDragHilite()
                    End If

                    Exit Sub

                End If

            End If

            dataType = LCase(GetDropDataType(data))

            If overNode Is Nothing Then


            Else

                If overNode.GetMap Is Nothing Then

                Else

                    Dim mapObject As Object = overNode.GetMap

                    arr = Split(data, "|")

                    key = arr(1)
                    domName = arr(2)

                    sourceDomainMap = m_Project.GetDomainMap(domName)

                    If mapObject.GetType Is GetType(ProjectModel.Project) Then

                        Select Case dataType

                            Case "domainmap"

                                DoHilite = True

                        End Select

                    ElseIf mapObject.GetType Is GetType(DomainMap) Then

                        Select Case dataType

                            Case "classmap", "sourcemap", "tablemap"    'tablemap for mapping

                                DoHilite = True

                        End Select

                    ElseIf mapObject.GetType Is GetType(classMap) Then

                        Select Case dataType

                            Case "propertymap", "tablemap"

                                DoHilite = True

                            Case "classmap"

                                'Set class to super class of dropped class

                                classMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(classMap))

                                superClassMap = mapObject

                                If Not classMap Is Nothing Then

                                    If classMap.IsLegalAsSuperClass(superClassMap) Then

                                        DoHilite = True

                                    End If

                                End If

                        End Select

                    ElseIf mapObject.GetType Is GetType(propertyMap) Then

                        Select Case dataType

                            Case "classmap", "propertymap", "tablemap", "columnmap"

                                DoHilite = True

                        End Select

                    ElseIf mapObject.GetType Is GetType(sourceMap) Then

                        Select Case dataType

                            Case "tablemap", "classmap" 'classmap for mapping

                                DoHilite = True

                        End Select

                    ElseIf mapObject.GetType Is GetType(tableMap) Then

                        Select Case dataType

                            Case "columnmap", "classmap", "propertymap"

                                DoHilite = True

                        End Select

                    ElseIf mapObject.GetType Is GetType(columnMap) Then

                        Select Case dataType

                            Case "propertymap", "tablemap", "columnmap"

                                DoHilite = True

                        End Select

                    End If

                End If

                If DoHilite Then
                    e.Effect = DragDropEffects.Copy
                    TurnOnTreeDragHilite(overNode)
                Else
                    TurnOffTreeDragHilite()
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub TreeDragDrop(ByVal e As System.Windows.Forms.DragEventArgs)

        Dim data As String
        Dim dataType As String
        Dim overNode As MapNode
        Dim X As Long
        Dim Y As Long

        Dim DoHilite As Boolean

        Dim arr() As String
        Dim arrFiles() As String
        Dim fileNames As ArrayList
        Dim fileName As String

        Dim ok As Boolean

        Dim sourceDomainMap As IDomainMap
        Dim sourceObject As Object

        Dim key As String

        Dim classMap As IClassMap
        Dim superClassMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim invPropertyMap As IPropertyMap
        Dim sourceMap As ISourceMap
        Dim tableMap As ITableMap
        Dim columnMap As IColumnMap
        Dim forColumnMap As IColumnMap

        Dim overwrite As Boolean

        e.Effect = DragDropEffects.None


        'X = e.X - Me.Location.X
        'Y = e.Y - Me.Location.Y - ToolBar1.Height - labelExplorer.Height - (Me.Height - Me.ClientSize.Height)

        Dim pt As Point = mapTreeView.PointToClient(New Point(e.X, e.Y))

        X = pt.X
        Y = pt.Y

        overNode = mapTreeView.GetNodeAt(New Point(X, Y))

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist"

                                If Not overNode Is Nothing Then

                                    If Not overNode.GetType Is GetType(ProjectNode) Then

                                        ok = False

                                        Exit For

                                    End If

                                End If

                            Case "omproject"

                                If Not overNode Is Nothing Then

                                    ok = False

                                    Exit For

                                End If

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then
                        e.Effect = DragDropEffects.Copy
                        If overNode Is Nothing Then
                            TurnOffTreeDragHilite()
                        Else

                            fileNames = New ArrayList

                            For Each fileName In arrFiles

                                fileNames.Add(fileName)

                            Next

                            Open(fileNames)

                        End If
                    Else
                        TurnOffTreeDragHilite()
                    End If

                    Exit Sub

                End If

            End If

            dataType = LCase(GetDropDataType(data))

            If overNode Is Nothing Then


            Else

                If overNode.GetMap Is Nothing Then

                Else

                    Dim mapObject As Object = overNode.GetMap

                    arr = Split(data, "|")

                    key = arr(1)

                    sourceDomainMap = m_Project.GetDomainMap(arr(2))

                    If Not sourceDomainMap Is Nothing Then


                        If mapObject.GetType Is GetType(ProjectModel.Project) Then

                            Select Case dataType

                                Case "domainmap"

                                    'PerformCut(, mapObject)

                            End Select

                        ElseIf mapObject.GetType Is GetType(DomainMap) Then

                            Select Case dataType

                                Case "classmap"

                                    sourceObject = GetMapObjectFromKey(sourceDomainMap, key, GetType(classMap))

                                    If Not sourceObject Is Nothing Then

                                        PerformCut(sourceObject, mapObject)

                                    End If

                                Case "sourcemap"

                                    sourceObject = GetMapObjectFromKey(sourceDomainMap, key, GetType(sourceMap))

                                    If Not sourceObject Is Nothing Then

                                        PerformCut(sourceObject, mapObject)

                                    End If


                                Case "tablemap"    'tablemap for mapping


                            End Select

                        ElseIf mapObject.GetType Is GetType(classMap) Then

                            Select Case dataType

                                Case "propertymap"

                                    sourceObject = GetMapObjectFromKey(sourceDomainMap, key, GetType(propertyMap))

                                    If Not sourceObject Is Nothing Then

                                        PerformCut(sourceObject, mapObject)

                                    End If

                                Case "tablemap"

                                    'Set class to map to the dropped table
                                    tableMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(tableMap))

                                    classMap = mapObject

                                    MapTableToClass(tableMap, classMap)



                                Case "classmap"

                                    'Set class to super class of dropped class

                                    classMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(classMap))

                                    superClassMap = mapObject

                                    If Not classMap Is Nothing Then

                                        If classMap.IsLegalAsSuperClass(superClassMap) Then

                                            MapServices.SetSuperClass(classMap, superClassMap)

                                        End If

                                    End If

                            End Select

                        ElseIf mapObject.GetType Is GetType(propertyMap) Then

                            Select Case dataType

                                Case "classmap"

                                    sourceObject = GetMapObjectFromKey(sourceDomainMap, key, GetType(classMap))

                                    If Not sourceObject Is Nothing Then

                                        'Set property data type to dropped class
                                        propertyMap = mapObject

                                        If propertyMap.IsCollection Then

                                            propertyMap.ItemType = CType(sourceObject, classMap).Name

                                        Else

                                            propertyMap.DataType = CType(sourceObject, classMap).Name

                                        End If

                                        If propertyMap.ReferenceType = ReferenceType.None Then

                                            If propertyMap.IsCollection Then

                                                propertyMap.ReferenceType = ReferenceType.ManyToMany

                                            Else

                                                propertyMap.ReferenceType = ReferenceType.OneToMany

                                            End If

                                            'Ask if user would like to create the inverse property
                                            If propertyMap.GetInversePropertyMap Is Nothing Then

                                                DialogueServices.AskToCreateIfInverseNotExist("", propertyMap)

                                            End If



                                        End If

                                    End If

                                Case "propertymap"

                                    'Set the inverse of the property to the dropped property
                                    invPropertyMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(propertyMap))

                                    propertyMap = mapObject

                                    If Not invPropertyMap Is Nothing Then

                                        If Not invPropertyMap Is propertyMap Then

                                            propertyMap.Inverse = invPropertyMap.Name

                                        End If

                                    End If


                                Case "tablemap"

                                    'Set property to map to the dropped table
                                    tableMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(tableMap))

                                    propertyMap = mapObject

                                    If Not tableMap Is Nothing Then

                                        propertyMap.Table = tableMap.Name

                                        If propertyMap.ClassMap.Table = "" And propertyMap.ClassMap.Source = "" Then

                                            propertyMap.ClassMap.Table = tableMap.Name

                                            If Not tableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                propertyMap.ClassMap.Source = tableMap.Name

                                            End If

                                        Else

                                            If Not LCase(propertyMap.ClassMap.Table) = LCase(tableMap.Name) Then

                                                propertyMap.Table = tableMap.Name

                                            End If

                                            If propertyMap.ClassMap.Source = "" Then

                                                If Not tableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                    propertyMap.Source = tableMap.SourceMap.Name

                                                End If

                                            Else

                                                If Not tableMap.SourceMap.Name = propertyMap.Source Then

                                                    If Not tableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                        propertyMap.Source = tableMap.SourceMap.Name

                                                    Else

                                                        propertyMap.Source = ""

                                                    End If

                                                End If

                                            End If

                                        End If



                                    End If


                                Case "columnmap"

                                    'Set property to map to the dropped column
                                    columnMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(columnMap))

                                    propertyMap = mapObject

                                    If Not columnMap Is Nothing Then

                                        propertyMap.Column = columnMap.Name

                                        If propertyMap.IsIdentity Then

                                            columnMap.IsPrimaryKey = True

                                        End If

                                        If columnMap.IsPrimaryKey Then

                                            propertyMap.IsIdentity = True

                                        End If

                                        If propertyMap.ClassMap.Table = "" And propertyMap.ClassMap.Source = "" Then

                                            propertyMap.ClassMap.Table = columnMap.TableMap.Name

                                            If Not columnMap.TableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                propertyMap.ClassMap.Source = columnMap.TableMap.Name

                                            End If

                                        Else

                                            If Not LCase(propertyMap.ClassMap.Table) = LCase(columnMap.TableMap.Name) Then

                                                propertyMap.Table = columnMap.TableMap.Name

                                            End If

                                            If propertyMap.ClassMap.Source = "" Then

                                                If Not columnMap.TableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                    propertyMap.Source = columnMap.TableMap.SourceMap.Name

                                                End If

                                            Else

                                                If Not columnMap.TableMap.SourceMap.Name = propertyMap.Source Then

                                                    If Not columnMap.TableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                        propertyMap.Source = columnMap.TableMap.SourceMap.Name

                                                    Else

                                                        propertyMap.Source = ""

                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                            End Select

                        ElseIf mapObject.GetType Is GetType(sourceMap) Then

                            Select Case dataType

                                Case "tablemap"

                                    sourceObject = GetMapObjectFromKey(sourceDomainMap, key, GetType(tableMap))

                                    If Not sourceObject Is Nothing Then

                                        PerformCut(sourceObject, mapObject)

                                    End If


                                Case "classmap" 'classmap for mapping
                                    DoHilite = True

                            End Select

                        ElseIf mapObject.GetType Is GetType(tableMap) Then

                            Select Case dataType

                                Case "columnmap"

                                    sourceObject = GetMapObjectFromKey(sourceDomainMap, key, GetType(columnMap))

                                    If Not sourceObject Is Nothing Then

                                        PerformCut(sourceObject, mapObject)

                                    End If

                                Case "classmap"

                                    'Set dropped class to map to the table
                                    classMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(classMap))

                                    tableMap = mapObject

                                    If Not classMap Is Nothing Then

                                        classMap.Table = tableMap.Name

                                        If classMap.Source = "" Then

                                            If Not tableMap.SourceMap.Name = classMap.DomainMap.Source Then

                                                classMap.Source = tableMap.Name

                                            End If

                                        Else

                                            If classMap.Source = "" Then

                                                If Not tableMap.SourceMap.Name = classMap.DomainMap.Source Then

                                                    classMap.Source = tableMap.SourceMap.Name

                                                End If

                                            Else

                                                If Not tableMap.SourceMap.Name = classMap.Source Then

                                                    If Not tableMap.SourceMap.Name = classMap.DomainMap.Source Then

                                                        classMap.Source = tableMap.SourceMap.Name

                                                    Else

                                                        classMap.Source = ""

                                                    End If

                                                End If

                                            End If

                                        End If


                                    End If

                                Case "propertymap"

                                    'Set dropped property to map to the table
                                    propertyMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(propertyMap))

                                    tableMap = mapObject

                                    If Not propertyMap Is Nothing Then

                                        propertyMap.Table = tableMap.Name

                                        If propertyMap.ClassMap.Table = "" And propertyMap.ClassMap.Source = "" Then

                                            propertyMap.ClassMap.Table = tableMap.Name

                                            If Not tableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                propertyMap.ClassMap.Source = tableMap.Name

                                            End If

                                        Else

                                            If Not LCase(propertyMap.ClassMap.Table) = LCase(tableMap.Name) Then

                                                propertyMap.Table = tableMap.Name

                                            End If

                                            If propertyMap.ClassMap.Source = "" Then

                                                If Not tableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                    propertyMap.Source = tableMap.SourceMap.Name

                                                End If

                                            Else

                                                If Not tableMap.SourceMap.Name = propertyMap.Source Then

                                                    If Not tableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                        propertyMap.Source = tableMap.SourceMap.Name

                                                    Else

                                                        propertyMap.Source = ""

                                                    End If

                                                End If

                                            End If

                                        End If


                                    End If

                            End Select

                        ElseIf mapObject.GetType Is GetType(columnMap) Then

                            Select Case dataType

                                Case "propertymap"

                                    'Set dropped property to map to the column
                                    propertyMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(propertyMap))

                                    columnMap = mapObject

                                    If Not propertyMap Is Nothing Then

                                        propertyMap.Column = columnMap.Name

                                        If propertyMap.IsIdentity Then

                                            columnMap.IsPrimaryKey = True

                                        End If

                                        If columnMap.IsPrimaryKey Then

                                            propertyMap.IsIdentity = True

                                        End If

                                        If propertyMap.ClassMap.Table = "" And propertyMap.ClassMap.Source = "" Then

                                            propertyMap.ClassMap.Table = columnMap.TableMap.Name

                                            If Not columnMap.TableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                propertyMap.ClassMap.Source = columnMap.TableMap.Name

                                            End If

                                        Else

                                            If Not LCase(propertyMap.ClassMap.Table) = LCase(columnMap.TableMap.Name) Then

                                                propertyMap.Table = columnMap.TableMap.Name

                                            End If

                                            If propertyMap.ClassMap.Source = "" Then

                                                If Not columnMap.TableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                    propertyMap.Source = columnMap.TableMap.SourceMap.Name

                                                End If

                                            Else

                                                If Not columnMap.TableMap.SourceMap.Name = propertyMap.Source Then

                                                    If Not columnMap.TableMap.SourceMap.Name = propertyMap.ClassMap.DomainMap.Source Then

                                                        propertyMap.Source = columnMap.TableMap.SourceMap.Name

                                                    Else

                                                        propertyMap.Source = ""

                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                                Case "tablemap"

                                    'Set dropped table as primary table of the column
                                    tableMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(tableMap))

                                    columnMap = mapObject

                                    If Not tableMap Is Nothing Then

                                        columnMap.PrimaryKeyTable = tableMap.Name

                                        columnMap.IsForeignKey = True

                                    End If

                                Case "columnmap"

                                    'Set dropped column as primary column of the column
                                    forColumnMap = GetMapObjectFromKey(sourceDomainMap, key, GetType(columnMap))

                                    columnMap = mapObject

                                    If Not forColumnMap Is Nothing Then

                                        columnMap.PrimaryKeyColumn = forColumnMap.Name

                                        columnMap.PrimaryKeyTable = forColumnMap.TableMap.Name

                                        columnMap.IsForeignKey = True

                                    End If

                            End Select

                        End If


                    End If

                End If

            End If

        Catch ex As Exception

        End Try

        TurnOffTreeDragHilite()

        RefreshAll()

    End Sub

    Private Sub TreeItemDrag(ByVal e As System.Windows.Forms.ItemDragEventArgs)

        Dim obj As Object
        Dim node As MapNode
        Dim dragMsg As String

        node = CType(e.Item, MapNode)

        mapTreeView.SelectedNode = node

        obj = node.GetMap

        If obj Is Nothing Then

        Else

            If obj.GetType Is GetType(DomainMap) Then

                dragMsg = "DomainMap|" & CType(obj, IMap).GetKey & "|" & CType(obj, DomainMap).Name

            ElseIf obj.GetType Is GetType(ClassMap) Then

                dragMsg = "ClassMap|" & CType(obj, IMap).GetKey & "|" & CType(obj, ClassMap).DomainMap.Name

            ElseIf obj.GetType Is GetType(PropertyMap) Then

                dragMsg = "PropertyMap|" & CType(obj, IMap).GetKey & "|" & CType(obj, PropertyMap).ClassMap.DomainMap.Name

            ElseIf obj.GetType Is GetType(SourceMap) Then

                dragMsg = "SourceMap|" & CType(obj, IMap).GetKey & "|" & CType(obj, SourceMap).DomainMap.Name

            ElseIf obj.GetType Is GetType(TableMap) Then

                dragMsg = "TableMap|" & CType(obj, IMap).GetKey & "|" & CType(obj, TableMap).SourceMap.DomainMap.Name

            ElseIf obj.GetType Is GetType(ColumnMap) Then

                dragMsg = "ColumnMap|" & CType(obj, IMap).GetKey & "|" & CType(obj, ColumnMap).TableMap.SourceMap.DomainMap.Name

            ElseIf obj.GetType Is GetType(UmlDiagram) Then

                dragMsg = "UmlDiagram|" & CType(obj, IMap).GetKey & "|" & CType(obj, UmlDiagram).GetDomainMap.Name

            End If

        End If

        If Len(dragMsg) > 0 Then

            mapTreeView.DoDragDrop(dragMsg, DragDropEffects.Copy Or DragDropEffects.Move)

        End If

    End Sub

    Public Function GetMapObjectFromKey(ByVal domainMap As IDomainMap, ByVal key As String, ByVal type As Type) As IMap

        Dim i As Long
        Dim className As String
        Dim propertyName As String
        Dim sourceName As String
        Dim tableName As String
        Dim columnName As String
        Dim diagramName As String
        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim sourceMap As ISourceMap
        Dim tableMap As ITableMap
        Dim columnMap As IColumnMap
        Dim umlDiagram As umlDiagram

        Dim arr() As String

        Try


            If type Is GetType(domainMap) Then


            ElseIf type Is GetType(classMap) Then

                className = StripKey(key, domainMap.Name)

                classMap = domainMap.GetClassMap(className)

                Return classMap

            ElseIf type Is GetType(propertyMap) Then

                arr = Split(key, ".")

                className = StripKey(key, domainMap.Name)

                classMap = domainMap.GetClassMap(className)

                propertyName = arr(UBound(arr))

                propertyMap = classMap.GetPropertyMap(propertyName)

                Return propertyMap

            ElseIf type Is GetType(sourceMap) Then

                sourceName = StripKey(key, domainMap.Name)

                sourceMap = domainMap.GetSourceMap(sourceName)

                Return sourceMap

            ElseIf type Is GetType(tableMap) Then

                arr = Split(key, ".")

                sourceName = StripKey(key, domainMap.Name)

                sourceMap = domainMap.GetSourceMap(sourceName)

                tableName = arr(UBound(arr))

                tableMap = sourceMap.GetTableMap(tableName)

                Return tableMap

            ElseIf type Is GetType(columnMap) Then

                arr = Split(key, ".")

                sourceName = StripKey(key, domainMap.Name)

                sourceMap = domainMap.GetSourceMap(sourceName)

                tableName = arr(UBound(arr) - 1)

                columnName = arr(UBound(arr))

                tableMap = sourceMap.GetTableMap(tableName)

                columnMap = tableMap.GetColumnMap(columnName)

                Return columnMap


            ElseIf type Is GetType(umlDiagram) Then

                diagramName = key

                'diagramName = StripKey(key, domainMap.Name)

                umlDiagram = m_Project.GetDiagram(domainMap, diagramName)

                Return umlDiagram

            End If

        Catch ex As Exception

        End Try

    End Function

    Private Function StripKey(ByVal key As String, ByVal domainName As String) As String

        If Len(key) > Len(domainName) Then

            If LCase(key).Substring(0, Len(domainName)) = LCase(domainName) Then

                Return key.Substring(Len(domainName) + 1)

            End If

        End If

        Return key

    End Function


    Private Sub MapTableToClass(ByVal tableMap As ITableMap, ByVal classMap As IClassMap)

        Dim overwrite As Boolean
        Dim columnMap As IColumnMap
        Dim forColumnMap As IColumnMap
        Dim addColumnMap As IColumnMap
        Dim propertyMap As IPropertyMap

        Dim addColumns As ArrayList
        Dim foreignColumns As ArrayList

        Dim tblToCls As New TablesToClasses
        Dim i As Long

        Dim name As String
        Dim mappedKeys As New ArrayList

        Dim ignore As Boolean

        If Not classMap Is Nothing Then

            overwrite = False

            If Len(classMap.Table) > 0 Then

                If classMap.GetTableMap Is tableMap Then Exit Sub

            End If

            If Len(classMap.Table) > 0 Then

                If Not MayMapAsNonPrimary(tableMap, classMap) Then

                    If MsgBox("The class you have dropped the table on is already mapping to a table, and the table you have dropped can not be mapped as non-primary (because it doesn not have a foreign key relationship with the primary table that the class is already mapping to)." & vbCrLf & vbCrLf & "Would you like to overwrite the existing table ('" & classMap.Table & "') and map let the class map to this table ('" & tableMap.Name & "') instead?", MsgBoxStyle.OKCancel, "Map Table") = MsgBoxResult.OK Then

                        overwrite = True

                    Else

                        Exit Sub

                    End If

                End If

            End If

            tableMap.SourceMap.DomainMap.Dirty = True

            If Len(classMap.Table) < 1 Or overwrite Then

                classMap.Table = tableMap.Name

                If classMap.Source = "" Then

                    If Not tableMap.SourceMap.Name = classMap.DomainMap.Source Then

                        classMap.Source = tableMap.SourceMap.Name

                    End If

                Else

                    If Not tableMap.SourceMap.Name = classMap.Source Then

                        If Not tableMap.SourceMap.Name = classMap.DomainMap.Source Then

                            classMap.Source = tableMap.SourceMap.Name

                        Else

                            classMap.Source = ""

                        End If

                    End If

                End If

                For Each columnMap In tableMap.ColumnMaps

                    If Not IsMapped(columnMap, classMap) Then

                        ignore = False

                        If columnMap.IsForeignKey Then

                            If Len(columnMap.ForeignKeyName) > 0 Then

                                If mappedKeys.Contains(columnMap.ForeignKeyName) Then

                                    ignore = True

                                    mappedKeys.Add(columnMap.ForeignKeyName)

                                End If

                            End If

                        End If

                        If Not ignore Then

                            name = tblToCls.GetPropertyNameForColumn(columnMap)

                            name = GetUniquePropertyName(classMap, name)

                            propertyMap = classMap.GetPropertyMap(name)

                            If propertyMap Is Nothing Then

                                propertyMap = New propertyMap

                                propertyMap.Name = name

                                propertyMap.ClassMap = classMap

                            End If

                            tblToCls.SetPropertyTypeFromColumn(propertyMap, columnMap)

                            propertyMap.Column = columnMap.Name

                            propertyMap.IsIdentity = columnMap.IsPrimaryKey

                            If columnMap.IsPrimaryKey Then

                                propertyMap.IdentityIndex = classMap.GetIdentityPropertyMaps.Count - 1

                            End If

                            If columnMap.IsForeignKey Then

                                If Len(columnMap.ForeignKeyName) > 0 Then

                                    addColumns = columnMap.TableMap.GetForeignKeyColumnMaps(columnMap.ForeignKeyName)

                                    For Each addColumnMap In addColumns

                                        If Not addColumnMap Is columnMap Then

                                            propertyMap.AdditionalIdColumns.Add(addColumnMap.Name)

                                        End If

                                    Next

                                End If

                            End If

                        End If

                    End If

                Next


            Else
                'Map non-primary table

                foreignColumns = GetNonPrimaryForeignColumns(tableMap, classMap)

                For Each columnMap In tableMap.ColumnMaps

                    If Not foreignColumns.Contains(columnMap) Then

                        If Not IsMapped(columnMap, classMap) Then

                            ignore = False

                            If columnMap.IsForeignKey Then

                                If Len(columnMap.ForeignKeyName) > 0 Then

                                    If mappedKeys.Contains(columnMap.ForeignKeyName) Then

                                        ignore = True

                                        mappedKeys.Add(columnMap.ForeignKeyName)

                                    End If

                                End If

                            End If

                            If Not ignore Then

                                name = tblToCls.GetPropertyNameForColumn(columnMap)

                                name = GetUniquePropertyName(classMap, name)

                                propertyMap = classMap.GetPropertyMap(name)

                                If propertyMap Is Nothing Then

                                    propertyMap = New propertyMap

                                    propertyMap.Name = name

                                    propertyMap.ClassMap = classMap

                                End If

                                tblToCls.SetPropertyTypeFromColumn(propertyMap, columnMap)

                                propertyMap.Table = tableMap.Name

                                propertyMap.Column = columnMap.Name

                                If columnMap.IsForeignKey Then

                                    If Len(columnMap.ForeignKeyName) > 0 Then

                                        addColumns = columnMap.TableMap.GetForeignKeyColumnMaps(columnMap.ForeignKeyName)

                                        For Each addColumnMap In addColumns

                                            If Not addColumnMap Is columnMap Then

                                                propertyMap.AdditionalIdColumns.Add(addColumnMap.Name)

                                            End If

                                        Next

                                    End If

                                End If

                                i = 0
                                For Each forColumnMap In foreignColumns

                                    If i = 0 Then

                                        propertyMap.IdColumn = forColumnMap.Name

                                    Else

                                        propertyMap.AdditionalIdColumns.Add(forColumnMap.Name)

                                    End If

                                    i += 1

                                Next

                            End If

                        End If

                    End If

                Next

            End If

        End If

    End Sub


    Private Function GetUniquePropertyName(ByVal classMap As IClassMap, ByVal name As String) As String

        Dim propertyMap As IPropertyMap
        Dim i As Long = 1
        Dim basename As String = name

        While Not GetMappedPropertyMap(classMap, name) Is Nothing

            name = basename & i

            i += 1

        End While

        Return name

    End Function

    Private Function GetMappedPropertyMap(ByVal classMap As IClassMap, ByVal name As String) As IPropertyMap

        Dim propertyMap As IPropertyMap = classMap.GetPropertyMap(name)

        If Not propertyMap Is Nothing Then

            If propertyMap.GetColumnMap Is Nothing Then

                Return Nothing

            End If

        End If

        Return propertyMap

    End Function

    Private Function IsMapped(ByVal columnMap As IColumnMap, ByVal classMap As IClassMap) As Boolean

        Dim propertyMap As IPropertyMap
        Dim addColumnMap As IColumnMap

        For Each propertyMap In classMap.GetAllPropertyMaps

            If propertyMap.GetColumnMap Is columnMap Then

                Return True

            End If

            If propertyMap.GetIdColumnMap Is columnMap Then

                Return True

            End If

            For Each addColumnMap In propertyMap.GetAdditionalColumnMaps

                If addColumnMap Is columnMap Then

                    Return True

                End If

            Next

            For Each addColumnMap In propertyMap.GetAdditionalIdColumnMaps

                If addColumnMap Is columnMap Then

                    Return True

                End If

            Next

        Next

        Return False

    End Function

    Private Function MayMapAsNonPrimary(ByVal tableMap As ITableMap, ByVal classMap As IClassMap) As Boolean

        Dim ok As Boolean
        Dim foreignColumns As ArrayList
        Dim primaryColumns As ArrayList

        Dim primTableMap As ITableMap = classMap.GetTableMap

        If Not primTableMap Is Nothing Then

            If primTableMap.SourceMap Is tableMap.SourceMap Then

                foreignColumns = GetNonPrimaryForeignColumns(tableMap, classMap)

                If foreignColumns.Count > 0 Then

                    primaryColumns = primTableMap.GetPrimaryKeyColumnMaps

                    If primaryColumns.Count = foreignColumns.Count Then

                        ok = True

                    End If

                End If

            End If

        End If

        Return ok

    End Function

    Private Function GetNonPrimaryForeignColumns(ByVal tableMap As ITableMap, ByVal classMap As IClassMap) As ArrayList

        Dim primTableMap As ITableMap = classMap.GetTableMap
        Dim columnMap As IColumnMap
        Dim foreignColumns As New ArrayList

        Dim keyName As String
        Dim isFirst As Boolean = True

        If Not primTableMap Is Nothing Then

            If primTableMap.SourceMap Is tableMap.SourceMap Then

                For Each columnMap In tableMap.ColumnMaps

                    If columnMap.IsPrimaryKey Then

                        If columnMap.IsForeignKey Then

                            If columnMap.GetPrimaryKeyTableMap Is primTableMap Then

                                If Not columnMap.GetPrimaryKeyColumnMap Is Nothing Then

                                    If columnMap.GetPrimaryKeyColumnMap.IsPrimaryKey Then

                                        If isFirst Then

                                            isFirst = False

                                            keyName = columnMap.ForeignKeyName

                                        End If

                                        If keyName = columnMap.ForeignKeyName Then

                                            foreignColumns.Add(columnMap)

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If

                Next

            End If

        End If

        Return foreignColumns

    End Function


#End Region

#Region " AppSettings "


    Private Sub LoadSettings()

        Dim settingsPath As String = Application.LocalUserAppDataPath & "\..\settings.xml"

        Try

            Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(ApplicationSettings))
            ' To read the file, create a FileStream object.
            Dim myFileStream As FileStream = New FileStream(settingsPath, FileMode.Open, FileAccess.Read)

            m_ApplicationSettings = CType( _
            mySerializer.Deserialize(myFileStream), ApplicationSettings)

            myFileStream.Close()

            m_ApplicationSettings.Setup()

        Catch ex As Exception

            m_ApplicationSettings = New ApplicationSettings

            m_ApplicationSettings.ResetAll()

            Try

                m_ApplicationSettings.Save(settingsPath)

            Catch ex2 As Exception

            End Try

        End Try

    End Sub

    Public Sub SaveAppSettings()

        Dim settingsPath As String = Application.LocalUserAppDataPath & "\..\settings.xml"

        Try

            m_ApplicationSettings.Save(settingsPath)

        Catch ex As Exception

        End Try

    End Sub


    Private Sub LoadWinSettings()

        If Not m_ApplicationSettings Is Nothing Then

            Me.Size = m_ApplicationSettings.WinSize
            Me.Location = m_ApplicationSettings.WinLocation
            Me.WindowState = m_ApplicationSettings.WinState

            'treeProviders.Font = m_ApplicationSettings.TreeFont.ToFont
            'listItems.Font = m_ApplicationSettings.ListFont.ToFont
            'HTMLRenderer.FontSettings = m_ApplicationSettings.ViewFont

        End If

    End Sub

    Private Sub SaveWinSettings()

        If Not m_ApplicationSettings Is Nothing And Not m_settingUp Then

            m_ApplicationSettings.WinState = Me.WindowState
            If Me.WindowState = FormWindowState.Normal Then
                m_ApplicationSettings.WinLocation = Me.Location
                m_ApplicationSettings.WinSize = Me.Size
            End If

            SaveAppSettings()

        End If

    End Sub

    Private Sub LoadPanelSizes()

        If Not m_ApplicationSettings Is Nothing Then

            'panelTree.Size = m_ApplicationSettings.TreePanelSize
            'panelProps.Size = m_ApplicationSettings.PropPanelSize
            'panelList.Size = m_ApplicationSettings.ListPanelSize

        End If

    End Sub

    Private Sub SavePanelSizes()

        If Not m_ApplicationSettings Is Nothing And Not m_settingUp Then

            'm_ApplicationSettings.TreePanelSize = panelTree.Size
            'm_ApplicationSettings.PropPanelSize = panelProps.Size
            'm_ApplicationSettings.ListPanelSize = panelList.Size

            SaveAppSettings()

        End If

    End Sub

    Public Sub SetLayout()

        With m_ApplicationSettings.OptionSettings.EnvironmentSettings

            'panelLeft.Dock = .PanelExplorerDock
            'Splitter1.Dock = .PanelExplorerDock

            'panelRight.Dock = .PanelToolsDock
            'Splitter5.Dock = .PanelToolsDock

            'panelBottom.Dock = .PanelListDock
            'Splitter4.Dock = .PanelListDock


            panelRight.Visible = .ShowToolsPanel
            panelProperties.Visible = .ShowPropertiesPanel
            panelMain.Visible = .ShowPropAndDocPanel
            panelLeft.Visible = .ShowExplorerPanel
            panelBottom.Visible = .ShowListPanel
            ToolBar1.Visible = .ShowToolBar
            panelStatus.Visible = .ShowStatusBar

        End With

        SetProjectExplorerVisibility()
        SetToolsVisibility()
        SetMsgListVisibility()
        SetToolBarVisibility()
        SetStatusBarVisibility()
        SetMainVisibility()

    End Sub



#End Region

#Region " Visibility "

    Private Sub ToggleProjectExplorerVisibility()

        If panelLeft.Visible Then

            panelLeft.Visible = False

        Else

            panelLeft.Visible = True

        End If

    End Sub

    Private Sub TogglePropertiesVisibility()

        If panelProperties.Visible Then

            panelProperties.Visible = False

            If panelRight.Visible Or panelMain.Visible Then
                If panelBottom.Dock = DockStyle.Fill Then panelBottom.Dock = DockStyle.Bottom
                buttonSwapPosList.Visible = True
            Else
                panelBottom.Dock = DockStyle.Fill
                buttonSwapPosList.Visible = False
            End If

        Else

            panelProperties.Visible = True

            If panelBottom.Dock = DockStyle.Fill Then panelBottom.Dock = DockStyle.Bottom

        End If

    End Sub


    Private Sub ToggleMainVisibility()

        If panelMain.Visible Then

            panelMain.Visible = False
            panelRight.Dock = DockStyle.Fill
            buttonSwapPosTools.Visible = False

            If panelRight.Visible Then
                If panelProperties.Dock = DockStyle.Fill Then panelProperties.Dock = DockStyle.Left
                buttonSwapPosProperties.Visible = True
            Else
                panelProperties.Dock = DockStyle.Fill
                buttonSwapPosProperties.Visible = False
            End If

            If panelRight.Visible Or panelProperties.Visible Then
                If panelBottom.Dock = DockStyle.Fill Then panelBottom.Dock = DockStyle.Bottom
                buttonSwapPosList.Visible = True
            Else
                panelBottom.Dock = DockStyle.Fill
                buttonSwapPosList.Visible = False
            End If

        Else

            panelMain.Visible = True
            buttonSwapPosList.Visible = True
            buttonSwapPosTools.Visible = True
            buttonSwapPosProperties.Visible = True

            If panelProperties.Dock = DockStyle.Fill Then panelProperties.Dock = DockStyle.Left
            If panelRight.Dock = DockStyle.Fill Then panelRight.Dock = DockStyle.Right
            If panelBottom.Dock = DockStyle.Fill Then panelBottom.Dock = DockStyle.Bottom

        End If

    End Sub

    Private Sub ToggleToolsVisibility()

        If panelRight.Visible Then

            panelRight.Visible = False
            Splitter5.Enabled = False
            buttonSwapPosMain.Visible = True

            If panelMain.Visible Then
                If panelProperties.Dock = DockStyle.Fill Then panelProperties.Dock = DockStyle.Left
                buttonSwapPosProperties.Visible = True
            Else
                panelProperties.Dock = DockStyle.Fill
                buttonSwapPosProperties.Visible = False
            End If

            If panelMain.Visible Or panelProperties.Visible Then
                If panelBottom.Dock = DockStyle.Fill Then panelBottom.Dock = DockStyle.Bottom
                buttonSwapPosList.Visible = True
            Else
                panelBottom.Dock = DockStyle.Fill
                buttonSwapPosList.Visible = False
            End If

        Else

            Splitter5.Enabled = True
            panelRight.Visible = True
            buttonSwapPosList.Visible = True
            buttonSwapPosMain.Visible = True
            buttonSwapPosProperties.Visible = True
            If panelProperties.Dock = DockStyle.Fill Then panelProperties.Dock = DockStyle.Left
            If panelBottom.Dock = DockStyle.Fill Then panelBottom.Dock = DockStyle.Bottom

        End If


    End Sub

    Private Sub ToggleMsgListVisibility()

        If panelBottom.Visible Then

            panelBottom.Visible = False

        Else

            panelBottom.Visible = True

        End If


    End Sub


    Private Sub ToggleToolBarVisibility()

        If ToolBar1.Visible Then

            ToolBar1.Visible = False

        Else

            ToolBar1.Visible = True

        End If

    End Sub

    Private Sub ToggleStatusBarVisibility()

        If panelStatus.Visible Then

            panelStatus.Visible = False

        Else

            panelStatus.Visible = True

        End If

    End Sub


    Private Sub SetProjectExplorerVisibility()

    End Sub

    Private Sub SetMainVisibility()

        If Not panelMain.Visible Then

            panelRight.Dock = DockStyle.Fill
            buttonSwapPosTools.Visible = False

            If panelRight.Visible Then
                panelBottom.Dock = DockStyle.Bottom
                buttonSwapPosList.Visible = True
            Else
                panelBottom.Dock = DockStyle.Fill
                buttonSwapPosList.Visible = False
            End If

        Else

            panelRight.Dock = DockStyle.Right
            panelBottom.Dock = DockStyle.Bottom
            buttonSwapPosList.Visible = True
            buttonSwapPosTools.Visible = True

        End If

    End Sub

    Private Sub SetToolsVisibility()

        If Not panelRight.Visible Then

            buttonSwapPosMain.Visible = False

            If panelMain.Visible Then
                panelBottom.Dock = DockStyle.Bottom
                buttonSwapPosList.Visible = True
            Else
                panelBottom.Dock = DockStyle.Fill
                buttonSwapPosList.Visible = False
            End If

        Else

            panelBottom.Dock = DockStyle.Bottom
            buttonSwapPosList.Visible = True
            buttonSwapPosMain.Visible = True

        End If


    End Sub

    Private Sub SetMsgListVisibility()

    End Sub


    Private Sub SetToolBarVisibility()

    End Sub

    Private Sub SetStatusBarVisibility()

    End Sub



    Private Sub SaveVisibilitySettings()

        With m_ApplicationSettings.OptionSettings.EnvironmentSettings

            .ShowExplorerPanel = panelLeft.Visible
            .ShowPropAndDocPanel = panelMain.Visible
            .ShowPropertiesPanel = panelProperties.Visible
            .ShowToolsPanel = panelRight.Visible

            .ShowListPanel = panelBottom.Visible
            .ShowToolBar = ToolBar1.Visible
            .ShowStatusBar = panelStatus.Visible

            .PanelExplorerDock = panelLeft.Dock
            .PanelToolsDock = panelRight.Dock
            .PanelListDock = panelBottom.Dock

        End With




        SaveAppSettings()

    End Sub

#End Region

#Region " Uml Diagrams "


    Public Function ViewUmlDiagram(ByVal umlDiagram As UmlDiagram) As UserDocTabPage

        ShowMain()

        If Not panelMain.Visible Then

            ToggleMainVisibility()

            SaveVisibilitySettings()

        End If

        tabControlDocuments.SelectedTab = tabPageMainUml

        Dim doc As UserDocTabPage

        For Each doc In tabControlUmlDoc.TabPages

            If doc.UmlDiagram Is umlDiagram Then

                tabControlUmlDoc.SelectedTab = doc

                doc.RefreshUml()

                doc.Focus()

                Return doc

            End If

        Next

        m_NoRefreshUmlDoc = True

        doc = New UserDocTabPage(Me, umlDiagram)

        ''doc.TextBox.ContextMenu = menuUserDoc

        'AddHandler doc.UpdatedDomain, AddressOf Me.HandleXmlBehindUpdatedDomain
        'AddHandler doc.TextBoxTextChanged, AddressOf Me.HandleXmlBehindTextBoxTextChanged

        'AddHandler doc.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

        ''AddHandler doc.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp


        tabControlUmlDoc.TabPages.Add(doc)

        tabControlUmlDoc.SelectedTab = doc

        doc.Dirty = False

        doc.RefreshUml()

        doc.Focus()

        doc.Dirty = False

        SetUmlDocumentTitle()

        m_NoRefreshUmlDoc = False

        Return doc

    End Function


    Private Function ViewDomainUmlDiagram(ByVal domainMap As IDomainMap) As UserDocTabPage

        If Not panelMain.Visible Then

            ToggleMainVisibility()

            SaveVisibilitySettings()

        End If

        tabControlDocuments.SelectedTab = tabPageMainUml

        Dim doc As UserDocTabPage
        Dim checkDomainMap As IDomainMap

        For Each doc In tabControlUmlDoc.TabPages

            If Not doc.UmlDiagram Is Nothing Then

                checkDomainMap = doc.UmlDiagram.GetDomainMap

                If Not checkDomainMap Is Nothing Then

                    If checkDomainMap Is domainMap Then

                        tabControlUmlDoc.SelectedTab = doc

                        doc.RefreshUml()

                        doc.Focus()

                        Return doc

                    End If

                End If

            End If

        Next


    End Function


#End Region


#Region " Xml Behind "


    Private Sub OpenCodeMapDocument(ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType, ByVal classMap As IClassMap, ByVal codeMap As ICodeMap)

        For Each tabPage As UserDocTabPage In tabControlCodeMapDoc.TabPages

            If Not tabPage.CodeMap Is Nothing Then

                If tabPage.ClassMap Is classMap And tabPage.CodeMap.CodeLanguage = codeMap.CodeLanguage Then

                    tabControlDocuments.SelectedTab = tabPageMainCodeMap

                    tabControlCodeMapDoc.SelectedTab = tabPage

                    SetCodeMapDocumentTitle()

                    Exit Sub

                End If

            End If

        Next

        AddNewCodeMapDocument(tabName, docTitle, docType, classMap, codeMap)

    End Sub

    Private Sub AddNewCodeMapDocument(ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType, ByVal classMap As IClassMap, ByVal codeMap As ICodeMap)

        Dim newTabPage As New UserDocTabPage(Me, tabName, docTitle, docType, classMap, codeMap)

        AddHandler newTabPage.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

        'newTabPage.TextBox.ContextMenu = menuUserDoc

        AddHandler newTabPage.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp

        tabControlCodeMapDoc.TabPages.Add(newTabPage)

        'tabControlDocuments.SelectedIndex = 2
        tabControlDocuments.SelectedTab = tabPageMainCodeMap

        tabControlCodeMapDoc.SelectedIndex = tabControlCodeMapDoc.TabPages.Count - 1

        SetCodeMapDocumentTitle()

    End Sub

    Private Sub OpenCodeMapDocument(ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType, ByVal domainMap As IDomainMap, ByVal codeMap As ICodeMap)

        For Each tabPage As UserDocTabPage In tabControlCodeMapDoc.TabPages

            If Not tabPage.CodeMap Is Nothing Then

                If tabPage.DomainMap Is domainMap And tabPage.CodeMap.CodeLanguage = codeMap.CodeLanguage Then

                    tabControlDocuments.SelectedTab = tabPageMainCodeMap

                    tabControlCodeMapDoc.SelectedTab = tabPage

                    SetCodeMapDocumentTitle()

                    Exit Sub

                End If

            End If

        Next

        AddNewCodeMapDocument(tabName, docTitle, docType, domainMap, codeMap)

    End Sub

    Private Sub AddNewCodeMapDocument(ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType, ByVal domainMap As IDomainMap, ByVal codeMap As ICodeMap)

        Dim newTabPage As New UserDocTabPage(Me, tabName, docTitle, docType, domainMap, codeMap)

        AddHandler newTabPage.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

        'newTabPage.TextBox.ContextMenu = menuUserDoc

        AddHandler newTabPage.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp

        tabControlCodeMapDoc.TabPages.Add(newTabPage)

        'tabControlDocuments.SelectedIndex = 2
        tabControlDocuments.SelectedTab = tabPageMainCodeMap

        tabControlCodeMapDoc.SelectedIndex = tabControlCodeMapDoc.TabPages.Count - 1

        SetCodeMapDocumentTitle()

    End Sub

    'Private Sub ViewCodeMap(ByVal classMap As IClassMap, ByVal codeLanguage)

    '    Dim domainMap As IDomainMap

    '    If Not panelMain.Visible Then

    '        ToggleMainVisibility()

    '        SaveVisibilitySettings()

    '    End If

    '    tabControlDocuments.SelectedTab = tabPageMainXmlBehind

    '    Dim doc As UserDocTabPage

    '    For Each doc In tabControlXmlBehind.TabPages

    '        If doc.DomainMap Is domainMap Then

    '            tabControlXmlBehind.SelectedTab = doc

    '            doc.RefreshXML()

    '            doc.Focus()

    '            Exit Sub

    '        End If

    '    Next

    '    m_NoRefreshXmlBehind = True

    '    doc = New UserDocTabPage(Me, domainMap)

    '    'doc.TextBox.ContextMenu = menuUserDoc

    '    AddHandler doc.UpdatedDomain, AddressOf Me.HandleXmlBehindUpdatedDomain
    '    AddHandler doc.TextBoxTextChanged, AddressOf Me.HandleXmlBehindTextBoxTextChanged

    '    AddHandler doc.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

    '    AddHandler doc.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp


    '    tabControlXmlBehind.TabPages.Add(doc)

    '    tabControlXmlBehind.SelectedTab = doc

    '    doc.Dirty = False

    '    doc.RefreshXML()

    '    doc.Focus()

    '    doc.Dirty = False

    '    SetXmlBehindTitle()

    '    m_NoRefreshXmlBehind = False

    'End Sub


    Private Sub ViewDomainXML(ByVal domainMap As IDomainMap)

        If Not panelMain.Visible Then

            ToggleMainVisibility()

            SaveVisibilitySettings()

        End If

        tabControlDocuments.SelectedTab = tabPageMainXmlBehind

        Dim doc As UserDocTabPage

        For Each doc In tabControlXmlBehind.TabPages

            If doc.DomainMap Is domainMap Then

                tabControlXmlBehind.SelectedTab = doc

                doc.RefreshXML()

                doc.Focus()

                Exit Sub

            End If

        Next

        m_NoRefreshXmlBehind = True

        doc = New UserDocTabPage(Me, domainMap)

        'doc.TextBox.ContextMenu = menuUserDoc

        AddHandler doc.UpdatedDomain, AddressOf Me.HandleXmlBehindUpdatedDomain
        AddHandler doc.TextBoxTextChanged, AddressOf Me.HandleXmlBehindTextBoxTextChanged

        AddHandler doc.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

        AddHandler doc.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp


        tabControlXmlBehind.TabPages.Add(doc)

        tabControlXmlBehind.SelectedTab = doc

        doc.Dirty = False

        doc.RefreshXML()

        doc.Focus()

        doc.Dirty = False

        SetXmlBehindTitle()

        m_NoRefreshXmlBehind = False

    End Sub


    Private Sub RefreshXmlBehind()

        If m_NoRefreshXmlBehind Then Exit Sub

        If Not tabControlDocuments.SelectedTab Is tabPageMainXmlBehind Then Exit Sub

        If Not tabControlXmlBehind.TabPages.Count > 0 Then Exit Sub

        Dim doc As UserDocTabPage = tabControlXmlBehind.SelectedTab

        If Not doc Is Nothing Then

            doc.RefreshXML()

        End If

    End Sub


    Public Sub RefreshUml()

        If m_NoRefreshUmlDoc Then Exit Sub

        If Not tabControlDocuments.SelectedTab Is tabPageMainUml Then Exit Sub

        If Not tabControlUmlDoc.TabPages.Count > 0 Then Exit Sub

        Dim doc As UserDocTabPage = tabControlUmlDoc.SelectedTab

        If Not doc Is Nothing Then

            doc.RefreshUml()

        End If

    End Sub

    Public Sub HandleXmlBehindUpdatedDomain(ByVal domainMap As IDomainMap)

        RefreshAll()

        SetXmlBehindTitle()

    End Sub

    Public Sub HandleXmlBehindTextBoxTextChanged()

        SetXmlBehindTitle()

    End Sub


    Private Function HasOpenXmlBehind(ByVal domainMap As IDomainMap)

        Dim doc As UserDocTabPage

        For Each doc In tabControlXmlBehind.TabPages

            If doc.DomainMap Is domainMap Then

                Return True

            End If

        Next

    End Function


    Private Function HasDirtyXmlBehind(ByVal domainMap As IDomainMap)

        Dim doc As UserDocTabPage

        For Each doc In tabControlXmlBehind.TabPages

            If doc.DomainMap Is domainMap Then

                Return doc.Dirty

            End If

        Next

    End Function

#End Region

#Region " Panels "


    Private Sub panelProperties_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelProperties.Enter

        Label1.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        Label1.ForeColor = Color.FromKnownColor(KnownColor.HighlightText)

        buttonCloseProperties.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseProperties.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseProperties.ImageIndex = 1

        buttonSwapPosProperties.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonSwapPosProperties.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        If buttonSwapPosProperties.ImageIndex = 2 Then
            buttonSwapPosProperties.ImageIndex = 6
        Else
            buttonSwapPosProperties.ImageIndex = 7
        End If

    End Sub

    Private Sub panelProperties_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelProperties.Leave

        Label1.BackColor = Color.FromKnownColor(KnownColor.Control)
        Label1.ForeColor = Color.FromKnownColor(KnownColor.ControlText)

        buttonCloseProperties.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseProperties.ForeColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseProperties.ImageIndex = 0

        buttonSwapPosProperties.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonSwapPosProperties.ForeColor = Color.FromKnownColor(KnownColor.Control)
        If buttonSwapPosProperties.ImageIndex = 6 Then
            buttonSwapPosProperties.ImageIndex = 2
        Else
            buttonSwapPosProperties.ImageIndex = 4
        End If

    End Sub

    Private Sub panelMain_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelMain.Enter

        Label5.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        Label5.ForeColor = Color.FromKnownColor(KnownColor.HighlightText)

        buttonCloseMain.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseMain.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseMain.ImageIndex = 1

        buttonSwapPosMain.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonSwapPosMain.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        If buttonSwapPosMain.ImageIndex = 2 Then
            buttonSwapPosMain.ImageIndex = 6
        Else
            buttonSwapPosMain.ImageIndex = 7
        End If

    End Sub

    Private Sub panelMain_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelMain.Leave

        Label5.BackColor = Color.FromKnownColor(KnownColor.Control)
        Label5.ForeColor = Color.FromKnownColor(KnownColor.ControlText)

        buttonCloseMain.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseMain.ForeColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseMain.ImageIndex = 0

        buttonSwapPosMain.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonSwapPosMain.ForeColor = Color.FromKnownColor(KnownColor.Control)
        If buttonSwapPosMain.ImageIndex = 6 Then
            buttonSwapPosMain.ImageIndex = 2
        Else
            buttonSwapPosMain.ImageIndex = 4
        End If

    End Sub

    Private Sub panelLeft_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelLeft.Enter

        labelExplorer.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        labelExplorer.ForeColor = Color.FromKnownColor(KnownColor.HighlightText)

        buttonCloseExplorer.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseExplorer.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseExplorer.ImageIndex = 1

        buttonSwapPosExplorer.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonSwapPosExplorer.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        If buttonSwapPosExplorer.ImageIndex = 2 Then
            buttonSwapPosExplorer.ImageIndex = 6
        Else
            buttonSwapPosExplorer.ImageIndex = 7
        End If

    End Sub

    Private Sub panelLeft_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelLeft.Leave

        labelExplorer.BackColor = Color.FromKnownColor(KnownColor.Control)
        labelExplorer.ForeColor = Color.FromKnownColor(KnownColor.ControlText)

        buttonCloseExplorer.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseExplorer.ForeColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseExplorer.ImageIndex = 0

        buttonSwapPosExplorer.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonSwapPosExplorer.ForeColor = Color.FromKnownColor(KnownColor.Control)
        If buttonSwapPosExplorer.ImageIndex = 6 Then
            buttonSwapPosExplorer.ImageIndex = 2
        Else
            buttonSwapPosExplorer.ImageIndex = 4
        End If
    End Sub

    Private Sub panelBottom_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelBottom.Enter

        labelListTitle.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        labelListTitle.ForeColor = Color.FromKnownColor(KnownColor.HighlightText)

        buttonCloseList.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseList.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseList.ImageIndex = 1

        buttonSwapPosList.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonSwapPosList.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        If buttonSwapPosList.ImageIndex = 8 Then
            buttonSwapPosList.ImageIndex = 9
        Else
            buttonSwapPosList.ImageIndex = 11
        End If
    End Sub

    Private Sub panelBottom_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelBottom.Leave

        labelListTitle.BackColor = Color.FromKnownColor(KnownColor.Control)
        labelListTitle.ForeColor = Color.FromKnownColor(KnownColor.ControlText)

        buttonCloseList.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseList.ForeColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseList.ImageIndex = 0

        buttonSwapPosList.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonSwapPosList.ForeColor = Color.FromKnownColor(KnownColor.Control)
        If buttonSwapPosList.ImageIndex = 9 Then
            buttonSwapPosList.ImageIndex = 8
        Else
            buttonSwapPosList.ImageIndex = 10
        End If
    End Sub

    Private Sub panelRight_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelRight.Enter

        Label3.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        Label3.ForeColor = Color.FromKnownColor(KnownColor.HighlightText)

        buttonCloseTools.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseTools.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonCloseTools.ImageIndex = 1

        buttonSwapPosTools.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        buttonSwapPosTools.ForeColor = Color.FromKnownColor(KnownColor.Highlight)
        If buttonSwapPosTools.ImageIndex = 2 Then
            buttonSwapPosTools.ImageIndex = 6
        Else
            buttonSwapPosTools.ImageIndex = 7
        End If
    End Sub

    Private Sub panelRight_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelRight.Leave

        Label3.BackColor = Color.FromKnownColor(KnownColor.Control)
        Label3.ForeColor = Color.FromKnownColor(KnownColor.ControlText)

        buttonCloseTools.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseTools.ForeColor = Color.FromKnownColor(KnownColor.Control)
        buttonCloseTools.ImageIndex = 0

        buttonSwapPosTools.BackColor = Color.FromKnownColor(KnownColor.Control)
        buttonSwapPosTools.ForeColor = Color.FromKnownColor(KnownColor.Control)
        If buttonSwapPosTools.ImageIndex = 6 Then
            buttonSwapPosTools.ImageIndex = 2
        Else
            buttonSwapPosTools.ImageIndex = 4
        End If
    End Sub

    Private Sub buttonCloseExplorer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseExplorer.Click

        ToggleProjectExplorerVisibility()

    End Sub

    Private Sub buttonCloseMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseMain.Click

        ToggleMainVisibility()

    End Sub

    Private Sub buttonCloseTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseTools.Click

        ToggleToolsVisibility()

    End Sub

    Private Sub buttonCloseList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseList.Click

        ToggleMsgListVisibility()

    End Sub


    Private Sub buttonCloseProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseProperties.Click

        TogglePropertiesVisibility()

    End Sub


#End Region

#Region " ListView"

    Private Sub SetListParent()

        'It is necessary to make a clean, shallow copy of the node
        'If a straight reference to the original is used (or a deep copy, for that matter)
        'the node object may try to be smart and contact its parnet node and whatnot,
        'which will throw an exception if the node is been removed from the tree
        Dim newNode As mapNode

        Dim mapObject As IMap
        Dim domainMap As IDomainMap

        Dim mapNode As mapNode = mapTreeView.SelectedNode

        mapObject = mapNode.GetMap

        If mapNode.GetType Is GetType(ProjectNode) Then

            newNode = New ProjectNode(mapObject)

        ElseIf mapNode.GetType Is GetType(DomainMapNode) Then

            newNode = New DomainMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(ClassListMapNode) Then

            newNode = New ClassListMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(SourceListMapNode) Then

            newNode = New SourceListMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(ConfigListNode) Then

            newNode = New ConfigListNode(mapObject)

        ElseIf mapNode.GetType Is GetType(NamespaceMapNode) Then

            newNode = New ClassListMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(ClassMapNode) Then

            newNode = New ClassMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(SourceMapNode) Then

            newNode = New SourceMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(TableMapNode) Then

            newNode = New TableMapNode(mapObject)

            'ElseIf mapNode.GetType Is GetType(ColumnMapNode) Then

            '    newNode = New ColumnMapNode(mapObject)

        ElseIf mapNode.GetType Is GetType(ConfigNode) Then

            domainMap = CType(mapNode.Parent, ConfigListNode).GetMap

            newNode = New ConfigNode(mapObject, domainMap)

        ElseIf mapNode.GetType Is GetType(ClassesToCodeConfigNode) Then

            domainMap = CType(mapNode.Parent.Parent, ConfigListNode).GetMap

            newNode = New ClassesToCodeConfigNode(mapObject, domainMap)

        ElseIf mapNode.GetType Is GetType(SourceCodeFileListNode) Then

            domainMap = CType(mapNode.Parent.Parent.Parent, ConfigListNode).GetMap

            newNode = New SourceCodeFileListNode(mapObject, domainMap)

            'ElseIf mapNode.GetType Is GetType(SourceCodeFileNode) Then

            '    domainMap = CType(mapNode.Parent.Parent.Parent.Parent, ConfigListNode).GetMap

            '    newNode = New SourceCodeFileNode(mapObject, domainMap)

        End If

        If Not newNode Is Nothing Then


            mapListView.Go(newNode)

            'tabControlMessages.SelectedIndex = 3

            RefreshToolBar()

        End If

    End Sub

    Private Sub RefreshListView()

        mapListView.RefreshItems()

    End Sub

    Private Sub mapListView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mapListView.Click

        If m_Updating Then Exit Sub
        Dim domainMap As IDomainMap

        Dim mapObject As IMap

        Dim mapItem As MapListItem

        For Each mapItem In mapListView.SelectedItems

            mapObject = mapItem.GetMap

            SelectPropertiesForMapObject(mapObject)
            'mapPropertyGrid.SelectMapObject(mapObject, m_Project)

            Exit For

        Next

        RefreshToolBar()

    End Sub

    Private Sub mapListView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles mapListView.DoubleClick


        If m_Updating Then Exit Sub

        Dim newNode As MapNode

        Dim mapObject As IMap

        Dim mapItem As MapListItem

        For Each mapItem In mapListView.SelectedItems

            mapObject = mapItem.GetMap

            If mapItem.GetType Is GetType(DomainMapItem) Then

                newNode = New DomainMapNode(mapObject)

            ElseIf mapItem.GetType Is GetType(ClassListMapItem) Then

                newNode = New ClassListMapNode(mapObject)

            ElseIf mapItem.GetType Is GetType(SourceListMapItem) Then

                newNode = New SourceListMapNode(mapObject)

            ElseIf mapItem.GetType Is GetType(ConfigListItem) Then

                newNode = New ConfigListNode(mapObject)

            ElseIf mapItem.GetType Is GetType(ClassMapItem) Then

                newNode = New ClassMapNode(mapObject)

            ElseIf mapItem.GetType Is GetType(SourceMapItem) Then

                newNode = New SourceMapNode(mapObject)

            ElseIf mapItem.GetType Is GetType(TableMapItem) Then

                newNode = New TableMapNode(mapObject)

                'ElseIf mapItem.GetType Is GetType(ColumnMapItem) Then

                '    newNode = New ColumnMapNode(mapObject)

            ElseIf mapItem.GetType Is GetType(ConfigItem) Then

                newNode = New ConfigNode(mapObject, CType(mapItem, ConfigItem).DomainMap)

            ElseIf mapItem.GetType Is GetType(ClassesToCodeConfigItem) Then

                newNode = New ClassesToCodeConfigNode(mapObject, CType(mapItem, ClassesToCodeConfigItem).DomainMap)

            ElseIf mapItem.GetType Is GetType(SourceCodeFileListItem) Then

                newNode = New SourceCodeFileListNode(mapObject, CType(mapItem, SourceCodeFileListItem).DomainMap)

                'ElseIf mapItem.GetType Is GetType(SourceCodeFileItem) Then

                '    newNode = New SourceCodeFileNode(mapObject, CType(mapItem, SourceCodeFileItem).DomainMap)

            End If


            If Not newNode Is Nothing Then


                mapListView.Go(newNode)


            End If

            Exit For

        Next

        RefreshToolBar()

    End Sub

    Public Sub GoBack()

        mapListView.GoBack()

        RefreshToolBar()

    End Sub

    Public Sub GoForward()

        mapListView.GoForward()

        RefreshToolBar()

    End Sub

    Public Sub GoUp()

        mapListView.GoUp()

        RefreshToolBar()

    End Sub

#End Region

#Region " Logging "

    Public MsgsPaneLogLevel As TraceLevel = TraceLevel.Info

    Public Overloads Sub LogMsg(ByVal msg As String, ByVal logLevel As TraceLevel)

        LogMsg(msg, logLevel, "")

    End Sub

    Public Overloads Sub LogMsg(ByVal msg As String, ByVal logLevel As TraceLevel, ByVal source As String)


        LogMsgToMsgsPane(msg, logLevel, source)


    End Sub

    Public Sub LogMsgToMsgsPane(ByVal msg As String, ByVal logLevel As TraceLevel, ByVal source As String)

        Dim showPane As Boolean

        If logLevel >= MsgsPaneLogLevel Then

            Select Case logLevel

                Case TraceLevel.Error, TraceLevel.Warning

                    showPane = True

            End Select

            AddMsg(msg, showPane)

        End If

    End Sub

#End Region

#Region " Unsorted Stuff (Or should I say 'Assorted'? Does that sound more mitigating?) "

    Private Sub RecentProjectClickHandler(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim path As String = CType(sender, MenuItem).Text

        If Len(path) > 0 Then

            OpenProject(path)

        End If

    End Sub

    Private Sub buttonSwapPosProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSwapPosProperties.Click

        TogglePropertiesPos()

        mapPropertyGrid.Focus()

    End Sub


    Private Sub buttonSwapPosExplorer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSwapPosExplorer.Click

        ToggleExplorerPos()

        mapTreeView.Focus()

    End Sub

    Private Sub buttonSwapPosMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSwapPosMain.Click

        ToggleMainToolsPos()

        tabControlDocuments.Focus()

    End Sub

    Private Sub buttonSwapPosTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSwapPosTools.Click

        ToggleMainToolsPos()

        tabControlTools.Focus()

    End Sub

    Private Sub buttonSwapPosList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSwapPosList.Click

        ToggleListPos()

        tabControlMessages.Focus()

    End Sub

    Private Sub ToggleExplorerPos()

        If panelLeft.Dock = DockStyle.Left Then

            panelLeft.Dock = DockStyle.Right
            Splitter1.Dock = DockStyle.Right

            If buttonSwapPosExplorer.ImageIndex = 7 Then
                buttonSwapPosExplorer.ImageIndex = 6
            Else
                buttonSwapPosExplorer.ImageIndex = 2
            End If
        Else

            panelLeft.Dock = DockStyle.Left
            Splitter1.Dock = DockStyle.Left

            If buttonSwapPosExplorer.ImageIndex = 6 Then
                buttonSwapPosExplorer.ImageIndex = 7
            Else
                buttonSwapPosExplorer.ImageIndex = 4
            End If

        End If

        SaveVisibilitySettings()

    End Sub


    Private Sub TogglePropertiesPos()

        If panelProperties.Dock = DockStyle.Left Then

            panelProperties.Dock = DockStyle.Right
            Splitter2.Dock = DockStyle.Right

            If buttonSwapPosProperties.ImageIndex = 7 Then
                buttonSwapPosProperties.ImageIndex = 6
            Else
                buttonSwapPosProperties.ImageIndex = 2
            End If
        Else

            panelProperties.Dock = DockStyle.Left
            Splitter2.Dock = DockStyle.Left

            If buttonSwapPosProperties.ImageIndex = 6 Then
                buttonSwapPosProperties.ImageIndex = 7
            Else
                buttonSwapPosProperties.ImageIndex = 4
            End If

        End If

        SaveVisibilitySettings()

    End Sub

    Private Sub ToggleMainToolsPos()

        If panelRight.Dock = DockStyle.Left Then

            panelRight.Dock = DockStyle.Right
            Splitter5.Dock = DockStyle.Right

            If buttonSwapPosTools.ImageIndex = 7 Then
                buttonSwapPosTools.ImageIndex = 6
            Else
                buttonSwapPosTools.ImageIndex = 2
            End If

            If buttonSwapPosMain.ImageIndex = 6 Then
                buttonSwapPosMain.ImageIndex = 7
            Else
                buttonSwapPosMain.ImageIndex = 4
            End If
        Else

            panelRight.Dock = DockStyle.Left
            Splitter5.Dock = DockStyle.Left

            If buttonSwapPosTools.ImageIndex = 6 Then
                buttonSwapPosTools.ImageIndex = 7
            Else
                buttonSwapPosTools.ImageIndex = 4
            End If

            If buttonSwapPosMain.ImageIndex = 7 Then
                buttonSwapPosMain.ImageIndex = 6
            Else
                buttonSwapPosMain.ImageIndex = 2
            End If
        End If

        SaveVisibilitySettings()

    End Sub


    Private Sub ToggleListPos()

        If panelBottom.Dock = DockStyle.Bottom Then

            panelBottom.Dock = DockStyle.Top
            Splitter4.Dock = DockStyle.Top

            If buttonSwapPosList.ImageIndex = 11 Then
                buttonSwapPosList.ImageIndex = 9
            Else
                buttonSwapPosList.ImageIndex = 8
            End If

        Else

            panelBottom.Dock = DockStyle.Bottom
            Splitter4.Dock = DockStyle.Bottom

            If buttonSwapPosList.ImageIndex = 9 Then
                buttonSwapPosList.ImageIndex = 11
            Else
                buttonSwapPosList.ImageIndex = 10
            End If

        End If

        SaveVisibilitySettings()

    End Sub


    Private Sub menuUserDocUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocUndo.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        txt.Undo()

    End Sub


    Private Sub menuUserDocRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocRedo.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        txt.Redo()

    End Sub

    Private Sub menuUserDocCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocCut.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        Clipboard.SetDataObject(txt.Selection.Text, False)

        txt.Selection.Text = ""

    End Sub

    Private Sub menuUserDocCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocCopy.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        Clipboard.SetDataObject(txt.Selection.Text, True)

    End Sub

    Private Sub menuUserDocPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocPaste.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        txt.Selection.Text = Clipboard.GetDataObject.GetData(GetType(String))

    End Sub

    Private Sub menuUserDocSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocSelectAll.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        txt.SelectAll()

    End Sub

    Private Sub menuUserDocDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUserDocDelete.Click

        Dim txt As SyntaxBoxControl = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, SyntaxBoxControl)

        txt.Selection.Text = ""

    End Sub


    Private Function CloseAllDocs(Optional ByVal noSave As Boolean = False) As Boolean

        'Huh????
        CloseUserDoc()

        While tabControlPreviewDoc.TabPages.Count > 0

            If Not ClosePreviewDoc(noSave) Then

                Return False

            End If

        End While


        While tabControlUserDoc.TabPages.Count > 0

            If Not CloseUserDoc(Nothing, noSave) Then

                Return False

            End If

        End While

        While tabControlXmlBehind.TabPages.Count > 0

            If Not CloseXmlBehind(noSave) Then

                Return False

            End If

        End While


        While tabControlUmlDoc.TabPages.Count > 0

            If Not CloseUmlDoc() Then

                Return False

            End If

        End While


        Return True

    End Function


    'Private Sub AddNewUmlDoc(ByVal umlDiagram As UmlDiagram, ByVal tabName As String, ByVal docTitle As String, ByVal docType As MainDocumentType)

    '    Dim newTabPage As New UserDocTabPage(tabName, docTitle, umlDiagram, docType)

    '    'AddHandler newTabPage.TextBoxEnter, AddressOf Me.HandleUserDocTextBoxEnter

    '    'newTabPage.TextBox.ContextMenu = menuUserDoc

    '    'AddHandler newTabPage.TextBoxMouseUp, AddressOf Me.HandleUserDocTextBoxMouseUp

    '    tabControlUmlDoc.TabPages.Add(newTabPage)

    '    tabControlDocuments.SelectedIndex = 4

    '    tabControlPreviewDoc.SelectedIndex = tabControlPreviewDoc.TabPages.Count - 1

    '    SetUmlDocumentTitle()

    'End Sub

    Private Sub menuUmlView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlView.Click

        OpenUmlDoc()

    End Sub

    Public Sub SelectTreeNodeForMapObject(ByVal mapObject As IMap, Optional ByVal lineStart As Boolean = False, Optional ByVal lineEnd As Boolean = False)

        If mapObject Is Nothing Then Exit Sub

        Dim map As IMap = mapObject

        Dim mapType As Type = CType(mapObject, Object).GetType()

        If mapType Is GetType(UmlClass) Then

            map = CType(mapObject, UmlClass).GetClassMap

        ElseIf mapType Is GetType(UmlLine) Then

            If lineStart Then

                map = CType(mapObject, UmlLine).GetEndPropertyMap

            ElseIf lineEnd Then

                map = CType(mapObject, UmlLine).GetStartPropertyMap

            End If

        End If

        Dim mapNode As mapNode = mapTreeView.ExpandToMapObject(map)

        If Not mapNode Is Nothing Then

            mapTreeView.SelectedNode = mapNode

        End If

        If Not mapObject Is map Then

            SelectPropertiesForMapObject(mapObject, lineStart, lineEnd)

        End If


    End Sub

    Public Sub SelectPropertiesForMapObject(ByVal mapObject As IMap, Optional ByVal lineStart As Boolean = False, Optional ByVal lineEnd As Boolean = False)

        mapPropertyGrid.SelectMapObject(mapObject, m_Project, lineStart)

    End Sub

    Private Sub frmDomainMapBrowser_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            doc.m_Panel_MouseWheel(sender, e)

            doc.RefreshUml()

            m_ProjectDirty = True

        End If

    End Sub

    Private Sub menuClassUmlAddToCurr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassUmlAddToCurr.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            AddClassToUmlDoc(m_contextMenuMapObject, Point.Empty, doc)

        End If

    End Sub

    Private Sub menuClassUmlAddToNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassUmlAddToNew.Click

        AddClassToUmlDoc(m_contextMenuMapObject, Point.Empty)

    End Sub

    Private Sub menuClassUmlShowInherit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            AddClassInheritanceLineToUmlDoc(m_contextMenuMapObject, doc)

        End If

    End Sub


    Private Sub menuPropertyUmlShowPropLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyUmlShowPropLine.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            AddPropertyAssociationLineToUmlDoc(m_contextMenuMapObject, doc)

        End If

    End Sub


    'OBS - is now show all lines
    Private Sub menuClassUmlShowAssocLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassUmlShowAssocLines.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            AddClassInheritanceLineToUmlDoc(m_contextMenuMapObject, doc, True)

            AddClassAssociationLinesToUmlDoc(m_contextMenuMapObject, doc)

        End If

    End Sub

    Private Sub menuClassUmlRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassUmlRemove.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            RemoveClassFromUmlDoc(m_contextMenuMapObject, doc)

        End If

    End Sub


    Private Sub menuPropertyUmlRemovePropLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuPropertyUmlRemovePropLine.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            RemovePropertyAssociationLineFromUmlDoc(m_contextMenuMapObject, doc)

        End If

    End Sub

    Private Sub menuClassUmlRemoveAllLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassUmlRemoveAllLines.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            RemoveClassLinesFromUmlDoc(m_contextMenuMapObject, doc, True)

        End If

    End Sub


    Private Sub menuUmlAddLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlAddLines.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            AddDiagramLinesToUmlDoc(m_contextMenuMapObject, doc, True)

        End If

    End Sub




    Private Sub menuUmlRemoveLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlRemoveLines.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            RemoveDiagramLinesFromUmlDoc(m_contextMenuMapObject, doc, True)

        End If

    End Sub








    Public Function AddClassToUmlDoc(ByVal classMap As IClassMap, ByVal pos As Point, Optional ByVal doc As UserDocTabPage = Nothing, Optional ByVal silent As Boolean = False) As UmlClass

        Dim diagram As UmlDiagram
        Dim umlClass As umlClass
        Dim checkClassMap As IClassMap

        Dim x As Integer
        Dim y As Integer

        Dim domainMap As IDomainMap

        If doc Is Nothing Then

            diagram = AddNewDiagram(classMap.DomainMap)

            doc = ViewUmlDiagram(diagram)

        Else

            diagram = doc.UmlDiagram

        End If

        If Not diagram Is Nothing Then

            'Check that the class belongs to the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Function

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The class and the diagram belong to different domain models! The class '" & classMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Function

            End If

            'Check that class is not already represented in diagram
            If doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is already represented in the diagram '" & diagram.Name & "!")

                Exit Function

            End If

            diagram.DeselectAll()

            umlClass = New umlClass

            If pos.Equals(Point.Empty) Then

                x = CInt((diagram.Location.X + (doc.Panel.Width / 2)) / diagram.Zoom)
                y = CInt((diagram.Location.Y + (doc.Panel.Height / 2)) / diagram.Zoom)

            Else

                x = CInt((diagram.Location.X + pos.X) / diagram.Zoom)
                y = CInt((diagram.Location.Y + pos.Y) / diagram.Zoom)

            End If

            umlClass.Location = New Point(x, y)

            umlClass.Name = classMap.Name
            umlClass.Selected = True

            umlClass.UmlDiagram = diagram

            doc.RefreshUml()

            m_ProjectDirty = True

            Return umlClass

        End If

    End Function


    Public Sub AddClassInheritanceLineToUmlDoc(ByVal classMap As IClassMap, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)

        Dim diagram As UmlDiagram = doc.UmlDiagram
        Dim umlClass As umlClass
        Dim umlSubClass As umlClass
        Dim umlSuperClass As umlClass
        Dim umlLine As umlLine
        Dim superClassMap As IClassMap
        Dim checkClassMap As IClassMap
        Dim checkStartClassMap As IClassMap
        Dim checkEndClassMap As IClassMap

        Dim domainMap As IDomainMap

        Dim found As Boolean

        If Not diagram Is Nothing Then

            'Check that the class belongs to the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The class and the diagram belong to different domain models! The class '" & classMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Sub

            End If

            'Check that the class inherits 
            If Not Len(classMap.InheritsClass) > 0 Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' that class '" & classMap.Name & "' inherits from could not be found!")

                Exit Sub

            End If

            'Check that the inherited class exists
            superClassMap = classMap.GetInheritedClassMap

            If superClassMap Is Nothing Then

                If Not silent Then MsgBox("The class '" & classMap.InheritsClass & "' that the class '" & classMap.Name & "' inherits from could not be found!")

                Exit Sub

            End If

            'Check that the class is already represented in diagram
            If Not doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            'Check that the super class is already represented in diagram
            If Not doc.DisplaysClassMap(superClassMap) Then

                If Not silent Then MsgBox("The class '" & superClassMap.Name & "' that the class '" & classMap.Name & "' inherits from is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            'Check that inheritance is not already represented in diagram
            If doc.DisplaysClassMapGeneralization(classMap) Then

                If Not silent Then MsgBox("The generalization  '" & classMap.Name & " inherits from " & superClassMap.Name & "' is already represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            umlSubClass = doc.GetUmlClassForClassMap(classMap)
            umlSuperClass = doc.GetUmlClassForClassMap(superClassMap)

            If Not (umlSubClass Is Nothing Or umlSuperClass Is Nothing) Then

                diagram.DeselectAll()

                umlLine = New umlLine

                umlLine.LineType = LineTypeEnum.Generalization

                umlLine.StartClass = umlSubClass.Name
                umlLine.EndClass = umlSuperClass.Name

                umlLine.StartSelected = True
                umlLine.EndSelected = True

                umlLine.UmlDiagram = diagram

                doc.RefreshUml()

                m_ProjectDirty = True

            End If

        End If

    End Sub



    Public Sub AddPropertyAssociationLineToUmlDoc(ByVal propertyMap As IPropertyMap, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)

        Dim diagram As UmlDiagram = doc.UmlDiagram
        Dim umlClass As umlClass
        Dim umlStartClass As umlClass
        Dim umlRefClass As umlClass
        Dim umlLine As umlLine
        Dim checkClassMap As IClassMap
        Dim checkStartClassMap As IClassMap
        Dim checkEndClassMap As IClassMap
        Dim checkPropertyMap As IPropertyMap
        Dim classMap As IClassMap = propertyMap.ClassMap
        Dim refClassMap As IClassMap = propertyMap.GetReferencedClassMap
        Dim invPropertyMap As IPropertyMap = propertyMap.GetInversePropertyMap

        Dim found As Boolean

        Dim loc As Point
        Dim sz As Size

        Dim domainMap As IDomainMap

        Dim umlLinePoint As umlLinePoint

        If Not diagram Is Nothing Then

            'Check that the property belongs to 
            'the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The property and the diagram belong to different domain models! The property '" & classMap.Name & "." & propertyMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Sub

            End If


            'Check that the property is a reference property
            If propertyMap.ReferenceType = ReferenceType.None Then

                If Not silent Then MsgBox("The property '" & classMap.Name & "." & propertyMap.Name & "' is not a reference property and can't be represented in the diagram with a line association!")

                Exit Sub

            End If

            'Check that the referenced class exists
            If refClassMap Is Nothing Then

                If Not silent Then MsgBox("The class '" & propertyMap.GetDataOrItemType & "' that the property '" & classMap.Name & "." & propertyMap.Name & "' references could not be found!")

                Exit Sub

            End If

            'Check that the class is already represented in diagram
            If Not doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            'Check that the referenced class is already represented in diagram
            If Not doc.DisplaysClassMap(refClassMap) Then

                If Not silent Then MsgBox("The class '" & refClassMap.Name & "' that is referenced by the property '" & classMap.Name & "." & propertyMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If


            'Check that the association is not already represented in diagram
            If doc.DisplaysPropertyMap(propertyMap) Then

                If Not silent Then MsgBox("The association '" & classMap.Name & "." & propertyMap.Name & " references " & refClassMap.Name & "' is already represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            umlStartClass = doc.GetUmlClassForClassMap(classMap)
            umlRefClass = doc.GetUmlClassForClassMap(refClassMap)

            If Not (umlStartClass Is Nothing Or umlRefClass Is Nothing) Then

                diagram.DeselectAll()

                umlLine = New umlLine

                umlLine.LineType = LineTypeEnum.Association

                umlLine.StartClass = umlStartClass.Name
                umlLine.EndClass = umlRefClass.Name

                umlLine.StartProperty = propertyMap.Name

                If Not invPropertyMap Is Nothing Then

                    umlLine.EndProperty = invPropertyMap.Name

                End If

                umlLine.StartSelected = True
                umlLine.EndSelected = True

                umlLine.UmlDiagram = diagram

                If umlStartClass Is umlRefClass Then

                    loc = umlStartClass.Location
                    sz = umlStartClass.Size

                    umlLinePoint = New umlLinePoint

                    umlLinePoint.Selected = True

                    umlLinePoint.X = loc.X + sz.Width + 100
                    umlLinePoint.Y = loc.Y + (sz.Height / 2)

                    umlLinePoint.UmlLine = umlLine


                    umlLinePoint = New umlLinePoint

                    umlLinePoint.Selected = True

                    umlLinePoint.X = loc.X + sz.Width + 100
                    umlLinePoint.Y = loc.Y + sz.Height + 100

                    umlLinePoint.UmlLine = umlLine



                    umlLinePoint = New umlLinePoint

                    umlLinePoint.Selected = True

                    umlLinePoint.X = loc.X + (sz.Width / 2)
                    umlLinePoint.Y = loc.Y + sz.Height + 100

                    umlLinePoint.UmlLine = umlLine


                End If

                m_ProjectDirty = True

                doc.RefreshUml()

            End If

        End If

    End Sub


    Public Sub RemoveClassFromUmlDoc(ByVal classMap As IClassMap, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)

        Dim diagram As UmlDiagram
        Dim umlClass As umlClass
        Dim theUmlClass As umlClass
        Dim checkClassMap As IClassMap


        Dim found As Boolean

        Dim domainMap As IDomainMap

        diagram = doc.UmlDiagram

        If Not diagram Is Nothing Then

            'Check that the class belongs to the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The class and the diagram belong to different domain models! The class '" & classMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Sub

            End If

            'Check that the class is represented in the diagram
            If Not doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            theUmlClass = doc.GetUmlClassForClassMap(classMap)

            If Not theUmlClass Is Nothing Then

                theUmlClass.Remove()

            End If

            m_ProjectDirty = True

            doc.RefreshUml()

        End If

    End Sub



    Public Sub AddClassAssociationLinesToUmlDoc(ByVal classMap As IClassMap, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)

        Dim diagram As UmlDiagram = doc.UmlDiagram
        Dim umlClass As umlClass
        Dim checkClassMap As IClassMap
        Dim propertyMap As IPropertyMap

        Dim found As Boolean

        Dim domainMap As IDomainMap

        If Not diagram Is Nothing Then

            'Check that the class belongs to 
            'the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The property and the diagram belong to different domain models! The property '" & classMap.Name & "." & propertyMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Sub

            End If

            'Check that the class is already represented in diagram
            If Not doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            'obs! we need to clone the list because the
            'drawing area might become updated
            For Each propertyMap In classMap.PropertyMaps.Clone

                If Not propertyMap.ReferenceType = ReferenceType.None Then

                    AddPropertyAssociationLineToUmlDoc(propertyMap, doc, True)

                End If

            Next

            doc.RefreshUml()

            m_ProjectDirty = True

        End If

    End Sub



    Public Sub RemoveClassLinesFromUmlDoc(ByVal classMap As IClassMap, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)


        Dim diagram As UmlDiagram
        Dim umlClass As umlClass
        Dim theUmlClass As umlClass
        Dim checkClassMap As IClassMap


        Dim found As Boolean

        Dim domainMap As IDomainMap

        diagram = doc.UmlDiagram

        If Not diagram Is Nothing Then

            'Check that the class belongs to the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The class and the diagram belong to different domain models! The class '" & classMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Sub

            End If

            'Check that the class is represented in the diagram
            If Not doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            theUmlClass = doc.GetUmlClassForClassMap(classMap)

            If Not theUmlClass Is Nothing Then

                theUmlClass.RemoveLines()

            End If

            doc.RefreshUml()

            m_ProjectDirty = True

        End If


    End Sub



    Public Sub RemovePropertyAssociationLineFromUmlDoc(ByVal propertyMap As IPropertyMap, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)

        Dim diagram As UmlDiagram = doc.UmlDiagram
        Dim umlClass As umlClass
        Dim umlStartClass As umlClass
        Dim umlRefClass As umlClass
        Dim umlLine As umlLine
        Dim checkClassMap As IClassMap
        Dim checkStartClassMap As IClassMap
        Dim checkEndClassMap As IClassMap
        Dim checkPropertyMap As IPropertyMap
        Dim classMap As IClassMap = propertyMap.ClassMap
        Dim refClassMap As IClassMap = propertyMap.GetReferencedClassMap
        Dim invPropertyMap As IPropertyMap = propertyMap.GetInversePropertyMap

        Dim found As Boolean

        Dim domainMap As IDomainMap

        If Not diagram Is Nothing Then

            'Check that the property belongs to 
            'the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            If Not domainMap Is classMap.DomainMap Then

                If Not silent Then MsgBox("The property and the diagram belong to different domain models! The property '" & classMap.Name & "." & propertyMap.Name & "' belongs to the domain model '" & classMap.DomainMap.Name & "' and the diagram '" & diagram.Name & " belongs to the domain model '" & domainMap.Name & "'")

                Exit Sub

            End If


            'Check that the property is a reference property
            If propertyMap.ReferenceType = ReferenceType.None Then

                If Not silent Then MsgBox("The property '" & classMap.Name & "." & propertyMap.Name & "' is not a reference property and can't be represented in the diagram with a line association!")

                Exit Sub

            End If

            'Check that the class is already represented in diagram
            If Not doc.DisplaysClassMap(classMap) Then

                If Not silent Then MsgBox("The class '" & classMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If


            'Check that the association is already represented in diagram
            If doc.DisplaysPropertyMap(propertyMap) Then

                If Not silent Then MsgBox("The association '" & classMap.Name & "." & propertyMap.Name & " references " & refClassMap.Name & "' is not represented in the diagram '" & diagram.Name & "!")

                Exit Sub

            End If

            umlLine = doc.GetUmlLineForPropertyMap(propertyMap)

            If Not umlLine Is Nothing Then

                diagram.UmlLines.Remove(umlLine)

            End If

            doc.RefreshUml()

            m_ProjectDirty = True

        End If

    End Sub

    Public Sub AddDiagramLinesToUmlDoc(ByVal diagram As UmlDiagram, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)

        Dim domainMap As IDomainMap
        Dim classMap As IClassMap

        If Not diagram Is Nothing Then

            'Check that the property belongs to 
            'the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            For Each classMap In domainMap.ClassMaps

                If doc.DisplaysClassMap(classMap) Then

                    AddClassInheritanceLineToUmlDoc(classMap, doc, True)

                    AddClassAssociationLinesToUmlDoc(classMap, doc, True)

                End If

            Next

            doc.RefreshUml()

            m_ProjectDirty = True

        End If

    End Sub

    Public Sub RemoveDiagramLinesFromUmlDoc(ByVal diagram As UmlDiagram, ByVal doc As UserDocTabPage, Optional ByVal silent As Boolean = False)


        Dim domainMap As IDomainMap
        Dim classMap As IClassMap

        If Not diagram Is Nothing Then

            'Check that the property belongs to 
            'the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            For Each classMap In domainMap.ClassMaps

                If doc.DisplaysClassMap(classMap) Then

                    RemoveClassLinesFromUmlDoc(classMap, doc, True)

                End If

            Next

            doc.RefreshUml()

            m_ProjectDirty = True

        End If

    End Sub


    Public Sub AddAllClassesToUmlDoc(ByVal domainMap As IDomainMap, ByVal pos As Point, Optional ByVal doc As UserDocTabPage = Nothing, Optional ByVal silent As Boolean = False)

        AddClassesToUmlDoc(domainMap, domainMap.ClassMaps, pos, doc, silent)

    End Sub

    Public Sub AddClassesToUmlDoc(ByVal domainMap As IDomainMap, ByVal classMaps As ArrayList, ByVal pos As Point, Optional ByVal doc As UserDocTabPage = Nothing, Optional ByVal silent As Boolean = False)

        Dim diagram As UmlDiagram
        Dim classMap As IClassMap
        Dim umlClass As umlClass

        Dim x As Integer
        Dim y As Integer

        Dim cnt As Integer
        Dim max As Integer
        Dim maxHeight As Integer

        Dim offsetX As Integer
        Dim offsetY As Integer

        Dim margin As Integer = 100

        If doc Is Nothing Then

            diagram = AddNewDiagram(domainMap)

            doc = ViewUmlDiagram(diagram)

        Else

            diagram = doc.UmlDiagram

        End If

        If Not diagram Is Nothing Then

            'Check that the class belongs to the same domain as the diagram
            domainMap = diagram.GetDomainMap

            If domainMap Is Nothing Then

                If Not silent Then MsgBox("Ërror! The diagram '" & diagram.Name & "' is not connected to any domain model!")

                Exit Sub

            End If

            diagram.DeselectAll()

            If pos.Equals(Point.Empty) Then

                'x = CInt((diagram.Location.X + (doc.Panel.Width / 2)) / diagram.Zoom)
                'y = CInt((diagram.Location.Y + (doc.Panel.Height / 2)) / diagram.Zoom)

                x = CInt((diagram.Location.X + margin) / diagram.Zoom)
                y = CInt((diagram.Location.Y + margin) / diagram.Zoom)

            Else

                x = CInt((diagram.Location.X + pos.X) / diagram.Zoom)
                y = CInt((diagram.Location.Y + pos.Y) / diagram.Zoom)

            End If

            max = CInt(Math.Sqrt(classMaps.Count) + 0.5)

            For Each classMap In classMaps

                umlClass = AddClassToUmlDoc(classMap, New Point(x + offsetX, y + offsetY), doc, True)

                offsetX += umlClass.Size.Width + margin

                If umlClass.Size.Height > maxHeight Then

                    maxHeight = umlClass.Size.Height

                End If

                cnt += 1

                If cnt >= max Then

                    offsetX = 0

                    offsetY += maxHeight + margin

                    maxHeight = 0

                    cnt = 0

                End If

            Next

            doc.RefreshUml()

            m_ProjectDirty = True

        End If

    End Sub












    Private Sub menuUmlDiagram_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlDiagram.Popup

        menuDiagramUml_HandlePopup()

    End Sub

    Private Sub menuUmlDeleteDiagram_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlDeleteDiagram.Click

        DeleteUmlDiagram(m_contextMenuMapObject)

    End Sub


    Private Sub menuUmSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmSelectAll.Click

        Dim diagram As UmlDiagram = m_contextMenuMapObject

        diagram.SelectAll()

        RefreshAll()

    End Sub




    Private Sub menuUmlProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuUmlAddClassNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlAddClassNew.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            AddNewClassToDiagram(m_contextMenuMapObject, doc)

        End If

    End Sub


    Private Sub menuUmlLineEndRemoveLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlLineEndRemoveLine.Click

        Dim umlLine As umlLine = m_contextMenuMapObject

        umlLine.UmlDiagram.UmlLines.Remove(umlLine)

        RefreshAll()

        m_ProjectDirty = True

    End Sub

    Private Sub menuUmlLineEndLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlLineEndLock.Click

        Dim umlLine As umlLine = m_contextMenuMapObject

        If m_contextMenuIsStart Then

            umlLine.FixedStart = True

        Else

            umlLine.FixedEnd = True

        End If

        RefreshAll()

        m_ProjectDirty = True

    End Sub

    Private Sub menuUmlLineEndUnLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlLineEndUnLock.Click

        Dim umlLine As umlLine = m_contextMenuMapObject

        If m_contextMenuIsStart Then

            umlLine.FixedStart = False

        Else

            umlLine.FixedEnd = False

        End If

        RefreshAll()

        m_ProjectDirty = True

    End Sub

    Private Sub menuUmlLineEndAddPoint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlLineEndAddPoint.Click

        Dim umlLine As umlLine = m_contextMenuMapObject

        umlLine.UmlDiagram.DeselectAll()

        umlLine.AddUmlLinePoint(m_contextMenuIsStart)

        RefreshAll()

    End Sub

    Private Sub menuUmlLineEndProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlLineEndProperties.Click

        OpenProperties()

    End Sub





    Private Sub menuUmlAddClassExistingAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlAddClassExistingAll.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            Try

                AddAllClassesToUmlDoc(CType(m_contextMenuMapObject, UmlDiagram).GetDomainMap, Point.Empty, doc, True)

            Catch ex As Exception

            End Try

        End If

    End Sub

    Private Sub menuUmlAddClassExistingAllWithLines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlAddClassExistingAllWithLines.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            Try

                AddAllClassesToUmlDoc(CType(m_contextMenuMapObject, UmlDiagram).GetDomainMap, Point.Empty, doc, True)

                AddDiagramLinesToUmlDoc(m_contextMenuMapObject, doc, True)

            Catch ex As Exception

            End Try

        End If

    End Sub



    Private Sub menuUmlAddClassExistingSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlAddClassExistingSelect.Click

    End Sub


    Private Sub menuUmlAddClassExistingClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            Try

                Dim domainMap As IDomainMap = CType(m_contextMenuMapObject, UmlDiagram).GetDomainMap

                Dim classMap As IClassMap = domainMap.GetClassMap(CType(sender, MenuItem).Text)

                AddClassToUmlDoc(classMap, Point.Empty, doc, True)

            Catch ex As Exception

            End Try

        End If

    End Sub



    Private Sub menuUmlFit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlFit.Click

        Dim doc As UserDocTabPage = GetCurrentUmlDoc()

        If Not doc Is Nothing Then

            FitUmlDiagram(doc)

        End If

    End Sub

    Private Sub FitUmlDiagram(ByVal doc As UserDocTabPage)

        If doc Is Nothing Then Exit Sub

        doc.FitUmlDiagram()

        m_ProjectDirty = True

    End Sub

    Private Sub tabControlTools_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tabControlTools.DragEnter

    End Sub

    Private Sub tabControlTools_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tabControlTools.DragDrop

        Dim data As String
        Dim dataType As String

        Dim arr() As String
        Dim arrFiles() As String

        Dim ok As Boolean

        Dim sourceDomainMap As IDomainMap
        Dim sourceObject As Object

        Dim key As String

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim umlDiagram As umlDiagram
        Dim DoHilite As Boolean

        e.Effect = DragDropEffects.None

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist"

                                ok = False

                                Exit For

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then
                        e.Effect = DragDropEffects.Copy
                    End If

                    Exit Sub

                End If

            End If

            dataType = LCase(GetDropDataType(data))

            arr = Split(data, "|")

            key = arr(1)

            sourceDomainMap = m_Project.GetDomainMap(arr(2))

            If dataType = "umldiagram" Then

                umlDiagram = GetMapObjectFromKey(sourceDomainMap, key, GetType(umlDiagram))

                ViewUmlDiagram(umlDiagram)

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub tabControlTools_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabControlTools.DragLeave

    End Sub

    Private Sub tabControlTools_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tabControlTools.DragOver

        Dim data As String
        Dim dataType As String

        Dim arr() As String
        Dim arrFiles() As String

        Dim ok As Boolean

        Dim sourceDomainMap As IDomainMap
        Dim sourceObject As Object

        Dim key As String

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim DoHilite As Boolean

        e.Effect = DragDropEffects.None

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist"

                                ok = False

                                Exit For

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then
                        e.Effect = DragDropEffects.Copy
                    End If

                    Exit Sub

                End If

            End If

            dataType = LCase(GetDropDataType(data))

            arr = Split(data, "|")

            key = arr(1)

            sourceDomainMap = m_Project.GetDomainMap(arr(2))

            Select Case dataType

                Case "umldiagram"

                    DoHilite = True

            End Select

            If DoHilite Then
                e.Effect = DragDropEffects.Copy
            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub menuUmlLinePointAddPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlLinePointAddPoint.Click

        Dim umlLinePoint As umlLinePoint = m_contextMenuMapObject

        umlLinePoint.UmlLine.UmlDiagram.DeselectAll()

        umlLinePoint.AddUmlLinePoint()

        RefreshAll()

        m_ProjectDirty = True

    End Sub

    Private Sub menuUmlLinePointRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlLinePointRemove.Click

        Dim umlLinePoint As umlLinePoint = m_contextMenuMapObject

        umlLinePoint.UmlLine.UmlLinePoints.Remove(umlLinePoint)

        RefreshAll()

        m_ProjectDirty = True

    End Sub

    Private Sub menuUmlLinePointProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlLinePointProperties.Click

        OpenProperties()

    End Sub

    Private Sub menuUmlLineEndSelectLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlLineEndSelectLine.Click

        Dim umlLine As umlLine = m_contextMenuMapObject
        Dim umlLinePoint As umlLinePoint

        umlLine.StartSelected = True
        umlLine.EndSelected = True

        For Each umlLinePoint In umlLine.UmlLinePoints

            umlLinePoint.Selected = True

        Next

        RefreshAll()

    End Sub

    Private Sub menuUmlZoom100_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoom100.Click

        SetUmlDiagramZoom(m_contextMenuMapObject, 1)

    End Sub

    Private Sub menuUmlZoom150_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoom150.Click

        SetUmlDiagramZoom(m_contextMenuMapObject, 1.5)
    End Sub

    Private Sub menuUmlZoom200_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoom200.Click

        SetUmlDiagramZoom(m_contextMenuMapObject, 2)
    End Sub

    Private Sub menuUmlZoom400_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoom400.Click

        SetUmlDiagramZoom(m_contextMenuMapObject, 4)

    End Sub

    Private Sub menuUmlZoom50_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoom50.Click

        SetUmlDiagramZoom(m_contextMenuMapObject, 0.5)

    End Sub

    Private Sub menuUmlZoom75_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoom75.Click

        SetUmlDiagramZoom(m_contextMenuMapObject, 0.75)

    End Sub

    Private Sub menuUmlZoomIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoomIn.Click

        Dim diagram As UmlDiagram = m_contextMenuMapObject

        diagram.ZoomInOut(0.25)

        RefreshUml()

    End Sub

    Private Sub menuUmlZoomOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuUmlZoomOut.Click

        Dim diagram As UmlDiagram = m_contextMenuMapObject

        diagram.ZoomInOut(-0.25)

        RefreshUml()

    End Sub

    Public Sub SetUmlDiagramZoom(ByVal diagram As UmlDiagram, ByVal value As Double)

        diagram.Zoom = value

        RefreshUml()

    End Sub


    Private Sub AddPropGridToTreeView()

        If m_PropTreeSplitter Is Nothing Then

            m_PropTreeSplitter = New Splitter

        End If

        'mapTreeView.Dock = DockStyle.Left
        'mapTreeView.Dock = DockStyle.Right

        'mapTreeView.Width = panelLeft.Width / 2

        m_PropTreeSplitter.Size = New System.Drawing.Size(4, 254)
        'm_PropTreeSplitter.Dock = DockStyle.Left
        m_PropTreeSplitter.Dock = DockStyle.Right
        panelLeft.Controls.Add(m_PropTreeSplitter)

        panelMain.Controls.Remove(mapPropertyGrid)

        'mapPropertyGrid.Dock = DockStyle.Fill
        'mapPropertyGrid.Dock = DockStyle.Left
        mapPropertyGrid.Dock = DockStyle.Right

        mapPropertyGrid.Width = panelLeft.Width / 2

        panelLeft.Controls.Add(mapPropertyGrid)

        Label5.Text = "Documents"

        'tabControlDocuments.TabPages.Remove(tabPageMainProperties)
        'tabPageMainProperties.Visible = False


    End Sub


#Region " Printing "


    ' Declare the PrintDocument object.
    Private m_PageSettings As Printing.PageSettings = Nothing
    Private WithEvents umlToPrint As New Printing.PrintDocument
    Private umlDocToPrint As UserDocTabPage


    Private Function MayPrint() As Boolean

        Dim doc As UserDocTabPage = GetCurrentOpenDoc()

        If Not doc Is Nothing Then

            Return True

        Else

            doc = GetCurrentUmlDoc()

            If Not doc Is Nothing Then Return True

        End If

    End Function

    Private Sub Print(Optional ByVal preview As Boolean = False)

        Dim doc As UserDocTabPage = GetCurrentOpenDoc()

        If Not doc Is Nothing Then

            'PrintText(doc, preview)
            PrintSyntaxBox(doc, preview)

        Else

            doc = GetCurrentUmlDoc()

            If Not doc Is Nothing Then PrintUml(doc, preview)

        End If

    End Sub


    Private Sub PrintUml(ByVal doc As UserDocTabPage, Optional ByVal preview As Boolean = False)

        Dim result As DialogResult

        umlDocToPrint = doc


        If Not m_PageSettings Is Nothing Then

            umlToPrint.DefaultPageSettings = m_PageSettings

        End If

        ' Allow the user to choose the page range he or she would
        ' like to print.
        PrintDialog1.AllowSomePages = False

        ' Show the help button.
        PrintDialog1.ShowHelp = False

        PrintDialog1.AllowPrintToFile = True
        PrintDialog1.ShowNetwork = True

        umlToPrint.DocumentName = doc.Text

        If preview Then

            PrintPreviewDialog1.Document = umlToPrint

            result = PrintPreviewDialog1.ShowDialog()

        Else

            PrintDialog1.Document = umlToPrint

            result = PrintDialog1.ShowDialog()

            ' If the result is OK then print the document.
            If (result = DialogResult.OK) Then
                umlToPrint.Print()
            End If

        End If


    End Sub

    ' This method will set properties on the PrintDialog object and
    ' then display the dialog.
    Private Sub PrintSyntaxBox(ByVal doc As UserDocTabPage, Optional ByVal preview As Boolean = False)

        Dim result As DialogResult

        SourceCodePrintDocument1.Document = doc.TextBox.Document

        If Not m_PageSettings Is Nothing Then

            SourceCodePrintDocument1.DefaultPageSettings = m_PageSettings

        End If

        PrintDialog1.AllowSomePages = False

        ' Show the help button.
        PrintDialog1.ShowHelp = False

        PrintDialog1.AllowPrintToFile = True
        PrintDialog1.ShowNetwork = True

        If preview Then

            PrintPreviewDialog1.Document = SourceCodePrintDocument1
            result = PrintPreviewDialog1.ShowDialog()

        Else

            PrintDialog1.Document = SourceCodePrintDocument1
            result = PrintDialog1.ShowDialog()

            ' If the result is OK then print the document.

            If (result = DialogResult.OK) Then
                SourceCodePrintDocument1.Print()
            End If

        End If



    End Sub


    ' This method will set properties on the PrintDialog object and
    ' then display the dialog.
    Private Sub PrintText(ByVal doc As UserDocTabPage, Optional ByVal preview As Boolean = False)

        Dim docToPrint As New TextPrintDocument(doc.TextBoxText)


        If Not m_PageSettings Is Nothing Then

            docToPrint.DefaultPageSettings = m_PageSettings

        End If

        PrintDialog1.AllowSomePages = False

        PrintDialog1.ShowHelp = False

        PrintDialog1.AllowPrintToFile = True
        PrintDialog1.ShowNetwork = True

        'docToPrint.Font = doc.TextFont
        docToPrint.DocumentName = doc.Text

        PrintDialog1.Document = docToPrint

        Dim result As DialogResult = PrintDialog1.ShowDialog()

        ' If the result is OK then print the document.
        If (result = DialogResult.OK) Then
            docToPrint.Print()
        End If

    End Sub

    Private Sub document_PrintPage(ByVal sender As Object, _
       ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles umlToPrint.PrintPage

        umlDocToPrint.UmlDiagram.Render(e.Graphics, False, False, False)

    End Sub

    Private Sub PageSetup()

        If m_PageSettings Is Nothing Then

            m_PageSettings = New Printing.PageSettings

        End If

        PageSetupDialog1.PageSettings = m_PageSettings

        If Not PageSetupDialog1.ShowDialog() = DialogResult.Cancel Then

            'No difference...

        End If

    End Sub

#End Region

#Region " Image Saving "

    Private Sub SaveUmlViewToFile(ByVal uml As UmlDiagram, Optional ByVal fileName As String = "", Optional ByVal format As Imaging.ImageFormat = Nothing)

        Dim umlDoc As UserDocTabPage = GetCurrentUmlDoc()

        If umlDoc Is Nothing Then Exit Sub

        SaveUmlDocumentToFile(uml, "", False, uml.Location.X, uml.Location.Y, umlDoc.Size.Width, umlDoc.Size.Height)

    End Sub

    Private Sub SaveUmlDocumentToFile(ByVal uml As UmlDiagram, Optional ByVal fileName As String = "", Optional ByVal unZoomed As Boolean = True, Optional ByVal x As Integer = -1, Optional ByVal y As Integer = -1, Optional ByVal w As Integer = -1, Optional ByVal h As Integer = -1, Optional ByVal format As Imaging.ImageFormat = Nothing)

        Dim margin As Integer = 20
        Dim ext As String
        Dim cacheLocation As Point
        Dim restore As Boolean
        Dim totalRect As Rectangle


        If fileName = "" Then


            fileName = uml.Name

            SaveFileDialog1.FileName = fileName
            'SaveFileDialog1.Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPG (*.jpg;*.jpeg;*.jpe)|*.jpg;*.jpeg;*.jpe|PNG (*.png)|*.png|TIFF (*.tif;*.tiff)|*.tif;*.tiff|Xml (*.xml)|*.xml|All files (*.*)|*.*"
            SaveFileDialog1.Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPG (*.jpg;*.jpeg;*.jpe)|*.jpg;*.jpeg;*.jpe|PNG (*.png)|*.png|TIFF (*.tif;*.tiff)|*.tif;*.tiff"

            If SaveFileDialog1.ShowDialog(Me) = DialogResult.Cancel Then

                Exit Sub

            End If

            fileName = SaveFileDialog1.FileName

            If Len(fileName) < 1 Then

                Exit Sub

            End If

            ext = GetExtension(fileName)

            Select Case LCase(ext)

                Case "bmp"

                    format = Imaging.ImageFormat.Bmp

                Case "gif"

                    format = Imaging.ImageFormat.Gif

                Case "jpg", "jpeg", "jpe"

                    format = Imaging.ImageFormat.Jpeg

                Case "png"

                    format = Imaging.ImageFormat.Png

                Case "tif", "tiff"

                    format = Imaging.ImageFormat.Tiff

                Case "xml"

                    Exit Sub

                Case Else

                    Exit Sub

            End Select

        End If

        If x = -1 And y = -1 And w = -1 And h = -1 Then

            restore = True
            cacheLocation = uml.Location

            totalRect = uml.GetTotalRectangle(False, True, True)

            uml.Location = New Point(totalRect.Location.X - margin, totalRect.Location.Y - margin)

            w = totalRect.Width + 2 * margin
            h = totalRect.Height + 2 * margin

        End If


        Dim bmp As Bitmap = New Bitmap(w, h)
        Dim g As Graphics = Graphics.FromImage(bmp)

        g.Clear(uml.BackColor1.ToColor)

        uml.Render(g, False, False, False)

        If restore Then

            uml.Location = cacheLocation

        End If

        bmp.Save(fileName, format)

    End Sub

    Private Function GetExtension(ByVal filename As String) As String

        Dim arr() As String

        arr = Split(filename, ".")

        Return arr(UBound(arr))

    End Function


#End Region

    Private Sub menuFilePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFilePrint.Click

        Print()

    End Sub

    Private Sub menuFilePrintPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuFilePrintPreview.Click

        Print(True)

    End Sub

    Private Sub menuFilePageSetup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuFilePageSetup.Click

        PageSetup()

    End Sub

    Private Sub menuUmlSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlSaveAs.Click

        SaveUmlDocumentToFile(m_contextMenuMapObject)

    End Sub

    Private Sub menuUmlSaveViewAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuUmlSaveViewAs.Click

        SaveUmlViewToFile(m_contextMenuMapObject)

    End Sub

    Private Sub tabControlDocuments_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabControlDocuments.SelectedIndexChanged

        If tabControlDocuments.SelectedTab Is tabPageMainXmlBehind Then

            If Not m_CurrSaveObject Is Nothing Then

                If m_CurrSaveObject.GetType Is GetType(UmlDiagram) Then

                ElseIf m_CurrSaveObject.GetType Is GetType(DomainMap) Then

                    OpenXmlBehind()

                End If

            End If

        End If



    End Sub

    Private Sub mapPropertyGrid_PropertySortChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mapPropertyGrid.PropertySortChanged

        If Not m_settingUp Then

            If mapPropertyGrid.TheGrid.PropertySort = PropertySort.Alphabetical Then

                m_ApplicationSettings.OptionSettings.EnvironmentSettings.AlphabeticPropertyGrid = True

            Else

                m_ApplicationSettings.OptionSettings.EnvironmentSettings.AlphabeticPropertyGrid = False

            End If

            SaveAppSettings()

        End If

    End Sub

    Private Sub AddErrorIcons()

        Dim newImages As IList = New ArrayList
        'Dim errorBmp As Bitmap = New Bitmap(16, 16)
        Dim errorImg As Image = imageListSmall.Images(imageListSmall.Images.Count - 1)

        'errorBmp.FromFile("../gfx/icons/error.bmp", True)
        'errorBmp.FromFile("../gfx/icons/error.bmp")
        'errorBmp.MakeTransparent()

        For Each img As Image In imageListSmall.Images

            'AddErrorIconToImage(img, newImages, errorBmp)
            AddErrorIconToImage(img, newImages, errorImg)

        Next

        For Each img As Image In newImages

            imageListSmall.Images.Add(img)

        Next

    End Sub

    Private Sub AddErrorIconToImage(ByVal img As Image, ByVal newImages As IList, ByVal errorImg As Image)

        Dim bmp As Bitmap = New Bitmap(16, 16)
        Dim g As Graphics = Graphics.FromImage(bmp)

        g.DrawImage(img, 0, 0, 16, 16)
        g.DrawImage(errorImg, 0, 0, 16, 16)
        g.Dispose()

        newImages.Add(bmp)

    End Sub


    Private Sub listViewExceptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listViewExceptions.Click

        If m_Updating Then Exit Sub
        Dim domainMap As IDomainMap

        Dim mapObject As IMap

        Dim mapExceptionItem As mapExceptionItem

        For Each mapExceptionItem In listViewExceptions.SelectedItems

            mapObject = mapExceptionItem.GetMap

            Dim mapNode As mapNode = mapTreeView.ExpandToMapObject(mapObject)

            If Not mapNode Is Nothing Then

                mapTreeView.SelectedNode = mapNode

            Else

                SelectPropertiesForMapObject(mapObject)

            End If

            Exit For

        Next

    End Sub



    Public Shared Function HasReferenceDataType(ByVal propertyMap As IPropertyMap) As Boolean

        Dim className As String
        If propertyMap.IsCollection Then
            className = propertyMap.ItemType
        Else
            className = propertyMap.DataType
        End If
        If Len(className) < 1 Then Return False
        Dim ns As String
        Dim classMap As IClassMap = propertyMap.ClassMap.DomainMap.GetClassMap(className)
        If classMap Is Nothing Then
            ns = propertyMap.ClassMap.GetNamespace()
            If Len(ns) > 0 Then classMap = propertyMap.ClassMap.DomainMap.GetClassMap(ns & "." & className)
        End If
        If Not classMap Is Nothing Then
            If classMap.ClassType = ClassType.Default Or classMap.ClassType = ClassType.Default Then
                Return True
            End If
        End If
        Return False

    End Function

    Public Shared Function GetCustomMetaDataConfigs() As IList

        Return m_CustomMetaDataConfigs

    End Function


    Private Sub LoadCustomMetaDataConfigs()

        If m_DoLoadCustomMetaDataConfigs = False Then Exit Sub

        Dim path As String = Application.LocalUserAppDataPath & "\..\custom\metadata"

        If Directory.Exists(path) Then

            For Each file As String In Directory.GetFiles(path)

                Dim fileInfo As fileInfo = New fileInfo(file)

                If fileInfo.Extension = ".xml" Then

                    Try

                        'Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(CustomMetaDataConfig))
                        '' To read the file, create a FileStream object.
                        'Dim myFileStream As FileStream = New FileStream(file, FileMode.Open, FileAccess.Read)

                        'Dim config As CustomMetaDataConfig = CType( _
                        'mySerializer.Deserialize(myFileStream), CustomMetaDataConfig)

                        'myFileStream.Close()

                        Dim config As CustomMetaDataConfig = CustomMetaDataConfig.Load(file)

                        If Not config Is Nothing Then

                            m_CustomMetaDataConfigs.Add(config)

                        End If

                    Catch ex As Exception


                    End Try

                End If

            Next

        End If

    End Sub

    Private Sub RunInDomainExplorer()

        If m_CurrSaveObject Is Nothing Then Exit Sub

        If Not m_CurrSaveObject.GetType Is GetType(DomainMap) Then Exit Sub

        Dim dmFrm As New Puzzle.NPersist.Tools.DomainExplorer.MainForm(CType(m_CurrSaveObject, IDomainMap).DeepClone)
        dmFrm.Show()

    End Sub

    Private Sub OpenDomainExplorer()

        Dim dmFrm As New Puzzle.NPersist.Tools.DomainExplorer.MainForm
        dmFrm.Show()

    End Sub

    Private Sub OpenDomainExplorer(ByVal domainMap As IDomainMap)

        Dim dmFrm As New Puzzle.NPersist.Tools.DomainExplorer.MainForm(domainMap.DeepClone)
        dmFrm.Show()

    End Sub


    Private Sub OpenQueryAnalyser()

        Dim dmFrm As New Puzzle.NPersist.Tools.QueryAnalyzer.Form1
        dmFrm.Show()

    End Sub

    Private Sub menuToolsNPersistDomainExplorer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsNPersistDomainExplorer.Click

        OpenDomainExplorer()

    End Sub

    Private Sub menuToolsNPersistQueryAnalyzer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsNPersistQueryAnalyzer.Click

        OpenQueryAnalyser()

    End Sub

    Private Sub menuDomainTestInDomainExplorer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainTestInDomainExplorer.Click

        OpenDomainExplorer(m_contextMenuMapObject)

    End Sub

    Private Sub menuClassCodeMapCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCodeMapCs.Click

        OpenClassCodeMap(m_contextMenuMapObject, CodeLanguage.CSharp)

    End Sub


    Private Sub menuClassCodeMapVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCodeMapVb.Click

        OpenClassCodeMap(m_contextMenuMapObject, CodeLanguage.VB)

    End Sub

    Private Sub menuClassCodeMapDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuClassCodeMapDelphi.Click

        OpenClassCodeMap(m_contextMenuMapObject, CodeLanguage.Delphi)

    End Sub


    Private Sub menuDomainCleanupAllOrMappings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCleanupAllOrMappings.Click

        CleanDomainMapOrMappings(m_contextMenuMapObject)

    End Sub


    Private Sub menuDomainCodeMapCs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeMapCs.Click

        OpenDomainCodeMap(m_contextMenuMapObject, CodeLanguage.CSharp)

    End Sub

    Private Sub menuDomainCodeMapVb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeMapVb.Click

        OpenDomainCodeMap(m_contextMenuMapObject, CodeLanguage.CSharp)

    End Sub

    Private Sub menuDomainCodeMapDelphi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDomainCodeMapDelphi.Click

        OpenDomainCodeMap(m_contextMenuMapObject, CodeLanguage.CSharp)

    End Sub


    Private Sub buttonPrevCodeMapDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonPrevCodeMapDoc.Click

        GoPrevCodeMapDoc()

    End Sub

    Private Sub buttonNextCodeMapDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNextCodeMapDoc.Click

        GoNextCodeMapDoc()

    End Sub

    Private Sub buttonCloseCodeMapDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseCodeMapDoc.Click

        CloseCodeMapDoc(True)

    End Sub


    Private Sub CleanDomainMapOrMappings(ByVal domainMap As IDomainMap)

        For Each classMap As IClassMap In domainMap.ClassMaps

            classMap.Source = ""
            classMap.Table = ""
            classMap.TypeColumn = ""

            For Each propertyMap As IPropertyMap In classMap.PropertyMaps

                propertyMap.Source = ""
                propertyMap.Table = ""
                propertyMap.Column = ""
                propertyMap.AdditionalColumns.Clear()
                propertyMap.IdColumn = ""
                propertyMap.AdditionalIdColumns.Clear()

            Next

        Next

        RefreshAll()

    End Sub
#End Region




End Class


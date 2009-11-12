Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping

Public Class ConverterMethodConverter
    Inherits StringConverter


    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim convClass As ConverterClass
        Dim convMethod As ConverterMethod
        Dim listMethods As New ArrayList

        For Each convClass In frmDomainMapBrowser.Converters

            For Each convMethod In convClass.ConverterMethods

                listMethods.Add(convClass.Name & " : " & convMethod.MethodName)

            Next

        Next

        listMethods.Sort()

        Return New StandardValuesCollection(listMethods)

    End Function

End Class


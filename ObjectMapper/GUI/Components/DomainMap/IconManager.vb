Imports Puzzle.NPersist.Framework.Mapping

Public Class IconManager

    Public Shared Sub GetIconIndexesForMap(ByVal map As IMap, ByRef imgIndex As Integer, ByRef selImgIndex As Integer)

        If map Is Nothing Then Exit Sub

        Dim mapType As Type = CType(map, Object).GetType()

        If Not mapType.GetInterface(GetType(IDomainMap).ToString()) Is Nothing Then

            imgIndex = 0
            selImgIndex = 1

        ElseIf Not mapType.GetInterface(GetType(IClassMap).ToString()) Is Nothing Then

            Dim classMap As IClassMap = map

            imgIndex = 6
            selImgIndex = 7

            Select Case classMap.ClassType

                Case NPersist.Framework.Enumerations.ClassType.Interface

                    imgIndex = 114
                    selImgIndex = 114

                Case NPersist.Framework.Enumerations.ClassType.Struct

                    imgIndex = 115
                    selImgIndex = 115

                Case NPersist.Framework.Enumerations.ClassType.Enum

                    imgIndex = 116
                    selImgIndex = 116

            End Select

        ElseIf Not mapType.GetInterface(GetType(IPropertyMap).ToString()) Is Nothing Then

            GetIconIndexesForPropertyMap(map, imgIndex, selImgIndex)

        ElseIf Not mapType.GetInterface(GetType(ISourceMap).ToString()) Is Nothing Then

            imgIndex = 12
            selImgIndex = 13

        ElseIf Not mapType.GetInterface(GetType(ITableMap).ToString()) Is Nothing Then

            imgIndex = 14
            selImgIndex = 15

        ElseIf Not mapType.GetInterface(GetType(IColumnMap).ToString()) Is Nothing Then

            GetIconIndexesForColumnMap(map, imgIndex, selImgIndex)

        End If


    End Sub

    Private Shared Sub GetIconIndexesForPropertyMap(ByVal map As IPropertyMap, ByRef imgIndex As Integer, ByRef selImgIndex As Integer)

        If map.IsIdentity Then

            imgIndex = 18
            selImgIndex = 19

        Else

            imgIndex = 8
            selImgIndex = 9

        End If

    End Sub


    Private Shared Sub GetIconIndexesForColumnMap(ByVal map As IColumnMap, ByRef imgIndex As Integer, ByRef selImgIndex As Integer)

        If map.IsPrimaryKey Then

            If map.IsForeignKey Then

                imgIndex = 44
                selImgIndex = 45

            Else

                imgIndex = 40
                selImgIndex = 41

            End If

        ElseIf map.IsForeignKey Then

            imgIndex = 42
            selImgIndex = 43

        Else

            imgIndex = 16
            selImgIndex = 17

        End If

    End Sub

End Class

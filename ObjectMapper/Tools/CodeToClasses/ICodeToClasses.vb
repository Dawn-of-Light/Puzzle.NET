Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Interface ICodeToClasses

    Sub AssemblyCodeToClasses(ByVal asm As System.Reflection.Assembly, ByVal targetDomainMap As IDomainMap)

    Sub AssemblyCodeToClasses(ByVal asm As System.Reflection.Assembly, ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap)

End Interface

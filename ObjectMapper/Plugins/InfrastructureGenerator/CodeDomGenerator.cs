using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Text;
using Microsoft.CSharp;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping;

namespace Puzzle.ObjectMapper.Plugins.ServiceLayerGenerator
{
	/// <summary>
	/// Summary description for CodeDomGenerator.
	/// </summary>
	public class CodeDomGenerator
	{
		public CodeDomGenerator()
		{
		}

		public static string ToCode(CodeCompileUnit compileunit, CodeDomProvider provider)
		{

			StringBuilder sb = new StringBuilder() ;
			StringWriter sw = new StringWriter(sb);
			IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");

			ICodeGenerator gen = provider.CreateGenerator(tw);

			gen.GenerateCodeFromCompileUnit(compileunit, tw, 
				new CodeGeneratorOptions());

			string code = sb.ToString();

			return code;
		}


		public static CodeCompileUnit GetFactoryClassCompileUnit(IClassMap classMap)
		{			
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();

			codeCompileUnit.Namespaces.Add(GenerateFactoryClass(classMap));

			return codeCompileUnit ;
		}

		#region Factory

		public static CodeNamespace GenerateFactoryClass(IClassMap classMap)
		{
			CodeNamespace domainNamespace = new CodeNamespace(classMap.GetFullNamespace()) ;
			CodeTypeDeclaration classDecl = new CodeTypeDeclaration(GetFactoryClassName(classMap)) ;

			classDecl.IsClass = true;

			GenerateFactoryMethods(classMap, classDecl);

			foreach (IPropertyMap propertyMap in classMap.PropertyMaps)
			{
				//classDecl.Members.Add(PropertyMapToCodeMemberField(propertyMap));					
				//classDecl.Members.Add(PropertyMapToCodeMemberProperty(propertyMap));					
			}

			domainNamespace.Types.Add(classDecl);

			return domainNamespace;
			

		}


		public static void GenerateFactoryMethods(IClassMap classMap, CodeTypeDeclaration classDecl)
		{
			IList propertyMaps = GetRequiredPropertyMaps(classMap);
			classDecl.Members.Add(GenerateFactoryMethod(classMap, propertyMaps));					

			IList optionalPropertyMaps = GetOptionalPropertyMaps(classMap);
			if (optionalPropertyMaps.Count > 0)
			{
				foreach (IPropertyMap propertyMap in optionalPropertyMaps)
					propertyMaps.Add(propertyMap);

				classDecl.Members.Add(GenerateFactoryMethod(classMap, propertyMaps));					
			}
		}


		public static CodeMemberMethod GenerateFactoryMethod(IClassMap classMap, IList propertyMaps)
		{

			CodeMemberMethod methodMember = new CodeMemberMethod() ;
			methodMember.Name = GetFactoryMethodName(classMap);

			CodeTypeReference typeReference = new CodeTypeReference(classMap.GetName());
			methodMember.ReturnType = typeReference;
			
			methodMember.Attributes = MemberAttributes.Public;

			foreach(IPropertyMap propertyMap in propertyMaps)
			{
				CodeParameterDeclarationExpression parameter = new CodeParameterDeclarationExpression(new CodeTypeReference(propertyMap.DataType), MakeCamelCase(propertyMap.Name));
				methodMember.Parameters.Add(parameter);
			}

			CodeVariableDeclarationStatement contextVarDecl = new CodeVariableDeclarationStatement("IContext", "context", null) ;
			CodeVariableReferenceExpression contextVar = new CodeVariableReferenceExpression("context");

			methodMember.Statements.Add(contextVarDecl);

			CodeTypeOfExpression typeOfExp = new CodeTypeOfExpression(typeReference) ;
			CodeMethodInvokeExpression newObjectInit = new CodeMethodInvokeExpression(contextVar, "CreateObject", new CodeExpression[] { typeOfExp } ) ;

			CodeCastExpression castExp = new CodeCastExpression(typeReference, newObjectInit) ;

			CodeVariableDeclarationStatement newObjectVarDecl = new CodeVariableDeclarationStatement(classMap.GetName(), MakeCamelCase(classMap.GetName()), castExp) ;
			CodeVariableReferenceExpression newObjectVar = new CodeVariableReferenceExpression(MakeCamelCase(classMap.GetName()));

			methodMember.Statements.Add(newObjectVarDecl);
 
			foreach(IPropertyMap propertyMap in propertyMaps)
			{
				CodeArgumentReferenceExpression argExp = new CodeArgumentReferenceExpression(MakeCamelCase(propertyMap.Name));
				CodeVariableReferenceExpression propExp = new CodeVariableReferenceExpression(MakeCamelCase(classMap.Name) + "." + propertyMap.Name);
				CodeAssignStatement assignStatement = new CodeAssignStatement(propExp, argExp);
				methodMember.Statements.Add(assignStatement);
			}

			CodeMethodInvokeExpression commitCtx = new CodeMethodInvokeExpression(contextVar, "Commit", new CodeExpression[] {} ) ;

			methodMember.Statements.Add(commitCtx);

			CodeMethodReturnStatement returnStmt = new CodeMethodReturnStatement(newObjectVar) ;

			methodMember.Statements.Add(returnStmt);

			return methodMember;

		}

		#endregion


		#region Template

		public static string GetFactoryClassName(IClassMap classMap)
		{		
			string name = classMap.GetName() + "Factory" ;

			return name;
		}

		public static string GetFactoryMethodName(IClassMap classMap)
		{		
			string name = "Create" + classMap.GetName() ;

			return name;
		}
	
		#endregion

		#region Helper Methods

		private static IList GetRequiredPropertyMaps(IClassMap classMap)
		{
			IList requiredPropertyMaps = new ArrayList();
			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps())
			{
				if (propertyMap.IsIdentity)
				{
					if (!propertyMap.GetIsAssignedBySource())
						requiredPropertyMaps.Add(propertyMap);				
				}
				else
				{
					if (!propertyMap.IsCollection)
						if (!propertyMap.GetIsNullable())
							requiredPropertyMaps.Add(propertyMap);								
				}				
			}

			return requiredPropertyMaps;
		}

		private static IList GetOptionalPropertyMaps(IClassMap classMap)
		{
			IList optionalPropertyMaps = new ArrayList();
			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps())
			{
				if (!propertyMap.IsIdentity)
				{
					if (!propertyMap.IsCollection)
						if (propertyMap.GetIsNullable())
							optionalPropertyMaps.Add(propertyMap);								
				}				
			}

			return optionalPropertyMaps;
		}

		private static string MakeCamelCase(string name)
		{
			return name.Substring(0, 1).ToLower() + name.Substring(1);
		}


		#endregion
	}
}

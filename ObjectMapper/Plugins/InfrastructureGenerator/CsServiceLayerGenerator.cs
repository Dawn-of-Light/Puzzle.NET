//using System;
//using System.Collections;
//using System.Text;
//using Puzzle.NPersist.Framework.Mapping;
//using Puzzle.ObjectMapper.Plugin;
//
//namespace Puzzle.ObjectMapper.Plugins.ServiceLayerGenerator
//{
//	/// <summary>
//	/// Summary description for CsServiceLayerGenerator.
//	/// </summary>
//	[PluginClass("Puzzle")]
//	public class CsServiceLayerGenerator
//	{
//		public CsServiceLayerGenerator()
//		{
//		}
//
//		[PluginMethod(typeof(IDomainMap), typeof(String), "Service Layer Generator")]
//		public string GenerateServiceLayer(IDomainMap domainMap) 
//		{
//			StringBuilder result = new StringBuilder();
//
//
//			return result.ToString();
//		}
//
//		[PluginMethod(typeof(IClassMap), typeof(String), "Service Layer Generator")]
//		public string GenerateServiceLayerMethods(IClassMap classMap) 
//		{
//			StringBuilder result = new StringBuilder();
//
//			result.Append(GenerateCreateMethods(classMap));
//
//			return result.ToString();
//		}
//
//
//
//		public string GenerateCreateMethods(IClassMap classMap) 
//		{
//			StringBuilder result = new StringBuilder();
//
//			IList requiredPropertyMaps = GetRequiredPropertyMaps(classMap);
//			result.Append(GenerateCreateMethod(classMap, requiredPropertyMaps));
//
//			IList optionalPropertyMaps = GetOptionalPropertyMaps(classMap);
//			if (optionalPropertyMaps.Count > 0)
//			{
//				foreach (IPropertyMap propertyMap in optionalPropertyMaps)
//					requiredPropertyMaps.Add(propertyMap);
//
//				result.Append(GenerateCreateMethod(classMap, requiredPropertyMaps));				
//			}
//
//			return result.ToString();
//		}
//
//
//		public string GenerateCreateMethod(IClassMap classMap, IList propertyMaps) 
//		{
//			StringBuilder result = new StringBuilder();
//
//			result.Append(GetTabs(2) + "public static Create" + classMap.Name + "(IContext context");
//
//			foreach (IPropertyMap propertyMap in propertyMaps)
//				result.Append(", " + propertyMap.DataType + " " + MakeCamelCase(propertyMap.Name));
//			
//			result.Append(")" + Environment.NewLine);
//			result.Append(GetTabs(2) + "{" + Environment.NewLine);
//
//			if (propertyMaps.Count > 0)
//			{
//				result.Append(GetTabs(3) + "//Verify incoming parameters" + Environment.NewLine);
//				foreach (IPropertyMap propertyMap in propertyMaps)
//					result.Append(GenerateParameterVerification(propertyMap));				
//			}
//
//			result.Append(GetTabs(3) + "//Create the new object" + Environment.NewLine);
//			result.Append(GetTabs(3) + classMap.GetName() + " " + MakeCamelCase(classMap.GetName()) + " = (" + 
//				classMap.GetName() + ") context.CreateObject(typeof(" + classMap.GetName() + "));" + Environment.NewLine);
//			result.Append(Environment.NewLine);
//
//			if (propertyMaps.Count > 0)
//			{
//				result.Append(GetTabs(3) + "//Transfer parameter values to properties" + Environment.NewLine);
//				foreach (IPropertyMap propertyMap in propertyMaps)
//					result.Append(GetTabs(3) + MakeCamelCase(propertyMap.ClassMap.GetName()) + "." + 
//						propertyMap.Name + " = " + MakeCamelCase(propertyMap.Name) + ";" + Environment.NewLine);
//				result.Append(Environment.NewLine);				
//			}
//
//			result.Append(GetTabs(3) + "//Commit the changes to the data source" + Environment.NewLine);
//			result.Append(GetTabs(3) + "context.Commit();" + Environment.NewLine);
//			result.Append(Environment.NewLine);
//
//			result.Append(GetTabs(3) + "//Return the new object" + Environment.NewLine);
//			result.Append(GetTabs(3) + "return " + MakeCamelCase(classMap.GetName()) + Environment.NewLine);
//
//			result.Append(GetTabs(2) + "}" + Environment.NewLine);
//			result.Append(Environment.NewLine);
//			return result.ToString();
//		}
//
//
//		public string GenerateParameterVerification(IPropertyMap propertyMap) 
//		{
//			StringBuilder result = new StringBuilder();
//
//
//			//verfiy non-nullable
//			if (propertyMap.GetIsNullable() == false || propertyMap.IsIdentity == true)
//			{
//				result.Append(GetTabs(3) + "if (" + MakeCamelCase(propertyMap.Name) + " == null)" + Environment.NewLine);
//				result.Append(GetTabs(4) + "throw new ArgumentNullException(\"" + MakeCamelCase(propertyMap.Name) + "\");" + Environment.NewLine);
//				result.Append(Environment.NewLine);
//			}
//
//			return result.ToString();
//		}
//
//
//
//
//
//
//		
//		private IList GetRequiredPropertyMaps(IClassMap classMap)
//		{
//			IList requiredPropertyMaps = new ArrayList();
//			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps())
//			{
//				if (propertyMap.IsIdentity)
//				{
//					if (!propertyMap.GetIsAssignedBySource())
//						requiredPropertyMaps.Add(propertyMap);				
//				}
//				else
//				{
//					if (!propertyMap.IsCollection)
//						if (!propertyMap.GetIsNullable())
//							requiredPropertyMaps.Add(propertyMap);								
//				}				
//			}
//
//			return requiredPropertyMaps;
//		}
//
//		private IList GetOptionalPropertyMaps(IClassMap classMap)
//		{
//			IList optionalPropertyMaps = new ArrayList();
//			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps())
//			{
//				if (!propertyMap.IsIdentity)
//				{
//					if (!propertyMap.IsCollection)
//						if (propertyMap.GetIsNullable())
//							optionalPropertyMaps.Add(propertyMap);								
//				}				
//			}
//
//			return optionalPropertyMaps;
//		}
//
//		private string MakeCamelCase(string name)
//		{
//			return name.Substring(0, 1).ToLower() + name.Substring(1);
//		}
//
//		private string tab = "    ";
//
//		public string GetTabs(int index)
//		{															
//			string str = "";
//
//			for(int i = 0; i <= index; i++)
//				str += tab;
//
//			return str;			
//		}
//
//	}
//}

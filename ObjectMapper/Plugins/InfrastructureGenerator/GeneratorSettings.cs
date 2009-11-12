using System;

namespace Puzzle.ObjectMapper.Plugins.ServiceLayerGenerator
{
	/// <summary>
	/// Summary description for GeneratorSettings.
	/// </summary>
	public class GeneratorSettings
	{
		public GeneratorSettings()
		{
		}

		#region Property  RootNamespace
		
		private string rootNamespace = "";
		
		public string RootNamespace
		{
			get { return this.rootNamespace; }
			set { this.rootNamespace = value; }
		}
		
		#endregion
	}
}

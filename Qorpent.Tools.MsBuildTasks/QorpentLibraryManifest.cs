#region LICENSE

// Copyright 2007-2012 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// Solution: Qorpent
// Original file : QorpentLibraryManifest.cs
// Project: Qorpent.Tools.MsBuildTasks
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Build.Utilities;
using Qorpent.Tools.ManifestGenerator.Lib;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.MsBuildTasks {
	/// <summary>
	/// 	Creates qorpent manifest for target DLL
	/// </summary>
	public class QorpentLibraryManifest : Task {
		/// <summary>
		/// 	Path to Assembly to make manifest
		/// </summary>
		public string AssemblyPath { get; set; }

		/// <summary>
		/// 	Path to manifest file
		/// </summary>
		public string ManifestPath { get; set; }

		/// <summary>
		/// 	True - use short notation
		/// </summary>
		public bool Short { get; set; }

		/// <summary>
		/// 	When overridden in a derived class, executes the task.
		/// </summary>
		/// <returns> true if the task successfully executed; otherwise, false. </returns>
		public override bool Execute() {
			var assemblypath = Path.GetFullPath(AssemblyPath);
			var manifestpath = Path.GetFullPath(ManifestPath);
			Log.LogMessage("Start generate Short:{0} manifest for {1}", Short, assemblypath);
			var generator = new ManifestBxlGenerator();
		    generator.NeedExportAttribute = false;
			var assembly = Assembly.Load(File.ReadAllBytes(assemblypath),File.ReadAllBytes(FileNameResolverExtensions.GetSymbolFileName(assemblypath)));
			var options = new ManifestBxlGeneratorOptions {UseShortNames = Short};
			var result = generator.GenerateManifest(new[] {assembly}, options);
			Directory.CreateDirectory(Path.GetDirectoryName(manifestpath));
			File.WriteAllText(ManifestPath, result, Encoding.UTF8);
			Log.LogMessage("Manifest {0} generated", manifestpath);
			return true;
		}
	}
}
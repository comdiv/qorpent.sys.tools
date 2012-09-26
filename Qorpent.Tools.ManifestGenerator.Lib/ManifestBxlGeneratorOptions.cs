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
// Original file : ManifestBxlGeneratorOptions.cs
// Project: Qorpent.Tools.ManifestGenerator.Lib
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;

namespace Qorpent.Tools.ManifestGenerator.Lib {
	/// <summary>
	/// 	options for manifest generation
	/// </summary>
	public class ManifestBxlGeneratorOptions {
		/// <summary>
		/// </summary>
		public ManifestBxlGeneratorOptions() {
			LibraryDirectory = Environment.CurrentDirectory;
		}

		/// <summary>
		/// 	true - will use class names without dll reference and namespaces+  reference/using list - need resolution on load, but shorter
		/// </summary>
		public bool UseShortNames { get; set; }

		/// <summary>
		/// 	file to write output
		/// </summary>
		public string OutputFile { get; set; }

		/// <summary>
		/// 	directory with dlls
		/// </summary>
		public string LibraryDirectory { get; set; }

		/// <summary>
		/// 	Local name of library to process
		/// </summary>
		public string LibraryName { get; set; }

		/// <summary>
		/// 	Regex to check dlls to exclude
		/// </summary>
		public string ExcludeLibRegex { get; set; }
	}
}
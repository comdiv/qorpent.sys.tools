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
// Original file : ManifestBxlGeneratorApplication.cs
// Project: Qorpent.Tools.ManifestGenerator.Lib
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;
using System.IO;
using Qorpent.Utils;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.ManifestGenerator.Lib {
	/// <summary>
	/// 	Console application implementation
	/// </summary>
	public class ManifestBxlGeneratorApplication {
		/// <summary>
		/// 	Executes manifest generator
		/// </summary>
		/// <param name="args"> </param>
		public void Run(string[] args) {
			var options = new ConsoleArgumentHelper().Parse<ManifestBxlGeneratorOptions>(args);
			if (options.LibraryDirectory.IsEmpty() && options.LibraryName.IsNotEmpty()) {
				options.LibraryDirectory = Path.GetDirectoryName(Path.GetFullPath(options.LibraryName));
				options.LibraryName = Path.GetFileName(options.LibraryName);
			}
			options.LibraryDirectory = Path.GetFullPath(options.LibraryDirectory);
			if (options.LibraryName.IsNotEmpty()) {
				options.LibraryName = Path.Combine(options.LibraryDirectory, options.LibraryName);
			}
			if (options.OutputFile.IsNotEmpty()) {
				Console.WriteLine("LibraryDir: " + options.LibraryDirectory);
			}
			var generator = new ManifestBxlGenerator();
			var content = generator.GenerateManifest(options);
			if (options.OutputFile.IsEmpty()) {
				Console.WriteLine(content);
			}
			else {
				if (content.IsEmpty() && options.LibraryName.IsNotEmpty()) {
					return;
				}
				File.WriteAllText(options.OutputFile, content);
				Console.WriteLine("manifest saved");
			}
		}
	}
}
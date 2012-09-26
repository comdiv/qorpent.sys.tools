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
// Original file : DslBuilderArgs.cs
// Project: Qorpent.Tools.DslBuilder.Lib
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System.IO;
using Qorpent.Applications;
using Qorpent.IO;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.DslBuilder.Lib {
	/// <summary>
	/// 	Describes console arguments for DslBuilder
	/// </summary>
	public class DslBuilderArgs {
		/// <summary>
		/// 	External library place - can be relative to RootDirectory
		/// </summary>
		public string ReferenceDirectory { get; set; }

		/// <summary>
		/// 	Name or full path to project
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// 	Directory where projects placed - can be relative to RootDirectory, ./projects by default
		/// </summary>
		public string ProjectsDirectory { get; set; }

		/// <summary>
		/// 	Root of DSL definition folder (logical root folder) - main application Root used if no other choosed
		/// </summary>
		public string RootDirectory { get; set; }

		/// <summary>
		/// 	Language to be used - by default will be encountered from default.proj file
		/// </summary>
		public string DslLang { get; set; }

		/// <summary>
		/// 	output for preprocess result - projfolder/obj will be used by default
		/// </summary>
		public string OutCodeDirectory { get; set; }

		/// <summary>
		/// 	output for preprocess result - projfolder/bin will be used by default
		/// </summary>
		public string OutDirectory { get; set; }

		/// <summary>
		/// 	Allows to give Assembly name as first unknown parameter
		/// </summary>
		public string Arg1 {
			get { return _arg1; }
			set {
				_arg1 = value;
				ProjectName = _arg1;
			}
		}

		/// <summary>
		/// 	Project type
		/// </summary>
		public string ProjectType { get; set; }

		/// <summary>
		/// 	Compiler options
		/// </summary>
		public string CompilerOptions { get; set; }

		/// <summary>
		/// 	After loading from console it will be used to setup valid parameters
		/// </summary>
		public void SetupDefaults() {
			if (RootDirectory.IsEmpty()) {
				RootDirectory = Application.Current.RootDirectory;
			}
			var resolver = Application.Current.Container.Get<IFileNameResolver>();
			resolver.Root = RootDirectory;
			if (ReferenceDirectory.IsEmpty()) {
				var custom = resolver.Resolve("~/lib", true);
				if (custom.IsNotEmpty()) {
					ReferenceDirectory = custom;
				}
				else {
					ReferenceDirectory = EnvironmentInfo.BinDirectory;
				}
			}
			else {
				ReferenceDirectory = resolver.Resolve(ReferenceDirectory, false);
			}

			if (ProjectsDirectory.IsEmpty()) {
				ProjectsDirectory = resolver.Resolve("~/projects", false);
			}
			ProjectsDirectory = resolver.Resolve(ProjectsDirectory, false);
			if (ProjectName.IsEmpty()) {
				ProjectName = "DefaultProject";
			}
			if (!Path.IsPathRooted(ProjectName)) {
				ProjectName = Path.Combine(ProjectsDirectory, ProjectName);
			}

			if (OutDirectory.IsEmpty()) {
				OutDirectory = "bin";
			}
			if (!Path.IsPathRooted(OutDirectory)) {
				OutDirectory = Path.Combine(ProjectName, OutDirectory);
			}

			if (OutCodeDirectory.IsEmpty()) {
				OutCodeDirectory = "obj";
			}
			if (!Path.IsPathRooted(OutCodeDirectory)) {
				OutCodeDirectory = Path.Combine(ProjectName, OutCodeDirectory);
			}
			Directory.CreateDirectory(RootDirectory);
			Directory.CreateDirectory(ReferenceDirectory);
			Directory.CreateDirectory(ProjectsDirectory);
			Directory.CreateDirectory(ProjectName);
			Directory.CreateDirectory(OutDirectory);
			Directory.CreateDirectory(OutCodeDirectory);
		}

		private string _arg1;
	}
}
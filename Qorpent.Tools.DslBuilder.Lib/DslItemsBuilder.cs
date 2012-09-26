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
// Original file : DslItemsBuilder.cs
// Project: Qorpent.Tools.DslBuilder.Lib
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System.IO;
using Qorpent.Applications;
using Qorpent.Dsl;
using Qorpent.IO;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.DslBuilder.Lib {
	/// <summary>
	/// 	Converts Build arguments to DslProject
	/// </summary>
	public class DslItemsBuilder {
		/// <summary>
		/// 	Generates dsl provider for given arguments
		/// </summary>
		/// <param name="args"> </param>
		/// <returns> </returns>
		public IDslProvider CreateProvider(DslBuilderArgs args) {
			var proj = CreateProject(args); //temporal project
			var cont = ContainerFactory.CreateDefault();
			cont.SetParameter<IFileNameResolver>("root", args.RootDirectory);
			var factory = cont.Get<IDslProviderService>();
			var provider = factory.GetProvider(proj);
			return provider;
		}

		/// <summary>
		/// 	Generates DSL project definition using given parameters
		/// </summary>
		/// <param name="args"> </param>
		/// <returns> </returns>
		public DslProject CreateProject(DslBuilderArgs args) {
			args.SetupDefaults();
			var result = new DslProject();
			result.ResultName = Path.GetFileNameWithoutExtension(args.ProjectName);

			result.NativeCodeDirectory = args.OutCodeDirectory;
			result.OutputDirectory = args.OutDirectory;
			result.ReferenceDirectory = args.ReferenceDirectory;
			result.RootDirectory = args.ProjectName;
			var projdeffile = Path.Combine(args.ProjectName, "proj.def");
			if (File.Exists(projdeffile)) {
				var projdef = Application.Current.Bxl.Parse(File.ReadAllText(projdeffile), projdeffile);
				var e = projdef.Element("name");
				if (null != e) {
					result.ResultName = e.Describe().Name;
				}
				e = projdef.Element("lang");
				if (null != e) {
					result.LangName = e.Describe().Name;
				}

				e = projdef.Element("type");
				if (null != e) {
					result.ProjectType = e.Describe().Name.To<DslProjectType>();
				}
				e = projdef.Element("options");
				if (null != e) {
					result.CompilerOptions = e.Describe().Name;
				}
				foreach (var src in projdef.Elements("source")) {
					result.FileSources.Add(src.Describe().Name);
				}
			}
			if (args.DslLang.IsNotEmpty()) {
				result.LangName = args.DslLang;
			}
			if (args.ProjectType.IsNotEmpty()) {
				result.ProjectType = args.ProjectType.To<DslProjectType>();
			}
			if (0 == result.FileSources.Count) {
				var files = Directory.GetFiles(result.RootDirectory, "*.bxl", SearchOption.AllDirectories);
				foreach (var file in files) {
					result.FileSources.Add(file);
				}
			}
			if (args.CompilerOptions.IsNotEmpty()) {
				result.CompilerOptions += "|" + args.CompilerOptions;
			}
			if (result.LangName.IsEmpty()) {
				throw new QorpentException("cannot evaluate or fine language name for this project");
			}
			return result;
		}
	}
}
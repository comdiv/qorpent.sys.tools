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
// Original file : QorpentDsl.cs
// Project: Qorpent.Tools.MsBuildTasks
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Qorpent.Dsl;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.MsBuildTasks {
	/// <summary>
	/// </summary>
	public class QorpentDsl : Task {
		internal static QorpentDsl LastInstance;

		/// <summary>
		/// </summary>
		public string DslType { get; set; }

		/// <summary>
		/// </summary>
		public bool Preprocess { get; set; }

		/// <summary>
		/// </summary>
		public string OutputCodeDir { get; set; }

		/// <summary>
		/// </summary>
		public DslProjectType DslProjectType {
			get { return DslType.To<DslProjectType>(); }
		}

		/// <summary>
		/// </summary>
		public string Lang { get; set; }

		/// <summary>
		/// </summary>
		public bool TraceOnly { get; set; }

		/// <summary>
		/// </summary>
		public string DslDir { get; set; }

		/// <summary>
		/// </summary>
		public ITaskItem[] Sources { get; set; }

		private void log(string message, params object[] args) {
			Log.LogMessage(MessageImportance.Normal, message, args);
		}

		/// <summary>
		/// 	When overridden in a derived class, executes the task.
		/// </summary>
		/// <returns> true if the task successfully executed; otherwise, false. </returns>
		public override bool Execute() {
			Called = true;
			LastInstance = this;
			log("start dsl");
			if (Lang.IsEmpty()) {
				Log.LogWarning("no DSL defined - skip dsl");
				return true;
			}
			log("DSL Lang: {0}", Lang);
			log("DSL Type: {0}", DslProjectType);
			log("DSL Root: {0}", DslDir);
			log("OutCode Dir: {0}", OutputCodeDir);
			log("PreprocessObly: {0}", Preprocess);
			if (TraceOnly) {
				log("skip because traceonly mode choosed");
				return true;
			}
			var proj = new DslProject
				{
					LangName = Lang,
					ProjectType = DslProjectType,
					NativeCodeDirectory = OutputCodeDir,
					PreprocessOnly = Preprocess,
					DslDirectory = DslDir
				};
			foreach (var src in Sources) {
				proj.FileSources.Add(src.GetMetadata("FullPath"));
			}
			var provider = new XsltBasedDslProvider();
			var result = provider.Run(proj);

			foreach (var srcfile in result.LoadedXmlSources) {
				log("Src: " + srcfile.Key);
			}

			if (Preprocess) {
				foreach (var generated in result.GeneratedNativeCode) {
					log("Generated: " + generated.Key);
				}
			}
			else {
				log("Compiled: " + result.GetProductionFileName());
			}


			return true;
		}

		internal bool Called;
	}
}
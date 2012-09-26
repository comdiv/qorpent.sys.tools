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
// Original file : BaseTaskUsageTest.cs
// Project: Qorpent.Tools.MsBuildTasks.Tests
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Qorpent.Utils.TestSupport;

namespace Qorpent.Tools.MsBuildTasks.Tests {
	[TestFixture]
	[QorpentFixture(UseTemporalFileSystem = true,
		PrepareFileSystemMap = @"
testp.xml~testp.proj,
mainp.xml~mainp.proj,
basep.xml~imports/basep.proj,
src1.bxl~src/1.bxl,
trydsl.xslt~dsl/test/default.xslt

	")]
	public class BaseTaskUsageTest : QorpentFixture {
		[Test]
		public void TaskIsCalled() {
			var man = BuildManager.DefaultBuildManager;
			var bp = new BuildParameters();

			var brd = new BuildRequestData(Path.Combine(Tmpdir, "testp.proj"), new Dictionary<string, string>
				{
					{
						"AssemblyPath",
						typeof (QorpentDsl).Assembly.CodeBase.
				                                                                   Replace("file:///", "")
					}
				}, null, new[] {"Test"}, null,
			                               BuildRequestDataFlags.None);
			ILogger l = new ConsoleLogger();
			bp.Loggers = new[] {l};
			man.Build(bp, brd);


			Assert.True(QorpentDsl.LastInstance.Called);
			foreach (var taskItem in QorpentDsl.LastInstance.Sources) {
				Console.WriteLine(taskItem.GetMetadata("FullPath"));
			}
		}


		[Test]
		public void TaskIsPrcessed() {
			var man = BuildManager.DefaultBuildManager;
			var bp = new BuildParameters();

			var brd = new BuildRequestData(Path.Combine(Tmpdir, "mainp.proj"), new Dictionary<string, string>
				{
					{
						"AssemblyPath",
						typeof (QorpentDsl).Assembly.CodeBase.
				                                                                   Replace("file:///", "")
					}
				}, null, new[] {"Test"}, null,
			                               BuildRequestDataFlags.None);
			ILogger l = new ConsoleLogger();
			bp.Loggers = new[] {l};
			man.Build(bp, brd);
			Assert.True(File.Exists(Path.Combine(Tmpdir, "A.cs")));
			Assert.True(File.Exists(Path.Combine(Tmpdir, "B.cs")));
			Assert.True(File.Exists(Path.Combine(Tmpdir, "C.cs")));

			Assert.AreEqual("namespace X{ public partial class A{}}", File.ReadAllText(Path.Combine(Tmpdir, "A.cs")));
		}
	}
}
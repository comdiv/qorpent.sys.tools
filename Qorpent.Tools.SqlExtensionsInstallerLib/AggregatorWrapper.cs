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
// Original file : AggregatorWrapper.cs
// Project: Qorpent.Tools.SqlExtensionsInstallerLib
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;
using Microsoft.SqlServer.Server;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.SqlExtensionsInstallerLib {
	internal class AggregatorWrapper : SqlExportMemberWrapper {
		public AggregatorWrapper(SqlUserDefinedAggregateAttribute functiondef, Type aggregatesyg, string schema)
			: base(aggregatesyg, schema) {
			_functiondef = functiondef;
		}

		public override string GetObjectName() {
			if (_functiondef.Name.IsEmpty()) {
				return QueryGeneratorHelper.GetSafeSqlName(_info.Name);
			}
			return QueryGeneratorHelper.GetSafeSqlName(_functiondef.Name);
		}

		public override string GetObjectType() {
			return "AGGREGATE";
		}

		public override string GetCreateScript() {
			throw new NotImplementedException();
		}

		private readonly SqlUserDefinedAggregateAttribute _functiondef;
	}
}
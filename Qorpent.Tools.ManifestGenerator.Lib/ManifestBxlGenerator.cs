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
// Original file : ManifestBxlGenerator.cs
// Project: Qorpent.Tools.ManifestGenerator.Lib
// 
// ALL MODIFICATIONS MADE TO FILE MUST BE DOCUMENTED IN SVN

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Qorpent.Bxl;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.ManifestGenerator.Lib {
	/// <summary>
	/// 	generates bxl code for assemblie's container export manifests
	/// </summary>
	public class ManifestBxlGenerator {
		/// <summary>
		/// </summary>
		public ManifestBxlGenerator() {
			NeedExportAttribute = true;
		}

		/// <summary>
		/// 	Требовать наличие атрибута <see cref="ContainerExportAttribute" />
		/// </summary>
		public bool NeedExportAttribute { get; set; }

		/// <summary>
		/// 	generates manifest for all libraries in given directory, if no given - environment.currentdir used
		/// </summary>
		/// <param name="options"> </param>
		/// <returns> </returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public string GenerateManifest(ManifestBxlGeneratorOptions options = null) {
			options = options ?? new ManifestBxlGeneratorOptions();
			if (options.LibraryDirectory.IsEmpty()) {
				options.LibraryDirectory = Environment.CurrentDirectory;
			}

			IList<string> assemblyfiles = null;
			if (options.LibraryName.IsEmpty()) {
				assemblyfiles = Directory.GetFiles(options.LibraryDirectory, "*.dll");
				if (options.ExcludeLibRegex.IsNotEmpty()) {
					assemblyfiles =
						assemblyfiles.Where(x => !Regex.IsMatch(Path.GetFileNameWithoutExtension(x), options.ExcludeLibRegex)).ToArray();
				}
			}
			else {
				assemblyfiles = new List<string>(new[] {options.LibraryName});
				NeedExportAttribute = false;
			}
			IList<Assembly> assembliestoconfigure = new List<Assembly>();
			foreach (var assemblyfile in assemblyfiles) {
				if (Path.GetFileNameWithoutExtension(assemblyfile) == "Qorpent.Core") {
					continue;
				}
				assembliestoconfigure.Add(Assembly.Load(File.ReadAllBytes(assemblyfile),File.ReadAllBytes(assemblyfile.Replace(".dll",".pdb"))));
			}
			return GenerateManifest(assembliestoconfigure.ToArray(), options);
		}

		/// <summary>
		/// 	generates single bxl string from assembly manifests
		/// </summary>
		/// <param name="assemblies"> </param>
		/// <param name="options"> </param>
		/// <returns> </returns>
		public string GenerateManifest(Assembly[] assemblies, ManifestBxlGeneratorOptions options = null) {
			if (assemblies == null) {
				throw new ArgumentNullException("assemblies");
			}
			if (0 == assemblies.Length) {
				throw new ArgumentException("no assemblies given");
			}
			var gen = new BxlGenerator();
			var genopts = new BxlGeneratorOptions
				{
					NoRootElement = true,
					UseTrippleQuotOnValues = false,
					InlineAlwaysAttributes = new[] {"code", "name", "priority"}
				};
			options = options ?? new ManifestBxlGeneratorOptions();
			var xml = new XElement("root");
			IList<IComponentDefinition> componentsToGenerate = new List<IComponentDefinition>();
			foreach (var assembly in assemblies.Distinct()) {
				Console.WriteLine("Enter: " + assembly.GetName().Name);
				var def = new AssemblyManifestDefinition(assembly, NeedExportAttribute);

				if (def.Descriptor != null) {
					Console.WriteLine("IsConfigurable: " + assembly.GetName().Name);
					foreach (var classDefinition in def.ComponentDefinitions) {
						componentsToGenerate.Add(classDefinition.GetComponent());
					}
				}
			}
			var useshortnames = false;
			if (options.UseShortNames) {
				useshortnames = CanUseShortNames(componentsToGenerate);
			}
			if (useshortnames) {
				foreach (var assembly in assemblies) {
					var def = new AssemblyManifestDefinition(assembly, NeedExportAttribute);
					if (def.Descriptor != null) {
						xml.Add(new XElement("ref", new XAttribute("code", assembly.GetName().Name)));
					}
				}
				var namespaces =
					componentsToGenerate.Select(x => x.ServiceType).Union(componentsToGenerate.Select(x => x.ImplementationType))
						.Distinct().Select(x => x.Namespace).Distinct();
				foreach (var ns in namespaces) {
					xml.Add(new XElement("using", new XAttribute("code", ns)));
				}
			}
			foreach (var definition in componentsToGenerate) {
				var elementname = definition.Lifestyle.ToString().ToLowerInvariant();
				var value = GetTypeName(definition.ServiceType, useshortnames);
				var e = new XElement(elementname, value);
				if (definition.Name.IsEmpty()) {
					e.Add(new XAttribute("code", GetTypeName(definition.ImplementationType, useshortnames)));
				}
				else {
					e.Add(new XAttribute("code", definition.Name));
					e.Add(new XAttribute("name", GetTypeName(definition.ImplementationType, useshortnames)));
				}
				e.Add(new XAttribute("priority", definition.Priority));
				if (definition.Role.IsNotEmpty()) {
					e.Add(new XAttribute("role", definition.Role));
				}
				if (definition.Help.IsNotEmpty()) {
					e.Add(new XAttribute("help", definition.Help));
				}
				xml.Add(e);
			}

			var result = gen.Convert(xml, genopts);
			return result;
		}

		private string GetTypeName(Type type, bool useshortnames) {
			return useshortnames ? type.Name : string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
		}

		private bool CanUseShortNames(IEnumerable<IComponentDefinition> defs) {
			var alltypes = defs.Select(x => x.ServiceType).Union(defs.Select(x => x.ImplementationType)).Distinct();
			return
				!alltypes.Join(alltypes, x => x.Name, y => y.Name, (x, y) => new {x, y})
					 .Any(u => u.x != u.y);
		}
	}
}
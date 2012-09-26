using System;
using System.IO;
using Microsoft.Build.Utilities;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.MsBuildTasks {
	/// <summary>
	/// Выполняет генерацию документации для указанной сборки (используя сконфигурированный NDoc3)
	/// </summary>
	public class QorpentDoc: Task {
		public override bool Execute() {
			Log.LogMessage("Start to generate documentation");
			if(LibraryPath.IsEmpty()) {
				Log.LogWarning("Library not choosed");
				return true;
			}
			LibraryPath = Path.GetFullPath(LibraryPath);
			
			

			if(OutputPath.IsEmpty()) {
				OutputPath = Path.Combine(Path.GetDirectoryName(LibraryPath), "doc");
			}
			Directory.CreateDirectory(OutputPath);

			throw new NotImplementedException();
		}

		/// <summary>
		/// Путь к файлу Xml 
		/// </summary>
		public string XmlPath { get; set; }

		/// <summary>
		/// Путь к сборке для документирования
		/// </summary>
		public string LibraryPath { get; set; }
		/// <summary>
		/// Путь к папке для результатов
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// Путь к папке, содержащей NDoc
		/// </summary>
		public string NDocPath { get; set; }


	}
}
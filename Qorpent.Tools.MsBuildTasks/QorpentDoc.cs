using System;
using System.IO;
using Microsoft.Build.Utilities;
using Qorpent.Utils.Extensions;

namespace Qorpent.Tools.MsBuildTasks {
	/// <summary>
	/// ��������� ��������� ������������ ��� ��������� ������ (��������� ������������������ NDoc3)
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
		/// ���� � ����� Xml 
		/// </summary>
		public string XmlPath { get; set; }

		/// <summary>
		/// ���� � ������ ��� ����������������
		/// </summary>
		public string LibraryPath { get; set; }
		/// <summary>
		/// ���� � ����� ��� �����������
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// ���� � �����, ���������� NDoc
		/// </summary>
		public string NDocPath { get; set; }


	}
}
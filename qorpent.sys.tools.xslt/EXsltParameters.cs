using Qorpent.Utils.Extensions;

namespace qorpent.sys.tools.xslt {
	/// <summary>
	/// ��������� ���������
	/// </summary>
	public class EXsltParameters {
		private string _src;
		private string _out;
		private string _trs;

		/// <summary>
		/// �������� ����
		/// </summary>
		public string Src {
			get { return _src.IsEmpty()?Arg1:_src; }
			set { _src = value; }
		}

		/// <summary>
		/// �������� ����
		/// </summary>
		public string Out {
			get { return _out.IsEmpty()?Arg3:_out; }
			set { _out = value; }
		}

		/// <summary>
		/// ���� �������������
		/// </summary>
		public string Trs {
			get { return _trs.IsEmpty()?Arg2:_trs; }
			set { _trs = value; }
		}

		/// <summary>
		/// ������ ����� ����� Src
		/// </summary>
		public string Arg1 { get; set; }

		/// <summary>
		/// ������ ����� Trs
		/// </summary>
		public string Arg2 { get; set; }

		/// <summary>
		/// ������ ����� Out
		/// </summary>
		public string Arg3 { get; set; }
	}
}
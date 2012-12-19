using Qorpent.Utils.Extensions;

namespace qorpent.sys.tools.xslt {
	/// <summary>
	/// Параметры программы
	/// </summary>
	public class EXsltParameters {
		private string _src;
		private string _out;
		private string _trs;

		/// <summary>
		/// Исходный файл
		/// </summary>
		public string Src {
			get { return _src.IsEmpty()?Arg1:_src; }
			set { _src = value; }
		}

		/// <summary>
		/// Выходной файл
		/// </summary>
		public string Out {
			get { return _out.IsEmpty()?Arg3:_out; }
			set { _out = value; }
		}

		/// <summary>
		/// Файл трансформации
		/// </summary>
		public string Trs {
			get { return _trs.IsEmpty()?Arg2:_trs; }
			set { _trs = value; }
		}

		/// <summary>
		/// Замена имени файла Src
		/// </summary>
		public string Arg1 { get; set; }

		/// <summary>
		/// Замена файла Trs
		/// </summary>
		public string Arg2 { get; set; }

		/// <summary>
		/// Замена файла Out
		/// </summary>
		public string Arg3 { get; set; }
	}
}
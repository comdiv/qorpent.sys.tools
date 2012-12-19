using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Utils.Extensions;

namespace qorpent.sys.tools.xslt
{
	/// <summary>
	/// Выполняет расширенное преобразование EXSLT
	/// </summary>
	public static class Program
	{	
		/// <summary>
		/// Выполняет работу по преобразованию XSLT
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args) {
			var opts = new Qorpent.Utils.ConsoleArgumentHelper().Parse<EXsltParameters>(args);
			if(opts.Src.IsEmpty()) {
				throw new Exception("source file not defined");
			}
			if(opts.Out.IsEmpty()) {
				opts.Out = opts.Src + ".result";
			}
			var h = new Qorpent.Dsl.SmartXslt.XsltHelper();
			var sw = new StringWriter();
			h.Process(opts.Src,sw,opts.Trs);
			var content = sw.ToString();
			var writer = new Qorpent.Dsl.FileSplitWriter();
			writer.WriteContent(opts.Out, content);
		}
	}
}

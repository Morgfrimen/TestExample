using System;
using System.IO;
using System.Text;

using ParserFile;

using static System.Text.Encoding;

namespace TestExample
{
	internal class Program
	{
		#region Constructors

		static Program() => RegisterProvider(CodePagesEncodingProvider.Instance);

#endregion

		#region Methods

		private static void Main(string[] args)
		{
			Console.WriteLine("Начало работы");
			_ = ParserXML.GetInstance(Path.Combine(Environment.CurrentDirectory, "Example.xml")).GetJsonFile();
			Console.WriteLine("Конец работы, нажми любую кнопку");
			_ = Console.ReadKey();
		}

		#endregion
	}
}
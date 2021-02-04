using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;

namespace ParserFile.Models
{
	public class VoltageLevel
	{
		#region Static Fields and Constants

		private static string Name;

		#endregion

		#region Properties

		/// <summary>
		///     Идентификатор
		/// </summary>
		[NotNull]
		[JsonIgnore]
		public string GuidElements { get; set; }

		/// <summary>
		///     Название распределительное устройства
		/// </summary>
		[JsonProperty("Название распределительное устройства")]
		public string NameVoltageLevel { get; set; }

		[JsonProperty("Список генераторов")]
		public IList<SynchronousMachine> SynchronousMachines { get; set; }

		#endregion
	}
}
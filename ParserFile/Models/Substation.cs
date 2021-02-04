using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;

namespace ParserFile.Models
{
	public class Substation
	{
		#region Properties

		/// <summary>
		///     Идентификатор
		/// </summary>
		[NotNull]
		[JsonIgnore]
		public string GuidElements { get; set; }

		/// <summary>
		///     Название подстанции
		/// </summary>
		[JsonProperty("Название подстанции")]
		public string NameSubstation { get; set; }

		[JsonProperty("Распределительное устройства")]
		public IList<VoltageLevel> VoltageLevels { get; set; }

		#endregion
	}
}
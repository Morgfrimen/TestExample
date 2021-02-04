using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;

namespace ParserFile.Models
{
	public class SynchronousMachine
	{
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
		[JsonProperty("Название генератора")]
		public string NameVoltageLevel { get; set; }

		#endregion
	}
}
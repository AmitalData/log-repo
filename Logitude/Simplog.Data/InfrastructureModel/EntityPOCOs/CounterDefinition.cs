using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
	public class CounterDefinition
	{
		[Key]
		public string Id { get; set; }

		public int Tenant { get; set; }

		public string Parameter1 { get; set; }

		public string Parameter2 { get; set; }

		public string Prefix { get; set; }

		public bool UniquePerPrefix { get; set; }

		public string CounterId { get; set; }

		public int StartNumber { get; set; }

		public int? CounterSize { get; set; }

		public string Suffix { get; set; }

		//[Include]
		//[Association("CounterCounterDefinition", "CounterId", "Id", IsForeignKey = true)]
		[ForeignKey("CounterId")]
		public virtual Counter Counter { get; set; }
	}
}
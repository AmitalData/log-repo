using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class TMEmployeeTimePM
    {
		public string Id { get; set; }
		public int Tenant { get; set; }
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public string WINumber { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeUserId { get; set; }
        public int TimeInMinutes { get; set; }
        public DateTime DateOfWork { get; set; }
        public string SprintId { get; set; }

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ParticipantsUsersActivitiesDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string SelectedParticipantName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<ActiveParticipantRecord> ActiveParticipantsList { get; set; }
    }

    public class ActiveParticipantRecord
    {
        [Key]
        public string Id { get; set; }
        public string ParticipantName { get; set; }
        public string UserFullName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string MessageType { get; set; }
        public int TotalTransmissionPerFWB { get; set; }
        public int TotalTransmissionPerFHL { get; set; }
        public int TotalTransmissionPerFFR { get; set; }
        public int TotalTransmissionPerFVR { get; set; }
        public int TotalTransmissionPerFSR { get; set; }
    }
}
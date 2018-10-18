using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TarrifHeaderPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string TarrifTypeCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public string TransitTimeNotes { get; set; }
        public DateTime CreateDate { get; set; }

        // Dummy
        public string FromLocationCode { get; set; }
        public string ToLocationCode { get; set; }
        public string FromLocationString { get; set; }
        public string ToLocationString { get; set; }

        private List<string> fromLocationList;
        public List<string> FromLocationList 
        {
            get
            {
                if (fromLocationList == null)
                {
                    fromLocationList = new List<string>();
                }

                return fromLocationList;
            }

            set 
            { 
                fromLocationList = value;
            }
        }

        private List<string> toLocationList;
        public List<string> ToLocationList 
        {
            get
            {
                if (toLocationList == null)
                {
                    toLocationList = new List<string>();
                }

                return toLocationList;
            }

            set
            {
                toLocationList = value;
            }
        }

        private List<TarrifChargePM> tarrifCharges;
        [Composition]
        [Include]
        [Association("TarrifHeaderPMTarrifChargePM", "Id", "TarrifHeaderId")]
        public virtual List<TarrifChargePM> TarrifCharges
        {
            get
            {
                if (tarrifCharges == null)
                {
                    tarrifCharges = new List<TarrifChargePM>();
                }
                return tarrifCharges;
            }
            set { tarrifCharges = value; }
        }

        //private List<TarrifStepPM> tarrifSteps;
        //[Composition]
        //[Include]
        //[Association("TarrifHeaderPMTarrifStepPM", "Id", "TarrifHeaderId")]
        //public virtual List<TarrifStepPM> TarrifSteps
        //{
        //    get
        //    {
        //        if (tarrifSteps == null)
        //        {
        //            tarrifSteps = new List<TarrifStepPM>();
        //        }
        //        return tarrifSteps;
        //    }
        //    set { tarrifSteps = value; }
        //}

        private List<TarrifFromToPM> tarrifFromToes;
        [Composition]
        [Include]
        [Association("TarrifHeaderPMTarrifFromToPM", "Id", "TarrifHeaderId")]
        public virtual List<TarrifFromToPM> TarrifFromToes
        {
            get
            {
                if (tarrifFromToes == null)
                {
                    tarrifFromToes = new List<TarrifFromToPM>();
                }
                return tarrifFromToes;
            }
            set { tarrifFromToes = value; }
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class MeasurementList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool InActive { get; set; }
        public bool IsContainerMeasurement { get; set; }
        public bool IsContainer { get; set; }
        public string SearchFields { get; set; }
        public string LocalName { get; set; }
    }
}
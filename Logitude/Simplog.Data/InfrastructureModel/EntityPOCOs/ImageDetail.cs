using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ImageDetail
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Extension { get; set; }
        public double Size { get; set; }

       // public List<Contact> Contacts { get; set; }
      //  public List<Card> Cards { get; set; }
    }
}
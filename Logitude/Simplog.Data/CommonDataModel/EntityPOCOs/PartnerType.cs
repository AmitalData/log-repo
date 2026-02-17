using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class PartnerType
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<Card> Cards { get; set; }
        //public List<Conversation> FromConversations { get; set; }
        //public List<Conversation> ToConversations { get; set; }
       


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTemplateTextDesignPM
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double FontSize { get; set; }
        public string TextColor { get; set; }
        public string FontFamily { get; set; }
        public string BackgroundColor { get; set; }
        public string FontWeight { get; set; }
        public bool Italic { get; set; }
        public bool UnDerLine { get; set; }
        public string Alignment { get; set; }
 
    }
}


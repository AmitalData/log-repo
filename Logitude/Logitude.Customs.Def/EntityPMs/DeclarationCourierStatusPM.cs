using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools;
using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Runtime.Serialization;
using Logitude.Customs.Def.Validators;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class DeclarationCourierStatusPM : EntityPM
    {
        public bool EdgeManifest { get; set; }
        public bool EdgeDeclaration { get; set; }
        public bool EdgePayment { get; set; }
        public bool IsNewEntity { get; set; }
    }
   
}

using Logitude.Server.Tools.Models;
using System;
namespace Logitude.Customs.BL.Messaging.Amital
{
    public class AmitalCommunicationModelBase : AmitalStandardCommunicationModel
        
    {
        public AmitalCommunicationModelBase(OperationMethod UnifaceMethodType, string UnifaceComponentName, string UnifaceOperation)
            :base(UnifaceMethodType, UnifaceComponentName, UnifaceOperation) 
        {}
        
        public string CommunicationSubject { get; set; }

        public string CommunicationLoggingEntityReference { get; set; }
        public string EntityId { get; set; }
        public string objectTableName { get; set; }


        public int Tenant { get; set; }
        public string UserId { get; set; }

        public AmitalMessaging.Infrastructure.Transmission.special_instructions special_instruction { get; set; }
        //public string UnifreightUserId { get; set; } //$$GSC_USER_ID

    }
}

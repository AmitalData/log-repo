using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.Def.Validators;
using Logitude.Server.Tools;
using Logitude.Server.Tools.ExternalServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
      [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
    public partial class InterfaceManagementPM : EntityPM
    {
          [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
          [DataMember]
          public int Tenant { get; set; }

          

#if false
          

          private string RequestCode
          {
              get
              {
                  //will be ResponseCode
                  if (Interactive == InteractiveMode.DCABatchIn)
                  {
                      switch (this.Code)
                      {
                          case "3050": //C:\SendTSH_MSG3050_PaymentOrderReply_Out.IL941079089.2014-06-29_14-59-48-815.87dee5e0-7857-479a-891b-4d5d5d2ee4a9.TST.xml
                          case "2": //TSH_MSG2_PaymentOrderReply
                              return "3053"; ///TSH_NG_3053_MSG8_AgentPaymentRequestMessageService
                          case "190":
                              return null;
                          case "103":
                              return "101";

                          case "2754":
                          case "10004":
                              return "10000";
                          case "052":
                              return "051";
                          default:
                              break;
                      }
                  }
                  return null;
              }
          } 
          public bool ToSign { get; set; }// will be IsSigned
#endif
          public SignQueueByType SignatureBy
          {
              get
              {
                  if (this.signatureTypeCode == null)
                  {
                      return SignQueueByType.None;
                  }
                  switch (this.signatureTypeCode.ToUpper() )
                  {
                      case "p":
                      case "P":
                          return SignQueueByType.SignQueueByPersonId;
                          break;
                      case "c":
                      case "C":
                          return SignQueueByType.SignQueueByCustomsAgentId;
                      default:
                          return SignQueueByType.None;
                          break;
                  }
              }
          }

        //[Flags] ///Indicates that an enumeration can be treated as a bit field; that is, a set !!!!!!!
          


          public InOutType INOUT
          {
              get
              {

                  if (this.InOut == "I")
                  {
                      return InOutType.In;
                  }
                  if (this.InOut == "O")
                  {
                      return InOutType.Out;
                  }
                  return InOutType.none;
              }
          }

          private  InteractiveMode Interactive
          {
              get
              {

                  this.DefaultSendOptionsCode = this.DefaultSendOptionsCode ?? "";//mohammad :this code must change to fit the new adjustments to interface,i put DefaultSendOptionsCode instead of SendOptionsCode
                  if (this.INOUT == InOutType.Out)
                  {
                      //InterfaceSendOptions
                      //D	DCA Out
                      //WB	Batch WS
                      //WI	Interactive WS


                      //if (this.DefaultSendOptionsCode.StartsWith("D"))
                      if (this.DefaultSendOptionsCode == InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString())
                      {
                          return InteractiveMode.DCABatchOutIn;
                      }

                      //if (this.DefaultSendOptionsCode.StartsWith("WB"))
                      if (this.DefaultSendOptionsCode == InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WB.ToString())
                      {
                          return InteractiveMode.WebServiceBatch;
                      }
                      //if (this.DefaultSendOptionsCode.StartsWith("WI"))
                      if (this.DefaultSendOptionsCode == InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString())
                      {
                          return InteractiveMode.WebServiceInteractive;
                      }
                  }
                  else if (this.INOUT == InOutType.In)
                  {
                      return InteractiveMode.DCABatchIn;
                  }
                  return InteractiveMode.WebServiceInteractive;
              }
          }


#if false
          public string  RenameFilePrefixFake { get; set; }
          public bool OurEnvironmentCheckFake
          {
              get
              {
                  return (this.Code == "8348");
              }
          }
#endif

    }


}

using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnifreightIIG.Common.MessageLib.ID;
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;
using Exception = UnifreightIIG.Common.MessageLib.ID.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
   public class Fake_DCAInCH_NG_5117_MSG14003_Amendment_Service : Fake_ImportDeclaration_Response
    {
        Declaration dec;
        ResponseContentHeader _header;

        public Fake_DCAInCH_NG_5117_MSG14003_Amendment_Service(GenericRequestParams requestParams) : base(requestParams) { }

        public DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            
             dynamic data = JObject.Parse(requestParamsData.TestCase.Param1);

             string requestNumber = data.RequestNumber;

            _header = new ResponseContentHeader();
            DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg response = new DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg();
            UpdateDeclaration();
            AddResponseHeader();
              dec = new Declaration();

       

              CastObject(fakeRespond.Response.Declaration, dec);
            response.Response = new Response
            {
                Declaration = dec
            };
            response.ResponseContentHeader = new ResponseContentHeader();
            AddResponseContentHeader();
            response.ResponseContentHeader = _header;
            List<ResponseAdditionalInformation> AdditionalInformation = new List< ResponseAdditionalInformation>();

            if (!string.IsNullOrEmpty(data.Content29.ToString()))
            {
                AdditionalInformation.Add(new ResponseAdditionalInformation
                {
                    StatementTypeCode = new AdditionalInformationStatementTypeCodeType()
                    {
                        Value = "29"
                    },
                    Content = new AdditionalInformationContentTextType() { Value = data.Content29 }
                }
            );
}

            if (!string.IsNullOrEmpty(data.Content27.ToString()))
            {
                AdditionalInformation.Add(new ResponseAdditionalInformation
                {
                    StatementTypeCode = new AdditionalInformationStatementTypeCodeType()
                    {
                        Value = "27"
                    },
                    Content = new AdditionalInformationContentTextType() { Value = data.Content27 }
                });
            }

            if (!string.IsNullOrEmpty(data.Content32.ToString()))
            {
                AdditionalInformation.Add(new ResponseAdditionalInformation
                {
                    StatementTypeCode = new AdditionalInformationStatementTypeCodeType()
                    {
                        Value = "32"
                    },
                    Content = new AdditionalInformationContentTextType() { Value = data.Content32 }
                });
                
            }

             response.Response.AdditionalInformation = AdditionalInformation.ToArray();
            response.Response.FunctionCode = new ResponseFunctionCodeType() { Value = "Amendment" };
            response.Response.IssueDateTime = XmlConvert.ToString(DateTime.Now);
            //response.Response.Amendment = new ResponseAmendment[1]; // reason to change?
            //response.Response.Amendment[0] = new ResponseAmendment
            //{

            //}
            response.Response.FunctionalReferenceID = new ResponseFunctionalReferenceIDType() { Value = requestNumber };


            if (data.error == "true")
            {
                response.Response.Error = new ResponseError[1];
                response.Response.Error[0] = new ResponseError
                {
                    ValidationCode = new ErrorValidationCodeType { name = "test error", listVersionID = "4", Value = "2584" },
                    Pointer = new ResponseErrorPointer[1]
                    { new ResponseErrorPointer {
                    DocumentSectionCode = new PointerDocumentSectionCodeType { Value = "42A" },
                SequenceNumeric = 0}

                    }




                };

            }


            if (data.constrain == "true")
            {
                response.Response.Error = new ResponseError[1];
                response.Response.Error[0] = new ResponseError
                {
                    ValidationCode = new ErrorValidationCodeType { name = "test constrain", listVersionID = "4", Value = "2584" , listName= "105" },
                    Pointer = new ResponseErrorPointer[1]
                    { new ResponseErrorPointer {
                        DocumentSectionCode = new PointerDocumentSectionCodeType { Value = "42A" },
                        SequenceNumeric = 0 }

                    }, DMExtensions = new ResponseErrorDMExtensions
                    {
                        
                            ConstraintID=99 , ConstraintStatus=2 , ConstraintType=1
 
                    }




                };

            }
            response.Response.Status = new ResponseStatus() { EffectiveDateTime = DateTime.Now.ToString() };
            response.Response.Status.NameCode = new StatusNameCodeType() { Value = data.status };




            //return new CH_NG_190_MSG1_NoticeToClient()
            //{
            //    RequestContentHeader = new RequestContentHeader()
            //    {
            //        TransmitionDateTime = DateTime.Now
            //    },
            //    NoticeToClient = new CH_NG_190_MSG1_NoticeToClientNoticeToClient()
            //    {
            //        operationCode = 1,
            //        statusMessage = 2,
            //        checkId = 2369229,
            //        entityType = 5,
            //        customsAgent = 1111,
            //        importerNumber = 111,
            //        storageSiteNumber = "ILMMN",
            //        checkSiteNumber = "10470",
            //        openDate = DateTime.Now,
            //        CheckType = 1,
            //        declarationID = requestParamsData.AppicationId,///change to number 


            //    },
            //    CheckEntity = new CH_NG_190_MSG1_NoticeToClientCheckEntity()
            //    {
            //        cargoIdentifier = new cargoIdentifier()
            //        {
            //            cargoIdentifierKey1 = "22",
            //            cargoIdentifierType = 1
            //        }

            //    },
            //    SplitCargoIdentifier = new CH_NG_190_MSG1_NoticeToClientSplitCargoIdentifier[]{
            //          new CH_NG_190_MSG1_NoticeToClientSplitCargoIdentifier()
            //      {
            //           cargoIdentifier= new cargoIdentifier()
            //           {
            //                cargoIdentifierType= 27 ,
            //                 cargoIdentifierKey1= "50497355"
            //           }
            //      }
            //      }

            //};

            return response;
        }


        //public void CastDeclaration()
        //{
        //    Type objectType = fakeRespond.Response.Declaration.GetType();
        //    Type target = dec.GetType();
        //    var x = Activator.CreateInstance(target, false);
        //    var z = from source in objectType.GetMembers().ToList()
        //            where source.MemberType == MemberTypes.Property
        //            select source;
        //    var d = from source in target.GetMembers().ToList()
        //            where source.MemberType == MemberTypes.Property
        //            select source;
        //    List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
        //       .ToList().Contains(memberInfo.Name)).ToList();
        //    PropertyInfo propertyInfo;
        //    object value;
        //    foreach (var memberInfo in members)
        //    {
        //        propertyInfo = dec.GetType().GetProperty(memberInfo.Name);
        //        value = fakeRespond.Response.Declaration.GetType().GetProperty(memberInfo.Name).GetValue(fakeRespond.Response.Declaration, null);

        //        propertyInfo.SetValue(x, value, null);
        //    }
        //}


        public void CastObject(object originObject , object targetObject)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(fakeRespond.Response.Declaration.GetType());
                serializer.Serialize(stringwriter, fakeRespond.Response.Declaration);
                DeclarationString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(dec.GetType());
                dec = serializer.Deserialize(stringReader) as UnifreightIIG.Common.MessageLib.ID.Declaration;
            }

            //Type objectType = originObject.GetType();
            //Type target = targetObject.GetType();
            //var x = Activator.CreateInstance(target, false);
            //var z = from source in objectType.GetMembers().ToList()
            //        where source.MemberType == MemberTypes.Property
            //        select source;
            //var d = from source in target.GetMembers().ToList()
            //        where source.MemberType == MemberTypes.Property
            //        select source;
            //List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
            //   .ToList().Contains(memberInfo.Name)).ToList();
            //PropertyInfo propertyInfo;
            //object value;
            //foreach (var memberInfo in members)
            //{
            //    propertyInfo = targetObject.GetType().GetProperty(memberInfo.Name);
            //    if (propertyInfo.PropertyType.Name == "String")
            //    {
            //        value = originObject.GetType().GetProperty(memberInfo.Name).GetValue(originObject, null);

            //        propertyInfo.SetValue(x, value, null);
            //    }

            //    else
            //    {
            //        value = originObject.GetType().GetProperty(memberInfo.Name).GetValue(originObject, null);
            //        var test = Activator.CreateInstance(originObject.GetType().GetProperty(memberInfo.Name).GetType());
            //        CastObject(value,test);
            //    }
            //}
        }


        public void AddResponseContentHeader()
        {
            Exception[] exception = new Exception[1];
            exception[0] = new Exception // שגיאות
            {
                ExeptionDescription = "testing"
            };
            _header = new ResponseContentHeader()
            {
                TransmitionDateTime = DateTime.Now,
                Remark = "",
                Exception = null,
            };
        }


    }
}

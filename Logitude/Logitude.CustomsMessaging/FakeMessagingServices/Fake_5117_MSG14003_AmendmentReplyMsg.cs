using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.ID;
using Exception = UnifreightIIG.Common.MessageLib.ID.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
   public class Fake_5117_MSG14003_AmendmentReplyMsg : Fake_ImportDeclaration_Response
    {
        public Declaration dec;
        public ResponseContentHeader _header;
        public Fake_5117_MSG14003_AmendmentReplyMsg(GenericRequestParams requestParams) : base(requestParams) { }

        // return ??? 
        public void CallWS(out DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg response)
        {
            _header = new ResponseContentHeader();
            response = new DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg();
            UpdateDeclaration();
            AddResponseHeader();
            dec = new Declaration();
            CastDeclaration();
            response.Response = new Response
            {
                Declaration = dec
            };
            response.ResponseContentHeader = new ResponseContentHeader();
            AddResponseContentHeader();
            response.ResponseContentHeader = _header;
            ResponseAdditionalInformation[] AdditionalInformation = new ResponseAdditionalInformation[3];
            AdditionalInformation[0].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "29" };
            AdditionalInformation[0].Content = new AdditionalInformationContentTextType() { Value = "t29" };
            AdditionalInformation[1].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "27" };
            AdditionalInformation[1].Content = new AdditionalInformationContentTextType() { Value = "t27" };
            AdditionalInformation[2].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "32" };
            AdditionalInformation[2].Content = new AdditionalInformationContentTextType() { Value = "1" };
            response.Response.AdditionalInformation = AdditionalInformation;
            response.Response.FunctionCode = new ResponseFunctionCodeType(){ Value = "Amendment" };
            response.Response.IssueDateTime = DateTime.Now.ToString();
            response.Response.Amendment = new ResponseAmendment[1]; // reason to change?
            response.Response.Status = new ResponseStatus() { EffectiveDateTime = DateTime.Now.ToString()};
            response.Response.Status.NameCode = new StatusNameCodeType() { Value = "5" };






        }
        public void CastDeclaration()
        {
            Type objectType = fakeRespond.GetType();
            Type target = dec.GetType();
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = dec.GetType().GetProperty(memberInfo.Name);
                value = objectType.GetType().GetProperty(memberInfo.Name).GetValue(objectType, null);

                propertyInfo.SetValue(x, value, null);
            }
        }
        public void AddResponseContentHeader()
        {
            Exception[] exception = new Exception[1];
            exception[0] = new Exception // שגיאות
            {
                ExeptionDescription = "testing"
            };
            _header= new ResponseContentHeader()
            {
                TransmitionDateTime = DateTime.Now,
                Remark = "",
                Exception = null,
            };
        }
        
    }
}

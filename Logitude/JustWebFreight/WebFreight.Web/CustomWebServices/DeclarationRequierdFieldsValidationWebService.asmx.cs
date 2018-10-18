using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Logitude.Customs.BL.Validators;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for DeclarationRequierdFieldsValidationWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeclarationRequierdFieldsValidationWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte [] GetRequiredFieldsErrorsForDeclaration(string declarationId,int tenant)
        {
            CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationId, tenant);

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomsRequiredFieldErrors));
            ser.Serialize(memstream, errors);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] GetRequiredFieldsErrorsForDeclarationPayment(string declarationId, int tenant)
        {
            CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclarationPayment(declarationId, tenant);

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomsRequiredFieldErrors));
            ser.Serialize(memstream, errors);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] GetRequiredFieldsErrorsForClaim(string claimId, int tenant)
        {
            CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForClaim(claimId, tenant);

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomsRequiredFieldErrors));
            ser.Serialize(memstream, errors);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
    }
}

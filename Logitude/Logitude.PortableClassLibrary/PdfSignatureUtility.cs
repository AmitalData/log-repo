using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.PortableClassLibrary
{
    public class PdfSignatureUtility
    {
        public static bool GetSignatureMetadata(string filename, byte[] filedata, out List<Dictionary<string, string>> signatures, out string message)
        {
            signatures = new List<Dictionary<string, string>>();
            message = "";
            try
            {

                using (PdfReader reader = new PdfReader(filedata))
                {
                    AcroFields af = reader.AcroFields;
                    var names = af.GetSignatureNames();
                    for (int i = 0; i < names.Count; ++i)
                    {
                        Dictionary<string, string> metadata = new Dictionary<string, string>();
                        String name = (string)names[i];
                        PdfPKCS7 pk = af.VerifySignature(name);
                        //metadata.Add("Name", name);
                        //metadata.Add("SignName", pk.SignName);
                        metadata.Add("SignDate", pk.SignDate.ToString("dd.MM.yyyy HH:mi.ss"));
                        var subjectFields = CertificateInfo.GetSubjectFields(pk.SigningCertificate);
                        List<string> mdlist = new List<string>() { "C", "CN", "SN", "T", "OU", "O", "GIVENNAME", "SURNAME", };
                        if (subjectFields != null)
                        {
                            foreach (var md in mdlist)
                            {
                                string value = subjectFields.GetField(md);
                                if (!string.IsNullOrEmpty(value))
                                    metadata.Add(md, value);
                            }
                        }
                        signatures.Add(metadata);
                    }
                }
                return (true);
            }
            catch (Exception ex)
            {
                message = "Failed to get metadata from pdf file '" + filename + "'" + Environment.NewLine + ex.ToString();
                return (false);
            }
        }


    }
}

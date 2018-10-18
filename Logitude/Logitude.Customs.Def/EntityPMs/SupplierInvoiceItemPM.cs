using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class SupplierInvoiceItemPM
    {
        [DataMember]
        public int SupplierInvoiceItemsQuantityLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsConnectedDeclarationLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsModificationLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemTaxLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvioceItemsCertificatLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsSerialNumberLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsProductIdentificationLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsDescriptionLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsProcessTypeLastLineNumber { get; set; }
        [DataMember]
        public int SupplierInvoiceItemsLevyLastLineNumber { get; set; }
        [DataMember]
        public string SupplierInvoiceTaxesActiveIds { get; set; }
        [DataMember]
        public int SupplierInvoiceItemVehicleLastLineNumber { get; set; }

        public ulong GetHashCode4Accumulation(
            List<string> SIItemPOCOFields,
            List<string> SIItemCerPOCOFields
            )
        {

            ulong hash = 0;

            if (false)
            {
                hash = CreateHashCodeFromStream(SIItemPOCOFields, SIItemCerPOCOFields);
                return hash;
            }

            Type objType = this.GetType();

            unchecked
            {
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    if (SIItemPOCOFields.Contains(property.Name))
                    {
                        object value = property.GetValue(this, null);
                        if(value != null)hash ^= (uint)value.GetHashCode() * 397; ;
                    }

                }
                if (this.SupplierInvioceItemCertificats != null)
                {
                    this.SupplierInvioceItemCertificats.OrderBy(rec => rec.ReqConfirmationTypeCode).ToList().ForEach(
                        cer =>
                        {
                            foreach (PropertyInfo property in cer.GetType().GetProperties())
                            {
                                if (SIItemCerPOCOFields.Contains(property.Name))
                                {
                                    object value = property.GetValue(cer, null);
                                    if (value != null) hash ^= (uint)value.GetHashCode() * 397; ;
                                }

                            }
                        }
                    );
                }
            }

            return hash;

        }

        public ulong CreateHashCodeFromStream(List<string> SIItemPOCOFields,
            List<string> SIItemCerPOCOFields)
        {
            ulong hash = 0;
            Type objType = this.GetType();
            Stream tempstream = null;
            unchecked
            {
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    if (SIItemPOCOFields.Contains(property.Name))
                    {
                        object value = property.GetValue(this, null);
                        var s1 = GetStreamFromString(value.ToString());
                        tempstream = Append(tempstream, s1);
                        
                    }

                }
                this.SupplierInvioceItemCertificats.ForEach(
                    cer =>
                    {
                        foreach (PropertyInfo property in cer.GetType().GetProperties())
                        {
                            if (SIItemCerPOCOFields.Contains(property.Name))
                            {
                                object value = property.GetValue(this, null);
                                var s1 = GetStreamFromString(value.ToString());
                                tempstream = Append(tempstream, s1);
                                
                            }

                        }
                    }
                );
                hash ^= (uint)tempstream.GetHashCode() * 397; ;
            }

            return hash;
        }

        public static Stream GetStreamFromString(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public static Stream Append(Stream destination, Stream source)
        {
            destination.Position = destination.Length;
            source.CopyTo(destination);

            return destination;
        }



       
    }
}

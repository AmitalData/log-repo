using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.Server.Tools
{
    [DataContract]
    public class EntityPM
    {
        [DataMember]
        public ChangeSetOperation ChangeSetOp { get; set; }

        /*// ihab : if is on all NVARCHAR have to decode from Base64 in mapping
         *ל
OnPMToPOCOEncodeBase64NVARCHARFields= ""   //  like false 
OnPMToPOCOEncodeBase64NVARCHARFields= null   //  like false 


OnPMToPOCOEncodeBase64NVARCHARFields= "windows-1255"
OnPMToPOCOEncodeBase64NVARCHARFields= "utf-8"
OnPMToPOCOEncodeBase64NVARCHARFields= "iso-….."



         */
        [DataMember]
        public string EncodeBase64NVARCHARFieldsBy { get; set; }  

        public static bool SuppressCreateNotifyPropertyChangeValues = false;
        //on server only  !!! [DataMember]
        object _CurrentContextTag;
        public object CurrentContextTag // itzik Genric object reflect Context (pass Context  to onUpdating - UnifreightUpdate)
        {
            get { return _CurrentContextTag; }
            set { _CurrentContextTag = value; }
        }
        

        List<NotifyPropertyChangeValues> changedProperties;
        public List<NotifyPropertyChangeValues> ChangedProperties
        {
            get
            {
                if (changedProperties == null)
                {
                    if (SuppressCreateNotifyPropertyChangeValues) return null;
                    changedProperties = new List<NotifyPropertyChangeValues>();
                }
                return changedProperties;
            }
        }
        public void NotifyPropertyChanged(NotifyPropertyChangeValues values)
        {
            if (SuppressCreateNotifyPropertyChangeValues) return;
            values.Id = Guid.NewGuid().ToString();
            ChangedProperties.Add(values);
        }
    }

    public class NotifyPropertyChangeValues
    {
        [Key]
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string PropertyType { get; set; }
    }
}

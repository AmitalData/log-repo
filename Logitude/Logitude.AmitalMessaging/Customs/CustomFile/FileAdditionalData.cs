using System;
using Logitude.AmitalMessaging.Customs.CustomFile;
using System.Xml.Serialization;

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/FileAdditionalData")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/FileAdditionalData", IsNullable = false)]

public partial class FileAdditionalData
{
    private string hAWBField;

    private string mAWBField;

    /// <remarks/>
    public string HAWB
    {
        get
        {
            return this.hAWBField;
        }
        set
        {
            this.hAWBField = value;
        }
    }

    /// <remarks/>
    public string MAWB
    {
        get
        {
            return this.mAWBField;
        }
        set
        {
            this.mAWBField = value;
        }
    }
}

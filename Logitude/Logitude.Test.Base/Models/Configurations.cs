namespace Logitude.Test.Base.Models
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Configurations
    {

        private ConfigurationsGeneral generalField;

        private ConfigurationsUser[] usersField;

        public ConfigurationsGeneral General
        {
            get
            {
                return this.generalField;
            }
            set
            {
                this.generalField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayItemAttribute("User", IsNullable = false)]
        public ConfigurationsUser[] Users
        {
            get
            {
                return this.usersField;
            }
            set
            {
                this.usersField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationsGeneral
    {

        private string serverUrlField;

        public string ServerUrl
        {
            get
            {
                return this.serverUrlField;
            }
            set
            {
                this.serverUrlField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationsUser
    {

        private bool defaultField;

        private bool defaultFieldSpecified;

        private string emailField;

        private string passwordField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DefaultSpecified
        {
            get
            {
                return this.defaultFieldSpecified;
            }
            set
            {
                this.defaultFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
    }
}
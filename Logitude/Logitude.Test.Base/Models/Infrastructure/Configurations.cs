namespace Logitude.Test.Base.Models.Infrastructure
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Configurations
    {
        private ConfigurationsServerSettings serverSettingsField;

        private ConfigurationsUser[] usersField;

        public ConfigurationsServerSettings ServerSettings
        {
            get
            {
                return this.serverSettingsField;
            }
            set
            {
                this.serverSettingsField = value;
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
    public partial class ConfigurationsServerSettings
    {

        private string urlField;

        public string Url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
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
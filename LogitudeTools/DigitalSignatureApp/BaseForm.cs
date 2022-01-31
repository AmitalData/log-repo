using Cloud.Sign.App.EnvironmentSettings;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace Cloud.Sign.App
{
    public class BaseForm : Form
    {
        protected readonly string Environment;
        protected NotifyIcon notifyIcon;
        protected PictureBox pictureBox;
        protected Label label;
        protected readonly IEnvironmentSetting environmentSetting;
        private readonly string settingsNameSpace = "Cloud.Sign.App.EnvironmentSettings.Settings";
        public BaseForm()
        {
            this.Environment = ConfigurationManager.AppSettings["Environment"]?.ToString();
            environmentSetting = GetEnvironmentSettings();
        }

        private IEnvironmentSetting GetEnvironmentSettings()
        {
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
              .Where(p => typeof(IEnvironmentSetting).IsAssignableFrom(p) && p.Namespace.StartsWith(settingsNameSpace))
              .Select(x => Activator.CreateInstance(x) as IEnvironmentSetting)
              .FirstOrDefault(x => x.Environment == Environment);
            }
            catch (Exception)
            {
                MessageBox.Show("Please provide Environment Setting for " + Environment);
                return null;
            }

        }

        protected void InitView()
        {
            pictureBox.Image = environmentSetting.Image;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            label.Text = environmentSetting.LabelText;
            this.Text = environmentSetting.HeaderText;
            this.Icon = environmentSetting.Icon;
        }

        protected void SetBalloonTipText()
        {
            SetDefaultIcon();
            notifyIcon.BalloonTipText = environmentSetting.BalloonTipText;
        }

        protected void SetMinimizeScreenMessage()
        {
            notifyIcon.BalloonTipText = environmentSetting.BalloonTipText;
            notifyIcon.BalloonTipTitle = environmentSetting.BalloonTipTitle;
        }

        protected void SetDefaultIcon()
        {
            notifyIcon.Icon = environmentSetting.Icon;
        }

        protected void SetDefaultText()
        {
            notifyIcon.Text = environmentSetting.BalloonTipTitle;
        }

        protected void SetBalloonTipTitle()
        {
            notifyIcon.BalloonTipTitle = environmentSetting.BalloonTipTitle;
        }
        protected void SetInactiveIcon()
        {
            notifyIcon.Icon = environmentSetting.InactiveIcon;
        }
    }
}

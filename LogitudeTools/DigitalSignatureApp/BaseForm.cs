using Cloud.Sign.App.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace Cloud.Sign.App
{
    public class BaseForm : Form
    {
        protected readonly string Environment;
        protected NotifyIcon notifyIcon;
        protected PictureBox pictureBox;
        protected Label label;
        public BaseForm()
        {
            this.Environment = ConfigurationManager.AppSettings["Environment"]?.ToString();
        }

        protected void InitView(Type classType)
        {
            ComponentResourceManager resources = new ComponentResourceManager(classType);
            if (Environment == "DSV")
            {
                InitDSVView(resources);
                return;
            }
            if (Environment == "PL")
            {
                InitPrivateLabelView(resources);
                return;
            }
            InitLogboxView(resources);
        }

        protected void InitPrivateLabelView(ComponentResourceManager resources)
        {
            pictureBox.Image = Properties.Resources.PLogo;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            label.Text = "Sign Client";
            this.Text = "Sign App";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.PLIcon")));
        }

        protected void InitLogboxView(ComponentResourceManager resources)
        {
            pictureBox.Image = Properties.Resources.LogBox;
            label.Text = "LogBox Sign Client";
            this.Text = "LogBox Sign App";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        }

        protected void InitDSVView(ComponentResourceManager resources)
        {
            pictureBox.Image = Properties.Resources.HeaderLogo;
            label.Text = "DSV Sign Client";
            this.Text = "DSV Sign App";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.DsvIcon")));
        }

        protected void SetBalloonTipText()
        {
            SetDefaultIcon();
            if (Environment == "DSV")
            {
                notifyIcon.BalloonTipText = "You can access DSV sign application from here.";
                return;
            }
            if (Environment == "PL")
            {
                notifyIcon.BalloonTipText = "You can access sign application from here.";
                return;
            }
            notifyIcon.BalloonTipText = "You can access LogBox sign application from here.";
        }

        protected void SetMinimizeScreenMessage()
        {
            if (Environment == "DSV")
            {
                notifyIcon.BalloonTipTitle = "DSV Sign Application";
                notifyIcon.BalloonTipText = "You can access DSV sign application from here.";
                return;
            }
            if (Environment == "PL")
            {
                notifyIcon.BalloonTipTitle = "Sign Application";
                notifyIcon.BalloonTipText = "You can access sign application from here.";
                return;
            }
            notifyIcon.BalloonTipTitle = "LogBox Sign Application";
            notifyIcon.BalloonTipText = "You can access LogBox sign application from here.";
        }

        protected void SetDefaultIcon()
        {
            if (Environment == "DSV")
            {
                notifyIcon.Icon = Resources.dsv;
                return;
            }
            if (Environment == "PL")
            {
                notifyIcon.Icon = BitmapToIcon(Resources.PLogo);
                return;
            }
            notifyIcon.Icon = Resources.logboxicon1;
        }
        protected void SetDefaultText()
        {
            if (Environment == "DSV")
            {
                notifyIcon.Text = "DSV Sign Application";
                return;
            }
            if (Environment == "PL")
            {
                notifyIcon.Text = "Sign Application";
                return;
            }
            notifyIcon.Text = "LogBox Sign Application";
        }

        protected void SetBalloonTipTitle()
        {
            if (Environment == "DSV")
            {
                notifyIcon.BalloonTipTitle = "DSV Sign Application";
                return;
            }
            if (Environment == "PL")
            {
                notifyIcon.BalloonTipTitle = "Sign Application";
                return;
            }
            notifyIcon.BalloonTipTitle = "LogBox Sign Application";
        }
        protected void SetInactiveIcon()
        {
            if (Environment == "DSV")
            {
                notifyIcon.Icon = Resources.dsvInActive;
                return;
            }
            if (Environment == "PL")
            {
                notifyIcon.Icon = BitmapToIcon(Resources.PLInactiveLogo);
                return;
            }
            notifyIcon.Icon = Resources.logboxiconInActive;
        }

        protected Icon BitmapToIcon(Bitmap bitmap)
        {
            Bitmap bm = new Bitmap(bitmap);
            return Icon.FromHandle(bm.GetHicon());
        }
    }
}

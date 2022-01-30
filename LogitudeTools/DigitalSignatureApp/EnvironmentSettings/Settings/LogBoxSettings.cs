using Cloud.Sign.App.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Sign.App.EnvironmentSettings.Settings
{
    public class LogBoxSettings : IEnvironmentSetting
    {
        public string Environment => "LogBox";

        public string HeaderText => "LogBox Sign App";

        public Icon Icon => Resources.logboxicon1;

        public string LabelText => "LogBox Sign Client";

        public Image Image => Resources.LogBox;

        public string BalloonTipText => "You can access LogBox sign application from here.";

        public string BalloonTipTitle => "LogBox Sign Application";

        public Icon InactiveIcon => Resources.logboxiconInActive;

    }
}

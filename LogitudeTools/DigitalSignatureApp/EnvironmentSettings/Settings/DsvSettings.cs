using Cloud.Sign.App.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Sign.App.EnvironmentSettings.Settings
{
    public class DsvSettings : IEnvironmentSetting
    {
        public string Environment => "DSV";

        public string HeaderText => "DSV Sign App";

        public Icon Icon => Resources.dsv;

        public string LabelText => "DSV Sign Client";

        public Image Image => Resources.HeaderLogo;

        public string BalloonTipText => "You can access DSV sign application from here.";

        public string BalloonTipTitle => "DSV Sign Application";

        public Icon InactiveIcon => Resources.dsvInActive;

        public Icon HeaderIcon => Resources.dsv;
    }
}

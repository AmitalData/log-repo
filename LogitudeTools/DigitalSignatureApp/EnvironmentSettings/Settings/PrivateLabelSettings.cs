using Cloud.Sign.App.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Sign.App.EnvironmentSettings.Settings
{
    public class PrivateLabelSettings : IEnvironmentSetting
    {
        public string Environment => "PL";

        public string HeaderText => "Sign App";

        public Icon Icon => BitmapToIcon(Resources.PLogo);

        public string LabelText => "Sign Client";

        public Image Image => Resources.PLogo;

        public string BalloonTipText => "You can access sign application from here.";

        public string BalloonTipTitle => "Sign Application";

        public Icon InactiveIcon => BitmapToIcon(Resources.PLInactiveLogo);


        private Icon BitmapToIcon(Bitmap bitmap)
        {
            Bitmap bm = new Bitmap(bitmap);
            return Icon.FromHandle(bm.GetHicon());
        }
    }
}

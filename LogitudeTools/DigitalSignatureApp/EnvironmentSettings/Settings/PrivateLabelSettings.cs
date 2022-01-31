using Cloud.Sign.App.Helpers;
using Cloud.Sign.App.Properties;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Cloud.Sign.App.EnvironmentSettings.Settings
{
    public class PrivateLabelSettings : IEnvironmentSetting
    {


        public string Environment => "PL";

        public string HeaderText
        {
            get
            {
                var tenantName = SharedPrivateLabelTenant.tenantManagmentPrivateLabel?.PrivateLabelName;
                tenantName = string.IsNullOrEmpty(tenantName?.Trim()) ? "" : tenantName.Trim() + " ";
                return tenantName + "Sign App";
            }
        }

        public Icon Icon
        {
            get
            {

                if (SharedPrivateLabelTenant.tenantManagmentPrivateLabel?.SmallLogo == null)
                    return Resources.PLIcon;

                using (var ms = new MemoryStream(SharedPrivateLabelTenant.tenantManagmentPrivateLabel.SmallLogo))
                {
                    return IconFromImage(Image.FromStream(ms));
                }
            }
        }

        public string LabelText
        {
            get
            {
                var tenantName = SharedPrivateLabelTenant.tenantManagmentPrivateLabel?.PrivateLabelName;
                tenantName = string.IsNullOrEmpty(tenantName?.Trim()) ? "" : tenantName.Trim() + " ";
                return tenantName + "Sign Client";
            }
        }

        public Image Image
        {
            get
            {
                if (SharedPrivateLabelTenant.tenantManagmentPrivateLabel?.MainLogo == null)
                    return Resources.PLogo;

                using (var ms = new MemoryStream(SharedPrivateLabelTenant.tenantManagmentPrivateLabel.MainLogo))
                {
                    return Image.FromStream(ms);
                }
            }
        }

        public string BalloonTipText => "You can access sign application from here.";

        public string BalloonTipTitle
        {
            get
            {
                var tenantName = SharedPrivateLabelTenant.tenantManagmentPrivateLabel?.PrivateLabelName;
                tenantName = string.IsNullOrEmpty(tenantName?.Trim()) ? "" : tenantName.Trim() + " ";
                return tenantName + "Sign Application";
            }
        }

        public Icon InactiveIcon
        {
            get
            {

                if (SharedPrivateLabelTenant.tenantManagmentPrivateLabel?.SmallLogo == null)
                    return Resources.PLIcon;

                using (var ms = new MemoryStream(SharedPrivateLabelTenant.tenantManagmentPrivateLabel.SmallLogo))
                {
                    return IconFromImage(Image.FromStream(ms));
                }
            }
        }

        private Icon IconFromImage(Image img)
        {
            var ms = new System.IO.MemoryStream();
            var bw = new System.IO.BinaryWriter(ms);
            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
                                  // Image directory
            var w = img.Width;
            if (w >= 256) w = 0;
            bw.Write((byte)w);    // 0 : width of image
            var h = img.Height;
            if (h >= 256) h = 0;
            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            var sizeHere = ms.Position;
            bw.Write((int)0);     // 8 : image size
            var start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
                                  // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imageSize = (int)ms.Position - start;
            ms.Seek(sizeHere, System.IO.SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // And load it
            return new Icon(ms);
        }
    }
}

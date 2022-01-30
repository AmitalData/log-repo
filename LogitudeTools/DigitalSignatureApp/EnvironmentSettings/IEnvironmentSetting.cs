using System.Drawing;

namespace Cloud.Sign.App.EnvironmentSettings
{
    public interface IEnvironmentSetting
    {
        string Environment { get; }
        string HeaderText { get; }
        Icon Icon { get; }
        string LabelText { get; }
        Image Image { get; }
        string BalloonTipText { get; }
        string BalloonTipTitle { get; }
        Icon InactiveIcon { get; }
    }
}

using Logitude.Test.Base.Models.Login;

namespace Logitude.SecurityTests.Models.FTPDetail
{
    public class FTPDetailContext
    {
        public FTPDetailContext()
        {
            FirstUserDetail = new FTPDetailPM();
            SecondUserDetail = new FTPDetailPM();
        }

        public FTPDetailPM FirstUserDetail { get; set; }
        public FTPDetailPM SecondUserDetail { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}

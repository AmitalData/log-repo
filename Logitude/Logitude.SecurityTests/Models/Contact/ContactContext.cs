using Logitude.Test.Base.Models.Login;


namespace Logitude.SecurityTests.Models.Contact
{
   public class ContactContext
    {
        public ContactContext()
        {
            FirstUserContact = new ContactPM();
            SecondUserContact = new ContactPM();
        }

        public ContactPM FirstUserContact { get; set; }
        public ContactPM SecondUserContact { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}

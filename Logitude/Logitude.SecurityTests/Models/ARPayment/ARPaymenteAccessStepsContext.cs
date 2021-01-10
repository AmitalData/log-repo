using Logitude.Test.Base.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SecurityTests.Models.ARPayment
{
    public class ARPaymenteAccessStepsContext
    {
        public ARPaymenteAccessStepsContext()
        {
            FirstUserARPayment = new ARPaymentPM();
            SecondUserARPayment = new ARPaymentPM();
        }

        public ARPaymentPM FirstUserARPayment { get; set; }
        public ARPaymentPM SecondUserARPayment { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}


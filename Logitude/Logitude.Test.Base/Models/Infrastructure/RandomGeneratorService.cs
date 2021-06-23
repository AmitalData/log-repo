using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Models.Infrastructure
{
    public class RandomGeneratorService
    {

        public static int RandomNumber(int digitCount)
        {
            if (digitCount <= 0)
                return 0;

            // max int 
            if (digitCount > 9)
                digitCount = 9;

            return new Random().Next(Convert.ToInt32(Math.Pow(10, (digitCount - 1))), Convert.ToInt32(Math.Pow(10, (digitCount))));

        }

        public string RandomGuid()
        {
            return Guid.NewGuid().ToString();
        }

    }
}

using Logitude.LXMLFixer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.LXMLFixer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose The Desired Module\n1: Accounting\n2: Booking\n3: CRM\n4: Customs\n5: Old Modules\n6: Social\n7: Tarrifs\n8: Time Management\n9: Warehouse\n10: Infrastructure\n11: All Modules\n");

            int moduleNumber = 0;

            while (moduleNumber < 1 || moduleNumber > 11)
            {
                string userInput = Console.ReadLine();
                moduleNumber = ReadModuleNumber(userInput);
                if (moduleNumber < 1 || moduleNumber > 11)
                {
                    Console.WriteLine("Invalid Input, Try Again");
                }
            }

            LXMLFilesFixer fixer = new LXMLFilesFixer(moduleNumber);

            Console.WriteLine("\nDo You Want To Extract LXML Files Mistakes? y/n");

            bool extractMistakes = Console.ReadLine().ToLower() == "y";

            if (extractMistakes)
            {
                fixer.ExtractLXMLFilesMistakes();
            }

            Console.WriteLine("\nDo You Want To Fix LXML Files Mistakes? y/n");

            bool fixMistakes = Console.ReadLine().ToLower() == "y";

            if (fixMistakes)
            {
                fixer.FixLXMLFilesMistakes();
            }
        }

        private static int ReadModuleNumber(string userInput)
        {
            int moduleNumber;

            try
            {
                moduleNumber = Convert.ToInt32(userInput);
            }
            catch (Exception)
            {
                moduleNumber = 0;
            }

            return moduleNumber;
        }

    }
}
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
            LXMLFilesFixer fixer = new LXMLFilesFixer();

            Console.WriteLine("Do You Want To Extract LXML Files Mistakes? y/n");
            string result = Console.ReadLine();
            if(result.ToLower() == "y")
            {
                fixer.ExtractLXMLFilesMistakes();
            }

            Console.WriteLine("Do You Want To Fix LXML Files Mistakes? y/n");
            string result2 = Console.ReadLine();
            if (result2.ToLower() == "y")
            {
                fixer.FixLXMLFilesMistakes();
            }
        }
    }
}
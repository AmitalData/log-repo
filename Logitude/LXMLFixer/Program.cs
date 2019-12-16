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

            bool extractMistakes = Console.ReadLine().ToLower() == "y";

            if (extractMistakes)
            {
                fixer.ExtractLXMLFilesMistakes();
            }

            Console.WriteLine("Do You Want To Fix LXML Files Mistakes? y/n");
            
            bool fixMistakes = Console.ReadLine().ToLower() == "y";

            if (fixMistakes)
            {
                fixer.FixLXMLFilesMistakes();
            }
        }
    }
}
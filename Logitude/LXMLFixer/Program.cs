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
            fixer.ExtractMistakes();
        }
    }
}

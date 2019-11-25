using Logitude.DXMLGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DXMLGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            DXMLFilesGenerator generator = new DXMLFilesGenerator();
            generator.GenerateDXMLFiles();
        }
    }

}
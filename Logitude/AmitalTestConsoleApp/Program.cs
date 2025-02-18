using Logitude.BL.Helpers;
using Newtonsoft.Json;
using System;

namespace AmitalTestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("************ start AmitalTestConsoleApp ************");

            TestSyncRecord.Run();
            //TestDefaults.Run(); 

            Console.WriteLine("************ end AmitalTestConsoleApp ************");

            Console.ReadKey();
        }

    }
}

using Logitude.BL.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AmitalTestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter Tenant ID (or type 'exit' to quit): ");
                string tenantInput = Console.ReadLine();

                if (tenantInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (!int.TryParse(tenantInput, out int tenantId))
                {
                    Console.WriteLine("Invalid Tenant ID. Please enter a valid number.");
                    continue;
                }

                Console.Write("Enter Key (optional, press Enter to skip): ");
                string key = Console.ReadLine();

                if (string.IsNullOrEmpty(key))
                {
                    var settingsList = DefaultService.Instance.Get(tenantId);

                    if (settingsList != null && settingsList.Count > 0)
                    {
                        Console.WriteLine($"\nSettings for Tenant ID: {tenantId}");
                        foreach (var setting in settingsList)
                        {
                            DisplaySetting(setting);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Settings list is empty or null.");
                    }
                }
                else
                {
                    var settingByKey = DefaultService.Instance.Get(tenantId, key);

                    if (settingByKey != null)
                    {
                        Console.WriteLine($"\nSettings for Tenant ID: {tenantId} and Key: {key}");
                        settingByKey.ForEach(DisplaySetting);                        
                    }
                    else
                    {
                        Console.WriteLine("No settings found for the given key.");
                    }
                }

                Console.WriteLine();
            }
        }

        private static void DisplaySetting(DefaultAndConfiguration_Ext setting)
        {
            var objVal1 = setting.ObjVal1;
            var objVal2 = setting.ObjVal2;
            Console.WriteLine("SetKey: " + setting.SetKey + ",AdditionalKey: " + setting.AdditionalKey);
            Console.WriteLine(objVal1 != null ? $"-- ObjVal1: {objVal1}" : "ObjVal1 deserialization failed.");
            Console.WriteLine(objVal2 != null ? $"-- ObjVal2: {objVal2}" : "ObjVal2 deserialization failed.");
            Console.WriteLine("------------");
            Console.WriteLine();
        }
    }
}

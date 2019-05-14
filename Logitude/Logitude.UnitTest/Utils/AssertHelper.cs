
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Utils
{
    public class AssertHelper
    {
        public static void HasEqualFieldValues<T>(T expected, T actual, string errorMessage = "")
        {
            var failures = new List<string>();
            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(T).GetProperties()
                .Where(d =>
                !d.PropertyType.Name.Contains("List")
                && (d.PropertyType.BaseType != null ? d.PropertyType.BaseType.Name.ToLower() != "enum" : true)
                ));
            foreach (var field in props)
            {
                var v1 = field.GetValue(expected);
                var v2 = field.GetValue(actual);
                if (v1 == null && v2 == null) continue;
                if (!v1.Equals(v2))
                    failures.Add(string.Format("Property {0}: Expected:<{1}> Actual:<{2}>", field.Name, v1, v2));
            }
            if (failures.Any())
                Assert.Fail(
                    (string.IsNullOrWhiteSpace(errorMessage) ? "AssertHelper.HasEqualFieldValues failed." : errorMessage)
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, failures)
                    );

        }
    }

}


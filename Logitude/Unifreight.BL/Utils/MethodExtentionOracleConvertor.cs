using Logitude.Server.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Unifreight.BL.Utils 
namespace Unifreight.BL.EntityPMs
{
    
    public static class MethodExtentionOracleConvertor
    {
        public static decimal? ToNullableDecimal(this double? myDouble, string fieldName)
        {
            decimal mydeciml = 0;
            if (myDouble == null)
            {
                return null;
            }

            if (!decimal.TryParse(myDouble.Value.ToString(), out mydeciml))
            {
                throw new BusinessErrorException(fieldName + " could not convert to double " + myDouble.Value.ToString());
            }
            return mydeciml;
        }
        
        public static double? ToNullableDouble(this decimal? myDecimal, string fieldName)
        {
            double mydouble = 0;
            if (myDecimal == null)
            {
                return null;
            }

            if (!double.TryParse(myDecimal.Value.ToString(), out mydouble))
            {
                throw new BusinessErrorException(fieldName + " could not convert to double " + myDecimal.Value.ToString());
            }
            return mydouble;
        }

        public static int? ToNullableInt(this decimal? myDecimal, string fieldName)
        {
            int myint = 0;
            if (myDecimal == null)
            {
                return null;
            }
            if (!myDecimal.Value.Equals(Math.Truncate(myDecimal.Value)))
            {
                throw new BusinessErrorException(fieldName + " could not convert to int " + myDecimal.Value.ToString());
            }
            //var truncate = Math.Truncate(actual);
            var stringDecimal = myDecimal.Value.ToString("0");

            if (!int.TryParse(stringDecimal, out myint))
            {
                throw new BusinessErrorException(fieldName + " could not convert to int " + myDecimal.Value.ToString());
            }
            return myint;
        }

        public static int? ToNullableInt(this string myString, string fieldName)
        {
            int myint = 0;
            if (string.IsNullOrWhiteSpace( myString))
            {
                return null;
            }

            if (!int.TryParse(myString, out myint))
            {
                throw new BusinessErrorException(fieldName + " could not convert to int " + myString);
            }
            return myint;
        }
    }
}

using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public abstract partial class EntityUpdateService<TEntityPOCO, TEntityPM, TEntityParentPM>
    {
#if false
        

        private void OnUpdatingEcodeBase64OnNVCHARFields()
        {
            

            if (!this.EntityPM.OnUpdatingEcodeBase64OnNVCHARFields)
            {
                return;
            }
            string typeName = this.EntityPM.GetType().Name;
            string objectTableName = typeName.Substring(0, typeName.Length - 2);
            var objectFieldList = ObjectFieldsRepository.GetObjectFieldsByObjectTableName(objectTableName, 0).ToList();
            var nvarcharFields = objectFieldList.Where(f => f.DataTypeCode == "nText").ToList();
            if (nvarcharFields.Count < 1)
            {
                return;
            }
            string decodeVal = "";
            string ecodeVal = "";
            var props = EntityPM.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
   .Where(p => p.CanRead && p.CanWrite)
   .Where(p => p.PropertyType == typeof(string))
   .Where(p => p.GetGetMethod(true).IsPublic)
   .Where(p => p.GetSetMethod(true).IsPublic);
            foreach (var p in props)
            {
                decodeVal = (string)p.GetValue(this.EntityPM, null);
                if (!string.IsNullOrEmpty(decodeVal) && EncodBase64(decodeVal, out ecodeVal))
                {
                    p.SetValue(this.EntityPM, ecodeVal, null);
                }
            }
            this.EntityPM.OnUpdatingEcodeBase64OnNVCHARFields = false;

        }

        public static bool EncodBase64(string base64String, out string base64Encoded)
        {
            base64Encoded = null;

            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }
            if (!Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
            {
                return false;
            }

            try
            {
                var bytes = Convert.FromBase64String(base64String);
                base64Encoded = Encoding.UTF8.GetString(bytes);

                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }
#endif
    }
}

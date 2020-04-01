using Logitude.BL.ShipmentsModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.DataVariablesContexts
{
    // https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/

    public sealed class ChargesTypesDataContext
    {
        private List<PreparationShortClass> items;
        private ChargesTypesDataContext()
        {
            this.items = new List<PreparationShortClass>();
        }

        private static readonly Lazy<ChargesTypesDataContext> lazy = new Lazy<ChargesTypesDataContext>(() => new ChargesTypesDataContext());
        public static ChargesTypesDataContext Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public void SetData(List<PreparationShortClass> items)
        {
            this.items = items;
        }

        public string GetId(string code)
        {
            string output = null;

            if (!string.IsNullOrEmpty(code))
            {
                PreparationShortClass item = this.items.Where(d => d.Code.ToUpper() == code.ToUpper()).FirstOrDefault();
                if (item != null)
                {
                    output = item.Id;
                }
            }

            return output;
        }
    }
}

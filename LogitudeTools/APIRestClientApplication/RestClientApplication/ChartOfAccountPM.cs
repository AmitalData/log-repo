
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Runtime.Serialization;

namespace Logitude.Accounting.BL.EntityPMs
{

    public partial class ChartOfAccountPM
    {
        private string id;


        public string Id
        {

            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {

                    id = value;
                }

            }
        }
        private int tenant;



        public int Tenant
        {

            get
            {
                return tenant;
            }
            set
            {
                if (tenant != value)
                {

                    tenant = value;
                }

            }
        }
        private string code;


        public string Code
        {

            get
            {
                return code;
            }
            set
            {
                if (code != value)
                {

                    code = value;
                }

            }
        }
        private string localName;



        public string LocalName
        {

            get
            {
                return localName;
            }
            set
            {
                if (localName != value)
                {

                    localName = value;
                }

            }
        }
        private string englishName;



        public string EnglishName
        {

            get
            {
                return englishName;
            }
            set
            {
                if (englishName != value)
                {

                    englishName = value;
                }

            }
        }
        private string parentId;



        public string ParentId
        {

            get
            {
                return parentId;
            }
            set
            {
                if (parentId != value)
                {

                    parentId = value;
                }

            }
        }
        private string typeCode;



        public string TypeCode
        {

            get
            {
                return typeCode;
            }
            set
            {
                if (typeCode != value)
                {

                    typeCode = value;
                }

            }
        }
        private bool? inactive;



        public bool? Inactive
        {

            get
            {
                return inactive;
            }
            set
            {
                if (inactive != value)
                {

                    inactive = value;
                }

            }
        }
        private string typeName;



        public string TypeName
        {

            get
            {
                return typeName;
            }
            set
            {
                if (typeName != value)
                {

                    typeName = value;
                }

            }
        }
        private string parentName;



        public string ParentName
        {

            get
            {
                return parentName;
            }
            set
            {
                if (parentName != value)
                {

                    parentName = value;
                }

            }
        }
        private string searchFields;



        public string SearchFields
        {

            get
            {
                return searchFields;
            }
            set
            {
                if (searchFields != value)
                {

                    searchFields = value;
                }

            }
        }
    }

}
       

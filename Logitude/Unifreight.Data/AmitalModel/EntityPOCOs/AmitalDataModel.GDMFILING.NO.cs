using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityPOCOs
{
#if reverseName
    

    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public partial class GDMFILING : INotifyPropertyChanged
    {
        /// <summary>
        /// There are no comments for SOURCE in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string SOURCE
        {
            get
            {
                return _SOURCE;
            }
            set
            {
                if (_SOURCE != value)
                {
                    _SOURCE = value;
                    OnPropertyChanged("SOURCE");
                }
            }
        }
        private string _SOURCE;


        /// <summary>
        /// There are no comments for DESC in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string DESC
        {
            get
            {
                return _DESC;
            }
            set
            {
                if (_DESC != value)
                {
                    _DESC = value;
                    OnPropertyChanged("DESC");
                }
            }
        }
        private string _DESC;


        /// <summary>
        /// There are no comments for FROM in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string FROM
        {
            get
            {
                return _FROM;
            }
            set
            {
                if (_FROM != value)
                {
                    _FROM = value;
                    OnPropertyChanged("FROM");
                }
            }
        }
        private string _FROM;


        /// <summary>
        /// There are no comments for TO in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string TO
        {
            get
            {
                return _TO;
            }
            set
            {
                if (_TO != value)
                {
                    _TO = value;
                    OnPropertyChanged("TO");
                }
            }
        }
        private string _TO;

    }
#endif
}

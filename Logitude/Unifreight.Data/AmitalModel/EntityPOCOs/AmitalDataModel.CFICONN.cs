using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Unifreight.Data.AmitalModel.EntityPOCOs
{

    /// <summary>
    /// There are no comments for Unifreight.Data.AmitalModel.CFICONN in the schema.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public partial class CFICONN : INotifyPropertyChanged
    {

        public CFICONN()
        {
        }

        #region Properties

        /// <summary>
        /// There are no comments for FILENO in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string FILENO
        {
            get
            {
                return _FILENO;
            }
            set
            {
                if (_FILENO != value)
                {
                    _FILENO = value;
                    OnPropertyChanged("FILENO");
                }
            }
        }
        private string _FILENO;


        /// <summary>
        /// There are no comments for CUSTOMFILE in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual long CUSTOMFILE
        {
            get
            {
                return _CUSTOMFILE;
            }
            set
            {
                if (_CUSTOMFILE != value)
                {
                    _CUSTOMFILE = value;
                    OnPropertyChanged("CUSTOMFILE");
                }
            }
        }
        private long _CUSTOMFILE;


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}


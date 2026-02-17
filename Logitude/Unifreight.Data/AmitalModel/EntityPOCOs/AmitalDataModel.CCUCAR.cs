using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityPOCOs
{
    /// <summary>
    /// There are no comments for Unifreight.Data.AmitalModel.CCUCAR in the schema.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public partial class CCUCAR : INotifyPropertyChanged
    {

        public CCUCAR()
        {
        }

        #region Properties

        /// <summary>
        /// There are no comments for FILENO in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual int FILENO
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
        private int _FILENO;


        /// <summary>
        /// There are no comments for LINENO in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual int LINENO
        {
            get
            {
                return _LINENO;
            }
            set
            {
                if (_LINENO != value)
                {
                    _LINENO = value;
                    OnPropertyChanged("LINENO");
                }
            }
        }
        private int _LINENO;


        /// <summary>
        /// There are no comments for ABSDEDUCT in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual global::System.Nullable<int> ABSDEDUCT
        {
            get
            {
                return _ABSDEDUCT;
            }
            set
            {
                if (_ABSDEDUCT != value)
                {
                    _ABSDEDUCT = value;
                    OnPropertyChanged("ABSDEDUCT");
                }
            }
        }
        private global::System.Nullable<int> _ABSDEDUCT;


        /// <summary>
        /// There are no comments for KARITDEDUCT in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual global::System.Nullable<int> KARITDEDUCT
        {
            get
            {
                return _KARITDEDUCT;
            }
            set
            {
                if (_KARITDEDUCT != value)
                {
                    _KARITDEDUCT = value;
                    OnPropertyChanged("KARITDEDUCT");
                }
            }
        }
        private global::System.Nullable<int> _KARITDEDUCT;


        /// <summary>
        /// There are no comments for BAKARADEDUCT in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual global::System.Nullable<int> BAKARADEDUCT
        {
            get
            {
                return _BAKARADEDUCT;
            }
            set
            {
                if (_BAKARADEDUCT != value)
                {
                    _BAKARADEDUCT = value;
                    OnPropertyChanged("BAKARADEDUCT");
                }
            }
        }
        private global::System.Nullable<int> _BAKARADEDUCT;


        /// <summary>
        /// There are no comments for MEMIRDEDUCT in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual global::System.Nullable<decimal> MEMIRDEDUCT
        {
            get
            {
                return _MEMIRDEDUCT;
            }
            set
            {
                if (_MEMIRDEDUCT != value)
                {
                    _MEMIRDEDUCT = value;
                    OnPropertyChanged("MEMIRDEDUCT");
                }
            }
        }
        private global::System.Nullable<decimal> _MEMIRDEDUCT;


        /// <summary>
        /// There are no comments for MADADDEDUCT in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual global::System.Nullable<decimal> MADADDEDUCT
        {
            get
            {
                return _MADADDEDUCT;
            }
            set
            {
                if (_MADADDEDUCT != value)
                {
                    _MADADDEDUCT = value;
                    OnPropertyChanged("MADADDEDUCT");
                }
            }
        }
        private global::System.Nullable<decimal> _MADADDEDUCT;


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

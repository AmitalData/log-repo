

//------------------------------------------------------------------------------
// this itzik dummy home made class - only for grant use !!!
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Unifreight.Data.AmitalModel.EntityPOCOs
{
    /// <summary>
    /// There are no comments for Unifreight.Data.AmitalModel.CFIFILEM in the schema.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public partial class CFIFILEM : INotifyPropertyChanged
    {

        public CFIFILEM()
        {
            ///throw new Exception("this itzik dummy home made class - only for grant use !!!");
        }

        #region Properties

       

        /// <summary>
        /// There are no comments for FILE_NO in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual global::System.Nullable<long> FILE_NO
        {
            get
            {
                return _FILE_NO;
            }
            set
            {
                if (_FILE_NO != value)
                {
                    _FILE_NO = value;
                    OnPropertyChanged("FILE_NO");
                }
            }
        }
        private global::System.Nullable<long> _FILE_NO;

        
        public virtual string LOGITUDE_FILE { get; set; }

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Unifreight.Data.AmitalModel.EntityPOCOs
{
    /// <summary>
    /// There are no comments for Unifreight.Data.AmitalModel.GTBITEMSMSV in the schema.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public partial class GTBITEMSMSV : INotifyPropertyChanged
    {

        public GTBITEMSMSV()
        {
        }

        #region Properties

        /// <summary>
        /// There are no comments for PRATID in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string PRATID
        {
            get
            {
                return _PRATID;
            }
            set
            {
                if (_PRATID != value)
                {
                    _PRATID = value;
                    OnPropertyChanged("PRATID");
                }
            }
        }
        private string _PRATID;


        /// <summary>
        /// There are no comments for NAMEENG in the schema.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string NAMEENG
        {
            get
            {
                return _NAMEENG;
            }
            set
            {
                if (_NAMEENG != value)
                {
                    _NAMEENG = value;
                    OnPropertyChanged("NAMEENG");
                }
            }
        }
        private string _NAMEENG;


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

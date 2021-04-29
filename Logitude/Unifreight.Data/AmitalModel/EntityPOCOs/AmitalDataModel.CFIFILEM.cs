//#define reserveword



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
    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public partial class CFIFILEM : INotifyPropertyChanged
    {

        public CFIFILEM()
        {
            OnCreated();
        }

        #region Properties

        [System.Runtime.Serialization.DataMember]
        public virtual long FILENO
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
        private long _FILENO;
#if reserveword


        [System.Runtime.Serialization.DataMember]
        public virtual string FILEPREFIX
        {
            get
            {
                return _FILEPREFIX;
            }
            set
            {
                if (_FILEPREFIX != value)
                {
                    _FILEPREFIX = value;
                    OnPropertyChanged("FILEPREFIX");
                }
            }
        }
        private string _FILEPREFIX;

        [System.Runtime.Serialization.DataMember]
        public virtual string BRANCHID
        {
            get
            {
                return _BRANCHID;
            }
            set
            {
                if (_BRANCHID != value)
                {
                    _BRANCHID = value;
                    OnPropertyChanged("BRANCHID");
                }
            }
        }
        private string _BRANCHID;

        [System.Runtime.Serialization.DataMember]
        public virtual string CUSTOMERID
        {
            get
            {
                return _CUSTOMERID;
            }
            set
            {
                if (_CUSTOMERID != value)
                {
                    _CUSTOMERID = value;
                    OnPropertyChanged("CUSTOMERID");
                }
            }
        }
        private string _CUSTOMERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string SUPPLIERID
        {
            get
            {
                return _SUPPLIERID;
            }
            set
            {
                if (_SUPPLIERID != value)
                {
                    _SUPPLIERID = value;
                    OnPropertyChanged("SUPPLIERID");
                }
            }
        }
        private string _SUPPLIERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string CUSSUPPLIERID
        {
            get
            {
                return _CUSSUPPLIERID;
            }
            set
            {
                if (_CUSSUPPLIERID != value)
                {
                    _CUSSUPPLIERID = value;
                    OnPropertyChanged("CUSSUPPLIERID");
                }
            }
        }
        private string _CUSSUPPLIERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string CUSTOMSBRANCHID
        {
            get
            {
                return _CUSTOMSBRANCHID;
            }
            set
            {
                if (_CUSTOMSBRANCHID != value)
                {
                    _CUSTOMSBRANCHID = value;
                    OnPropertyChanged("CUSTOMSBRANCHID");
                }
            }
        }
        private string _CUSTOMSBRANCHID;

        [System.Runtime.Serialization.DataMember]
        public virtual string DEPARTID
        {
            get
            {
                return _DEPARTID;
            }
            set
            {
                if (_DEPARTID != value)
                {
                    _DEPARTID = value;
                    OnPropertyChanged("DEPARTID");
                }
            }
        }
        private string _DEPARTID;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? OPENDATE
        {
            get
            {
                return _OPENDATE;
            }
            set
            {
                if (_OPENDATE != value)
                {
                    _OPENDATE = value;
                    OnPropertyChanged("OPENDATE");
                }
            }
        }
        private global::System.DateTime? _OPENDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string USERID
        {
            get
            {
                return _USERID;
            }
            set
            {
                if (_USERID != value)
                {
                    _USERID = value;
                    OnPropertyChanged("USERID");
                }
            }
        }
        private string _USERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string PACKTYPEID
        {
            get
            {
                return _PACKTYPEID;
            }
            set
            {
                if (_PACKTYPEID != value)
                {
                    _PACKTYPEID = value;
                    OnPropertyChanged("PACKTYPEID");
                }
            }
        }
        private string _PACKTYPEID;

        [System.Runtime.Serialization.DataMember]
        public virtual string FILETYPE
        {
            get
            {
                return _FILETYPE;
            }
            set
            {
                if (_FILETYPE != value)
                {
                    _FILETYPE = value;
                    OnPropertyChanged("FILETYPE");
                }
            }
        }
        private string _FILETYPE;

        [System.Runtime.Serialization.DataMember]
        public virtual long? QUANTITY
        {
            get
            {
                return _QUANTITY;
            }
            set
            {
                if (_QUANTITY != value)
                {
                    _QUANTITY = value;
                    OnPropertyChanged("QUANTITY");
                }
            }
        }
        private long? _QUANTITY;

        [System.Runtime.Serialization.DataMember]
        public virtual double? WEIGHT
        {
            get
            {
                return _WEIGHT;
            }
            set
            {
                if (_WEIGHT != value)
                {
                    _WEIGHT = value;
                    OnPropertyChanged("WEIGHT");
                }
            }
        }
        private double? _WEIGHT;

        [System.Runtime.Serialization.DataMember]
        public virtual double? VOLUME
        {
            get
            {
                return _VOLUME;
            }
            set
            {
                if (_VOLUME != value)
                {
                    _VOLUME = value;
                    OnPropertyChanged("VOLUME");
                }
            }
        }
        private double? _VOLUME;

        [System.Runtime.Serialization.DataMember]
        public virtual string AWBCARRIER
        {
            get
            {
                return _AWBCARRIER;
            }
            set
            {
                if (_AWBCARRIER != value)
                {
                    _AWBCARRIER = value;
                    OnPropertyChanged("AWBCARRIER");
                }
            }
        }
        private string _AWBCARRIER;

        [System.Runtime.Serialization.DataMember]
        public virtual string VESSEL
        {
            get
            {
                return _VESSEL;
            }
            set
            {
                if (_VESSEL != value)
                {
                    _VESSEL = value;
                    OnPropertyChanged("VESSEL");
                }
            }
        }
        private string _VESSEL;

        [System.Runtime.Serialization.DataMember]
        public virtual string FLIGHTNUM
        {
            get
            {
                return _FLIGHTNUM;
            }
            set
            {
                if (_FLIGHTNUM != value)
                {
                    _FLIGHTNUM = value;
                    OnPropertyChanged("FLIGHTNUM");
                }
            }
        }
        private string _FLIGHTNUM;

        [System.Runtime.Serialization.DataMember]
        public virtual double? FREIGHT
        {
            get
            {
                return _FREIGHT;
            }
            set
            {
                if (_FREIGHT != value)
                {
                    _FREIGHT = value;
                    OnPropertyChanged("FREIGHT");
                }
            }
        }
        private double? _FREIGHT;

        [System.Runtime.Serialization.DataMember]
        public virtual string ORIGINID
        {
            get
            {
                return _ORIGINID;
            }
            set
            {
                if (_ORIGINID != value)
                {
                    _ORIGINID = value;
                    OnPropertyChanged("ORIGINID");
                }
            }
        }
        private string _ORIGINID;

        [System.Runtime.Serialization.DataMember]
        public virtual string DESTINATIONID
        {
            get
            {
                return _DESTINATIONID;
            }
            set
            {
                if (_DESTINATIONID != value)
                {
                    _DESTINATIONID = value;
                    OnPropertyChanged("DESTINATIONID");
                }
            }
        }
        private string _DESTINATIONID;

        [System.Runtime.Serialization.DataMember]
        public virtual string MAWB
        {
            get
            {
                return _MAWB;
            }
            set
            {
                if (_MAWB != value)
                {
                    _MAWB = value;
                    OnPropertyChanged("MAWB");
                }
            }
        }
        private string _MAWB;

        [System.Runtime.Serialization.DataMember]
        public virtual string HAWB
        {
            get
            {
                return _HAWB;
            }
            set
            {
                if (_HAWB != value)
                {
                    _HAWB = value;
                    OnPropertyChanged("HAWB");
                }
            }
        }
        private string _HAWB;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? HAWBDATE
        {
            get
            {
                return _HAWBDATE;
            }
            set
            {
                if (_HAWBDATE != value)
                {
                    _HAWBDATE = value;
                    OnPropertyChanged("HAWBDATE");
                }
            }
        }
        private global::System.DateTime? _HAWBDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? ARRIVALDATE
        {
            get
            {
                return _ARRIVALDATE;
            }
            set
            {
                if (_ARRIVALDATE != value)
                {
                    _ARRIVALDATE = value;
                    OnPropertyChanged("ARRIVALDATE");
                }
            }
        }
        private global::System.DateTime? _ARRIVALDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string ISKA
        {
            get
            {
                return _ISKA;
            }
            set
            {
                if (_ISKA != value)
                {
                    _ISKA = value;
                    OnPropertyChanged("ISKA");
                }
            }
        }
        private string _ISKA;

        [System.Runtime.Serialization.DataMember]
        public virtual string FORWARDERID
        {
            get
            {
                return _FORWARDERID;
            }
            set
            {
                if (_FORWARDERID != value)
                {
                    _FORWARDERID = value;
                    OnPropertyChanged("FORWARDERID");
                }
            }
        }
        private string _FORWARDERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string MANIFESTNO
        {
            get
            {
                return _MANIFESTNO;
            }
            set
            {
                if (_MANIFESTNO != value)
                {
                    _MANIFESTNO = value;
                    OnPropertyChanged("MANIFESTNO");
                }
            }
        }
        private string _MANIFESTNO;

        [System.Runtime.Serialization.DataMember]
        public virtual string TRUCKERID
        {
            get
            {
                return _TRUCKERID;
            }
            set
            {
                if (_TRUCKERID != value)
                {
                    _TRUCKERID = value;
                    OnPropertyChanged("TRUCKERID");
                }
            }
        }
        private string _TRUCKERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string MVZONE
        {
            get
            {
                return _MVZONE;
            }
            set
            {
                if (_MVZONE != value)
                {
                    _MVZONE = value;
                    OnPropertyChanged("MVZONE");
                }
            }
        }
        private string _MVZONE;

        [System.Runtime.Serialization.DataMember]
        public virtual string CONTACTID
        {
            get
            {
                return _CONTACTID;
            }
            set
            {
                if (_CONTACTID != value)
                {
                    _CONTACTID = value;
                    OnPropertyChanged("CONTACTID");
                }
            }
        }
        private string _CONTACTID;

        [System.Runtime.Serialization.DataMember]
        public virtual string TRANSPORTATIONTYPE
        {
            get
            {
                return _TRANSPORTATIONTYPE;
            }
            set
            {
                if (_TRANSPORTATIONTYPE != value)
                {
                    _TRANSPORTATIONTYPE = value;
                    OnPropertyChanged("TRANSPORTATIONTYPE");
                }
            }
        }
        private string _TRANSPORTATIONTYPE;

        [System.Runtime.Serialization.DataMember]
        public virtual string INSURANCE
        {
            get
            {
                return _INSURANCE;
            }
            set
            {
                if (_INSURANCE != value)
                {
                    _INSURANCE = value;
                    OnPropertyChanged("INSURANCE");
                }
            }
        }
        private string _INSURANCE;

        [System.Runtime.Serialization.DataMember]
        public virtual string HAWBSHORT
        {
            get
            {
                return _HAWBSHORT;
            }
            set
            {
                if (_HAWBSHORT != value)
                {
                    _HAWBSHORT = value;
                    OnPropertyChanged("HAWBSHORT");
                }
            }
        }
        private string _HAWBSHORT;

        [System.Runtime.Serialization.DataMember]
        public virtual string WTVAL
        {
            get
            {
                return _WTVAL;
            }
            set
            {
                if (_WTVAL != value)
                {
                    _WTVAL = value;
                    OnPropertyChanged("WTVAL");
                }
            }
        }
        private string _WTVAL;

        [System.Runtime.Serialization.DataMember]
        public virtual string PAYMENTTERM
        {
            get
            {
                return _PAYMENTTERM;
            }
            set
            {
                if (_PAYMENTTERM != value)
                {
                    _PAYMENTTERM = value;
                    OnPropertyChanged("PAYMENTTERM");
                }
            }
        }
        private string _PAYMENTTERM;

        [System.Runtime.Serialization.DataMember]
        public virtual string GUSH
        {
            get
            {
                return _GUSH;
            }
            set
            {
                if (_GUSH != value)
                {
                    _GUSH = value;
                    OnPropertyChanged("GUSH");
                }
            }
        }
        private string _GUSH;

        [System.Runtime.Serialization.DataMember]
        public virtual bool? FILECLOSED
        {
            get
            {
                return _FILECLOSED;
            }
            set
            {
                if (_FILECLOSED != value)
                {
                    _FILECLOSED = value;
                    OnPropertyChanged("FILECLOSED");
                }
            }
        }
        private bool? _FILECLOSED;

        [System.Runtime.Serialization.DataMember]
        public virtual string STATUSID
        {
            get
            {
                return _STATUSID;
            }
            set
            {
                if (_STATUSID != value)
                {
                    _STATUSID = value;
                    OnPropertyChanged("STATUSID");
                }
            }
        }
        private string _STATUSID;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? STATUSDATE
        {
            get
            {
                return _STATUSDATE;
            }
            set
            {
                if (_STATUSDATE != value)
                {
                    _STATUSDATE = value;
                    OnPropertyChanged("STATUSDATE");
                }
            }
        }
        private global::System.DateTime? _STATUSDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string LSTSTATUSID
        {
            get
            {
                return _LSTSTATUSID;
            }
            set
            {
                if (_LSTSTATUSID != value)
                {
                    _LSTSTATUSID = value;
                    OnPropertyChanged("LSTSTATUSID");
                }
            }
        }
        private string _LSTSTATUSID;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? LSTSTATUSDATE
        {
            get
            {
                return _LSTSTATUSDATE;
            }
            set
            {
                if (_LSTSTATUSDATE != value)
                {
                    _LSTSTATUSDATE = value;
                    OnPropertyChanged("LSTSTATUSDATE");
                }
            }
        }
        private global::System.DateTime? _LSTSTATUSDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string CUSTPACK
        {
            get
            {
                return _CUSTPACK;
            }
            set
            {
                if (_CUSTPACK != value)
                {
                    _CUSTPACK = value;
                    OnPropertyChanged("CUSTPACK");
                }
            }
        }
        private string _CUSTPACK;

        [System.Runtime.Serialization.DataMember]
        public virtual string CUSTPORT
        {
            get
            {
                return _CUSTPORT;
            }
            set
            {
                if (_CUSTPORT != value)
                {
                    _CUSTPORT = value;
                    OnPropertyChanged("CUSTPORT");
                }
            }
        }
        private string _CUSTPORT;

        [System.Runtime.Serialization.DataMember]
        public virtual string OLDCCFILE
        {
            get
            {
                return _OLDCCFILE;
            }
            set
            {
                if (_OLDCCFILE != value)
                {
                    _OLDCCFILE = value;
                    OnPropertyChanged("OLDCCFILE");
                }
            }
        }
        private string _OLDCCFILE;

        [System.Runtime.Serialization.DataMember]
        public virtual int? ENTRYFILENO
        {
            get
            {
                return _ENTRYFILENO;
            }
            set
            {
                if (_ENTRYFILENO != value)
                {
                    _ENTRYFILENO = value;
                    OnPropertyChanged("ENTRYFILENO");
                }
            }
        }
        private int? _ENTRYFILENO;

        [System.Runtime.Serialization.DataMember]
        public virtual long? RELEASEFILENO
        {
            get
            {
                return _RELEASEFILENO;
            }
            set
            {
                if (_RELEASEFILENO != value)
                {
                    _RELEASEFILENO = value;
                    OnPropertyChanged("RELEASEFILENO");
                }
            }
        }
        private long? _RELEASEFILENO;

        [System.Runtime.Serialization.DataMember]
        public virtual string RESHIMONNO
        {
            get
            {
                return _RESHIMONNO;
            }
            set
            {
                if (_RESHIMONNO != value)
                {
                    _RESHIMONNO = value;
                    OnPropertyChanged("RESHIMONNO");
                }
            }
        }
        private string _RESHIMONNO;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? RESHIMONDATE
        {
            get
            {
                return _RESHIMONDATE;
            }
            set
            {
                if (_RESHIMONDATE != value)
                {
                    _RESHIMONDATE = value;
                    OnPropertyChanged("RESHIMONDATE");
                }
            }
        }
        private global::System.DateTime? _RESHIMONDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string RESHIMONTYPE
        {
            get
            {
                return _RESHIMONTYPE;
            }
            set
            {
                if (_RESHIMONTYPE != value)
                {
                    _RESHIMONTYPE = value;
                    OnPropertyChanged("RESHIMONTYPE");
                }
            }
        }
        private string _RESHIMONTYPE;

        [System.Runtime.Serialization.DataMember]
        public virtual string ORIGINCOUNTRY
        {
            get
            {
                return _ORIGINCOUNTRY;
            }
            set
            {
                if (_ORIGINCOUNTRY != value)
                {
                    _ORIGINCOUNTRY = value;
                    OnPropertyChanged("ORIGINCOUNTRY");
                }
            }
        }
        private string _ORIGINCOUNTRY;

        [System.Runtime.Serialization.DataMember]
        public virtual string COMMODITYID
        {
            get
            {
                return _COMMODITYID;
            }
            set
            {
                if (_COMMODITYID != value)
                {
                    _COMMODITYID = value;
                    OnPropertyChanged("COMMODITYID");
                }
            }
        }
        private string _COMMODITYID;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? MAWBDATE
        {
            get
            {
                return _MAWBDATE;
            }
            set
            {
                if (_MAWBDATE != value)
                {
                    _MAWBDATE = value;
                    OnPropertyChanged("MAWBDATE");
                }
            }
        }
        private global::System.DateTime? _MAWBDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string PTERMID
        {
            get
            {
                return _PTERMID;
            }
            set
            {
                if (_PTERMID != value)
                {
                    _PTERMID = value;
                    OnPropertyChanged("PTERMID");
                }
            }
        }
        private string _PTERMID;

        [System.Runtime.Serialization.DataMember]
        public virtual double? CHARGWT
        {
            get
            {
                return _CHARGWT;
            }
            set
            {
                if (_CHARGWT != value)
                {
                    _CHARGWT = value;
                    OnPropertyChanged("CHARGWT");
                }
            }
        }
        private double? _CHARGWT;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? ETA
        {
            get
            {
                return _ETA;
            }
            set
            {
                if (_ETA != value)
                {
                    _ETA = value;
                    OnPropertyChanged("ETA");
                }
            }
        }
        private global::System.DateTime? _ETA;

        [System.Runtime.Serialization.DataMember]
        public virtual bool? FUCLOSE
        {
            get
            {
                return _FUCLOSE;
            }
            set
            {
                if (_FUCLOSE != value)
                {
                    _FUCLOSE = value;
                    OnPropertyChanged("FUCLOSE");
                }
            }
        }
        private bool? _FUCLOSE;

        [System.Runtime.Serialization.DataMember]
        public virtual bool? ACCOUNTINGCLOSE
        {
            get
            {
                return _ACCOUNTINGCLOSE;
            }
            set
            {
                if (_ACCOUNTINGCLOSE != value)
                {
                    _ACCOUNTINGCLOSE = value;
                    OnPropertyChanged("ACCOUNTINGCLOSE");
                }
            }
        }
        private bool? _ACCOUNTINGCLOSE;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? GRANTDATE
        {
            get
            {
                return _GRANTDATE;
            }
            set
            {
                if (_GRANTDATE != value)
                {
                    _GRANTDATE = value;
                    OnPropertyChanged("GRANTDATE");
                }
            }
        }
        private global::System.DateTime? _GRANTDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual string OPENBYUSER
        {
            get
            {
                return _OPENBYUSER;
            }
            set
            {
                if (_OPENBYUSER != value)
                {
                    _OPENBYUSER = value;
                    OnPropertyChanged("OPENBYUSER");
                }
            }
        }
        private string _OPENBYUSER;

        [System.Runtime.Serialization.DataMember]
        public virtual string PROFILEID
        {
            get
            {
                return _PROFILEID;
            }
            set
            {
                if (_PROFILEID != value)
                {
                    _PROFILEID = value;
                    OnPropertyChanged("PROFILEID");
                }
            }
        }
        private string _PROFILEID;

        [System.Runtime.Serialization.DataMember]
        public virtual string IMPORTTYPE
        {
            get
            {
                return _IMPORTTYPE;
            }
            set
            {
                if (_IMPORTTYPE != value)
                {
                    _IMPORTTYPE = value;
                    OnPropertyChanged("IMPORTTYPE");
                }
            }
        }
        private string _IMPORTTYPE;

        [System.Runtime.Serialization.DataMember]
        public virtual string FILECLASS
        {
            get
            {
                return _FILECLASS;
            }
            set
            {
                if (_FILECLASS != value)
                {
                    _FILECLASS = value;
                    OnPropertyChanged("FILECLASS");
                }
            }
        }
        private string _FILECLASS;

        [System.Runtime.Serialization.DataMember]
        public virtual string CANCELLED
        {
            get
            {
                return _CANCELLED;
            }
            set
            {
                if (_CANCELLED != value)
                {
                    _CANCELLED = value;
                    OnPropertyChanged("CANCELLED");
                }
            }
        }
        private string _CANCELLED;

        [System.Runtime.Serialization.DataMember]
        public virtual string MEDIATORID
        {
            get
            {
                return _MEDIATORID;
            }
            set
            {
                if (_MEDIATORID != value)
                {
                    _MEDIATORID = value;
                    OnPropertyChanged("MEDIATORID");
                }
            }
        }
        private string _MEDIATORID;

        [System.Runtime.Serialization.DataMember]
        public virtual string FCL
        {
            get
            {
                return _FCL;
            }
            set
            {
                if (_FCL != value)
                {
                    _FCL = value;
                    OnPropertyChanged("FCL");
                }
            }
        }
        private string _FCL;

        [System.Runtime.Serialization.DataMember]
        public virtual string SACREDIT
        {
            get
            {
                return _SACREDIT;
            }
            set
            {
                if (_SACREDIT != value)
                {
                    _SACREDIT = value;
                    OnPropertyChanged("SACREDIT");
                }
            }
        }
        private string _SACREDIT;

        [System.Runtime.Serialization.DataMember]
        public virtual string TEAMID
        {
            get
            {
                return _TEAMID;
            }
            set
            {
                if (_TEAMID != value)
                {
                    _TEAMID = value;
                    OnPropertyChanged("TEAMID");
                }
            }
        }
        private string _TEAMID;

        [System.Runtime.Serialization.DataMember]
        public virtual bool? PROFITCLOSE
        {
            get
            {
                return _PROFITCLOSE;
            }
            set
            {
                if (_PROFITCLOSE != value)
                {
                    _PROFITCLOSE = value;
                    OnPropertyChanged("PROFITCLOSE");
                }
            }
        }
        private bool? _PROFITCLOSE;

        [System.Runtime.Serialization.DataMember]
        public virtual string TRACKINGNO
        {
            get
            {
                return _TRACKINGNO;
            }
            set
            {
                if (_TRACKINGNO != value)
                {
                    _TRACKINGNO = value;
                    OnPropertyChanged("TRACKINGNO");
                }
            }
        }
        private string _TRACKINGNO;

        [System.Runtime.Serialization.DataMember]
        public virtual string BANKID
        {
            get
            {
                return _BANKID;
            }
            set
            {
                if (_BANKID != value)
                {
                    _BANKID = value;
                    OnPropertyChanged("BANKID");
                }
            }
        }
        private string _BANKID;

        [System.Runtime.Serialization.DataMember]
        public virtual string BUYERID
        {
            get
            {
                return _BUYERID;
            }
            set
            {
                if (_BUYERID != value)
                {
                    _BUYERID = value;
                    OnPropertyChanged("BUYERID");
                }
            }
        }
        private string _BUYERID;

        [System.Runtime.Serialization.DataMember]
        public virtual string IIGTYPE
        {
            get
            {
                return _IIGTYPE;
            }
            set
            {
                if (_IIGTYPE != value)
                {
                    _IIGTYPE = value;
                    OnPropertyChanged("IIGTYPE");
                }
            }
        }
        private string _IIGTYPE;

        [System.Runtime.Serialization.DataMember]
        public virtual string LOGITUDEFILE
        {
            get
            {
                return _LOGITUDEFILE;
            }
            set
            {
                if (_LOGITUDEFILE != value)
                {
                    _LOGITUDEFILE = value;
                    OnPropertyChanged("LOGITUDEFILE");
                }
            }
        }
        private string _LOGITUDEFILE;

        [System.Runtime.Serialization.DataMember]
        public virtual decimal? CIFVALUE
        {
            get
            {
                return _CIFVALUE;
            }
            set
            {
                if (_CIFVALUE != value)
                {
                    _CIFVALUE = value;
                    OnPropertyChanged("CIFVALUE");
                }
            }
        }
        private decimal? _CIFVALUE;

        [System.Runtime.Serialization.DataMember]
        public virtual decimal? TOTALTAX
        {
            get
            {
                return _TOTALTAX;
            }
            set
            {
                if (_TOTALTAX != value)
                {
                    _TOTALTAX = value;
                    OnPropertyChanged("TOTALTAX");
                }
            }
        }
        private decimal? _TOTALTAX;

        [System.Runtime.Serialization.DataMember]
        public virtual string DECLARSTSCODE
        {
            get
            {
                return _DECLARSTSCODE;
            }
            set
            {
                if (_DECLARSTSCODE != value)
                {
                    _DECLARSTSCODE = value;
                    OnPropertyChanged("DECLARSTSCODE");
                }
            }
        }
        private string _DECLARSTSCODE;

        [System.Runtime.Serialization.DataMember]
        public virtual bool? PACKSFLAG
        {
            get
            {
                return _PACKSFLAG;
            }
            set
            {
                if (_PACKSFLAG != value)
                {
                    _PACKSFLAG = value;
                    OnPropertyChanged("PACKSFLAG");
                }
            }
        }
        private bool? _PACKSFLAG;

        [System.Runtime.Serialization.DataMember]
        public virtual string INSCOMPANY
        {
            get
            {
                return _INSCOMPANY;
            }
            set
            {
                if (_INSCOMPANY != value)
                {
                    _INSCOMPANY = value;
                    OnPropertyChanged("INSCOMPANY");
                }
            }
        }
        private string _INSCOMPANY;

        [System.Runtime.Serialization.DataMember]
        public virtual string POLICYNO
        {
            get
            {
                return _POLICYNO;
            }
            set
            {
                if (_POLICYNO != value)
                {
                    _POLICYNO = value;
                    OnPropertyChanged("POLICYNO");
                }
            }
        }
        private string _POLICYNO;

        [System.Runtime.Serialization.DataMember]
        public virtual string INSBYUS
        {
            get
            {
                return _INSBYUS;
            }
            set
            {
                if (_INSBYUS != value)
                {
                    _INSBYUS = value;
                    OnPropertyChanged("INSBYUS");
                }
            }
        }
        private string _INSBYUS;

        [System.Runtime.Serialization.DataMember]
        public virtual string WAREHOUSEID
        {
            get
            {
                return _WAREHOUSEID;
            }
            set
            {
                if (_WAREHOUSEID != value)
                {
                    _WAREHOUSEID = value;
                    OnPropertyChanged("WAREHOUSEID");
                }
            }
        }
        private string _WAREHOUSEID;

        [System.Runtime.Serialization.DataMember]
        public virtual string UNLOADPORTID
        {
            get
            {
                return _UNLOADPORTID;
            }
            set
            {
                if (_UNLOADPORTID != value)
                {
                    _UNLOADPORTID = value;
                    OnPropertyChanged("UNLOADPORTID");
                }
            }
        }
        private string _UNLOADPORTID;

        [System.Runtime.Serialization.DataMember]
        public virtual string BALDARHAWB
        {
            get
            {
                return _BALDARHAWB;
            }
            set
            {
                if (_BALDARHAWB != value)
                {
                    _BALDARHAWB = value;
                    OnPropertyChanged("BALDARHAWB");
                }
            }
        }
        private string _BALDARHAWB;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? BALDARHAWBDATE
        {
            get
            {
                return _BALDARHAWBDATE;
            }
            set
            {
                if (_BALDARHAWBDATE != value)
                {
                    _BALDARHAWBDATE = value;
                    OnPropertyChanged("BALDARHAWBDATE");
                }
            }
        }
        private global::System.DateTime? _BALDARHAWBDATE;

        [System.Runtime.Serialization.DataMember]
        public virtual double? SHIPUSDVAL
        {
            get
            {
                return _SHIPUSDVAL;
            }
            set
            {
                if (_SHIPUSDVAL != value)
                {
                    _SHIPUSDVAL = value;
                    OnPropertyChanged("SHIPUSDVAL");
                }
            }
        }
        private double? _SHIPUSDVAL;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? ARRIVALTIME
        {
            get
            {
                return _ARRIVALTIME;
            }
            set
            {
                if (_ARRIVALTIME != value)
                {
                    _ARRIVALTIME = value;
                    OnPropertyChanged("ARRIVALTIME");
                }
            }
        }
        private global::System.DateTime? _ARRIVALTIME;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? ESTARRIVALTIME
        {
            get
            {
                return _ESTARRIVALTIME;
            }
            set
            {
                if (_ESTARRIVALTIME != value)
                {
                    _ESTARRIVALTIME = value;
                    OnPropertyChanged("ESTARRIVALTIME");
                }
            }
        }
        private global::System.DateTime? _ESTARRIVALTIME;

        [System.Runtime.Serialization.DataMember]
        public virtual global::System.DateTime? LASTUPDATETIME
        {
            get
            {
                return _LASTUPDATETIME;
            }
            set
            {
                if (_LASTUPDATETIME != value)
                {
                    _LASTUPDATETIME = value;
                    OnPropertyChanged("LASTUPDATETIME");
                }
            }
        }
        private global::System.DateTime? _LASTUPDATETIME;

        [System.Runtime.Serialization.DataMember]
        public virtual string OLDCUSTPACKTYPEID
        {
            get
            {
                return _OLDCUSTPACKTYPEID;
            }
            set
            {
                if (_OLDCUSTPACKTYPEID != value)
                {
                    _OLDCUSTPACKTYPEID = value;
                    OnPropertyChanged("OLDCUSTPACKTYPEID");
                }
            }
        }
        private string _OLDCUSTPACKTYPEID;

        [System.Runtime.Serialization.DataMember]
        public virtual string COUWTVAL
        {
            get
            {
                return _COUWTVAL;
            }
            set
            {
                if (_COUWTVAL != value)
                {
                    _COUWTVAL = value;
                    OnPropertyChanged("COUWTVAL");
                }
            }
        }
        private string _COUWTVAL;

        [System.Runtime.Serialization.DataMember]
        public virtual string UNIQUECHECK
        {
            get
            {
                return _UNIQUECHECK;
            }
            set
            {
                if (_UNIQUECHECK != value)
                {
                    _UNIQUECHECK = value;
                    OnPropertyChanged("UNIQUECHECK");
                }
            }
        }
        private string _UNIQUECHECK;

        [System.Runtime.Serialization.DataMember]
        public virtual string INTEGRATORID
        {
            get
            {
                return _INTEGRATORID;
            }
            set
            {
                if (_INTEGRATORID != value)
                {
                    _INTEGRATORID = value;
                    OnPropertyChanged("INTEGRATORID");
                }
            }
        }
        private string _INTEGRATORID;

        [System.Runtime.Serialization.DataMember]
        public virtual string SHOPID
        {
            get
            {
                return _SHOPID;
            }
            set
            {
                if (_SHOPID != value)
                {
                    _SHOPID = value;
                    OnPropertyChanged("SHOPID");
                }
            }
        }
        private string _SHOPID;

        [System.Runtime.Serialization.DataMember]
        public virtual string LEADFILE
        {
            get
            {
                return _LEADFILE;
            }
            set
            {
                if (_LEADFILE != value)
                {
                    _LEADFILE = value;
                    OnPropertyChanged("LEADFILE");
                }
            }
        }
        private string _LEADFILE;

        [System.Runtime.Serialization.DataMember]
        public virtual string SERVLEVELID
        {
            get
            {
                return _SERVLEVELID;
            }
            set
            {
                if (_SERVLEVELID != value)
                {
                    _SERVLEVELID = value;
                    OnPropertyChanged("SERVLEVELID");
                }
            }
        }
        private string _SERVLEVELID;

        [System.Runtime.Serialization.DataMember]
        public virtual string WITHPAPER
        {
            get
            {
                return _WITHPAPER;
            }
            set
            {
                if (_WITHPAPER != value)
                {
                    _WITHPAPER = value;
                    OnPropertyChanged("WITHPAPER");
                }
            }
        }
        private string _WITHPAPER;
#endif
        #endregion

        #region Extensibility Method Definitions
        partial void OnCreated();
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

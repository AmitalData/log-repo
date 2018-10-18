using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using System.ComponentModel;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;


namespace Logitude.Accounting.BL.Extensions
{
    public class LedgerTransactionExtension : IComparable<LedgerTransactionExtension>
    {
        private string currencyCode;
        private string originalCurrencyId;
        private string originalCurrencyCode;
        private string accountingCurrencyId;
        private string accountingCurrencyCode;
        private string openAmountCurrencyCode;
        private IAccountingContext accountingContext;
        public LedgerTransactionExtension()
        {
            this.mark = false;
        }

        public LedgerTransactionExtension(LedgerTransactionList ledgerTransactionList, string reconcileMethodCode, string accountingCurrencyId, string accountingCurrencyCode)
        {
            accountingContext = AccountingContext.GetContext(ledgerTransactionList.Tenant); 
            this.ledgerTransactionList = ledgerTransactionList;
            this.mark = false;
            this.reconcileMethodCode = reconcileMethodCode;
            this.accountingCurrencyId = accountingCurrencyId;
            this.accountingCurrencyCode = accountingCurrencyCode;
            CurrencyQuery currencyQueryService = new CurrencyQuery(ledgerTransactionList.Tenant);
            this.OpenAmount = ledgerTransactionList.OpenAmount;
            if (reconcileMethodCode == "1") // foreign currency
            {
                OriginalCurrencyId = ledgerTransactionList.CurrencyId;
                OriginalCurrencyCode = ledgerTransactionList.CurrencyCode;
                if (ledgerTransactionList.ForeignAmountCredit == 0)
                {
                    OriginalAmount = -ledgerTransactionList.ForeignAmountDebit ;
                }
                else
                {
                    OriginalAmount = ledgerTransactionList.ForeignAmountCredit ;
                }
            }
            else // local currency
            {

                OriginalCurrencyId = accountingCurrencyId;
                if (String.IsNullOrEmpty(accountingCurrencyCode))
                {
                    //CurrencyQuery currencyQueryService = new CurrencyQuery(ledgerTransactionList.Tenant);
                    //CurrencyPM currency = currencyQueryService.GetSinglePM(accountingCurrencyId, ledgerTransactionList.Tenant);

                    
                    //CurrencyList currency = TenantContext.Current.CommonDataContext.CurrencyLists.Where(d => d.Id == OriginalCurrencyId).FirstOrDefault();
                    //if (currency != null) accountingCurrencyCode = currency.Code;
                }
                OriginalCurrencyCode = accountingCurrencyCode;
                if (ledgerTransactionList.LocalAmountCredit == 0)
                {
                    OriginalAmount = -ledgerTransactionList.LocalAmountDebit ;
                }
                else
                {
                    OriginalAmount = ledgerTransactionList.LocalAmountCredit ;
                }
            }


            CurrencyPM currency = currencyQueryService.GetSinglePM(ledgerTransactionList.CurrencyId, ledgerTransactionList.Tenant);
            if (currency != null)
            {
                CurrencyCode = currency.Code;
            }

            if (!String.IsNullOrEmpty(ledgerTransactionList.OpenAmountCurrencyId))
            {
                CurrencyPM openCurrency = currencyQueryService.GetSinglePM(ledgerTransactionList.OpenAmountCurrencyId, ledgerTransactionList.Tenant);
                if (openCurrency != null)
                {
                    CurrencyCode = openCurrency.Code;
                }
            }
            else if (currency != null)
            {
                OpenAmountCurrencyCode = currency.Code;
            }

            //if (trigger.reconciliationPM != null)
            //{
            //    ReconciliationLinePM recoLine = trigger.reconciliationPM.ReconciliationLines.Where(d => d.TransactionId == ledgerTransactionList.Id).FirstOrDefault();
            //    if (recoLine != null)
            //    {
            //        Mark = true;
            //        AmountToReconcile = recoLine.ReconciliationAmount;
            //    }
            //}
            if (Mark == true)
            {
                IsEnabled = true;
            }
            else
            {
                IsEnabled = false;
            }
        }

        #region Sorting
        private decimal _xDecimal = 0;
        private decimal _yDecimal = 0;
        private decimal _zDecimal = 0;

        private DateTime? _xDate = null;
        private DateTime? _yDate = null;
        private DateTime? _zDate = null;

        private string _xString = "";
        private string _yString = "";
        private string _zString = "";

        public decimal xDecimal { get { return _xDecimal; } set { _xDecimal = value; } }
        public decimal yDecimal { get { return _yDecimal; } set { _yDecimal = value; } }
        public decimal zDecimal { get { return _zDecimal; } set { _zDecimal = value; } }

        public DateTime? xDate { get { return _xDate; } set { if (value.HasValue) _xDate = value.Value.Date; } }
        public DateTime? yDate { get { return _yDate; } set { if (value.HasValue) _yDate = value.Value.Date; } }
        public DateTime? zDate { get { return _zDate; } set { if (value.HasValue) _zDate = value.Value.Date; } }

        public string xString { get { return _xString; } set { _xString = value; } }
        public string yString { get { return _yString; } set { _yString = value; } }
        public string zString { get { return _zString; } set { _zString = value; } }

        public void SetXYZ(string method1, string method2, string method3)
        {
            switch (method1)
            {
                case "1":
                    if (ledgerTransactionList.OpenAmount >= 0)
                        xDecimal = ledgerTransactionList.OpenAmount;
                    else
                        xDecimal = -ledgerTransactionList.OpenAmount;
                    break;
                case "2":
                    xDate = ledgerTransactionList.DocumentDate;
                    break;
                case "3":
                    xDate = ledgerTransactionList.DueDate;
                    break;
                case "4":
                    xDate = ledgerTransactionList.AccountingDate;
                    break;
                case "5":
                    xString = ledgerTransactionList.Reference1;
                    break;
                case "6":
                    xString = ledgerTransactionList.Reference2;
                    break;
                case "7":
                    xString = ledgerTransactionList.Reference3;
                    break;
                default:
                    break;
            }
            switch (method2)
            {
                case "1":
                    if (ledgerTransactionList.OpenAmount >= 0)
                        yDecimal = ledgerTransactionList.OpenAmount;
                    else
                        yDecimal = -ledgerTransactionList.OpenAmount;
                    break;
                case "2":
                    yDate = ledgerTransactionList.DocumentDate;
                    break;
                case "3":
                    yDate = ledgerTransactionList.DueDate;
                    break;
                case "4":
                    yDate = ledgerTransactionList.AccountingDate;
                    break;
                case "5":
                    yString = ledgerTransactionList.Reference1;
                    break;
                case "6":
                    yString = ledgerTransactionList.Reference2;
                    break;
                case "7":
                    yString = ledgerTransactionList.Reference3;
                    break;
                default:
                    break;
            }
            switch (method3)
            {
                case "1":
                    if (ledgerTransactionList.OpenAmount >= 0)
                        zDecimal = ledgerTransactionList.OpenAmount;
                    else
                        zDecimal = -ledgerTransactionList.OpenAmount;
                    break;
                case "2":
                    zDate = ledgerTransactionList.DocumentDate;
                    break;
                case "3":
                    zDate = ledgerTransactionList.DueDate;
                    break;
                case "4":
                    zDate = ledgerTransactionList.AccountingDate;
                    break;
                case "5":
                    zString = ledgerTransactionList.Reference1;
                    break;
                case "6":
                    zString = ledgerTransactionList.Reference2;
                    break;
                case "7":
                    zString = ledgerTransactionList.Reference3;
                    break;
                default:
                    break;
            }
        }

        //        private class sortLedgerTransactionExtensionHelper : IComparer
        private class sortLedgerTransactionExtensionHelper : IComparer<LedgerTransactionExtension>
        {
            private string _datatype1;
            private string _datatype2;
            private string _datatype3;

            public sortLedgerTransactionExtensionHelper(string key1, string key2, string key3)
            {
                _datatype1 = GetDataType(key1);
                _datatype2 = GetDataType(key2);
                _datatype3 = GetDataType(key3);
            }

            static string GetDataType(string key)
            {
                string rv;
                if (key == "1")
                    rv = "decimal";
                else if (key == "2" || key == "3" || key == "4")
                    rv = "date";
                else if (key == "5" || key == "6" || key == "7")
                    rv = "string";
                else
                    rv = "";
                return rv;
            }

            public int Compare(LedgerTransactionExtension a, LedgerTransactionExtension b)
            {
                LedgerTransactionExtension lt1 = (LedgerTransactionExtension)a;
                LedgerTransactionExtension lt2 = (LedgerTransactionExtension)b;
                if (_datatype1 == "decimal")
                {
                    if (lt1.xDecimal > lt2.xDecimal)
                        return 1;
                    if (lt1.xDecimal < lt2.xDecimal)
                        return -1;
                    else return CompareY(a, b);
                }
                else if (_datatype1 == "date")
                {
                    if (lt1.xDate.GetValueOrDefault() > lt2.xDate.GetValueOrDefault())
                        return 1;
                    if (lt1.xDate.GetValueOrDefault() < lt2.xDate.GetValueOrDefault())
                        return -1;
                    else return CompareY(a, b);
                }
                else if (_datatype1 == "string")
                {
                    int result = String.Compare(lt1.xString, lt2.xString, StringComparison.OrdinalIgnoreCase);
                    if (result == 1 || result == -1)
                        return result;
                    else return CompareY(a, b);
                }
                else
                    return 0;
            }

            int CompareY(object a, object b)
            {
                LedgerTransactionExtension lt1 = (LedgerTransactionExtension)a;
                LedgerTransactionExtension lt2 = (LedgerTransactionExtension)b;
                if (_datatype1 == "decimal")
                {
                    if (lt1.yDecimal > lt2.yDecimal)
                        return 1;
                    if (lt1.yDecimal < lt2.yDecimal)
                        return -1;
                    else return CompareZ(a, b);
                }
                else if (_datatype1 == "date")
                {
                    if (lt1.yDate.GetValueOrDefault() > lt2.yDate.GetValueOrDefault())
                        return 1;
                    if (lt1.yDate.GetValueOrDefault() < lt2.yDate.GetValueOrDefault())
                        return -1;
                    else return CompareZ(a, b);
                }
                else if (_datatype1 == "string")
                {
                    int result = String.Compare(lt1.yString, lt2.yString, StringComparison.OrdinalIgnoreCase);
                    if (result == 1 || result == -1)
                        return result;
                    else return CompareZ(a, b);
                }
                else
                    return 0;
            }

            int CompareZ(object a, object b)
            {
                LedgerTransactionExtension lt1 = (LedgerTransactionExtension)a;
                LedgerTransactionExtension lt2 = (LedgerTransactionExtension)b;
                if (_datatype1 == "decimal")
                {
                    if (lt1.zDecimal > lt2.zDecimal)
                        return 1;
                    if (lt1.zDecimal < lt2.zDecimal)
                        return -1;
                    else return 0;
                }
                else if (_datatype1 == "date")
                {
                    if (lt1.zDate.GetValueOrDefault() > lt2.zDate.GetValueOrDefault())
                        return 1;
                    if (lt1.zDate.GetValueOrDefault() < lt2.zDate.GetValueOrDefault())
                        return -1;
                    else return 0;
                }
                else if (_datatype1 == "string")
                {
                    int result = String.Compare(lt1.zString, lt2.zString, StringComparison.OrdinalIgnoreCase);
                    if (result == 1 || result == -1)
                        return result;
                    else return 0;
                }
                else
                    return 0;
            }
        }

        // Method to return IComparer object for sort helper.
        public static IComparer<LedgerTransactionExtension> sortLedgerTransactionExtension(string key1, string key2, string key3)
        {
            return new sortLedgerTransactionExtensionHelper(key1, key2, key3);
        }

        public int CompareTo(LedgerTransactionExtension obj)// Implements IComparable CompareTo to provide default sort order.
        {
            LedgerTransactionExtension c = (LedgerTransactionExtension)obj;
            int tempres = DateTime.Compare(this.ledgerTransactionList.AccountingDate, c.ledgerTransactionList.AccountingDate);
            if (tempres != 0)
            {
                return tempres;
            }
            else
            {
                return String.Compare(this.ledgerTransactionList.JournalNumber, c.ledgerTransactionList.JournalNumber);
            }
        }
        #endregion


        public string reconcileMethodCode;

        #region Mark
        private bool mark;
        public bool Mark
        {
            get { return this.mark; }
            set
            {
                if (this.mark != value)
                {
                    this.mark = value;
                    if (Mark == true)
                    {
                        AmountToReconcile = OpenAmount;
                        IsEnabled = true;
                    }
                    else
                    {
                        if (this.HasErrors) RemoveErrorFromPropertyAndNotifyErrorChanges("AmountToReconcile", 101);
                        AmountToReconcile = null;
                        IsEnabled = false;
                    }
                }
            }
        }
        #endregion

        public string CurrencyCode
        {
            get
            {
                return currencyCode;
            }
            set
            {
                if (currencyCode != value)
                {
                    currencyCode = value;
                }
            }
        }


        #region AmountToReconcile
        private decimal? amountToReconcile;
        public decimal? AmountToReconcile
        {
            get { return amountToReconcile; }
            set
            {
                if (value.HasValue && value.Value * OpenAmount < 0)
                {
                    string errorMsgSign = TextCodesTranslator.TranslateText("GLAccounts.O.AmountToReconcileWrongSign", ledgerTransactionList.Tenant);
                    AddErrorToPropertyAndNotifyErrorChanges("AmountToReconcile", new ClassValidationErrorInfo() { ErrorMessage = errorMsgSign, ErrorCode = 101 });
                }
                else if (value.HasValue && ((value.Value > OpenAmount && OpenAmount > 0) || (value.Value < OpenAmount && OpenAmount < 0)))
                {
                    string errorMsgBig = TextCodesTranslator.TranslateText("GLAccounts.O.AmountToReconcileTooBig", ledgerTransactionList.Tenant);
                    AddErrorToPropertyAndNotifyErrorChanges("AmountToReconcile", new ClassValidationErrorInfo() { ErrorMessage = errorMsgBig, ErrorCode = 101 });
                }
                else
                {
                    if (this.HasErrors) this.RemoveErrorFromPropertyAndNotifyErrorChanges("AmountToReconcile", 101);
                }

                if (amountToReconcile != value)
                {
                    if (value.HasValue)
                    {
                        if (value.Value * OpenAmount < 0)
                        {
                        }
                        else if ((value.Value > OpenAmount && OpenAmount > 0) || (value.Value < OpenAmount && OpenAmount < 0))
                        {
                        }
                        else
                        {
                            amountToReconcile = value.GetValueOrDefault();
                            if (amountToReconcile == 0) amountToReconcile = null;
                           // if (!Trigger.Loading) Trigger.ComputeTotals();
                        }
                    }
                    else
                    {
                        amountToReconcile = value.GetValueOrDefault();
                        if (amountToReconcile == 0) amountToReconcile = null;
                    }
                }
            }
        }
        #endregion



        #region OriginalCurrency
        public string OriginalCurrencyId
        {
            get { return originalCurrencyId; }
            set
            {
                if (originalCurrencyId != value)
                {

                    if (value != null)
                    {
             ////           CurrencyList currency = TenantContext.Current.CommonDataContext.CurrencyLists.Where(d => d.Id == value).FirstOrDefault();

                        ////if (currency != null)
                        ////{
                        originalCurrencyId = value;
                        ////    originalCurrencyCode = currency.Code;
                        ////}
                    }
                    else
                    {
                        originalCurrencyId = null;
                        originalCurrencyCode = null;
                    }
                }

            }
        }

        public string OriginalCurrencyCode
        {
            get { return originalCurrencyCode; }
            set
            {
                if (originalCurrencyCode != value)
                {
                    originalCurrencyCode = value;
                }
            }
        }
        #endregion

        #region OriginalAmount
        decimal originalAmount;
        public decimal OriginalAmount
        {
            get { return originalAmount; }
            set
            {
                if (originalAmount != value)
                {
                    originalAmount = value;
                }
            }
        }
        #endregion

        #region OpenAmountCurrency
        public string OpenAmountCurrencyId
        {
            get { return !String.IsNullOrEmpty(ledgerTransactionList.OpenAmountCurrencyId) ? ledgerTransactionList.OpenAmountCurrencyId : originalCurrencyId; }
            set
            {
                //if (ledgerTransactionList.OpenAmountCurrencyId != value)
                //{

                //    if (value != null)
                //    {
                //        CurrencyList currency = TenantContext.Current.CommonDataContext.CurrencyLists.Where(d => d.Id == value).FirstOrDefault();

                //        if (currency != null)
                //        {
                //            ledgerTransactionList.OpenAmountCurrencyId = currency.Id;
                //            openAmountCurrencyCode = currency.Code;
                //        }
                //    }
                //    else
                //    {
                //        ledgerTransactionList.OpenAmountCurrencyId = null;
                //        openAmountCurrencyCode = null;
                //    }
                //}

            }
        }

        public string OpenAmountCurrencyCode
        {
            get { return openAmountCurrencyCode; }
            set
            {
                if (openAmountCurrencyCode != value)
                {
                    openAmountCurrencyCode = value;
                }
            }
        }
        #endregion



        #region IsEnabled
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                }
            }
        }
        #endregion

        #region OpenAmount
        decimal openAmount;
        public decimal OpenAmount
        {
            get { return openAmount; }
            set
            {
                if (openAmount != value)
                {
                    openAmount = value;
                }
            }
        }
        #endregion

        //#region LedgerTransactionList
        public LedgerTransactionList ledgerTransactionList;
        //public LedgerTransactionList LedgerTransactionList
        //{
        //    get { return ledgerTransactionList; }
        //    set
        //    {
        //        if (ledgerTransactionList != value)
        //        {
        //            ledgerTransactionList = value;
        //        }
        //    }
        //}
        //#endregion

        #region Errors
        public class ClassValidationErrorInfo
        {
            public string MessageType { get; set; }
            public int ErrorCode { get; set; }

            public string ErrorMessage { get; set; }

            public override string ToString()
            {
                return ErrorMessage;
            }
        }
        private Dictionary<string, List<ClassValidationErrorInfo>> errors = new Dictionary<string, List<ClassValidationErrorInfo>>();
        public List<ClassValidationErrorInfo> ValidationErrors = new List<ClassValidationErrorInfo>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (errors != null)
            {
                if (!errors.ContainsKey(propertyName))
                {
                    return errors.Values;
                }

                else
                {
                    return errors[propertyName];
                }
            }

            else
            {
                return null;
            }
        }

        public bool HasErrors
        {
            get { return this.errors.Count > 0; }

        }
        private void NotifyErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void RemoveErrorFromPropertyAndNotifyErrorChanges(
                string propertyName,
                int errorCode)
        {
            if (errors.ContainsKey(propertyName))
            {
                var errorToRemove = errors[propertyName].SingleOrDefault(error => error.ErrorCode == errorCode);

                if (errorToRemove != null)
                {
                    errors[propertyName].Remove(errorToRemove);
                    errors.Remove(propertyName);
                    ValidationErrors.Remove(errorToRemove);
                    NotifyErrorsChanged(propertyName);
                }
            }

        }

        public void AddErrorToPropertyAndNotifyErrorChanges(
            string propertyName,
            ClassValidationErrorInfo errorInfo)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors.Add(propertyName, new List<ClassValidationErrorInfo>());

                errors[propertyName].Add(errorInfo);

                ValidationErrors.Add(errorInfo);

                NotifyErrorsChanged(propertyName);
            }
        }
        #endregion

  

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.ImportDeclaration
{
#if false
    class DeclarationUpsertService
    {
        void OldProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            DataOut1 = DataOut2 = MessageOut = "";
            SUCCESS = false.ToString();

            try
            {

                MyGenericResponseObj.Stage = "Initalize ProccessRequest";
                _sbLog.AppendLine("DeclarationUpsertService.ProccessRequest");

                _sbLog.AppendLine("Deserialize(DataIn1) ..");


                //int tenant;
                //if (!int.TryParse(stenant, out tenant))
                //{
                //    throw new Exception("Tenant is not int  !!!");
                //}

                if (string.IsNullOrWhiteSpace(DataIn1))
                {

                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "DataIn1 is missing !!!";
                    return;
                    //_GenericResponseObj.ExceptionType=""
                    //throw new Exception("DataIn1 is missing !!!");
                }
                if (DataIn1.Length > 1000)
                {
                    _sbLog.AppendLine("XmlIn=" + DataIn1.Substring(0, 1000));
                    _sbLog.AppendLine(".Substring(0, 1000)");
                }
                else
                {
                    _sbLog.AppendLine("XmlIn=" + DataIn1);
                }
                _sbLog.AppendLine("Tring DeserilazeObject");
                MyGenericResponseObj.Stage = "Tring DeserilazeObject";
                this._LOGICUSTFILE = XmlGenericUtil<LOGICUSTFILE>.DeSerializeObject(DataIn1);

                if (_LOGICUSTFILE.LogitudeCustomsFile == null || _LOGICUSTFILE.LogitudeCustomsFile.Length != 1)
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "customFile.LogitudeCustomsFile.Length !=1 !!!";
                    return;
                    //throw new Exception("customFile.LogitudeCustomsFile.Length !=1 !!!");
                }

                this._AmitalCustomsFile = _LOGICUSTFILE.LogitudeCustomsFile[0];
                MyGenericResponseObj.Stage = "Upsert";
                Upsert();
                SUCCESS = true.ToString();



            }
            catch (DbEntityValidationException ex)
            {
                var formatedException = ExceptionFormatUtil.GetFormated(ex);
                _sbLog.Insert(0, "ProccessRequest():Exception " + formatedException.ToString() + Environment.NewLine + "---------------------------------------------");
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("ProccessRequest():Exception " + formatedException.ToString(), true);
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "Error while DeclarationUpdateService.Update " + formatedException.Message;
                MyGenericResponseObj.ErrorDescription = formatedException.ToString();
                if (formatedException.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = formatedException.InnerException.ToString();
                }
            }
            catch (Exception e)
            {

                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;
                MyGenericResponseObj.Message = "Exception: " + e.Message;
                MyGenericResponseObj.ErrorDescription = e.ToString();
                if (e.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = e.InnerException.ToString();
                }

            }
            finally
            {
                //_GenericResponseObj 
                try
                {
                    DataOut1 = XmlGenericUtil<GenericResponseObj>.SerializeObject(MyGenericResponseObj);

                    MyGenericResponseObj = null;
                }
                catch (Exception)
                {

                    ///                    throw;
                }

            }
        }
    }
#endif
}

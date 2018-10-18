using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CollateralAnswerMsgServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class COLT_NG_8212_MSG10041_CollateralAnswerMsgRequestService : RequestServiceBase
        <COLT_NG_8212_MSG10041_CollateralAnswerMsg, CollateralRequestParams>
    {
        public override COLT_NG_8212_MSG10041_CollateralAnswerMsg GetRequest(CollateralRequestParams requestParams)
        {
            //Build request 8212 - Message Request for Collateral
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var customsCollateralQueryService = new CustomsCollateralQueryService(dbContext);
            int customsCollateralNumber;

            var myCOLT_NG_8212_MSG10041_CollateralAnswerMsg = new COLT_NG_8212_MSG10041_CollateralAnswerMsg();
            myCOLT_NG_8212_MSG10041_CollateralAnswerMsg.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            CustomsCollateralPM CustomsCollateralPM = customsCollateralQueryService.GetSingle(requestParams.CustomCollateralId, true, false);
            int.TryParse(CustomsCollateralPM.CollateralRequestNumber, out customsCollateralNumber);
            
            var myCollateralAnswersList = new List<AnswerForCollateralAnswersList>();
            foreach (var collateralAnswerItem in CustomsCollateralPM.CustomsCollateralsAnswers)
            {
                var myCollateralAnswer = new AnswerForCollateralAnswersList();
                int answerEntityType;
                int numeral;

                if (!collateralAnswerItem.IsClosed) //Yuval Chalup 06.12.2016 TASK-24839 (Do NOT send CLOSED collateralAnswerItem)
                {
                    //myCollateralAnswer.remarks = collateralAnswerItem.Remarks; // todo - XSD not update
                    if (collateralAnswerItem.NewFileRequest == true)
                    {
                        myCollateralAnswer.IsNewRequest = true;
                        myCollateralAnswer.allocatedAmount = (decimal)collateralAnswerItem.RequestFileAmount;
                        int.TryParse(collateralAnswerItem.RequestFileTypeCode, out answerEntityType);
                        myCollateralAnswer.answerEntityType = answerEntityType;

                        if (collateralAnswerItem.CollateralsRequestFileConds != null)
                        {
                            var myCollateralAnswersConditionList = new List<AnswerForCollateralAnswersListConditions>();
                            foreach (var collateralConditionItem in collateralAnswerItem.CollateralsRequestFileConds)
                            {
                                AnswerForCollateralAnswersListConditions answerCondition = new AnswerForCollateralAnswersListConditions();
                                int conditionCode;
                                int.TryParse(collateralConditionItem.ConditionCode, out conditionCode);
                                answerCondition.Condition = conditionCode;
                                answerCondition.Amount = (decimal)collateralConditionItem.RequestedAmount;

                                myCollateralAnswersConditionList.Add(answerCondition);
                            }
                            myCollateralAnswer.Conditions = myCollateralAnswersConditionList.ToArray();
                        }
                    }
                    else
                    {
                        myCollateralAnswer.IsNewRequest = false;
                        myCollateralAnswer.allocatedAmount = (decimal)collateralAnswerItem.AllocatedAmount;
                        int.TryParse(collateralAnswerItem.AnswerEntityTypeCode, out answerEntityType);
                        myCollateralAnswer.answerEntityType = answerEntityType;

                        myCollateralAnswer.TPGIdentifier = new TPGIdentifier();
                        myCollateralAnswer.TPGIdentifier.fileNumber = collateralAnswerItem.CustomsTapgFile;
                        int.TryParse(collateralAnswerItem.CustomsNumeral, out numeral);
                        myCollateralAnswer.TPGIdentifier.numeral = numeral;
                    }

                    myCollateralAnswersList.Add(myCollateralAnswer);
                }
            }

            myCOLT_NG_8212_MSG10041_CollateralAnswerMsg.AnswerForCollateralRequest = new AnswerForCollateral[1];
            myCOLT_NG_8212_MSG10041_CollateralAnswerMsg.AnswerForCollateralRequest[0] = new AnswerForCollateral();
            myCOLT_NG_8212_MSG10041_CollateralAnswerMsg.AnswerForCollateralRequest[0].AnswersList = myCollateralAnswersList.ToArray();
            myCOLT_NG_8212_MSG10041_CollateralAnswerMsg.AnswerForCollateralRequest[0].collateralRequestNumber = customsCollateralNumber;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "מענה לדרישת בטוחה " + customsCollateralNumber;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral");
            this.MyRequestSheetParam.EntityId1 = requestParams.CustomCollateralId;
            this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId2 = CustomsCollateralPM.DeclarationId;

            return myCOLT_NG_8212_MSG10041_CollateralAnswerMsg;
        }
    }
}

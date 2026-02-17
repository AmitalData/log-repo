using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AmitalMessaging.Infrastructure.Transmission
{
    public class Factory<T>
    {
        private Factory() { }

        static readonly Dictionary<int, Func<T>> _dict
             = new Dictionary<int, Func<T>>();

        public static T Create(int id)
        {
            Func<T> constructor = null;
            if (_dict.TryGetValue(id, out constructor))
                return constructor();

            throw new ArgumentException("No type registered for this id");
        }

        public static void Register(int id, Func<T> ctor)
        {
            _dict.Add(id, ctor);
        }
    }

//    public class GenrateTrasmission
//    {
//        public static string GetTrasmissionCFIFILEM()
//        {
//            var mytransmission = new transmission();
//            var mytransmission_details = new List<transmission_details>();
//            var mytransmission_detail1 = new transmission_details()
//            {
//                sender = new sender() { Value = "AMITAL" },
//                subject = new subject() { Value = "custom file  From Logitutde " }
//            };

//            var myArrayOfdata = new List<data>();
//            var dataFUStatus = new data();
//            dataFUStatus.entity = UnifreightIIG.Common.MessageLib.Unifreight.Customs.
//                FileGenrator.GetCFIFILEMTest();
//            myArrayOfdata.Add(dataFUStatus);
//            mytransmission.data = myArrayOfdata.ToArray();
//            mytransmission_details.Add(mytransmission_detail1);
//            mytransmission.transmission_details = mytransmission_details.ToArray();

//            var mySerilazeObject = UnifreightIIG.Common.Utils.XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);

//            Debug.WriteLine(mySerilazeObject);

//            //GGGHQANALYZE >>  GGGHQANALYZE
//            /*
//Entry Lp_RunExec
//Params
//    String p_com_id 	  :in
//    String p_errmess :out
//EndParams
//variables
//    string v_in,v_out
//    Numeric v_status
//endvariables
//    $$GGG_OUT = ""
//    PutItem/Id v_in,"COM_ID",p_com_id
//    PutItem/Id v_in,"ORIGIN_QUE",$origin_que$
//    PutItem/Id v_in,"FORCE_ANALYZE","1"
//    PutItem/Id v_in,"QUE_ID",QUE_ID.GGGQ
//    Call Lp_SyncLog("Activate GCCFANALYZEII, COM_ID=%%p_com_id",0)	
//    Call G_LOG(LOG_LEVEL.GGGQ,5,"Before Activate 'GCCFANALYZEII.ProcessComId([%%v_in%%%],[%%v_out%%%],[%%p_errmess%%%])","I","SERVICE_INIT","PLACE=Lp_RunExec","")
//    If(!$Batch && $debug_mode$) Debug
//    Activate "GCCFANALYZEII".ProcessComId(v_in,v_out,p_errmess)
//    v_status = $Status
//    Call G_LOG(LOG_LEVEL.GGGQ,3,"After Activate 'GCCFANALYZEII.ProcessComId([%%v_in%%%],[%%v_out%%%],[%%p_errmess%%%])","I","SERVICE_INIT","PLACE=Lp_RunExec","")
//    If($$GGG_OUT!="")
//        If(p_errmess!="") p_errmess = "%%p_errmess%%^"
//        p_errmess = "%%p_errmess%%$$GGG_OUT"
//    EndIf
//    Return(v_status)
//END
//             * 
//GCCFANALYZEII.Lp_StandardXMLAnalyze
//    Activate $gggftrans_service$.Analyze_xml(p_data, p_message, p_params_out)

//             * 
//             GGGFLOGITUDE
//             http://polarion:86/polarion/#/project/OnGoingDev/workitem?id=AMI-45731
//             * 
//             */
//            return mySerilazeObject;
//        }


//        public static string GetFU()
//        {
//            var mytransmission = new transmission();
//            var mytransmission_details=new List<transmission_details>();
//            var mytransmission_detail1 = new transmission_details() { 
//                sender = new sender() { Value = "AMITAL" }, 
//                subject = new subject() { Value = "Status From Logitutde " } 
//            };

//            var myArrayOfdata = new List<data>();
//            var dataFUStatus=new data();
//            dataFUStatus.entity = UnifreightIIG.Common.MessageLib.Unifreight.FuStatus.Examples.FuStatusGenrate.GetTSTAmitalFile100New();
//            myArrayOfdata.Add(dataFUStatus);  
//            mytransmission.data = myArrayOfdata.ToArray();
//            mytransmission_details.Add(mytransmission_detail1);
//            mytransmission.transmission_details = mytransmission_details.ToArray();

//            var mySerilazeObject = UnifreightIIG.Common.Utils.XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);

//            Debug.WriteLine(mySerilazeObject);

//            //GGGHQANALYZE >>  GGGHQANALYZE
//            /*
//Entry Lp_RunExec
//Params
//    String p_com_id 	  :in
//    String p_errmess :out
//EndParams
//variables
//    string v_in,v_out
//    Numeric v_status
//endvariables
//    $$GGG_OUT = ""
//    PutItem/Id v_in,"COM_ID",p_com_id
//    PutItem/Id v_in,"ORIGIN_QUE",$origin_que$
//    PutItem/Id v_in,"FORCE_ANALYZE","1"
//    PutItem/Id v_in,"QUE_ID",QUE_ID.GGGQ
//    Call Lp_SyncLog("Activate GCCFANALYZEII, COM_ID=%%p_com_id",0)	
//    Call G_LOG(LOG_LEVEL.GGGQ,5,"Before Activate 'GCCFANALYZEII.ProcessComId([%%v_in%%%],[%%v_out%%%],[%%p_errmess%%%])","I","SERVICE_INIT","PLACE=Lp_RunExec","")
//    If(!$Batch && $debug_mode$) Debug
//    Activate "GCCFANALYZEII".ProcessComId(v_in,v_out,p_errmess)
//    v_status = $Status
//    Call G_LOG(LOG_LEVEL.GGGQ,3,"After Activate 'GCCFANALYZEII.ProcessComId([%%v_in%%%],[%%v_out%%%],[%%p_errmess%%%])","I","SERVICE_INIT","PLACE=Lp_RunExec","")
//    If($$GGG_OUT!="")
//        If(p_errmess!="") p_errmess = "%%p_errmess%%^"
//        p_errmess = "%%p_errmess%%$$GGG_OUT"
//    EndIf
//    Return(v_status)
//END
//             * 
//GCCFANALYZEII.Lp_StandardXMLAnalyze
//    Activate $gggftrans_service$.Analyze_xml(p_data, p_message, p_params_out)

//             * 
//             GGGFLOGITUDE
//             http://polarion:86/polarion/#/project/OnGoingDev/workitem?id=AMI-45731
//             * 
//             */
//            return mySerilazeObject;
//        }
//    }
}

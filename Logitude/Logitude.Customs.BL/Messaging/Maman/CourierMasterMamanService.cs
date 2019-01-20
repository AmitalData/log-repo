using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class CourierMasterMamanService
    {
        private CourierMasterPM _CourierMasterPM;
        private ICustomContext _Context;

        public string SendFTPMamanRequest(string courierMasterId, int tenant)
        {
            _Context = CustomContext.GetContext(tenant);
            var myCourierMasterQueryService = new CourierMasterQueryService(_Context);

            _CourierMasterPM = myCourierMasterQueryService.GetSingle(courierMasterId, false, false);
            if (_CourierMasterPM == null)
            {
                //throw new Exception("Declaration is null:" + _CustomFileCreditModel.AppicationId);
                return "CourierMasterPM is null " + courierMasterId;
            }

            var messageToMaman = BuildMessageToMaman();

            using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToMaman);
                //http://192.116.221.103:584/Courier58/api/couriermasters/getsingle?id=1-106

                var myFTPMamanService = new FTPOutMamanSubManifestService();
                myFTPMamanService.BuildCommunicationLog(bytearray, 1, courierMasterId);
                scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר נבנה בהצלחה וישלח בתהליך רקע";

        }

        private string BuildMessageToMaman()
        {
            CourierMasterMamanModel courierMasterMamanModel = new CourierMasterMamanModel();
            //string space = " ";
            courierMasterMamanModel.MAWB = _CourierMasterPM.MAWB != null ? _CourierMasterPM.MAWB : "";
            courierMasterMamanModel.AirlineId = _CourierMasterPM.AirlinePrefix != null ? _CourierMasterPM.AirlinePrefix : "";
            courierMasterMamanModel.HAWBShort = _CourierMasterPM.ShortHAWB != null ? _CourierMasterPM.ShortHAWB : "";
            courierMasterMamanModel.GatewayPortCode = _CourierMasterPM.GatewayPortCode != null ? _CourierMasterPM.GatewayPortCode.Substring(_CourierMasterPM.GatewayPortCode.Length - 3) : "";
            courierMasterMamanModel.Weight = "K";
            courierMasterMamanModel.PackageQuantity = _CourierMasterPM.PackageQuantity > 0 && _CourierMasterPM.PackageQuantity.ToString().Length <= 4 ? _CourierMasterPM.PackageQuantity.ToString() : "";
            courierMasterMamanModel.PackageQuantityExt = _CourierMasterPM.PackageQuantity > 0 && _CourierMasterPM.PackageQuantity.ToString().Length <= 6 ? _CourierMasterPM.PackageQuantity.ToString() : "";
            courierMasterMamanModel.GrossMassMeasure = "";
            if (_CourierMasterPM.GrossMassMeasure > 0 && ((int)_CourierMasterPM.GrossMassMeasure).ToString().Length <= 5)
            {
                decimal result = (decimal)_CourierMasterPM.GrossMassMeasure - Math.Truncate((decimal)_CourierMasterPM.GrossMassMeasure);
                var firstdigits = ((int)(Math.Round(result, 2) * 100)).ToString().Substring(0, 1);
                var number = ((int)_CourierMasterPM.GrossMassMeasure).ToString();
                courierMasterMamanModel.GrossMassMeasure = string.Concat(number, firstdigits);
            }
            courierMasterMamanModel.Description = "";

            var declarationQS = new DeclarationQueryService(_Context);
            string forwarder = declarationQS.GetDefault("ISRAEL", "CGO_CUST_FORW", "NON", "NON", _CourierMasterPM.Tenant);
            forwarder = forwarder.Substring(forwarder.Length -3);

            courierMasterMamanModel.Agent = forwarder;
            courierMasterMamanModel.SystemDate = String.Format("{0:yyMMdd}", DateTime.Now);
            courierMasterMamanModel.Forwarder = forwarder;
            courierMasterMamanModel.Internet = " ";
            courierMasterMamanModel.HAWB = _CourierMasterPM.HAWB != null ? _CourierMasterPM.HAWB : "";

            StringBuilder messageToMaman = new StringBuilder(444);
            messageToMaman.Append(courierMasterMamanModel.MAWB.PadLeft(8,'0'));
            messageToMaman.Append(courierMasterMamanModel.AirlineId.PadRight(3));
            messageToMaman.Append(courierMasterMamanModel.HAWBShort.PadLeft(8, '0'));
            messageToMaman.Append(' ', 7);
            messageToMaman.Append(courierMasterMamanModel.GatewayPortCode.PadRight(3));
            messageToMaman.Append(courierMasterMamanModel.Weight);
            messageToMaman.Append(courierMasterMamanModel.PackageQuantity.PadLeft(4, '0'));
            messageToMaman.Append(courierMasterMamanModel.GrossMassMeasure.PadLeft(6, '0')); // to check round????
            messageToMaman.Append(' ', 15); // Description
            messageToMaman.Append(courierMasterMamanModel.Agent.PadRight(3));
            messageToMaman.Append(courierMasterMamanModel.SystemDate);
            messageToMaman.Append(courierMasterMamanModel.Forwarder.PadRight(3));
            messageToMaman.Append(' ', 4);
            messageToMaman.Append(' ', 15);
            messageToMaman.Append(' ', 3);
            messageToMaman.Append(' ', 9);
            messageToMaman.Append(' ', 35);
            messageToMaman.Append(' ', 35);
            messageToMaman.Append(' ', 17);
            messageToMaman.Append(' ', 9);
            messageToMaman.Append(' ', 2);
            messageToMaman.Append(' ', 35);
            messageToMaman.Append(' ', 35);
            messageToMaman.Append(' ', 4);
            messageToMaman.Append(' ', 15);
            messageToMaman.Append(' ', 12);
            messageToMaman.Append(' ', 60);
            messageToMaman.Append(' ', 25);
            messageToMaman.Append(courierMasterMamanModel.Internet);
            messageToMaman.Append(' ', 17);
            messageToMaman.Append(' ', 9);
            messageToMaman.Append(courierMasterMamanModel.HAWB.PadRight(35));
            messageToMaman.Append(courierMasterMamanModel.PackageQuantityExt.PadLeft(6, '0'));

            return messageToMaman.ToString();

        }
    }

    public class CourierMasterMamanModel
    {
        public string MAWB { get; set; }
        public string AirlineId { get; set; }
        public string HAWBShort { get; set; }
        //public string EndDate { get; set; } // palet
        public string GatewayPortCode { get; set; }
        public string Weight { get; set; }
        public string PackageQuantity { get; set; }
        public string GrossMassMeasure { get; set; }
        public string Description { get; set; }
        public string Agent { get; set; }
        public string SystemDate { get; set; }
        public string Forwarder { get; set; }
        public string Internet { get; set; }
        public string HAWB { get; set; }
        public string PackageQuantityExt { get; set; }
    }
}

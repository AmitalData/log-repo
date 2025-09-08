using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityQueryServicesExt
{
	public interface ICustomsAutoDecClosing
	{
		void Send8235(DeclarationPM decPm,string LoggingUserId = "", bool isAutoSendByErrorDiamondDec = false);
	}
	public interface ICustomCreateTicket
	{
		bool CreateTicket(string documentFilingId,string documentFilingCode, string documentTypeCode,int tenant,string declaratinId);
	}
}

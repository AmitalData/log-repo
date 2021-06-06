using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Contracts
{
    public interface IUploadResultM
    {


        OutlookConnection.Common.ActivityWcfServiceReference.DocumentDataPM[] myDocumentDataPMArray { get; set; }

    }
  
}

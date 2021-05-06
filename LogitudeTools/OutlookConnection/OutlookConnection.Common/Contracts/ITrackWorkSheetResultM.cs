using OutlookConnection.Common.Models;

namespace OutlookConnection.Common.Contracts
{


    public interface ITrackWorkSheetResultM
    {
        //bool Canceled { get; set; }

        bool DeleteMailAfterUplaod { get; set; }

        string SelectedEntityTypeName { get; set; }

        string EmailSubjectName { get; set; }

        dynamic SelectedEntityType { get; set; }

        OfficeType OfficeItemType { get; set; }

        bool SaveAsPDF { get; set; }

        bool TrackWithoutConnect { get; set; }

        OutlookConnection.Common.ActivityWcfServiceReference.DocumentDataPM[] myDocumentDataPMArray { get; set; }

        //OutlookConnection.Common.CustomerWcfServiceReference.CustomerList SelectedCustomer { get; set; }
        //OutlookConnection.Common.OpportunityWcfServiceReference.OpportunityList SelectedOpportunity { get; set; }

        // List<AttachM> SelectedAttachmentList { get; set; }

    }
}

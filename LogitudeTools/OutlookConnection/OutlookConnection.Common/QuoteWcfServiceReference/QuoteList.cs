
namespace OutlookConnection.Common.QuoteWcfServiceReference
{
    public partial class QuoteList
    {
        public string DirectionImage
        {
            get
            {
                //
                switch (this.DirectionName)
                {
                    case "Import":
                        return "/OutlookConnection.wpf;component/Images/Import.png";

                    case "Export":
                        return "/OutlookConnection.wpf;component/Images/Export.png";

                    case "Domestic":
                        return "/OutlookConnection.wpf;component/Images/Domestic.png";
                    default:
                        break;
                }
                return "/OutlookConnection.wpf;component/Images/Customer.png";
            }
        }


        public string TransportModeImage
        {
            get
            {
                //
                switch (this.TransportModeName)
                {
                    case "Air":
                        return "/OutlookConnection.wpf;component/Images/Air.png";

                    case "Ocean":
                        return "/OutlookConnection.wpf;component/Images/Ocean.png";

                    case "Domestic":
                        return "/OutlookConnection.wpf;component/Images/Domestic.png";

                    case "Inland":
                        return "/OutlookConnection.wpf;component/Images/Inland.png";

                    default:
                        break;
                }
                return "/OutlookConnection.wpf;component/Images/Customer.png";
            }
        }



    }
}

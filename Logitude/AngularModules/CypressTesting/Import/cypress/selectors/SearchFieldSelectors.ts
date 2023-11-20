export class SearchFieldSelectors {

    public static readonly GeneralMHDeclarationsTab = "#GeneralMHDeclarations"
    public static readonly SearchField = "searchtextbox input[id*=SearchFieldsId]"
    public static readonly ListDataLoaded = "#ListDataLoaded";
    public static readonly ShortTitleControl = '.ShortTitleControl span:first-child';
    public static readonly FindFileInGrid = "logitude-grid table tr:eq(1) td>div:contains('{0}')";

    public static readonly FindCustInGrid = "logitude-grid table tr:eq(2) td>div:contains('{0}')";

    public static readonly FindDecInGrid = "logitude-grid table tr:eq(4) td>div:contains('{0}')";

    //public static readonly FindSecondCargoIDInGrid = "logitude-grid table tr:eq(9) td>div:contains('{0}')";
    public static readonly FindSecondCargoIDInGrid = "#Customs\\.Consignment_SecondCargoID";

    public static readonly Declaration1 ="#LogGrid_0_0LogGridRows_0_0";

}
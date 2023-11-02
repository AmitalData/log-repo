export class CargoSerialDataSelectors {
   
    public static readonly ExportDeclaration = "#CustomsMHExportDeclaration"  
    public static readonly SearchField = "searchtextbox input[id*=SearchFieldsId]"  
    public static readonly FirestDeclaration ="logitude-grid table tr:eq(1)";
    //public static readonly AddButton = '[src="./Images/Buttons/Add.png"] :eq(1).FlipImgHoriz';
    public static readonly AddButton = ".LogitudeIconButton :eq(5)" 
    //public static readonly SaveButton = "td > div .Button :eq(1)";
    public static readonly SaveButton = "#CustomsDeclaration-Save";
    //public static readonly focus = ".InputDiv :eq(0)";
    public static readonly Quantity = ".LogCellTemplate:eq(2)";
    public static readonly SignsAndNumbersClick = '#edit-log-grid_0_00_5_0 > .LogCellTemplateInnerChild > div'
    public static readonly SignsAndNumbers = "#Customs\\.ConsignmentPackage_MarksNumbers";
    //public static readonly SignsAndNumbers = "#textboxdiv_Customs\\.ConsignmentPackage_MarksNumbers";
    public static readonly CargoSerialDataFirstRow  ="#row0";

    public static readonly CargoSerialDataDeletRow  = '.removePackageButton'
    //public static readonly CargoSerialDataDeletRow  ='[src="./Images/Buttons/Delete.png"]:eq(2)';
    public static readonly Yes = 'ConfirmWindow table tr td Button.RedButton'


    //public static readonly PostAPPayments = 'PostAPPayments';


    
}
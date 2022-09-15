import { IScreenLayoutService} from '../Interface/IScreenLayoutService';

export class ClassicScreenLayoutService implements IScreenLayoutService {



    private screenComponent: any;
    constructor(screenComponent: any) {
        this.screenComponent = screenComponent;
    }


    public GenerateScreen(screen: any) {
        this.screenComponent.ScreenRows = [];
        let numberOfScreenColumns = this.GetNumberOfScreenColumns(screen);
        
        for (var i = 0; i < numberOfScreenColumns; i++) {
            var screenRowDetails = this.screenComponent.BuildScreenRowDetails(i);
            this.screenComponent.ScreenRows.push(screenRowDetails);
        }
    }

    private GetNumberOfScreenColumns(screen: any) {
        let additionalFieldsShipmentPackageScreenCode = 'ShipmentPackage.AdditionalFields';
        return screen.ScreenCode == additionalFieldsShipmentPackageScreenCode ? 1 : screen.NumberOfColumns;
    }

    public GetScreenRows(sectionNumber: number) {

        return this.screenComponent.ScreenRows;

    }


    public ChangeScreenFieldPosition(screenFieldPositionargs: any) {
        this.screenComponent.ScreenRows.forEach(screenRow => {
            this.screenComponent.ChangeScreenFieldPosition(screenRow, screenFieldPositionargs);
      
        });
    }


    public BuildScreenUpdateArgs() {

        this.screenComponent.MyArgs.ScreenFields = [];
        let columns = 0;
        this.screenComponent.MyArgs.Rows = 0;
        this.screenComponent.ScreenRows.forEach(screenRowDetails => {
            columns++;
            this.screenComponent.AddScreenField(screenRowDetails, null);

        });
        this.screenComponent.MyArgs.Columns = columns;
    }




}

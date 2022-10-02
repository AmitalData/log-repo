import { AppTool } from '../../../Infrastructure/Tools';
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
        let additionalFieldsForOneColumnScreensCodes = this.GetScreensCodesWithOneColumnForAdditionalFields();
        let screenCode = screen.ScreenCode ? screen.ScreenCode : screen.Code;
        return additionalFieldsForOneColumnScreensCodes.indexOf(screenCode) > -1 ? 1 : screen.NumberOfColumns;
    }

    private GetScreensCodesWithOneColumnForAdditionalFields() {
        return [
            "ShipmentPackage.AdditionalFields",
            "ShipmentPayable.AdditionalFields",
            "ShipmentReceivable.AdditionalFields"
        ];
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

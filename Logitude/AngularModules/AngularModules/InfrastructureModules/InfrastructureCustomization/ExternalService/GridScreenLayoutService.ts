import { IScreenLayoutService } from '../Interface/IScreenLayoutService';
import { ScreenFieldPM } from '../../../Infrastructure/EntityPMs/ScreenFieldPM';
import { ObjectFieldPM } from '../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

export class GridScreenLayoutService implements IScreenLayoutService {

    private screenComponent: any;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(screenComponent: any) {
        this.screenComponent = screenComponent;
    }


    public GenerateScreen(screen: any) {
        this.screenComponent.ScreenRows = [];

        for (var i = 0; i < screen.NumberOfColumns; i++) {
            var screenRowDetails = this.screenComponent.BuildScreenRowDetails(i);
            this.screenComponent.ScreenRows.push(screenRowDetails);
        }
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
        if (this.screenComponent.ReloadGridSections) {
            this.ReladGridSections();
        }
        this.screenComponent.MyArgs.ScreenFields = [];
        this.screenComponent.MyArgs.RemovedScreenFields = [];
        this.screenComponent.GridScreenSelectedFields.forEach(objectField => {
            this.AddScreenFieldToMyArgs(objectField);
        });
        this.screenComponent.currentScreenFields.forEach(screenField => {
            this.AddRemovedScreenFieldToMyArgs(screenField);
        });
        this.screenComponent.MyArgs.Rows = 0;
        this.screenComponent.MyArgs.Columns = this.screenComponent.GridScreenSelectedFields.length;
        this.screenComponent.MyArgs.SortedByFieldCode = this.screenComponent.SelectedItem.ScreenPM.SortedByFieldCode;
        this.screenComponent.MyArgs.SortedType = this.screenComponent.SelectedItem.ScreenPM.SortedType;
        this.screenComponent.MyArgs.RelatedScreenCode = this.screenComponent.SelectedItem.ScreenPM.RelatedScreenCode;
    }

    private ReladGridSections() {
        this.screenComponent.ReloadGridSections = false;
        this.CurrentSession.SessionEvent.emit({ Name: "ReloadGridSections" });
    }

    private AddScreenFieldToMyArgs(objectField: ObjectFieldPM) {
        let screenField = new ScreenFieldPM();
        screenField.Column = objectField.IndexOrder;
        screenField.ObjectFieldId = objectField.Id;
        screenField.ScreenId = this.screenComponent.SelectedItem.ScreenPM.Id;
        screenField.ScreenCode = this.screenComponent.SelectedItem.ScreenPM.Code;
        screenField.Tenant = SessionLocator.Tenant;
        screenField.Row = 0;
        screenField.DataTypeCode = objectField.DataTypeCode;
        screenField.ObjectFieldCode = objectField.FieldCode;

        this.screenComponent.MyArgs.ScreenFields.push(screenField);
    }

    private AddRemovedScreenFieldToMyArgs(screenField: ScreenFieldPM) {

        let objectField = this.screenComponent.GridScreenSelectedFields.filter(field => field.FieldCode == screenField.ObjectFieldCode)[0];
        if (objectField) return;
        this.screenComponent.MyArgs.RemovedScreenFields.push(screenField);
    }
}

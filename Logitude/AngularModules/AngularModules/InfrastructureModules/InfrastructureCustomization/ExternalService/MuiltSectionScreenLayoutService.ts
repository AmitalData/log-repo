import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ScreenSectionExtendedService } from '../../../Infrastructure/Services/ExtendedPMs/ScreenSectionExtendedService';
import { ScreenRowDetails, SectionScreenItem } from '../Components/Customization/ScreenLayoutComponent';
import { IScreenLayoutService } from '../Interface/IScreenLayoutService';

export class MuiltSectionScreenLayoutService implements IScreenLayoutService {

    private screenComponent: any;
    constructor(screenComponent: any) {
        this.screenComponent = screenComponent;
    }

    public GenerateScreen(screen: any) {

        this.screenComponent.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
        this.screenComponent.SectionScreens = [];
        let screenSectionExtendedService: ScreenSectionExtendedService = new ScreenSectionExtendedService();
        screenSectionExtendedService.GetByScreenCode(this.screenComponent.SelectedItem.ScreenPM.Code).subscribe((result: any) => {
            this.screenComponent.CurrentSession.CurrentWindow.StopBusyIndicator();

            var myResponse: ServiceResponse = result;
            if (myResponse.HasError) {
                this.HandleException(myResponse);
                return;
            }
            myResponse.Result.forEach(section => {
                var sectionScreenItem = this.BuildSectionScreen(section, screen);
                this.screenComponent.SectionScreens.push(sectionScreenItem);
            });
        });
    }
    

    public GetScreenRows(sectionNumber:number =null) {
        return this.screenComponent.SectionScreens.filter(d => d.Section.Number == sectionNumber)[0].ScreenRows;

    }


    public ChangeScreenFieldPosition(screenFieldPositionargs: any) {
        this.screenComponent.SectionScreens.forEach(sectionScreen => {
            sectionScreen.ScreenRows.forEach(screenRow => {
                this.screenComponent.ChangeScreenFieldPosition(screenRow, screenFieldPositionargs);
            });
        });
    }


    public BuildScreenUpdateArgs() {

        this.screenComponent.MyArgs.ScreenFields = [];

        this.screenComponent.SectionScreens.forEach(sectionScreen => {
            sectionScreen.Section.NumberOfRows = 0;
            this.AddScreenSectionFields(sectionScreen);
        });

        this.screenComponent.MyArgs.Columns = this.screenComponent.OldItem.ScreenPM.NumberOfColumns;
        this.screenComponent.MyArgs.Rows = this.screenComponent.OldItem.ScreenPM.NumberOfRows;
        this.screenComponent.MyArgs.ScreenSections = this.GetModifySections();



    }

    private BuildSectionScreen(section: any, screenItem: any) {

        this.screenComponent.ScreenRows = [];
        var sectionScreen: SectionScreenItem = new SectionScreenItem(section);
        for (var i = 0; i < screenItem.NumberOfColumns; i++) {
            var screenRowDetails = this.screenComponent.BuildScreenRowDetails(i, section.Number);
            this.screenComponent.ScreenRows.push(screenRowDetails);
        }
        sectionScreen.ScreenRows = this.screenComponent.ScreenRows;
        return sectionScreen;
    }

    private AddScreenSectionFields(sectionScreen: SectionScreenItem) {
        sectionScreen.ScreenRows.forEach(screenRowDetails => {
            this.AddScreenFields(screenRowDetails, sectionScreen);
        });
    }


    private AddScreenFields(screenRowDetails: ScreenRowDetails, sectionScreen: SectionScreenItem) {
        let screenFieldRow = 0;
        screenRowDetails.ScreenFieldPMs.forEach(screenField => {
            this.AddScreenField(screenField, sectionScreen, screenFieldRow++  );
        });
    }

    private AddScreenField(screenField, sectionScreen, screenFieldRow ) {
        screenField.Row = screenFieldRow;
        if (!this.screenComponent.IsLineSpaceField(screenField.DataTypeCode)) {
            this.screenComponent.MyArgs.ScreenFields.push(screenField);
        }
        this.screenComponent.MyArgs.Rows += 1;
        sectionScreen.Section.NumberOfRows += 1;
    }


  private  HandleException(serviceResponse: ServiceResponse) {
        if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) {
            this.ShowMessageWindow(serviceResponse.ErrorsArray[0], "Logitude Message");
        }
    }


    private ShowMessageWindow(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
        if (!title) return;
        messageWindow.Title = title;


    }


    GetModifySections() {
        var sections: any[] = [];
        this.screenComponent.SectionScreens.forEach(sectionScreen => {

            if (!sectionScreen.Section.ChangeSetOp && sectionScreen.Section.IsDirty) {
                sectionScreen.Section.ChangeSetOp = "Update";
            }
            if (sectionScreen.Section.ChangeSetOp) sections.push(sectionScreen.Section);

        });
        return sections;


    }

}

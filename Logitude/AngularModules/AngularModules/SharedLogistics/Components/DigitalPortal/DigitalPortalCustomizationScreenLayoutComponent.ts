import { Component } from '@angular/core';
import { DigitalPortalScreenList } from "../../../infrastructure/entitylists/digitalportalscreenlist"
import { DigitalCustomizationService, DigitalPortalScreenUpdateModel } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

@Component({
    templateUrl: './DigitalPortalCustomizationScreenLayoutComponent.html',
})

export class DigitalPortalCustomizationScreenLayoutComponent {

    public Screens: Array<DigitalPortalScreenList> = [];
    digitalCustomizationService: DigitalCustomizationService;
    public IsModified: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    ModifiedScreenData: DigitalPortalScreenUpdateModel;

    constructor() {
        this.Screens = [];
        this.ModifiedScreenData = new DigitalPortalScreenUpdateModel();
        this.digitalCustomizationService = new DigitalCustomizationService();
        this.GetDefaultScreens();
    }

    SetWindowArgs(args: any) {

    }

    GetDefaultScreens() {
        this.Screens = [];
        this.digitalCustomizationService.GetDigitalPortalScreenNames().subscribe((myResult) => {
            if (!myResult.HasError) {
                this.Screens = myResult.Result;
                if (this.Screens != null) {
                    this.SelectedItem = this.Screens[0];
                    this.GetHTMLText();
                }
            }
        });
    }

    public SelectedItem: DigitalPortalScreenList;
    private selectedItem: DigitalPortalScreenList;
    SelectionChanged(Item) {
        this.selectedItem = Item;
        this.ChangeSelectedItem();
    }
    ChangeSelectedItem() {
        if (this.selectedItem == this.SelectedItem) return;
        this.SelectedItem = this.selectedItem;
        this.GetHTMLText();
    }

    GetHTMLText() {
        var objectTableId = this.SelectedItem.ObjectTableId;
        var screenCode = this.SelectedItem.ScreenCode;
        this.digitalCustomizationService.GetDigitalPortalScreens(objectTableId, screenCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                var screen = myResult.Result;
                if (this.Screens != null)
                    this.hTMLEditor = screen.Content;
            }
        });
    }

    private hTMLEditor: string = null;
    get HTMLEditor() {
        return this.hTMLEditor;
    }
    set HTMLEditor(newValue: string) {
        if (newValue != this.hTMLEditor) {
            this.hTMLEditor = newValue;
            this.IsModified = true;
        }
    }

    AddPredefinedComponentClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        var windowArgs: any = {};
        windowArgs.ObjectTableId = this.SelectedItem.ObjectTableId;
        logWindow.Title = "Insert Predefined Component";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/AddDigitalPredefinedComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {

            }
        });
    }

    AddFieldCodesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        var windowArgs: any = {};
        windowArgs.ObjectTableId = this.SelectedItem.ObjectTableId;
        logWindow.Title = "Insert Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/AddDigitalFieldCodeComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {

            }
        });
    }

    RestoreDefaultLayoutClicked() {

    }

    PublichChangesClicked(isDraft) {
        if (this.IsModified) {
            var confirm = new ConfirmWindow();
            confirm.Width = 400;
            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show("The changes will be reflected to the portal and customers will see the changes");
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.ContinueSaveChanges(isDraft);
                }
            });
        }
    }

    ContinueSaveChanges(isDraft) {
        this.CurrentSession.StartBusyIndicatorLoading();
        var objectTableId = this.SelectedItem.ObjectTableId;
        var screenCode = this.SelectedItem.ScreenCode;
        var name = this.SelectedItem.Name;
        this.ModifiedScreenData.ObjectTableId = objectTableId;
        this.ModifiedScreenData.ScreenCode = screenCode;
        this.ModifiedScreenData.Name = name;
        this.ModifiedScreenData.Content = this.HTMLEditor;
        this.ModifiedScreenData.IsDraft = isDraft;
        if (isDraft == true) {
            this.ModifiedScreenData.DraftContent = this.HTMLEditor;
        }
        this.digitalCustomizationService.UpdateDigitalPortalScreen(this.ModifiedScreenData).subscribe((myResult) => {
            this.IsModified = false;
            this.ModifiedScreenData = new DigitalPortalScreenUpdateModel();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    PreviewChangesClicked() {

    }
}

import { Component } from '@angular/core';
import { DigitalPortalScreenList } from "../../../infrastructure/entitylists/digitalportalscreenlist"
import { DigitalCustomizationService, DigitalPortalScreenUpdateModel } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Guid } from '../../../Infrastructure/Utilities/Guid';

@Component({
    templateUrl: './DigitalPortalCustomizationScreenLayoutComponent.html',
})

export class DigitalPortalCustomizationScreenLayoutComponent {

    public Screens: Array<DigitalPortalScreenList> = [];
    digitalCustomizationService: DigitalCustomizationService;
    public IsModified: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    ModifiedScreenData: DigitalPortalScreenUpdateModel;
    TextAreaInputId: string = Guid.newGuid();
    TextAreaInputCurrentPosition: number = 0;
    IsPreviewChanges: boolean = false;
    public ProfileCode: string;
    public ProfileId: string;

    public editorOptions = { theme: '', language: 'html', validate: 'true' };
    CurrentTenantScreen: DigitalPortalScreenList;
    constructor() {
        this.Screens = [];
        this.ModifiedScreenData = new DigitalPortalScreenUpdateModel();
        this.digitalCustomizationService = new DigitalCustomizationService();
    }

    public GetDefaultScreens() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.Screens = [];
        this.digitalCustomizationService.GetDigitalPortalScreenNames(this.ProfileCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                this.Screens = myResult.Result;
                if (this.Screens != null) {
                    this.SelectedItem = this.Screens[0];
                    this.GetHTMLText();
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    public SelectedItem: DigitalPortalScreenList;
    private selectedItem: DigitalPortalScreenList;
    SelectionChanged(Item) {
        this.selectedItem = Item;
        if (this.IsModified) {
            this.OpenConfirmWindow();
            return;
        }
        this.ChangeSelectedItem();
    }

    OpenConfirmWindow() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.NoButtonText = "Don't Save";
        confirmWindow.YesButtonText = "Save ";
        confirmWindow.CancelButtonText = "Cancel";
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        confirmWindow.Show("This Screen has unsaved changes. Do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            this.IsModified = false;
            if (confirmWindow.Yes) {
                this.ContinueSaveChanges(false);
                return;
            }
            if (confirmWindow.No) {
                this.ChangeSelectedItem();
                return;
            }
        });
    }

    ChangeSelectedItem() {
        if (this.selectedItem == this.SelectedItem) return;
        this.SelectedItem = this.selectedItem;
        this.GetHTMLText();
    }

    GetHTMLText(isDraft: boolean = false) {
        this.CurrentSession.StartBusyIndicatorLoading();
        var objectTableId = this.SelectedItem.ObjectTableId;
        var screenCode = this.SelectedItem.ScreenCode;

        this.digitalCustomizationService.GetDigitalPortalScreens(objectTableId, screenCode, this.ProfileCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                var screen = myResult.Result;
                this.CurrentTenantScreen = screen;
                if (screen != null)
                    isDraft == true ? this.hTMLEditor = this.CurrentTenantScreen.DraftContent: this.hTMLEditor = screen.Content;
            }
            this.CurrentSession.StopBusyIndicator();
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
                this.attachValue($event);
            }
        });
    }

    AddFieldCodesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        var windowArgs: any = {};
        windowArgs.ObjectTableId = this.SelectedItem.ObjectTableId;
        windowArgs.ProfileCode = this.ProfileCode;
        windowArgs.ScreenCode = this.SelectedItem.ScreenCode;
        windowArgs.IsList = this.SelectedItem.IsList;
        logWindow.Title = "Insert Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/AddDigitalFieldCodeComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event) {
                    var isAddingComponent = s.IsAddingComponent;
                    var htmlField = "";
                    if (isAddingComponent) {
                        htmlField = "<LogFieldContainer >\n<LogLabel field-code='" + $event + "' ></LogLabel>\n\n<LogField field-code='" + $event + "'></LogField> \n</LogFieldContainer >";
                    }
                    else {
                        htmlField = $event;
                    }

                    this.attachValue(htmlField);
                }
            });
        });
    }

    RestoreDefaultLayoutClicked() {
        var confirm = new ConfirmWindow();
        confirm.Width = 400;
        confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        confirm.ShowNoButton = true;
        confirm.Show("All changes will be lost, continue ? ");
        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                confirm.Close();
                var objectTableId = this.SelectedItem.ObjectTableId;
                var screenCode = this.SelectedItem.ScreenCode;

                this.digitalCustomizationService.GetDefaultScreenLayout(objectTableId, screenCode, this.ProfileCode).subscribe((myResult) => {
                    if (!myResult.HasError) {
                        var screen = myResult.Result;
                        if (this.Screens != null) {
                            this.hTMLEditor = screen.DraftContent;
                            this.IsModified = true;
                        }
                    }
                });
            }
        });
    }

    LoadDraftLayoutClicked() {
        this.GetHTMLText(true);
    }
    

    PublishChangesClicked(isDraft) {
        if (this.IsModified) {
            var confirm = new ConfirmWindow();
            confirm.Width = 400;
            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show("By publishing the changes on the screen " + this.SelectedItem.Name + "  , the changes will be reflected and viewed by the customers, continue ? ");
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
        var isList = this.SelectedItem.IsList;
        var name = this.SelectedItem.Name;
        this.ModifiedScreenData.ObjectTableId = objectTableId;
        var profileId = this.ProfileId;
        var profileCode = this.ProfileCode;
        this.ModifiedScreenData.ProfileId = profileId;
        this.ModifiedScreenData.ProfileCode = profileCode;
        this.ModifiedScreenData.ScreenCode = screenCode;
        this.ModifiedScreenData.IsList = isList;
        this.ModifiedScreenData.Name = name;
        this.ModifiedScreenData.IsDraft = isDraft;
        if (isDraft == true) {
            this.ModifiedScreenData.DraftContent = this.HTMLEditor;
        }
        else {
            this.IsModified = false;
            this.ModifiedScreenData.Content = this.HTMLEditor;
            this.ModifiedScreenData.DraftContent = this.CurrentTenantScreen.DraftContent;
        }

        this.digitalCustomizationService.UpdateDigitalPortalScreen(this.ModifiedScreenData).subscribe((myResult) => {
            this.ModifiedScreenData = new DigitalPortalScreenUpdateModel();
            this.CurrentSession.StopBusyIndicator();
            if (this.IsPreviewChanges) {
                this.IsPreviewChanges = false;
                this.PreviewDigitalPortal();
            }
        });
    }

    PreviewChangesClicked() {
        this.IsPreviewChanges = true;
        this.ContinueSaveChanges(true);
    }

    PreviewDigitalPortal() {
        window.open(`https://${SessionLocator.TenantManagementJS.CustomerURL}/preview-draft`, "_blank");
    }

    blur(event) {
        const start = event.target.selectionStart;
        this.TextAreaInputCurrentPosition = start;
    }

    myRange : any;
    myEditor : any;
    onInit(editor) {
        editor.onDidBlurEditorText(() => {
            this.myEditor = editor;
            this.myRange = editor.getSelection(); 
        });
    }

    attachValue(selectedValue: string) {
            var id = { major: 1, minor: 1 };
            var text = selectedValue;
            var op = { identifier: id, range: this.myRange, text: text, forceMoveMarkers: true };
            this.myEditor.executeEdits("my-source", [op]);
    }
}

import {Component, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ExcelExportService} from '../../../Common/Services/Others/ExcelExportService'
import {ImageParameter} from '../../../Infrastructure/DataContracts/ImageParameter';
declare var UploadLogoFile, ArrayBufferToBase64;

@Component({
    selector: 'SettingsWorkspaceComponent',
    moduleId: module.id,
    templateUrl: './SettingsWorkspaceComponent.html',
    providers: [EntityResourceService],
})

export class SettingsWorkspaceComponent {

    constructor(private _entityResourceService: EntityResourceService) {

    }

    ClockClicked() {
        document.getElementById(this.ClockTimeHtmlId).click();
    }
    public ClockTimeHtmlId: string = Guid.NewRandomString();
    UploadClockTimeFile(event: any) {

        var file: any = UploadLogoFile(this.ClockTimeHtmlId);
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            this.ArrayBufferToBase64(file, this);
        }
    }
    ArrayBufferToBase64(file: any, viewModel: any) {
        if (file) {
            var reader: FileReader = new FileReader();
            var reader = new FileReader();

            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewModel.ImportFeatures(window.btoa(binary));
            };

            reader.onerror = function (e) {
                SessionLocator.CurrentSession.StopBusyIndicator();

                var wind = new MessageWindow();
                wind.Show("Error Importing file");
            };

            reader.readAsArrayBuffer(file);
        }
    }
    ImportFeatures(data: any) {
        var service: ExcelExportService = new ExcelExportService();
        var file: ImageParameter = new ImageParameter();
        file.Base64String = data;
        service.ImportClockTimeData(file).subscribe(res => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            var wind = new MessageWindow();
            wind.Show("Import completed successfully");
        });
    }

    CategoriesClicked() {
        var displayTitle = "All Categories";
        var code = "All Categories";
        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMProjectCategory";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    SessionLocator.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }

    ProjectsClicked() {
        var displayTitle = "All Projects";
        var code = "Active Projects";
        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMProject";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    SessionLocator.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }

    SprintsClicked() {
        var displayTitle = "All Sprints";
        var code = "All Sprints";
        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "Sprint";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    SessionLocator.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }
}

declare var System: any, window: any;
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {Http, Response} from '@angular/http';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {HybridPartnerExtendedListService} from '../../../../Common/Services/ExtendedLists/HybridPartnerExtendedListService';
import {HybridPartnerList} from '../../../../Common/EntityLists/HybridPartnerList';
import {WebFreightDomainService} from '../../../../Infrastructure/Services/WebFreightDomainService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CustomerPMService} from '../../../../Common/Services/StandardPMs/CustomerPMService';
import {CustomerTenantAccessRequestPM} from '../../../../Common/EntityPMs/CustomerTenantAccessRequestPM';
import {CustomerTenantAccessRequestExtendedPMService} from '../../../../Common/Services/ExtendedPMs/CustomerTenantAccessRequestExtendedPMService'
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'DropBoxTestFile',
    moduleId: module.id,
    templateUrl: './DropBoxTestFileComponent.html',
})

export class DropBoxTestFileComponent extends BaseComponent implements OnInit, AfterViewInit {

    private messageWindow: MessageWindow = new MessageWindow();
    IsConnected: boolean = false;
    ShowTestButton: boolean = false;
    DataContext: DropBoxTestFileComponent = this;
    ValidationErrorsList: any[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public CD: ChangeDetectorRef) {
        super();
    }
    ngOnInit() {
        this.ValidationErrorsList = [];
    }
    ngAfterViewInit() {

    }


    CloseBtnClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendTestFile() {
        if (AppTool.IsNullOrEmpty(this.FolderName)) {
            this.ValidationErrorsList.push("Folder Name is required ");
        }
        if (AppTool.IsNullOrEmpty(this.FileName)) {
            this.ValidationErrorsList.push("File Name is required ");
        }
        if (AppTool.IsNullOrEmpty(this.ObjectTableId)) {
            this.ValidationErrorsList.push("ObjectTable is required ");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            var myService: CommonDomainService = new CommonDomainService();
            myService.GetDropBoxComLogTestFile(SessionLocator.Tenant, this.FileName, this.FolderName, this.FileText, this.ObjectTableId).subscribe((myResult) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var temp = myResult.Result;
                this.messageWindow.Width = 300;
                this.messageWindow.Height = 200;
                this.messageWindow.Title = "DropBox Communicaiton Log";
                this.messageWindow.Message = "Communicaiton Log Created For DropBox Test File Successfully";
                this.messageWindow.Show(this.messageWindow.Message);

            });
        }
       
    }


    private folderName: string;
    public get FolderName() { return this.folderName }
    public set FolderName(newValue: string) { this.folderName = newValue; }

    private fileName: string;
    public get FileName() { return this.fileName }
    public set FileName(newValue: string) { this.fileName = newValue; }

    private fileText: string;
    public get FileText() { return this.fileText }
    public set FileText(newValue: string) { this.fileText = newValue; }

    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; }


}


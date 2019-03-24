import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BIReportList } from '../../../Infrastructure/EntityLists/BIReportList';
import { BIReportListService } from '../../../Infrastructure/Services/StandardLists/BIReportListService';
import { InfrastructureDomainService } from '../../../Infrastructure/Services/InfrastructureDomainService';
import { BIReportFolderList } from '../../../Infrastructure/EntityLists/BIReportFolderList';
import { BIReportFolderListService } from '../../../Infrastructure/Services/StandardLists/BIReportFolderListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow'; 
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIFolderReportComponent.html',
})

export class BIFolderReportComponent {
    public ItemsSource: BIFolderClass[] = [];
    private folderListService: BIReportFolderListService;
    private reportListService: BIReportListService;
    public _InfrastructureDomainService: InfrastructureDomainService;

    constructor() {
        this.folderListService = new BIReportFolderListService();
        this.reportListService = new BIReportListService();
        this._InfrastructureDomainService = new InfrastructureDomainService();
        this.LoadData();
        this.Listen();
    }

    private Listen() {
        SessionLocator.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "BIRefresh") {
                this.LoadData();
            }
        });
    }

    public InitComponent() {

    }

    private folderList: BIReportFolderList[];
    private reportList: BIReportList[];
    LoadData() {        
        this.folderListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.folderList = myResponse.Result;
                
                this.reportListService.getAll().subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.reportList = myResponse.Result;
                        this.FillItemsSource();
                    }
                });                
            }
        });
    }

    FillItemsSource() {
        this.ItemsSource = [];

        this.folderList.forEach((item) => {
            var myReports: BIReportList[] = this.reportList.filter(d => d.BIReportFolderId == item.Id);

            if (AppTool.IsNullOrEmpty(this.mySearchText)) {
                this.ItemsSource.push(new BIFolderClass(item, myReports));
            }

            else {
                if (!AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.mySearchText.toUpperCase()) > -1
                    ||
                    !AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.mySearchText.toUpperCase()) > -1) {
                    this.ItemsSource.push(new BIFolderClass(item, myReports));
                }
            }
        }); 
    }

    NewFolderButtonClicked() {
        var logWindow = new LogitudeWindow();        
        logWindow.Title = "New Folder";
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReportFolderComponent');
        logWindow.WindowClosed.subscribe(d => {
            if (d) {
                this.LoadData();
            }
        });
    }

    NewReportButtonClicked(){
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New BI Report";
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');
        logWindow.WindowClosed.subscribe(d => {
            if (d) {
                //this.LoadData();
            }
        });
    }

    DeleteFolderClicked(item: BIFolderClass) {
        if (item != null) {
            if (item.reportsList != null && item.reportsList.length > 0) {
                var msg = new MessageWindow();
                msg.Width = 450;
                msg.Show("Can't delete this folder since it contains reports, please delete/move them first");
            }
            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.NoButtonText = "No";
                confirmWindow.YesButtonText = "Yes";
                confirmWindow.Title = "Confirm Deletion";
                confirmWindow.Show("Are you sure you want to delete this folder?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        // save
                        this._InfrastructureDomainService.DeleteFolder(item.FolderId).subscribe(myResult => {
                            if (!myResult.HasError) {
                                this.LoadData();
                            }
                        });
                    }
                    else if (confirmWindow.No) {
                        //nth
                    }
                });

            }
        }
    }
        
    ViewFolderClicked(folder: BIFolderClass) {
        var objectTableName = "BIReport";
        var queryCode = "ALLBIREPORTS";
        var filterName = "BIReportFolderId";
        var filterAgrs = new ApiQueryFilters();       

        filterAgrs.addAdditionalFilter(filterName, folder.FolderId, null, null, "Equals", false, false, false, "String");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = folder.Name; //listArgs.QueryCode;
        listArgs.BackButtonTitle = "Back";
        listArgs.BIReportFolderId = folder.FolderId;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadData());
            });
    }
    
    public mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
        this.FillItemsSource();
    }
}

export class BIFolderClass {
    private folder: BIReportFolderList;
    public reportsList: BIReportList[];
    public Title: string;
    public FolderId: string;
    public Name: string;
    constructor(myFolder: BIReportFolderList, myReports: BIReportList[]) {
        this.folder = myFolder;
        this.reportsList = myReports;
        this.FolderId = myFolder.Id;
        this.Name = myFolder.Name;
        this.FolderIcon = this.FolderIcon + SessionLocator.CurrentSession.GetNewId(this.FolderIcon);

        this.ComputeTitle();
    }

    private ComputeTitle() {
        this.Title = this.folder.Name + " (" + this.reportsList.length + ")";        
    }
    
    public FolderIcon: string = "folder_icon";
    public LinkColor: string = "#282E30";
    FolderIconMouseOver() {
        var img = document.getElementById(this.FolderIcon);
        img.setAttribute("src", "./Images/Icons/Folder_L.png");

        this.LinkColor = "#1B90CB";
    }
    FolderIconMouseLeave() {
        var img = document.getElementById(this.FolderIcon);
        img.setAttribute("src", "./Images/Icons/Folder_B.png");

        this.LinkColor = "#282E30";
    }


}

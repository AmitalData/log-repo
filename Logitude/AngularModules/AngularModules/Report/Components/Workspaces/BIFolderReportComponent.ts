import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BIReportList } from '../../../Infrastructure/EntityLists/BIReportList';
import { BIReportListService } from '../../../Infrastructure/Services/StandardLists/BIReportListService';
import { BIReportFolderList } from '../../../Infrastructure/EntityLists/BIReportFolderList';
import { BIReportFolderListService } from '../../../Infrastructure/Services/StandardLists/BIReportFolderListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../Infrastructure/Args';

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIFolderReportComponent.html',
})

export class BIFolderReportComponent {
    public ItemsSource: BIFolderClass[] = [];
    private folderListService: BIReportFolderListService;
    private reportListService: BIReportListService;
    constructor() {
        this.folderListService = new BIReportFolderListService();
        this.reportListService = new BIReportListService();

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
                this.folderList = this.folderList.sort((a, b) => { return a.Index - b.Index });

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
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
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
    private reportsList: BIReportList[];
    public Title: string;
    public FolderId: string;
    public Name: string;
    constructor(myFolder: BIReportFolderList, myReports: BIReportList[]) {
        this.folder = myFolder;
        this.reportsList = myReports;
        this.FolderId = myFolder.Id;
        this.Name = myFolder.Name;

        this.ComputeTitle();
    }


    private ComputeTitle() {
        this.Title = this.folder.Name + " (" + this.reportsList.length + ")";        
    }
    
    public FolderIcon: string = "folder_icon";
    FolderIconMouseOver() {
        var img = document.getElementById(this.FolderIcon);
        img.setAttribute("src", "./Images/Icons/Folder_L.png");
    }
    FolderIconMouseLeave() {
        var img = document.getElementById(this.FolderIcon);
        img.setAttribute("src", "./Images/Icons/Folder_B.png");
    }
}

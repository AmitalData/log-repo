declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeCopyPM} from '../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    
    selector: 'SharedDocumentsPermissionsComponent',
    templateUrl: './SharedDocumentsPermissionsComponent.html',
    providers: [DocumentTypePMExtendedService],
})

export class SharedDocumentsPermissionsComponent implements OnInit {

    AllDocumentPermissiosLists: SharedDocumentsPermissionsViewModel[];
    DocumentPermissiosSelectedViewModel: SharedDocumentsPermissionsViewModel;
    DocumentPermissiosLists: SharedDocumentsPermissionsViewModel[];
    ObjectTableId: string;
    FullComponentsVisibility: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypePMExtendedService: DocumentTypePMExtendedService) {
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit() {
        this.Run();
    }


    Run() {
        this.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Shipment")[0].Id;
        this.LoadData();
    }

    LoadData() {
        this.DocumentPermissiosLists = [];
        this.AllDocumentPermissiosLists = [];
        this._documentTypePMExtendedService.GetDocumentTypesPMByObjectTableIdForDocumentPremissions(this.ObjectTableId, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myList = pmResponse.Result;
                myList.forEach((item) => {
                    this.DocumentPermissiosLists.push(new SharedDocumentsPermissionsViewModel(item));
                    this.AllDocumentPermissiosLists.push(new SharedDocumentsPermissionsViewModel(item));
                });

            }
            this.SortItemSource();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    mySearchText: string;


    BuildData() {

        this.DocumentPermissiosLists = [];
        if (!this.mySearchText) {
            this.DocumentPermissiosLists = this.AllDocumentPermissiosLists;
        }
        else {

            this.DocumentPermissiosLists = this.AllDocumentPermissiosLists.filter(d => (d.EntityPM.Code && d.EntityPM.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.EntityPM.Name && d.EntityPM.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }


        this.SortItemSource();
    }

    
    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }
    
    SaveButtonClicked() {
        var documentTypePMList: DocumentTypePM[] = [];

        this.CurrentSession.StartBusyIndicatorSaving();

        this.DocumentPermissiosLists.forEach((item) => {

            if (item.EntityPM.IsDirty) {
                documentTypePMList.push(item.EntityPM);
            }
        });

        if (documentTypePMList.length > 0) {

            this._documentTypePMExtendedService.update(documentTypePMList).subscribe((res:any) => {
                this.CloseButtonClicked();
            });
        }
        else {
            this.CloseButtonClicked();
        }


    }
    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
    }

    SortItemSource() {

        this.DocumentPermissiosLists = this.DocumentPermissiosLists.sort((a, b) => {
            if (a.DocumentTypeName.toLowerCase() < b.DocumentTypeName.toLowerCase()) {
                return -1;
            }
            else if (a.DocumentTypeName.toLowerCase() > b.DocumentTypeName.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });
    }


}


export class SharedDocumentsPermissionsViewModel {


    DocumentTypeName: string;




    IsAgentSharedInHouseEnable: boolean;
    IsAgentSharedInMasterEnable: boolean;
    IsAgentSharedInDirectEnable: boolean;

    CopyName: string;
    EntityPM: DocumentTypePM;
    DocumentTypeCopyLists: DocumentTypeCopyPM[] = [];
    SelectedDocumentTypeCopy: DocumentTypeCopyPM;
    constructor(entityPM: DocumentTypePM) {
        this.EntityPM = entityPM;
        this.DocumentTypeName = entityPM.Name;
        this.IsAgentSharedInHouseEnable = entityPM.IsHouse;
        this.IsAgentSharedInMasterEnable = entityPM.IsMaster;
        this.IsAgentSharedInDirectEnable = entityPM.IsDirect;
        if (entityPM.DocumentTypeCopies && entityPM.DocumentTypeCopies.length > 1) {
            this.DocumentTypeCopyLists = entityPM.DocumentTypeCopies;
            if (this.EntityPM.SharedDocumentTypeCopyId) {
                this.SelectedDocumentTypeCopy = this.DocumentTypeCopyLists.filter(d => d.Id == this.EntityPM.SharedDocumentTypeCopyId)[0];
            } 

            if (!this.SelectedDocumentTypeCopy) this.SelectedDocumentTypeCopy = this.DocumentTypeCopyLists[0];
        }

    }
 

    public get IsAgentSharedInHouse() {

        if (this.EntityPM) {
            return this.EntityPM.IsAgentSharedInHouse;
        }
        else return false;
    }
    public set IsAgentSharedInHouse(value: boolean) {
        if (this.EntityPM != null) {
            this.EntityPM.IsAgentSharedInHouse = value;
            this.SetSharedDocumentTypeCopyId();
        }

    }

    public get IsAgentSharedInDirect() {

        if (this.EntityPM) {
            return this.EntityPM.IsAgentSharedInDirect;
        }
        else return false;
    }
    public set IsAgentSharedInDirect(value: boolean) {
        if (this.EntityPM != null) {
            this.EntityPM.IsAgentSharedInDirect = value;
            this.SetSharedDocumentTypeCopyId();
        }

    }


    public get IsAgentSharedInMaster() {

        if (this.EntityPM) {
            return this.EntityPM.IsAgentSharedInMaster;
        }
        else return false;
    }
    public set IsAgentSharedInMaster(value: boolean) {
        if (this.EntityPM != null) {
            this.EntityPM.IsAgentSharedInMaster = value;
            this.SetSharedDocumentTypeCopyId();
           
        }

    }


    DocumentTypeCopyListValueChanged(copy: DocumentTypeCopyPM) {
        if (copy) {
            this.EntityPM.SharedDocumentTypeCopyId = copy.Id;
            this.SelectedDocumentTypeCopy = copy;
        }
    }


    SetSharedDocumentTypeCopyId() {

        if (AppTool.IsNullOrEmpty(this.EntityPM.SharedDocumentTypeCopyId)) {
            if (this.SelectedDocumentTypeCopy) {
                this.EntityPM.SharedDocumentTypeCopyId = this.SelectedDocumentTypeCopy.Id;
            }
            else if (this.EntityPM.DocumentTypeCopies && this.EntityPM.DocumentTypeCopies.length > 0) {
                this.EntityPM.SharedDocumentTypeCopyId = this.EntityPM.DocumentTypeCopies[0].Id;
            }
        }

    }
}

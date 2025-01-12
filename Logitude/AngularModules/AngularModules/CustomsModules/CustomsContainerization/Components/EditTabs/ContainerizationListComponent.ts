
import { Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ContainerizationListService } from '../../../../Customs/Services/StandardLists/ContainerizationListService';
import { INF_MSG_GenericResponseData } from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ContainerizationExtendedListService } from '../../../../Customs/Services/ExtendedLists/ContainerizationExtendedListService';



@Component({

    templateUrl: './ContainerizationListComponent.html',
})

export class ContainerizationListComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public YellowMessage: string;
    public data = [];
    public ObjectTableName: string = "Customs.Containerization";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;
    public Counter: number = 0;
    public tempRes: any[] = [];
    AddDeclarationToContainerizationEVENT: any;
    ResponseData: INF_MSG_GenericResponseData;
    containerizationExtendedListService: ContainerizationExtendedListService = new ContainerizationExtendedListService();
    ontainerizationListService: ContainerizationListService = new ContainerizationListService();
    public ContainerizationDeclarationList: ObservableCollection;
    public declarationList: ObservableCollection;
    IsDisplayOnly: boolean = false;
    visibile: boolean = true;

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Containerization").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                    this.EntityPM = this.entityArgs.EntityPM;
                    this.ObjectTableName = this.entityArgs.ObjectTableName;
                    this.getRows();
                });
            });
        });

    }

    getRows() {
        let _containersId: string = this.EntityPM.Consignments?.map((x: any) => x?.exportContainerizationID).toString().split(',');
        this.ContainerizationDeclarationList = new ObservableCollection([]);
        this.containerizationExtendedListService.GetContainerizationsByIds(_containersId).subscribe((res: any) => {
            this.ContainerizationDeclarationList.InsertCollection(res.Result);
            SessionLocator.SelectedSession.StopBusyIndicator();

        });
    }

    rowClicked(item) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                console.log(item)
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: item.Id,
                    ObjectTableName: "Customs.Containerization"
                });
                cmpRef.instance.BackCompleted.subscribe(() => {
                    this.getRows();
                });
            })


    }

    get ContainerizationDate() { return this.EntityPM != null ? this.EntityPM.ContainerizationDate : null; }
    set ContainerizationDate(value) { this.EntityPM.ContainerizationDate = value; }

}


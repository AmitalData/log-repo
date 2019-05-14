import { Component, OnDestroy } from '@angular/core';
import { DatePipe } from '@angular/common';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { TariffVersionPM } from '../../../../TariffModule/EntityPMs/TariffVersionPM';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';

@Component({
    moduleId: module.id,
    templateUrl: './VersionHistoryTabComponent.html',
})

export class VersionHistoryTabComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public VersionPM: TariffVersionPM;
    public VersionLinesSource: ObservableCollection;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.VersionLinesSource = new ObservableCollection([]);

        this.SetStepsLabelsAndVisibility();
        this.BuildVersionsList();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
    }

    public Step1PriceLabel: string;
    public Step2PriceLabel: string;
    public Step3PriceLabel: string;
    public Step4PriceLabel: string;
    public Step5PriceLabel: string;
    public Step6PriceLabel: string;
    public Step7PriceLabel: string;
    public Step8PriceLabel: string;

    public Step1PriceVisibility: boolean;
    public Step2PriceVisibility: boolean;
    public Step3PriceVisibility: boolean;
    public Step4PriceVisibility: boolean;
    public Step5PriceVisibility: boolean;
    public Step6PriceVisibility: boolean;
    public Step7PriceVisibility: boolean;
    public Step8PriceVisibility: boolean;

    SetStepsLabelsAndVisibility() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.PriceSteps)) {
            if (this.EntityPM.PriceSteps.indexOf(',') > -1) {
                var steps: string[] = [] = this.EntityPM.PriceSteps.split(",");
                var count = steps.length;
                if (count == 1) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step1PriceVisibility = true;
                }
                else if (count == 2) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                }
                else if (count == 3) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                }
                else if (count == 4) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                }
                else if (count == 5) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                }
                else if (count == 6) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                }
                else if (count == 7) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                }
                else if (count == 8) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step8PriceLabel = steps[7] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                    this.Step8PriceVisibility = true;
                }
            }
            else {
                this.Step1PriceLabel = this.EntityPM.PriceSteps;
                this.Step1PriceVisibility = true;
            }
        }
    }
    
    public VersionsList: CodeNameClass[];
    private BuildVersionsList() {
        this.VersionsList = [];

        var datePipe: DatePipe = new DatePipe("en-US");

        this.EntityPM.TariffVersions.forEach(item => {
            var from: string = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
            var to: string = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');

            var newVersion: CodeNameClass = new CodeNameClass();
            newVersion.Code_Int = item.Version;
            newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";

            this.VersionsList.push(newVersion);
        });

        this.SelectedVersion = this.VersionsList[0];
    }

    private selectedVersion: CodeNameClass;
    get SelectedVersion() { return this.selectedVersion; }
    set SelectedVersion(value: CodeNameClass) {
        if (this.selectedVersion != value) {
            this.selectedVersion = value;

            this.VersionPM = this.EntityPM.TariffVersions.filter(d => d.Version == this.SelectedVersion.Code_Int)[0];
            this.FillLines();
        }
    }

    private FillLines() {
        this.VersionLinesSource.Clear();
        var itemsCollection: TariffLinePM[] = [];

        this.VersionPM.TariffLines.forEach(item => {
            itemsCollection.push(item);
        });

        this.VersionLinesSource.InsertCollection(itemsCollection);
    }
}

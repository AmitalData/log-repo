import { Component } from '@angular/core';
import { TariffLineData } from './TariffGeneralTabComponent';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditTariffLineComponent.html',
})

export class AddEditTariffLineComponent {

    public EntityPM: TariffLinePM;
    public DataContext: TariffLineData;
    public ObjectTableName: string = "TariffLine";
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {

    }


    SetDataContext(dataContext: TariffLineData) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.TariffLine;
        //this.Clone();
    }
}

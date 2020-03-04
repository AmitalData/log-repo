import { Component} from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TariffValidator } from '../../Validators/TariffValidator';
import { TariffPM } from '../../EntityPMs/TariffPM';


@Component({
    selector: 'AddTariffContainersComponent',
    moduleId: module.id,
    templateUrl: './AddTariffContainersComponent.html',
})

export class AddTariffContainersComponent extends BaseComponent {
    public DataContext: AddTariffContainersComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[] = [];
    public EntityPM: TariffPM;

    constructor() {
        super();
        this.EntityPM = new TariffPM();
       
    }

    SetWindowArgs(args) {
        this.ContainerType1Id = args.ContainerType1Id;
        this.ContainerType2Id = args.ContainerType2Id;
        this.ContainerType3Id = args.ContainerType3Id;
        this.ContainerType4Id = args.ContainerType4Id;
        this.ContainerType5Id = args.ContainerType5Id;
    }

    get ContainerType1Id() {
        return this.EntityPM.ContainerType1Id;
    }
    set ContainerType1Id(value: string) {
        if (this.EntityPM.ContainerType1Id != value) {
            this.EntityPM.ContainerType1Id = value;
        }
    }

    get ContainerType2Id() {
        return this.EntityPM.ContainerType2Id;
    }
    set ContainerType2Id(value: string) {
        if (this.EntityPM.ContainerType2Id != value) {
            this.EntityPM.ContainerType2Id = value;
        }
    }

    get ContainerType3Id() {
        return this.EntityPM.ContainerType3Id;
    }
    set ContainerType3Id(value: string) {
        if (this.EntityPM.ContainerType3Id != value) {
            this.EntityPM.ContainerType3Id = value;
        }
    }

    get ContainerType4Id() {
        return this.EntityPM.ContainerType4Id;
    }
    set ContainerType4Id(value: string) {
        if (this.EntityPM.ContainerType4Id != value) {
            this.EntityPM.ContainerType4Id = value;
        }
    }

    get ContainerType5Id() {
        return this.EntityPM.ContainerType5Id;
    }
    set ContainerType5Id(value: string) {
        if (this.EntityPM.ContainerType5Id != value) {
            this.EntityPM.ContainerType5Id = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var validator = new TariffValidator();
        //validator.FillContainersIDs();
        //var validateContainersErrors = validator.ValidateContainers(this.EntityPM);
        //if (!validateContainersErrors) {
        //    validateContainersErrors.forEach(error => {
        //        this.ValidationErrorsList.push(error);
        //    });
        //}
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }
}

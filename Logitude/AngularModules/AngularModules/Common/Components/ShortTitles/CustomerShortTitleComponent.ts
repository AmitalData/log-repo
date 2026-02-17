import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {CustomerPM} from '../../EntityPMs/CustomerPM';

@Component({
    moduleId: module.id,
    templateUrl: "./CustomerShortTitleComponent.html",
})

export class CustomerShortTitleComponent {
    public EntityPM: CustomerPM;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            //this.BuildComponent();
        }
    }

    get EnglishName() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.EnglishName;
        }

        return myResult;
    }

    get RankName() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.RankName;
        }

        return myResult;
    }

    get RankSource1() {
        var myResult: string = null;
        // silver to lower
        if (this.EntityPM != null) {
            var RankCode = this.EntityPM.RankCode;

            switch (RankCode) {
                case "1":
                case "2":
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }

    get RankSource2() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            var RankCode = this.EntityPM.RankCode;

            switch (RankCode) {                
                case "2":
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }

    get RankSource3() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            var RankCode = this.EntityPM.RankCode;

            switch (RankCode) {
                case "3": {
                    myResult = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    myResult = "./Images/Icons/StarGray.png";
                    break;
                }
            }
        }

        return myResult;
    }

}
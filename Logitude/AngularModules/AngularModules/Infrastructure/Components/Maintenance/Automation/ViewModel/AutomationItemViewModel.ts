
import {AutomationPM} from '../../../../../Common/EntityPMs/AutomationPMExtended';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';

export class AutomationItemViewModel {

    public EntityPM: AutomationPM;
    public Id: string;
    public CreateDate: Date;
    public UpdateDate: Date;
    public CreatedByUserName: string;
    public UpdatedByUserName: string;
    public ResultCode: string;
    public ResultName: string;
    public From: string;
    public FromEmail: string;
    public Version: number;

    IsShowArrowUpDown: boolean = false;
    public Inactive: boolean;
    IsEditAtomationEnable: boolean;
    Order: number;

    Tenant: number;

    //public KeyInActive: string;
    //public KeyInRequired: string;



    private description: string = "";
    get Description() {
        if (this.EntityPM) {
            this.description = this.EntityPM.Description;
        }

        return this.description;

    }
    set Description(newValue: string) {
        if (this.Description != newValue) {
            this.Description = newValue;
            this.EntityPM.Description = newValue;
           
        }
    }



    private name: string = "";
    get Name() {
        if (this.EntityPM) {
            this.name = this.EntityPM.Name;
        }

        return this.name;
    }
    set Name(newValue: string) {
        if (this.Name != newValue) {
            this.Name = newValue;
            this.EntityPM.Name = newValue;
        }
    }

    //private order: number =0;
    //get Order() {

    //    if (this.EntityPM) {
    //        this.order = this.EntityPM.Order;
    //    }

    //    return this.order;
    //}
    //set Order(newValue: number) {
    //    if (this.Order != newValue) {
    //        this.Order = newValue;
    //        this.EntityPM.Order = newValue;
    //    }
    //}




    public constructor(entityPM: AutomationPM) {


        this.EntityPM = entityPM;
        this.Id = entityPM.Id;
        this.CreateDate = entityPM.CreateDate;
        this.UpdateDate = entityPM.UpdateDate;
        this.CreatedByUserName = entityPM.CreatedByUserName;
        this.UpdatedByUserName = entityPM.UpdatedByUserName;
        this.ResultCode = entityPM.ResultCode;
        this.From = entityPM.From;
        this.FromEmail = entityPM.FromEmail;
        this.Inactive = entityPM.Inactive;
        this.Name = entityPM.Name;
        this.Description = entityPM.Description;
        this.Order = entityPM.Order;
        this.Version = entityPM.Version;
        this.Tenant = entityPM.Tenant;
        //"DOCOUTFOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCINFOLLOWUP"
        if (entityPM && !AppTool.IsNullOrEmpty(entityPM.ResultCode)) {
            if (entityPM.ResultCode == "EMAIL") this.ResultName = "E-mail";
            else if (entityPM.ResultCode == "FIELDSET") this.ResultName = "Set Fields Value";
            else if (entityPM.ResultCode == "FOLLOWUP") this.ResultName = "F/U Creation";
            else if (entityPM.ResultCode == "DOCINFOLLOWUP") this.ResultName = "Doc In F/U Creation";
            else if (entityPM.ResultCode == "DOCOUTFOLLOWUP") this.ResultName = "Doc Out F/U Creation";
            else if (entityPM.ResultCode == "SETSLA") this.ResultName = "Set SLA";
            else if (entityPM.ResultCode == "QUEUE") this.ResultName = "Queued Task";
        }

        if (!FeatureLocator.HasFeaturePermession("Automation", "UPDATE")) this.IsEditAtomationEnable = false;
      
        else   this.IsEditAtomationEnable = true;
   



    }


}
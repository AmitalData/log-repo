import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import { DecCargoSplitConsItemPM } from '../../../../Customs/EntityPMs/DecCargoSplitConsItemPM';
import { DecCargoSplitConPM } from '../../../../Customs/EntityPMs/DecCargoSplitConPM';
import { DecCargoSplitConsPackDetPM } from '../../../../Customs/EntityPMs/DecCargoSplitConsPackDetPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
declare var window: any;
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmationTypePM } from  '../../../../Customs/EntityPMs/ConfirmationTypePM';
import { PackingTypePM } from  '../../../../Customs/EntityPMs/PackingTypePM';
import { PackingTypeListService } from  '../../../../Customs/Services/StandardLists/PackingTypeListService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
//import { DecCargoSplitConPMService } from '../../../Services/StandardPMs/DecCargoSplitConPMService';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import { DeclarationCargoSplitPM } from '../../../../Customs/EntityPMs/DeclarationCargoSplitPM';
import { DeclarationCargoSplitPMService } from '../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService';
declare var window: any;

@Component({
    
    templateUrl: './DecCargoSplitConsPackDetComponent.html',
})

export class DecCargoSplitConsPackDetComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.DecCargoSplitConsPackDet";
    public DataContext = this;
    public decCargoSplitConsItemPM: DecCargoSplitConsItemPM;
    public decCargoSplitCon: DecCargoSplitConPM;
    public declarationCargoSplitPM: DeclarationCargoSplitPM;
    
    public ItemsSource: ObservableCollection;
    declarationCargoSplitPMService: DeclarationCargoSplitPMService = new DeclarationCargoSplitPMService();
    //public DecCargoSplitConPMService: DecCargoSplitConPMService = new DecCargoSplitConPMService();

    public ValidationErrorsList: string[] = [];
    public OriginalItemPM: DecCargoSplitConsItemPM;
    public ClonedItemPM: DecCargoSplitConsItemPM;
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    IsHeaderVisible: boolean = false;
    IsChanged: boolean = false;
    //IsFromCustomsAnswers: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

    }

    IsExportDeclaration: boolean=false;
    LineNumber: string;
    SetWindowArgs(args: any) {
        var _entityResourceService: EntityResourceService = new EntityResourceService();
        if (args.DeclarationDirection != null && args.DeclarationDirection=="E") {
            this.IsExportDeclaration = true;
        }
        _entityResourceService.getEntityResourceByTableName("Customs.PackingType", 0).subscribe((res: any) => {
            var entityListService: PackingTypeListService = new PackingTypeListService();
            entityListService.getAllFromCache().subscribe((res: any) => {
            });
        });
            
        if (!AppTool.IsNullOrEmpty(args)) {
            this.decCargoSplitConsItemPM = args.DecCargoSplitConsItemPM;
            this.declarationCargoSplitPM = args.DeclarationCargoSplitPM;
            this.BuildCertificatesList();
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.OriginalItemPM = args.DecCargoSplitConsItemPM;
            this.ClonedItemPM = this.CloneEntity(args.DecCargoSplitConsItemPM);
            
            
            if (!AppTool.IsNullOrEmpty(args.DeclarationError)) {
                //this.IsFromCustomsAnswers = true;
                this.IsHeaderVisible = true;

                this.decCargoSplitCon = args.DecCargoSplitConPM;
                //this.PackageTypeCode = args.PackageTypeCode;

                //Select a line
                this.LineNumber = args.LineNumber;
                this.LineNumber = this.LineNumber.split(",")[0];
                var selectedRow = this.ItemsSource.Collection.find(d => d.DecCargoSplitConsItemLine == this.LineNumber);
                this.SelectedRow = selectedRow;

                this.ShowXMLErrors(args.DeclarationError);
            }
            if (!AppTool.IsNullOrEmpty(args.AmendmentView)) {
                this.IsHeaderVisible = true;

                this.decCargoSplitCon = args.DecCargoSplitConPM;
                //this.PackageTypeCode = args.PackageTypeCode;
                this.ShowXMLCorrections(args.AmendmentView);
            }

        }
    }


    ShowXMLErrors(error) {
        if (!AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, this.ObjectTableName, false, error.Description);
        }

        var errors = [];
        if (!AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors: any[] = error.Description.split(/,|:/); 
            for (var xmlError of xmlErrors) {
                errors.push(xmlError);
            }
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "DecCargoSplitConsItem") {
                
            }
        }
    }

    ShowXMLCorrections(error) {
        if (!AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, "Customs.DecCargoSplitConsPackDet", false, error.ErrorType);
        }
        var errors = [];
        errors.push(error.ErrorType);
        this.ValidationErrorsList = errors;
    }


    BuildCertificatesList() {
        this.ItemsSource.Clear();
        for (let item of this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets) {
            this.ItemsSource.Insert(new DecCargoSplitConsPackDetLine(item, this));
        }

    }

    Add() {
        if (!this.IsDisplayOnly) {
            var counter: number = 0;
           
            if (this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.length > 0) {

                var items = this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.sort((a, b) => { return (a.PackageLine === b.PackageLine) ? 0 : (a.PackageLine < b.PackageLine) ? -1 : 1 });
                if (items.length == 0) counter = 0;
                else {
                    counter = items[this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.length - 1].PackageLine;
                }

            }

            counter += 1;

            var item: DecCargoSplitConsPackDetPM = new DecCargoSplitConsPackDetPM(this.decCargoSplitConsItemPM);

            item.DeclarationCargoSplitId = this.decCargoSplitConsItemPM.DeclarationCargoSplitId;
            item.Tenant = this.decCargoSplitConsItemPM.Tenant;
            item.DecCargoSplitConsLineNo = this.decCargoSplitConsItemPM.DecCargoSplitConsLineNo;
            item.DecCargoSplitConsItemLine = this.decCargoSplitConsItemPM.ItemLine;
            item.PackageLine = counter;

            if (!this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.includes(item)) {
                this.decCargoSplitConsItemPM.AddDecCargoSplitConsPackDet(item);
                this.ItemsSource.Insert(new DecCargoSplitConsPackDetLine(item, this));

            }

        }
 
    }



    CloneEntity(entityToClone: DecCargoSplitConsItemPM) {

        var clonedEntity: DecCargoSplitConsItemPM;
        clonedEntity = new DecCargoSplitConsItemPM(entityToClone.EntityParentPM); 

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        
        clonedEntity.DecCargoSplitConsPackDets = [];
        entityToClone.DecCargoSplitConsPackDets.forEach((itemMod) => {
            var clonedItemMod = new DecCargoSplitConsPackDetPM(itemMod.EntityParentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.DecCargoSplitConsPackDets.push(clonedItemMod);
        });
        
        return clonedEntity;
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }

    CancelButtonClicked() {

        if (!this.IsDisplayOnly && this.IsChanged) {
            var confirm = new ConfirmWindow();

            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator.Translate("Customs.General.O.CancelMessage"));
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    this.OkButtonClicked();
                    
                }
                else {
                    this.RejectChanges();
                    this.CurrentSession.CloseCurrentWindow();
                }

            });

        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit('cancel');
        }
        
    }

    isValid: boolean;
    inValid: boolean;
     hasRequest:boolean;
    notMandatoryIsNotEmpty: boolean = false;
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        this.isValid = true;
        this.inValid = false;

        for (let item of this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets) {
            
            if (AppTool.IsNullOrEmpty(item.PackageTypeCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsPackDet.F.PackageTypeCode")));
                this.inValid = true;
                this.isValid = false;
                break;
            }


            else {

                if (item.PackageQuantity == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsPackDet.F.PackageQuantity")));
                    this.inValid = true;
                    this.isValid = false;
                    break;
                }
                else {
                    if (item.PackageQuantity.toString().length > 8) {
                        errors.push("כמות לא יכולה להיות ארוכה משמונה תווים");
                        this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", false, "כמות לא יכולה להיות ארוכה משמונה תווים");
                        this.inValid = false;
                        this.isValid = false;
                        break;
                    }
                    else {
                        this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
                        if (item.GrossMassMeasure != null) {
                            var strValue = item.GrossMassMeasure.toString();
                            if (strValue.indexOf(".") > -1) strValue = item.GrossMassMeasure.toString().substring(0, item.GrossMassMeasure.toString().indexOf("."));
                            if (AppTool.IsNullOrEmpty(strValue)) {
                                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
                            }
                            else if (strValue.length > 11) {
                                errors.push("משקל לא יכול להיות ארוך מאחד עשר תווים");
                                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", false, "משקל לא יכול להיות ארוך מאחד עשר תווים");
                                this.inValid = false;
                                this.isValid = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (errors.length != 0) {
            this.ValidationErrorsList = errors;
        }
        if (this.inValid) {
            this.isValid = false;

            var confirm = new ConfirmWindow();


            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            //confirm.ShowNoButton = true;
            

            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {

                    if (errors.length == 0) {
                        var isSave = 1;
                        if (isSave == 1) {
                            this.OriginalItemPM.ChangeSetOp = "Update";
                            this.CurrentSession.CloseCurrentWindow();
                            /*
                            this.declarationCargoSplitPMService.update(this.declarationCargoSplitPM).subscribe((response: any) => {
                                var result = response.Result;
                                this.IsChanged = false;
                                console.log("[response/declarationCargoSplitPMService.update]", result);
                                this.CurrentSession.CloseCurrentWindow();
                                if (!AppTool.IsNullOrEmpty(result)) {

                                } else {
                                }
                            });
                            */
                        }
                        else {
                            this.CurrentSession.CloseCurrentWindow();
                        }
                    }

                }
                else {
                    this.ValidationErrorsList = errors;
                }

                confirm.Close();
            });
            
           // confirm.Show(errors.toString());
        }

        else {

            if (errors.length == 0) {

                var isSave = 1;
                if (isSave == 1) {
                    this.OriginalItemPM.ChangeSetOp = "Update";
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                    //this.CurrentSession.CloseCurrentWindow();
                    /*
                    this.declarationCargoSplitPMService.update(this.declarationCargoSplitPM).subscribe((response: any) => {
                        var result = response.Result;
                        this.IsChanged = false;
                        console.log("[response/declarationCargoSplitPMService.update]", result);
                        this.CurrentSession.CloseCurrentWindow();
                        if (!AppTool.IsNullOrEmpty(result)) {

                        } else {
                        }
                    });
                    */
                }
                else {
                    this.CurrentSession.CloseCurrentWindow();
                }
            }

            else {
                this.ValidationErrorsList = errors;
            }
        }
     
        return this.isValid;
        
    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
        this.IsChanged = true;
    }

    OnRowEnded($event) {
        console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            this.Add();

        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    }

}

export class DecCargoSplitConsPackDetLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "Customs.DecCargoSplitConsPackDet";
    public entityPM: DecCargoSplitConsPackDetPM;
    public decCargoSplitConsItem: DecCargoSplitConsItemPM;
    public parent: DecCargoSplitConsPackDetComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(EntityPM: DecCargoSplitConsPackDetPM, Parent: DecCargoSplitConsPackDetComponent) {
        super();
        this.entityPM = EntityPM;

        this.parent = Parent;
       
    }

    //#region properties
    
    

    get PackageTypeCode() { return this.entityPM.PackageTypeCode; }
    set PackageTypeCode(value: string) {
        if (this.entityPM.PackageTypeCode != value) {
            this.entityPM.PackageTypeCode = value;

        }
    }

    get PackageTypeName() { return this.entityPM.PackageTypeName; }
    set PackageTypeName(value: string) {
        if (this.entityPM.PackageTypeName != value) {
            this.entityPM.PackageTypeName = value;

        }
    }
    
    packingType: PackingTypePM;
    get PackingType() { return this.PackingType; }
    set PackingType(value: PackingTypePM) {

        if (this.PackingType != value) {
            this.PackingType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.PackageTypeName = value.LocalName;


        } else {
            this.PackageTypeCode = null;
            this.PackageTypeName = null;
        }
    }
    
    get PackageQuantity() { return this.entityPM.PackageQuantity; }
    set PackageQuantity(value: number) {
        if (this.entityPM.PackageQuantity != value) {
            this.entityPM.PackageQuantity = value;

        }
    }

    get GrossMassMeasure() { return this.entityPM.GrossMassMeasure; }
    set GrossMassMeasure(value: number) {
        if (this.entityPM.GrossMassMeasure != value) {
            this.entityPM.GrossMassMeasure = value;

        }
    }

    get MarksNumbers() { return this.entityPM.MarksNumbers; }
    set MarksNumbers(value: string) {
        if (this.entityPM.MarksNumbers != value) {
            this.entityPM.MarksNumbers = value;

        }
    }

    get ManifestNumber() { return this.entityPM.ManifestNumber; }
    set ManifestNumber(value: string) {
        if (this.entityPM.ManifestNumber != value) {
            this.entityPM.ManifestNumber = value;

        }
    }
        //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
   

    DeleteButtonClicked() {

        this.parent.ItemsSource.Remove(this);
        if (this.parent.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.includes(this.entityPM)) {
            this.parent.decCargoSplitConsItemPM.RemoveDecCargoSplitConsPackDet(this.entityPM);
        }

    //    this.parent.BuildCertificatesList();



    }
    
    valid: boolean = true;

    PackageQuantityKeyUp(event, logCellTemplate: any, packageQuantityTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnPackageQuantityLostFocus(logCellTemplate, packageQuantityTextBox);
        }
    }

    OnPackageQuantityLostFocus(logCellTemplate: any, packageQuantityTextBox: any) {
        var newValue = this.PackageQuantity;
        this.valid = true;
        this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
        }
        else if (newValue.toString().length > 8) {
            this.valid = false;
            this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", false, "כמות לא יכולה להיות ארוכה משמונה תווים");
        }
        else {
            this.valid = true;
            this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
        }

        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: packageQuantityTextBox.InputId });

        }
        //this.PackageQuantity = newValue;
        //packageQuantityTextBox.TextValue = newValue;
    }

    GrossMassMeasureKeyUp(event, logCellTemplate: any, grossMassMeasureTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnGrossMassMeasureLostFocus(logCellTemplate, grossMassMeasureTextBox);
        }
    }

    OnGrossMassMeasureLostFocus(logCellTemplate: any, grossMassMeasureTextBox: any) {
        var newValue = this.GrossMassMeasure;
        this.valid = true;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
        }
        else {
            this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1) strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (AppTool.IsNullOrEmpty(strValue)) {
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
            }
            else if (strValue.length > 11) {
                this.valid = false;
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", false, "משקל לא יכול להיות ארוך מאחד עשר תווים");
            }
            else {
                this.valid = true;
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
            }
        }
        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: grossMassMeasureTextBox.InputId });

        }
    }
}

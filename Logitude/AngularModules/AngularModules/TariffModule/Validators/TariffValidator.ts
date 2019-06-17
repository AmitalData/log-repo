import {AppTool, DateTool} from '../../Infrastructure/Tools';
import { ChargesTypeListService } from '../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../Common/EntityLists/ChargesTypeList';

export class TariffValidator {
    private IdProps: string[] = [];
    private UOMProps: string[] = [];
    private chargesTypePMService: ChargesTypeListService;

    public Validate(entityPM: any) {
        var error: any = [];

        if (entityPM.TypeCode == "ASC") {
            this.chargesTypePMService = new ChargesTypeListService();
            this.FillChargesIDsAndUOMS();
            error = this.ValidateSurcharge(entityPM);
        }
        return error;

    }


    FillChargesIDsAndUOMS() {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    }


    ValidateSurcharge(entityPM) {
        var ValidationErrorsList = [];
            var IdProps: string[] = [];
            var UOMProps: string[] = [];
            var IdPropsName: string[] = [];
            var UOMPropsName: string[] = [];
            var DuplicatedChargesIds: string[] = [];
            var EmptyIndex = 1;
            var emptyLines: boolean = false;
            var FirstLineEmpty: boolean = false;
            var tempErrors: Array<string> = [];
            for (var index = 1; index <= 10; index++) {
                IdProps.push("Surcharge" + index + "Id");
                UOMProps.push("Surcharge" + index + "UOM");

                IdPropsName.push("Charge Type " + index);
                UOMPropsName.push("UOM " + index);
                if (this.IdProps.filter(p => entityPM[p + ""] == entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && entityPM[IdProps[index - 1]] != null)[0] != null) {
                    var chargresType = this.IdProps.filter(p => entityPM[p + ""] == entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && entityPM[IdProps[index - 1]] != null)[0];
                    if (!DuplicatedChargesIds.includes(entityPM[chargresType + ""])) {
                        DuplicatedChargesIds.push(entityPM[chargresType + ""]);
                        this.chargesTypePMService.getSingleFromCache(entityPM[chargresType + ""]).subscribe(res => {
                            if (!res.HasError) {
                                var chargesTypeList: ChargesTypeList = res.Result;
                                if (res) {
                                    ValidationErrorsList.push("Charge type " + chargesTypeList.EnglishName + " is duplicated");
                                }
                            }
                        });

                    }
                }
                if (index == 1) {
                    if (AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                        tempErrors.push(IdPropsName[index - 1] + " is required");
                        FirstLineEmpty = true;
                    }

                    if (AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]])) {
                        tempErrors.push(UOMPropsName[index - 1] + " is required");
                    }
                }

                else {



                    if (AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                        if (EmptyIndex == 1) {
                            EmptyIndex = index;
                        }
                    }

                    if (AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                        ValidationErrorsList.push(IdPropsName[index - 1] + " is filled without a UOM");
                    }
                    if (index == 2) {
                        if (!AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                            if (FirstLineEmpty) {
                                emptyLines = true;
                                ValidationErrorsList.push("Empty Charge Lines aren't allowed between line 1 and line 2");
                                EmptyIndex = 1;
                            }
                        }
                    }

                    if (index >= 3) {
                        if (!AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                            if (EmptyIndex != 1) {
                                emptyLines = true;
                                ValidationErrorsList.push("Empty Charge Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + index);
                                EmptyIndex = 1;
                            }
                            if (AppTool.IsNullOrEmpty(entityPM[IdProps[index - 2]])) {
                                //   this.ValidationErrorsList.push("no empty line between 2 charges in line "+index+ " and "+(index-2));
                            }
                        }
                    }
                }

            }

            if (!emptyLines) {
                tempErrors.forEach(error => {
                    ValidationErrorsList.push(error);
                });
        }

        return ValidationErrorsList;
        

    }
}

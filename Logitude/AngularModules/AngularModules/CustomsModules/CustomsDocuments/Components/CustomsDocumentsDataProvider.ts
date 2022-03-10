declare var window: any;
import { DeclarationCustomsDocumentsController } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/Documents/DeclarationCustomsDocumentsController';
import {CollateralCustomsDocumentsController} from '../../../CustomsModules/CustomsCollateral/Components/Documents/CollateralCustomsDocumentsController';
import { ClaimCustomsDocumentsController } from '../../../CustomsModules/CustomsClaim/Components/Documents/ClaimCustomsDocumentsController';
import { VehicleCustomsDocumentsController } from '../../../CustomsModules/CustomsVehicle/Components/Documents/VehicleCustomsDocumentsController';
import {CustDocRelatedDocsWebService} from '../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';
import {ICustomsDocumentsController} from './ICustomsDocumentsController';
import { DeclarationCancellationCustomsDocumentsController } from '../../CustomsDeclarationModules/DeclarationOthers/Components/DeclarationCancellation/Documents/DeclarationCancellationCustomsDocumentsController';
import { SpecialActivityCustomsDocumentsController } from '../../CustomsGeneralRequests/Components/Documents/SpecialActivityCustomsDocumentsController';
import { LogisticActionRequestCustomsDocumentsController } from 'CustomsModules/CustomsLogisticActionRequest/Components/EditTabs/LogisticActionRequestCustomsDocumentsController';

export class CustomsDocumentsDataProvider {
    private declarationCustomsDocumentsController: DeclarationCustomsDocumentsController;
    private collateralCustomsDocumentsController: CollateralCustomsDocumentsController;
    private declarationCancellationCustomsDocumentsController: DeclarationCancellationCustomsDocumentsController;
    private specialActivityCustomsDocumentsController: SpecialActivityCustomsDocumentsController;
    private logisticActionRequestCustomsDocumentsController: LogisticActionRequestCustomsDocumentsController;

    private claimCustomsDocumentsController: ClaimCustomsDocumentsController;
    private vehicleCustomsDocumentsController: VehicleCustomsDocumentsController;
    private custDocRelatedDocsWebService: CustDocRelatedDocsWebService;
    private ObjectTableId: string;
    private parentEntityCode: string;

    constructor(private objectTableName: string, private entityPM: any, private childEntity1Id = null, private childEntity1Name = null, private _parentEntityCode =null) {
        this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
        var objectTable = window.ObjectTables.filter(d => d.Name === this.objectTableName)[0];
        this.ObjectTableId = objectTable.Id;
        this.parentEntityCode = _parentEntityCode;
         switch (this.objectTableName) {
            case 'Customs.Declaration': {
                if (this.parentEntityCode == "DeclarationCancellation")
                    
                    this.declarationCancellationCustomsDocumentsController = new DeclarationCancellationCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                else if (this.parentEntityCode == "SpecialRequest")
                    this.specialActivityCustomsDocumentsController = new SpecialActivityCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);

                else
                this.declarationCustomsDocumentsController = new DeclarationCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
            case 'Customs.CustomsCollateral': {
                this.collateralCustomsDocumentsController = new CollateralCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
             
            case 'Customs.Claim': {
                this.claimCustomsDocumentsController = new ClaimCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
            case 'Customs.Vehicle': {
                this.vehicleCustomsDocumentsController = new VehicleCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
            case 'Customs.LogisticActionRequest': {
                this.logisticActionRequestCustomsDocumentsController = new LogisticActionRequestCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
        }
    }

    GetCustomsDocumentsController(): ICustomsDocumentsController {
         switch (this.objectTableName) {
            case 'Customs.Declaration': {
                if (this.parentEntityCode == "DeclarationCancellation")
                    return this.declarationCancellationCustomsDocumentsController;
                else if (this.parentEntityCode == "SpecialRequest")
                    return this.specialActivityCustomsDocumentsController;

                   else
                return this.declarationCustomsDocumentsController;
            }
            case 'Customs.CustomsCollateral': {
                return this.collateralCustomsDocumentsController;
            }
             
            case 'Customs.Claim': {
                return this.claimCustomsDocumentsController;
            }
            case 'Customs.Vehicle': {
                return this.vehicleCustomsDocumentsController;
            }
            case 'Customs.LogisticActionRequest': {
                return this.logisticActionRequestCustomsDocumentsController;
            }
        }
    }

   

    GetCustomsDocumentsRelatedDocuments(filterValue:string) {
        switch (this.objectTableName) {
            case 'Customs.Declaration': {
                if (this.entityPM.Direction == "E" && filterValue != "customs" ) {
                    return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', this.entityPM.ExportFile, filterValue, this.entityPM.Direction);

                }
                else {
                    return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', this.entityPM.CustomFileNo, filterValue, this.entityPM.Direction);

                }
                
            }
            case 'Customs.Claim': {
                return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', null, null);
            }
        }
    }
   
}

declare var window: any;
import { DeclarationCustomsDocumentsController } from '../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/Documents/DeclarationCustomsDocumentsController';
import {CollateralCustomsDocumentsController} from '../../../CustomsModules/CustomsCollateral/Components/Documents/CollateralCustomsDocumentsController';
import { ClaimCustomsDocumentsController } from '../../../CustomsModules/CustomsClaim/Components/Documents/ClaimCustomsDocumentsController';
import { VehicleCustomsDocumentsController } from '../../../CustomsModules/CustomsVehicle/Components/Documents/VehicleCustomsDocumentsController';
import {CustDocRelatedDocsWebService} from '../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';
import {ICustomsDocumentsController} from './ICustomsDocumentsController';

export class CustomsDocumentsDataProvider {
    private declarationCustomsDocumentsController: DeclarationCustomsDocumentsController;
    private collateralCustomsDocumentsController: CollateralCustomsDocumentsController;
    private claimCustomsDocumentsController: ClaimCustomsDocumentsController;
    private vehicleCustomsDocumentsController: VehicleCustomsDocumentsController;
    private custDocRelatedDocsWebService: CustDocRelatedDocsWebService;
    private ObjectTableId: string;
    constructor(private objectTableName: string, private entityPM: any, private childEntity1Id = null, private childEntity1Name = null) {
        this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
        var objectTable = window.ObjectTables.filter(d => d.Name === this.objectTableName)[0];
        this.ObjectTableId = objectTable.Id;
        switch (this.objectTableName) {
            case 'Customs.Declaration': {
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
        }
    }

    GetCustomsDocumentsController(): ICustomsDocumentsController {
        switch (this.objectTableName) {
            case 'Customs.Declaration':{
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
        }
    }

   

    GetCustomsDocumentsRelatedDocuments(filterValue:string) {
        switch (this.objectTableName) {
            case 'Customs.Declaration': {
                return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', this.entityPM.CustomFileNo, filterValue);
                
            }
            case 'Customs.Claim': {
                return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', null, null);
            }
        }
    }
   
}

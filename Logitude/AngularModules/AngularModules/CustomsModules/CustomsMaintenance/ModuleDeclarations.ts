import { AddEditRequiredFieldsComponent } from './Components/RequiredFields/AddEditRequiredFieldsComponent';
import { CustomsClosedTablesComponent } from './Components/CustomsClosedTablesComponent';
import { ClosedTableNotExistedComponent } from './Components/ClosedTableNotExistedComponent';
import { CustomsSettingsComponent } from './Components/CustomsSettingsComponent';
import { RequiredFieldsComponent } from './Components/RequiredFields/RequiredFieldsComponent';
import { InterfaceManagementComponent } from './Components/InterfaceManagementComponent';
import { AddEditInterfaceManagementComponent } from './Components/AddEditInterfaceManagementComponent';
import { LoadTestComponent } from './Components/LoadTestComponent';
import { SignStationsComponent } from './Components/SignStationsComponent';
import { CourierSendStatusComponent } from './Components/CourierSendStatusComponent';

import { DocumentTypeCustomsDataComponent } from './Components/DocumentTypeCustomsDataComponent';
import { GeneralLOVComponent } from './Components/GeneralLOVComponent';

import { CustomsDocumentsDefinitionComponent } from './Components/CustomsDocumentsDefinitionComponent';
import { AddEditCustomsAirlineComponent } from './Components/AddEditCustomsAirlineComponent';
import { CustomsPartnerFtpListComponent } from './Components/CustomsPartnerFtpListComponent';
import { DeclarationRemarksComponent } from './Components/DeclarationRemarksComponent'
import { InterfaceTenantPriorityComponent } from './Components/InterfaceTenantPriorityComponent';
import { ExportRequiredFieldsComponent } from './Components/RequiredFields/ExportRequiredFieldsComponent';
import { AddEditExportRequiredFieldsComponent } from './Components/RequiredFields/AddEditExportRequiredFieldsComponent';
import { OcrDefaultsSettingsComponent } from 'Common/Components/Maintenance/OcrDefaultsSettingsComponent';
//import { CustomsPartnerFtpEditComponent } from './Components/CustomsPartnerFtpEditComponent';
 

export const Components =
    [
        AddEditRequiredFieldsComponent,
        AddEditExportRequiredFieldsComponent,
        CustomsClosedTablesComponent,
        ClosedTableNotExistedComponent,
        CustomsSettingsComponent,
        RequiredFieldsComponent,
        ExportRequiredFieldsComponent,
        InterfaceManagementComponent,
        AddEditInterfaceManagementComponent,
        InterfaceTenantPriorityComponent,
        LoadTestComponent,
        SignStationsComponent,
        CourierSendStatusComponent,
        DocumentTypeCustomsDataComponent,
        GeneralLOVComponent,
        CustomsDocumentsDefinitionComponent,
        AddEditCustomsAirlineComponent,
        CustomsPartnerFtpListComponent,
        //CustomsPartnerFtpEditComponent,
        DeclarationRemarksComponent,
        
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddEditRequiredFieldsComponent": { myResult = AddEditRequiredFieldsComponent; break; }
            case "AddEditExportRequiredFieldsComponent": { myResult = AddEditExportRequiredFieldsComponent; break; }
            case "CustomsClosedTablesComponent": { myResult = CustomsClosedTablesComponent; break; }
            case "ClosedTableNotExistedComponent": { myResult = ClosedTableNotExistedComponent; break; }
            case "CustomsSettingsComponent": { myResult = CustomsSettingsComponent; break; }
            case "RequiredFieldsComponent": { myResult = RequiredFieldsComponent; break; }
            case "ExportRequiredFieldsComponent": { myResult = ExportRequiredFieldsComponent; break; }
            case "InterfaceManagementComponent": { myResult = InterfaceManagementComponent; break; }
            case "AddEditInterfaceManagementComponent": { myResult = AddEditInterfaceManagementComponent; break; }
            case "InterfaceTenantPriorityComponent": { myResult = InterfaceTenantPriorityComponent; break; }
                
            case "LoadTestComponent": { myResult = LoadTestComponent; break; }
            case "SignStationsComponent": { myResult = SignStationsComponent; break; }
            case "CourierSendStatusComponent": { myResult = CourierSendStatusComponent; break; }
            case "DocumentTypeCustomsDataComponent": { myResult = DocumentTypeCustomsDataComponent; break; }
            case "GeneralLOVComponent": { myResult = GeneralLOVComponent; break; }
                
            case "CustomsDocumentsDefinitionComponent": { myResult = CustomsDocumentsDefinitionComponent; break; }
            case "AddEditCustomsAirlineComponent": { myResult = AddEditCustomsAirlineComponent; break; }
            case "CustomsPartnerFtpListComponent": { myResult = CustomsPartnerFtpListComponent; break; }
            case "DeclarationRemarksComponent": { myResult = DeclarationRemarksComponent; break; }
            //case "CustomsPartnerFtpEditComponent": { myResult = CustomsPartnerFtpEditComponent; break; }
            case "DeclarationRemarksComponent": { myResult = DeclarationRemarksComponent; break; }
          


        }

        return myResult;
    }
}

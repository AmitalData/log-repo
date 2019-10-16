// General
import {OperationsComponent} from './Components/Workspaces/OperationsComponent';
import {ShipmentsComponent} from './Components/Workspaces/ShipmentsComponent';
import { ContainersFUsComponent } from './Components/Workspaces/ContainersFUsComponent';
import { AMANACComponent } from './Components/Workspaces/AMANACComponent';
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {ShipmentHelperComponent} from './Components/Helpers/ShipmentHelperComponent';
import {ShipmentShortTitleComponent} from './Components/ShortTitles/ShipmentShortTitleComponent';
import {ShipmentFiltersMenuComponent} from './Components/FiltersMenu/ShipmentFiltersMenuComponent';
import {TransportModeListHeaderTemplate} from './Components/ListHeaderTemplates/TransportModeListHeaderTemplate';
import {DirectionListHeaderTemplate} from './Components/ListHeaderTemplates/DirectionListHeaderTemplate';
import {ReactivateShipmentComponent} from './Components/Reactivate/ReactivateShipmentComponent';
import {ReferenceNumberCellDisplayListTemplate} from './Components/ListTemplates/ReferenceNumberCellDisplayListTemplate';
import { StatusCellDisplayListTemplate } from './Components/ListTemplates/StatusCellDisplayListTemplate';
import { TaskCellDisplayListTemplate } from './Components/ListTemplates/TaskCellDisplayListTemplate';
import {DateCellDisplayListTemplate} from './Components/ListTemplates/DateCellDisplayListTemplate';
import {ApprovePaymentButtonListTemplate} from './Components/ListTemplates/ApprovePaymentButtonListTemplate';
import {NewShipmentComponent} from './Components/NewEntity/NewShipmentComponent';
import {NewMasterComponent} from './Components/NewEntity/NewMasterComponent';
import {WizardAddEditAddressComponent} from './Components/NewEntity/WizardAddEditAddressComponent';
import {WizardDimensionsComponent} from './Components/NewEntity/WizardDimensionsComponent';
import {WizardAddEditDimensionsComponent} from './Components/NewEntity/WizardAddEditDimensionsComponent';
import {ActionButtonsListTemplate} from './Components/ListTemplates/ActionButtonsListTemplate';
import {CustomReferenceListTemplate} from './Components/ListTemplates/CustomReferenceListTemplate';
import {EditShipmentButtonListTemplate} from './Components/ListTemplates/EditShipmentButtonListTemplate';
import {ConnectButtonsListTemplate} from './Components/ListTemplates/ConnectButtonsListTemplate';
import {ArchiveListTemplate} from './Components/ListTemplates/ArchiveListTemplate';
import {RequestedDocumentsCountListTemplate} from './Components/ListTemplates/RequestedDocumentsCountListTemplate';
import {DocumentSearchResultListTemplate} from './Components/ListTemplates/DocumentSearchResultListTemplate';
import {ActionValidationComponent} from './Components/ActionValidationComponent/ActionValidationComponent';
import {MenuButtonsTemplateComponent} from './Components/MenuButtons/MenuButtonsTemplateComponent';
import {MasterActionConfirmationComponent} from './Components/MenuButtons/MasterActionConfirmationComponent';
import {ShipmentSpotlightComponent} from './Components/Spotlight/ShipmentSpotlightComponent';
import {SpotLightDateComponent} from './Components/Spotlight/SpotLightDateComponent';
import {SplitShipmentComponent} from './Components/SplitShipment/SplitShipmentComponent';
import { SplitPartialPackageComponent } from './Components/SplitShipment/SplitPartialPackageComponent';
import { RemoveTasksButtonListTemplate } from './Components/ListTemplates/RemoveTasksButtonListTemplate';
import { AnalyzeChampXMLComponent } from './Components/Helpers/AnalyzeChampXMLComponent';
import { ShipmenDirectionConvertComponent } from './Components/MenuButtons/ShipmenDirectionConvertComponent';
import { CustomsTransferHeaderHelperComponent } from './Components/Helpers/CustomsTransferHeaderHelperComponent';

export const Components =
    [
        OperationsComponent,
        ShipmentsComponent,
        ContainersFUsComponent,
        AMANACComponent,
        FieldTemplateComponent,
        ShipmentHelperComponent,
        ShipmentShortTitleComponent,
        ShipmentFiltersMenuComponent,
        TransportModeListHeaderTemplate,
        DirectionListHeaderTemplate,
        ReactivateShipmentComponent,
        ReferenceNumberCellDisplayListTemplate,
        StatusCellDisplayListTemplate,
        TaskCellDisplayListTemplate,
        NewShipmentComponent,
        NewMasterComponent,
        WizardAddEditAddressComponent,
        WizardDimensionsComponent,
        WizardAddEditDimensionsComponent,
        DateCellDisplayListTemplate,
        ActionButtonsListTemplate,
        ConnectButtonsListTemplate,
        ArchiveListTemplate,
        RequestedDocumentsCountListTemplate,
        DocumentSearchResultListTemplate,
        ActionValidationComponent,
        MenuButtonsTemplateComponent,
        MasterActionConfirmationComponent,
        ShipmentSpotlightComponent,
        ApprovePaymentButtonListTemplate,
        EditShipmentButtonListTemplate,
        CustomReferenceListTemplate,
        SplitShipmentComponent,
        SplitPartialPackageComponent,
        RemoveTasksButtonListTemplate,
        AnalyzeChampXMLComponent,
        ShipmenDirectionConvertComponent,
        CustomsTransferHeaderHelperComponent,
    ];

export const ControlsComponents =
    [
        SpotLightDateComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "OperationsComponent": { myResult = OperationsComponent; break; }
            case "ShipmentsComponent": { myResult = ShipmentsComponent; break; }
            case "ContainersFUsComponent": { myResult = ContainersFUsComponent; break; }
            case "AMANACComponent": { myResult = AMANACComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "ShipmentHelperComponent": { myResult = ShipmentHelperComponent; break; }
            case "ShipmentShortTitleComponent": { myResult = ShipmentShortTitleComponent; break; }
            case "ShipmentFiltersMenuComponent": { myResult = ShipmentFiltersMenuComponent; break; }
            case "TransportModeListHeaderTemplate": { myResult = TransportModeListHeaderTemplate; break; }
            case "DirectionListHeaderTemplate": { myResult = DirectionListHeaderTemplate; break; }
            case "ReactivateShipmentComponent": { myResult = ReactivateShipmentComponent; break; }
            case "ReferenceNumberCellDisplayListTemplate": { myResult = ReferenceNumberCellDisplayListTemplate; break; }
            case "NewShipmentComponent": { myResult = NewShipmentComponent; break; }
            case "NewMasterComponent": { myResult = NewMasterComponent; break; }
            case "WizardAddEditAddressComponent": { myResult = WizardAddEditAddressComponent; break; }
            case "WizardDimensionsComponent": { myResult = WizardDimensionsComponent; break; }
            case "WizardAddEditDimensionsComponent": { myResult = WizardAddEditDimensionsComponent; break; }
            case "StatusCellDisplayListTemplate": { myResult = StatusCellDisplayListTemplate; break; }
            case "TaskCellDisplayListTemplate": { myResult = TaskCellDisplayListTemplate; break; }
            case "DateCellDisplayListTemplate": { myResult = DateCellDisplayListTemplate; break; }
            case "ActionButtonsListTemplate": { myResult = ActionButtonsListTemplate; break; }
            case "ConnectButtonsListTemplate": { myResult = ConnectButtonsListTemplate; break; }
            case "ArchiveListTemplate": { myResult = ArchiveListTemplate; break; }
            case "RequestedDocumentsCountListTemplate": { myResult = RequestedDocumentsCountListTemplate; break; }
            case "DocumentSearchResultListTemplate": { myResult = DocumentSearchResultListTemplate; break; }
            case "ActionValidationComponent": { myResult = ActionValidationComponent; break; }
            case "MenuButtonsTemplateComponent": { myResult = MenuButtonsTemplateComponent; break; }
            case "MasterActionConfirmationComponent": { myResult = MasterActionConfirmationComponent; break; }
            case "ShipmentSpotlightComponent": { myResult = ShipmentSpotlightComponent; break; }
            case "ApprovePaymentButtonListTemplate": { myResult = ApprovePaymentButtonListTemplate; break; }
            case "EditShipmentButtonListTemplate": { myResult = EditShipmentButtonListTemplate; break; }
            case "CustomReferenceListTemplate": { myResult = CustomReferenceListTemplate; break; }
            case "SpotLightDateComponent": { myResult = SpotLightDateComponent; break; }
            case "SplitShipmentComponent": { myResult = SplitShipmentComponent; break; }
            case "SplitPartialPackageComponent": { myResult = SplitPartialPackageComponent; break; }
            case "RemoveTasksButtonListTemplate": { myResult = RemoveTasksButtonListTemplate; break; }
            case "AnalyzeChampXMLComponent": { myResult = AnalyzeChampXMLComponent; break; }
            case "ShipmenDirectionConvertComponent": { myResult = ShipmenDirectionConvertComponent; break; }
            case "CustomsTransferHeaderHelperComponent": { myResult = CustomsTransferHeaderHelperComponent; break; }
        }

        return myResult;
    }
}

import { WorkflowBuilderComponent } from "./Components/WorkflowBuilder/WorkflowBuilderComponent";
import { StartPropertiesComponent } from "./Components/Properties/StartPropertiesComponent";
import { StartEventTriggeredPropertiesComponent } from "./Components/Properties/StartEventTriggeredPropertiesComponent";
import { ConditionPropertiesComponent } from "./Components/Properties/ConditionPropertiesComponent";
import { LoopPropertiesComponent } from "./Components/Properties/LoopPropertiesComponent";
import { SetValuePropertiesComponent } from "./Components/Properties/SetValuePropertiesComponent";
import { DeclareVariablePropertiesComponent } from "./Components/Properties/DeclareVariablePropertiesComponent";
import { CreateRecordPropertiesComponent } from "./Components/Properties/CreateRecordPropertiesComponent";
import { UpdateRecordPropertiesComponent } from "./Components/Properties/UpdateRecordPropertiesComponent";
import { GetRecordPropertiesComponent } from "./Components/Properties/GetRecordPropertiesComponent";
import { SendEmailPropertiesComponent } from "./Components/Properties/SendEmailPropertiesComponent";
import { CollectionFilterPropertiesComponent } from "./Components/Properties/CollectionFilterPropertiesComponent";
import { AppendItemPropertiesComponent } from "./Components/Properties/AppendItemPropertiesComponent";
import { DeleteItemPropertiesComponent } from "./Components/Properties/DeleteItemPropertiesComponent";
import { CreateTaskPropertiesComponent } from "./Components/Properties/CreateTaskPropertiesComponent";
import { FieldTemplateComponent } from "./Components/Templates/FieldTemplateComponent";
import { CreateWorkflowComponent } from "./Components/CreateEditWorkflow/CreateWorkflowComponent";
import { EditWorkflowComponent } from "./Components/CreateEditWorkflow/EditWorkflowComponent";
import { CreateEditWorkflowComponent } from "./Components/CreateEditWorkflow/CreateEditWorkflowComponent";
import { RunHistoryWorkflowComponent } from "./Components/WorkflowInstance/RunHistoryWorkflowComponent";
import { WorkflowInstanceDetailsComponent } from "./Components/WorkflowInstance/WorkflowInstanceDetailsComponent";
import { ConditionsComponent } from "./Components/Base/ConditionsComponent";
import { ConditionGroupsComponent } from "./Components/Base/ConditionGroupsComponent";
import { FieldValueComponent } from "./Components/Base/FieldValueComponent";
import { FooterButtonsComponent } from "./Components/Base/FooterButtonsComponent";
import { TreeSelectComponent } from "./Components/Base/TreeSelectComponent";
import { SetValuesComponent } from "./Components/Base/SetValuesComponent";
import { DeleteNodeWarningComponent } from "./Components/Messages/DeleteNodeWarningComponent";
import { ObjectFieldPipe } from "./Pipes/ObjectFieldPipe";
import { ListItemPipe } from "./Pipes/ListItemPipe";
import { IsDateTimeTypePipe } from "./Pipes/IsDateTimeTypePipe";
import { IsNoObjectFieldVariablePipe } from "./Pipes/IsNoObjectFieldVariablePipe";
import { IsFieldOperatorPipe } from "./Pipes/IsFieldOperatorPipe";
import { IsNoValueOperatorPipe } from "./Pipes/IsNoValueOperatorPipe";
import { ShowFlowVariablesTreeItemPipe } from "./Pipes/ShowFlowVariablesTreeItemPipe";
import { ShowFlowVariablesTypeTreeItemPipe } from "./Pipes/ShowFlowVariablesTypeTreeItemPipe";
import { ShowEntitiesTreeItemPipe } from "./Pipes/ShowEntitiesTreeItemPipe";
import { ShowExpressionTreeItemPipe } from "./Pipes/ShowExpressionTreeItemPipe";
import { EntityLabelPipe } from "./Pipes/EntityLabelPipe";
import { ConditionDisabledPipe } from "./Pipes/ConditionDisabledPipe";
import { SetValueDisabledPipe } from "./Pipes/SetValueDisabledPipe";
import { ExpressionComponent } from "./Components/Base/ExpressionComponent";
import { ExpressionBuilderComponent } from "./Components/Base/ExpressionBuilderComponent";
import { ShowEditableVariablesTreeItemPipe } from "./Pipes/ShowEditableVariablesTreeItemPipe";
import { IsCollectionTypePipe } from "./Pipes/IsCollectionTypePipe";
import { SetValuesOperatorsItemsPipe } from "./Pipes/SetValuesOperatorsItemsPipe";
import { ShowTreeItemPipe } from "./Pipes/ShowTreeItemPipe";
import { ExpressionPipe } from "./Pipes/ExpressionPipe";
import { ConditionOperatorsItemsPipe } from "./Pipes/ConditionOperatorsItemsPipe";
import { WorkflowVersionComponent } from "./Components/WorkflowVersion/WorkflowVersionComponent";
import { CreateWorkflowVersionComponent } from "./Components/WorkflowVersion/CreateWorkflowVersionComponent";
import { WorkFlowShortTitleComponent } from "./Components/ShortTitles/WorkFlowShortTitleComponent";
import { IsObjectTypePipe } from "./Pipes/IsObjectTypePipe";
import { ObjectVariableComponent } from "./Components/Base/ObjectVariableComponent";
import { WorkFlowHelperComponent } from "./Components/Helpers/WorkFlowHelperComponent";
import { EditWorkflowVersionComponent } from "./Components/WorkflowVersion/EditWorkflowVersionComponent";
import { GetFieldApiQueryFiltersPipe } from "./Pipes/GetFieldApiQueryFiltersPipe ";

export const Components = [
    WorkflowBuilderComponent,
    StartPropertiesComponent,
    StartEventTriggeredPropertiesComponent,
    ConditionPropertiesComponent,
    LoopPropertiesComponent,
    SetValuePropertiesComponent,
    DeclareVariablePropertiesComponent,
    CreateRecordPropertiesComponent,
    UpdateRecordPropertiesComponent,
    GetRecordPropertiesComponent,
    SendEmailPropertiesComponent,
    CollectionFilterPropertiesComponent,
    AppendItemPropertiesComponent,
    DeleteItemPropertiesComponent,
    CreateTaskPropertiesComponent,
    FieldTemplateComponent,
    CreateWorkflowComponent,
    EditWorkflowComponent,
    CreateEditWorkflowComponent,
    RunHistoryWorkflowComponent,
    ConditionsComponent,
    ConditionGroupsComponent,
    FieldValueComponent,
    FooterButtonsComponent,
    WorkflowInstanceDetailsComponent,
    TreeSelectComponent,
    SetValuesComponent,
    DeleteNodeWarningComponent,
    ExpressionComponent,
    ExpressionBuilderComponent,
    WorkflowVersionComponent,
    CreateWorkflowVersionComponent,
    EditWorkflowVersionComponent,
    WorkFlowShortTitleComponent,
    ObjectVariableComponent,
    WorkFlowHelperComponent,
];

export const Pipes = [
    ObjectFieldPipe,
    ListItemPipe,
    IsDateTimeTypePipe,
    IsNoObjectFieldVariablePipe,
    IsFieldOperatorPipe,
    IsNoValueOperatorPipe,
    ShowEntitiesTreeItemPipe,
    ShowExpressionTreeItemPipe,
    ShowFlowVariablesTreeItemPipe,
    ShowFlowVariablesTypeTreeItemPipe,
    EntityLabelPipe,
    ConditionDisabledPipe,
    SetValueDisabledPipe,
    ShowEditableVariablesTreeItemPipe,
    IsObjectTypePipe,
    IsCollectionTypePipe,
    SetValuesOperatorsItemsPipe,
    ShowTreeItemPipe,
    ExpressionPipe,
    ConditionOperatorsItemsPipe,
    GetFieldApiQueryFiltersPipe
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var result: any = null;
        switch (name) {
            case "WorkflowBuilderComponent": { result = WorkflowBuilderComponent; break; }
            case "StartPropertiesComponent": { result = StartPropertiesComponent; break; }
            case "StartEventTriggeredPropertiesComponent": { result = StartEventTriggeredPropertiesComponent; break; }
            case "ConditionPropertiesComponent": { result = ConditionPropertiesComponent; break; }
            case "LoopPropertiesComponent": { result = LoopPropertiesComponent; break; }
            case "SetValuePropertiesComponent": { result = SetValuePropertiesComponent; break; }
            case "DeclareVariablePropertiesComponent": { result = DeclareVariablePropertiesComponent; break; }
            case "CreateRecordPropertiesComponent": { result = CreateRecordPropertiesComponent; break; }
            case "UpdateRecordPropertiesComponent": { result = UpdateRecordPropertiesComponent; break; }
            case "GetRecordPropertiesComponent": { result = GetRecordPropertiesComponent; break; }
            case "SendEmailPropertiesComponent": { result = SendEmailPropertiesComponent; break; }
            case "CollectionFilterPropertiesComponent": { result = CollectionFilterPropertiesComponent; break; }
            case "AppendItemPropertiesComponent": { result = AppendItemPropertiesComponent; break; }
            case "DeleteItemPropertiesComponent": { result = DeleteItemPropertiesComponent; break; }
            case "CreateTaskPropertiesComponent": { result = CreateTaskPropertiesComponent; break; }
            case "FieldTemplateComponent": { result = FieldTemplateComponent; break; }
            case "CreateWorkflowComponent": { result = CreateWorkflowComponent; break; }
            case "EditWorkflowComponent": { result = EditWorkflowComponent; break; }
            case "CreateEditWorkflowComponent": { result = CreateEditWorkflowComponent; break; }
            case "RunHistoryWorkflowComponent": { result = RunHistoryWorkflowComponent; break; }
            case "ConditionsComponent": { result = ConditionsComponent; break; }
            case "ConditionGroupsComponent": { result = ConditionGroupsComponent; break; }
            case "FieldValueComponent": { result = FieldValueComponent; break; }
            case "FooterButtonsComponent": { result = FooterButtonsComponent; break; }
            case "WorkflowInstanceDetailsComponent": { result = WorkflowInstanceDetailsComponent; break; }
            case "TreeSelectComponent": { result = TreeSelectComponent; break; }
            case "SetValuesComponent": { result = SetValuesComponent; break; }
            case "DeleteNodeWarningComponent": { result = DeleteNodeWarningComponent; break; }
            case "ExpressionComponent": { result = ExpressionComponent; break; }
            case "ExpressionBuilderComponent": { result = ExpressionBuilderComponent; break; }
            case "WorkflowVersionComponent": { result = WorkflowVersionComponent; break; }
            case "CreateWorkflowVersionComponent": { result = CreateWorkflowVersionComponent; break; }
            case "EditWorkflowVersionComponent": { result = EditWorkflowVersionComponent; break; }
            case "WorkFlowShortTitleComponent": { result = WorkFlowShortTitleComponent; break; }
            case "ObjectVariableComponent": { result = ObjectVariableComponent; break; }
            case "WorkFlowHelperComponent": { result = WorkFlowHelperComponent; break; }
        }
        return result;
    }
}

import {CommunicationLogMessageBodyComponent} from './Components/CommunicationLog/CommunicationLogMessageBodyComponent';
import {CommunicationLogErrorComponent} from './Components/CommunicationLog/CommunicationLogErrorComponent';
import { CommunicationStepsComponent } from './Components/Communications/CommunicationStepsComponent';
import { CommunicationLogMoreDetailsComponent } from './Components/Communications/CommunicationLogMoreDetailsComponent';
import { CommunicationMoreComponent } from './Components/Communications/CommunicationMoreComponent';
import { LogFieldComponent } from './Components/Communications/LogFieldComponent';
import {APILogsErrorsComponent} from './Components/APILogs/APILogsErrorsComponent';
import {APILogsDiagnosticComponent} from './Components/APILogs/APILogsDiagnosticComponent';
import {APILogsResponceBodyComponent} from './Components/APILogs/APILogsResponceBodyComponent';
import {APILogsRequestBodyComponent} from './Components/APILogs/APILogsRequestBodyComponent';
import {CommunicationsTabComponent} from './Components/Communications/CommunicationsTabComponent';
import {MessageBodyTabComponent} from './Components/AnalyzeQueue/MessageBodyTabComponent';
import {AnalyzeQueueErrorsTabComponent} from './Components/AnalyzeQueue/AnalyzeQueueErrorsTabComponent';

export const Components =
    [
        CommunicationLogMessageBodyComponent,
        CommunicationLogErrorComponent,
        CommunicationLogMoreDetailsComponent,
        CommunicationStepsComponent,
        CommunicationMoreComponent,
        LogFieldComponent,
        APILogsDiagnosticComponent,
        APILogsErrorsComponent,
        APILogsRequestBodyComponent,
        APILogsResponceBodyComponent,
        CommunicationsTabComponent,
        MessageBodyTabComponent,
        AnalyzeQueueErrorsTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CommunicationLogMessageBodyComponent": { myResult = CommunicationLogMessageBodyComponent; break; }
            case "CommunicationLogErrorComponent": { myResult = CommunicationLogErrorComponent; break; }
            case "CommunicationLogMoreDetailsComponent": { myResult = CommunicationLogMoreDetailsComponent; break; }
            case "CommunicationStepsComponent": { myResult = CommunicationStepsComponent; break; }
            case "CommunicationMoreComponent": { myResult = CommunicationMoreComponent; break; }
            case "LogFieldComponent": { myResult = LogFieldComponent; break; }
            case "APILogsErrorsComponent": { myResult = APILogsErrorsComponent; break; }
            case "APILogsDiagnosticComponent": { myResult = APILogsDiagnosticComponent; break; }
            case "APILogsRequestBodyComponent": { myResult = APILogsRequestBodyComponent; break; }
            case "APILogsResponceBodyComponent": { myResult = APILogsResponceBodyComponent; break; }
            case "CommunicationsTabComponent": { myResult = CommunicationsTabComponent; break; }
            case "AnalyzeQueueErrorsTabComponent": { myResult = AnalyzeQueueErrorsTabComponent; break; }
            case "MessageBodyTabComponent": { myResult = MessageBodyTabComponent; break; }
        }

        return myResult;
    }
}
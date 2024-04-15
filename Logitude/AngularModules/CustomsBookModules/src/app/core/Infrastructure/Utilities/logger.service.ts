import { ErrorLogPM } from "Infrastructure/EntityPMs/ErrorLogPM";
import { ErrorLogPMFileLoggerService } from "Infrastructure/Services/ExtendedPMs/ErrorLogPMFileLoggerService";
import { DateTool } from "Infrastructure/Tools";
import { SessionLocator } from "./SessionLocator";

export class loggerService {
    loggerAppSettingsName: string = '';
    errorLogPMFileLoggerService: ErrorLogPMFileLoggerService = new ErrorLogPMFileLoggerService();
    
    
    constructor() {}


    sendError(exception: string, stack: string = '', loggerAppSettingsName: string = this.loggerAppSettingsName) {
        if(!loggerAppSettingsName)
            return console.log('log not send, dont have loggerAppSettingsName value')


        const errorLogPM: ErrorLogPM = new ErrorLogPM();
        errorLogPM.Id = loggerAppSettingsName;
        errorLogPM.StackTrace = stack;
        errorLogPM.Tenant = SessionLocator.Tenant;
        errorLogPM.UserName = SessionLocator.LoggedUserId + "/" + SessionLocator.LoggedUserPM.Email;
        errorLogPM.LogDate = DateTool.GetCurrentDateTimeAsUtc();
        errorLogPM.Exception = exception;

        this.errorLogPMFileLoggerService.insert(errorLogPM).subscribe();
    }
}
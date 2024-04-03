import { ErrorHandler, Injectable } from "@angular/core";
import { LogitudeMonitoringService } from "./logging.service";

@Injectable()
export class ErrorHandlerService extends ErrorHandler {

    constructor(private logitudeMonitoringService: LogitudeMonitoringService) {
        super();
    }

    handleError(error: Error) {
        this.logitudeMonitoringService.logException(error); // Manually log exception
    }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { ObjectTableTabPM } from 'Infrastructure/EntityPMs/ObjectTableTabPM';

@Injectable()
export class TableTabService
{
    private http: HttpClient;
    private apiUrl: string;
    constructor()
    {
        this.http = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TableTabs';
    }

    UpdateTabs(tabs: ObjectTableTabPM[])
    {
        return defer(() =>
        {
            return this.http.post(this.apiUrl, JSON.stringify(tabs), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: any) =>
                    {
                        return response.body;
                    }),

                    catchError(ServiceHelper.HandleServiceError));

        });
    }
}

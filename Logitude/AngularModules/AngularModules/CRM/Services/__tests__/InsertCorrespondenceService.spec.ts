import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';

jest.mock('../../../Infrastructure/Components/LogitudeComponents/UIProperties', () => {
    return {
        UIProperties: class {
            constructor() {}
        }
    };
});

import { InsertCorrespondenceService } from '../InsertCorrespondenceService';

describe('InsertCorrespondenceService', () => {
    const baseUrl = 'https://crm/';
    let httpClient: { post: jest.Mock };
    let validateSpy: jest.SpyInstance;

    beforeEach(() => {
        httpClient = {
            post: jest.fn()
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        validateSpy = jest
            .spyOn(ClassLevelValidator.prototype, 'Validate')
            .mockReturnValue([]);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('insert posts mapped correspondence when validation succeeds', done => {
        const service = new InsertCorrespondenceService();
        const entity: any = { Subject: 'Hello' };
        httpClient.post.mockReturnValue(of({ Id: 'CORR-1', Subject: 'Server Subject' }));

        service.insert(entity).subscribe(res => {
            expect(validateSpy).toHaveBeenCalledWith('Correspondence', entity);
            const [url, body, headers] = httpClient.post.mock.calls[0];
            expect(url).toBe(`${baseUrl}api/InsertCorrespondence`);
            expect(typeof body).toBe('string');
            expect(JSON.parse(body).Subject).toBe('Hello');
            expect(headers).toEqual({ headers: { Authorization: 'token' } });
            expect(res.Result).toBe(entity);
            expect(entity.Id).toBe('CORR-1');
            expect(entity.Subject).toBe('Server Subject');
            expect(entity.OldEntityPM).toBeDefined();
            done();
        });
    });

    it('insert returns validation errors without calling http when validation fails', done => {
        validateSpy.mockReturnValue(['Subject required']);
        const service = new InsertCorrespondenceService();
        const entity: any = { Subject: '' };

        service.insert(entity).subscribe(res => {
            expect(httpClient.post).not.toHaveBeenCalled();
            expect(res.HasError).toBe(true);
            expect(res.ErrorsArray).toEqual(['Subject required']);
            done();
        });
    });

    it('MapJsonToEntityPM handles existing entity and map flag false', () => {
        const service = new InsertCorrespondenceService();
        const existing: any = { Id: 'EXIST', OldEntityPM: {} };

        const mapped = service.MapJsonToEntityPM({ Id: 'UPDATED', Subject: 'Updated' }, false, existing) as any;

        expect(mapped.Id).toBe('UPDATED');
        expect(mapped.Subject).toBe('Updated');
        expect(mapped.OldEntityPM).toBeNull();
    });

    it('clone filters UI-related fields', () => {
        const service = new InsertCorrespondenceService();
        const cloned = service.clone({
            Id: 'ID-1',
            UIProperties: {},
            entityParentPM: {},
            OldEntityPM: {},
            Value: 'keep'
        });
        expect(cloned).toEqual({ Id: 'ID-1', Value: 'keep' });
    });
});


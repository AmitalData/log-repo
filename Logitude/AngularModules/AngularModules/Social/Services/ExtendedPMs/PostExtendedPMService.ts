
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {PostPM} from '../../EntityPMs/PostPM';

import {PostLikePM} from '../../EntityPMs/PostLikePM';


@Injectable()

export class PostExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PostExtended';
    }


    GetSocialContact(id: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSocialContact/?' + 'id=' + id +'&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
         
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }




    GetPostSummaryData(userId: string, loggedUserId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetPostSummaryData/?' + 'userId=' + userId + '&loggedUserId=' + loggedUserId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    DeletePostLike( postId:string ,userId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDeletePostLike/?' + 'postId=' + postId + '&userId=' + userId+  '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    InsertPostLike(postLike: PostLikePM) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return this._http.post(this._apiUrl + '/PostInsertPostLike', JSON.stringify(postLike),
                { headers: authHeader }).map((response) => {

                    var result = response.json();
                    var entity: PostLikePM;
                    entity = result;
                    if (result) {
                        entity = this.MapPostLikes2(result);
                    }
               
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();

                    pmresponse.Result = entity;
                    return pmresponse;

                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }
    
    



    
    GetSinglePostComment(id: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSinglePostComment/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: PostPM;
            var PostPMLists: PostPM[];
            PostPMLists = new Array<PostPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                PostPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = PostPMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


    PostFilteredPosts(postFilter: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');



            return this._http.post(this._apiUrl + '/PostFilteredPosts', JSON.stringify(postFilter),
                { headers: authHeader }).map((response) => {

                    var result = response.json();
                    var entity: PostPM;
                    var postPMLists: PostPM[];
                    postPMLists = new Array<PostPM>();
                    result.forEach((item) => {
                        entity = this.MapJsonToEntityPM(item);
                        postPMLists.push(entity);
                    });
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();

                    pmresponse.Result = postPMLists;
                    return pmresponse;

                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


    GetCountPostPMsByFilter(postFilter: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            return this._http.post(this._apiUrl + '/PostGetCountPostPMsByFilter', JSON.stringify(postFilter),
                { headers: authHeader }).map((response) => {

                    var result = response.json();
            
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();

                    pmresponse.Result = result;
                    return pmresponse;

                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

    




    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: PostPM = null) {


        if (!entityPM) {

            entityPM = new PostPM();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }

        this.MapPostComments(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapPostLikes(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.PostComments = [];
            for (var item in entityPM.PostComments) {
                var myPostPM = entityPM.PostComments[item];
                var newPostPM: PostPM = this.clone(myPostPM);

                newPostPM.PostLikes = [];
                for (var k in myPostPM.PostLikes) {
                    var myPostLikePM = myPostPM.PostLikes[k];
                    var newPostLikePM = this.clone(myPostPM.PostLikes[k]);
                    newPostPM.PostLikes.push(newPostLikePM);

                }

                entityPM.OldEntityPM.PostComments.push(newPostPM);
            }

            entityPM.OldEntityPM.PostLikes = [];
            for (var item in entityPM.PostLikes) {
                var myPostLikePM = entityPM.PostLikes[item];
                var newPostLikePM2: PostLikePM = this.clone(myPostLikePM);


                entityPM.OldEntityPM.PostLikes.push(newPostLikePM2);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapPostComments(entityPM: PostPM, jsonPM: any, mapParent: boolean = true) {

        entityPM.PostComments = new Array<PostPM>();
        for (var item in jsonPM.PostComments) {

            var jItem = jsonPM.PostComments[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newPostPM: PostPM;
            newPostPM = new PostPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newPostPM[pmProperty] = jItem[pmProperty];
            }
            newPostPM.IsDirty = false;
            entityPM.PostComments.push(newPostPM);
        }
    }
    MapPostLikes(entityPM: PostPM, jsonPM: any, mapParent: boolean = true) {

        var oldPostLikes: PostLikePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPostLikes = entityPM.OldEntityPM.PostLikes;
        }

        entityPM.PostLikes = new Array<PostLikePM>();
        for (var item in jsonPM.PostLikes) {
            var jItem = jsonPM.PostLikes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newPostLikePM: PostLikePM;

            if (mapParent) {
                newPostLikePM = new PostLikePM(entityPM);
            }
            else {
                newPostLikePM = new PostLikePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newPostLikePM[pmProperty] = jItem[pmProperty];
            }
            newPostLikePM.IsDirty = false;

            if (mapParent) {
                newPostLikePM.UniqueKey = Guid.newGuid();
                newPostLikePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newPostLikePM.OldEntityPM = this.clone(newPostLikePM);


            }
            else {
                if (newPostLikePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newPostLikePM.ChangeSetOp = "Update";
                }
                else {
                    newPostLikePM.ChangeSetOp = "Insert";
                }

                newPostLikePM.OldEntityPM = null;
                newPostLikePM.EntityParentPM = null;
            }


            entityPM.PostLikes.push(newPostLikePM);
        }
        if (oldPostLikes) {

            for (var itemKey in oldPostLikes) {
                if (entityPM.PostLikes.filter(p => p.UniqueKey === oldPostLikes[itemKey].UniqueKey).length === 0) {

                    if (oldPostLikes[itemKey]) {
                        //oldPostLikes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.PostLikes.push(oldPostLikes[itemKey]);
                        var oldItemJson = oldPostLikes[itemKey];
                        var deletedPM: PostLikePM = new PostLikePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.PostLikes.push(deletedPM);
                    }
                }
            }
        }
    }


    MapPostLikes2(jsonPM: any) {

        var entityPM: PostLikePM;
        entityPM = new PostLikePM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }




}

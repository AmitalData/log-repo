"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TicketStageListService_1 = require("../Services/StandardLists/TicketStageListService");
var TicketSourceListService_1 = require("../Services/StandardLists/TicketSourceListService");
var TicketCreatedByTypeListService_1 = require("../Services/StandardLists/TicketCreatedByTypeListService");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var TicketPMInitService = /** @class */ (function () {
    function TicketPMInitService() {
    }
    TicketPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            this.GetTicketStageMethod(entityPM);
            this.GetTicketSourceMethod(entityPM);
            this.GetTicketCreatedByTypeMethod(entityPM);
        }
    };
    TicketPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    // Ticket Stages
    TicketPMInitService.GetTicketStageMethod = function (entityPM) {
        var myService = new TicketStageListService_1.TicketStageListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var list = resp.Result;
                var stage = list.filter(function (d) { return d.Code == "OP" && d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
                if (stage != null) {
                    entityPM.StageId = stage.Id;
                }
            }
        });
    };
    // Ticket Source
    TicketPMInitService.GetTicketSourceMethod = function (entityPM) {
        var myService = new TicketSourceListService_1.TicketSourceListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var list = resp.Result;
                var source = list.filter(function (d) { return d.Code == "LOG"; })[0];
                if (source != null) {
                    entityPM.Source = source.Code;
                }
            }
        });
    };
    //Ticket CreatedByType List
    TicketPMInitService.GetTicketCreatedByTypeMethod = function (entityPM) {
        var myService = new TicketCreatedByTypeListService_1.TicketCreatedByTypeListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var list = resp.Result;
                var type = list.filter(function (d) { return d.Code == "INU"; })[0];
                if (type != null) {
                    entityPM.CreatedbyType = type.Code;
                }
            }
        });
    };
    return TicketPMInitService;
}());
exports.TicketPMInitService = TicketPMInitService;
//# sourceMappingURL=TicketPMInitService.js.map
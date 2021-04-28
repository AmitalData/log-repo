export var DynamicLoaderTSC = (function () {
    function DynamicLoaderTSC() {
    }
    DynamicLoaderTSC.Load = function (myComponentPath, location) {
        var _this = this;
        if (myComponentPath != null) {
            var stringParts = myComponentPath.split("/");
            var myModuleName_1 = "Logitude" + stringParts[1] + "Module";
            var myModulePath_1 = "./" + stringParts[1] + "/" + myModuleName_1;
            var myComponentName_1 = null;
            if (myComponentPath != null) {
                var urlParts = myComponentPath.split("/");
                myComponentName_1 = urlParts[urlParts.length - 1];
            }
            return new Promise(function (resolve) {
                window.System.import(myModulePath_1)
                    .then(function (m) { return m[myModuleName_1]; })
                    .then(function (type) {
                    return _this.Compiler.compileModuleAndAllComponentsAsync(type);
                })
                    .then(function (moduleWithFactories) {
                    var factory = moduleWithFactories.componentFactories.find(function (x) { return x.componentType.name === myComponentName_1; });
                    if (factory) {
                        var componentRef = location.createComponent(factory);
                        resolve(componentRef);
                    }
                    else {
                        alert("(TSC) " + myComponentName_1 + " is not declared in " + myModuleName_1);
                    }
                });
            });
        }
    };
    DynamicLoaderTSC.GetInstance = function (myPath) {
        if (myPath != null) {
            myPath = myPath.replace("CommunicationStatusTypesList", "CommunicationStatusTypeList");
            var urlParts = myPath.split("/");
            var myModuleName_2 = "Logitude" + urlParts[1] + "Module";
            var instanceName_1 = urlParts[urlParts.length - 1];
            return new Promise(function (resolve, reject) {
                window.System.import(myPath)
                    .then(function (m) {
                    var instanceType = m[instanceName_1];
                    if (instanceType) {
                        var instance = Object.create(instanceType.prototype);
                        instance.constructor.apply(instance);
                        resolve(instance);
                    }
                    else {
                        alert("(TSC) " + instanceName_1 + " is not declared in " + myModuleName_2);
                    }
                });
            });
        }
        else {
            alert("Instance Path is null");
        }
    };
    return DynamicLoaderTSC;
}());
//# sourceMappingURL=DynamicLoaderTSC.js.map
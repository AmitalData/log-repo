var ng2 = require('angular2/core');
var wijmo_angular2_MetaFactory_1 = require('./wijmo.angular2.MetaFactory');
var wj;
(function (wj) {
    var angular2;
    (function (angular2) {
        'use strict';
        angular2.WjComponent = function (options) {
            return function (target, targetKey) {
                //if (options.wjInit) {
                //    options.wjInit();
                //}
                var metaId = options.wjMetadataId;
                if (metaId) {
                    delete options.wjMetadataId;
                }
                else {
                    // Use base class reference
                    metaId = Object.getPrototypeOf(target.prototype).constructor;
                }
                var metaData = wijmo_angular2_MetaFactory_1.wjNg2Meta.MetaFactory.getMetaData(metaId);
                options.inputs = (options.inputs || [WjDirectiveBehavior.parPropAttr]).concat(metaData.props.map(function (propDesc) { return propDesc.propertyName; }));
                var changeEventsMap = Ng2Utils.getChangeEventMap(metaData);
                options.outputs = (options.outputs || []).concat(Ng2Utils.initEvents(target, changeEventsMap));
                target[Ng2Utils.directiveTypeDataProp] = new DirectiveTypeData(metaData, changeEventsMap, options.wjParentDirectives, options.wjSiblingDirectiveId);
                var retNg = options.wjIsDirective ? ng2.Directive(options) : ng2.Component(options);
                var ret = retNg(target, targetKey);
                new MethodProxy(target, 'ngOnInit', WjDirectiveBehavior.prototype.dirOnInit);
                new MethodProxy(target, 'ngOnDestroy', WjDirectiveBehavior.prototype.dirOnDestroy);
                new MethodProxy(target, 'ngOnChanges', WjDirectiveBehavior.prototype.dirOnChanges);
                return ret;
            };
        };
        var DirectiveTypeData = (function () {
            function DirectiveTypeData(metaData, changeEventMap, parentDirectives, siblingId) {
                this._fwdResolved = false;
                this.metaData = metaData;
                this.changeEventMap = changeEventMap;
                if (parentDirectives) {
                    this.parentDirectives = [].concat(parentDirectives);
                }
                this.siblingId = siblingId || (++DirectiveTypeData._siblingIdCounter).toString();
            }
            // called by behavior
            DirectiveTypeData.prototype.resolveForwardDeclarations = function () {
                if (!this._fwdResolved) {
                    this._fwdResolved = true;
                    WjDirectiveBehavior.resolveForwardDecl(this.parentDirectives);
                }
            };
            DirectiveTypeData._siblingIdCounter = 0;
            return DirectiveTypeData;
        })();
        angular2.DirectiveTypeData = DirectiveTypeData;
        var WjDirectiveBehavior = (function () {
            function WjDirectiveBehavior(directive, elementRef, injector) {
                this.isDestroyed = false;
                this.directive = directive;
                this.elementRef = elementRef;
                this.injector = injector;
                var typeData = this.typeData = directive.constructor[Ng2Utils.directiveTypeDataProp];
                directive[WjDirectiveBehavior.BehaviourRefProp] = this;
                // have to do it during directive instance creation because it's too early during directive type initialization 
                typeData.resolveForwardDeclarations();
                this.createEvents();
                MethodProxy.attachInstance(directive, this);
                this._setupAsChild();
                if (this._isHostElement()) {
                    elementRef.nativeElement.setAttribute(WjDirectiveBehavior.siblingDirIdAttr, typeData.siblingId);
                }
            }
            WjDirectiveBehavior.getHostElement = function (directive, ngHostElRef) {
                return ngHostElRef.nativeElement;
            };
            WjDirectiveBehavior.attach = function (directive, elementRef, injector) {
                return new WjDirectiveBehavior(directive, elementRef, injector);
            };
            // ----- Directive lifecycle hook proxies
            WjDirectiveBehavior.prototype.dirOnInit = function (originalMethod) {
                originalMethod();
                this._initParent();
                this.subscribeToEvents();
                if (this.directive['wjAfterParentInit']) {
                    this.directive['wjAfterParentInit']();
                }
            };
            WjDirectiveBehavior.prototype.dirOnDestroy = function (originalMethod) {
                originalMethod();
                if (this.isDestroyed) {
                    return;
                }
                this.isDestroyed = true;
                var control = this.directive;
                if (this._siblingInsertedEH) {
                    this.elementRef.nativeElement.removeEventListener('DOMNodeInserted', this._siblingInsertedEH);
                }
                if (this._isParentArray() && !this.parentBehavior.isDestroyed) {
                    var parControl = this.parentBehavior.directive, parProp = this._getParentProp();
                    if (parControl && parProp && control) {
                        var parArr = parControl[parProp];
                        if (parArr) {
                            var idx = parArr.indexOf(control);
                            if (idx >= 0) {
                                parArr.splice(idx, 1);
                            }
                        }
                    }
                }
                if (control instanceof wijmo.Control) {
                    // We call dispose() with a delay, to get directives such as ng-if/ng-repeat a chance to remove its child subtree
                    // berore the control will be disposed. Otherwise, Control.dispose() replaces its host element with an assignment 
                    // to outerHTML, that creates an element clone in its parent with a different pointer, not the one that
                    // ng-if stores locally, so this clone is out of ng-if control and stays in DOM forever.
                    // TBD: do we need this delay in Ng2?
                    // Answer: no, it breaks controls in templates, because Ng2 reuses control's host elements.
                    //setTimeout(function () {
                    if (control.hostElement) {
                        // control.dispose() kills current host element (by outerHTML=... assignment), while Ng2 reuses it,
                        // so we need to keep it in its correct position after call to control.dispose().
                        var host = this.elementRef.nativeElement, hostParent = host && host.parentNode, hostIdx = hostParent ? Array.prototype.indexOf.call(hostParent.childNodes, host) : -1;
                        //TBD: !!! control.dispose() will dispose all child controls, we need to dispose all directives before it!!!
                        control.dispose();
                        if (hostIdx > -1 && Array.prototype.indexOf.call(hostParent.childNodes, host) < 0) {
                            host.textContent = '';
                            hostParent.replaceChild(host, hostParent.childNodes[hostIdx]);
                        }
                    }
                }
            };
            WjDirectiveBehavior.prototype.dirOnChanges = function (originalMethod, changes) {
                originalMethod(changes);
            };
            // ----- end of Directive lifecycle hook proxies
            WjDirectiveBehavior.instantiateTemplate = function (parent, viewContainerRef, templateRef, domRenderer) {
                //var contEl = <Element>viewContainerRef.element.nativeElement,
                //    contParEl = contEl.parentElement;
                var viewRef = viewContainerRef.createEmbeddedView(templateRef, viewContainerRef.length);
                //var nodes = <any[]>domRenderer['getRootNodes'](viewRef['renderFragment']);
                var nodes = viewRef.rootNodes;
                var rootEl = document.createElement('div');
                console.log(viewRef.habal);
                for (var _i = 0; _i < nodes.length; _i++) {
                    var curNode = nodes[_i];
                    rootEl.appendChild(curNode);
                }
                if (parent) {
                    parent.appendChild(rootEl);
                }
                return { viewRef: viewRef, rootElement: rootEl };
            };
            WjDirectiveBehavior.prototype.createEvents = function () {
                var changeEvents = this.typeData.changeEventMap, directive = this.directive;
                //TBD: investigate a way to honor only events/props that are really bound
                // Add event properties.
                for (var _i = 0; _i < changeEvents.length; _i++) {
                    var curEventMap = changeEvents[_i];
                    var changeProps = curEventMap.props;
                    if (curEventMap.eventImpl) {
                        directive[curEventMap.eventImpl] = new ng2.EventEmitter(false);
                    }
                    if (changeProps && changeProps.length) {
                        for (var _a = 0; _a < changeProps.length; _a++) {
                            var curChangeProp = changeProps[_a];
                            directive[curChangeProp.evImpl] = new ng2.EventEmitter(false);
                        }
                    }
                }
            };
            WjDirectiveBehavior.prototype.subscribeToEvents = function () {
                var changeEvents = this.typeData.changeEventMap;
                // Add handlers
                for (var _i = 0; _i < changeEvents.length; _i++) {
                    var curEventMap = changeEvents[_i];
                    this.addHandlers(curEventMap);
                }
            };
            WjDirectiveBehavior.prototype.addHandlers = function (eventMap) {
                var directive = this.directive;
                WjDirectiveBehavior.evaluatePath(directive, eventMap.event).addHandler(function (s, e) {
                    if (eventMap.props && eventMap.props.length) {
                        // Trigger property change events
                        for (var _i = 0, _a = eventMap.props; _i < _a.length; _i++) {
                            var curChangeProp = _a[_i];
                            directive[curChangeProp.evImpl].next(directive[curChangeProp.prop]);
                        }
                    }
                    // Trigger Wijmo event
                    if (eventMap.eventImpl) {
                        directive[eventMap.eventImpl].next(e);
                    }
                });
            };
            WjDirectiveBehavior.prototype._setupAsChild = function () {
                var parDirs;
                if (!(this._isChild() && (parDirs = this.typeData.parentDirectives) && parDirs.length > 0)) {
                    return;
                }
                if (this._isHostElement()) {
                    this.elementRef.nativeElement.style.display = 'none';
                }
                var nearestDir;
                for (var _i = 0; _i < parDirs.length; _i++) {
                    var parType = parDirs[_i];
                    if (parType) {
                        var curParDir = this.injector.getOptional(parType);
                        if (curParDir) {
                            if (nearestDir) {
                                if (WjDirectiveBehavior.containsDirective(nearestDir, curParDir)) {
                                    nearestDir = curParDir;
                                }
                            }
                            else {
                                nearestDir = curParDir;
                            }
                        }
                    }
                }
                if (!nearestDir) {
                    return;
                }
                var parBehavior = this.parentBehavior = WjDirectiveBehavior.getBehavior(nearestDir), metaData = this.typeData.metaData;
                this._parentPropDesc = new wijmo_angular2_MetaFactory_1.wjNg2Meta.ComplexPropDesc(metaData.parentProperty, metaData.isParentPropertyArray, metaData.ownsObject);
            };
            // --------------------- Child directive ------------------------
            //Determines whether this is a child link.
            //NOTE: functionality is *not* based on _parentPropDesc
            WjDirectiveBehavior.prototype._isChild = function () {
                return this._isParentInitializer() || this._isParentReferencer();
            };
            // Indicates whether this directictive operates as a child directictive that initializes a property of its parent.
            WjDirectiveBehavior.prototype._isParentInitializer = function () {
                return this.typeData.metaData.parentProperty != undefined;
            };
            // Indicates whether this directictive operates as a child directictive that references a parent in its property or
            // a constructor.
            WjDirectiveBehavior.prototype._isParentReferencer = function () {
                return this.typeData.metaData.parentReferenceProperty != undefined;
            };
            //For the child directives returns parent's property name that it services. Property name defined via
            //the wjProperty attribute of directive tag has priority over the directive._property definition.
            //NOTE: functionality is *not* based on _parentPropDesc
            WjDirectiveBehavior.prototype._getParentProp = function () {
                return this._isParentInitializer() ?
                    this.directive[WjDirectiveBehavior.parPropAttr] || this.typeData.metaData.parentProperty
                    : undefined;
            };
            // For a child directive, the name of the property of the directive's underlying object that receives the reference
            // to the parent, or an empty string that indicates that the reference to the parent should be passed as the 
            // underlying object's constructor parameter.
            WjDirectiveBehavior.prototype._getParentReferenceProperty = function () {
                return this.typeData.metaData.parentReferenceProperty;
            };
            // Determines whether the child link uses an object created by the parent property, instead of creating it by
            // itself, and thus object's initialization should be delayed until parent link's control is created.
            //IMPORTANT: functionality is *based* on _parentPropDesc
            WjDirectiveBehavior.prototype._useParentObj = function () {
                // we can't support this, all affected properties should be read-write
                return false;
            };
            // For the child link, determines whether the servicing parent property is an array.
            //IMPORTANT: functionality is *based* on _parentPropDesc
            WjDirectiveBehavior.prototype._isParentArray = function () {
                return this._isParentInitializer() && this._parentPropDesc.isArray;
            };
            // For the child referencer directive, indicates whether the parent should be passed as a parameter the object
            // constructor.
            WjDirectiveBehavior.prototype._parentInCtor = function () {
                return this._isParentReferencer() && this._getParentReferenceProperty() == '';
            };
            WjDirectiveBehavior.prototype._initParent = function () {
                if (!this.parentBehavior || this._useParentObj()) {
                    return;
                }
                var parDir = this.parentBehavior.directive, propName = this._getParentProp(), control = this.directive;
                if (this._isParentInitializer()) {
                    var parProp = this._getParentProp(), parPropDescOverride = wijmo_angular2_MetaFactory_1.wjNg2Meta.MetaFactory.findComplexProp(parProp, this.parentBehavior.typeData.metaData.complexProps);
                    if (parPropDescOverride) {
                        this._parentPropDesc = parPropDescOverride;
                    }
                    else {
                        this._parentPropDesc.propertyName = parProp;
                    }
                    if (this._isParentArray()) {
                        // insert child at correct index, which is the same as an index of the directive element amid sibling directives
                        // of the same type
                        var parArr = parDir[propName], isHostElement = this._isHostElement(), linkIdx = isHostElement ? this._getSiblingIndex() : -1;
                        if (linkIdx < 0 || linkIdx >= parArr.length) {
                            linkIdx = parArr.length;
                        }
                        parArr.splice(linkIdx, 0, control);
                        if (isHostElement) {
                            this._siblingInsertedEH = this._siblingInserted.bind(this);
                            this.elementRef.nativeElement.addEventListener('DOMNodeInserted', this._siblingInsertedEH);
                        }
                    }
                    else {
                        parDir[propName] = control;
                    }
                }
                if (this._isParentReferencer() && !this._parentInCtor()) {
                    control[this._getParentReferenceProperty()] = parDir;
                }
            };
            // Gets an index of this directive host element among another host elements pertain to the same directive type.
            WjDirectiveBehavior.prototype._getSiblingIndex = function () {
                var thisEl = this.elementRef.nativeElement, parEl = thisEl.parentElement;
                // If parentElement is null, e.g. because this element is temporary in DocumentFragment, the index
                // of the element isn't relevant to the item's position in the array, so we return -1 and thus force
                // a calling code to not reposition the item in the array at all.  
                if (!parEl) {
                    return -1;
                }
                var siblings = parEl.childNodes, idx = -1, dirId = this.typeData.siblingId;
                for (var i = 0; i < siblings.length; i++) {
                    var curEl = siblings[i];
                    if (curEl.nodeType == 1 && curEl.getAttribute(WjDirectiveBehavior.siblingDirIdAttr) == dirId) {
                        ++idx;
                        if (curEl === thisEl) {
                            return idx;
                        }
                    }
                }
                return -1;
            };
            WjDirectiveBehavior.prototype._siblingInserted = function (e) {
                if (e.target === this.elementRef.nativeElement) {
                    var lIdx = this._getSiblingIndex(), parArr = this.parentBehavior.directive[this._getParentProp()], directive = this.directive, arrIdx = parArr.indexOf(directive);
                    if (lIdx >= 0 && arrIdx >= 0 && lIdx !== arrIdx) {
                        parArr.splice(arrIdx, 1);
                        lIdx = Math.min(lIdx, parArr.length);
                        parArr.splice(lIdx, 0, directive);
                    }
                }
            };
            // Indicates whether the host node is HTMLElement. E.g. for template directive a host node is comment.
            WjDirectiveBehavior.prototype._isHostElement = function () {
                return this.elementRef.nativeElement.nodeType === Node.ELEMENT_NODE;
            };
            // --- end of Child directive ------------------------
            // ----- Utility methods
            WjDirectiveBehavior.evaluatePath = function (obj, path) {
                this._pathBinding.path = path;
                return this._pathBinding.getValue(obj);
            };
            WjDirectiveBehavior.getBehavior = function (directive) {
                return directive ? directive[WjDirectiveBehavior.BehaviourRefProp] : null;
            };
            WjDirectiveBehavior.containsDirective = function (parentDirective, childDirective) {
                if (!(parentDirective && childDirective)) {
                    return false;
                }
                var parInj = WjDirectiveBehavior.getBehavior(parentDirective).injector, childInj = WjDirectiveBehavior.getBehavior(childDirective).injector;
                for (var curInj = childInj.parent; curInj; curInj = curInj.parent) {
                    if (parInj === curInj) {
                        return true;
                    }
                }
                return false;
            };
            // updates the array with resolved forwardRef(s), non-forward refs stay untouched
            WjDirectiveBehavior.resolveForwardDecl = function (array) {
                if (!array) {
                    return;
                }
                for (var i = 0; i < array.length; i++) {
                    var curDecl = array[i];
                    array[i] = array[i] ? ng2.resolveForwardRef(array[i]) : array[i];
                }
            };
            // Name of the property created on directive instance that references this behavior
            WjDirectiveBehavior.BehaviourRefProp = 'wjBehaviour';
            WjDirectiveBehavior.parPropAttr = 'wjProperty';
            WjDirectiveBehavior.siblingDirIdAttr = 'wj-directive-id';
            WjDirectiveBehavior._pathBinding = new wijmo.Binding('');
            return WjDirectiveBehavior;
        })();
        angular2.WjDirectiveBehavior = WjDirectiveBehavior;
        var Ng2Utils = (function () {
            function Ng2Utils() {
            }
            // Returns an array for the @Component 'outputs' property.
            Ng2Utils.initEvents = function (directiveType, changeEvents) {
                var ret = [];
                for (var _i = 0; _i < changeEvents.length; _i++) {
                    var curEventMap = changeEvents[_i];
                    var changeProps = curEventMap.props;
                    if (curEventMap.event && curEventMap.eventImpl) {
                        ret.push(curEventMap.eventImpl + ':' + curEventMap.event);
                    }
                    if (changeProps && changeProps.length) {
                        for (var _a = 0; _a < changeProps.length; _a++) {
                            var curChangeProp = changeProps[_a];
                            ret.push(curChangeProp.evImpl + ':' + curChangeProp.evExposed);
                        }
                    }
                }
                return ret;
            };
            Ng2Utils.getChangeEventNameImplemented = function (propertyName) {
                return Ng2Utils.getChangeEventNameExposed(propertyName) + 'Ng';
            };
            Ng2Utils.getChangeEventNameExposed = function (propertyName) {
                return propertyName + 'Change';
            };
            Ng2Utils.getWjEventNameImplemented = function (eventName) {
                return eventName + 'Wj';
            };
            Ng2Utils.getChangeEventMap = function (metaData) {
                var ret = [], eventDescArr = metaData.events || [], propDescArr = metaData.props || [];
                ret = eventDescArr.map(function (ed) {
                    return {
                        event: ed.eventName,
                        eventImpl: Ng2Utils.getWjEventNameImplemented(ed.eventName),
                        props: (function (arr) { return arr && arr.length ? arr : null; })(metaData.props.filter(function (pd) { return pd.changeEvent === ed.eventName; })
                            .map(function (pd) {
                            var ret = 
                            //return
                            {
                                prop: pd.propertyName,
                                evExposed: Ng2Utils.getChangeEventNameExposed(pd.propertyName),
                                evImpl: Ng2Utils.getChangeEventNameImplemented(pd.propertyName)
                            };
                            return ret;
                        }))
                    };
                })
                    .concat(propDescArr.filter(function (pd) {
                    return pd.changeEvent && pd.changeEvent.indexOf('.') > -1;
                }).map(function (pd) {
                    var evParts = pd.changeEvent.split('.'), parentEvName = evParts[evParts.length - 1];
                    var ret = {
                        event: pd.changeEvent,
                        eventImpl: null,
                        props: [{
                                prop: pd.propertyName,
                                evExposed: Ng2Utils.getChangeEventNameExposed(pd.propertyName),
                                evImpl: Ng2Utils.getChangeEventNameImplemented(pd.propertyName)
                            }]
                    };
                    return ret;
                }));
                return ret;
            };
            //static initInstanceEventsMethod = 'wjInitDirectiveEvents';
            //TBD: use Symbol?
            Ng2Utils.directiveTypeDataProp = 'wjDirTypeData';
            return Ng2Utils;
        })();
        angular2.Ng2Utils = Ng2Utils;
        var MethodProxy = (function () {
            function MethodProxy(targetType, methodName, proxyMethod) {
                this.proxyMethod = proxyMethod;
                var targetProto = targetType.prototype;
                // take original method, if exists
                this.originalMethod = targetProto[methodName];
                targetProto[methodName] = this._getMethodStub();
            }
            MethodProxy.attachInstance = function (target, proxy) {
                target[MethodProxy.ProxyRefProp] = proxy;
            };
            // implementation looks from the target
            MethodProxy.prototype._getMethodStub = function () {
                // closure variables
                var methodProxy = this;
                // this function will be called by target
                var ret = function () {
                    var params = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        params[_i - 0] = arguments[_i];
                    }
                    var targetThis = this, proxy = targetThis[MethodProxy.ProxyRefProp];
                    var originalCall = function () {
                        var params = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            params[_i - 0] = arguments[_i];
                        }
                        if (methodProxy.originalMethod) {
                            methodProxy.originalMethod.apply(targetThis, params);
                        }
                    };
                    var parArr = [originalCall];
                    if (params != null && params.length) {
                        parArr = parArr.concat(params);
                    }
                    methodProxy.proxyMethod.apply(proxy, parArr);
                };
                return ret;
            };
            MethodProxy.ProxyRefProp = 'wjProxyHostRef';
            return MethodProxy;
        })();
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Base = wj.angular2;
exports.WjComponent = exports.wjNg2Base.WjComponent;
exports.WjDirectiveBehavior = exports.wjNg2Base.WjDirectiveBehavior;
//# sourceMappingURL=wijmo.angular2.directiveBase.js.map
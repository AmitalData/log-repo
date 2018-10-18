/// <reference path="../../../node_modules/reflect-metadata/reflect-metadata.d.ts"/>
import {Type, Injectable, Input, Component, ViewEncapsulation} from 'angular2/core';
import {
RouteConfig,
ROUTER_DIRECTIVES,
RouteRegistry,
} from 'angular2/router';

@Injectable()
export class DynamicRouteConfigurator {
    constructor(private registry: RouteRegistry) { }
    addRoute(component: Type, route) {

        let routeConfig = this.getRoutes(component);
        let configs: any[] = <any[]>routeConfig.configs;
        //console.log(route, configs.find(x => x === route));
        if (configs.find(x => x.name === route.name)) {
            console.log("return");
            return;
        }
        routeConfig.configs.push(route);
        this.updateRoutes(component, routeConfig);
        this.registry.config(component, route);
    }
    removeRoute() {
        // need to touch private APIs - bad
    }
    getRoutes(component: Type) {
        return Reflect.getMetadata('annotations', component)
            .filter(a => {
                return a.constructor.name === 'RouteConfig';
            }).pop();
    }
    updateRoutes(component: Type, routeConfig) {
        let annotations = Reflect.getMetadata('annotations', component);
        let routeConfigIndex = -1;
        for (let i = 0; i < annotations.length; i += 1) {
            if (annotations[i].constructor.name === 'RouteConfig') {
                routeConfigIndex = i;
                //console.log("annotations const name", annotations[i]);
                break;
            }
            //if (routeConfig.configs[i].name) {
            //    //console.log("route config", routeConfig.configs[i].name);
            //}
        }
        if (routeConfigIndex < 0) {
            throw new Error('No route metadata attached to the component');
        }
        //else {
        //    return;
        //}
        //console.log(annotations[routeConfigIndex]);
        annotations[routeConfigIndex] = routeConfig;
        Reflect.defineMetadata('annotations', annotations, Type);
        
    }
}
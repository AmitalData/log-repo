import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router, RouterState, RouterStateSnapshot } from '@angular/router';
import { HostScreenService } from './host-screen.service';
import { AppTool } from '../Infrastructure/Tools';
import { LoginService } from '../Infrastructure/Services/LoginService';
import { SessionLocator } from '../Infrastructure/Utilities/SessionLocator';

@Injectable({
  providedIn: 'root',
})
export class HostScreenGuardService implements CanActivate {
  constructor(public hostScreen: HostScreenService, public router: Router, public activatedRoute: ActivatedRoute, private loginService: LoginService) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    let url = state.url;
    this.hostScreen.redirectUrl = url;

    const hostResult = await this.checkHostScreen(route, state);

    if (hostResult === true) {
      return true;
    }

    return this.checkLogin(url);
  }


  checkLogin(url: string): boolean {
    if (this.hostScreen.isAuthenticated()) { return true; }
    this.hostScreen.redirectUrl = url;
    this.router.navigate(["/login"]);
    return false;
  }


  checkHostScreen(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> | boolean {
    let AmitalSSOAngular = this.getParameterByName(
      'AmitalSSOAngular',
      window.location.href
    );
    let amitaltoken = this.getParameterByName(
      'token',
      window.location.href
    );
    let amitaltenant = this.getParameterByName(
      'tenant',
      window.location.href
    );
    let searchValue = this.getParameterByName(
      'searchValue',
      window.location.href
    );
    if (searchValue) {
      sessionStorage.setItem('searchValue', searchValue);
    }
    else {
      sessionStorage.removeItem('searchValue');
    }
    if (AmitalSSOAngular) {
      sessionStorage.setItem('AmitalSSOAngular', AmitalSSOAngular);
    }
    if (amitaltoken) {
      sessionStorage.setItem('Token', amitaltoken);
    }
    if (amitaltenant) {
      sessionStorage.setItem('Tenant', amitaltenant);
    }
    
    if (!AppTool.IsNullOrEmpty(AmitalSSOAngular) &&
    !AppTool.IsNullOrEmpty(amitaltoken) &&
    !AppTool.IsNullOrEmpty(amitaltenant)) {
      if (this.hostScreen.isAuthenticated()) {
        return this.initLoginHost();
      }
      else {
        return false;
      }
    }

  }
  getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
      results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
  }

  private initLoginHost(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      let AmitalSSOAngular = sessionStorage.getItem('AmitalSSOAngular');
      let amitaltoken = sessionStorage.getItem('Token');
      let amitaltenant = sessionStorage.getItem('Tenant');

      if (
        !AppTool.IsNullOrEmpty(AmitalSSOAngular) &&
        !AppTool.IsNullOrEmpty(amitaltoken) &&
        !AppTool.IsNullOrEmpty(amitaltenant)
      ) {
        let loginParameters: any = {};
        loginParameters.Tenant = amitaltenant;
        loginParameters.Token = amitaltoken;

        this.loginService.PostAuthentication(loginParameters).subscribe({
          next: (userData: any) => {
            if (!userData.HasError) {
              userData.AmitalBrowserInUse = true;
              sessionStorage.setItem('userdata', JSON.stringify(userData));
              SessionLocator.IsExternalParams = false;
              resolve(true);
            } else {
              this.hostScreen.closeSession();
              this.router.navigate(['/login']);
              resolve(false);
            }
          },
          error: () => {
            this.hostScreen.closeSession();
            this.router.navigate(['/login']);
            resolve(false);
          }
        });
      }
      else {
        resolve(false);
      }
    });
  }
}

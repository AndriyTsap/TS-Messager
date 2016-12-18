/// <reference path="../../typings/globals/es6-shim/index.d.ts" />

import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import {enableProdMode} from '@angular/core';

enableProdMode();
import { MembershipService } from './core/services/membership.service';
import { User } from './core/domain/user';

@Component({
    selector: 'photogallery-app',
    templateUrl: './app/app.component.html'
})
export class AppComponent implements OnInit {

    constructor(public membershipService: MembershipService,
                public location: Location) { }

    ngOnInit() {
        let userData = JSON.parse(localStorage.getItem('user'));
        if(userData!=null){
            if(userData.RememberMe==true){
                this.membershipService.login(userData)
                .then(data => {
                    var tempData: any=data;
                    let token=JSON.parse(tempData);
                    localStorage.setItem('token', token.access_token);
                },
                err => { console.log("Error"+ err)})
                
            }
            
        }
        this.location.go('/');
    }

    isUserLoggedIn(): boolean {
        return this.membershipService.isUserAuthenticated();
    }

    getUserName(): string {
        if (this.isUserLoggedIn()) {
            var _user = this.membershipService.getLoggedInUser();
            return _user.Username;
        }
        else
            return 'Account';
    }

    logout(): void {
        this.membershipService.logout()
            .subscribe(res => {
                localStorage.removeItem('user');
                localStorage.removeItem('token');
                localStorage.removeItem('currentChatId');
            },
            error => console.error('Error: ' + error),
            () => { });
    }
}

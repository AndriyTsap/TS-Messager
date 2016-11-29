import { Component, OnInit, Inject } from '@angular/core';
import { User } from "../core/domain/user";
import { UserService } from "../core/services/user.service";

@Component({
    selector: 'profile',
    templateUrl: './app/components/profile.component.html',
    styleUrls:[ './app/components/profile.component.css']
})

export class ProfileComponent{
    username:string ="Igor";
    phone:string="0978059695";
    email:string="igor.was@meta.ua";
    photoPath:string="https://pp.vk.me/c837522/v837522134/1f75/a0PT55_KNPg.jpg"
    birthDate:string="29.09.1995";

    /*
    public users:User[];
    
    constructor(@Inject(UserService) public userService:UserService){
        userService.getAll().subscribe(res =>{
            var data = res.json();
            var user:any;
            this.users=data;
            this.users.forEach(user=>console.log(user.Username));
        });
    }*/
}


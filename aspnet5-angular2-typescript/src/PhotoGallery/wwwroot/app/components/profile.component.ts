import { Component, OnInit, Inject } from '@angular/core';
import { User } from "../core/domain/user";
import { UserService } from "../core/services/user.service";

@Component({
    selector: 'profile',
    templateUrl: './app/components/profile.component.html',
    styleUrls:[ './css/profile.component.css']
})

export class ProfileComponent{
    showEditDialog:boolean=false;
    username:string ="Igor";
    phone:string="0978059695";
    email:string="igor.was@meta.ua";
    photoPath:string="https://pp.vk.me/c413131/v413131877/967d/9b3K-Xg9M8o.jpg";
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


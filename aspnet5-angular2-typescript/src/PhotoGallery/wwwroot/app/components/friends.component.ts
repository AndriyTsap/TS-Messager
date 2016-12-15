import { Component, OnInit, Inject } from '@angular/core';
import { UserFull } from "../core/domain/user-full";
import { UserService } from "../core/services/user.service";

@Component({
    selector: 'friends',
    templateUrl: './app/components/friends.component.html',
    styleUrls: ['./css/friends.component.css']
})

export class FriendsComponent {
    friends: UserFull[];
    tenOrMoreFriends:boolean ;
    constructor( @Inject(UserService) public userService: UserService) {
        this.friends = []
        this.tenOrMoreFriends=false;
    }

    ngOnInit() {
        this.getFriends();
    }

    getFriends(){
        this.userService.getFriends(localStorage.getItem("token")).subscribe(res => {
            var data = res.json();
            this.tenOrMoreFriends=(data.length>=10);
            data.forEach((user) => {
                this.friends.push({
                    Username: user.Username,
                    Password: "",
                    Email: user.Email,
                    FirstName: user.FirstName,
                    LastName: user.LastName,
                    Phone: user.Phone,
                    BirthDate: user.BirthDate,
                    Photo: user.Photo,
                    About: user.About
                });
            })
        })
    }
    search(username:string ){
        if(username==""){
            this.getFriends()
        }
        else{
            this.userService.search(localStorage.getItem("token"),username).subscribe(res => {
                var data = res.json();
                this.friends=[];
                this.tenOrMoreFriends=(data.length>=10);
                data.forEach((user) => {
                    this.friends.push({
                        Username: user.Username,
                        Password: "",
                        Email: user.Email,
                        FirstName: user.FirstName,
                        LastName: user.LastName,
                        Phone: user.Phone,
                        BirthDate: user.BirthDate,
                        Photo: user.Photo,
                        About: user.About
                    });
                })
            })
        }
    }
}
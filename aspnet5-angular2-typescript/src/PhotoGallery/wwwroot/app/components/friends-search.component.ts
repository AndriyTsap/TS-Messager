import { Component, OnInit, Inject } from '@angular/core';
import { UserFull } from "../core/domain/user-full";
import { UserService } from "../core/services/user.service";

@Component({
    selector: 'friends-search',
    templateUrl: './app/components/friends-search.component.html',
    styleUrls: ['./css/friends.component.css']
})

export class FriendsSearchComponent {
    randomPeople: UserFull[];
    tenOrMorePeople:boolean;

    constructor( @Inject(UserService) public userService: UserService) {
        this.randomPeople = [];
        this.tenOrMorePeople=false;
    }

    ngOnInit() {
        this.getPeople();
    }

    search(username:string ){;
        if(username==""){
            this.getPeople()
        }
        else{
            this.userService.search(localStorage.getItem("token"),username)
                .subscribe(res => {
                    var data = res.json();
                    this.randomPeople=[];
                    this.tenOrMorePeople=(data.length>=10);
                    data.forEach((user) => { 
                        this.randomPeople.push({
                            Id: user.Id,
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

    getPeople(){
        this.userService.getAll().subscribe(res => {
            var data = res.json();
            var user: any;

            //this.loadedPeople+=10;
            this.tenOrMorePeople=(data.length>=10);
            for (let i=0;i<10;i++){//change to previous version after fixing API
                user=data[i];
            //data.forEach((user) => {
                this.randomPeople.push({
                    Id: user.Id,
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
            }//)
        })
    }

    createChat(){
       
    }


}
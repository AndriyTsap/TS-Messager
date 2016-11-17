import { Component, OnInit, Inject } from '@angular/core';
import { UserService } from "../core/services/user.service";
import { MessageService } from "../core/services/message.service";
import { User } from "../core/domain/user";


@Component({
    selector: 'test',
    styleUrls: ["./css/test.component.css"],
    templateUrl: './app/components/test.component.html'
})
export class TestComponent implements OnInit{
    public users:User[];
    
    constructor(@Inject(UserService) public userService:UserService, @Inject(MessageService) public messageService:MessageService){
        this.users = [];
    }

    ngOnInit(){
        this.userService.getAll().subscribe(res =>{
            var data = res.json();
            var user:any;

            data.forEach((user)=> {
                this.users.push({Password:"",RememberMe:true,Username:user.Username});          
            })
        })
    }   

    sendMessage(text: string, token: string){
        console.log(text, token);
    }
}
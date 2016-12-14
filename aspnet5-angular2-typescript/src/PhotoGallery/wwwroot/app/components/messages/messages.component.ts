import { Component, OnInit } from '@angular/core';
import { MessageService } from '../../core/services/message.service';

@Component({
    selector: 'messages',
    templateUrl: './app/components/messages/messages.component.html',
    styleUrls:[ '././css/messages.component.css'],
})

export class MessagesComponent{
    chats:any;
    constructor(public messageService: MessageService){
        this.chats=[];
    }

    ngOnInit() {
        this.messageService.setToken(localStorage.getItem("token"))
        this.messageService.getChats().subscribe(res => {
            this.chats= res.json();
            console.log(this.chats+"щось прийшло")
            },
            error => {
                if (error.status == 401 || error.status == 404) {
                    console.log(error)
                }
            });   
    }

    getMessage(){
    
    }
    
    searchChat(){

    }

    sendMessage(){
        
    }

}
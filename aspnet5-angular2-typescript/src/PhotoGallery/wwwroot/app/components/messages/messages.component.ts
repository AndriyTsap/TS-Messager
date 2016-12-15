import { Component, OnInit } from '@angular/core';
import { MessageService } from '../../core/services/message.service';
import { NotificationService } from '../../core/services/notification.service';
import { OperationResult } from "../../core/domain/operationResult";

@Component({
    selector: 'messages',
    templateUrl: './app/components/messages/messages.component.html',
    styleUrls:[ '././css/messages.component.css'],
})

export class MessagesComponent{
    chats:any;
    currentChatId:number;
    constructor(public messageService: MessageService,
                public notificationService: NotificationService){
        this.chats=[];
    }

    ngOnInit() {
        this.messageService.setToken(localStorage.getItem("token"))
        this.messageService.getChats().subscribe(res => {
            this.chats= res.json();
                console.log(res);
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

    sendMessage(newMessage:string){
        var _sendResult: OperationResult = new OperationResult(false, '');
        this.currentChatId=2;
        console.log("in message controller")
        this.messageService.send(newMessage,this.currentChatId)
            .subscribe(res => {
                    _sendResult.Succeeded = res.Succeeded;
                    _sendResult.Message = res.Message;
                },
                error => console.error('Error: ' + error),
                () => {
                    if (!_sendResult.Succeeded) {
                        console.log(_sendResult.Message)
                        this.notificationService.printErrorMessage(_sendResult.Message);
                    }
                }); 
    }

}
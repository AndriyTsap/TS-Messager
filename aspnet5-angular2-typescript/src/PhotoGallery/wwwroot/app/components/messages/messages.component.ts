import { Component, OnInit } from '@angular/core';
import { MessageService } from '../../core/services/message.service';
import { NotificationService } from '../../core/services/notification.service';
import { OperationResult } from "../../core/domain/operationResult";
import { Message } from "../../core/domain/message";
import { Chat } from "../../core/domain/chat";
import { Angular2AutoScroll } from "angular2-auto-scroll/lib/angular2-auto-scroll.directive";
//import { SignalRConnectionStatus, Message, Chat } from '../../interfaces';

@Component({
    selector: 'messages',
    templateUrl: './app/components/messages/messages.component.html',
    styleUrls:[ '././css/messages.component.css'],
})

export class MessagesComponent{
    chats:Chat[];
    messages:Message[];
    currentChatId:number;
    messageOffset:number;

    subscribed: boolean;
    connectionId: string;
    
    constructor(public messageService: MessageService,
                public notificationService: NotificationService){
        this.chats=[];
        this.messages=[];
        this.messageOffset=0;
        this.currentChatId=+localStorage.getItem("currentChatId");
    }

    ngOnInit() {
        this.messageService.setToken(localStorage.getItem("token"));
        this.getChats();
        this.getMessage(this.currentChatId);
        //for signalR
       
         /* 
        this.messageService.start(true).subscribe(
            null,
            error => console.log('Error on init: ' + error));
    
        //
        this.listenForConnection();

       
        this.messageService.connectionState
            .subscribe(
            connectionState => {
                if (connectionState == SignalRConnectionStatus.Connected) {
                    console.log('Connected!');
                    //self.getChats();
                } else {
                    console.log(connectionState.toString());
                }
            },
            error => {
                console.log(error);
            });*/
        //    
    }

    getChats(){
        this.messageService.getChats()
            .subscribe(res => {
                this.chats= res.json().reverse();
                console.log(this.chats)
                },
                error => {
                    if (error.status == 401 || error.status == 404) {
                        console.log(error)
                    }
                });  
    }

    getMessage(currentChatId:number){
        this.messages=[];
        this.messageService.getMessageByChatId(currentChatId,this.messageOffset)
            .subscribe(res => {
                let data= res.json();
                let theSameSenderInLine=false;
                this.messages.push({
                            Id: data[0].Id,
                            ChatId: data[0].ChatId,
                            Date: data[0].Date,
                            SenderId: data[0].SenderId,
                            Text: data[0].Text,
                            SenderFirstName: data[0].FirstName,
                            SenderLastName: data[0].LastName,
                            Photo: data[0].Photo
                }); 
                for(let i=1;i<data.length;i++){
                    theSameSenderInLine=(data[i-1].SenderId==data[i].SenderId);
                    this.messages.push({
                            Id: data[i].Id,
                            ChatId: data[i].ChatId,
                            Date: theSameSenderInLine ? null : data[i].Date,
                            SenderId: data[i].SenderId,
                            Text: data[i].Text,
                            SenderFirstName: theSameSenderInLine ? null : data[i].FirstName,
                            SenderLastName: theSameSenderInLine ? null :data[i].LastName,
                            Photo:  theSameSenderInLine ? null : data[i].Photo 
                    })
                }
                },
                error => {
                    if (error.status == 401 || error.status == 404) {
                        console.log(error)
                    }
            });  
         
    }
    
    searchChat(name:string){
        console.log("serach"+name)
        if(name!=""){
            this.messageService.searchChat(name)
                .subscribe(res => {
                    this.chats = res.json();
                })
        }
        else{
            this.getChats()
        }
        
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
    
    getMoreMessages(){
        this.messageOffset+=20;
        this.getMessage(this.currentChatId);
    }

    onSelect(chatId: number) { 
        this.currentChatId = chatId;
    }


    //for signalR
    listenForConnection() {
        // Listen for connected / disconnected events
        this.messageService.setConnectionId.subscribe(
            id => {
                console.log(id);
                this.connectionId = id;
            }
        );
    }

    listenForMessages(){//only in console
        this.messageService.addMessage.subscribe(
            message => {
                console.log('received message..');
                console.log(message);
                //update view
            }
        )
    }

    listenForChats(){//only in console
        this.messageService.addChat.subscribe(
            chat => {
                console.log('received chat..');
                console.log(chat);
                //update view
            }
        )
    }
}
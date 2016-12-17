import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
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
    @ViewChild('chat') private chatForScroll: ElementRef;

    subscribed: boolean;
    connectionId: string;
    
    constructor(public messageService: MessageService,
                public notificationService: NotificationService){
        this.chats=[];
        this.messages=[];
        this.messageOffset=0;
    }

    ngOnInit() {
        this.messageService.setToken(localStorage.getItem("token"));
        this.getChats();
        this.getMessage();
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
        //temp
        this.currentChatId=2; //take from localStorage
        this.messageService.getMessageByChatId(this.currentChatId,this.messageOffset)
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
                this.goToBottom()
                console.log(this.messages);
                },
                error => {
                    if (error.status == 401 || error.status == 404) {
                        console.log(error)
                    }
            });  
         
    }
    
    searchChat(name:string){
        this.messageService.searchChat(name)
                .subscribe(res => {
                    var data = res.json();
                    this.chats=[];
                    data.forEach((chat) => { 
                        this.chats.push({
                            Id: chat.Id,
                            Name: chat.Name
                        });
                    })
                })
        console.log(this.chats)
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

    goToBottom(){
        this.chatForScroll.nativeElement.scrollTop = -10000;
        console.log("new " +this.chatForScroll.nativeElement.scrollTop);
    }
    
    getMoreMessages(){
        this.messageOffset+=20;
        this.getMessage();
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
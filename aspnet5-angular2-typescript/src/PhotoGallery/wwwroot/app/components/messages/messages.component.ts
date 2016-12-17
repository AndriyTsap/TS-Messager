import { Component, OnInit } from '@angular/core';
import { MessageService } from '../../core/services/message.service';
import { NotificationService } from '../../core/services/notification.service';
import { OperationResult } from "../../core/domain/operationResult";
import { Message } from "../../core/domain/message";
import { Chat } from "../../core/domain/chat";

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

    subscribed: boolean;
    connectionId: string;
    
    constructor(public messageService: MessageService,
                public notificationService: NotificationService){
        this.chats=[];
        this.messages=[];
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
        this.currentChatId=2;
        this.messageService.getMessageByChatId(this.currentChatId)
            .subscribe(res => {
                var data= res.json();
                data.forEach((message) => { 
                        this.messages.push({
                            Id: message.Id,
                            ChatId: message.ChatId,
                            Date: message.Date,
                            SenderId: message.SenderId,
                            Text: message.Text                            
                        });
                    })
                    console.log(this.messages);
                },
                error => {
                    if (error.status == 401 || error.status == 404) {
                        console.log(error)
                    }
            });  
    }
    
    searchChat(username:string){
        this.messageService.searchChat(username)
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
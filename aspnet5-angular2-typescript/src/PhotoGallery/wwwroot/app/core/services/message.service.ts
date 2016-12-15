import { Http, Response, Request } from '@angular/http';
import { Injectable, Inject } from '@angular/core';
import { DataService } from './data.service';
import { User } from '../domain/user';

@Injectable()
export class MessageService {
    private _messagesGetAllAPI: string = 'api/messages';
    private _chatsGetAllAPI: string = 'api/messages/chats';

    private _token: string;
    
    constructor(@Inject(DataService) public dataService: DataService) { }

    public setToken(token: string){
        this._token = token;
    }

    public send(text:string, chat:number){
        this.dataService.set(this._messagesGetAllAPI);
        console.log(" in message service");
        return this.dataService.postAuthenticate(this._token,{text, chat});
        
    }
    //not try yet
    public getChats(){
        this.dataService.set(this._chatsGetAllAPI);
        return this.dataService.getAuthenticate(this._token);
    }
}
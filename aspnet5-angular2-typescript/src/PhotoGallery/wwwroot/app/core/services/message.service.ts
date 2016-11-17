import { Http, Response, Request } from '@angular/http';
import { Injectable, Inject } from '@angular/core';
import { DataService } from './data.service';
import { User } from '../domain/user';

@Injectable()
export class MessageService {
    private _userGetAllAPI: string = 'api/messages';
    private _token: string;
    
    constructor(@Inject(DataService) public dataService: DataService) { }

    public setToken(token: string){
        this._token = token;
    }

    public send(text:string, chat:number){
        var _result: any;

        this.dataService.set(this._userGetAllAPI);
        _result = this.dataService.postAuthenticate(this._token, {text, chat});
        return _result;
    }
    
}
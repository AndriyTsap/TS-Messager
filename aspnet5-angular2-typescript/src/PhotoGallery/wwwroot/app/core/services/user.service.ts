import { Http, Response, Request } from '@angular/http';
import { Injectable, Inject } from '@angular/core';
import { DataService } from './data.service';
import { User } from '../domain/user';

@Injectable()
export class UserService {
    private _userGetAllAPI: string = 'api/users';

    constructor(@Inject(DataService) public dataService: DataService) { }

    public getAll(){
        var _users;
        
        this.dataService.set(this._userGetAllAPI);
        _users = this.dataService.get();
        return _users;
    }
    
}
import { Http, Response, Request } from '@angular/http';
import { Injectable, Inject } from '@angular/core';
import { DataService } from './data.service';
import { User } from '../domain/user';
import { UserFull } from '../domain/user-full';

@Injectable()
export class UserService {
    private _userGetAllAPI: string = 'api/users/';
    private _userGetByIdAPI: string =  'api/users/getById?id=';
    private _userGetByTokenAPI: string =  'api/users/getByToken';
    private _userDelete: string =  'api/users/delete'
    private _userEditPersonalDataAPI: string =  'api/users/editPersonalData';
   

    constructor(@Inject(DataService) public dataService: DataService) { }

    public getAll(){
        var _users;
        
        this.dataService.set(this._userGetAllAPI);
        _users = this.dataService.get();
        return _users;
    }
    
    public getById(id: number){

        this.dataService.set(this._userGetByIdAPI+id);
        return this.dataService.get();;
    }

    public getByToken(token: string){
        this.dataService.set(this._userGetByTokenAPI);
        return this.dataService.getAuthenticate(token);
    }

    public update(token:string,user:UserFull){ 
        
        this.dataService.set(this._userEditPersonalDataAPI);
        return this.dataService.putAuthenticate(token,user); ; 
    }

    public delete(token:string){ 
        
        this.dataService.set(this._userDelete);
        return this.dataService.delete(token); 

    }

    public uploadPhoto(photo: any){
        return this.dataService.upload(photo); 
    }
}
﻿import { Http, Response, Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DataService {

    public _pageSize: number;
    public _baseUri: string;

    constructor(public http: Http) {

    }

    set(baseUri: string, pageSize?: number): void {
        this._baseUri = baseUri;
        this._pageSize = pageSize;
    }

    get(page?: number) {
        
        if(page!=undefined){
            var uri = this._baseUri + page.toString() + '/' + this._pageSize.toString();
        }
        else
            var uri = this._baseUri;
        

        return this.http.get(uri)
            .map(response => (<Response>response));
    }

    post(data?: any, mapJson: boolean = true) {
        if (mapJson)
            return this.http.post(this._baseUri, data)
                .map(response => <any>(<Response>response).json());
        else
            return this.http.post(this._baseUri, data);
    }

    postAuthenticate(token:string, data?: any, mapJson: boolean = true){
        var headers = new Headers();
        headers.append("Authorization", "Bearer "+token)
        if (mapJson)
            return this.http.post(this._baseUri, data, {
                headers:headers
            })
                .map(response => <any>(<Response>response).json());
        else
            return this.http.post(this._baseUri, data, {
                headers:headers
            });
    }

    delete(id: number) {
        return this.http.delete(this._baseUri + '/' + id.toString())
            .map(response => <any>(<Response>response).json())
    }

    deleteResource(resource: string) {
        return this.http.delete(resource)
            .map(response => <any>(<Response>response).json())
    }
}
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  register(data: any): Promise<Response> {
    // return this.http.post<Response>('https://localhost:44305/auth/register', data).toPromise();
    return this.http.post<Response>('https://192.168.1.106:45461/auth/register', data).toPromise(); 
  }

  login(data: any): Promise<Response> {
    return this.http.post<Response>('https://192.168.1.106:45461/auth/login', data, {observe: 'response' as "body"}).toPromise();
  }

  edit(data: any): Promise<Response> {
    return this.http.patch<Response>('https://192.168.1.106:45461/auth/edit', data, {observe: 'response' as "body"}).toPromise();
  }

  incrementTours(data: any): Promise<Response> {
    return this.http.patch<Response>('https://192.168.1.106:45461/auth/incrementTours', data).toPromise();
  }

  incrementPlaces(data: any): Promise<Response> {
    return this.http.patch<Response>('https://192.168.1.106:45461/auth/incrementPlaces', data).toPromise();
  }

  refreshProfile(data: any): Promise<Response> {
    let params = new HttpParams().set("uid", data['userId']);
    return this.http.get<Response>('https://192.168.1.106:45461/auth/refreshProfile', {observe: 'response' as "body", params}).toPromise();
  }

}

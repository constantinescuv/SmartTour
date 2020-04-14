import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  register(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/auth/register', data).toPromise();
  }

  login(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/auth/login', data, {observe: 'response' as "body"}).toPromise();
  }

  edit(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/auth/edit', data, {observe: 'response' as "body"}).toPromise();
  }
}

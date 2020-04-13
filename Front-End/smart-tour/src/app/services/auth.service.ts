import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  register(data: any) {
    return this.http.post('https://localhost:44305/auth/register', data).toPromise();
  }

  login(data: any) {
    return this.http.post('https://localhost:44305/auth/login', data).toPromise();
  }
}

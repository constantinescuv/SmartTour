import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FeedService {

  constructor(private http: HttpClient) { }

  addPost(data: any): Promise<Response> {
    return this.http.post<Response>('https://192.168.1.106:45461/feed/addPost', data).toPromise();
  }

  getPosts(data: any): Promise<Response> {
    let params = new HttpParams().set("uid", data['userId']);
    return this.http.get<Response>('https://192.168.1.106:45461/feed/getPosts', {observe: 'response' as "body", params}).toPromise();

  }

}

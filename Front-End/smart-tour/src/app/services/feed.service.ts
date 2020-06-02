import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FeedService {

  constructor(private http: HttpClient) { }

  addPost(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/feed/addPost', data).toPromise();
  }

  getPosts(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/feed/getPosts', data, {observe: 'response' as "body"}).toPromise();
  }

}

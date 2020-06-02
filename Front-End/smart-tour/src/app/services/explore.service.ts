import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExploreService {

  constructor(private http: HttpClient) { }

  getPlaces(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/explore/getExploreList', data, {observe: 'response' as "body"}).toPromise();
  }
}
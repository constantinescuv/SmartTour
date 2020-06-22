import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExploreService {

  constructor(private http: HttpClient) { }

  getPlaces(data: any): Promise<Response> {
    let params = new HttpParams().set("Latitude", data['Latitude']).set("Longitude", data['Longitude']);
    return this.http.get<Response>('https://192.168.1.106:45461/explore/getExploreList', {observe: 'response' as "body", params}).toPromise();

  }
}
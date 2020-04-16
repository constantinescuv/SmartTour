import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TourService {

  constructor(private http: HttpClient) { }

  generateTour(data: any): Promise<Response> {
    return this.http.post<Response>('https://localhost:44305/tour/getTour', data, {observe: 'response' as "body"}).toPromise();
  }
}

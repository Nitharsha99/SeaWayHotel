import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Activity } from '../../Models/activity';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor( private http: HttpClient) {
   }

   baseUrl = "https://localhost:44353/api/Activity";

   GetActivities(): Observable<Activity[]>{
    return this.http.get<Activity[]>(this.baseUrl);
  }
}

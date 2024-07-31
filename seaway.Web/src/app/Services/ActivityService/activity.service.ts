import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Activity } from '../../Models/activity';
import { catchError, Observable, throwError } from 'rxjs';

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

  FindActivityById(id: number): Observable<Activity>{
    return this.http.get<Activity>(this.baseUrl + "/" + id);
  }

  PostActivity(data: any): Observable<any>{
    return this.http.post<any>(this.baseUrl, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  UpdateActivity(data: any, id: number): Observable<any>{
    return this.http.put<any>(this.baseUrl+ "/" + id, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  DeleteImages(ids: string[]){
    console.log("Deleting images:", ids);
    let params = new HttpParams();
    ids.forEach((id) => params = params.append('ids', id));
    return this.http.delete(`${this.baseUrl}/image`, { params, responseType: 'text'});
  }

}

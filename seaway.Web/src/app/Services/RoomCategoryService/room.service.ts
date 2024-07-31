import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { RoomCategory } from 'src/app/Models/roomCategory';

@Injectable({
  providedIn: 'root'
})
export class RoomCategoryService {

  constructor(private http: HttpClient) { }

  baseUrl = "https://localhost:44353/api/RoomCategory";

  GetAllRoomCategories(): Observable<RoomCategory[]>{
    return this.http.get<RoomCategory[]>(this.baseUrl);
  }

  FindRoomCategoryById(id: number): Observable<RoomCategory>{
    return this.http.get<RoomCategory>(this.baseUrl + "/" + id);
  }

  PostRoomCategory(data: any): Observable<any>{
    return this.http.post<any>(this.baseUrl, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  UpdateRoomCategory(data: any, id: number): Observable<any>{
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

  DeleteRoomCategory(id: number): Observable<any>{
    return this.http.delete<any>(this.baseUrl + "/" + id);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Room } from 'src/app/Models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  constructor(private http: HttpClient) { }

  baseUrl = "https://localhost:44353/api/Room";

  GetAllRooms(): Observable<Room[]>{
    return this.http.get<Room[]>(this.baseUrl);
  }

  FindRoomById(id: number): Observable<Room>{
    return this.http.get<Room>(this.baseUrl + "/" + id);
  }

  
  PostRoom(data: any): Observable<any>{
    return this.http.post<any>(this.baseUrl, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  UpdateRoom(data: any, id: number): Observable<any>{
    return this.http.put<any>(this.baseUrl+ "/" + id, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  DeleteRoom(id: number): Observable<any>{
    return this.http.delete<any>(this.baseUrl + "/" + id);
  }
}

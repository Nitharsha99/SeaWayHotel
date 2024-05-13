import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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
    console.log("save data", data);
    return this.http.post<any>(this.baseUrl, data);
  }

  UpdateRoom(data: any): Observable<any>{
    return this.http.put<any>(this.baseUrl, data);
  }

  DeleteImages(ids: string[]){
    console.log("Deleting images:", ids);
    const params = new HttpParams().set('ids', ids.join(','));
    return this.http.delete(`${this.baseUrl}/image`, { params });
  }

  DeleteRoom(id: number): Observable<any>{
    return this.http.delete<any>(this.baseUrl + "/" + id);
  }
}

import { HttpClient } from '@angular/common/http';
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

  PostRoom(data: any): Observable<any>{
    console.log("save data", data);
    return this.http.post<any>(this.baseUrl, data);
  }
}
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from 'src/app/Models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  constructor(private http: HttpClient) { }

  baseUrl = "https://localhost:44353/api/Activity";

  GetAllRooms(): Observable<Room>{
    return this.http.get<Room>(this.baseUrl);
  }
}

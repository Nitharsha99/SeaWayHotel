import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingList } from 'src/app/Models/bookings';

@Injectable({
  providedIn: 'root'
})
export class BookingsService {

  constructor(private http: HttpClient) { }

  baseUrl = "https://localhost:44353/api/Bookings";

  GetAllBookings(): Observable<BookingList[]>{
    return this.http.get<BookingList[]>(this.baseUrl);
  }
}

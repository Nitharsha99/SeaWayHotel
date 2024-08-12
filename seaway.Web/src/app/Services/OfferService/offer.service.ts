import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Offer } from 'src/app/Models/offer';

@Injectable({
  providedIn: 'root'
})
export class OfferService {

  constructor(private http: HttpClient) { }

  baseUrl = "https://localhost:44353/api/Offer";

  GetOffers(): Observable<Offer[]>{
    return this.http.get<Offer[]>(this.baseUrl);
  }

  FindOfferById(id: number): Observable<Offer>{
    return this.http.get<Offer>(this.baseUrl + "/" + id);
  }

  DeleteOffer(id: number): Observable<any>{
    return this.http.delete<any>(this.baseUrl + "/" + id);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
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

  PostOffer(data: any): Observable<any>{
    return this.http.post<any>(this.baseUrl, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  updateOffer(data: any, id: number): Observable<any>{
    return this.http.put<any>(this.baseUrl+ "/" + id, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  ChangeStatus(status: boolean, id: number): Observable<any>{
    return this.http.patch<any>(this.baseUrl, { isActive: status, offerId: id }).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }
}

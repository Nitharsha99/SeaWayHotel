import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Admin } from 'src/app/Models/admin';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  baseUrl = "https://localhost:44353/api/Admin";

  // login(data: any): Observable<boolean>{
  //   var res = this.http.post<boolean>(this.baseurl + "/Login", data);
  //   return res;
  // }

  getAdminList(): Observable<Admin[]>{
    return this.http.get<Admin[]>(this.baseUrl);
  }

  FindAdminById(id: number): Observable<Admin>{
    return this.http.get<Admin>(this.baseUrl + "/" + id);
  }

  PostAdmin(data: any): Observable<any>{
    return this.http.post<any>(this.baseUrl, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  UpdateAdmin(data: any, id: number): Observable<any>{
    return this.http.put<any>(this.baseUrl+ "/" + id, data).pipe(
      catchError((error: any) => {
        return throwError(error.error);
      })
    );
  }

  DeleteAdmin(id: number): Observable<any>{
    return this.http.delete<any>(this.baseUrl + "/" + id);
  }

}

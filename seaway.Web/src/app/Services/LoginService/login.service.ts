import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  baseurl = "https://localhost:44353/api/Login";

  login(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl, data);
  }
}

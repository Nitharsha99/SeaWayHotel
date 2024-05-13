import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  baseurl = "https://localhost:44353/api/admin";

  login(data: any): Observable<boolean>{
    var res = this.http.post<boolean>(this.baseurl + "/Login", data);
    return res;
  }
}

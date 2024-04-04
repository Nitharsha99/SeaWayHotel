import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CloudinaryService {

  constructor(private http: HttpClient) { }

  cloudUrl = "https://api.cloudinary.com/v1_1/dly7yjg1w";

  uploadImage(values: any):Observable<any>{
    let data = values;
    return this.http.post(this.cloudUrl + '/image/upload', data);
  }
}

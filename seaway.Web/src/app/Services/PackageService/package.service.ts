import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Package } from 'src/app/Models/package';

@Injectable({
  providedIn: 'root'
})
export class PackageService {
  private apiUrl = '"https://localhost:44353/api/Package"';

  constructor(private http: HttpClient) { }

  // Get all packages
  getAllPackages(): Observable<Package[]> {
    return this.http.get<Package[]>(this.apiUrl);
  }

  // Get package by ID
  getPackageById(id: number): Observable<Package> {
    return this.http.get<Package>(`${this.apiUrl}/${id}`);
  }

  // Create a new package
  createPackage(pkg: Package): Observable<Package> {
    return this.http.post<Package>(this.apiUrl, pkg);
  }

  // Update an existing package
  updatePackage(id: number, pkg: Package): Observable<Package> {
    return this.http.put<Package>(`${this.apiUrl}/${id}`, pkg);
  }

  // Delete a package
  deletePackage(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

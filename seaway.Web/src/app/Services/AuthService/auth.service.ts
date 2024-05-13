import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private storageKey = 'currentUser';
  private currentUser: any;
  private currentUserChanged = new Subject<any>();

  constructor() { 
    const storedUser = localStorage.getItem(this.storageKey);
    if (storedUser) {
      this.currentUser = JSON.parse(storedUser);
    }
  }

  login(username: string, password: string): void {
    const user = { username, password };
    localStorage.setItem(this.storageKey, JSON.stringify(user));
    this.currentUser = user;
    this.currentUserChanged.next(this.currentUser);
    console.log("login", this.currentUserChanged);
  }

  getCurrentUser(): any {
    return this.currentUser;
  }

  onCurrentUserChanged(): Observable<any> {
    return this.currentUserChanged.asObservable();
  }

}

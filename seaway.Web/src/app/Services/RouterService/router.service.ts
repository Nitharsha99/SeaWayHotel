import { Injectable } from '@angular/core';
import { NavigationEnd, Router, Event } from '@angular/router';
import { BehaviorSubject, Observable, filter } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RouterService {

  private _showHeaderFooter: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private _adminHeaderFooter: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  showHeaderFooter$ = this._showHeaderFooter.asObservable();
  adminHeaderFooter$ = this._adminHeaderFooter.asObservable(); 

  constructor(private router: Router) { 
    this.router.events.pipe(
      filter((event: Event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      const showHeaderFooter = !event.url.includes('/Administration');
      const adminHeaderFooter$ = event.url.includes('/Administration/')
      console.log("checking", showHeaderFooter, event.url, '/Administration');
      this._showHeaderFooter.next(showHeaderFooter);
      this._adminHeaderFooter.next(adminHeaderFooter$);
    });
  }

}



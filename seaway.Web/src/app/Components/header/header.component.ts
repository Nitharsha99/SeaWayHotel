import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterService } from 'src/app/Services/RouterService/router.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{
  canDisplay: boolean = true;
  adminHeaderFooter: boolean = false;
  passwordHeaderFooter: boolean = false;
  isDropdownOpen = false;

  constructor(private routerService: RouterService, private router: Router, private route: ActivatedRoute){
  }

  ngOnInit(): void {
    this.routerService.showHeaderFooter$.subscribe(showHeaderFooter => {
      this.canDisplay = showHeaderFooter;
    });

    this.routerService.adminHeaderFooter$.subscribe(adminHeaderFooter => {
      this.adminHeaderFooter = adminHeaderFooter;
    });

    this.routerService.passwordHeaderFooter$.subscribe(passwordHeaderFooter => {
      this.passwordHeaderFooter = passwordHeaderFooter;
    });
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  logout(){
    localStorage.removeItem('token');
    this.router.navigate(['/Administration']);
  }

}

import { Component, OnInit } from '@angular/core';
import { RouterService } from 'src/app/Services/RouterService/router.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{
  canDisplay: boolean = true;
  adminHeaderFooter: boolean = false;
  isDropdownOpen = false;

  constructor(private routerService: RouterService){
  }

  ngOnInit(): void {
    this.routerService.showHeaderFooter$.subscribe(showHeaderFooter => {
      this.canDisplay = showHeaderFooter;
    });

    this.routerService.adminHeaderFooter$.subscribe(adminHeaderFooter => {
      this.adminHeaderFooter = adminHeaderFooter;
    });
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

}

import { Component, OnInit } from '@angular/core';
import { RouterService } from 'src/app/Services/RouterService/router.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit{

  canDisplay: boolean = true;

  constructor(private routerService: RouterService){
  }

  ngOnInit(): void {
    this.routerService.showHeaderFooter$.subscribe(showHeaderFooter => {
      this.canDisplay = showHeaderFooter;
      console.log("router:", showHeaderFooter, "store:", this.canDisplay);
    });
    
  }

}

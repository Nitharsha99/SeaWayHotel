import { Component, OnInit } from '@angular/core';
import { Admin } from 'src/app/Models/admin';

@Component({
  selector: 'app-all-managers',
  templateUrl: './all-managers.component.html',
  styleUrls: ['./all-managers.component.css']
})
export class AllManagersComponent implements OnInit{

  pageSize: number = 5;
  page: number = 1;
  managers: Admin[] = [];
  filteredManagers: Admin[] = [];
  search: string = '';

  constructor(){}

  ngOnInit(): void {
    
  }

  onSearch(): void{

  }

  navigateToAddManager(): void{

  }

  onPageChange(event: any): void{
  
  }

  navigateToUpdatePage(): void{

  }

  deleteManager(): void{

  }
}

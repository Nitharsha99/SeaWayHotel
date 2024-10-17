import { Component, OnInit } from '@angular/core';
import { Admin } from 'src/app/Models/admin';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from 'src/app/Services/AdminService/admin.service';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import Swal from 'sweetalert2';

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
  totalItems: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, 
              private adminService: AdminService, private commonFunction: CommonFunctionComponent
  ){}

  ngOnInit(): void {
    this.adminService.getAdminList().subscribe(res => {
      if(res != null){
        res.forEach(item => {
          item.picPath = this.commonFunction.convertBase64ToString(item.picPath);
        });
        this.managers = this.filteredManagers = res;
        this.totalItems = this.filteredManagers.length;
      }
    });
    //this.updateDisplayeditems();
  }

  onSearch(): void{
    this.filteredManagers = this.managers.filter(m => {
      return m.username.toLowerCase().includes(this.search.toLowerCase());
    });
    this.totalItems = this.filteredManagers.length;
    this.updateDisplayeditems();
  }

  navigateToAddManager(): void{
    this.router.navigate(['addManager'], {relativeTo: this.route});
  }

  onPageChange(event: number):void {
    this.page = event;
  }

  navigateToUpdatePage(id: number): void{
    this.router.navigate(['editManager', id], {relativeTo: this.route});
  }

  deleteManager(id: number): void{
    this.commonFunction.showDeleteNotification().then((result: { isConfirmed: any; dismiss: Swal.DismissReason; }) => {
      if (result.isConfirmed) {
       this.adminService.DeleteAdmin(id).subscribe(res => {
        Swal.fire({
          icon: "success",
          title: "Successfully Deleted " + res.username + " !!! ",
          showConfirmButton: true,
          iconColor: '#570254',
          confirmButtonColor: '#570254'
        }).then(() => {
          setTimeout(() => {
            window.location.reload();
          });
        });
       });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        setTimeout(() => {
          window.location.reload();
        });
      }
  });
  }

  
  updateDisplayeditems(): void {
    this.page =1;
    this.pageSize = 5;
    this.totalItems = this.filteredManagers.length;
  }
}

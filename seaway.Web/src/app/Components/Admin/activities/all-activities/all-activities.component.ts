import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { Activity } from 'src/app/Models/activity';
import { ActivityService } from 'src/app/Services/ActivityService/activity.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-all-activities',
  templateUrl: './all-activities.component.html',
  styleUrls: ['./all-activities.component.css']
})
export class AllActivitiesComponent implements OnInit{

  pageSize: number = 5;
  page: number = 1;
  activities: Activity[] = [];
  filteredActivities: Activity[] = [];
  search: string = '';

  constructor(private router: Router, private route: ActivatedRoute, 
    private activityService: ActivityService, private commonFunction: CommonFunctionComponent){
  }

  ngOnInit(): void {
    this.activityService.GetActivities().subscribe(res => {
      this.activities = res;
      this.filteredActivities = this.activities;
    })
  }

  navigateToAddActivity(): void{
    this.router.navigate(['addActivity'], {relativeTo: this.route});
  }

  
  onPageChange(event: number):void {
    this.page = event;
  }

  onSearch(): void{
    this.filteredActivities = this.activities.filter(act => {
      return act.activityName.toLowerCase().includes(this.search.toLowerCase());
    })
  }

  deleteActivity(id: number){
    this.commonFunction.showDeleteNotification().then((result: { isConfirmed: any; dismiss: Swal.DismissReason; }) => {
      if (result.isConfirmed) {
       this.activityService.DeleteActivity(id).subscribe(res => {
        Swal.fire({
          icon: "success",
          title: "Successfully Deleted " + res.activityName + " !!! ",
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

  navigateToUpdatePage(id: number): void{
    this.router.navigate(['editActivity', id], {relativeTo: this.route});
  }

  onChange(act: any){
    Swal.fire({
      text: `Do you want to ${act.isActive ? 'activate' : 'deactivate'}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      iconColor: '#570254',
      confirmButtonColor: '#570254',
      didOpen: () => {
        const cancelButton = document.querySelectorAll('.swal2-cancel')[0] as HTMLElement;
        cancelButton.style.backgroundColor = '#fff';
        cancelButton.style.border = '1px solid #570254';
        cancelButton.style.color = '#570254';

        // Add hover style
        cancelButton.addEventListener('mouseover', () => {
          cancelButton.style.backgroundColor = '#f8edf7'; // Change background color on hover
          cancelButton.style.borderColor = '#570254'; // Change border color on hover
          cancelButton.style.cursor = 'pointer'; // Change cursor on hover
        });

        cancelButton.addEventListener('mouseout', () => {
          cancelButton.style.backgroundColor = '#fff'; // Reset background color on mouse out
          cancelButton.style.borderColor = '#570254';
        });
      }
    }).then((result) => {
      if (result.isConfirmed) {
        this.changeStatus(act.isActive, act.activityId);
      } else {
        window.location.reload();
      }
    });
  }

  changeStatus(status: boolean, id: number): void{
    this.activityService.ChangeStatus(status, id).subscribe(() => {
      setTimeout(() => {
        window.location.reload();
      });
    });
  }
  

}

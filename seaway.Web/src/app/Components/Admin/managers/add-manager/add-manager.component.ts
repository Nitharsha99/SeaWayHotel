import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-manager',
  templateUrl: './add-manager.component.html',
  styleUrls: ['./add-manager.component.css']
})
export class AddManagerComponent implements OnInit{

  updateMode: boolean = false;
  profilePic: string = "../../../../../assets/images/profile.jpg";

  constructor(private builder: FormBuilder, private router: Router,  private route: ActivatedRoute){}

  managerForm: FormGroup = this.builder.group({
    username:[''],
    password:[''],
    createdBy: ['Nitharsha'],
    updatedBy: [''],
    picName: [null],
    picValue: [null],
    cloudinaryPublicId: [null]
  });

  ngOnInit(): void {
    
  }

  onSelectProfilePic(event: any): void{

  }

  redirectToBack(): void{

  }

  resetForm(): void{

  }

  addManager(): void{

  }
}

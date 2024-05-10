import { Component, OnInit } from '@angular/core';
import { CloudinaryService } from 'src/app/Services/CloudinaryService/cloudinary.service';
import { Buffer } from 'buffer';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit{

  files: File[] = [];
  image: string = '';
  imageVisible: boolean = false;

  constructor(private cloudinaryService: CloudinaryService){

  }

  ngOnInit(): void {
    this.convertBase64ToString();
  }

  onSelect(event: any){
    console.log(event);
    this.files.push(...event.addedFiles);
  }

  onRemove(event: any){
    this.files.splice(this.files.indexOf(event), 1);
    console.log(this.files);
  }

  uploadImage(){
    if(!this.files[0]){
      alert("No Image Selected Yet!!");
    }

    for(let i = 0; i < this.files.length; i++){
      const file_data = this.files[i];
      const data = new FormData();
      data.append('file', file_data);
      data.append('upload_preset', 'seaway_hotel');
      data.append('cloud_name', 'dly7yjg1w');

      this.cloudinaryService.uploadImage(data).subscribe(res => {
        console.log(res);
        console.log('url',res.url);
        const hexVal = Buffer.from(res.url, 'utf-8').toString('hex');
        console.log('value', hexVal);
      })
    }

    this.files = [];
  }

  convertBase64ToString(){
    const base64 = 'aHR0cDovL3Jlcy5jbG91ZGluYXJ5LmNvbS9kbHk3eWpnMXcvaW1hZ2UvdXBsb2FkL3YxNzExNTIzNzc3L1NlYXdheS9pbWxwOXhqYzQ4eHJnZG9vaHppai5wbmc=';

    const decodedString = atob(base64);
    console.log(decodedString);

    this.image = decodedString;
    console.log(this.image, 'immmm');
    this.imageVisible = true;
  }
}

import { Component } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-common-function',
  template: '<p>common function component</p>'
})

export class CommonFunctionComponent{

    convertBase64ToString(base64: string){
        const decodedString = atob(base64);
      
        return decodedString;
      }

      showLoadingNotification() {
        Swal.fire({
          title: 'Loading...',
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          }
        });
      }
}
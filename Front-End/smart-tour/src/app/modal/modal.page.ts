import { Component, OnInit } from '@angular/core';
import { NavParams, ModalController } from '@ionic/angular';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.page.html',
  styleUrls: ['./modal.page.scss'],
})
export class ModalPage implements OnInit {

  information: any[];
  
  automaticClose = false;

  constructor(private modalController: ModalController, private http: HttpClient) {
    this.http.get('assets/tourConfig.json').subscribe(res => {
      this.information = res['items'];

      this.information[0].open = true; //open
    });
   }

   toggleSection(index) {
     this.information[index].open = !this.information[index].open;

     if (this.automaticClose && this.information[index].open) {
       this.information.filter((item, itemIndex) => itemIndex != index).map(item => item.open = false);
     }
   }

   toggleItem(index, childIndex) {
     this.information[index].children[childIndex].open = !this.information[index].children[childIndex].open;
   }

  ngOnInit() {
    
  }

  async closeModal() {
    await this.modalController.dismiss(this.information);
  }

}

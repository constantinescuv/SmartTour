import { Component, OnInit, Input } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss'],
})
export class DetailComponent implements OnInit {

  @Input() tour: any;
  slideOpts = {
    initialSlide: 0,
    speed: 400
  };
  days: any;
  date = '';

  constructor(private modalController: ModalController, private http: HttpClient) {
    this.http.get('assets/date.json').subscribe(res => {
      this.days = res['days'];
      var day = this.tour.date.match(/(\d+)/)[0];
      var month = this.tour.date.replace(/[0-9]/g, '');
      this.date += this.days[day] + ' of ' + month;
      console.log(this.tour);
    });
   }

  ngOnInit() {
  }

  async closeModal() {
    await this.modalController.dismiss();
  }

}

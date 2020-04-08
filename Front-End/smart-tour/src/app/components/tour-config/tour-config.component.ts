import { Component, OnInit, Input } from '@angular/core';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-tour-config',
  templateUrl: './tour-config.component.html',
  styleUrls: ['./tour-config.component.scss'],
})
export class TourConfigComponent implements OnInit {

  @Input('tourConfig') tourConfig: any;

  constructor(private toastCtrl: ToastController) { }

  ngOnInit() {}

  async buyItem(tourConfig) {
    let toast = await this.toastCtrl.create({
      message: `${tourConfig.name} set`
    });
    toast.present();
  }

}

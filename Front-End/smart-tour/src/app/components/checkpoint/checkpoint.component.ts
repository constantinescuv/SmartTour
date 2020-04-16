import { Component, OnInit, Input } from '@angular/core';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-checkpoint',
  templateUrl: './checkpoint.component.html',
  styleUrls: ['./checkpoint.component.scss'],
})
export class CheckpointComponent implements OnInit {

  @Input('checkpoint') checkpoint: any;

  constructor(private toastController: ToastController) { }

  ngOnInit() {}

  async expand(checkpoint) {
    let toast = await this.toastController.create({
      message: `expanded`
    });
    toast.present();
  }

}

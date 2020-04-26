import { Component, OnInit } from '@angular/core';
import { AlertController, NavController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-tour',
  templateUrl: './tour.page.html',
  styleUrls: ['./tour.page.scss'],
})
export class TourPage implements OnInit {

  tour: any;
  automaticClose = true;

  constructor(private alertController: AlertController, private navController: NavController, private toastController: ToastController) { 
    this.tour = JSON.parse(localStorage.getItem('tour'))['result']['tour'];

  }

  ngOnInit() {
  }

  toggleSection(index) {
    this.tour[index].open = !this.tour[index].open;

    if (this.automaticClose && this.tour[index].open) {
      this.tour.filter((item, itemIndex) => itemIndex != index).map(item => item.open = false);
    }
  }

  toggleItem(index, childIndex) {
    this.tour[index].children[childIndex].open = !this.tour[index].children[childIndex].open;
  }
  
  async presentAlert() {
    const alert = await this.alertController.create({
      header: 'Close Tour',
      message: 'You will not be able to return to this tour if you decide to close it and all progress will be lost. Are you sure?',
      buttons: [
        { text: 'Yes', handler: async () => {
          const toast = await this.toastController.create({
            message: 'Tour cancelled!',
            color: 'danger',
            duration: 2000
          });
  
          toast.present();

          this.navController.navigateForward(['home/start'], { animated: false });
        }}, 
        'No']
    });

    await alert.present();
  }

}

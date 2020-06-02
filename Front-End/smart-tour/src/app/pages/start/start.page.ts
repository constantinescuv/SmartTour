import { Component } from '@angular/core';
import { Capacitor, Plugins, GeolocationPosition } from '@capacitor/core';
import { Observable, of, from as fromPromise } from 'rxjs';
import { tap, map, switchMap } from 'rxjs/operators';

import { LoadingController, AlertController, ModalController, NavController, ToastController } from '@ionic/angular';
import { ModalPage } from '../../modal/modal.page';

const { Toast, Geolocation } = Capacitor.Plugins;

@Component({
  selector: 'app-start',
  templateUrl: './start.page.html',
  styleUrls: ['./start.page.scss'],
})
export class StartPage {

  public coordinates: Observable<GeolocationPosition>;
  public defaultPos: { latitude: 42, longitude: 42 };

  constructor(public loading: LoadingController, 
    public alertCtrl: AlertController, 
    private modalCtrl: ModalController,
    private navController: NavController,
    private toastController: ToastController){

  }

  public data: string;

  ngOnInit() {
    if(!localStorage.getItem('user')) {
      this.navController.navigateForward(['login'], { animated: false });
      this.notLoggedInToast();
    }
    else {
      if(localStorage.getItem('tour') != null) {
        this.navController.navigateForward(['tour'], { animated: false });
      }
      //start the loader
      this.displayLoader()
      .then((loader: any) => {
        //get position
        return this.getCurrentPosition()
          .then(position => {
            //close the loader + return position
            loader.dismiss();
            return position;
          })
          .catch(err => {
            //close loader + return Null
            loader.dismiss();
            return null;
          });
      });
    }
  }

  async notLoggedInToast() {
    const toast = await this.toastController.create({
      message: 'You are not logged in!',
      color: 'danger',
      duration: 2000
    });

    toast.present();
  }

  async openModal() {
    const modal = await this.modalCtrl.create({
      component: ModalPage,
      // componentProps: {
      //   send_data: this.send_data
      // }
    });
    modal.onWillDismiss().then(dataReturned => {
      this.data = dataReturned.data;
    })
    return await modal.present();
  }

  async displayLoader() {
    const loading = await this.loading.create({
      message: 'Loading'
    });
    await loading.present();
    return loading;
  }

  private async presentAlert(message: string): Promise<HTMLIonAlertElement>{
    const alert = await this.alertCtrl.create({
      header: 'Alert',
      message: message,
      buttons: ['OK']
    });
    await alert.present();
    return alert;
  }

  private async getCurrentPosition(): Promise<any> {
    const isAvailable: boolean = Capacitor.isPluginAvailable('Geolocation');
    if (!isAvailable) {
      console.log('Err: plugin is not available');
      return of(new Error('Err: plugin is not available'));
    }
    const POSITION = Plugins.Geolocation.getCurrentPosition()
    .catch(err => {
      console.log('ERR', err);
      return new Error(err.message || 'msg');
    });
    this.coordinates = fromPromise(POSITION).pipe(
      switchMap((data: any) => of(data.coords)),
      tap(data => this.setCoord(data))
    );
    
    return POSITION;
  }

  private setCoord(data)
  {
    localStorage.setItem('Latitude', data.latitude);
    localStorage.setItem('Longitude', data.longitude);
  }


}

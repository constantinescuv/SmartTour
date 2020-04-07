import { Component } from '@angular/core';
import { Capacitor, Plugins, GeolocationPosition } from '@capacitor/core';
import { Observable, of, from as fromPromise } from 'rxjs';
import { tap, map, switchMap } from 'rxjs/operators';

import { LoadingController, AlertController } from '@ionic/angular';

const { Toast, Geolocation } = Capacitor.Plugins;

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss']
})
export class Tab2Page {

  public coordinates: Observable<GeolocationPosition>;
  public defaultPos: { latitude: 42, longitude: 42 };

  constructor(public loading: LoadingController, public alertCtrl: AlertController){

  }

  ngOnInit() {
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
      tap(data => console.log(data))
    );
    return POSITION;
  }
  

}

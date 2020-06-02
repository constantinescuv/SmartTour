import { Component, OnInit } from '@angular/core';
import { NavController, ToastController, LoadingController } from '@ionic/angular';
import { GeolocationPosition, Plugins, Capacitor } from '@capacitor/core';
import { Observable, of, from as fromPromise } from 'rxjs';
import { switchMap, tap, map } from 'rxjs/operators';
import { ExploreService } from 'src/app/services/explore.service';

const { Toast, Geolocation } = Capacitor.Plugins;

@Component({
  selector: 'app-explore',
  templateUrl: './explore.page.html',
  styleUrls: ['./explore.page.scss'],
})
export class ExplorePage {
  public coordinates: Observable<GeolocationPosition>;
  public defaultPos: { latitude: 42, longitude: 42 };

  exploreList: any;

  constructor(public loading: LoadingController, private navController: NavController, private toastController: ToastController, private exploreService: ExploreService) { }

  ngOnInit() {
    if(!localStorage.getItem('user')) {
      this.navController.navigateForward(['login'], { animated: false });
      this.notLoggedInToast();
    }
    else {
      if(localStorage.getItem('tour') != null) {
        this.navController.navigateForward(['tour'], { animated: false });
      }
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

  async displayLoader() {
    const loading = await this.loading.create({
      message: 'Loading'
    });
    await loading.present();
    return loading;
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
      tap(data => this.explore(data))
    );
    
    return POSITION;
  }

  private async explore(data){
    this.setCoord(data);
    var res = await this.exploreService.getPlaces({
      'Latitude': data.latitude.toString(),
      'Longitude': data.longitude.toString()
    });

    var l = res.body['attractions'];

    for (var i = 0; i < res.body['restaurants'].length; i++) {
      var index = Math.floor(Math.random() * l.length) + 1;
      l.splice(index, 0, res.body['restaurants'][i])
    }
    this.exploreList = l;
  }

  private setCoord(data)
  {
    localStorage.setItem('Latitude', data.latitude);
    localStorage.setItem('Longitude', data.longitude);
  }

  async savePlace(index) {
    if (localStorage.getItem('saveList')) {
      var saveList = JSON.parse(localStorage.getItem('saveList'));
      var found = false;
      for (var place in saveList) {
        if (saveList[place]['name'] == this.exploreList[index].name) {
          found = true;
        }
      }
      if (found == true) {
        const toast = await this.toastController.create({
          message: 'Place is already saved!',
          color: 'danger',
          duration: 2000
        });
  
        toast.present();

      }
      else {
        saveList.push(this.exploreList[index].name);
        localStorage.setItem('saveList', JSON.stringify(saveList));
  
        const toast = await this.toastController.create({
          message: 'Place saved!',
          color: 'success',
          duration: 2000
        });
  
        toast.present();
      }
    } else {
      var l = [];
      l.push(this.exploreList[index].name);
      localStorage.setItem('saveList', JSON.stringify(l));

      const toast = await this.toastController.create({
        message: 'Place saved!',
        color: 'success',
        duration: 2000
      });
  
      toast.present();
    }
  }
}

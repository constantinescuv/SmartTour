import { Component } from '@angular/core';
import { NavController, ToastController, LoadingController } from '@ionic/angular';
import { ExploreService } from 'src/app/services/explore.service';
import { Geolocation } from '@ionic-native/geolocation/ngx';
import { PromotedService } from 'src/app/services/promoted.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-explore',
  templateUrl: './explore.page.html',
  styleUrls: ['./explore.page.scss'],
})
export class ExplorePage {
  public defaultPos: { latitude: 42, longitude: 42 };

  exploreList: any;
  promotedList: any;
  promotedType = {};

  constructor(public loading: LoadingController, private navController: NavController, private toastController: ToastController, private exploreService: ExploreService, public geolocation: Geolocation, private promotedService: PromotedService) { }

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
          return this.geolocation.getCurrentPosition({
            timeout: 10000,
            enableHighAccuracy: false
          })
            .then(async position => {
              //close the loader + return position
              await this.explore(position.coords);
              this.promotedService.getPromotedPlaces().snapshotChanges().pipe(
                map(changes =>
                  changes.map(c =>
                    ({ key: c.payload.doc.id, ...c.payload.doc.data() })
                  )
                )
              ).subscribe(promotions => {
                this.promotedList = promotions;
                for (var i = 0; i < promotions.length; i++) {
                  this.promotedType[promotions[i].placeName] = promotions[i].tierType;
                }
                for (var i = 0; i < this.exploreList.length; i++) {
                  var aux = this.exploreList[i];
                  if (this.promotedType[aux.name]) {
                    this.exploreList.splice(i, 1);
                    this.exploreList.splice(0, 0, aux);
                  } 
                }
              });
              loader.dismiss();
              return position;
            })
            .catch(err => {
              console.log('got here error');
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
        if (saveList[place] == this.exploreList[index].name) {
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

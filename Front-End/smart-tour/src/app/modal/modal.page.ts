import { Component, OnInit } from '@angular/core';
import { ModalController, ToastController, NavController, LoadingController } from '@ionic/angular';
import { HttpClient } from '@angular/common/http';
import { TourService } from '../services/tour.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.page.html',
  styleUrls: ['./modal.page.scss'],
})
export class ModalPage implements OnInit {

  information: any[];
  
  automaticClose = true;

  resetPlaces: boolean;
  postData = {
    Transport: '',
    TimeRange: '',
    DistanceRange: '',
    Latitude: '',
    Longitude: '',
    EatingBreak: false,
    savedPlaces: ''
  };
  private loading;

  constructor(private modalController: ModalController, 
    private http: HttpClient, 
    private toastController: ToastController,
    private tourService: TourService,
    private navController: NavController,
    private loadingCtrl: LoadingController) 
    {
    this.http.get('assets/tourConfig.json').subscribe(res => {
      this.information = res['items'];

      this.information[0].open = true; //open
    });
   }

  ngOnInit() {
    
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

  async configureTour() {

    this.loadingCtrl.create({
      message: 'Generating Tour...'
    }).then((overlay) => {
      this.loading = overlay;
      this.loading.present();
    });

    if (localStorage.getItem("Latitude") === null || localStorage.getItem("Longitude") === null)
    {
      this.loading.dismiss();
      const toast = await this.toastController.create({
        message: 'Error! No location data..',
        color: 'danger',
        duration: 2000
      });

      toast.present();
    }
    else if (localStorage.getItem("Transport") === null || localStorage.getItem("TimeRange") === null || localStorage.getItem("DistanceRange") === null) 
    {
      this.loading.dismiss();
      const toast = await this.toastController.create({
        message: 'Error! Reconfigure tour details..',
        color: 'danger',
        duration: 2000
      });

      toast.present();     
    }
    else 
    {
      this.postData.savedPlaces = localStorage.getItem('saveList');
      this.postData.DistanceRange = localStorage.getItem('DistanceRange');
      this.postData.TimeRange = localStorage.getItem('TimeRange');
      this.postData.Transport = localStorage.getItem('Transport');
      this.postData.Latitude = localStorage.getItem('Latitude');
      this.postData.Longitude = localStorage.getItem('Longitude');
      
      if (parseInt(this.postData.TimeRange.match(/(\d+)/)[0]) * 60 > 240) {
        this.postData.EatingBreak = localStorage.getItem('EatingBreak') == 'true';
      }
      else this.postData.EatingBreak = false;
      try { 
        const res = await this.tourService.generateTour(this.postData);
  
        this.loading.dismiss();

        const toast = await this.toastController.create({
          message: 'Tour created succesfully!',
          color: 'success',
          duration: 2000
        });
  
        toast.present();
  
        localStorage.setItem('tour', JSON.stringify(res.body));
        
        this.navController.navigateForward(['tour'], { animated: false });

        this.closeModal();
      } catch {
        this.loading.dismiss();
        const toast = await this.toastController.create({
          message: 'Error! Something went wrong with the tour generation..',
          color: 'danger',
          duration: 2000
        });
  
        toast.present();
      }
    }
    
  }

  async closeModal() {
    await this.modalController.dismiss(this.information);
  }


}

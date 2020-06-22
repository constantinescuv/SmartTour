import { Injectable } from '@angular/core';
import { AngularFirestoreCollection, AngularFirestore } from 'angularfire2/firestore';
import { PromotedPlace } from '../models/promotedPlace.model';

@Injectable({
  providedIn: 'root'
})
export class PromotedService {
  private dbPath = '/promoted';

  promotedRef: AngularFirestoreCollection<PromotedPlace> = null;

  constructor(private db: AngularFirestore) {
    this.promotedRef = this.db.collection(this.dbPath);
   }

  getPromotedPlaces(): AngularFirestoreCollection<PromotedPlace> {
    return this.promotedRef;
  }
}

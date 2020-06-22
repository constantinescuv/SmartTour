export class PromotedPlace {
    placeName: string;
    tierType: string;
    
    constructor(obj: any = null) {
      if (obj != null) {
        Object.assign(this, obj);
      }
    }
  }
  
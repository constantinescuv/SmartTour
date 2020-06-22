using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTour.Domain
{
    public class TourModel
    {
        public IEnumerable<PlaceEntity> Tour { set; get; }
        public IEnumerable<RestaurantEntity> Restaurants { set; get; }
        public double Distance { set; get; }
        public double Time { set; get; }

        public TourModel(IEnumerable<PlaceEntity> placeEntities)
        {
            Tour = placeEntities;
        }

        public TourModel()
        {
        }

        public void EditTour(Tuple<double, int, List<int>> TSPResult)
        {
            List<PlaceEntity> newTour = new List<PlaceEntity>();
            foreach (int i in TSPResult.Item3)
            {
                newTour.Add(Tour.ElementAt(i));
            }
            Tour = newTour;
            Distance = TSPResult.Item1;
            Time = TSPResult.Item2;
        }

        public TourModel GetNRandomPlaces(int maxTime, int[] timeList, WeatherEntity weather, SubtypeDataEntity subtypeData, bool addRestaurant, string savedPlaces)
        {
            List<PlaceEntity> newTourList = new List<PlaceEntity>();

            bool addedRestaurant = false;

            if (savedPlaces != null)
            {
                var places = new List<string>(savedPlaces.Split(','));
                for (int i = 0; i < places.Count(); i++)
                {
                    places[i] = places[i].Trim('"', '[', ']');
                }
                foreach (string name in places)
                {
                    PlaceEntity place = Tour.Where(i => i.Name == name).FirstOrDefault();
                    if (place != null)
                    {
                        newTourList.Add(place);
                        maxTime -= 45;
                        Tour = Tour.Where(u => u.Name != place.Name).ToList();
                    }
                }
                if (addedRestaurant)
                {
                    bool found = false;
                    RestaurantEntity restaurant = new RestaurantEntity();
                    foreach (string name in places)
                    {
                        restaurant = Restaurants.Where(i => i.Name == name).FirstOrDefault();
                        if (restaurant != null)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        newTourList.Add(restaurant);
                        Restaurants = Restaurants.Where(u => u.Name != restaurant.Name).ToList();
                        addedRestaurant = true;
                    }
                }
                
            }

            if (!addedRestaurant)
            {
                if (addRestaurant)
                {
                    double[] restaurantRatings = new double[Restaurants.Count()];
                    for (int i = 0; i < Restaurants.Count(); i++)
                    {
                        restaurantRatings[i] = double.Parse(Tour.ElementAt(i).Rating, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    int restaurant_index = GetRandomRestaurantIndex(restaurantRatings);

                    newTourList.Add(Restaurants.ElementAt(restaurant_index));

                    Restaurants = Restaurants.Where(u => u.Name != Restaurants.ElementAt(restaurant_index).Name).ToList();
                }
            }

            double[] ratings = new double[Tour.Count()];

            for (int i = 0; i < Tour.Count(); i++)
            {
                if (Tour.ElementAt(i).Rating != null)
                    ratings[i] = double.Parse(Tour.ElementAt(i).Rating, System.Globalization.CultureInfo.InvariantCulture);
                else
                    ratings[i] = 3;
            }

            List<int> index_list = GetRandomIndexList(ratings, timeList, maxTime, weather, subtypeData);

            foreach (int i in index_list)
            {
                newTourList.Add(Tour.ElementAt(i));
            }

            

            int[] index_array = index_list.ToArray();

            foreach (int i in index_array)
            {
                Tour = Tour.Where(u => u.Name != Tour.ElementAt(i).Name).ToList();

                for (int j = 0; j < index_array.Length; j++)
                    if (index_array[j] > i)
                        index_array[j] -= 1;                 
            }
       
            return new TourModel(newTourList);           
        }

        private int GetRandomRestaurantIndex(double[] ratings)
        {
            double total_score = 0;

            for (int i = 0; i < ratings.Length; i++)
                total_score += ratings[i];

            for (int i = 0; i < ratings.Length; i++)
                ratings[i] = ratings[i] / total_score;

            Random random = new Random();
            double n;
            
            double ratings_prob = 0;

            n = random.NextDouble();

            int rand = 0;

            for (int i = 0; i < ratings.Length; i++)
            {
                ratings_prob += ratings[i];
                if (ratings_prob > n)
                    break;
                else
                    rand = i + 1;
            }

            return rand;
        }

        // Factors taken into consideration when generating these indexes: - TripAdvisor ratings
        //                                                                 - Time and type of places (for each type of attraction selected, the probability of selecting attractions of the same type is slightly lowered)
        //                                                                 - Weather (temperature + humidity)
        //                                                                 - Subtype 
        private List<int> GetRandomIndexList(double [] ratings, int [] timeList, int maxTime, WeatherEntity weather, SubtypeDataEntity subtypeData)
        {
            List<int> index_list = new List<int>();

            double total_score = 0;

            for (int i = 0; i < ratings.Length; i++)
                total_score += ratings[i];

            for (int i = 0; i < ratings.Length; i++)
                ratings[i] = ratings[i] / total_score;

            Random random = new Random();
            double n;
            int timeCounter = 0;

            if (timeList.Sum() < maxTime)
                return Enumerable.Range(0, ratings.Length).ToList();

            double temp = weather.Main["temp"] - 273.15;
            double humidity = weather.Main["humidity"];

            List<int> outdoorIndexList = getOutdoorIndexList(subtypeData);
            (List<int> hotIndexList, List<int> coldIndexList) = getSeasonIndexLists(subtypeData);

            if (temp > 28)
            {
                ratings = AdjustProbability(ratings, hotIndexList.ToArray(), 1.5);
                ratings = AdjustProbability(ratings, coldIndexList.ToArray(), 0.2);
            }
            else if (temp < 10)
            {
                ratings = AdjustProbability(ratings, coldIndexList.ToArray(), 1.5);
                ratings = AdjustProbability(ratings, hotIndexList.ToArray(), 0.2);
            }

            if (humidity <= 30)
                ratings = AdjustProbability(ratings, outdoorIndexList.ToArray(), 1.2);
            else if (humidity <= 70)
                ratings = AdjustProbability(ratings, outdoorIndexList.ToArray(), 0.7);
            else
                ratings = AdjustProbability(ratings, outdoorIndexList.ToArray(), 0.3);

            while (timeCounter < maxTime)
            {
                if (index_list.Count > 10)
                    break;
                double ratings_prob = 0;

                n = random.NextDouble();

                int rand = 0;

                for (int i = 0; i < ratings.Length; i++)
                {
                    ratings_prob += ratings[i];
                    if (ratings_prob > n)
                        break;
                    else
                        rand = i + 1;
                }

                if (index_list.Contains(rand))
                    continue;
                else
                {
                    index_list.Add(rand);
                    timeCounter += timeList[rand];

                    // for each newly added place slightly adjust the probabilities to favour other types of places on the next choice
                    List<int> to_change = new List<int>();
                    for (int k = 0; k < ratings.Length; k++)
                        if (timeList[k] == timeList[rand])
                            to_change.Add(k);

                    ratings = AdjustProbability(ratings, index_list.ToArray(), 0.9);
                }
            }

            return index_list;
        }

        private List<int> getOutdoorIndexList(SubtypeDataEntity subtypeData)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < Tour.Count(); i++)
            {
                PlaceEntity el = Tour.ElementAt(i);
                if (el is AttractionEntity)
                {
                    if (subtypeData.is_outdoor.ContainsKey(((AttractionEntity)el).Subtype[0]["name"]))
                    {
                        if (subtypeData.is_outdoor[((AttractionEntity)el).Subtype[0]["name"]])
                            indexList.Add(i);
                    }
                    else
                    {
                        indexList.Add(i);
                    }
                    
                }
            }
            return indexList;
        }

        private (List<int>, List<int>) getSeasonIndexLists(SubtypeDataEntity subtypeData)
        {
            List<int> HotIndexList = new List<int>();
            List<int> ColdIndexList = new List<int>();

            for (int i = 0; i < Tour.Count(); i++)
            {
                PlaceEntity el = Tour.ElementAt(i);
                if (el is AttractionEntity)
                {
                    if (subtypeData.season.ContainsKey(((AttractionEntity)el).Subtype[0]["name"]))
                    {
                        if (subtypeData.season[((AttractionEntity)el).Subtype[0]["name"]] == "hot")
                            HotIndexList.Add(i);
                        else if (subtypeData.season[((AttractionEntity)el).Subtype[0]["name"]] == "cold")
                            ColdIndexList.Add(i);
                    }
                }  
            }
            return (HotIndexList, ColdIndexList);
        }

        static double[] AdjustProbability(double[] prob_list, int[] indexes, double percentage)
        {
            double[] new_list = new double[prob_list.Length];
            double difference = 0.0;
            foreach (int index in indexes)
                difference += prob_list[index] * percentage - prob_list[index];

            double sum = 0;
            for (int k = 0; k < prob_list.Length; k++)
            {
                if (!indexes.Contains(k))
                {
                    sum += prob_list[k];
                }
            }

            for (int k = 0; k < prob_list.Length; k++)
            {
                if (!indexes.Contains(k))
                {
                    new_list[k] = prob_list[k] - prob_list[k] / sum * difference;
                }
                else
                    new_list[k] = prob_list[k] * percentage;
            }

            return new_list;
        }

        public void RemoveNulls()
        {
            IEnumerable<PlaceEntity> no_name = Tour.Where(p => p.Name != null).ToList();
            IEnumerable<PlaceEntity> no_address = no_name.Where(p => p.Address != "").ToList();
            IEnumerable<PlaceEntity> no_photo = no_address.Where(p => p.Photo != null).ToList();
            Tour = no_photo;

            if (Restaurants != null)
            {
                IEnumerable<RestaurantEntity> no_name1 = Restaurants.Where(p => p.Name != null).ToList();
                IEnumerable<RestaurantEntity> no_address1 = no_name1.Where(p => p.Address != "").ToList();
                IEnumerable<RestaurantEntity> no_photo1 = no_address1.Where(p => p.Photo != null).ToList();
                IEnumerable<RestaurantEntity> no_ratings = no_photo1.Where(p => Int16.Parse(p.Num_reviews) > 20).ToList();
                Restaurants = no_ratings;
            }
        }

        public List<string> GetPlacesByName() 
        {
            List<string> Names = new List<string>();
            foreach (PlaceEntity Place in Tour)
            {
                Names.Add(Place.Name);
            }
            return Names;
        }

        public void Remove(PlaceEntity place)
        {
            IEnumerable<PlaceEntity> modified = Tour.Where(p => p.Name != place.Name).ToList();
            Tour = modified;
        }

        public List<List<PlaceEntity>> DistributeBackup(TourModel tour)
        {
            List<List<PlaceEntity>> l = new List<List<PlaceEntity>>();
            for (int i = 0; i < tour.Tour.Count(); i++)
                l.Add(new List<PlaceEntity>());

            foreach (PlaceEntity place in Tour)
            {
                double min = 99999;
                int optimalPosition = -1;
                for (int i = 0; i < tour.Tour.Count(); i++)
                {
                    double d = GetDistance(double.Parse(place.Latitude, System.Globalization.CultureInfo.InvariantCulture),
                                    double.Parse(place.Longitude, System.Globalization.CultureInfo.InvariantCulture),
                                    double.Parse(tour.Tour.ElementAt(i).Latitude, System.Globalization.CultureInfo.InvariantCulture),
                                    double.Parse(tour.Tour.ElementAt(i).Longitude, System.Globalization.CultureInfo.InvariantCulture));
                    if (d < min)
                    {
                        optimalPosition = i;
                        min = d;
                    }       
                }
                l[optimalPosition].Add(place);
            }
            return l;
        }

        public double GetRatingOf(PlaceEntity place)
        {
            IEnumerable<PlaceEntity> resp = Tour.Where(p => p.Name == place.Name).ToList();
            return double.Parse(resp.ElementAt(0).Rating, System.Globalization.CultureInfo.InvariantCulture);
        }

        private static double GetDistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var d1 = sLatitude * (Math.PI / 180.0);
            var num1 = sLongitude * (Math.PI / 180.0);
            var d2 = eLatitude * (Math.PI / 180.0);
            var num2 = eLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
        }
    }
}

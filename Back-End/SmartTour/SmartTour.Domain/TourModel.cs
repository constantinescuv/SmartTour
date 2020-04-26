using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTour.Domain
{
    public class TourModel
    {
        public IEnumerable<PlaceEntity> Tour { set; get; }
        public double Distance { set; get; }
        public double Time { set; get; }

        public TourModel(IEnumerable<PlaceEntity> placeEntities)
        {
            Tour = placeEntities;
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

        public TourModel GetNRandomPlaces(int size)
        {
            double[] ratings = new double[Tour.Count()];

            for (int i = 0; i < Tour.Count(); i++)
            {
                if (Tour.ElementAt(i).Rating != null)
                    ratings[i] = double.Parse(Tour.ElementAt(i).Rating, System.Globalization.CultureInfo.InvariantCulture);
                else
                    ratings[i] = 3;
            }

            int[] index_list = GetRandomIndexList(ratings, size);

            List<PlaceEntity> newTourList = new List<PlaceEntity>();
            foreach (int i in index_list)
            {
                newTourList.Add(Tour.ElementAt(i));
            }

            foreach (int i in index_list)
            {
                Tour = Tour.Where(u => u.Name != Tour.ElementAt(i).Name).ToList();

                for (int j = 0; j < index_list.Length; j++)
                    if (index_list[j] > i)
                        index_list[j] -= 1;                 
            }
       
            return new TourModel(newTourList);           
        }

        private int [] GetRandomIndexList(double [] ratings, int size)
        {
            int[] index_list = new int[size];
            double total_score = 0;

            for (int i = 0; i < ratings.Length; i++)
                total_score += ratings[i];

            Random random = new Random();
            double n;
            int counter = 0;
            while (counter < size)
            {
                double ratings_prob = 0;

                n = random.NextDouble();

                int rand = 0;

                for (int i = 0; i < ratings.Length; i++)
                {
                    ratings_prob += ratings[i] / total_score;
                    if (ratings_prob > n)
                        break;
                    else
                        rand = i;
                }

                if (index_list.Contains(rand))
                    continue;
                else
                {
                    index_list[counter] = rand;
                    counter++;
                }
            }

            return index_list;
        }

        public void RemoveNulls()
        {
            IEnumerable<PlaceEntity> no_name = Tour.Where(p => p.Name != null).ToList();
            IEnumerable<PlaceEntity> no_address = no_name.Where(p => p.Address != "").ToList();

            Tour = no_address;
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

        public double GetRatingOf(PlaceEntity place)
        {
            IEnumerable<PlaceEntity> resp = Tour.Where(p => p.Name == place.Name).ToList();
            return double.Parse(resp.ElementAt(0).Rating, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

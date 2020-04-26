using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTour.Business.Funct
{
    public class SortTour
    {
        public static TourModel Sort(TourDetailsEntity tourDetails, TourModel tour)
        {
            tour.RemoveNulls();
            int maxDistance;
			int maxTime;
            int currentTime = 0; //in minutes
            int currentLength = 0; //in km
			

            switch (tourDetails.DistanceRange)
            {
                case "less than 5 km":
                    maxDistance = 5;
                    break;
                case "less than 10 km":
                    maxDistance = 10;
                    break;
                case "less than 20 km":
                    maxDistance = 20;
                    break;
                default:
                    maxDistance = 30;
                    break;
            }

            foreach (PlaceEntity place in tour.Tour)
            {
                double distance = GetDistance(
                    double.Parse(tourDetails.Latitude, System.Globalization.CultureInfo.InvariantCulture), 
                    double.Parse(tourDetails.Longitude, System.Globalization.CultureInfo.InvariantCulture), 
                    double.Parse(place.Latitude, System.Globalization.CultureInfo.InvariantCulture), 
                    double.Parse(place.Longitude, System.Globalization.CultureInfo.InvariantCulture));

                if (distance > maxDistance)
                {
                    tour.Remove(place);
                }
            }

			TourModel aux = tour.GetNRandomPlaces(10); // TSP will only be applied on a max of 10 randomly chosen places from the tour; the probability is not uniform, it depends on their TripAdvisor rating
													   // this needs to be done mainly to cut the time needed for the TSP execution but also to make the algorithm non-deterministic
													   // if all the 10 places can't be fit in the final ordered Tour, then the ones that are not added will go to a backup List along with all the other places returned by the TripAdvisor api call but not selected in the random function

			TourModel backup = new TourModel(tour.Tour);

			tour = aux;

			switch (tourDetails.TimeRange)
			{
				case "less than 2 hours":
					maxTime = 2 * 60;
					break;
				case "less than 5 hours":
					maxTime = 5 * 60;
					break;
				case "less than 8 hours":
					maxTime = 8 * 60;
					break;
				default:
					maxTime = 12 * 60;
					break;
			}

			double[,] distances = new double[tour.Tour.Count(), tour.Tour.Count()];

			for (int i = 0; i < tour.Tour.Count(); i++)
				for (int j = 0; j < tour.Tour.Count(); j++)
					distances[i, j] = GetDistance(
					double.Parse(tour.Tour.ElementAt(i).Latitude, System.Globalization.CultureInfo.InvariantCulture),
					double.Parse(tour.Tour.ElementAt(i).Longitude, System.Globalization.CultureInfo.InvariantCulture),
					double.Parse(tour.Tour.ElementAt(j).Latitude, System.Globalization.CultureInfo.InvariantCulture),
					double.Parse(tour.Tour.ElementAt(j).Longitude, System.Globalization.CultureInfo.InvariantCulture));

			Tuple<double, int, List<int>> bestTour = TSP(distances, 0, tour.Tour.Count(), maxTime, false);
			tour.EditTour(bestTour); //apply the results TSP algorithm

			return tour;
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

		static Tuple<double, int, List<int>> TSP(double[,] map, int start, int size, int maxTime, bool circuit)
		{
			List<int> vertex = new List<int>();
			for (int i = 0; i < size; i++)
				if (i != start)
					vertex.Add(i);

			// store minimum weight Hamiltonian Cycle. 
			double min_path = 99999;
			int time = 0;
			//int[] tour = new int[size - (circuit ? 0 : 1)];
			List<int> tour = new List<int>();

			while (true)
			{
				// store current Path weight(cost) 
				double current_pathweight = 0;
				int current_time = 0;
				List<int> current_tour = new List<int>();

				// compute current path weight 
				int k = start;
				//int counter = 0;
				for (int i = 0; i < vertex.Count; i++)
				{
					if ((current_time + 45 + 3 * Convert.ToInt32(Math.Ceiling((map[k, vertex[i]] * 10)))) > maxTime)
					{
						//counter += 1;
						break;
					}
					else
					{
						current_time += 45 + 3 * Convert.ToInt32(Math.Ceiling((map[k, vertex[i]] * 10)));
						current_pathweight += map[k, vertex[i]];
						k = vertex[i];
						current_tour.Add(k);
					}
				}

				if (circuit)
				{
					current_pathweight += map[k, start];
					current_tour.Add(start);
				}

				// update minimum 
				if (current_pathweight < min_path)
				{
					min_path = current_pathweight;
					tour = current_tour;
					time = current_time;
				}

				if (!NextPermutation(vertex))
					break;
			}

			tour.Insert(0, start);
			return new Tuple<double, int, List<int>> (min_path, time, tour);
		}

		static bool NextPermutation(List<int> L)
		{
			int n = L.Count;

			int i = n - 2;
			while (i >= 0 && L[i] >= L[i + 1])
				i -= 1;

			if (i == -1)
				return false;

			int j = i + 1;
			while (j < n && L[j] > L[i])
				j += 1;
			j -= 1;

			int aux = L[i];
			L[i] = L[j];
			L[j] = aux;
			int left = i + 1;
			int right = n - 1;

			while (left < right)
			{
				aux = L[left];
				L[left] = L[right];
				L[right] = aux;

				left += 1;
				right -= 1;
			}
			return true;
		}
	}
}

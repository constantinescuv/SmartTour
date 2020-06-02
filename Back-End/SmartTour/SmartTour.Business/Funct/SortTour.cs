using Newtonsoft.Json;
using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartTour.Business.Funct
{
    public class SortTour
    {
        public static (TourModel, List<List<PlaceEntity>>) Sort(TourDetailsEntity tourDetails, TourModel tour, WeatherEntity weather)
        {
            tour.RemoveNulls();
            int maxDistance;
			int maxTime;
            int currentTime = 0; //in minutes
            int currentLength = 0; //in km
			using (StreamReader r = new StreamReader("F:/SmartTour/Front-End/smart-tour/src/assets/subtypeConfig.json"))
			{
				string json = r.ReadToEnd();
				SubtypeDataEntity subtypeData = JsonConvert.DeserializeObject<SubtypeDataEntity>(json);

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

				int[] timeList = new int[tour.Tour.Count()];
				double[] ratingList = new double[tour.Tour.Count()];

				foreach (PlaceEntity place in tour.Tour)
					if (place.Rating == null)
						place.Rating = "3.0";

				double ratingMean = tour.Tour.Sum(item => double.Parse(item.Rating, System.Globalization.CultureInfo.InvariantCulture)) / tour.Tour.Count();

				int t = 0;
				foreach (PlaceEntity place in tour.Tour)
				{
					if (place is AttractionEntity)
					{
						timeList[t] = subtypeData.time[((AttractionEntity)place).Subtype[0]["name"]];

						ratingList[t] = 1 + (ratingMean - double.Parse(place.Rating, System.Globalization.CultureInfo.InvariantCulture)) / 2.5;

						t += 1;
					}
				}

				TourModel aux = tour.GetNRandomPlaces(maxTime, timeList, weather, subtypeData, tourDetails.EatingBreak, tourDetails.savedPlaces); // from the list returned by the TripAdvisor API, enough places to fill the time selected by the user will be selected randomly, with some elements
																				   // having a better probability to be chosen (depends on rating, distance from start, recommended time to visit the place)

				TourModel backup = new TourModel(tour.Tour);

				tour = aux;

				(double, double)[] coordList = new (double, double)[tour.Tour.Count() + 1];
				double[,] distances = new double[tour.Tour.Count() + 1, tour.Tour.Count() + 1];

				var item = tour.Tour.FirstOrDefault(o => o.Address == "Copou Blvd., Iasi Romania"); // even tough in the tripAdvisor api this location has the correct address, the coordinates are wrong 
																									// ("47.16067", "27.5875" which is somewhere close to the center of Iasi) so I corrected them
																									// so far this is the only address-coordinates mismatch that stood out. If more are found, I will use
																									// an external api to get the coordinates based on the address which seems to be correct in the tripAdvisor api
				if (item != null)
				{
					item.Latitude = "47.179223";
					item.Longitude = "27.567325";
				}

				coordList[0] = (double.Parse(tourDetails.Latitude, System.Globalization.CultureInfo.InvariantCulture), double.Parse(tourDetails.Longitude, System.Globalization.CultureInfo.InvariantCulture));
				for (int i = 1; i < coordList.Length; i++)
					coordList[i] = (double.Parse(tour.Tour.ElementAt(i - 1).Latitude, System.Globalization.CultureInfo.InvariantCulture), double.Parse(tour.Tour.ElementAt(i - 1).Longitude, System.Globalization.CultureInfo.InvariantCulture));

				for (int i = 0; i < tour.Tour.Count() + 1; i++)
					for (int j = 0; j < tour.Tour.Count() + 1; j++)
						distances[i, j] = GetDistance(coordList[i].Item1, coordList[i].Item2, coordList[j].Item1, coordList[j].Item2);

				Tuple<double, int, List<int>> bestTour = TSP(distances, timeList, 0, tour.Tour.Count() + 1, false);
				tour.EditTour(bestTour); //apply the results TSP algorithm
				List<List<PlaceEntity>> distributedBackup = backup.DistributeBackup(tour);

				return (tour, distributedBackup);
			}

			
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

		static Tuple<double, int, List<int>> TSP(double[,] map, int[] timeList, int start, int size, bool circuit)
		{
			List<int> vertex = new List<int>();
			for (int i = 0; i < size; i++)
				if (i != start)
					vertex.Add(i);

			// store minimum weight Hamiltonian Cycle. 
			int time = 0;
			double min_path = 99999;
			List<int> tour = new List<int>();

			while (true)
			{
				// store current Path weight(cost) 
				double current_pathweight = 0;
				List<int> current_tour = new List<int>();

				// compute current path weight 
				int k = start;
				//int counter = 0;
				for (int i = 0; i < vertex.Count; i++)
				{
					current_pathweight += map[k, vertex[i]]; //* ratingList[i];
					time += timeList[i];
					k = vertex[i];
					current_tour.Add(k - 1);
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
				}

				if (!NextPermutation(vertex))
					break;
			}

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

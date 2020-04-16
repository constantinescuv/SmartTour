using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business
{
    public interface ITourService
    {
        Task<TourModel> GetTour(TourDetailsEntity tourDetails);
    }
}

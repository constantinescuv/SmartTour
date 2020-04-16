using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.Funct
{
    public interface IGetTour
    {
        Task<TourModel> ReturnTourBasedOnCriteria(TourDetailsEntity tourDetails);

    }
}

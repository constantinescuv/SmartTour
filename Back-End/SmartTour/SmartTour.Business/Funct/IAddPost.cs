using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTour.Business.Funct
{
    public interface IAddPost
    {
        public PostEntity Add(PostEntity post);
    }
}

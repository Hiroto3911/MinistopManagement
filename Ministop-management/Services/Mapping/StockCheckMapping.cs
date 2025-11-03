using AutoMapper;
using Domain.Entity;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping
{
    public class StockCheckMapping : Profile
    {
        public StockCheckMapping()
        {
            CreateMap<StockCheck, StockCheckDto>().ReverseMap();
        }
    }
}

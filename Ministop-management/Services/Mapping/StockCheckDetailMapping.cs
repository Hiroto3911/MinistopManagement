using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping
{
    public class StockCheckDetailMapping : Profile
    {
        public StockCheckDetailMapping()
        {
            CreateMap<StockCheckDetail, StockCheckDetailDto>().ReverseMap();
        }
    }
}

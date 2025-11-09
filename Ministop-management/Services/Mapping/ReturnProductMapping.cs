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
    public class ReturnProductMapping : Profile
    {
        public ReturnProductMapping()
        {
            CreateMap<ReturnProduct, ReturnProductDto>().ReverseMap();
        }
    }
}

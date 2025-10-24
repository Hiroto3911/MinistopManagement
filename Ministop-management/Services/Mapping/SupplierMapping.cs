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
    public class SupplierMapping : Profile
    {
        public SupplierMapping()
        {
            CreateMap<Supplier, SupplierDto>().ReverseMap();
        }
    }
}

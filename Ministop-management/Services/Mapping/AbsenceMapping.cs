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
    public class AbsenceMapping : Profile
    {
        public AbsenceMapping()
        {
            CreateMap<Absence, AbsenceDto>().ReverseMap();
        }
    }
}

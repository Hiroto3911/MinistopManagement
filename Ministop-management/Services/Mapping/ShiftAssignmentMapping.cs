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
    public class ShiftAssignmentMapping : Profile
    {
        public ShiftAssignmentMapping() 
        {
            CreateMap<ShiftAssignment, ShiftAssignmentDto>().ReverseMap();
        }
    }
}

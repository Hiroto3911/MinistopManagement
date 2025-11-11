using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping
{
    public class SalaryContractAllowanceMapping : Profile
    {
        public SalaryContractAllowanceMapping() 
        {
            CreateMap<SalaryContract_Allowance, SalaryContractAllowanceDto>().ReverseMap();
        }
    }
}

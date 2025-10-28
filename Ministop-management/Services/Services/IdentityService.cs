using AutoMapper;
using Domain.DTO;
using Infrastructure.Interfaces;
using Services.Interfaces;
using Shared.Helpers;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public IdentityService(IMinistopUnitOfWork ministopUnitOfWork, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _userSession = userSession;
            _mapper = mapper;
        }
        public  bool Authentication(string userID, string password)
        {
            EmployeeDto user = null;
            if (string.IsNullOrWhiteSpace(userID))
            {
             return false;
            }
            var userEntity = _ministopUnitOfWork.EmployeeRepository.Find(x => x.EmployeeID == userID && !x.IsDeleted);
            if (userEntity == null) return false;
            user = _mapper.Map<EmployeeDto>(userEntity);
            var isValid = HashPasswordSHA256.VerifyPassword(password, user.PasswordHash);
            if (isValid == false) return false;
            _userSession.Login(user.EmployeeId, user.FullName, user.Position, user.StoreId ?? "");
            return true;

        }
        public void LogOut()
        {
            _userSession.LogOut();
        }

    }
}

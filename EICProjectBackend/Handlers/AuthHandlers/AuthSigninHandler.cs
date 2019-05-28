using Domain.ModelEntities;
using Services;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using EICProjectBackend.Services;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;
using LoggingServices.LogItems.Items;
using LoggingServices.ServiceQueues;
using UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EICProjectBackend.Handlers
{
    public class AuthSigninHandler : IRequestHandler<AuthSigninCommand, Object>
    {      
        private readonly IAuthService _authService;
        private readonly IUserRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;      

        public AuthSigninHandler(IAuthService authService, IUnitOfWork unitOfWork)
        {           
            if (authService == null)
                throw new ArgumentNullException(nameof(authService));
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
           
            this._authService = authService;
            this._unitOfWork = unitOfWork;
            this._userRepository = this._unitOfWork.UserRepository;            
        }        

        public async Task<object> Handle(AuthSigninCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Finds the user
                var user = this._userRepository.ReadViaUserName(request.Model.UserName).Result;
                if (user == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }
                    
                //Validates the login
                var match = this._authService.CheckMatch(user.Password, request.Model.Password);
                if (match == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                //Finds Role of the user
                var role = this._userRepository.ReadUserRole(user.RoleId).Result;
                if (role == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                //Creates token
                var token = this._authService.CreateToken(user, role.RoleName);
                if (string.IsNullOrWhiteSpace(token))
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(
                    ActivityTypes.Activities.UserLogin, user.Id);
                
                return token;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.UserLogin,
                    ActivityTypes.Areas.AuthSigninHandler,
                    e.Message);
                return false;
            }            
        }
    }

    public class AuthSigninCommand : IRequest<Object>
    {
        public AuthSigninCommand(AuthSigninModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Model = model;
        }

        public AuthSigninModel Model { get; private set; }
    }
}

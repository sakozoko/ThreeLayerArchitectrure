using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services.Interfaces;
using DAL;
using Entities;

namespace BLL.Services
{
    internal sealed class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger, IMapper mapper) :
            base(unitOfWork, tokenHandler, logger, mapper)
        {
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            if (request is null) LogAndThrowAuthenticationException("Request is null");
            var user = UnitOfWork.UserRepository.GetAll().FirstOrDefault
                (x => x.Name == request.Name && x.Password == request.Password);
            if (user is null) return null;
            Logger.Log($"{user.Name} signed in");
            return CreateAuthenticateResponse(user);
        }

        public AuthenticateResponse Registration(AuthenticateRequest request)
        {
            if (request is null) LogAndThrowAuthenticationException("Request is null");
            if (request.Name.Length < 4 || request.Password.Length < 6)
                return null;
            if (UnitOfWork.UserRepository.GetAll().FirstOrDefault(x => x.Name == request.Name) != null)
                LogAndThrowAuthenticationException("Name taken");
            var user = new UserEntity { Name = request.Name, Surname = request.Surname, Password = request.Password };
            UnitOfWork.UserRepository.Add(Mapper.Map<UserEntity>(user));
            Logger.Log($"{user.Name} registrated.");
            return CreateAuthenticateResponse(user);
        }

        public AuthenticateResponse GetAuthenticateResponse(string token)
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);
            return CreateAuthenticateResponse(requestUser);
        }

        public Task<User> GetByName(string token, string name)
        {
            return Task.Factory.StartNew(() =>
            {
                ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));

                return Mapper.Map<User>(UnitOfWork.UserRepository.GetAll().FirstOrDefault(x => x.Name == name));
            });
        }

        public Task<User> GetById(string token, int id)
        {
            return Task.Factory.StartNew(() =>
            {
                ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));

                return Mapper.Map<User>(UnitOfWork.UserRepository.GetById(id));
            });
        }

        public Task<bool> ChangePassword(string token, string value, string oldPsw, int targetId = 0)
        {
            return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x =>
            {
                if (x.Password != oldPsw)
                    return false;
                x.Password = value;
                return true;
            }, targetId));
        }

        public Task<bool> ChangeName(string token, string value, int targetId = 0)
        {
            return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x =>
            {
                var userWithTheSameName = Mapper.Map<IEnumerable<User>>(UnitOfWork.UserRepository.GetAll()?
                    .Where(userEntity => userEntity.Name.Equals(value, StringComparison.OrdinalIgnoreCase)));
                if (userWithTheSameName?.Count() > 0)
                    return false;
                x.Name = value;
                return true;
            }, targetId));
        }

        public Task<bool> ChangeSurname(string token, string value, int targetId = 0)
        {
            return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x =>
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                    return false;
                x.Surname = value;
                return true;
            }, targetId));
        }

        public Task<bool> ChangeIsAdmin(string token, bool value, int targetId = 0)
        {
            var requestUser = TokenHandler.GetUser(token);
            return Task<bool>.Factory.StartNew(() =>
                targetId != 1 && requestUser.Id != targetId && ChangeProperty(token, x => x.IsAdmin = value, targetId));
        }

        public Task<IEnumerable<User>> GetAll(string token)
        {
            return Task.Factory.StartNew(() =>
            {
                var requestUser = TokenHandler.GetUser(token);
                ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
                Logger.Log($"Admin {requestUser.Name} invoked get all users");
                return Mapper.Map<IEnumerable<User>>(UnitOfWork.UserRepository.GetAll());
            });
        }

        public Task<bool> Remove(string token, int id)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                var requestUser = TokenHandler.GetUser(token);
                ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
                Logger.Log($"Admin {requestUser.Name} invoked remove user with id {id}");
                return requestUser.Id != id && UnitOfWork.UserRepository.Delete(UnitOfWork.UserRepository.GetById(id));
            });
        }

        private AuthenticateResponse CreateAuthenticateResponse(UserEntity user)
        {
            return new AuthenticateResponse(user, TokenHandler.GenerateToken(Mapper.Map<User>(user)));
        }

        private bool ChangeProperty(string token, Func<UserEntity, bool> act, int targetId = 0)
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);
            if (targetId != 0)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                var targetUser = UnitOfWork.UserRepository.GetById(targetId);
                if (!(targetUser is null) && targetUser.Id != requestUser.Id)
                {
                    requestUser = targetUser;
                    Logger.Log($"Admin {requestUser.Name} changed property for user id {targetId}");
                }
            }

            if (!act.Invoke(requestUser)) return false;
            UnitOfWork.UserRepository.Update(Mapper.Map<UserEntity>(requestUser));
            return true;
        }
    }
}
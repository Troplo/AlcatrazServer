using Alcatraz.Context;
using Alcatraz.Context.Entities;
using Alcatraz.DTO.Helpers;
using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using QNetZ;

namespace Alcatraz.GameServices.Services
{
	public interface IUserService
	{
		AuthenticateResponse Authenticate(AuthenticateRequest model);
		ResultModel Register(UserRegisterModel model);
		ResultModel Update(UserModel model);
		ResultModel ChangePassword(Guid userId, string newPassword);
		IEnumerable<UserModel> GetAll();
		UserModel GetById(uint id);
		UserModel GetByIdGuid(Guid id);

		public IEnumerable<Claim> GetUserClaims(UserModel user);
		string GenerateJwtToken(UserModel userModel);
	}

	public class UserService : IUserService
	{
		// users hardcoded for simplicity, store in a db with hashed passwords in production applications
		private readonly AppSettings _appSettings;
		private readonly MainDbContext _dbContext;

		public UserService(IOptions<AppSettings> appSettings, MainDbContext dbContext)
		{
			_appSettings = appSettings.Value;
			_dbContext = dbContext;
		}

		public AuthenticateResponse Authenticate(AuthenticateRequest model)
		{
			var user = _dbContext.Users
				.AsNoTracking()
				.SingleOrDefault(x => x.Email == model.Email);

			// return null if user not found
			if (user == null) 
				return null;

			if(!SecurePasswordHasher.Verify($"{user.Id}-{model.Password}", user.Password))
			{
				return null;
			}

			var userModel = new UserModel
			{
				Id = user.Id,
				Guid = user.Guid,
				PlayerNickName = user.PlayerNickName,
				Email = user.Email,
				RewardFlags = user.RewardFlags
			};

			// authentication successful so generate jwt token
			var token = GenerateJwtToken(userModel);

			return new AuthenticateResponse(userModel, token);
		}

		public IEnumerable<UserModel> GetAll()
		{
			return _dbContext.Users
				.AsNoTracking()
				.Select(x => new UserModel
				{
					Id = x.Id,
					Guid = x.Guid,
					PlayerNickName = x.PlayerNickName,
					Email = x.Email,
					RewardFlags = x.RewardFlags
				}).ToArray();
		}
		
		public UserModel GetById(uint id)
		{
			return _dbContext.Users
				.AsNoTracking()
				.Select(x => new UserModel
				{
					Id = x.Id,
					Guid = x.Guid,
					PlayerNickName = x.PlayerNickName,
					Email = x.Email,
					RewardFlags = x.RewardFlags
				})
				.FirstOrDefault(x => x.Id == id);
		}

		public UserModel GetByIdGuid(Guid id)
		{
			return _dbContext.Users
				.AsNoTracking()
				.Select(x => new UserModel
				{
					Id = x.Id,
					Guid = x.Guid,
					PlayerNickName = x.PlayerNickName,
					Email = x.Email,
					RewardFlags = x.RewardFlags
				})
				.FirstOrDefault(x => x.Guid == id);
		}

		private User GetByIdInternal(Guid id)
		{
			return _dbContext.Users.FirstOrDefault(x => x.Guid == id);
		}

		public ResultModel Register(UserRegisterModel model)
		{
			if(!QConfiguration.Instance.AllowRegistrations)
				return new ResultModel("Registrations are disabled");
			
			if (string.IsNullOrWhiteSpace(model.Email))
				return new ResultModel("Email is incorrect or empty");

			if (string.IsNullOrWhiteSpace(model.PlayerNickName))
				return new ResultModel("PlayerNickName is incorrect or empty");

			if (string.IsNullOrWhiteSpace(model.Password))
				return new ResultModel("Password is incorrect or empty");

			var newUser = new User()
			{
				Email = model.Email,
				PlayerNickName = model.PlayerNickName,
				Password = "tmp",
			};

			if(!_dbContext.Users.Any())
			{
				// Add dummy user with starting ID == 1000
				_dbContext.Users.Add(new User()
				{
					Id = 1000,
					Email = "dummy",
					PlayerNickName = "dummy",
					Password = "dummy",
					RewardFlags = 0,
				});
				_dbContext.SaveChanges();
			}

			if (_dbContext.Users.Any(x => x.Email == model.Email || x.PlayerNickName == model.PlayerNickName))
				return new ResultModel("User with same name or nickname is already present");

			try
			{
				_dbContext.Users.Add(newUser);
				_dbContext.SaveChanges();
			}
			catch(Exception ex)
			{
				QLog.WriteLine(1, ex.Message);
				return new ResultModel("Unable to add user (internal error)");
			}

			// update password as user Id is acquired
			{
				newUser.Password = SecurePasswordHasher.Hash($"{newUser.Id}-{model.Password}");
				_dbContext.SaveChanges();
			}

			return new ResultModel(newUser.Guid, newUser.Id);
		}

		public ResultModel Update(UserModel model)
		{
			var user = GetByIdInternal(model.Guid);

			if (user == null)
				return new ResultModel("User with that Id was not found");

			if(user.Email == model.Email && user.PlayerNickName == model.PlayerNickName)
			{
				// User name and nickname remains unchanged
				return new ResultModel();
			}

			if (user.Email != model.Email && _dbContext.Users.Any(x => x.Email == model.Email))
			{
				return new ResultModel("User with same email is already present");
			}

			if (user.PlayerNickName != model.PlayerNickName && _dbContext.Users.Any(x => x.PlayerNickName == model.PlayerNickName))
			{
				return new ResultModel("User with same nickname is already present");
			}

			try
			{
				user.Email = model.Email;
				user.PlayerNickName = model.PlayerNickName;

				_dbContext.SaveChanges();
			}
			catch
			{
				return new ResultModel("Unable to update user (internal error)");
			}

			return new ResultModel();
		}

		public ResultModel ChangePassword(Guid userId, string newPassword)
		{
			var user = GetByIdInternal(userId);

			if (user == null)
				return new ResultModel("User with that Id was not found");

			try
			{
				user.Password = SecurePasswordHasher.Hash($"{user.Id}-{newPassword}");
				_dbContext.SaveChanges();
			}
			catch
			{
				return new ResultModel("Unable to update user (internal error)");
			}

			return new ResultModel();
		}

		// helper methods

		public string GenerateJwtToken(UserModel user)
		{
			var claims = GetUserClaims(user);

			// generate token that is valid for 7 days
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public IEnumerable<Claim> GetUserClaims(UserModel user)
		{
			return new[] {
				new Claim("uid", user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Email),
			};
		}
	}
}

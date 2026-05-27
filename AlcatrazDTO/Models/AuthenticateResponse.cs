using System;

namespace Alcatraz.DTO.Models
{
	public class AuthenticateResponse
	{
		public uint Id { get; set; }
		public Guid Guid { get; set; }
		public string PlayerNickName { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }

		public AuthenticateResponse()
		{

		}

		public AuthenticateResponse(UserModel user, string token)
		{
			Id = user.Id;
			Guid = user.Guid;
			PlayerNickName = user.PlayerNickName;

			Email = user.Email;
			Token = token;
		}
	}
}

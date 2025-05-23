using System;
using Api_comerce.Dtos;

namespace Api_comerce.Services.Authentication
{
	public interface IAuthenticationService
	{
        Task<dynamic> RegisterAsync(UsuarioRegisterRequestDto request);
        Task<dynamic> GoogleLogin(GoogleAuthDto data);
    }
}


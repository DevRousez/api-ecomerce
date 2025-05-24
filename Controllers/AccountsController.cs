using System;
using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Services.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api_comerce.Controllers
{
    [Route("accounts")]
	public class AccountsController : ControllerBase
	{
        private readonly IAuthenticationService AuthService_;
        private AppDbContext ConexionDB_;

        public AccountsController(AppDbContext ConexionDB, IAuthenticationService AuthService)
		{
			AuthService_ = AuthService;
            ConexionDB_ = ConexionDB;
        }

        [HttpGet]
        [Route("test")]
        public async Task<ActionResult<dynamic>> Test(){
            return new {success= true, message= "API Funcionando Correctamente"};
        }


        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<dynamic>> Register([FromBody] UsuarioRegisterRequestDto DataNewUser) {
            dynamic ResultResponse = AuthService_.RegisterAsync(DataNewUser);
            return (ResultResponse.Result.success == true) ? Ok(ResultResponse.Result) : BadRequest(ResultResponse.Result);
             
        }

        [HttpPost]
        [Route("google/sign-in")]
        public async Task<ActionResult<dynamic>> GoogleSingIn([FromBody] GoogleAuthDto data)
        {
            dynamic ResultResponse = AuthService_.GoogleLogin(data);
            return (ResultResponse.Result.success == true) ? Ok(ResultResponse.Result) : BadRequest(ResultResponse.Result);
        }
       
    }

}


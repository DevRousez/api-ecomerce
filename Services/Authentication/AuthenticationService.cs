using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Azure.Core;
using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Api_comerce.Services.Authentication
{
	public class AuthenticationService : IAuthenticationService
    {
        private AppDbContext ConexionDB_;
        private readonly IConfiguration Config_;

        public AuthenticationService( AppDbContext ConDB, IConfiguration Config)
        {
        
            ConexionDB_ = ConDB;
            Config_ = Config;
        }

        public async Task<dynamic> RegisterAsync(UsuarioRegisterRequestDto request)
        {
            try
            {
                if (ConexionDB_.Accounts.Any(a => a.Email == request.Email)) {
                    return new { success = false, respuesta = "Lo sentimos, ya existe un usuario con la cuenta de correo electrónico proporcionada" };
                }
                Accounts NewAccount = new Accounts();
                NewAccount.FullName = request.FullName;
                NewAccount.Email = request.Email;
                NewAccount.Password = HashPassword(request.Password);
                
                NewAccount.AccountTypeId = 1;
                NewAccount.IsActive = true;
                NewAccount.GoogleId = "";
                NewAccount.Picture = "";
                NewAccount.CreatedAt = DateTime.Now;
                NewAccount.UpdatedAt = DateTime.Now;

                ConexionDB_.Accounts.Add(NewAccount);
                await ConexionDB_.SaveChangesAsync();

                return new { success = true, mensaje = "Usuario creado correctamente", usuario = NewAccount };
            }
            catch (Exception ex)
            {
                return new { success = false, mensaje = ex.Message };

            }

        }

        public async Task<dynamic> GoogleLogin(GoogleAuthDto data)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {

                    Audience = new List<string> { Config_["GoogleServices:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(data.Token, settings);

                var userAccount = ConexionDB_.Accounts.FirstOrDefault(a => a.Email == payload.Email);

                if (userAccount != null)
                {
                    return new { success = true, mensaje = "Usuario Autenticado Correctamente", usuario = userAccount };
                }
                else
                {
                    Accounts NewAccount = new Accounts();
                    NewAccount.FullName = payload.Name;
                    NewAccount.Email = payload.Email;
                    NewAccount.Password = HashPassword(payload.Subject);
                    NewAccount.CreatedAt = DateTime.Now;
                    NewAccount.AccountTypeId = 1;
                    NewAccount.IsActive = true;
                    NewAccount.GoogleId = payload.Subject;
                    NewAccount.Picture = payload.Picture;

                    ConexionDB_.Accounts.Add(NewAccount);
                    await ConexionDB_.SaveChangesAsync();

                    return new { success = true, mensaje = "Usuario Autenticado Correctamente", usuario = NewAccount };
                }

               
            }
            catch (Exception ex)
            {
                return new { success = false, mensaje = ex.Message };
            }
        }

        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            //using var sha256 = SHA256.Create();
            //var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hashedPassword;
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}


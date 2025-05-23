using System;
namespace Api_comerce.Dtos
{
	public class UsuarioRegisterRequestDto
	{
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }

    }
}


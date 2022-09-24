using IWantApp.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Clients;

public class ClientGet
{
	public static string Template => "/clients";
	public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
	public static Delegate Handle => Action;

	[Authorize(Policy = "Authorize")]
	public static async Task<IResult> Action(HttpContext http)
	{
		var user = http.User;
		var result = new
		{
			Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
			Name = user.Claims.First(c => c.Type == "Name").Value,
			Cpf = user.Claims.First(c => c.Type == "Cpf").Value
		};

		return Results.Ok(result);
	}
}

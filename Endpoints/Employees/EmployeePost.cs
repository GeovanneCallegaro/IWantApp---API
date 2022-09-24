﻿using IWantApp.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
	public static string Template => "/employees";
	public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
	public static Delegate Handle => Action;

	[Authorize(Policy = "EmployeePolicy")]
	public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext http, UserCreator userCreator)
	{
		var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
		var userClaims = new List<Claim>
		{
			new Claim("Name", employeeRequest.Name),
			new Claim("CreatedBy", userId),
			new Claim("EmployeeCode", employeeRequest.EmployeeCode)
		};

		(IdentityResult result, string user) = await userCreator.Create(employeeRequest.Email, employeeRequest.Password, userClaims);
		if (!result.Succeeded) return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

		return Results.Created($"/employees/{user}", user);
	}
}

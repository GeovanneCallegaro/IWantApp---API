using Dapper;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
	public static string Template => "/employees";
	public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
	public static Delegate Handle => Action;

	[Authorize(Policy = "EmployeePolicy")]
	public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithName query)
	{
		if (page == null || rows == null) return Results.NotFound("É necessário passar os parâmetros de page e rows!");
		var result = await query.Execute(page.Value, rows.Value);
		return Results.Ok(result);
	}
}

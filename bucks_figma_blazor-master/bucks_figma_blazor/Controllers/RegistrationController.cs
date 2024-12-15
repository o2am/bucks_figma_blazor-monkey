using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace bucks_figma_blazor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private static readonly string HomeDir = AppContext.BaseDirectory;
    private static string OutputPath = Path.Combine(HomeDir, "registration.csv");

    public RegistrationController()
    {
        if (!System.IO.File.Exists(OutputPath))
        {
            System.IO.File.WriteAllText(OutputPath,
                $"Team Name, Institution, Phone Number, Email, Group Names, Member Count, Timestamp, IP {Environment.NewLine}"
                );
        }
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitForm([FromBody] FormModel form)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder
            .Append(form.TeamName)
            .Append(',')
            .Append(form.Institution)
            .Append(',')
            .Append(form.PhoneNumber)
            .Append(',')
            .Append(form.Email)
            .Append(',')
            .Append($"\"[ {string.Join(',', form.GroupNames)} ]\"")
            .Append(',')
            .Append(form.MemberCount)
            .Append(',')
            .Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}")
            .Append(',')
            .Append(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown")
            .AppendLine()
            ;

        await System.IO.File.AppendAllTextAsync(OutputPath, stringBuilder.ToString());

        return Ok();
    }
}
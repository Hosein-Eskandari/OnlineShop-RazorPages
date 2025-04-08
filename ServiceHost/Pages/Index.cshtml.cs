using _0_Framework.Application.Email;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ServiceHost.Pages;

public class IndexModel : PageModel
{
    private readonly IEmailService _emailService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public void OnGet()
    {
        //_emailService.SendEmail("salam", "salam Khoobi", "hosein.eskandariii1994@gmail.com");
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjektArbeitDanijelMademidda.Models;
using AppContext = ProjektArbeitDanijelMademidda.Models.AppContext;

namespace ProjektArbeitDanijelMademidda.Pages
{
    public class adminModel : PageModel
    {
        private readonly ILogger<adminModel> _logger;
        private readonly AppContext _context;

        public adminModel(ILogger<adminModel> logger, AppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<User> Users { get; set; }

        public void OnGet()
        {
            // Hier holen Sie die Benutzerdaten aus der Datenbank
            Users = _context.Users.ToList();
        }
    }
}

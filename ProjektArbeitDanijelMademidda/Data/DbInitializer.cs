using ProjektArbeitDanijelMademidda.Models;
using AppContext = ProjektArbeitDanijelMademidda.Models.AppContext;

namespace ProjektArbeitDanijelMademidda.Data
{
    public class DbInitializer
    {
        private readonly AppContext _context;

        public DbInitializer(AppContext context)
        {
            _context = context;
        }



        public void Run()
        {
            if (_context.Database.EnsureCreated())
            {
                string salt;
                string pwHash = HashGenerator.GenerateHash("user1234", out salt);
                User normalUser = new User { Username = "user1234", Password = pwHash, Salt = salt };

                _context.Users.Add(normalUser);
                _context.SaveChanges();
//            }

//			if (_context.Database.EnsureCreated())
//			{
				string adminSalt;
				string adminPwHash = HashGenerator.GenerateHash("admin1234", out adminSalt);
				Admin adminUser = new Admin { Username = "admin1234", Password = adminPwHash, Salt = adminSalt };

				_context.Admins.Add(adminUser);
				_context.SaveChanges();
			}
		}
    }
}

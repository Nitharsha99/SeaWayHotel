using seaway.API.Configurations;
using seaway.API.Models;

namespace seaway.API.Manager
{
    public class AdminManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly PasswordHelper _passwordHelper;
        string _conString;


        public AdminManager(ILogger<LogHandler> logger, IConfiguration configuration, PasswordHelper passwordHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
            _passwordHelper = passwordHelper;
        }

        public bool NewAdmin(Admin admin)
        {
            if(admin.Password == null) return false;

            var hashPassword = _passwordHelper.HashingPassword(admin.Password);



            return true;
        }


    }
}

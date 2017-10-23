using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace Savonia.AdManagement
{
    /// <summary>
    /// Uses <see cref="System.DirectoryServices.AccountManagement"/>
    /// to find AD user
    /// </summary>
    public class BetterAdManager
    {
        private AccountManagementConfig _config;

        public BetterAdManager(AccountManagementConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Demo metodi... tekee jotain.
        /// </summary>
        public void Demo(string username)
        {
            Console.WriteLine($"Using System.DirectoryServices.AccountManagement to find user: {username}");

            UserPrincipal result = FindUserByUsername(username);
            
            if (null != result)
            {
                Console.WriteLine($"Display name: {result.DisplayName}");
                Console.WriteLine($"E-mail: {result.EmailAddress}");
            }
        }

        public SavoniaUserObject FindUser(string username)
        {
            var user = FindUserByUsername(username);
            SavoniaUserObject su = new SavoniaUserObject();
            su.DisplayName = user.DisplayName;
            su.Dn = user.DistinguishedName;
            su.Email = user.EmailAddress;
            su.IsEnabled = user.Enabled.GetValueOrDefault();
            su.Path = ((System.DirectoryServices.DirectoryEntry)user.GetUnderlyingObject()).Path;
            su.Title = user.Title;
            su.Username = user.SamAccountName;

            user.Dispose();
            return su;
        }

        public void UpdateUser(SavoniaUserObject user)
        {
            // taerkista syöte
            if(user == null || string.IsNullOrEmpty( user.Username))
            {
                //viallinen syöte --> heitä virhe
                throw new ArgumentNullException(nameof(user));
            }

            var adUser = FindUserByUsername(user.Username);
            if(adUser == null)
            {
                throw new ArgumentException($"User \"{user.Username}\" not found on AD");
            }

            //päivitetään ad-objektin tiedot

            adUser.Title = user.Title;
            //+muut

            //tallennetaan muutokset
            adUser.Save();
        }

        private SavoniaUserPrincipal FindUserByUsername(string username)
        {
            var context = GetSearchRoot();
            // search model
            SavoniaUserPrincipal up = new SavoniaUserPrincipal(context);
            up.SamAccountName = username;

            // search
            PrincipalSearcher search = new PrincipalSearcher(up);
            SavoniaUserPrincipal result = (SavoniaUserPrincipal)search.FindOne();
            search.Dispose();

            return result;
        }

        private PrincipalContext GetSearchRoot()
        {
            // setup
            PrincipalContext context;
            if (string.IsNullOrWhiteSpace(_config.Username) ||
                string.IsNullOrWhiteSpace(_config.Password))
            {
                context = new PrincipalContext(
                ContextType.Domain,
                _config.Domain,
                _config.Container,
                ContextOptions.Negotiate);
            }
            else
            {
                context = new PrincipalContext(
                    ContextType.Domain,
                    _config.Domain,
                    _config.Container,
                    ContextOptions.Negotiate,
                    _config.Username,
                    _config.Password);
            }
            return context;
        }
    }
}

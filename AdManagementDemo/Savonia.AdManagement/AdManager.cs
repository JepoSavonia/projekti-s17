using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace Savonia.AdManagement
{
    /// <summary>
    /// Uses <see cref="System.DirectoryServices"/> to read AD object.
    /// </summary>
    public class AdManager
    {
        private DirectoryServicesConfig _config;

        public AdManager(DirectoryServicesConfig config)
        {
            _config = config;
        }

        public void Demo(string username)
        {
            Console.WriteLine($"Searching AD with System.DirectoryServices: {username}");
            
            string filter = $"(&(objectClass=User)(sAMAccountName={username}))";


            var searchRoot = GetSearchRoot();
            var searcher = new DirectorySearcher(searchRoot, filter);
            searcher.SearchScope = SearchScope.Subtree;

            var result = searcher.FindOne();
            // kirjoita tulokset konsoliin
            if (null != result)
            {
                Console.WriteLine($"Found: {result.Path}");
                var de = result.GetDirectoryEntry();
                foreach (string item in result.Properties.PropertyNames)
                {
                    Console.Write($"\n{item}: ");
                    foreach (object value in result.Properties[item])
                    {
                        Console.Write($"{value}\t");
                    }
                }
            }

            searcher.Dispose();
            searchRoot.Dispose();
        }

        public void UpdateUser(SavoniaUserObject user)
        {
            if (null == user)
            {
                return;
            }
            var de = FindUserByUsername(user.Username);

            // voisi tehdä tarkastelun onko tieto muuttunut...
            // ja muokata vain muuttuneet tiedot jne.

            // esimerkki päivittää vain nimikkeen (title)
            de.Properties["title"].Value = user.Title;
            de.CommitChanges();

            de.Dispose();
        }

        public SavoniaUserObject FindUser(string username)
        {
            var de = FindUserByUsername(username);
            if (null == de)
            {
                return null;
            }

            SavoniaUserObject su = new SavoniaUserObject();
            su.DisplayName = de.Properties["displayName"][0].ToString();
            su.Path = de.Path;
            su.Dn = de.Properties["distinguishedName"][0].ToString();
            su.Email = de.Properties["mail"][0].ToString();
            su.Title = de.Properties["title"][0].ToString();
            su.Username = de.Properties["samaccountname"][0].ToString();
            // user enabled/disabled state is in UserAccountControl property
            // UserAccountControl == number --> 0x02 == disabled
            int userAccountControl = (int)de.Properties["useraccountcontrol"][0];
            su.IsEnabled = !((userAccountControl & 0x2) == 0x2);

            de.Dispose();

            return su;
        }

        private DirectoryEntry FindUserByUsername(string username)
        {
            string filter = $"(&(objectClass=User)(sAMAccountName={username}))";

            var searchRoot = GetSearchRoot();
            var searcher = new DirectorySearcher(searchRoot, filter);
            searcher.SearchScope = SearchScope.Subtree;

            var result = searcher.FindOne();
            DirectoryEntry de = null;
            if (null != result)
            {
                de = result.GetDirectoryEntry();
            }
            searcher.Dispose();
            searchRoot.Dispose();

            return de;
        }

        private DirectoryEntry GetSearchRoot()
        {
            string path = _config.LdapPath;
            DirectoryEntry searchRoot;
            if (string.IsNullOrWhiteSpace(_config.Username) ||
                string.IsNullOrWhiteSpace(_config.Password))
            {
                searchRoot = new DirectoryEntry(path)
                {
                    AuthenticationType = AuthenticationTypes.Secure
                };
            }
            else
            {
                searchRoot = new DirectoryEntry(path, _config.Username, _config.Password)
                {
                    AuthenticationType = AuthenticationTypes.Secure
                };
            }
            return searchRoot;
        }
    }
}

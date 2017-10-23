using Savonia.AdManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdManagementDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Monday!");
            Console.WriteLine("Search for username:");
            var username = Console.ReadLine();
            //var adManager = new AdManager(GetDSConfig());
            //var user = adManager.FindUser(username);
            //if (null != user)
            //{
            //    Console.WriteLine(user.ToString());
            //    Console.WriteLine("\nType new title for user:");
            //    var newTitle = Console.ReadLine();
            //    if (!string.IsNullOrWhiteSpace(newTitle))
            //    {
            //        user.Title = newTitle;
            //        adManager.UpdateUser(user);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine($"User \"{username}\" was not found.");
            //}
            //adManager.Demo(username);
            //
            Console.WriteLine("\n\n\n");
            var betterManager = new BetterAdManager(GetAMConfig());
            //betterManager.Demo(username);
            var user = betterManager.FindUser(username);
            if (null != user)
            {
                Console.WriteLine(user.ToString());

                //    Console.WriteLine(user.ToString());
                //    Console.WriteLine("\nType new title for user:");
                //    var newTitle = Console.ReadLine();
                //    if (!string.IsNullOrWhiteSpace(newTitle))
                //    {
                //        user.Title = newTitle;
                //        betterAdManager.UpdateUser(user);
                //    }
            }
            else
            {
                Console.WriteLine($"User \"{username}\" was not found.");
            }


#if DEBUG
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
#endif
        }

        static DirectoryServicesConfig GetDSConfig()
        {
            return new DirectoryServicesConfig()
            {
                // luetaan arvot App.config:sta
                LdapPath = ConfigurationManager.AppSettings["ds_searchLdapPath"],
                Username = ConfigurationManager.AppSettings["ds_username"],
                Password = ConfigurationManager.AppSettings["ds_password"],
            };
        }

        static AccountManagementConfig GetAMConfig()
        {
            return new AccountManagementConfig()
            {
                // luetaan arvot App.config:sta
                Domain = ConfigurationManager.AppSettings["am_domain"],
                Container = ConfigurationManager.AppSettings["am_searchContainer"],
                Username = ConfigurationManager.AppSettings["am_username"],
                Password = ConfigurationManager.AppSettings["am_password"],
            };
        }
    }
}

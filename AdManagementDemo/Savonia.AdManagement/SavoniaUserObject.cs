using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savonia.AdManagement
{
    public class SavoniaUserObject
    {
        public string Path { get; set; }
        public string Dn { get; set; }

        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            return $"Path: {Path}\nDn: {Dn}\nUsername: {Username}\nDisplay name: {DisplayName}\nTitle: {Title}\nEmail: {Email}\nIs enabled: {IsEnabled}";
        }
    }
}

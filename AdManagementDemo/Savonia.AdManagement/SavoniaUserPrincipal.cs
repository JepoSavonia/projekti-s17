using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace Savonia.AdManagement
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("User")]
    public class SavoniaUserPrincipal : UserPrincipal
    {
        // DN == DistinguishedName
        // CN == CommonName
        // RDN == relative distinguished name
        // CN: etunimi sukunimi
        // DN: CN=etunimi sukunimi,OU=kansio,OU=kansio2,DC=domain,DC=local

        public const string TitleAttribute = "title";

        public SavoniaUserPrincipal(PrincipalContext context) : base(context)
        { }

        [DirectoryProperty(TitleAttribute)]
        public string Title
        {
            get
            {
                if (ExtensionGet(TitleAttribute).Length != 1)
                {
                    return null;
                }
                return (string)ExtensionGet(TitleAttribute)[0];
            }
            set
            {
                ExtensionSet(TitleAttribute, value);
            }
        }
    }
}

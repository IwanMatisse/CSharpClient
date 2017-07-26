using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    [Serializable]
    public class AccountSettings
    {
        public string Name { get; set; }
        public string AddressStreamData { get; set; }
        public string AddressRequestData { get; set; }
       

        public AccountSettings()
        {

        }

        public AccountSettings(AccountSettings s)
        {
            Name = s.Name;
            AddressStreamData = s.AddressStreamData;
            AddressRequestData = s.AddressRequestData;     

        }
    }
}

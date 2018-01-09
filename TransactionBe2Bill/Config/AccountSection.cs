using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TransactionBe2Bill.Config
{
    class AccountSection : ConfigurationSection
    {
        [ConfigurationProperty("accounts", IsDefaultCollection = true)]  
        public AccountElementCollection Accounts
        {
            get { return (AccountElementCollection)this["accounts"]; }
        }  
    }
}

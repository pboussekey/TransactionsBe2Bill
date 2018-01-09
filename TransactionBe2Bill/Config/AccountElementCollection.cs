using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TransactionBe2Bill.Config
{
    [ConfigurationCollection(typeof(AccountElement), AddItemName = "account")]  
    class AccountElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AccountElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AccountElement)element).Name;
        }    
    }
}

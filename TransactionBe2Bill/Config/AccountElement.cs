using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace TransactionBe2Bill
{
    class AccountElement : ConfigurationElement 
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey=true)]
        public String Name 
        {
            get { return (String)this["name"]; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public String Password
        {
            get { return (String)this["password"]; }
        }  
    }
}

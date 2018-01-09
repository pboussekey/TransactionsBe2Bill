using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TransactionBe2Bill.Config;

namespace TransactionBe2Bill
{
    class Program
    {


        static void Main(string[] args)
        {
            String method = System.Configuration.ConfigurationManager.AppSettings["method"];
            string[] emails = (System.Configuration.ConfigurationManager.AppSettings["email"] as string).Split(',');
            String URL = System.Configuration.ConfigurationManager.AppSettings["method_url"];
            String startdate;
            int days = -2;
            if(args.Length == 0 || int.TryParse(args[0], out days)){
                DateTime date = DateTime.Now.AddDays(days);
                startdate = date.ToString("yyyy-MM-dd");
            }
            else{
                startdate = args[0];
            }


            Dictionary<String, String> parameters = new Dictionary<String, String>(){
               
                            {"OPERATIONTYPE",method},
                            {"DATE",startdate},
                            {"COMPRESSION","ZIP"},
                            {"VERSION","2.0"},
                            {"TIMEZONE","Europe/Paris"},
                            {"COLUMNS","ORDERID;EXECCODE;NATURE;AMOUNT;CURRENCY;BILLINGFEES INCL. VAT;DATE;CHARGEBACKDATE;OPERATIONTYPE;TRANSFER REFERENCE;IDENTIFIER;EXTRADATA"}
                    
                
                };
            AccountSection section = ConfigurationManager.GetSection("accountSection") as AccountSection;
            AccountElementCollection coll = section.Accounts;
            foreach(String email in emails){
                foreach (AccountElement account in coll)
                {
                    parameters["MAILTO"] = email;
                    parameters["IDENTIFIER"] = account.Name;
                    String clearMessage = account.Password;
                    foreach (String key in parameters.Keys.OrderBy(k => k))
                    {
                        clearMessage += string.Format("{0}={1}{2}", key, parameters[key], account.Password);
                    }

                    //Init the ASCII Encoder
                    var encoder = new UTF8Encoding();

                    //Transform the clear query string to a byte array
                    byte[] messageBytes = encoder.GetBytes(clearMessage);


                    var sha256 = new SHA256Managed();

                    //Hash the message
                    byte[] hashValue = sha256.ComputeHash(messageBytes);

                    //Transform the hash bytes array to a string
                    string hash = "";
                    foreach (byte x in hashValue)
                    {
                        hash += String.Format("{0:x2}", x);
                    }


                    parameters["HASH"] = hash;
                    var postData = "method=" + method;
                    foreach (String key in parameters.Keys.OrderBy(k => k))
                    {
                        postData += String.Format("&params[{0}]={1}", key, System.Web.HttpUtility.UrlEncode(parameters[key]));
                    }


                    var request = (HttpWebRequest)WebRequest.Create(URL);

                    var data = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream2 = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream2, Encoding.UTF8);
                    Console.WriteLine(account.Name + " / " + email + " => " + reader.ReadToEnd());
                    parameters.Remove("HASH");
                }
            }

            Console.ReadLine();
        }




    }


}

using Bibliotheek.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bibliotheek.Domain.Utility
{
    public class WebZoekBoek
    {
        private static string generateString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();

        }
        public static Boek zoekBoek(string isbn)
        {
            
            
            string url = System.Configuration.ConfigurationManager.AppSettings["isbndb_url"] + isbn + "&ticket=" + WebZoekBoek.generateString(10, true);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            Stream myReceiveStream = myResponse.GetResponseStream();
            //xml response verwerken
            XmlDocument myXmlResponse = new XmlDocument();
            myXmlResponse.Load(myReceiveStream);
            XmlNodeList myXmlNodeList;

            myXmlNodeList = myXmlResponse.SelectNodes("/ISBNdb/BookList/BookData");
            
            Boek hetBoek = null;
            Isbn isbnObject = new Isbn{Nummer=isbn};

            
            if (myXmlNodeList.Count != 0)
            {
                foreach (XmlNode myNode in myXmlNodeList)
                {
                    hetBoek = new Boek
                    {
                        Isbn = isbnObject,
                        Titel = myNode.ChildNodes[0].InnerText.Trim(),
                        
                    };
                    //hetBoek = new Boek(isbn, myNode.ChildNodes[0].InnerText.Trim(),
                    //    myNode.ChildNodes[2].InnerText.Trim(),
                    //    myNode.ChildNodes[3].InnerText.Trim());
                }
                hetBoek.GevondenStatus = Gevonden.GevondenOpHetWeb;
            }
            return hetBoek;
        }
    }
}

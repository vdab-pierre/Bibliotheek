using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public static class WebZoekBoek
    {
        public static Boek ZoekBoek(string isbn)
        {


            string url = System.Configuration.ConfigurationManager.AppSettings["isbndb_url"] + isbn;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string strJsonBoek;
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    strJsonBoek = reader.ReadToEnd();
                }
            }

            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                string errorText;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    errorText = reader.ReadToEnd();
                }
                throw new Exception(errorText);
            }

            Boek hetBoek = null;
            Isbn isbnObject = new Isbn { Nummer = isbn };

            var result = JsonConvert.DeserializeObject<dynamic>(strJsonBoek);

            return hetBoek;
            //if (myXmlNodeList.Count != 0)
            //{
            //    foreach (XmlNode myNode in myXmlNodeList)
            //    {
            //        string strGevondenAuteursNamen = myNode.ChildNodes[2].InnerText.Trim();
            //        string [] arrGevondenAuteursNamen =strGevondenAuteursNamen.Split(',');

            //        using (var ctx = new EFBibliotheekRepository()) { 
            //            foreach(string naam in arrGevondenAuteursNamen){
            //                var auteurInDb = ctx.Auteurs.Where(a=>a.Naam.Contains(naam.Substring(naam.)
            //            }
            //        }

            //        hetBoek = new Boek
            //        {
            //            Isbn = isbnObject,
            //            Titel = myNode.ChildNodes[0].InnerText.Trim(),


            //        };
            //hetBoek = new Boek(isbn, myNode.ChildNodes[0].InnerText.Trim(),
            //    myNode.ChildNodes[2].InnerText.Trim(),
            //    myNode.ChildNodes[3].InnerText.Trim());
        }
        //hetBoek.GevondenStatus = Gevonden.GevondenOpHetWeb;
    }
    //return hetBoek;
}


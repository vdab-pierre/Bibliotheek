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
using Tools;

namespace Bibliotheek.Domain.Utility
{
    public static class WebZoekBoek
    {
        public static Boek ZoekBoek(Isbn isbn)
        {

            //boek zoeken op isbndb
            string urlBoek = System.Configuration.ConfigurationManager.AppSettings["isbndb_url_boek"] + isbn.Nummer;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBoek);
            string strJsonBoek;
            if (Netwerk.AssertConnect("www.isbndb.com"))
            {
                try
                {

                    WebResponse boekResponse = request.GetResponse();
                    using (Stream boekResponseStream = boekResponse.GetResponseStream())
                    {
                        StreamReader boekReader = new StreamReader(boekResponseStream, Encoding.UTF8);
                        strJsonBoek = boekReader.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    WebResponse boekErrorResponse = ex.Response;
                    string boekErrorText;
                    using (Stream responseStream = boekErrorResponse.GetResponseStream())
                    {
                        StreamReader boekErrorReader = new StreamReader(responseStream, Encoding.UTF8);
                        boekErrorText = boekErrorReader.ReadToEnd();
                    }
                    throw new Exception(boekErrorText);
                }

                JObject jsonBoek = JObject.Parse(strJsonBoek);
                if (jsonBoek["error"] != null)
                {
                    return null;
                }
                else
                {
                    string jsonBoekTitel = (string)jsonBoek["data"][0]["title"];
                    string jsonBoekSummary = (string)jsonBoek["data"][0]["summary"];
                    string jsonBoekPublisherName = (string)jsonBoek["data"][0]["publisher_name"];
                    string[] jsonBoekAuthor_ids = new string[jsonBoek["data"][0]["author_data"].LongCount()];

                    Boek hetBoek = new Boek
                    {
                        Isbn = isbn,
                        Titel = jsonBoekTitel,
                        Samenvatting = jsonBoekSummary,
                    };

                    //auteurs
                    for (var i = 0; i < jsonBoek["data"][0]["author_data"].LongCount(); i++)
                    {
                        jsonBoekAuthor_ids[i] = (string)jsonBoek["data"][0]["author_data"][i]["id"];
                    }

                    string[,] jsonBoekAuteurNames = new string[jsonBoek["data"][0]["author_data"].LongCount(), 2];
                    for (var i = 0; i < jsonBoekAuthor_ids.LongCount(); i++)
                    {
                        //auteur zoek op isbndb

                        string urlAuteur = System.Configuration.ConfigurationManager.AppSettings["isbndb_url_auteur"] + jsonBoekAuthor_ids[i];
                        request = (HttpWebRequest)WebRequest.Create(urlAuteur);
                        string strJsonAuteur;
                        try
                        {
                            WebResponse response = request.GetResponse();
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                strJsonAuteur = reader.ReadToEnd();
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
                        JObject jsonAuteur = JObject.Parse(strJsonAuteur);
                        jsonBoekAuteurNames[i, 0] = (string)jsonAuteur["data"][0]["last_name"];
                        jsonBoekAuteurNames[i, 1] = (string)jsonAuteur["data"][0]["first_name"];
                    }

                    using (var ctx = new EFBibliotheekRepository())
                    {
                        //auteurs toevoegen aan boek
                        for (var i = 0; i < jsonBoekAuteurNames.GetLength(0); i++)
                        {
                            string famNaam = jsonBoekAuteurNames[i, 0];
                            string voorNaam = jsonBoekAuteurNames[i, 1];
                            var deAuteurInDb = ctx.Auteurs.Where(a => a.Familienaam.Contains(famNaam) && a.Voornaam.Contains(voorNaam)).FirstOrDefault();
                            if (deAuteurInDb != null)
                            {
                                hetBoek.Auteurs.Add(deAuteurInDb);
                            }
                            else
                            {
                                Auteur nieuweAuteur = new Auteur { Familienaam = famNaam, Voornaam = voorNaam };
                                hetBoek.Auteurs.Add(nieuweAuteur);
                            }

                        }
                        //uitgever toevoegen aan boek
                        var deUitgeverInDb = ctx.Uitgevers.Where(u => u.Naam.Contains(jsonBoekPublisherName)).FirstOrDefault();
                        if (deUitgeverInDb != null)
                        {
                            hetBoek.Uitgever = deUitgeverInDb;
                        }
                        else
                        {
                            hetBoek.Uitgever = new Uitgever { Naam = jsonBoekPublisherName };
                        }
                    }
                    return hetBoek;
                }
            }
            else {
                throw new WebException("Isbndb.com is niet bereikbaar", WebExceptionStatus.ConnectFailure);
            }
        }
    }
}


using HtmlAgilityPack;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace KF2_Workshop_URL_Converter
{
    class Program
    {
        private static string appVersion = "1.0";

        static void Main(string[] args)
        {
            //List and catch arguments
            bool help = false;
            bool version = false;
            string url = null;
            string path = null;
            string defaultWorkshopUrl = "steamcommunity.com/sharedfiles/filedetails/?id=";
            List<string> extra;

            OptionSet options = new OptionSet() {
                { "u|url=", "The Url of the Workshop Collection. (required)",
                    v => url = v },
                { "o|output=", "Path of a text file to export. (optional)",
                    v => path = v },
                { "v|version", "Version info.",
                    v => version = v != null },
                { "h|help",  "Show this message and exit.",
                    v => help = v != null }
            };
            
            try
            {
                extra = options.Parse(args);
            }
            catch (Exception e)
            {
                ShowError(e);
                return;
            }
            
            if (help)
            {
                ShowHelp(options);
                return;
            }

            if(version)
            {
                ShowVersion();
                return;
            }

            if (url == null)
            {
                ShowError("Missing required option u|url=");
                return;
            }
            
            if (!url.Contains(defaultWorkshopUrl))
            {
                ShowError("Invalid URL Format.");
                return;
            }
            
            //Program
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            
            //Check if the url has a protocol
            try
            {
                doc = web.Load(url);
            }
            catch(UriFormatException)
            {
                ShowError("Must contain http:// or https:// on the URL.");
                return;
            }

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='collectionItemDetails']");

            if (nodes == null)
            {
                ShowError("Not a Collection.");
                return;
            }

            //List Format
            string title = doc.DocumentNode.SelectNodes("//div[@class='workshopItemTitle']")[0].InnerText;
            string header = "### " + title + " ###" + Environment.NewLine + 
                            "### Coll URL: " + url + " ###" + Environment.NewLine + 
                            "### " + nodes.Count + " Items | Last Query: " + DateTime.Now + " ###";
            string dotIniUrlFormat = "ServerSubscribedWorkshopItems=";
            string footer = "## END of " + title + " ##" + Environment.NewLine;

            if (path == null)
            {
                Console.WriteLine(header);
            }
            else
            {
                //File Exists?
                if (new FileInfo(path).Exists)
                {
                    using (FileStream fs = new FileStream(path, FileMode.Append))
                    {
                        using (StreamWriter file = new StreamWriter(fs))
                        {
                            file.WriteLine(header);
                        }
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter file = new StreamWriter(fs))
                        {
                            file.WriteLine(header);
                        }
                    }
                }
            }

            foreach (HtmlNode n in nodes)
            {
                string itemurl = n.SelectSingleNode(".//a").Attributes["href"].Value;
                string output = dotIniUrlFormat + Regex.Replace(itemurl, @"[^0-9]", "") + " # " + n.SelectSingleNode(".//div[@class='workshopItemTitle']").InnerText;

                if(path == null)
                {
                    Console.WriteLine(output);
                    if(nodes.IndexOf(n) == nodes.Count-1)
                    {
                        Console.WriteLine(footer);
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(path, FileMode.Append))
                    {
                        using (StreamWriter file = new StreamWriter(fs))
                        {
                            file.WriteLine(output);
                            if(nodes.IndexOf(n) == nodes.Count-1)
                            {
                                file.WriteLine(footer);
                            }
                        }
                    }
                }
            }
            if(path != null)
            {
                Console.WriteLine("Success! File Saved to \"" + path + "\"" + Environment.NewLine);
            }
        }

        private static void ShowVersion()
        {
            Console.WriteLine("KF2 Workshop Collection URL Converter v" + appVersion);
            Console.WriteLine("Project Page: https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter");
            Console.WriteLine("Try `dotnet kf2workshopurlconverter.dll --help' for more information.");
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: dotnet kf2workshopurlconverter.dll [OPTIONS]");
            Console.WriteLine("Converts the URL of a Steam Workshop Collection to the format that the file \"PCServer-KFEngine.ini\" accepts.");
            Console.WriteLine("Requires URL on format like https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXXXX... (http:// is accepted as well).");
            Console.WriteLine("## For more info or updates, go to https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        private static void ShowError(Exception e)
        {
            Console.Write("kf2workshopurlconverter: ");
            Console.WriteLine(e.Message);
            Console.WriteLine("Try `dotnet kf2workshopurlconverter.dll --help' for more information.");
        }

        private static void ShowError(string error)
        {
            Console.Write("kf2workshopurlconverter: ");
            Console.WriteLine(error);
            Console.WriteLine("Try `dotnet kf2workshopurlconverter.dll --help' for more information.");
        }
    }
}
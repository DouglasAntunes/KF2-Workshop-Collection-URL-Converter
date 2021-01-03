using KF2WorkshopUrlConverter.Core.KF2ServerUtils;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Services;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KF2WorkshopUrlConverter.Test")]
namespace KF2WorkshopUrlConverter.Core
{
    class Program
    {
        public const string appVersion = "1.1";
        public static string dllFileName;

        public static void Main(string[] args)
        {
            #region List and catch arguments
            bool help = false;
            bool version = false;
            string url = null;
            string path = null;
            dllFileName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
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
                ShowError(e.Message);
                return;
            }

            if (help)
            {
                ShowHelp(options);
                return;
            }

            if (version)
            {
                ShowVersion();
                return;
            }

            if (url == null)
            {
                ShowError("Missing required option u|url=");
                return;
            }
            #endregion

            #region Program
            try
            {
                SteamWorkshopService workshopService = new SteamWorkshopService();
                Collection collection = workshopService.FetchCollectionFromURL(url);

                //List Format
                StringBuilder header = new StringBuilder()
                    .AppendLine($"### {collection.Name} ###")
                    .AppendLine($"### Coll URL: {url} ###")
                    .AppendLine($"### {collection.ItemCount} Items | Last Query: {DateTime.Now} ###");
                string footer = $"## END of {collection.Name} ##";

                string collectionList = new WorkshopCollectionListBuilder()
                    .WithHeader(header.ToString())
                    .WithCollection(collection)
                    .WithFooter(footer)
                    .WithFormat("ServerSubscribedWorkshopItems={0} # {1}")
                .Build();

                if (path == null)
                {
                    Console.WriteLine(collectionList);
                }
                else
                {
                    using (FileStream fs = new FileStream(path, new FileInfo(path).Exists ? FileMode.Append : FileMode.OpenOrCreate))
                    {
                        using (StreamWriter file = new StreamWriter(fs))
                        {
                            file.WriteLine(collectionList);
                        }
                    }
                    Console.WriteLine($"Success! File Saved to \"{path}\"" + Environment.NewLine);
                }
            }
            catch (Exception e)
            {
                ShowError(e.Message);
                return;
            }
            #endregion
        }

        private static void ShowVersion()
        {
            Console.WriteLine();
            Console.WriteLine($"KF2 Workshop Collection URL Converter v{appVersion}");
            Console.WriteLine("Project Page: https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter");
            Console.WriteLine($"Use: '{dllFileName} --help' for more information.");
            Console.WriteLine(Environment.NewLine);
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine();
            Console.WriteLine($"Usage: {dllFileName} [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("About:");
            Console.WriteLine("Converts the URL of a Steam Workshop Collection to the format that the file \"PCServer-KFEngine.ini\" accepts.");
            Console.WriteLine("Requires URL on format like https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXXXX... (http:// is accepted as well).");
            Console.WriteLine("## For more info or updates, go to https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine(Environment.NewLine);
        }

        private static void ShowError(string error)
        {
            Console.WriteLine();
            Console.Write($"KF2 Workshop Collection URL Converter v{appVersion}: ");
            Console.WriteLine(error);
            Console.WriteLine($"Use: '{dllFileName} --help' for more information.");
            Console.WriteLine(Environment.NewLine);
        }
    }
}

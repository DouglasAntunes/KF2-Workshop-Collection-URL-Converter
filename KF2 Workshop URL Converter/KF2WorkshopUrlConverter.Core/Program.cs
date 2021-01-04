using KF2WorkshopUrlConverter.Core.KF2ServerUtils;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Services;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

using static KF2WorkshopUrlConverter.Core.Properties.ProgramStrings;
using static System.String;

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
                { "u|url=", ArgumentDescriptionURL,
                    v => url = v },
                { "o|output=", ArgumentDescriptionOutput,
                    v => path = v },
                { "v|version", ArgumentDescriptionVersion,
                    v => version = v != null },
                { "h|help",  ArgumentDescriptionHelp,
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
                ShowError(ErrorMissingUrlArgumentValue);
                return;
            }
            #endregion

            #region Program
            try
            {
                SteamWorkshopService workshopService = new SteamWorkshopService();
                Collection collection = workshopService.FetchCollectionFromURL(url);

                string collectionList = new WorkshopCollectionListBuilder()
                    .WithHeader(Format(HeaderFormat, collection.Name, url, collection.ItemCount, DateTime.Now))
                    .WithCollection(collection)
                    .WithFooter(Format(FooterFormat, collection.Name))
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
                    Console.WriteLine(ResponseSuccessFileSaved, path);
                }
            }
            catch (Exception e)
            {
                ShowError(e.Message);
                return;
            }
            #endregion
        }

        private static void ShowVersion() => Console.WriteLine(ResponseShowVersion, AppName, appVersion, AppGitHubURL, dllFileName);

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine(ResponseShowHelp, dllFileName, AppGitHubURL);
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
        }

        private static void ShowError(string error) => Console.WriteLine(ResponseShowError, AppName, appVersion, error, dllFileName);
    }
}

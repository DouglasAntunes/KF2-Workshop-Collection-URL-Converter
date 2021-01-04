using KF2WorkshopUrlConverter.Core;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

using static KF2WorkshopUrlConverter.Core.Properties.ProgramStrings;
using static System.String;

namespace KF2WorkshopUrlConverter.Test
{
    [TestFixture]
    class ProgramTest
    {
#pragma warning disable IDE0052 // Remover membros particulares não lidos
        private static readonly string[] ArgsPrefix = { "--", "-", "/" };
        private static readonly string[] ArgsSeparator = { "=" }; // Not included " " because is unreliable on automated tests for some reason.
        private static readonly string[] HelpArgs = { "help", "h" };
        private static readonly string[] UrlArgs = { "url", "u" };
        private static readonly string[] VersionArgs = { "version", "v" };
        private static readonly string[] OutputArgs = { "output", "o" };
#pragma warning restore IDE0052 // Remover membros particulares não lidos

        [TearDown, Description("Restores the default Output & Error Output")]
        public void Teardown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Console.SetError(new StreamWriter(Console.OpenStandardError()));
        }

        private static string DefaultErrorMessage(string message) =>
            Format(ResponseShowError, AppName, Program.appVersion, message, Program.dllFileName) + Environment.NewLine;
        
        private static string DefaultOptionsMessage()
        {
            StringBuilder sb = new StringBuilder(Format(ResponseShowHelp, Program.dllFileName, AppGitHubURL) + Environment.NewLine);
            sb.AppendLine($"  -u, --url=VALUE            {ArgumentDescriptionURL}");
            sb.AppendLine($"  -o, --output=VALUE         {ArgumentDescriptionOutput}");
            sb.AppendLine($"  -v, --version              {ArgumentDescriptionVersion}");
            sb.AppendLine($"  -h, --help                 {ArgumentDescriptionHelp}");
            sb.AppendLine();
            return sb.ToString();
        }

        [Test, Description("Trys to Start without arguments. Expect an error message requiring the url argument.")]
        public void NoArgsStartup()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            
            Program.Main(Array.Empty<string>());

            string expectedResult = DefaultErrorMessage(ErrorMissingUrlArgumentValue);
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Trys to Start with the help argument. Expects the help text with all options.")]
        public void OnlyHelpArg([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("HelpArgs")] string command)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { argPrefix + command });

            string expectedResult = DefaultOptionsMessage();

            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Trys to Start with the version argument. Expects to show the version and command to show more help.")]
        public void OnlyVersionArg([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("VersionArgs")] string command)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { argPrefix + command });
            string expectedResult = Format(ResponseShowVersion, AppName, Program.appVersion, AppGitHubURL, Program.dllFileName) + Environment.NewLine;
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Trys to Start with the url command but with null string. Expects message with missing value.")]
        public void MissingURLArgData([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("UrlArgs")] string command)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { argPrefix + command });

            string expectedResult = DefaultErrorMessage($"Missing required value for option '{argPrefix + command}'.");
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Start the program with a invalid Steam collection url. Expects error message that is not a Steam workshop url.")]
        public void InvalidURLArgData([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("UrlArgs")] string command, 
                                      [ValueSource("ArgsSeparator")] string argSeparator, [Values("google.com")] string url)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { argPrefix + command + argSeparator + url });

            string expectedResult = DefaultErrorMessage("Not a Steam Workshop URL");
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Start the program with a valid Steam collection url. Expects output of the example collection.")]
        public void ValidURLArgData([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("UrlArgs")] string command, [ValueSource("ArgsSeparator")] string argSeparator)
        {
            string validUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { argPrefix + command + argSeparator + validUrl });

            string[] expectedResult = {
                "### Map Collection Example ###",
                $"### Coll URL: {validUrl} ###",
                "### 1 Items \\| Last Query: [0-9]{1,2}\\/[0-9]{1,2}\\/[0-9]{4} [0-9]{1,2}:[0-9]{2}:[0-9]{1,2} ###",
                "ServerSubscribedWorkshopItems=650252240 # DooM 2: Map 1 Entryway",
                "## END of Map Collection Example ##"
            };
            string result = sw.ToString();
            string[] resultArr = result.Split(Environment.NewLine);

            Assert.That(resultArr.Length, Is.EqualTo(7));
            Assert.AreEqual(expectedResult[0], resultArr[0]);
            Assert.AreEqual(expectedResult[1], resultArr[1]);
            Assert.That(resultArr[2], Does.Match(expectedResult[2]));
            Assert.AreEqual(expectedResult[3], resultArr[3]);
            Assert.AreEqual(expectedResult[4], resultArr[4]);
        }

        [Test, Description("Start the program with a valid Steam collection url and a random file output. Expects a message that has save the file and the correct file contents.")]
        public void ValidURLAndOutArgData([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("UrlArgs")] string urlCommand,
                                          [ValueSource("ArgsSeparator")] string argSeparator, [ValueSource("OutputArgs")] string outputCommand)
        {
            string validUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            string fileName = $"{TestContext.CurrentContext.Random.GetString()}.txt";
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { 
                argPrefix + urlCommand + argSeparator + validUrl,
                argPrefix + outputCommand + argSeparator + fileName,
            });

            string[] expectedFileResult = {
                "### Map Collection Example ###",
                $"### Coll URL: {validUrl} ###",
                "### 1 Items \\| Last Query: [0-9]{1,2}\\/[0-9]{1,2}\\/[0-9]{4} [0-9]{1,2}:[0-9]{2}:[0-9]{1,2} ###",
                "ServerSubscribedWorkshopItems=650252240 # DooM 2: Map 1 Entryway",
                "## END of Map Collection Example ##"
            };
            string expectedConsoleResult = Format(ResponseSuccessFileSaved, fileName) + Environment.NewLine;

            Assert.AreEqual(expectedConsoleResult, sw.ToString());

            using(FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                using StreamReader sr = new StreamReader(fs);

                string fileResult = sr.ReadToEnd();
                string[] fileResultArr = fileResult.Split(Environment.NewLine);

                Assert.That(fileResultArr.Length, Is.EqualTo(7));
                Assert.AreEqual(expectedFileResult[0], fileResultArr[0]);
                Assert.AreEqual(expectedFileResult[1], fileResultArr[1]);
                Assert.That(fileResultArr[2], Does.Match(expectedFileResult[2]));
                Assert.AreEqual(expectedFileResult[3], fileResultArr[3]);
                Assert.AreEqual(expectedFileResult[4], fileResultArr[4]);
            }

            File.Delete(fileName);
        }

        [Test, 
         Description("Start the program with a valid Steam collection url and a random file output. Runs 2 Times on the same file output. Expects a message that has save the file and the correct 2x file contents. with a space between.")]
        public void ValidUrlAndOutArgDataAppend([ValueSource("ArgsPrefix")] string argPrefix, [ValueSource("UrlArgs")] string urlCommand,
                                                [ValueSource("ArgsSeparator")] string argSeparator, [ValueSource("OutputArgs")] string outputCommand)
        {
            string validUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            string fileName = $"{TestContext.CurrentContext.Random.GetString()}.txt";
            string[] args = new string[] {
                argPrefix + urlCommand + argSeparator + validUrl,
                argPrefix + outputCommand + argSeparator + fileName,
            };
            
            string[] expectedFileResult = {
                "### Map Collection Example ###",
                $"### Coll URL: {validUrl} ###",
                "### 1 Items \\| Last Query: [0-9]{1,2}\\/[0-9]{1,2}\\/[0-9]{4} [0-9]{1,2}:[0-9]{2}:[0-9]{1,2} ###",
                "ServerSubscribedWorkshopItems=650252240 # DooM 2: Map 1 Entryway",
                "## END of Map Collection Example ##"
            };
            string expectedConsoleResult = Format(ResponseSuccessFileSaved, fileName) + Environment.NewLine;

            // Execute 2 times
            for (int i = 0; i < 2; i++)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    Program.Main(args);
                    Assert.AreEqual(expectedConsoleResult, sw.ToString());

                    using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    {
                        using StreamReader sr = new StreamReader(fs);

                        string fileResult = sr.ReadToEnd();
                        string[] fileResultArr = fileResult.Split(Environment.NewLine);

                        Assert.That(fileResultArr.Length, Is.EqualTo(7 * (i+1) + (i*-1)));
                        Assert.AreEqual(expectedFileResult[0], fileResultArr[(i * 6) + 0]);
                        Assert.AreEqual(expectedFileResult[1], fileResultArr[(i * 6) + 1]);
                        Assert.That(fileResultArr[(i * 6) + 2], Does.Match(expectedFileResult[2]));
                        Assert.AreEqual(expectedFileResult[3], fileResultArr[(i * 6) + 3]);
                        Assert.AreEqual(expectedFileResult[4], fileResultArr[(i * 6) + 4]);

                        // Check if exists a new line space between.
                        if(i == 1) Assert.AreEqual(fileResultArr[5], Empty);
                    }
                }
            }
            File.Delete(fileName);
        }
    }
}

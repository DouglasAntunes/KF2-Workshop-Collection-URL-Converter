using KF2WorkshopUrlConverter.Core;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace KF2WorkshopUrlConverter.Test
{
    [TestFixture]
    class ProgramTest
    {
        [TearDown, Description("Restores the default Output & Error Output")]
        public void Teardown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Console.SetError(new StreamWriter(Console.OpenStandardError()));
        }

        private string DefaultErrorMessage(string appVersion, string dllName, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"KF2 Workshop Collection URL Converter v{appVersion}: ");
            sb.AppendLine(message);
            sb.AppendLine($"Try `dotnet {dllName} --help' for more information.");
            return sb.ToString();
        }

        private string DefaultOptionsMessage(string dllName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Usage: dotnet {dllName} [OPTIONS]");
            sb.AppendLine("Converts the URL of a Steam Workshop Collection to the format that the file \"PCServer-KFEngine.ini\" accepts.");
            sb.AppendLine($"Requires URL on format like https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXXXX... (http:// is accepted as well).");
            sb.AppendLine("## For more info or updates, go to https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter");
            sb.AppendLine();
            sb.AppendLine("Options:");
            sb.AppendLine("  -u, --url=VALUE            The Url of the Workshop Collection. (required)");
            sb.AppendLine("  -o, --output=VALUE         Path of a text file to export. (optional)");
            sb.AppendLine("  -v, --version              Version info.");
            sb.AppendLine("  -h, --help                 Show this message and exit.");
            return sb.ToString();
        }

        [Test, Description("Trys to Start without arguments. Expect an error message requiring the url argument.")]
        public void NoArgsStartup()
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            
            Program.Main(new string[] { });

            string expectedResult = DefaultErrorMessage(Program.appVersion, Program.dllFileName, "Missing required option u|url=");
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Trys to Start with the help argument. Expects the help text with all options.")]
        public void OnlyHelpArg([Values("--help", "--h", "-help", "-h")] string command)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { command });

            string expectedResult = DefaultOptionsMessage(Program.dllFileName);

            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Trys to Start with the version argument. Expects to show the version and command to show more help.")]
        public void OnlyVersionArg([Values("--version", "--v", "-version", "-v")] string command)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { command });

            string expectedResult = $"KF2 Workshop Collection URL Converter v{Program.appVersion}" + Environment.NewLine +
                                     "Project Page: https://github.com/DouglasAntunes/KF2-Workshop-Collection-URL-Converter" + Environment.NewLine +
                                    $"Try `dotnet {Program.dllFileName} --help' for more information." + Environment.NewLine;
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Trys to Start with the url command but with null string. Expects message with missing value.")]
        public void MissingURLArgData([Values("--url", "-url")] string command)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { command });

            string expectedResult = DefaultErrorMessage(Program.appVersion, Program.dllFileName, $"Missing required value for option '{command}'.");
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Start the program with a invalid Steam collection url. Expects error message that is not a Steam workshop url.")]
        public void InvalidURLArgData([Values("--url=", "-url=")] string command, [Values("", "google.com")] string url)
        {
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { command + url });

            string expectedResult = DefaultErrorMessage(Program.appVersion, Program.dllFileName, "Not a Steam Workshop URL");
            Assert.AreEqual(expectedResult, sw.ToString());
        }

        [Test, Description("Start the program with a valid Steam collection url. Expects output of the example collection.")]
        public void ValidURLArgData([Values("--url=", "-url=")] string command)
        {
            string validUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { command + validUrl });

            string[] expectedResult = {
                "### Map Collection Example ###",
                $"### Coll URL: {validUrl} ###",
                "### 1 Items \\| Last Query: [0-9]{1,2}\\/[0-9]{1,2}\\/[0-9]{4} [0-9]{1,2}:[0-9]{2}:[0-9]{1,2} ###",
                "",
                "ServerSubscribedWorkshopItems=650252240 # DooM 2: Map 1 Entryway",
                "## END of Map Collection Example ##"
            };
            string result = sw.ToString();
            string[] resultArr = result.Split(Environment.NewLine);

            Assert.That(resultArr.Length, Is.EqualTo(8));
            Assert.AreEqual(expectedResult[0], resultArr[0]);
            Assert.AreEqual(expectedResult[1], resultArr[1]);
            Assert.That(resultArr[2], Does.Match(expectedResult[2]));
            Assert.AreEqual(expectedResult[3], resultArr[3]);
            Assert.AreEqual(expectedResult[4], resultArr[4]);
            Assert.AreEqual(expectedResult[5], resultArr[5]);
        }

        [Test, Description("Start the program with a valid Steam collection url and a random file output. Expects a message that has save the file and the correct file contents.")]
        public void ValidURLAndOutArgData([Values("--url=", "-url=")] string urlCommand, [Values("--o=", "-o=")] string outputCommand)
        {
            string validUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            string fileName = $"{TestContext.CurrentContext.Random.GetString()}.txt";
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            Program.Main(new string[] { urlCommand + validUrl, outputCommand + fileName });

            string[] expectedFileResult = {
                "### Map Collection Example ###",
                $"### Coll URL: {validUrl} ###",
                "### 1 Items \\| Last Query: [0-9]{1,2}\\/[0-9]{1,2}\\/[0-9]{4} [0-9]{1,2}:[0-9]{2}:[0-9]{1,2} ###",
                string.Empty,
                "ServerSubscribedWorkshopItems=650252240 # DooM 2: Map 1 Entryway",
                "## END of Map Collection Example ##"
            };
            string expectedConsoleResult = $"Success! File Saved to \"{fileName}\"" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expectedConsoleResult, sw.ToString());

            using(FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                using StreamReader sr = new StreamReader(fs);

                string fileResult = sr.ReadToEnd();
                string[] fileResultArr = fileResult.Split(Environment.NewLine);

                Assert.That(fileResultArr.Length, Is.EqualTo(8));
                Assert.AreEqual(expectedFileResult[0], fileResultArr[0]);
                Assert.AreEqual(expectedFileResult[1], fileResultArr[1]);
                Assert.That(fileResultArr[2], Does.Match(expectedFileResult[2]));
                Assert.AreEqual(expectedFileResult[3], fileResultArr[3]);
                Assert.AreEqual(expectedFileResult[4], fileResultArr[4]);
                Assert.AreEqual(expectedFileResult[5], fileResultArr[5]);
            }

            File.Delete(fileName);
        }

        [Test, 
         Description("Start the program with a valid Steam collection url and a random file output. Runs 2 Times on the same file output. Expects a message that has save the file and the correct 2x file contents. with a space between.")]
        public void ValidUrlAndOutArgDataAppend([Values("--url=", "-url=")] string urlCommand, [Values("--o=", "-o=")] string outputCommand)
        {
            string validUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            string fileName = $"{TestContext.CurrentContext.Random.GetString()}.txt";
            string[] args = new string[] { urlCommand + validUrl, outputCommand + fileName };
            
            string[] expectedFileResult = {
                "### Map Collection Example ###",
                $"### Coll URL: {validUrl} ###",
                "### 1 Items \\| Last Query: [0-9]{1,2}\\/[0-9]{1,2}\\/[0-9]{4} [0-9]{1,2}:[0-9]{2}:[0-9]{1,2} ###",
                string.Empty,
                "ServerSubscribedWorkshopItems=650252240 # DooM 2: Map 1 Entryway",
                "## END of Map Collection Example ##"
            };
            string expectedConsoleResult = $"Success! File Saved to \"{fileName}\"" + Environment.NewLine + Environment.NewLine;

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

                        Assert.That(fileResultArr.Length, Is.EqualTo(8 * (i+1) + (i*-1)));
                        Assert.AreEqual(expectedFileResult[0], fileResultArr[(i * 7) + 0]);
                        Assert.AreEqual(expectedFileResult[1], fileResultArr[(i * 7) + 1]);
                        Assert.That(fileResultArr[(i * 7) + 2], Does.Match(expectedFileResult[2]));
                        Assert.AreEqual(expectedFileResult[3], fileResultArr[(i * 7) + 3]);
                        Assert.AreEqual(expectedFileResult[4], fileResultArr[(i * 7) + 4]);
                        Assert.AreEqual(expectedFileResult[5], fileResultArr[(i * 7) + 5]);

                        // Check if exists a new line space between.
                        if(i == 1)
                        {
                            Assert.AreEqual(fileResultArr[6], string.Empty);
                        }
                    }
                }
            }
            File.Delete(fileName);
        }
    }
}

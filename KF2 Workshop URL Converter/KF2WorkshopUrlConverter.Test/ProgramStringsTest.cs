using NUnit.Framework;
using System.Globalization;
using KF2WorkshopUrlConverter.Core.Properties;

namespace KF2WorkshopUrlConverter.Test
{
    [TestFixture]
    class ProgramStringsTest
    {
        [Test, Description("Dummy Test the default constructor")]
        public void ConstructorEmpty() {
            ProgramStrings programStrings = new ProgramStrings();
            Assert.IsNotNull(programStrings);
        }

        [Test, Description("Dummy test to cover get/set of the property Culture")]
        public void CultureInfoData()
        {
            string cultureName = "en-US";
            CultureInfo toApply = new CultureInfo(cultureName);

            ProgramStrings.Culture = toApply;
            CultureInfo culture = ProgramStrings.Culture;

            Assert.IsNotNull(culture);
            Assert.AreEqual(cultureName, culture.Name);
        }
    }
}

using KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions;
using NUnit.Framework;
using System;

namespace KF2WorkshopUrlConverter.Test.SteamWorkshop.Exceptions
{
    [TestFixture]
    class NotACollectionExceptionTest
    {
        [Test, Description("Instantiate the exception with the no args constructor to test attributes.")]
        public void NoArgClassConstructor()
        {
            NotACollectionException ex = new NotACollectionException();
            Assert.AreEqual(ex.Message, "Error in the application.");
            Assert.IsNull(ex.InnerException);
        }

        [Test, Description("Instantiate the exception with the message constructor to test attributes.")]
        public void MessageArgClassConstructor()
        {
            string message = TestContext.CurrentContext.Random.GetString();
            NotACollectionException ex = new NotACollectionException(message);
            Assert.AreEqual(ex.Message, message);
            Assert.IsNull(ex.InnerException);
        }

        [Test, Description("Instantiate the exception with the message & inner exception constructor to test attributes.")]
        public void MessageAndInnerExClassConstructor()
        {
            string message = TestContext.CurrentContext.Random.GetString();
            string message2 = TestContext.CurrentContext.Random.GetString();
            Exception inner = new Exception(message);
            NotACollectionException ex = new NotACollectionException(message2, inner);
            Assert.AreEqual(ex.Message, message2);
            Assert.IsNotNull(ex.InnerException);
            Assert.IsInstanceOf(inner.GetType(), ex.InnerException);
            Assert.AreEqual(ex.InnerException.Message, inner.Message);
        }
    }
}

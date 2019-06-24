using System;

namespace KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions
{
    class NotACollectionException : ApplicationException
    {
        public NotACollectionException()
        {
        }

        public NotACollectionException(string message) : base(message)
        {
        }

        public NotACollectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

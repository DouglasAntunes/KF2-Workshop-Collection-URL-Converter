using System;

namespace KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions
{
    class NotASteamWorkshopUrlException : ApplicationException
    {
        public NotASteamWorkshopUrlException()
        {
        }

        public NotASteamWorkshopUrlException(string message) : base(message)
        {
        }

        public NotASteamWorkshopUrlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

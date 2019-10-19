using System;

namespace Com.WIC.BusinessLogic.Exceptions
{
    public class UserFacingException : Exception
    {
        public string Details { get; set; }
        public UserFacingException(string message, string details = null)
            : base(message)
        {
            Details = details;
        }
        public UserFacingException(string message, Exception innerException, string details = null) 
            : base(message, innerException)
        {
            Details = details;
        }
    }
}

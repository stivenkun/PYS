using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PYS.IdentityServer.Security.Administration.Utilities
{
    public class ServiceResponse
    {
        private const int DEFAULT_SUCCESSFUL_CODE = 0;
        private const int DEFAULT_ERROR_CODE = 1;

        public ServiceResponse()
        {
            ResponseMessage = string.Empty;
        }

        public ServiceResponse(int responseCode) : this()
        {
            ResponseCode = responseCode;
        }

        public ServiceResponse(int responseCode, string responseMessage) : this(responseCode)
        {
            ResponseMessage = responseMessage;
        }

        public ServiceResponse(int responseCode, string responseMessage, object responseData) : this(responseCode, responseMessage)
        {
            ResponseData = responseData;
        }

        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object ResponseData { get; set; }

        public static ServiceResponse GetErrorResponse()
        {
            return GetErrorResponse(string.Empty);
        }

        public static ServiceResponse GetErrorResponse(string responseMessage)
        {
            return GetErrorResponse(responseMessage, null);
        }

        public static ServiceResponse GetErrorResponse(string responseMessage, object responseData)
        {
            return new ServiceResponse(DEFAULT_ERROR_CODE, responseMessage, responseData);
        }

        public static ServiceResponse GetSuccessfulResponse()
        {
            return GetSuccessfulResponse(string.Empty);
        }

        public static ServiceResponse GetSuccessfulResponse(string responseMessage)
        {
            return GetSuccessfulResponse(responseMessage, null);
        }

        public static ServiceResponse GetSuccessfulResponse(string responseMessage, object responseData)
        {
            return new ServiceResponse(DEFAULT_SUCCESSFUL_CODE, responseMessage, responseData);
        }
    }
}

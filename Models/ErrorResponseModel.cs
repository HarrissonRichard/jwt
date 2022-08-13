namespace Twwet.Models
{

    public class ErrorResponseModel
    {
        public ResponseCode ResponseCode { get; set; }

        public string Message { get; set; }
        public ErrorResponseModel(ResponseCode responseCode, string message)
        {
            this.ResponseCode = responseCode;
            this.Message = message;
        }
    }
}


using System.Collections.Generic;


namespace HotelesCore.Models.DTOs
{
    public class ResponseNormalaizer
    {
        public string providerName { get; set; }      
        public string destination { get; set; }      
        public string price { get; set; }
        public string code { get; set; }
        public string hotelname { get; set; }
        public string roomType { get; set; }
       
    }

    public class Root
    {
        public string uuid { get; set; }
        public string processType { get; set; }
        public string providerType { get; set; }
        public List<ResponseNormalaizer> providersResponse { get; set; }
    }
    public class Root1
    {
        public string uuid { get; set; }
        public string processType { get; set; }
        public string providerType { get; set; }
        public List<ResponseNormalaizer1> providersResponse { get; set; }
    }
    public class ResponseNormalaizer1
    {
        public string providerName { get; set; }
        public bool status { get; set; }      
    }
    public class RequestCache
    {
        public string request { get; set; }
    }

}

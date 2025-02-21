namespace AIIcsoftAPI.Dto
{
    public class ErrorlogDto
    {

        public string APIURL { get; set; }
        public string UrlHttpMethod { get; set; }    //(Get, Post, Put etc)
        public string ModuleName { get; set; }    //(MaterialMaster, BOMMaster etc)
        public string UrlResponseType { get; set; }    //(Success or Failure)
        public string UrlResponseData { get; set; }
        public string UrlResponseMessage { get; set; }
        public string AdditionalRemarks { get; set; }
        public string jsonInput { get; set; }        //newly added
        public string EntryEmailId { get; set; }
        public string EntryComputer { get; set; }
      
    }
}

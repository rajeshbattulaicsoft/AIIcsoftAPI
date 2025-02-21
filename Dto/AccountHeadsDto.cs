namespace AIIcsoftAPI.Dto
{
    public class AccountHeadsDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string ParentName { get; set; }
        public int AccountTypeId { get; set; }
        public int OLevelId { get; set; }
        public int TreeLevel { get; set; }
        public int ParentAccountId { get; set; }
        //  public string EmailAddress { get; set; }
    }
}

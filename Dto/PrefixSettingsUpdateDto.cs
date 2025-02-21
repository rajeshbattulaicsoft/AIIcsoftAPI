namespace AIIcsoftAPI.Dto
{
    public class PrefixSettingsUpdateDto
    {
        public decimal parentid { get; set; }
        public DateTime effectivefrom { get; set; }
        public decimal locationid { get; set; }
        public string transactioncode { get; set; }
        public string yearcode { get; set; }
        public string monthcode { get; set; }
        public decimal locationcodedisplayorder { get; set; }
        public decimal yearcodedisplayorder { get; set; }
        public decimal monthcodedisplayorder { get; set; }
        public decimal transactioncodedisplayorder { get; set; }
        public decimal numberdisplayorder { get; set; }
        public string startno { get; set; }
        public string prefixseperator { get; set; }

        public int entryempid { get; set; }
        public string entrycomputer { get; set; }

    }
}

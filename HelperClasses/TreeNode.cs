namespace AIIcsoftAPI.HelperClasses
{
    public class TreeNode
    {
        public int AccountId { get; set; }
        public int? ParentAccountId { get; set; }
        public int Treelevel { get; set; }
        public string AccountName { get; set; }
        public int TreeLevel { get; set; }
        public int AccountTypeId { get; set; }
        public int OlevelId { get; set; }

        public TreeNode ParentCategory { get; set; }
        public List<TreeNode> Children { get; set; }
    }
}

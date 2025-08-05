namespace BookLibrarySystem.Models.ViewModels
{
    public class ClaimRecordViewModel
    {
        public int ClaimID { get; set; }
        public string StaffName { get; set; }
        public int OrderID { get; set; }
        public DateTime ClaimDate { get; set; }
    }

    public class ClaimRecordsSearchModel
    {
        public string? SearchTerm { get; set; }
        public string? SearchType { get; set; } // "OrderID" or "StaffName"
        public List<ClaimRecordViewModel> Claims { get; set; } = new();
    }
} 
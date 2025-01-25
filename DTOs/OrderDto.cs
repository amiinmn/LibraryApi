public class OrderDto
{
    public int Id { get; set; }
    public string BorrowerName { get; set; }
    public string StartBorrowDate { get; set; }
    public string EndBorrowDate { get; set; }
    public int BorrowDuration { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; }
}
public class Orders
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BorrowerName { get; set; }
    public DateTime StartBorrowDate { get; set; }
    public DateTime EndBorrowDate { get; set; }

    public Books Book { get; set; }
}
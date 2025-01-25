public class Books
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateTime PublishDate { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsAvailable { get; set; }

    public ICollection<Orders> Orders { get; set; }
}
public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public int PublishedYear { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public string WillBeAvailableAt { get; set; }
}
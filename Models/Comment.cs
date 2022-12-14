namespace ZenBlog.Models;


public class Comment
{
  public string CommentId { get; set; } 
  public string? BlogId { get; set; } 
  public string? Author { get; set; } 

  public string? Content { get; set; } 
  
  public string? DayComment { get; set; } 
 
}
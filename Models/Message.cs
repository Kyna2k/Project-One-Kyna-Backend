namespace KynaShop.Models
{
    public class Message
    {
        public Message(int id, string title, string Body)
        {
            Id = id;
            Title = title;
            this.Body = Body; 
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}

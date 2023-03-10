namespace ChatApp.Client.Contracts;

using MessagePack;

[MessagePackObject]
public class Message
{
    [Key(0)]
    public string Id { get; set; }

    [Key(1)]
    public string UserName { get; set; }

    [Key(3)]
    public DateTime SendDate { get; set; }

    [Key(4)]
    public string Text { get; set; }
}
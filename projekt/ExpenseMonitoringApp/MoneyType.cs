public class MoneyType
{

    public long Id { get; set; }
    public string Name { get; set; }

    public MoneyType(string name)
    {
        Name = name;
    }
}

public class Comment
{
    public long Id { get; set; }
    public string Text { get; set; }
    public Entry Entry { get; set; }

    public Comment(string text)
    {
        Text = text;
    }
}

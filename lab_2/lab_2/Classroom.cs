using System.Text;

public class Classroom
{
    private string name;
    private Person[] persons;

    public string Name => name;

    public Classroom(string name, Person[] persons)
    {
        this.name = name;
        this.persons = persons;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"Classroom: {name}\n\n");
        for (int i = 0; i < persons.Length; i++)
        {
            stringBuilder.Append(persons[i].ToString());
            stringBuilder.Append("\n\n");

        }
        return stringBuilder.ToString();
    }
}
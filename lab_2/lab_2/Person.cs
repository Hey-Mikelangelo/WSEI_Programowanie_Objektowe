using System;
using System.Diagnostics.CodeAnalysis;

public class Person : IEquatable<Person>
{
    protected string name;
    protected int age;

    public string Name => name;
    public int Age => age;

    public Person(string name, int age)
    {
        this.name = name;
        this.age = age;
    }

    public bool Equals([AllowNull] Person other)
    {
        if (this == other) return true;
        return other != null && this.name == other.name && this.age == other.age;
    }

    public override string ToString()
    {
        return $"{name} ({age} y.o)";
    }
}

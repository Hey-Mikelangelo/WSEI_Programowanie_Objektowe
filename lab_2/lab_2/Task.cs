using System;
using System.Diagnostics.CodeAnalysis;

public class Task : IEquatable<Task>{
    private string name;
    private TaskStatus status;

    public string Name => name;
    public TaskStatus Status { get => status; set => status = value; }

    public Task(string name, TaskStatus status)
    {
        this.name = name;
        this.status = status;
    }

    public override string ToString()
    {
        return $"{name} [{status}]";
    }

    public bool Equals([AllowNull] Task other)
    {
        if (this == other) return true;
        return other != null && this.name == other.name && this.status == other.status;
    }
}

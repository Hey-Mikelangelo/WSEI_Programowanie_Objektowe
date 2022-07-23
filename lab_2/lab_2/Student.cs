using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

public class Student : Person, IEquatable<Student>
{
    private string group;
    protected List<Task> tasks = new List<Task>();

    protected string Group => group;

    public Student(string name, int age, string group) : base(name, age)
    {
        this.group = group;
    }

    public Student(string name, int age, string group, List<Task> tasks) : base(name, age)
    {
        this.group = group;
        this.tasks = tasks;
    }

    public void AddTask(string taskName, TaskStatus taskStatus)
    {
        tasks.Add(new Task(taskName, taskStatus));
    }

    public void RemoveTask(int index)
    {
        tasks.RemoveAt(index);
    }

    public void UpdateTask(int index, TaskStatus taskStatus)
    {
        Task task = tasks[index];
        task.Status = taskStatus;
    }

    public string RenderTasks(string prefix = "\t")
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Tasks:\n");
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            stringBuilder.Append(prefix);
            stringBuilder.Append($"{i}. {task}\n");
        }
        return stringBuilder.ToString();
    }

    public override string ToString()
    {
        string tasksString = RenderTasks();
        return $"Student: {base.ToString()}\nGroup: {group}\n{tasksString}";
    }

    public bool Equals([AllowNull] Student other)
    {
        if (this == other) return true;
        return 
            ((Person)this).Equals(other) 
            && this.group == other.group 
            && SequenceEqual(this.tasks, other.tasks);
    }

    private static bool SequenceEqual<T>(List<T> a, List<T> b)
        where T : IEquatable<T>
    {
        
        if(a.Count != b.Count)
        {
            return false;
        }
        int count = a.Count;
        for (int i = 0; i < count; i++)
        {
            if(a[i].Equals(b[i]) == false)
            {
                return false;
            }
        }
        return true;
    }
}
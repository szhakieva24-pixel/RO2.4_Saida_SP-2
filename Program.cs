using System;
using System.Linq;

class Student
{
    private static int _nextId = 1;

    public int StudentId { get; private set; }
    public string Name { get; set; }
    public string Faculty { get; set; }

    private double _gpa;
    public double GPA
    {
        get => _gpa;
        set
        {
            if (value < 0.0 || value > 4.0)
                throw new ArgumentOutOfRangeException(nameof(GPA), "GPA must be between 0.0 and 4.0.");
            _gpa = value;
        }
    }

    public Student(string name, double gpa, string faculty)
    {
        StudentId = _nextId++;
        Name = name; GPA = gpa; Faculty = faculty;
    }

    public override string ToString() =>
        $"[{StudentId,3}] {Name,-20} GPA:{GPA:F2}  {Faculty}";
}

class Registry
{
    private const int Max = 100;
    private readonly Student[] _data = new Student[Max];
    private int _count = 0;

    public bool Add(Student s)
    {
        if (_count >= Max) { Console.WriteLine("Registry is full (100 students)."); return false; }
        _data[_count++] = s;
        return true;
    }

    public Student FindById(int id) =>
        _data.Take(_count).FirstOrDefault(s => s.StudentId == id);

    public Student[] FindByName(string name) =>
        _data.Take(_count)
             .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
             .ToArray();

    public Student[] GetTopStudents(int n) =>
        _data.Take(_count).OrderByDescending(s => s.GPA).Take(n).ToArray();

    public void PrintAll()
    {
        if (_count == 0) { Console.WriteLine("No students in the registry."); return; }
        for (int i = 0; i < _count; i++) Console.WriteLine(_data[i]);
    }
}

class Program
{
    static readonly Registry reg = new();

    static void Main()
    {
        reg.Add(new("Aisha Bekova", 3.9, "Computer Science"));
        reg.Add(new("Daniyar Seitkali", 3.4, "Mathematics"));
        reg.Add(new("Zarina Mukhanova", 3.7, "Physics"));

        while (true)
        {
            PrintMenu();
            switch (Console.ReadLine())
            {
                case "1": AddStudent(); break;
                case "2": FindById(); break;
                case "3": FindByName(); break;
                case "4": ShowTop(); break;
                case "5": reg.PrintAll(); break;
                case "6": return;
                default: Console.WriteLine("Invalid option."); break;
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
    static void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine(" ~~~ STUDENT REGISTRY SYSTEM ~~~");
        Console.WriteLine("1. Add Student");
        Console.WriteLine("2. Find by ID");
        Console.WriteLine("3. Find by Name");
        Console.WriteLine("4. Top N Students");
        Console.WriteLine("5. Print All");
        Console.WriteLine("6. Exit");
        Console.Write("Choose: ");
    }

    static void AddStudent()
    {
        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("GPA: "); double.TryParse(Console.ReadLine(), out double gpa);
        Console.Write("Faculty: "); string faculty = Console.ReadLine();

        try
        {
            var s = new Student(name, gpa, faculty);
            if (reg.Add(s)) Console.WriteLine($"Added: {s}");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    static void FindById()
    {
        Console.Write("ID: ");
        int.TryParse(Console.ReadLine(), out int id);
        var s = reg.FindById(id);
        Console.WriteLine(s != null ? $"{s}" : "Not found.");
    }

    static void FindByName()
    {
        Console.Write("Name: ");
        var list = reg.FindByName(Console.ReadLine());
        if (list.Length == 0) Console.WriteLine("Not found.");
        else foreach (var s in list) Console.WriteLine(s);
    }

    static void ShowTop()
    {
        Console.Write("How many (N): ");
        int.TryParse(Console.ReadLine(), out int n);
        foreach (var s in reg.GetTopStudents(n)) Console.WriteLine(s);
    }
}
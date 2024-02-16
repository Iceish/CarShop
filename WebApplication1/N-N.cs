namespace WebApplication1
{
public class Student
{
    public string Name { get; set; }
    public IList<Teacher> Teachers { get; set; }
}
public class Teacher
{
    public string Name { get; set; }
    public IList<Student> Students { get; set; }
}
}

namespace lab3.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public HashSet<Student> Students { get; set; } = new HashSet<Student>();
    }
}

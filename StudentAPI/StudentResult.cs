namespace StudentAPI
{
    public class StudentResult
    {
        public string? StudentName { get; set; }

        public int Marks { get; set; }

        public double Average => Marks / TotalSubjects;

        public int TotalSubjects => 5;

        public string? Grade {
            get
            {
                if (Average > 90.0)
                {
                    return "A";
                }
                else if (Average > 80.0)
                {
                    return "B";
                }
                else if(Average > 70.0)
                {
                    return "C";
                }
                else
                {
                    return "D";
                }
            }
        }
    }
}
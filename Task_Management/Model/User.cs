namespace Task_Management.Model
{
    public class User
    {        
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ICollection<TaskModel> TaskModels { get; set; }
    }
}

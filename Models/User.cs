namespace Models
{
    public class User
    {
        
        public User(string Name, int id=0)
        {
            this.Name = Name;
            this.Id = id;
        }

        public string Name { get; }
        public int Id { get; }
    }
}
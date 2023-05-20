namespace PitchFinder.RambitMQ.Events
{
    public class UserAddedIntergrationEvent : IntegrationEvent
    {
        public UserAddedIntergrationEvent()
        {
            
        }
        public UserAddedIntergrationEvent(int id, string email)
        {
            UserId = id;
            Email = email;
        }

        public int UserId { get; set; }
        public string Email { get; set; }
    }
}

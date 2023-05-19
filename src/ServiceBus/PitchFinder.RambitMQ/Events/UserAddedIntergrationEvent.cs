namespace PitchFinder.RambitMQ.Events
{
    public class UserAddedIntergrationEvent : IntegrationEvent
    {
        public UserAddedIntergrationEvent(string email)
        {
            Email = email;
        }  

        public string Email { get; set; }
    }
}

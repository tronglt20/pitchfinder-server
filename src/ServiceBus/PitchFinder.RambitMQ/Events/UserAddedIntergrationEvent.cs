namespace PitchFinder.RambitMQ.Events
{
    public class UserAddedIntergrationEvent : IntegrationEvent
    {
        public UserAddedIntergrationEvent()
        {

        }

        public UserAddedIntergrationEvent(int id, string email, bool isCustomer)
        {
            UserId = id;
            Email = email;
            IsCustomer = isCustomer;
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public bool IsCustomer { get; set; }
    }
}

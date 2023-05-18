﻿using Pitch.Domain.Enums;
using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class Store : BaseEntity<int>
    {
        public Store()
        {
            
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public StoreStatusEnum Status { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
        public int OwnerId { get; set; }

        public User Owner { get; set; }
        public virtual ICollection<Pitch> Pitchs { get; set; } = new HashSet<Pitch>();
    }
}
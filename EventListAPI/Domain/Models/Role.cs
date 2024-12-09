﻿namespace Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
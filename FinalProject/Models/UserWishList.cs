﻿namespace FinalProject.Model
{
    public class UserWishList : Entity<int>
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}

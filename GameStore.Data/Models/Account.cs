﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Data.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] [MaxLength(20)] public string Username { get; set; }

        // SHA-512
        [Required] public string Password { get; set; }
        public string CreditCard { get; set; }
        public string DeletedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsGuest { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }
}
﻿using System;

namespace GameStore.Data.Models.Abstract
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
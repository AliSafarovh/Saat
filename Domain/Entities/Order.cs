﻿using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Order :BaseEntity
    {
        // Müştəri məlumatları
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ? Mail { get; set; }
        public string ? Location { get; set; }
        public string City { get; set; }
        public string AdditionalInfo { get; set; }
        // Sifarişdə olan məhsullar
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Ümumi qiymətlər (toplamaq üçün)
        public decimal TotalPrice => OrderItems.Sum(oi => oi.TotalPrice);
        public decimal TotalDiscountPrice => OrderItems.Sum(oi => oi.TotalDiscountPrice);
    }
}

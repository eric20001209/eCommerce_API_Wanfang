﻿using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Settings
    {
        public int Id { get; set; }
        public string Cat { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool? Hidden { get; set; }
        public bool? BoolValue { get; set; }
        public int? Access { get; set; }
    }
}

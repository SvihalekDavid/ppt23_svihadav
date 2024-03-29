﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ppt23.Shared
{
    public class UkonVM
    {
        public Guid Id { get; set; }
        public string Detail { get; set; } = "";
        public string Kod { get; set; } = "";

        public DateTime DateTime { get; set; } = DateTime.Now;

        public Guid VybaveniId { get; set; }

        public Guid? PracovnikId { get; set; }

        public string? PracovnikName { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airmily.Services.Models
{
    public class TmdbVideo
    {
        public string Id { get; set; }
        public string Iso_639_1 { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
}

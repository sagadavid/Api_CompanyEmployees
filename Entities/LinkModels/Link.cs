﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class Link
    {
        public string? Href { get; set; }

        public string? Rel { get; set; }

        public string? Method { get; set; }

        public Link() { }//empty constructor..We'll need that for XML serialization purposes,
                         //so keep it that way.

        public Link(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}

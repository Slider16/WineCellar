using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineCellar.Net.API.Models
{
    public class LinkDto
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }

        public LinkDto()
        {

        }
        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Tag
    {
        public Tag(string tag)
        {
            this.ItemTag = tag;
        }

        public string ItemTag { get; private set; }

        public override string ToString()
        {
            return this.ItemTag;
        }
    }
}

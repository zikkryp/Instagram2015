using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Pagination
    {
        public Pagination()
        {
            this.hasMorePages = false;
        }

        public Pagination(string nextUrl, string nextMaxId)
        {
            this.NextUrl = nextUrl;
            this.NextMaxId = nextMaxId;
            this.hasMorePages = true;
        }

        public string NextUrl { get; set; }
        public string NextMaxId { get; set; }

        private bool hasMorePages = false;

        public bool HasMorePages
        {
            get
            {
                return this.hasMorePages;
            }
            private set
            {
                this.hasMorePages = value;
            }
        }

        public override string ToString()
        {
            return this.NextUrl;
        }
    }
}

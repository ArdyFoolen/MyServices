using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyServices.Models
{
    public class Session
    {
        public Guid Token { get; set; }
        public EndPoint? EndPoint { get; set; }

        public override string ToString()
        {
            return @$"Session: {Token}
Endpoint: {EndPoint.ToString()}";
        }
    }
}

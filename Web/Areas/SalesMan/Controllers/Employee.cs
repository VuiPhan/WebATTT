using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.SalesMan.Controllers
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public bool Status { get; set; }
    }
}
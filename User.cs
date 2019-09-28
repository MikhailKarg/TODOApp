using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODOApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }

        public List<Task> Tasks {get;set;}
    }
}

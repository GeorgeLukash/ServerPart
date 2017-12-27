using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.DataModel.Auth
{
    public class UserAuthModel
    {
        public int Id { get; set; }
        public int Role { get; set; }
        public string Email { get; set; }
    }
}

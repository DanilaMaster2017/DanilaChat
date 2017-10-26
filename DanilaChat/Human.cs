using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanilaChat
{
    public class Human
    {
        public int UserId { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Brithday { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }

        public override string ToString()
        {
            string result;

            result = UserId + " "
                + Name + " "
                + Surname + " "
                + Brithday + " "
                + Gender;

            if (Avatar != null) result += Avatar;

            return result;
        }
    }
}

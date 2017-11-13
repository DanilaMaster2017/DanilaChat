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

        public static Human Parse(string input)
        {
            string[] parametrs = input.Split();

            Human man = new Human();

            man.UserId = int.Parse(parametrs[0]);
            man.Name = parametrs[1];
            man.Surname = parametrs[2];
            man.Brithday = DateTime.Parse(parametrs[3] + " " + parametrs[4]);
            man.Gender = parametrs[5];
            
            return man;
        }

        public static Human Parse(string[] input, int beginIndex)
        {
            Human man = new Human();

            man.UserId = int.Parse(input[beginIndex]);
            man.Name = input[beginIndex + 1];
            man.Surname = input[beginIndex + 2];
            man.Brithday = DateTime.Parse
                (input[beginIndex + 3] + " " + input[beginIndex + 4]);
            man.Gender = input[beginIndex + 5];

            return man;
        }
    }
}

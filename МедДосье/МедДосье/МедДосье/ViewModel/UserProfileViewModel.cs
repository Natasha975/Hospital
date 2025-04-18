using System;
using System.Collections.Generic;
using System.Text;
using МедДосье.Model;

namespace МедДосье.ViewModel
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public string Email => User?.Email;
        public string FullName => User?.FullName;
        public DateTime BirthDate => User.BirthDate;
        public string Gender => User?.Gender;


        public UserProfileViewModel(User user)
        {
            User = user;
        }
    }
}
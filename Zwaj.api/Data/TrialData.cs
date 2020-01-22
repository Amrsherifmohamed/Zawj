using System.Collections.Generic;
using Newtonsoft.Json;
using Zwaj.api.Models;  
namespace Zwaj.api.Data
{
    public class TrialData
    {
        private readonly Datacontext _context;
        public TrialData(Datacontext conext)
        {
            _context=conext;
        }
        public void TrialUsers(){
            var userData=System.IO.File.ReadAllText("Data/UserTrialData.json");
            //to read data from json file and save in object userData
            var users =JsonConvert.DeserializeObject<List<User>>(userData);
            //to convert json object from jeson file to object users
            foreach (var user in users)
            {
                byte[] passwordHash,passwordSalt;
                createpasswordhash("password",out passwordHash,out passwordSalt);
                user.Passwordhash=passwordHash;
                user.PasswordsSalt=passwordSalt;  
                user.Username=user.Username.ToLower();
                _context.Add(user);
            }
            _context.SaveChanges();

        }
          private void createpasswordhash(string password ,out byte[] passwordhash,out byte[] passwordsolt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA512()){
                passwordsolt=hmac.Key;
                passwordhash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
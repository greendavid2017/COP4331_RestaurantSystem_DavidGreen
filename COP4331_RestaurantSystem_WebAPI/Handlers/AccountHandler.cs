using COP4331_RestaurantSystem_WebAPI.Helpers;
using COP4331_RestaurantSystem_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COP4331_RestaurantSystem_WebAPI.Handlers
{
    public class AccountHandler
    {
        // API calls which require a user to be logged in call this method to determine if the user has a valid token
        public bool isTokenValid(String apiToken, String email)
        { 
            // Check if a token and email address was provided in the header
            if (apiToken != null && email != null)
            {
                // Decipher the token using the agreed upon deciphering method
                var decipheredToken = EncryptionHelper.aesDecipher(apiToken, email);

                // The deciphered token should be a DateTime if it is valid,
                // If it does parse as a DateTime and the DateTime is in the future the user is still authorized
                DateTime expires;
                if (DateTime.TryParse(decipheredToken, out expires))
                {
                    if (DateTime.Now < expires)
                    {
                        return true;
                    }
                }
            }
            return false;
            
        }

        public bool LoginDb(String email, String password)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                // Search the database for a registered user with the corresponding email and password
                var encryptedPassword = EncryptionHelper.sha256(password);
                var user = db.Users.Where(u => u.Email == email && u.Password == encryptedPassword).FirstOrDefault();

                return (user != null);
            }
        }

        public bool RegisterDb(String email, String password)
        {
            using (var db = new RestaurantSystemDataContext())
            {
                // Check and make sure this email doesn't already exist
                var user = db.Users.Where(u => u.Email == email).FirstOrDefault();

                // If a user with the given username does not already exist, we can proceed with registration
                if (user == null)
                {
                    db.Users.Add(new User()
                    {
                        Email = email,
                        Password = EncryptionHelper.sha256(password),
                        IsEmployee = false
                    });

                    db.SaveChanges();
                    return true;
                }

                return false;

            }
        }
    }
}
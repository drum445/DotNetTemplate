using System;
using System.Linq;

namespace backend.Common
{
    public class UserHelper
    {
        public string GetPersonId(System.Security.Claims.ClaimsPrincipal user)
        {
            var userId = (from c in user.Claims
                          where c.Type == "Id"
                          select c.Value).FirstOrDefault();
            return userId;

        }
    }
}

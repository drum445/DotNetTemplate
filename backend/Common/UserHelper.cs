using System;

namespace backend.Common
{
    public class UserHelper
    {
        public static Guid GetPersonId(System.Security.Claims.ClaimsPrincipal user)
        {
            // var userId = (from c in user.Claims
            //               where c.Type == "Id"
            //               select c.Value).FirstOrDefault();

            var userId = user.FindFirst("Id").Value;
            return Guid.Parse(userId);
        }
    }
}

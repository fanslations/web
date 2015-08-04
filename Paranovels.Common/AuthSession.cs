using System.Collections;
using System.Collections.Generic;

namespace Paranovels.Common
{
    public class AuthSession
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IList<int> HiddenSeriesIDs { get; set; }
    }
}

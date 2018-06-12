using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [DataContract]
    public class ULoginUser
    {
        #region Public Properties

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember]
        public string Identity { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "network")]
        public string Network { get; set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember(Name = "profile")]
        public string Profile { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember(Name = "uid")]
        public string Uid { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format("network={0},uid={1},first_name={2},last_name={3}", Network, Uid, FirstName, LastName);
        }
    }
}

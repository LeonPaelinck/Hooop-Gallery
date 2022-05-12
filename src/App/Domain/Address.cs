using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
   public class Address : ValueObject {
       public string Land { get; }
       public string Gemeente { get; }
       public string Postcode { get; }
       public string Straat { get; }

       public Address(string land, string gemeente, string postcode, string straat) {
           Land = Guard.Against.NullOrEmpty(land, nameof(land));
           Gemeente = Guard.Against.NullOrEmpty(gemeente, nameof(gemeente));
           Postcode = Guard.Against.NullOrEmpty(postcode, nameof(postcode));
           Straat = Guard.Against.NullOrEmpty(straat, nameof(straat));
       }

       protected override IEnumerable<object> GetEqualityComponents() {
           yield return Straat.ToLower();
           yield return Land.ToLower();
           yield return Postcode.ToLower();
           yield return Gemeente.ToLower();
       }
   }
}


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASXList.Models
{
    public class Company
    {
        string headOfficeAddress = string.Empty;
        bool isforeign = false;
        [JsonProperty(PropertyName ="code")]
        public string ASXCode { get; set; }

        [JsonProperty(PropertyName = "name_full")]
        public string Name { get; set; }

        //[JsonProperty(PropertyName = "mailing_address")] 
        //public Address  HeadOfficeAddress { get; set; }

        //[JsonProperty(PropertyName = "registry_address")]
        //public Address RegisteredOfficeAddress { get; set; }

        [JsonProperty(PropertyName = "mailing_address")]
        public string HeadOfficeAddress
        {
            get
            {
                return headOfficeAddress;
            }
            set
            {
                headOfficeAddress = value;
                if (!string.IsNullOrEmpty(headOfficeAddress))
                {
                    if (headOfficeAddress.ToUpper().Contains("AUSTRALIA"))
                    {
                        isforeign = false;
                    }
                    else
                    {
                        isforeign = true;
                    }
                }
            }
        }

        [JsonProperty(PropertyName = "registry_address")]
        public string RegisteredOfficeAddress { get; set; }

        [JsonProperty(PropertyName = "foreign_exempt")]
        public bool IsForeign { get { return isforeign; }  }
    }

    public class Address
    {
        public string FullAddress { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
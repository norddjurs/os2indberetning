﻿
namespace Core.DomainModel
{
    public enum PersonalAddressType
    {
        Standard,
        Home,
        Work,
        AlternativeHome,
        AlternativeWork
    }

    public class PersonalAddress : Address
    {
        public PersonalAddressType Type { get; set; }

        public virtual Person Person { get; set; }
    }
}
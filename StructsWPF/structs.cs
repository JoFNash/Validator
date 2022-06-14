using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;

namespace StructsWPF
{
    public struct FullnameWPF
    {
        public string Name;
        public string Surname;
        public string Patronymic;
    }

    public struct DataRequestWPF
    {
        public FullnameWPF Fullname;
        public List<string> PhoneNumbers;
        public List<string> Emails;
        public List<string> Addresses;
        public string Passport;
        public Timestamp BirthDate;
    }
    
    public struct ValidateStringWPF
    {
        public string Value;
        public bool IsValid;
        public string Comment;
    }
    
    public struct FullnameReplyWPF 
    {
        public ValidateStringWPF Name;
        public ValidateStringWPF Surname;
        public ValidateStringWPF Patronymic;
    }
    
    public struct ValidateTimestampWPF
    {
        public Timestamp Value;
        public bool IsValid;
        public string Comment;
    }

    public struct DataReplyWPF
    {
        public FullnameReplyWPF Fullname;
        public List<ValidateStringWPF> PhoneNumbers;
        public List<ValidateStringWPF> Emails;
        public List<ValidateStringWPF> Addresses;
        public ValidateStringWPF Passport;
        public ValidateTimestampWPF BirthDate;
    }
    
    public struct FullnameRequestWPF 
    {
        public string Name;
        public string Surname;
        public string Patronymic;
    }
    
}
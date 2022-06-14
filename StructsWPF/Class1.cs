using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;

namespace StructsWPF
{
    public class Class1
    {
        public struct FullnameWPF
        {
            public string name;
            public string surname;
            public string patronymic;
        }

        public struct DataRequestWPF
        {
            public FullnameWPF fullName;
            public List<string> phoneNumbers;
            public List<string> emails;
            public List<string> addresses;
            public string passport;
            public Timestamp birthDate;
        }
        
        public struct ValidateStringWPF
        {
            public string Value;
            public bool IsValid;
            public string comment;
        }
        
        public struct FullnameReplyWPF 
        {
            public ValidateStringWPF name;
            public ValidateStringWPF surname;
            public ValidateStringWPF patronymic;
        }
        
        public struct ValidateTimestampWPF
        {
            public Timestamp Value;
            public bool IsValid;
            public string comment;
        }

        public struct DataReplyWPF
        {
            public FullnameReplyWPF fullname;
            public List<ValidateStringWPF> phoneNumbers;
            public List<ValidateStringWPF> emails;
            public List<ValidateStringWPF> addresses;
            public ValidateStringWPF passport;
            public ValidateTimestampWPF birthDate;
        }
        
        public struct FullnameRequestWPF 
        {
            public string name;
            public string surname;
            public string patronymic;
        }
    }
}
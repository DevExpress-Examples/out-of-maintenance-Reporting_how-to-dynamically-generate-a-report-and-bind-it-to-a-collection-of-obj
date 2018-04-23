using System;
using System.Collections.Generic;

namespace K18078 {
    public class MyComplexObject {
        int age;

        [Reportable(LenFactor = 1)]
        public int Age {
            get { return age; }
            set { age = value; }
        }
        string firstName;
        //[Reportable(LenFactor = 2)]
        public string FirstName {
            get { return firstName; }
            set { firstName = value; }
        }
        string lastName;
        //[Reportable(LenFactor = 2,AlternateName = "Surname")]
        public string LastName {
            get { return lastName; }
            set { lastName = value; }
        }

        [Reportable(LenFactor = 3, AlternateName = "Full name")]
        public string Name {
            get { return firstName + " " + lastName; }
        }
        string address;

        [Reportable(LenFactor = 3)]
        public string Address {
            get { return address; }
            set { address = value; }
        }
        string city;

        [Reportable(LenFactor = 1)]
        public string City {
            get { return city; }
            set { city = value; }
        }
        string federalId;

        [Reportable(AlternateName = "Social Security Number", LenFactor = 3)]
        public string FederalId {
            get { return federalId; }
            set { federalId = value; }
        }
        double salary;

        public double Salary {
            get { return salary; }
            set { salary = value; }
        }
    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class Reportable : Attribute {
        private string altName;
        public string AlternateName {
            get { return this.altName; }
            set { this.altName = value; }
        }
        private int lenFactor;
        public int LenFactor {
            get { return this.lenFactor; }
            set { this.lenFactor = value; }
        }
    }
}

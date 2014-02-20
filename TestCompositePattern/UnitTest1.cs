﻿using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TestCompositePattern
{
    [TestFixture]
    public class UnitTest1
    {
        private Person _party;

        [TestFixtureSetUp]
        public void Startup()
        {
            _party = new Person();
        }

        [Test]
        public void ConstructPerson()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(_party);
        }

        [Test]
        public void GetName_GivenPerson_ShouldGetName()
        {
            //---------------Set up test pack-------------------
            string name = "Test";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            _party.Name = name;
            //---------------Test Result -----------------------
            Assert.AreEqual("Test",_party.Name);
        }

        [Test]
        public void GetGold_Given500Gold_ShouldReturn500Gold()
        {
            //---------------Set up test pack-------------------
            int gold = 500;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            _party.Gold = gold;
            //---------------Test Result -----------------------
            Assert.AreEqual(500,_party.Gold);
        }

        [Test]
        public void Construct_GivenGroup()
        {
            //---------------Set up test pack-------------------
            var group = new MyGroup();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsNotNull(group);
        }

        [Test]
        public void Add_GivenPerson_ShouldHaveGroupSizeGreaterThanZero()
        {
            //---------------Set up test pack-------------------
            var group = new MyGroup();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            group.Add(_party);
            //---------------Test Result -----------------------
            Assert.IsTrue(group.Size > 0);
        }

        [Test]
        public void GetGold_GivenGroupWithPerson_ShouldReturnAllGoldOfPersons()
        {
            //---------------Set up test pack-------------------
            var group = new MyGroup();
            group.Name = "Test";
            var person = new Person();
            person.Name = "Steven";
            group.Add(person);
            group.Gold = 500;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var groupGold = group.Gold;
            //---------------Test Result -----------------------
            Assert.AreEqual(500,groupGold);
        }

        [Test]
        public void GetGoldPerPerson_GivenGroupWithMultiplePersons_ShouldReturn75()
        {
            //---------------Set up test pack-------------------
            var group = new MyGroup();
            group.Name = "test";
            group.Add(new Person() { Name = "A" });
            group.Add(new Person() { Name = "B" });
            group.Add(new Person() { Name = "C" });
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            group.Gold = 225;
            //---------------Test Result -----------------------
            Assert.AreEqual(75,group.Parties[0].Gold);
        }


        [Test]
        public void GetGoldPerPerson_GivenLayeredGroups_ShouldReturnIndividualAmounts()
        {
            //---------------Set up test pack-------------------
            IParty steve = new Person() {Name = "Steve"};
            IParty dan = new Person() {Name = "Dan"};
            IParty matt = new Person() {Name = "Matt"};
            IParty bob1 = new Person() {Name = "Bob1"};
            IParty bob2 = new Person() {Name = "Bob2"};
            IParty julia = new Person() {Name = "Julia"};
            IParty james = new Person() {Name = "James"};
            
            var group = new MyGroup();
            group.Name = "Devs";
            group.Add(steve);
            group.Add(dan);
            group.Add(matt);
            
            var group2 = new MyGroup();
            group2.Name = "Bobs";
            group2.Add(bob1);
            group2.Add(bob2);

            var root = new MyGroup() {Name = "Root"};
            root.Add(group);
            root.Add(group2);
            root.Add(julia);
            root.Add(james);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            root.Gold = 1000;
            //---------------Test Result -----------------------
            Assert.AreEqual(250,julia.Gold);
            Assert.AreEqual(250,james.Gold);
            Assert.AreEqual(125,bob1.Gold);
            Assert.AreEqual(125,bob2.Gold);
            Assert.AreEqual(84,steve.Gold);
            Assert.AreEqual(83,dan.Gold);
            Assert.AreEqual(83,matt.Gold);
        }
    }

    public class MyGroup : IParty
    {
        public void Add(IParty person)
        {
            this.Parties.Add(person);
        }

        public List<IParty> Parties = new List<IParty>();
        private int _gold;

        public int Size
        {
            get { return Parties.Count; }
        }

        public int Gold
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
                int goldToSplit = _gold/Parties.Count;
                int goldLeftOver = _gold%Parties.Count;
                foreach (var person in Parties)
                {
                    person.Gold = goldToSplit + goldLeftOver;
                    goldLeftOver = 0;
                }
            }
            
        }

        public string Name { get; set; }
    }

    public interface IParty
    {
        int Gold { get; set; }
        string Name { get; set; }

        void Add(IParty party);

    }

    public class Person : IParty
    {
        public string Name { get; set; }
        public void Add(IParty party)
        {
            Parties.Add(party);
        }

        public List<IParty> Parties { get; set; }

        public int Gold { get; set; }
    }
}

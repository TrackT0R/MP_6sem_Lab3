using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HashTable;
using System.Collections.Generic;

namespace HashTableTest
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void NewTableIsNotNull()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            Assert.IsNotNull(HashTable);
        }

        [TestMethod]
        public void NewTableCountIsZero()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            Assert.AreEqual(0, HashTable.Count);
        }

        [TestMethod]
        public void CapacityIncreasesAfterAdding()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            int oldCapacity = HashTable.Capacity;
            HashTable.Add(1, "a");
            HashTable.Add(2, "a");
            HashTable.Add(3, "a");
            HashTable.Add(5, "a");
            HashTable.Add(6, "a");
            HashTable.Add(7, "a");
            
            Assert.IsTrue(oldCapacity < HashTable.Capacity);
        }

        [TestMethod]
        public void CountIncrementesAfterAdd()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            int oldCount = HashTable.Count;

            HashTable.Add(1, string.Empty);

            int actualCount = HashTable.Count;

            Assert.AreEqual(actualCount, oldCount + 1);
        }

        [TestMethod]
        public void ExceptionAfterAddingExistItem()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();
            HashTable.Add(1, string.Empty);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashTable.Add(1, "a");
            });
        }

        [TestMethod]
        public void ExceptionWithNotSimpleEven()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>(capacity: 4);

            Assert.ThrowsException<ApplicationException>(() =>
            {
                HashTable.Add(0, string.Empty);
                HashTable.Add(2, string.Empty);
                HashTable.Add(4, string.Empty);
            });
        }

        [TestMethod]
        public void ExceptionItemNotExistRemoving()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashTable.Remove(1);
            });
        }

        [TestMethod]
        public void NewTableDoesntContainsItem()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            Assert.IsFalse(HashTable.ContainsKey(1));
        }

        [TestMethod]
        public void ItemContainsAfterAdding()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();
            HashTable.Add(1, string.Empty);

            Assert.IsTrue(HashTable.ContainsKey(1));
        }

        [TestMethod]
        public void ItemNotContainsAfterRemoving()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();
            HashTable.Add(1, string.Empty);
            HashTable.Remove(1);

            Assert.IsFalse(HashTable.ContainsKey(1));
        }

        [TestMethod]
        public void CountDecrementesAfterRemoving()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();
            HashTable.Add(1, string.Empty);

            int oldCount = HashTable.Count;
            HashTable.Remove(1);
            int actualCount = HashTable.Count;

            Assert.AreEqual(actualCount, oldCount - 1);
        }

        [TestMethod]
        public void ExceptionItemNotContains()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();

            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                var value = HashTable[0];
            });
        }

        [TestMethod]
        public void NewTableFirstItem()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();
            string testValue = "testValue";
            HashTable.Add(0, testValue);

            var value = HashTable[0];

            Assert.AreEqual(testValue, value);
        }

        [TestMethod]
        public void AllItemsContains()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>();
            HashTable.Add(0, "a");
            HashTable.Add(1, "b");
            HashTable.Add(2, "c");
            HashTable.Add(3, "d");

            List<KeyValuePair<int, string>> AddedInHashTableItems = new List<KeyValuePair<int, string>>();
            AddedInHashTableItems.Add(new KeyValuePair<int, string>(0, "a"));
            AddedInHashTableItems.Add(new KeyValuePair<int, string>(1, "b"));
            AddedInHashTableItems.Add(new KeyValuePair<int, string>(2, "c"));
            AddedInHashTableItems.Add(new KeyValuePair<int, string>(3, "d"));


            int k = 0;
            foreach (var item in HashTable) {
                Assert.AreEqual(item, AddedInHashTableItems[k++]);
            }
        }

        [TestMethod]
        public void CapacityMustBeIncreasedAfterAddingSoManyItemsAfterInitHashTableWithAssignedCapacity()
        {
            OpenAddressHashTable<int, string> HashTable = new OpenAddressHashTable<int, string>(19);

            for (int i = 0; i < 19; i++) {
                HashTable.Add(i, string.Empty);
            }

            Assert.AreEqual(61, HashTable.Capacity);
        }
    }
}

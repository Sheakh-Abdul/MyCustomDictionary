using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2.Tests
{
    [TestFixture]
    public class MyCustomDictionaryTests
    {
        private MyCustomDictionary<string, int> _dict;

        [SetUp]
        public void Setup()
        {
            _dict = new MyCustomDictionary<string, int>();
        }

        [Test]
        public void Add_ShouldAddNewItems()
        {
            Assert.IsTrue(_dict.Add("one", 1));
            Assert.IsTrue(_dict.Add("two", 2));
            Assert.AreEqual(2, _dict.Count);
        }

        [Test]
        public void Add_DuplicateKey_ShouldReturnFalse()
        {
            _dict.Add("one", 1);
            Assert.IsFalse(_dict.Add("one", 10));
            Assert.AreEqual(1, _dict.Count);
        }

        [Test]
        public void Indexer_ShouldGetAndSetValue()
        {
            _dict["a"] = 10;
            Assert.AreEqual(10, _dict["a"]);

            _dict["a"] = 20;
            Assert.AreEqual(20, _dict["a"]);
        }

        [Test]
        public void ContainsKey_ShouldReturnCorrectResult()
        {
            _dict.Add("x", 5);
            Assert.IsTrue(_dict.ContainsKey("x"));
            Assert.IsFalse(_dict.ContainsKey("y"));
        }

        [Test]
        public void Remove_ShouldRemoveExistingKey()
        {
            _dict.Add("key", 100);
            Assert.IsTrue(_dict.Remove("key"));
            Assert.IsFalse(_dict.ContainsKey("key"));
            Assert.AreEqual(0, _dict.Count);
        }

        [Test]
        public void Remove_NonExistingKey_ShouldReturnFalse()
        {
            Assert.IsFalse(_dict.Remove("nope"));
        }

        [Test]
        public void RemoveValue_ShouldRemoveFirstMatchingValue()
        {
            _dict.Add("a", 1);
            _dict.Add("b", 2);
            _dict.Add("c", 1);

            Assert.IsTrue(_dict.RemoveValue(1));
            Assert.AreEqual(2, _dict.Count);
        }

        [Test]
        public void RemoveAllValue_ShouldRemoveAllMatchingValues()
        {
            _dict.Add("a", 5);
            _dict.Add("b", 5);
            _dict.Add("c", 10);

            int removedCount = _dict.RemoveAllValue(5);
            Assert.AreEqual(2, removedCount);
            Assert.AreEqual(1, _dict.Count);
            Assert.IsTrue(_dict.ContainsKey("c"));
        }

        [Test]
        public void Clear_ShouldEmptyDictionary()
        {
            _dict.Add("one", 1);
            _dict.Clear();
            Assert.AreEqual(0, _dict.Count);
            Assert.IsFalse(_dict.ContainsKey("one"));
        }

        [Test]
        public void Enumeration_ShouldReturnAllItems()
        {
            _dict.Add("one", 1);
            _dict.Add("two", 2);
            var items = new Dictionary<string, int>(_dict);

            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(1, items["one"]);
            Assert.AreEqual(2, items["two"]);
        }

        [Test]
        public void Rehash_ShouldKeepAllItems()
        {
            for (int i = 0; i < 100; i++)
            {
                _dict.Add($"key{i}", i);
            }
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(_dict.ContainsKey($"key{i}"));
                Assert.AreEqual(i, _dict[$"key{i}"]);
            }
            Assert.AreEqual(100, _dict.Count);
        }

        [Test]
        public void ThreadSafety_ShouldHandleConcurrentAdds()
        {
            var dict = new MyCustomDictionary<int, int>();
            int itemCount = 1000;
            Parallel.For(0, itemCount, i =>
            {
                dict.Add(i, i);
            });

            Assert.AreEqual(itemCount, dict.Count);
            for (int i = 0; i < itemCount; i++)
            {
                Assert.IsTrue(dict.ContainsKey(i));
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab3
{
    public class OpenAddressHashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IHashTable<TKey, TValue> where TKey: IEquatable<TKey>
    {
        #region Data
        private int[] SimpleNumbers = new int[] { 7, 19, 61, 167, 359, 857, 1721, 3469,
            7103, 14177, 29063, 50833, 99991, 331777, 614657, 1336337, 4477457,
            8503057, 29986577, 45212107, 99990001, 126247697 };

        Pair<TKey, TValue>[] _table;
        private int _capacity;
        HashMaker<TKey> _hashMaker1, _hashMaker2;

        public int Count { get; private set; }
        private double FillFactor = 0.85;
        #endregion
        
        #region Constructors
        public OpenAddressHashTable() : this(9973, 0.77)
        { }
        
        public OpenAddressHashTable(double fillfactor) : this(7, fillfactor)
        { }
        
        public OpenAddressHashTable(int capacity) : this(capacity, 0.77)
        { }
        
        public OpenAddressHashTable(int m, double FillFactor)
        {
            _table = new Pair<TKey, TValue>[m];
            _capacity = m;
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
            this.FillFactor = FillFactor;
        }
        #endregion


        #region Private methods
        private bool TryToPut(int place, TKey key, TValue value)
        {
            //ячейка свободна
            if (_table[place] == null || _table[place].IsDeleted()) {
                _table[place] = new Pair<TKey, TValue>(key, value);
                Count++;
                return true;
            }
            //такой элемент существует
            if (_table[place].Key.Equals(key)) {
                throw new ArgumentException();
            }
            //ячейка занята другим элементом - коллизия
            return false;
        }

        private Pair<TKey, TValue> Find(TKey x)
        {
            var hash = _hashMaker1.ReturnHash(x);
            if (_table[hash] == null)
                return null;
            if (!_table[hash].IsDeleted() && _table[hash].Key.Equals(x)) {
                return _table[hash];
            }
            int iterationNumber = 1;
            while (true) {
                var place = (hash + iterationNumber * (1 + _hashMaker2.ReturnHash(x))) % _capacity;
                if (_table[place] == null)
                    return null;
                if (!_table[place].IsDeleted() && _table[place].Key.Equals(x)) {
                    return _table[place];
                }
                iterationNumber++;
                if (iterationNumber >= _capacity)
                    return null;
            }
        }

        private void IncreaseTable()
        {
            //если не найдёт, выкинет исключение
            _capacity = SimpleNumbers.First(item => item > _capacity);

            var oldTable = this._table;

            _table = new Pair<TKey, TValue>[_capacity];

            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);

            Count = 0;

            foreach (var pair in oldTable) {
                if (pair != null && !pair.IsDeleted()) {
                    Add(pair.Key, pair.Value);
                }
            }
        }
        #endregion

        #region Public methods
        public void Add(TKey key, TValue value)
        {
            var hash = _hashMaker1.ReturnHash(key);

            if (!TryToPut(hash, key, value)) // ячейка занята
            {
                int iterationNumber = 1;
                while (true) {
                    var place = (hash + iterationNumber * (1 + _hashMaker2.ReturnHash(key))) % _capacity;
                    if (TryToPut(place, key, value))
                        break;
                    iterationNumber++;
                    if (iterationNumber >= _capacity)
                        throw new ApplicationException("HashTable full!!!");
                }
            }
            if ((double)Count / _capacity >= FillFactor) {
                IncreaseTable();
            }
        }

        public void Remove(TKey key)
        {
            var pair = Find(key);
            if (pair == null)
                throw new ArgumentException();
            if (pair.DeletePair())
                Count--;
        }

        public bool ContainsKey(TKey key)
        {
            return Find(key) != null;
        }
        #endregion
        

        #region get, set
        public TValue this[TKey key]
        {
            get
            {
                var pair = Find(key);
                if (pair == null)
                    throw new KeyNotFoundException();
                return pair.Value;
            }

            set
            {
                var pair = Find(key);
                if (pair== null)
                    throw new KeyNotFoundException();
                pair.Value = value;
            }
        }
        #endregion

        #region Enumerator
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from pair in _table where pair != null && !pair.IsDeleted() select new KeyValuePair<TKey, TValue>(pair.Key, pair.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsConsoleApp
{
    public class MyDict<TK, TV>: IEnumerable<KeyValuePair<TK,TV>>
    {
        private const int IntialSize = 20;

        private LinkedList<KeyValuePair<TK, TV>>[] _valuePairs;

        public MyDict()
        {
            this._valuePairs = new LinkedList<KeyValuePair<TK, TV>>[IntialSize];
        }

        public int Count { get; private set; }

        public int Capacity
        {
            get { return this._valuePairs.Length; }
        }

        public void Add(TK key, TV value)
        {
            var hash = this.HashKey(key);
            if (this._valuePairs[hash] == null)
            {
                this._valuePairs[hash] = new LinkedList<KeyValuePair<TK, TV>>();
            }

            var keyAlreadyExists = this._valuePairs[hash].Any(x => x.Key.Equals(key));

            if (keyAlreadyExists)
            {
              throw new InvalidOperationException("The key already exists, can not add two elements with same key.");
            }

            var keyValuePair = new KeyValuePair<TK,TV>(key, value);
            this._valuePairs[hash].AddLast(keyValuePair);
            this.Count++;

            if (this.Count >= this.Capacity * 0.75)
            {
                this.ResizeAndReAddValues();
            }
        }

        public TV Find(TK key)
        {
            var hash = key.GetHashCode();
            if (this._valuePairs[hash] == null)
            {
                return default(TV);
            }
            var valuePair = this._valuePairs[hash];
            return valuePair.First(x => x.Key.Equals(key)).Value;
        }

        public bool ContainsKey(TK key)
        {
            var hash = key.GetHashCode();
            if (this._valuePairs[hash] == null)
            {
                return false;
            }
            var valuePair = this._valuePairs[hash];
            return valuePair.Any(x => x.Key.Equals(key));
        }

        private int HashKey(TK Key)
        {
           return Math.Abs(Key.GetHashCode()) % this.Capacity;
        }

        private void ResizeAndReAddValues()
        {
            //cache the old values
            var cacheValuePairs = this._valuePairs;
            //resize
            this._valuePairs = new LinkedList<KeyValuePair<TK, TV>>[2 * this.Capacity];
            //Add values
            this.Count = 0;
            foreach (var cacheValuePair in cacheValuePairs)
            {
                if (cacheValuePair != null)
                {
                    foreach (var valuePair in cacheValuePair)
                    {
                        this.Add(valuePair.Key, valuePair.Value);
                    }
                }
            }
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            foreach (var valuePair in this._valuePairs)
            {
                if (valuePair != null)
                {
                    foreach (var value in valuePair)
                    {
                        yield return value;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

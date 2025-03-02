using System;
using System.Collections.Generic;
using System.Linq;

namespace UniHelper {
    public class ObservableListDictionary<TKey, TValue> {
        readonly Dictionary<TKey, List<TValue>> internalDictionary = new();
        IReadOnlyDictionary<TKey, IEnumerable<TValue>> cachedDictionary;

        bool isCacheValid;

        public event Action OnDictionaryChange;

        public IReadOnlyDictionary<TKey, IEnumerable<TValue>> ReadOnlyDictionary {
            get {
                if (!isCacheValid) {
                    cachedDictionary = internalDictionary.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.AsReadOnly() as IEnumerable<TValue>
                    );
                
                    isCacheValid = true;
                }

                return cachedDictionary;
            }
        }

        public void Add(TKey key, TValue value) {
            if (!internalDictionary.ContainsKey(key)) {
                internalDictionary[key] = new List<TValue>();
            }

            internalDictionary[key].Add(value);
            HandleDictionaryChange();
        }

        public bool Remove(TKey key, TValue value) {
            if (internalDictionary.TryGetValue(key, out var list)) {
                bool removed = list.Remove(value);
                if (removed) {
                    HandleDictionaryChange();
                }

                return removed;
            }

            return false;
        }

        public void Clear(TKey key) {
            if (internalDictionary.ContainsKey(key)) {
                internalDictionary[key].Clear();
                HandleDictionaryChange();
            }
        }

        public void Clear() {
            internalDictionary.Clear();
            HandleDictionaryChange();
        }

        void HandleDictionaryChange() {
            isCacheValid = false;
            OnDictionaryChange?.Invoke();
        }
    }
}
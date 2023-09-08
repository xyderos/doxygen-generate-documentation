#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Example.ConcurrentPriorityQueue.Interfaces;

#endregion

namespace Example.ConcurrentPriorityQueue.MinimumBinaryHeap;
/// <summary>
/// Implements the IMinimumBinaryHeap used for the priority queue
/// </summary>
/// <typeparam name="TP">The type of the key</typeparam>
/// <typeparam name="T">The type of the item to be inserted</typeparam>
public sealed class MinimumBinaryHeap<TP, T> : IMinimumBinaryHeap<TP, T>
    where TP : IEquatable<TP>, IComparable<TP>
{
    /// <summary>
    /// The internal representation of the heap is a list
    /// </summary>
    private volatile List<KeyValuePair<TP, T>> _items;

    /// <summary>
    /// Default constructor
    /// </summary>
    public MinimumBinaryHeap()
    {
        _items = new List<KeyValuePair<TP, T>>();
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="toCopy">The IMinimumBinaryHeap to copy the elements from</param>
    public MinimumBinaryHeap(IMinimumBinaryHeap<TP, T> toCopy)
    {
        _items = new List<KeyValuePair<TP, T>>(toCopy.Items() as IEnumerable<KeyValuePair<TP, T>> ??
                                               Array.Empty<KeyValuePair<TP, T>>());
    }

    /// <summary>
    /// Get the items as an ICollection
    /// </summary>
    /// <returns>The items as an ICollection</returns>
    public ICollection Items()
    {
        return _items;
    }

    /// <summary>
    /// Return the number of elements in the heap
    /// </summary>
    /// <returns>Number of elements in the heap</returns>
    public int Count()
    {
        return _items.Count;
    }

    /// <summary>
    /// Clear the heap
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }

    /// <summary>
    /// Remove and return the item with the highest priority from the binary heap
    /// </summary>
    /// <returns>Either null that indicates that the heap is empty, the KeyPairValue otherwise</returns>
    public KeyValuePair<TP, T>? Remove()
    {
        return Delete();
    }
    
    /// <summary>
    /// Peek the first element in the binary heap
    /// </summary>
    /// <returns>Either null that indicates that the heap is empty, the KeyPairValue otherwise</returns>
    public bool Insert(TP key, T value)
    {
        return Insert(new KeyValuePair<TP, T>(key, value));
    }

    /// <summary>
    /// Peek the first item in the heap
    /// </summary>
    /// <returns></returns>
    public KeyValuePair<TP, T>? Peek()
    {
        return _items.Count == 0 ? null : _items[0];
    }

    /// <summary>
    /// Internal class that inserts the KeyValuePair in the heap
    /// </summary>
    /// <param name="entry">The KeyValuePair entry to be inserted</param>
    /// <returns>Whether the operation was successful or not</returns>
    private bool Insert(KeyValuePair<TP, T> entry)
    {
        if (_items.Any(x => Equals(x.Key, entry.Key)))
            return false;

        if (entry.Value is null)
            return false;

        _items.Add(entry);

        var pos = _items.Count - 1;

        if (pos == 0) return true;

        while (pos > 0)
        {
            var nextPos = (pos - 1) / 2;

            var toCheck = _items[nextPos];

            if (entry.Key.CompareTo(toCheck.Key) < 0)
            {
                _items[pos] = toCheck;
                pos = nextPos;
            }
            else
            {
                break;
            }
        }

        _items[pos] = entry;

        return true;
    }

    /// <summary>
    /// Internal method that deletes the first item in the heap
    /// </summary>
    /// <returns></returns>
    private KeyValuePair<TP, T>? Delete()
    {
        if (_items.Count == 0) return null;

        var toReturn = _items[0];

        if (_items.Count <= 2)
        {
            _items.RemoveAt(0);
        }
        else
        {
            _items[0] = _items[^1];
            _items.RemoveAt(_items.Count - 1);

            int current = 0, possibleSwap = 0;

            while (true)
            {
                int leftChildPos = 2 * current + 1, rightChildPos = leftChildPos + 1;

                if (leftChildPos < _items.Count)
                {
                    var entry1 = _items[current];
                    var entry2 = _items[leftChildPos];

                    if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = leftChildPos;
                }
                else
                {
                    break;
                }

                if (rightChildPos < _items.Count)
                {
                    var entry1 = _items[possibleSwap];
                    var entry2 = _items[rightChildPos];

                    if (entry2.Key.CompareTo(entry1.Key) < 0) possibleSwap = rightChildPos;
                }

                if (current != possibleSwap)
                    (_items[current], _items[possibleSwap]) = (_items[possibleSwap], _items[current]);
                else break;

                current = possibleSwap;
            }
        }

        return toReturn;
    }
}
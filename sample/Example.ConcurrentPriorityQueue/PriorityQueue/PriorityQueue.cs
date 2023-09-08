#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Example.ConcurrentPriorityQueue.Interfaces;
using Example.ConcurrentPriorityQueue.MinimumBinaryHeap;

#endregion

namespace Example.ConcurrentPriorityQueue.PriorityQueue;

/// <summary>
/// Implements the interface for a generic binary heap that associates a TP type with a T value
/// </summary>
/// <typeparam name="TP">The type of the key</typeparam>
/// <typeparam name="T">The type of the item to be inserted</typeparam>
public class PriorityQueue<TP, T>
    : IPriorityQueue<TP, T> where TP : IComparable<TP>, IEquatable<TP>
{
    /// <summary>
    /// The binary heap that is used for internal representation
    /// </summary>
    private volatile IMinimumBinaryHeap<TP, T> _minHeap = new MinimumBinaryHeap<TP, T>();

    /// <summary>
    /// The lock
    /// </summary>
    private volatile object _syncLock = new();

    /// <summary>
    /// Return the number of elements in the queue
    /// </summary>
    public int Count
    {
        get
        {
            lock (_syncLock)
            {
                return _minHeap.Count();
            }
        }
    }

    /// <summary>
    /// Indicates if the collection is thread safe
    /// </summary>
    bool ICollection.IsSynchronized => true;

    /// <summary>
    /// The lock 
    /// </summary>
    object ICollection.SyncRoot => _syncLock;
    
    /// <summary>
    /// Enumerator that iterates over the collection
    /// </summary>
    /// <returns>The enumerator</returns>
    public IEnumerator<KeyValuePair<TP, T>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<TP, T>>)ToArray()).GetEnumerator();
    }

    /// <summary>
    /// Enumerator that iterates over the collection
    /// </summary>
    /// <returns>The enumerator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    /// <summary>
    /// Copy the elements from index to an array
    /// </summary>
    /// <param name="array">The output array</param>
    /// <param name="index">Starting index</param>
    public void CopyTo(Array array, int index)
    {
        lock (_syncLock)
        {
            try
            {
                _minHeap.Items().CopyTo(array, index);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }


    /// <summary>
    /// Copy the elements from index to an KeyValuePair array
    /// </summary>
    /// <param name="array">The output array</param>
    /// <param name="index">Starting index</param>
    public void CopyTo(KeyValuePair<TP, T>[] array, int index)
    {
        lock (_syncLock)
        {
            try
            {
                _minHeap.Items().CopyTo(array, index);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
    /// <summary>
    /// Dump the elements to an array
    /// </summary>
    /// <returns>The array</returns>
    public KeyValuePair<TP, T>[] ToArray()
    {
        lock (_syncLock)
        {
            var clonedHeap = new MinimumBinaryHeap<TP, T>(_minHeap);

            var result = new KeyValuePair<TP, T>[clonedHeap.Count()];

            for (var i = 0; i < result.Length; i++)
            {
                var poppedItem = clonedHeap.Remove();

                Debug.Assert(poppedItem != null, nameof(poppedItem) + " != null");

                result[i] = poppedItem.Value;
            }

            return result;
        }
    }

    /// <summary>
    /// Try adding a KeyValuePair into the priority heap
    /// </summary>
    /// <param name="item">The KeyValuePair to be inserted</param>
    /// <returns>Whether the operation was successful or not</returns>
    public bool TryAdd(KeyValuePair<TP, T> item)
    {
        bool res;

        lock (_syncLock)
        {
            res = _minHeap.Insert(item.Key, item.Value);
        }

        return res;
    }

    /// <summary>
    /// Attempt to remove an element from the queue
    /// </summary>
    /// <param name="item">The variable where the item wil be returned</param>
    /// <returns>Whether the operation was successful or not</returns>
    public bool TryTake(out KeyValuePair<TP, T> item)
    {
        item = default;
        lock (_syncLock)
        {
            if (_minHeap.Count() <= 0) return false;

            var poppedItem = _minHeap.Remove();

            Debug.Assert(poppedItem != null, nameof(poppedItem) + " != null");

            item = poppedItem.Value;
            return true;
        }
    }

    /// <summary>
    /// Clear the priority queue
    /// </summary>
    public void Clear()
    {
        lock (_syncLock)
        {
            _minHeap.Clear();
        }
    }

    /// <summary>
    /// Check whether the queue is empty or not
    /// </summary>
    /// <returns>Whether the queue is empty or not</returns>
    bool IPriorityQueue<TP, T>.IsEmpty()
    {
        return Count == 0;
    }

    /// <summary>
    /// Try getting the first item from the queue
    /// </summary>
    /// <param name="result">The variable to write out the result</param>
    /// <returns>Whether the operation was successful or not</returns>
    public bool TryPeek(out KeyValuePair<TP, T> result)
    {
        result = default;
        lock (_syncLock)
        {
            if (_minHeap.Count() <= 0) return false;

            var peekedItem = _minHeap.Peek();

            Debug.Assert(peekedItem != null, nameof(peekedItem) + " != null");

            result = peekedItem.Value;
            return true;
        }
    }
}

#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Example.ConcurrentPriorityQueue.Interfaces;

/// <summary>
/// Interface for a generic binary heap that associates a TP type with a T value
/// </summary>
/// <typeparam name="TP">The type of the key</typeparam>
/// <typeparam name="T">The type of the item to be inserted</typeparam>
public interface IMinimumBinaryHeap<TP, T>
    where TP : IComparable<TP>, IEquatable<TP>
{
    /// <summary>
    /// Clear the internal binary heap
    /// </summary>
    void Clear();
    /// <summary>
    /// Insert an element of tpe T with an associated key of type TP
    /// </summary>
    /// <param name="key">The key type</param>
    /// <param name="value">The value type</param>
    /// <returns>Whether adding was successful or not</returns>
    bool Insert(TP key, T value);
    /// <summary>
    /// Peek the first element in the binary heap
    /// </summary>
    /// <returns>Either null that indicates that the heap is empty, the KeyPairValue otherwise</returns>
    KeyValuePair<TP, T>? Peek();
    /// <summary>
    /// Return the number of elements in the heap
    /// </summary>
    /// <returns>The number of elements in the heap</returns>
    int Count();
    /// <summary>
    /// Remove and return the item with the highest priority from the binary heap
    /// </summary>
    /// <returns>Either null that indicates that the heap is empty, the KeyPairValue otherwise</returns>
    KeyValuePair<TP, T>? Remove();
    /// <summary>
    /// Return an ICollection of the items
    /// </summary>
    /// <returns>The collection</returns>
    ICollection Items();
}
#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

namespace Example.ConcurrentPriorityQueue.Interfaces;

/// <summary>
/// Interface for a generic binary heap that associates a TP type with a T value
/// </summary>
/// <typeparam name="TP">The type of the key</typeparam>
/// <typeparam name="T">The type of the item to be inserted</typeparam>
public interface IPriorityQueue<TP, T> : IProducerConsumerCollection<KeyValuePair<TP, T>>
    where TP : IComparable<TP>, IEquatable<TP>
{
    /// <summary>
    /// Try adding a KeyValuePair into the priority heap
    /// </summary>
    /// <param name="item">The KeyValuePair to be inserted</param>
    /// <returns>Whether the operation was successful or not</returns>
    new bool TryAdd(KeyValuePair<TP, T> item);
    /// <summary>
    /// Clear the priority queue
    /// </summary>
    void Clear();
    /// <summary>
    /// Check whether the queue is empty or not
    /// </summary>
    /// <returns>Whether the queue is empty or not</returns>
    bool IsEmpty();
    /// <summary>
    /// Try getting the first item from the queue
    /// </summary>
    /// <param name="result">The variable to write out the result</param>
    /// <returns>Whether the operation was successful or not</returns>
    bool TryPeek(out KeyValuePair<TP, T> result);
}
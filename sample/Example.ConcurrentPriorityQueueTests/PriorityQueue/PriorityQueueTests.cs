#region

using System;
using System.Collections.Generic;
using System.Linq;
using Example.ConcurrentPriorityQueue.Interfaces;
using NUnit.Framework;

#endregion

namespace Example.ConcurrentPriorityQueueTests.PriorityQueue;

[TestFixture]
public class PriorityQueueTests
{
    [SetUp]
    public void Initialise()
    {
        _queue = new ConcurrentPriorityQueue.PriorityQueue.PriorityQueue<DateTimeOffset, object>();
    }

    private IPriorityQueue<DateTimeOffset, object> _queue;

    [Test]
    public void Constructor_should_initialise_an_empty_heap()
    {
        Assert.AreEqual(_queue.Count, 0);
    }

    [Test]
    public void IsEmpty_should_return_true_if_no_elements_are_present()
    {
        _queue = new ConcurrentPriorityQueue.PriorityQueue.PriorityQueue<DateTimeOffset, object>();

        Assert.IsTrue(_queue.IsEmpty());
    }

    [Test]
    public void IsEmpty_should_return_false_if_elements_are_present()
    {
        _queue = new ConcurrentPriorityQueue.PriorityQueue.PriorityQueue<DateTimeOffset, object>();

        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        Assert.IsFalse(_queue.IsEmpty());
    }

    [Test]
    public void TryAdd_should_insert_elements_with_distinct_keys()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(1), new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(2), new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(3), new object()));

        Assert.AreEqual(_queue.Count, 4);
    }

    [Test]
    public void TryAdd_should_not_insert_elements_with_similar_keys()
    {
        var dup = DateTimeOffset.Now;

        Assert.AreEqual(_queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(dup, new object())), true);
        Assert.AreEqual(_queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(dup, new object())), false);

        Assert.AreEqual(_queue.Count, 1);
    }

    [Test]
    public void TryAdd_should_not_insert_elements_that_are_null_or_default()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, null));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, default));

        Assert.AreEqual(_queue.Count, 0);
    }

    [Test]
    public void TryTake_should_return_false_if_the_queue_is_empty()
    {
        Assert.IsFalse(_queue.TryTake(out _));
    }

    [Test]
    public void TryTake_should_return_default_key_value_pair_if_the_queue_is_empty()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, null));

        _queue.TryTake(out var result);

        Assert.AreEqual(result, new KeyValuePair<DateTimeOffset, object>(default, null));
    }

    [Test]
    public void TryTake_with_populated_list_should_be_okay()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());

        _queue.TryAdd(obj);
        _queue.TryTake(out var result);

        Assert.AreEqual(obj, result);
    }

    [Test]
    public void TryTake_should_take_the_oldest_input()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());
        _queue.TryAdd(obj);
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(1), new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(2), new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(3), new object()));
        _queue.TryTake(out var result);

        Assert.AreEqual(obj, result);
    }

    [Test]
    public void TryTake_with_populated_list_should_update_the_count()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());

        _queue.TryAdd(obj);
        _queue.TryTake(out _);

        Assert.IsEmpty(_queue);
    }

    [Test]
    public void TryPeek_should_return_false_if_the_queue_is_empty()
    {
        Assert.IsFalse(_queue.TryPeek(out _));
    }

    [Test]
    public void TryPeek_should_return_default_key_value_pair_if_the_queue_is_empty()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, null));

        _queue.TryPeek(out var result);

        Assert.AreEqual(result, new KeyValuePair<DateTimeOffset, object>(default, null));
    }

    [Test]
    public void TryPeek_with_populated_list_should_be_okay()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());

        _queue.TryAdd(obj);
        _queue.TryPeek(out var result);

        Assert.AreEqual(obj, result);
    }

    [Test]
    public void TryPeek_should_take_the_oldest_input()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());
        _queue.TryAdd(obj);
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(1), new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(2), new object()));
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now.AddMinutes(3), new object()));
        _queue.TryTake(out var result);

        Assert.AreEqual(obj, result);
    }

    [Test]
    public void TryPeek_with_populated_list_should_not_update_the_count()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());

        _queue.TryAdd(obj);
        _queue.TryPeek(out _);

        Assert.AreEqual(_queue.Count, 1);
    }

    [Test]
    public void Clear_should_clear_the_queue()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());

        _queue.TryAdd(obj);
        _queue.Clear();

        Assert.IsEmpty(_queue);
    }

    [Test]
    public void ToArray_should_return_the_queue_as_an_array()
    {
        var obj = new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object());

        _queue.TryAdd(obj);

        var result = _queue.ToArray();

        Assert.IsNotEmpty(result);
    }

    [Test]
    public void SyncRoot_should_not_be_null()
    {
        Assert.IsNotNull(_queue.SyncRoot);
    }

    [Test]
    public void IsSynchronized_should_be_true()
    {
        Assert.IsTrue(_queue.IsSynchronized);
    }

    [Test]
    public void CopyTo_should_not_copy_if_index_is_less_than_zero()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        var result = new KeyValuePair<DateTimeOffset, object>[1];

        Assert.DoesNotThrow(() => _queue.CopyTo(result, -1));
    }

    [Test]
    public void CopyTo_should_not_copy_if_the_array_is_null()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        _queue.CopyTo(null, -1);

        Assert.DoesNotThrow(() => _queue.CopyTo(null, 1));
    }

    [Test]
    public void CopyTo_should_copy_elements_from_one_array_to_another()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        var result = new KeyValuePair<DateTimeOffset, object>[1];

        Assert.DoesNotThrow(() => _queue.CopyTo(result, 0));

        Assert.IsNotEmpty(result);
    }

    [Test]
    public void GetEnumerator_should_return_a_valid_enumerator()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        var result = _queue.GetEnumerator();

        Assert.IsNotNull(result);
    }

    [Test]
    public void CopyToArray_should_copy_to_internal_array_type()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        var result = new object[1];

        Assert.DoesNotThrow(() => _queue.CopyTo(result, 0));

        Assert.IsNotEmpty(result);
    }

    [Test]
    public void CopyToArray_should_not_copy_to_internal_array_type_if_invalid_index_is_provided()
    {
        _queue.TryAdd(new KeyValuePair<DateTimeOffset, object>(DateTimeOffset.Now, new object()));

        var result = new object[1];

        Assert.DoesNotThrow(() => _queue.CopyTo(result, -1));

        Assert.IsNull(result.First());
    }
}
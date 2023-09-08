#region

using System;
using Example.ConcurrentPriorityQueue.Interfaces;
using Example.ConcurrentPriorityQueue.MinimumBinaryHeap;
using NUnit.Framework;

#endregion

namespace Example.ConcurrentPriorityQueueTests.MinimumBinaryHeap;

[TestFixture]
public class MinimumBinaryHeapTests
{
    [SetUp]
    public void Initialise()
    {
        _heap = new MinimumBinaryHeap<DateTimeOffset, object>();
    }

    private IMinimumBinaryHeap<DateTimeOffset, object> _heap;

    [Test]
    public void Constructor_should_initialize_an_empty_list()
    {
        Assert.AreEqual(_heap.Count(), 0);
    }

    [Test]
    public void Copy_constructor_should_copy_the_internal_elements()
    {
        _heap.Insert(DateTimeOffset.Now, new object());
        _heap.Insert(DateTimeOffset.Now.AddMinutes(1), new object());
        _heap.Insert(DateTimeOffset.Now.AddMinutes(2), new object());
        _heap.Insert(DateTimeOffset.Now.AddMinutes(3), new object());

        var copy = new MinimumBinaryHeap<DateTimeOffset, object>(_heap);

        Assert.AreEqual(_heap.Items(), copy.Items());
    }

    [Test]
    public void Clear_should_have_an_empty_container_and_zero_items_count()
    {
        _heap.Insert(DateTimeOffset.Now, new object());
        _heap.Insert(DateTimeOffset.Now.AddMinutes(1), new object());
        _heap.Insert(DateTimeOffset.Now.AddMinutes(2), new object());
        _heap.Insert(DateTimeOffset.Now.AddMinutes(3), new object());

        _heap.Clear();

        Assert.IsEmpty(_heap.Items());
        Assert.AreEqual(_heap.Count(), 0);
    }

    [Test]
    public void Clear_with_no_elements_should_do_nothing()
    {
        _heap.Clear();

        Assert.IsEmpty(_heap.Items());
        Assert.AreEqual(_heap.Count(), 0);
    }

    [Test]
    public void Insert_with_similar_keys_should_not_affect_the_heap()
    {
        var dup = DateTimeOffset.Now;

        Assert.AreEqual(_heap.Insert(dup, new object()), true);
        Assert.AreEqual(_heap.Insert(dup, new object()), false);

        Assert.AreEqual(_heap.Count(), 1);
    }

    [Test]
    public void Insert_null_or_default_object_should_not_modify_the_queue()
    {
        var dup = DateTimeOffset.Now;

        var r1 = _heap.Insert(dup, null);
        var r2 = _heap.Insert(dup, default);

        Assert.AreEqual(_heap.Count(), 0);
        Assert.AreEqual(r1, false);
        Assert.AreEqual(r2, false);
    }

    [Test]
    public void Insert_with_different_keys_should_be_okay()
    {
        var dup = DateTimeOffset.Now;

        _heap.Insert(dup, new object());
        _heap.Insert(dup.AddMinutes(1), new object());

        Assert.AreEqual(_heap.Count(), 2);
    }

    [Test]
    public void Insert_with_keys_in_no_order_should_be_okay()
    {
        var dup = DateTimeOffset.Now;

        _heap.Insert(dup.AddMinutes(1), new object());
        _heap.Insert(dup, new object());

        Assert.AreEqual(_heap.Count(), 2);
    }

    [Test]
    public void Peek_an_empty_heap_should_return_null_object()
    {
        var poppedItem = _heap.Peek();

        Assert.AreEqual(_heap.Count(), 0);
        Assert.AreEqual(poppedItem, null);
    }

    [Test]
    public void Peek_should_return_the_latest_object()
    {
        var now = DateTimeOffset.Now;
        _heap.Insert(now, new object());
        _heap.Insert(now.AddMinutes(1), new object());
        _heap.Insert(now.AddMinutes(2), new object());
        _heap.Insert(now.AddMinutes(3), new object());

        var poppedItem = _heap.Peek();

        if (poppedItem != null)
            Assert.AreEqual(poppedItem.Value.Key, now);
    }

    [Test]
    public void Remove_should_preserve_the_order()
    {
        var now = DateTimeOffset.Now;
        _heap.Insert(now, new object());
        _heap.Insert(now.AddMinutes(1), new object());
        _heap.Insert(now.AddMinutes(2), new object());
        _heap.Insert(now.AddMinutes(3), new object());
        _heap.Insert(now.AddMinutes(4), new object());
        _heap.Insert(now.AddMinutes(5), new object());
        _heap.Insert(now.AddMinutes(6), new object());

        var e1 = _heap.Remove();
        var e2 = _heap.Remove();
        var e3 = _heap.Remove();
        var e4 = _heap.Remove();


        if (e4 != null && e3 != null)
            Assert.Greater(e4.Value.Key, e3.Value.Key);
        if (e3 != null && e2 != null)
            Assert.Greater(e3.Value.Key, e2.Value.Key);
        if (e2 != null && e1 != null)
            Assert.Greater(e2.Value.Key, e1.Value.Key);
    }

    [Test]
    public void Remove_with_unordered_keys_should_preserve_the_order()
    {
        var now = DateTimeOffset.Now;

        _heap.Insert(now.AddMinutes(6), new object());
        _heap.Insert(now.AddMinutes(5), new object());
        _heap.Insert(now.AddMinutes(4), new object());
        _heap.Insert(now.AddMinutes(2), new object());
        _heap.Insert(now.AddMinutes(1), new object());
        _heap.Insert(now, new object());

        var e1 = _heap.Remove();
        var e2 = _heap.Remove();
        var e3 = _heap.Remove();
        var e4 = _heap.Remove();


        if (e4 != null && e3 != null)
            Assert.Greater(e4.Value.Key, e3.Value.Key);
        if (e3 != null && e2 != null)
            Assert.Greater(e3.Value.Key, e2.Value.Key);
        if (e2 != null && e1 != null)
            Assert.Greater(e2.Value.Key, e1.Value.Key);
    }

    [Test]
    public void Remove_in_an_empty_heap_should_return_null_object()
    {
        var poppedItem = _heap.Remove();

        Assert.AreEqual(poppedItem, null);
    }
}
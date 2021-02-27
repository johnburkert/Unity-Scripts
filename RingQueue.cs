using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RingQueue<T> : IEnumerable<T>
{
    public int Count { get; private set; }

    private readonly T[] _buffer;
    private readonly int _capacity;
    private int _index;

    public RingQueue(int capacity)
    {
        _buffer = new T[capacity];
        _capacity = capacity;
    }

    public RingQueue(IEnumerable<T> collection)
    {
        _buffer = collection.ToArray();
        _capacity = _buffer.Length;
        Count = _capacity;
    }

    public void Clear()
    {
        _index = 0;
        Count = 0;
    }

    public T Dequeue()
    {
        if (!(Count > 0))
            throw new InvalidOperationException();

        return _buffer[(_capacity - Count-- + _index) % _capacity];
    }
    
    public void Enqueue(T item)
    {
        _buffer[_index] = item;
        _index = (_index + 1) % _capacity;
        Count = Math.Min(Count + 1, _capacity);
    }

    public T Peek() => Peek(0);

    private T Peek(int i)
    {
        if (!(Count > 0))
            throw new InvalidOperationException();
        
        return _buffer[(_capacity - Count + _index + i) % _capacity];
    }

    public bool TryDequeue(out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = Dequeue();
        return true;

    }

    public bool TryPeek(out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = Peek();
        return true;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Math.Min(Count, _capacity); i++)
        {
            yield return Peek(i);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
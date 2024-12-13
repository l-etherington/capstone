using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    public List<(T item, double priority)> elements = new List<(T, double)>();

    public void Enqueue(T item, double priority)
    {
        elements.Add((item, priority));
        int childIndex = elements.Count - 1;

        // Bubble up the new element to maintain the heap property
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (elements[childIndex].priority >= elements[parentIndex].priority)
                break;

            // Swap
            (elements[childIndex], elements[parentIndex]) = (elements[parentIndex], elements[childIndex]);
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        // Get the item with the highest priority (which is at the root of the heap)
        T result = elements[0].item;
        int lastIndex = elements.Count - 1;
        elements[0] = elements[lastIndex];
        elements.RemoveAt(lastIndex);

        // Bubble down the new root to maintain the heap property
        int parentIndex = 0;

        while (true)
        {
            int leftChildIndex = 2 * parentIndex + 1;
            int rightChildIndex = 2 * parentIndex + 2;
            int swapIndex = parentIndex;

            if (leftChildIndex < elements.Count && elements[leftChildIndex].priority < elements[swapIndex].priority)
            {
                swapIndex = leftChildIndex;
            }

            if (rightChildIndex < elements.Count && elements[rightChildIndex].priority < elements[swapIndex].priority)
            {
                swapIndex = rightChildIndex;
            }

            if (swapIndex == parentIndex)
                break;

            // Swap
            (elements[parentIndex], elements[swapIndex]) = (elements[swapIndex], elements[parentIndex]);
            parentIndex = swapIndex;
        }

        return result;
    }

    public int Count => elements.Count;

    public bool IsEmpty => Count == 0;

    public T Peek()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");
        
        return elements[0].item;
    }
}

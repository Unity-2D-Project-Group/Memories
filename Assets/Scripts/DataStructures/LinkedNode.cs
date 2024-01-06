using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LinkedNode<T>
{
    public LinkedNode<T> next;
    public T data;
    public LinkedNode(T value)
    {
        next = null;
        data = value;
    }
    public T Data
    {
        get { return data; }
        set { data = value; }
    }
    public LinkedNode<T> Next
    {
        get { return next; }
        set { next = value; }
    }
}

[System.Serializable]
public class LinkedList<T>
{
    public LinkedNode<T> head;
    public int count = 0;
    public LinkedList()
    {
        head = null;
    }
    public int Length()
    {
        return count;
    }
    public void InsertAtBegin(T data)
    {
        LinkedNode<T> new_node = new LinkedNode<T>(data);
        new_node.Next = head;
        head = new_node;
        count++;
    }
    public LinkedNode<T> takeAtBegin()
    {
        LinkedNode<T> temp_node = head;
        head = head.Next;
        temp_node.Next = null;
        return temp_node;
    }
    public LinkedNode<T> getNode(int index)
    {
        if (index == 0)
        {
            return head;
        }
        LinkedNode<T> current = head.Next;
        for (int i = 1; i <= index; i++)
        {
            LinkedNode<T> current_node = current;
            if (i == index)
            {
                return current_node;
            }
            else
            {
                current = current_node.Next;
            }
        }
        return null;
    }
    public void setNode(int index, LinkedNode<T> new_node)
    {
        if (index == 0)
        {
            head = new_node;
        }
        LinkedNode<T> current = head.Next;
        for (int i = 1; i <= index; i++)
        {
            LinkedNode<T> current_node = current;
            if (i == index)
            {
                current_node = new_node;
            }
            else
            {
                current_node = current_node.Next;
            }

        }
    }
}

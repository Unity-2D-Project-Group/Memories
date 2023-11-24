using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TreeNode<T> 
{
    public T _data { get; set; }
    public TreeNode<T> _left { get; set; }
    public TreeNode<T> _right { get; set; }

    public TreeNode(T data, TreeNode<T> left, TreeNode<T> right)
    {
        this._data = data;
        this._left = left;
        this._right = right;
    }
}

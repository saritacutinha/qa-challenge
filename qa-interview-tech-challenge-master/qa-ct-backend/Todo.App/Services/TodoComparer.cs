using System;
using System.Collections.Generic;
using Todo.App.Models;

namespace Todo.App.Services
{
    public class TodoComparer : IEqualityComparer<TodoItem>
    {

        public bool Equals(TodoItem x, TodoItem y)
        {
            return string.Compare(x.Text, y.Text, StringComparison.InvariantCultureIgnoreCase) == 0
                && x.Completed == y.Completed;
        }

        public int GetHashCode(TodoItem obj)
        {
            return obj.Text.ToLowerInvariant().GetHashCode() ^
                obj.Completed.GetHashCode();
        }
    }
}

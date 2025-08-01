﻿using System.Reflection;

namespace GitUI
{
    public static class ControlUtil
    {
        private static readonly MethodInfo SetStyleMethod = typeof(TabControl)
            .GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Enumerates all descendant controls.
        /// </summary>
        public static IEnumerable<Control> FindDescendants(this Control control)
        {
            Queue<Control> queue = new();

            foreach (Control child in control.Controls)
            {
                queue.Enqueue(child);
            }

            while (queue.Count != 0)
            {
                Control c = queue.Dequeue();

                yield return c;

                foreach (Control child in c.Controls)
                {
                    queue.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// Enumerates all descendant controls of type <typeparamref name="T"/> in breadth-first order.
        /// </summary>
        public static IEnumerable<T> FindDescendantsOfType<T>(this Control control)
        {
            return FindDescendants(control).OfType<T>();
        }

        /// <summary>
        /// Finds the first descendent of <paramref name="control"/> that has type
        /// <typeparamref name="T"/> and satisfies <paramref name="predicate"/>.
        /// </summary>
        public static T? FindDescendantOfType<T>(this Control control, Func<T, bool> predicate)
        {
            return FindDescendants(control).OfType<T>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Enumerates all ancestor controls.
        /// </summary>
        /// <remarks>
        /// The returned sequence does not include <paramref name="control"/>.
        /// </remarks>
        public static IEnumerable<Control> FindAncestors(this Control control)
        {
            Control parent = control.Parent;

            while (parent is not null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// Calls protected method <see cref="Control.SetStyle"/>.
        /// </summary>
        public static void SetStyle(this Control control, ControlStyles styles, bool value) =>
            SetStyleMethod.Invoke(control, new object[] { styles, value });
    }
}

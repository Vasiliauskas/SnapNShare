using System;

namespace SnapNShare
{
    public static class NullabilityExtensions
    {
        public static void IfNotNull<T>(this T member, Action<T> action) where T : class
        {
            if (member != null)
                action(member);
        }

        public static void IfNull<T>(this T member, Action<T> action) where T : class
        {
            if (member == null)
                action(member);
        }

        public static void IfNotNull<T>(this T member, Action action) where T : class
        {
            if (member != null)
                action();
        }

        public static void IfNull<T>(this T member, Action action) where T : class
        {
            if (member == null)
                action();
        }
    }
}

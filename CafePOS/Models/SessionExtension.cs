using System.Text.Json;

namespace CafePOS.Models
{
    public static class SessionExtension
    {
        public static void Set<T> (this ISession session, string key, T Value)
        {
            session.SetString(key, JsonSerializer.Serialize (Value));
        }

        public static T Get<T> (this ISession session, string key)
        {
            var json = session.GetString(key);
            if (String.IsNullOrEmpty(json))
            {
                return default (T);
            }
            else
            {
               return JsonSerializer.Deserialize<T> (json); ;
            }

        }
    }
}

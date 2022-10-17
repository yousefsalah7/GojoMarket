using System.Text.Json;

namespace GojoMarket.Utility
{
    public static class SessionExtention
    {
        //Setter
        public static void Set<T> (this ISession session,String key,T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T? Get<T> (this ISession session,String key )
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}

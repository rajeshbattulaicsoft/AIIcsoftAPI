namespace AIIcsoftAPI.utility
{
    using System.Reflection;

    public static class CommonUtils
    {
        public static string? GetEmptyPropertyName<T>(T model) where T : class
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = property.GetValue(model)?.ToString() ?? string.Empty;
                    if (string.IsNullOrEmpty(value))
                    {
                        return property.Name;
                    }
                }
            }
            return null;
        }
    }

}

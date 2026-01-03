namespace JustTouch_ApiServices.SupabaseService
{
    public static class SupabaseHelpers
    {
        public static string BuildSelect(params string[] includes)
        {
            if (includes.Length == 0)
            {
                return "*";
            }
            var includeList = new List<string>();
            var Closers = new List<string>();
            
            foreach (var include in includes)
            {
                includeList.Add($", {include}(*");
                Closers.Add(")");
            }

            var result = "*" + string.Join(string.Empty, includeList) + string.Join(string.Empty, Closers);
            return result;
        }
    }
}

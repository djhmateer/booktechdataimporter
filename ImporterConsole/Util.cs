namespace ImporterConsole
{
    public static class Util
    {
        public static string CapitaliseFirstLetter(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpper();
            return s.Remove(1).ToUpper() + s.Substring(1);
        }
    }
}
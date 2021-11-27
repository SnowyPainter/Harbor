using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Ship
{
    public static class UriExtention
    {
        public static Uri? GetUriIfAbsolute(string url)
        {
            Uri? uri = null;
            Uri.TryCreate(url, UriKind.Absolute, out uri);
            return uri;
        }
    }
}

using System;

namespace MazeApp.Model.Enums
{
    public class ThemeUriAttribute : Attribute
    {
        public string Uri { get; private set; }

        public ThemeUriAttribute(string uri)
        {
            Uri = uri;
        }
    }
}

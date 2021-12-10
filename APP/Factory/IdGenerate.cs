using System;

namespace APP.Factory
{
    public static class IdGenerate
       {
        public static string generateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

using System.Runtime.CompilerServices;

namespace CWTExample
{
    /*
     * See in the `Hooks` class for an usage example.
     */
    public static class CWT
    {
        /*
         * The ConditionalWeakTable is a powerful modding tool, it allows you to attach fields to an object dynamically.
         *  This allows you to use custom fields and variables in hooks.
         *  A CWT works by taking in an instance of a class as a key, which will then return an instance of a data class that holds your fields.
         *  It's very similar to Dictionaries, and you could use a Dictionary instead of a CWT.
         *  However what seperates a ConditionalWeakTable from a Dictionary is in the name, "WeakTable"
         *  Keys in ConditionalWeakTables are weakly referenced, meaning they can be garbage collected when not in use.
         *  Dictionary keys are strongly referenced, which means if they are not in use it can cause a memory leak.
         */
        public static readonly ConditionalWeakTable<PlayerGraphics, DataClass> exampleCWT = new();
        public static bool TryGetData(PlayerGraphics key, out DataClass data)
        {
            /*
             * This `TryGetData` is how you access your CWT in your code, it takes in your key and gives out an instance of your data class.
             *  The method returns a bool to check whether the key does return an instance of a data class, this is to prevent errors if the key does not work.
             */
            if (key != null)
            {
                data = exampleCWT.GetOrCreateValue(key);
            }
            else data = null;

            return data != null;
        }
        public class DataClass
        {
            /*
             * This is your DataClass, it is not static because it is being instanced by your CWT when it gets a key.
             *  Store your fields here.
             */
            public int sLeaserLength;
            public int NewSpriteIndex;
        }
    }
}

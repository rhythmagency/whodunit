namespace Whodunit.app.ExtensionMethods
{

    //  Namespaces.
    using System.Collections.Generic;


    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class CollectionExtensionMethods
    {

        #region Methods

        /// <summary>
        /// Combines a collection of strings into a single string.
        /// </summary>
        /// <param name="strings">
        /// The strings to combine.
        /// </param>
        /// <param name="separator">
        /// The separator to place between each string.
        /// </param>
        /// <returns>
        /// The combined strings.
        /// </returns>
        public static string CombineStrings(this IEnumerable<string> strings, string separator)
        {
            if (strings == null)
            {
                return null;
            }
            else
            {
                return string.Join(separator, strings);
            }
        }

        #endregion

    }

}
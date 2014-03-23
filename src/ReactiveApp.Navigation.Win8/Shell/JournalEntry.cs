using System;

namespace ReactiveApp.Navigation
{
    public class JournalEntry : IJournalEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JournalEntry"/> class.
        /// </summary>
        /// <param name="sourceViewType">Type of the source view.</param>
        /// <param name="parameter">The parameter.</param>
        public JournalEntry(Type sourceViewType, object parameter = null)
        {
            this.ViewType = sourceViewType;
            this.Parameter = parameter;
        }

        public object Parameter
        {
            get;
            private set;
        }

        public Type ViewType
        {
            get;
            private set;
        }

        public object State
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var je = obj as JournalEntry;
            if (je == null)
            {
                return false;
            }

            bool ret = this.ViewType.Equals(je.ViewType) &&
                ((this.Parameter == null && je.Parameter == null) || (this.Parameter.Equals(je.Parameter)));

            return ret;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 17;

            if (this.Parameter != null)
            {
                hash = hash * 23 + this.Parameter.GetHashCode();
            }
            else
            {
                hash = hash * 23;
            }

            hash = hash * 23 + this.ViewType.GetHashCode();

            return hash;
        }
    }
}

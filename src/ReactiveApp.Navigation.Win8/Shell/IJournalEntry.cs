using System;

namespace ReactiveApp.Navigation
{
    public interface IJournalEntry
    {
        object Parameter { get; }

        Type ViewType { get; }
    }
}

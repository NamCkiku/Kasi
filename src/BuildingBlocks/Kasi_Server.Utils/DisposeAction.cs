﻿using Kasi_Server.Utils.Helpers;

namespace Kasi_Server.Utils
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        public DisposeAction(Action action)
        {
            Check.NotNull(action, nameof(action));
            _action = action;
        }

        public void Dispose() => _action();
    }
}
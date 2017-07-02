﻿using System;


namespace FluentHelium.Module
{
    /// <summary>
    /// Back side of Lazy 
    /// Lazy can be use after first Value property access
    /// Usable can be use before Dispose call
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Usable<T> : IDisposable 
    {
        internal Usable(T resource, IDisposable usageTime) : this(resource, usageTime.Dispose) {}

        internal Usable(T resource, Action dispose)
        {
            _dispose = dispose;
            _value = resource;
        }

        public void Dispose()
        {
            (_dispose ?? throw new ObjectDisposedException("")).Invoke();
            _dispose = null;
            _value = default(T);
        }

        internal T Value
        {
            get
            {
                if (_dispose == null)
                    throw new ObjectDisposedException("");
                return _value;
            }
        }

        public override string ToString() => _dispose != null ? $"Usable{{{_value}}}" : $"Disposed<{typeof(T).Name}>";

        private Action _dispose;
        private T _value;
    }
}

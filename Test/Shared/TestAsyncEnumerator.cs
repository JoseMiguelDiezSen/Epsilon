﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Shared
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public T Current => enumerator.Current;

        public ValueTask DisposeAsync() { 
            return new ValueTask(Task.Run(() => enumerator.Dispose()));
        }

        public ValueTask<bool> MoveNextAsync() { 
            return new ValueTask<bool>(enumerator.MoveNext());
        } 
    }
}

﻿using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Linq.Expressions;

namespace Test.Shared
{
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider innerQueryProvider;

        public TestAsyncQueryProvider(IQueryProvider innerQueryProvider){
            this.innerQueryProvider = innerQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression) {
           return new TestAsyncEnumerable<TEntity>(expression);   
        }

        public IQueryable <TElement>CreateQuery<TElement>(Expression expression){
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression) { 
            return innerQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression){
            return innerQueryProvider.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = ((IQueryProvider)this).Execute(expression);

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }
    }
}

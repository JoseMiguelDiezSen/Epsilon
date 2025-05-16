using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Shared
{
    public class TestContextAbstract<C> where C : DbContext
    {
        protected Mock <ILogger> iloggerFake;
        protected Mock <ILogger<C>> iLoggerEpsilonFake;
        protected Mock<C> contextFake;

        public TestContextAbstract() { 
            iloggerFake = new Mock<ILogger>();
            iLoggerEpsilonFake = new Mock<ILogger<C>>();
            contextFake = GetDbContextMock(iLoggerEpsilonFake.Object);
        }

        protected Mock<C> GetDbContextMock(ILogger<C> logger) { 
            var optionsBuilder = new DbContextOptionsBuilder<C>();
            return new Mock<C>(optionsBuilder.Options, logger);
        }

        protected Mock<C> GetDbContextMock() {
            var optionsBuilder = new DbContextOptionsBuilder<C>();
            return new Mock<C>(optionsBuilder.Options);
        }

        protected DbSet<T> GetQueryableMockDbSet<T>(List<T> source) where T : class {
            var queryable = source.AsQueryable<T>();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => source.Add(s));
            return dbSet.Object;
        }

        protected Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> items) where T : class { 
        
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new TestAsyncEnumerator<T> (items.GetEnumerator()));
                        
            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(items.Provider));

            dbSetMock.As<IQueryable<T>>()
               .Setup(m => m.Expression)
               .Returns(items.Expression);

            dbSetMock.As<IQueryable<T>>()
               .Setup(m => m.ElementType)
               .Returns(items.ElementType);

            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

            return dbSetMock;
        }  
    }
}
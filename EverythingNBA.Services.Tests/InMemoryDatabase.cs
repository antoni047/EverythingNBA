namespace EverythingNBA.Services.Tests
{
    using System;

    using EverythingNBA.Data;
    using Microsoft.EntityFrameworkCore;

    public class InMemoryDatabase
    {
        public static EverythingNBADbContext Get()
        {
            var dbOptions = new DbContextOptionsBuilder<EverythingNBADbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EverythingNBADbContext(dbOptions);
        }
    }
}

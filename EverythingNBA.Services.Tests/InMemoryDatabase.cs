namespace EverythingNBA.Services.Tests
{
    using EverythingNBA.Data;
    using Microsoft.EntityFrameworkCore;

    using System;

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

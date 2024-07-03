using Bogus;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Data.Context;
using TechChallenge.Domain.Models;

namespace TechChallenge.Domain.IntegrationTest;
[CollectionDefinition(nameof(ContextCollection))]

public class ContextCollection : ICollectionFixture<ContextFixture>
{

}

public class ContextFixture
{
    public techchallengeDbContext _context { get; set; }

    public ContextFixture()
    {
        var options = new DbContextOptionsBuilder<techchallengeDbContext>()
          .UseSqlServer("Data Source=PC-DELL-01,49172\\SQLEXPRESS01;Initial Catalog=techchallengeDb;Integrated Security=False;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;User ID=desenv;Password=desenv;")
          .Options;

        _context = new techchallengeDbContext(options);
    }

    public void ResetDatabase()
    {
        _context.Database.ExecuteSqlRawAsync($"DELETE FROM [TechChallenge].[Contact]");
        _context.Database.ExecuteSqlRawAsync("DELETE FROM [TechChallenge].[State] s WHERE s.DDD > 99");
    }

    public void ContactCreator()
    {
        List<Contact> lst = new List<Contact>();
        lst.Add(new Contact { Id = new Guid("F3763F34-1A52-47BD-4FFE-08DC79FBC12B"), Name = "Test Creator", Email = "creator@mail.com", Phone = "31987654321", StateId = new Guid("868460E5-DA8C-49F1-8A9D-FBC8D1D499AE") });
        lst.Add(new Contact { Id = new Guid("542B8709-4A74-433C-16CC-08DC7AC6277E"), Name = "Test Update", Email = "update@mail.com", Phone = "14012365478", StateId = new Guid("FDA86EF7-307D-43AE-B2FE-1F2DB8238F92") });
        lst.Add(new Contact { Id = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6"), Name = "Test Delete", Email = "update@mail.com", Phone = "21012365478", StateId = new Guid("20ADA234-2122-4339-9B3A-A41A5C0FA7E2") });

        _context.Contacts.AddRangeAsync(lst);
        _context.SaveChanges();
    }

    public async Task FakerStateCreator()
    {
        var stateFaker = new Faker<State>()
            .CustomInstantiator(f => new State
            {
                Name = f.Random.RandomLocale(),
                DDD = f.Random.Int(100, 2000)
            });        

        var states = stateFaker.Generate(200);
        await _context.States.AddRangeAsync(states);

        _context.SaveChanges();
    }
}
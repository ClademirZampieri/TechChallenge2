using TechChallenge.Data.Context;
using TechChallenge.Data.Repository;
using TechChallenge.Domain.Models;
using TechChallenge.Domain.Services;
using Xunit.Abstractions;

namespace TechChallenge.Domain.IntegrationTest;
[Collection(nameof(ContextCollection))]

public class ContactEdit
{
    private readonly techchallengeDbContext _context;

    public ContactEdit(ITestOutputHelper output, ContextFixture fixture)
    {
        _context = fixture._context;
        output.WriteLine(_context.GetHashCode().ToString());
    }

    [Fact]
    public async void ShouldEditNewContact()
    {
        //arrange        
        var contact = new Contact { Id = new Guid("542B8709-4A74-433C-16CC-08DC7AC6277E"), Name = "Updated", Email = "updated@mail.com", Phone = "14000000000", StateId = new Guid("24F07EA5-9A36-46E2-AE6C-7F7D273BB7AA") };
        var contactRepository = new ContactRepository(_context);
        var service = new ContactService(contactRepository);

        //act
        await service.Update(contact);
        
        //assert
        var updatedContact = await service.GetById(contact.Id);        
        Assert.NotNull(updatedContact);
        Assert.Equal("Updated", updatedContact.Name);
        Assert.Equal("updated@mail.com", updatedContact.Email);
        Assert.Equal("14000000000", updatedContact.Phone);
    }
}
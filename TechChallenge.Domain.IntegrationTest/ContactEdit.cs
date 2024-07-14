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
        var contact = new Contact { Id = new Guid("542B8709-4A74-433C-16CC-08DC7AC6277E"), Name = "Updated", Email = "updated@mail.com", Phone = "14000000000"};
        
        
        var contactRepository = new ContactRepository(_context);
        var service = new ContactService(contactRepository);

        var id = new Guid("542B8709-4A74-433C-16CC-08DC7AC6277E");
        var contact2 = await service.GetById(id);
        contact2.Name = contact.Name;
        contact2.Email = contact.Email;
        contact2.Phone = contact.Phone;
        //act
        await service.Update(contact2);
        
        //assert
        var updatedContact = await service.GetById(contact.Id);        
        Assert.NotNull(updatedContact);
        Assert.Equal(contact.Name, updatedContact.Name);
        Assert.Equal(contact.Email, updatedContact.Email);
        Assert.Equal(contact.Phone, updatedContact.Phone);

        _context.ChangeTracker.Clear();
    }
}
using AR4Layers.Catalog.DataAccess.Data;
using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using AR4Layers.Catalog.Services.Services;
using AutoFixture.Xunit2;
using FluentAssertions;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Services.Tests.Integration.Services;

public class BrandApplicationServiceTests
{
    [Theory(DisplayName = "Проверка возможности получения элемента")]
    [AutoData]
    public void TestAllBrandItems(BrandEntry entry)
    {
        var storage = new BrandStorage();
        entry = new BrandEntry { Id = 0, Name = entry.Name };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new BrandApplicationService();

        var items = sut.AllItems()
            .Throw(f => new ApplicationException());

        items.Count().Should().Be(1);
        var first = items.First();
        first.ToString().Should().Be($"{entry.Name}");
    }

    [Theory(DisplayName = "Проверка возможности добавления нового элемента")]
    [AutoData]
    public void TestCreateNewBrandItem(string name)
    {
        var storage = new BrandStorage();
        DataRegistry.InitFake(storage);
        var sut = new BrandApplicationService();

        var result = sut.AddItem(name);

        result.Should().BeOfType<Result<int>>();
        storage.Load(e => !e.IsDeleted).Count().Should().Be(1);
        var first = storage.Load(e => !e.IsDeleted).First();
        first.Name.Should().Be(name);
    }

    [Theory(DisplayName = "Проверка возможности изменения названия элемента")]
    [AutoData]
    public void TestChangeNameOfBrandItem(BrandEntry entry)
    {
        var expected = "newName";
        var storage = new BrandStorage();
        entry = new BrandEntry { Id = 0, Name = entry.Name };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new BrandApplicationService();

        var result = sut.ChangeName(entry.Id, expected);

        result.Should().BeOfType<Result>();
        var first = storage.Load(e => !e.IsDeleted).First();
        first.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности удаления элемента")]
    [AutoData]
    public void TestDeleteBrandItem(BrandEntry entry)
    {
        var storage = new BrandStorage();
        entry = new BrandEntry { Id = 0, Name = entry.Name };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new BrandApplicationService();

        var result = sut.DeleteItem(entry.Id);

        result.Should().BeOfType<Result>();
        storage.Load(e => !e.IsDeleted).Should().BeEmpty();
    }
}
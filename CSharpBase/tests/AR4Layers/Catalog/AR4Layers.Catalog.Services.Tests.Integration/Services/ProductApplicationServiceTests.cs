using AR4Layers.Catalog.DataAccess.Data;
using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using AR4Layers.Catalog.Services.Services;
using AutoFixture.Xunit2;
using FluentAssertions;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Services.Tests.Integration.Services;

public class ProductApplicationServiceTests
{
    [Theory(DisplayName = "Проверка возможности получения элементов")]
    [AutoData]
    public void TestAllProductItems(ProductEntry entry)
    {
        var storage = new ProductStorage();
        entry = new ProductEntry { Id = 0, Name = entry.Name, Price = entry.Price, BrandId = entry.BrandId, CategoryId = entry.CategoryId };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new ProductApplicationService();

        var items = sut.AllItems()
            .Throw(f => new ApplicationException());

        items.Count().Should().Be(1);
        var first = items.First();
        first.ToString().Should().Be($"{entry.Name}");
        first.FullText().Should().Be($"{entry.Name} - {entry.Price} руб.");
    }

    [Theory(DisplayName = "Проверка возможности добавления нового элементов")]
    [AutoData]
    public void TestCreateNewProductItem(string name, decimal price, int brandId, int categoryId)
    {
        var storage = new ProductStorage();
        DataRegistry.InitFake(storage);
        var sut = new ProductApplicationService();

        sut.AddItem(name, price, brandId, categoryId);

        storage.Load(e => !e.IsDeleted).Count().Should().Be(1);
        var first = storage.Load(e => !e.IsDeleted).First();
        first.Name.Should().Be(name);
        first.Price.Should().Be(price);
        first.BrandId.Should().Be(brandId);
        first.CategoryId.Should().Be(categoryId);
    }

    [Theory(DisplayName = "Проверка возможности изменения названия элемента")]
    [AutoData]
    public void TestChangeNameOfProductItem(ProductEntry entry)
    {
        var expected = "newName";
        var storage = new ProductStorage();
        entry = new ProductEntry { Id = 0, Name = entry.Name, Price = entry.Price, BrandId = entry.BrandId, CategoryId = entry.CategoryId };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new ProductApplicationService();

        var result = sut.ChangeName(entry.Id, expected);

        result.Should().BeOfType<Result>();
        var first = storage.Load(e => !e.IsDeleted).First();
        first.Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности изменения цены элемента")]
    [AutoData]
    public void TestChangePriceOfProductItem(ProductEntry entry)
    {
        var expected = 333;
        var storage = new ProductStorage();
        entry = new ProductEntry { Id = 0, Name = entry.Name, Price = entry.Price, BrandId = entry.BrandId, CategoryId = entry.CategoryId };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new ProductApplicationService();

        var result = sut.ChangePrice(entry.Id, expected);

        result.Should().BeOfType<Result>();
        var first = storage.Load(e => !e.IsDeleted).First();
        first.Price.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности удаления элемента")]
    [AutoData]
    public void TestDeleteProductItem(ProductEntry entry)
    {
        var storage = new ProductStorage();
        entry = new ProductEntry { Id = 0, Name = entry.Name, Price = entry.Price, BrandId = entry.BrandId, CategoryId = entry.CategoryId };
        storage.Save(entry);
        DataRegistry.InitFake(storage);
        var sut = new ProductApplicationService();

        var result = sut.DeleteItem(entry.Id);

        result.Should().BeOfType<Result>();
        storage.Load(e => !e.IsDeleted).Should().BeEmpty();
    }
}
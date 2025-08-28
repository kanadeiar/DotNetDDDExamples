using AutoFixture.Xunit2;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Events;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Events;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Events;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;
using FluentAssertions;
using Kanadeiar.Common.Tests;

namespace DMHexagonal.Catalog.Core.Tests.Unit.ProductAggregate;

public class ProductItemTests
{
    [Theory(DisplayName = "Проверка создания нового товара")]
    [InlineAutoMoqData(1, "Товар")]
    public void TestCreate(int id, string name, decimal price)
    {
        var actual = ProductItem.Create(new ProductId(id), new ProductNameValue(name), new PriceValue(price), new BrandId(1), new CategoryId(1));

        var entry = actual.Entry();
        entry.Id.Should().Be(id);
        entry.Name.Should().Be(name);
        entry.Price.Should().Be(price);
        var events = actual.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductCreated).Id.Id.Should().Be(id);
        (events.Last() as ProductCreated).Name.Name.Should().Be(name);
        (events.Last() as ProductCreated).Price.Price.Should().Be(price);
    }

    [Theory(DisplayName = "Проверка нарушения инвариантов товара")]
    [InlineAutoMoqData(0, "Тест", 300, 1, 1)]
    [InlineAutoMoqData(1, "Т", 300, 1, 1)]
    [InlineAutoMoqData(1, "Тест", -1, 1, 1)]
    [InlineAutoMoqData(1, "Тест", 300, -1, 1)]
    [InlineAutoMoqData(1, "Тест", 300, 1, -1)]
    public void TestCreate_WhenInvariantError(int id, string name, decimal price, int brandId, int categoryId)
    {
        var act = () =>
        {
            _ = ProductItem.Create(new ProductId(id), new ProductNameValue(name), new PriceValue(price), new BrandId(brandId), new CategoryId(categoryId));
        };

        act.Should().Throw<ApplicationException>();
    }

    [Theory(DisplayName = "Проверка возможности изменения названия элемента")]
    [AutoMoqData]
    public void TestRename(ProductEntry entry)
    {
        var expected = new ProductNameValue("Новое имя");
        var sut = ProductItem.Restore(entry);

        sut.Rename(expected);

        var actualEntry = sut.Entry();
        actualEntry.Name.Should().Be(expected.Name);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductRenamed).Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности изменения цены элемента")]
    [AutoData]
    public void TestChangePrice(ProductEntry entry)
    {
        var expected = new PriceValue(333);
        var sut = ProductItem.Restore(entry);

        sut.ChangePrice(expected);

        var actualEntry = sut.Entry();
        actualEntry.Price.Should().Be(expected.Price);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductPriceChanged).Price.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности удаления элемента")]
    [AutoData]
    public void TestDeleteItem(ProductEntry entry)
    {
        var expected = 1;
        entry = new ProductEntry { Id = expected, Name = entry.Name, Price = entry.Price, BrandId = entry.BrandId, CategoryId = entry.CategoryId, IsDeleted = false };
        var sut = ProductItem.Restore(entry);

        sut.Delete();

        var actualEntry = sut.Entry();
        actualEntry.IsDeleted.Should().Be(true);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductDeleted).Id.Id.Should().Be(expected);
    }
}
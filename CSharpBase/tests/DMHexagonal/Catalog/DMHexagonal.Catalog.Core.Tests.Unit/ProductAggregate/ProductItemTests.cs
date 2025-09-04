using AutoFixture.Xunit2;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
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
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Товар", 1, "5C60F693-BEF5-E011-A485-80EE7300C695")]
    public void TestCreate(Guid id, string name, decimal price, Guid otherId)
    {
        var actual = ProductItem.Create(new ProductId(id), new ProductNameValue(name), new PriceValue(price), new BrandId(otherId), new CategoryId(otherId));

        var entry = actual.Entry();
        entry.Id.Should().Be(id);
        entry.Name.Should().Be(name);
        entry.Price.Should().Be(price);
        var events = actual.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductCreated).Id.Value.Should().Be(id);
        (events.Last() as ProductCreated).Name.Value.Should().Be(name);
        (events.Last() as ProductCreated).Price.Value.Should().Be(price);
    }

    [Theory(DisplayName = "Проверка нарушения инвариантов товара")]
    [InlineAutoMoqData("00000000-0000-0000-0000-000000000000", "Тест", 300, "5C60F693-BEF5-E011-A485-80EE7300C695", "5C60F693-BEF5-E011-A485-80EE7300C695")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Т", 300, "5C60F693-BEF5-E011-A485-80EE7300C695", "5C60F693-BEF5-E011-A485-80EE7300C695")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Тест", -1, "5C60F693-BEF5-E011-A485-80EE7300C695", "5C60F693-BEF5-E011-A485-80EE7300C695")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Тест", 300, "00000000-0000-0000-0000-000000000000", "5C60F693-BEF5-E011-A485-80EE7300C695")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Тест", 300, "5C60F693-BEF5-E011-A485-80EE7300C695", "00000000-0000-0000-0000-000000000000")]
    public void TestCreate_WhenInvariantError(Guid id, string name, decimal price, Guid brandId, Guid categoryId)
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
        actualEntry.Name.Should().Be(expected.Value);
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
        actualEntry.Price.Should().Be(expected.Value);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductPriceChanged).Price.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности удаления элемента")]
    [AutoData]
    public void TestDeleteItem(ProductEntry entry)
    {
        var expected = Guid.Parse("5C60F693-BEF5-E011-A485-80EE7300C695");
        entry = new ProductEntry { Id = expected, Name = entry.Name, Price = entry.Price, BrandId = entry.BrandId, CategoryId = entry.CategoryId, IsDeleted = false };
        var sut = ProductItem.Restore(entry);

        sut.Delete();

        var actualEntry = sut.Entry();
        actualEntry.IsDeleted.Should().Be(true);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as ProductDeleted).Id.Value.Should().Be(expected);
    }
}
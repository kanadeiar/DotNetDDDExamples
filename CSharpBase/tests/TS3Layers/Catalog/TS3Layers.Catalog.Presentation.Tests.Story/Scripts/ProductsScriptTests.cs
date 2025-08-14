using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using TS3Layers.Catalog.DataAccess.Data;
using TS3Layers.Catalog.DataAccess.Entries;
using TS3Layers.Catalog.Presentation.Scripts;

namespace TS3Layers.Catalog.Presentation.Tests.Story.Scripts;

public class ProductsScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех товаров, " +
                          "чтобы просмотреть все доступные товары.")]
    [AutoMoqData]
    public void StoryTestAllProducts(ProductStorage storage)
    {
        var expected = "Товар";
        storage.Save(new ProductEntry { Id = 1, Name = expected, Price = 11, BrandId = 1, CategoryId = 1 });
        var sut = new ProductScript(storage);

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }

    [Theory(DisplayName = "История: Как пользователь, " +
                          "я могу отфильтровать товары по названию, бренду и категории," +
                          "для того, чтобы быстро найти нужный мне товар.")]
    [AutoMoqData]
    public void StoryTestFilter_WhenAnd(ProductStorage storage)
    {
        var expected = new ProductEntry { Id = 2, Name = "Суперимя", Price = 311, BrandId = 4, CategoryId = 8 };
        storage.Save(new ProductEntry { Id = 1, Name = "Суперимя", Price = 131, BrandId = 4, CategoryId = 1 });
        storage.Save(expected);
        storage.Save(new ProductEntry { Id = 3, Name = "Суперимя", Price = 11, BrandId = 1, CategoryId = 8 });
        storage.Save(new ProductEntry { Id = 3, Name = "Имя", Price = 11, BrandId = 4, CategoryId = 8 });
        var sut = new ProductScript(storage);

        var items = sut.Filter(expected.Name, expected.BrandId, expected.CategoryId)
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().Entry().Id.Should().Be(expected.Id);
    }

    [Theory(DisplayName = "История: Как пользователь, " +
                          "я могу как угодно отфильтровать товары как по названию, по бренду, так и по категории," +
                          "для того, чтобы быстро найти нужный мне товар.")]
    [AutoMoqData]
    public void StoryTestFilter_WhenOr(ProductStorage storage)
    {
        var expected = new ProductEntry { Id = 2, Name = "Суперимя", Price = 311, BrandId = 4, CategoryId = 8 };
        storage.Save(new ProductEntry { Id = 1, Name = "Суперимя", Price = 131, BrandId = 4, CategoryId = 1 });
        storage.Save(expected);
        storage.Save(new ProductEntry { Id = 3, Name = "Суперимя", Price = 11, BrandId = 1, CategoryId = 8 });
        storage.Save(new ProductEntry { Id = 4, Name = "Имя", Price = 11, BrandId = 4, CategoryId = 8 });
        var sut = new ProductScript(storage);

        var items = sut.Filter(expected.Name)
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Entry().Id.Should().Be(1);

        items = sut.Filter(brandId: expected.BrandId)
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Entry().Id.Should().Be(1);

        items = sut.Filter(categoryId: expected.CategoryId)
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Entry().Id.Should().Be(expected.Id);
    }

    [Theory(DisplayName = "История: Как администратор, " +
                          "я могу отредактировать любой товар в каталоге товаров, " +
                          "чтобы поддерживать актуальное состояние каталога.")]
    [AutoMoqData]
    public void StoryTestEdit(ProductStorage storage)
    {
        storage.Save(new ProductEntry { Id = 1, Name = "Демо", Price = 11, BrandId = 1, CategoryId = 1 });
        var expectedName = "Новое название";
        var expectedPrice = 3000;
        var sut = new ProductScript(storage);

        var result = sut.ChangeName(1, expectedName);
        var resultTwo = sut.ChangePrice(1, expectedPrice);

        result.Should().BeOfType<Result>();
        resultTwo.Should().BeOfType<Result>();
        var actuals = storage.All();
        actuals.Count().Should().Be(1);
        actuals.First().Name.Should().Be(expectedName);
        actuals.First().Price.Should().Be(expectedPrice);
    }

    [Theory(DisplayName = "История: Как администратор, " +
                          "я могу удалить любой товар из каталоге, " +
                          "чтобы удалить уже проданные позиции.")]
    [AutoMoqData]
    public void StoryTestDelete(ProductStorage storage)
    {
        var expectedName = "Обычный товар";
        storage.Save(new ProductEntry { Id = 1, Name = "Демо", Price = 11, BrandId = 1, CategoryId = 1 });
        storage.Save(new ProductEntry { Id = 2, Name = expectedName, Price = 11, BrandId = 1, CategoryId = 1 });
        var sut = new ProductScript(storage);

        var result = sut.DeleteItem(1);

        result.Should().BeOfType<Result>();
        var actuals = storage.All();
        actuals.Count().Should().Be(1);
        actuals.First().Name.Should().Be(expectedName);
    }
}
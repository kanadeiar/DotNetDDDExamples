using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;
using DMHexagonal.Catalog.Presentation.Scripts;
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using Moq;

namespace DMHexagonal.Catalog.Presentation.Tests.Story.Scripts;

public class ProductScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех товаров, " +
                          "чтобы выбрать какие-либо из них для покупки.")]
    [AutoMoqData]
    public void StoryTestAllProducts(Mock<IProductStorage> mock)
    {
        var expected = "Товар";
        var item = new ProductItem(new ProductId(Guid.NewGuid()), new ProductNameValue(expected), new PriceValue(100M), new BrandId(Guid.NewGuid()), new CategoryId(Guid.NewGuid()));
        mock.Setup(x => x.Load(It.IsAny<Predicate<ProductEntry>>())).Returns([item]);
        var service = new ProductApplicationService(mock.Object);
        var sut = new ProductScript(service);

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
    public void StoryTestFilter(Mock<IProductStorage> mock)
    {
        var expected = new ProductEntry { Id = Guid.NewGuid(), Name = "Суперимя", Price = 311, BrandId = Guid.NewGuid(), CategoryId = Guid.NewGuid() };
        mock.Setup(x => x.Load(It.IsAny<Predicate<ProductEntry>>())).Returns(
            [
                new ProductItem(ProductId.New(), new ProductNameValue("Суперимя"), new PriceValue(131), new BrandId(expected.BrandId), new CategoryId(Guid.NewGuid())),
                ProductItem.Restore(expected),
                new ProductItem(ProductId.New(), new ProductNameValue("Суперимя"), new PriceValue(11), new BrandId(Guid.NewGuid()), new CategoryId(expected.CategoryId)),
                new ProductItem(ProductId.New(), new ProductNameValue("Имя"), new PriceValue(11), new BrandId(expected.BrandId), new CategoryId(expected.CategoryId)),
            ]);
        var service = new ProductApplicationService(mock.Object);
        var sut = new ProductScript(service);

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
    public void StoryTestFilter_WhenOr(Mock<IProductStorage> mock)
    {
        var id = ProductId.New();
        var expected = new ProductEntry { Id = Guid.NewGuid(), Name = "Суперимя", Price = 311, BrandId = Guid.NewGuid(), CategoryId = Guid.NewGuid() };
        mock.Setup(x => x.Load(It.IsAny<Predicate<ProductEntry>>())).Returns(
        [
            new ProductItem(id, new ProductNameValue("Суперимя"), new PriceValue(131), new BrandId(expected.BrandId), new CategoryId(Guid.NewGuid())),
            ProductItem.Restore(expected),
            new ProductItem(ProductId.New(), new ProductNameValue("Суперимя"), new PriceValue(11), new BrandId(Guid.NewGuid()), new CategoryId(expected.CategoryId)),
            new ProductItem(ProductId.New(), new ProductNameValue("Имя"), new PriceValue(11), new BrandId(expected.BrandId), new CategoryId(expected.CategoryId)),
        ]);
        var service = new ProductApplicationService(mock.Object);
        var sut = new ProductScript(service);

        var items = sut.Filter(expected.Name)
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Entry().Id.Should().Be(id.Value);

        items = sut.Filter(brandId: expected.BrandId)
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Entry().Id.Should().Be(id.Value);

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
    public void StoryTestEdit(Mock<IProductStorage> mock)
    {
        var expectedName = "Новое название";
        var expectedPrice = 3000;
        var id = ProductId.New();
        var expected = new ProductItem(id, new ProductNameValue("one"), new PriceValue(11M), BrandId.New(), CategoryId.New());
        mock.Setup(x => x.Load(id))
            .Returns(expected);
        var service = new ProductApplicationService(mock.Object);
        var sut = new ProductScript(service);

        var result = sut.ChangeName(id.Value, expectedName);

        result.Should().BeOfType<Result>();
        mock.Verify(x => x.Save(expected), Times.Once);
        expected.Entry().Name.Should().Be(expectedName);

        var resultTwo = sut.ChangePrice(id.Value, expectedPrice);

        resultTwo.Should().BeOfType<Result>();
        mock.Verify(x => x.Save(expected), Times.Exactly(2));
        expected.Entry().Price.Should().Be(expectedPrice);
    }

    [Theory(DisplayName = "История: Как администратор, " +
                          "я могу удалить любой товар из каталоге, " +
                          "чтобы удалить уже проданные позиции.")]
    [AutoMoqData]
    public void StoryTestDelete(Mock<IProductStorage> mock)
    {
        var id = ProductId.New();
        var expected = new ProductItem(id, new ProductNameValue("one"), new PriceValue(11M), BrandId.New(), CategoryId.New());
        mock.Setup(x => x.Load(id))
            .Returns(expected);
        var service = new ProductApplicationService(mock.Object);
        var sut = new ProductScript(service);

        var result = sut.DeleteItem(id.Value);

        result.Should().BeOfType<Result>();
        mock.Verify(x => x.Save(expected), Times.Once);
        expected.Entry().IsDeleted.Should().Be(true);
    }
}
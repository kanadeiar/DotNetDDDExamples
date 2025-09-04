using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate;
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

public class CommonScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть каталог всех товаров, " +
                          "чтобы выбрать какой-либо из них для покупки.")]
    [AutoMoqData]
    public void StoryTestAllProducts(Mock<IProductStorage> mock, Mock<IBrandStorage> brandsMock, Mock<ICategoryStorage> categoriesMock)
    {
        var expected = "Шапки Adidas Шапка-ушанка - 100 руб.";
        var brand = new BrandItem(new BrandId(1), new BrandNameValue("Adidas"));
        brandsMock.Setup(x => x.Load(It.IsAny<Predicate<BrandEntry>>()))
            .Returns([brand]);
        var category = new CategoryItem(new CategoryId(1), new CategoryNameValue("Шапки"));
        categoriesMock.Setup(x => x.Load(It.IsAny<Predicate<CategoryEntry>>()))
            .Returns([category]);
        var item = new ProductItem(new ProductId(1), new ProductNameValue("Шапка-ушанка"), new PriceValue(100M), new BrandId(1), new CategoryId(1));
        mock.Setup(x => x.Load(It.IsAny<Predicate<ProductEntry>>()))
            .Returns([item]);
        var service = new CommonApplicationService(mock.Object, brandsMock.Object, categoriesMock.Object);
        var sut = new CommonScript(service);

        var items = sut.AllProducts()
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().Should().Be(expected);
    }
}
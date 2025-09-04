using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using TSEasy.Catalog.DataAccess;
using TSEasy.Catalog.Presentation;

namespace TSEasy.Catalog.Tests.Story.Presentation;

public class CommonScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть каталог всех товаров, " +
                          "чтобы выбрать какой-либо из них для покупки.")]
    [AutoMoqData]
    public void StoryTestAllProducts(ProductStorage product, BrandStorage brand, CategoryStorage category)
    {
        var expected = "Шапки Adidas - Шапка-ушанка - 100 руб.";
        var sut = new CommonScript(product, brand, category);
        sut.InitDemo().Should().BeOfType<Result>();

        var items = sut.AllProducts()
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Should().Be(expected);
    }
}
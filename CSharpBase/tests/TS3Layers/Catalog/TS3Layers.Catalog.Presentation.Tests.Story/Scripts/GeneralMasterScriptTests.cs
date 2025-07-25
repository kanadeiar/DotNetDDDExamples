using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using TS3Layers.Catalog.DataAccess.Data;
using TS3Layers.Catalog.Presentation.Scripts;

namespace TS3Layers.Catalog.Presentation.Tests.Story.Scripts;

public class GeneralMasterScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, я хочу просмотреть каталог всех товаров, " +
                          "чтобы выбрать какой-либо из них.")]
    [AutoMoqData]
    public void StoryTestAllProducts(ProductStorage product, BrandStorage brand, CategoryStorage category)
    {
        var expected = "Шапки Adidas - Шапка-ушанка - 100 руб.";
        var sut = new GeneralMasterScript(product, brand, category);
        sut.InitDemo().Should().BeOfType<Result>();

        var items = sut.AllProducts()
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Should().Be(expected);
    }
}
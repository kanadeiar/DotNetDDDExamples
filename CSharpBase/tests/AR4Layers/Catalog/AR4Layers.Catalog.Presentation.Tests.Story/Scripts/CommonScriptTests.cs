using AR4Layers.Catalog.DataAccess.Data;
using AR4Layers.Catalog.DataAccess.Registries;
using AR4Layers.Catalog.Presentation.Scripts;
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;

namespace AR4Layers.Catalog.Presentation.Tests.Story.Scripts;

[Collection(nameof(SystemTestCollectionDefinition))]
public class CommonScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть каталог всех товаров, " +
                          "чтобы выбрать какой-либо из них для покупки.")]
    [AutoMoqData]
    public void StoryTestAllProducts(ProductStorage product, BrandStorage brand, CategoryStorage category)
    {
        var expected = "Шапки Adidas - Шапка-ушанка - 100 руб.";
        var sut = new CommonScript();
        DataRegistry.InitFake(product);
        DataRegistry.InitFake(brand);
        DataRegistry.InitFake(category);
        sut.InitDemo().Should().BeOfType<Result>();

        var items = sut.AllProducts()
            .Throw(_ => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(3);
        items.First().Should().Be(expected);
    }
}

[CollectionDefinition(nameof(SystemTestCollectionDefinition), DisableParallelization = true)]
public class SystemTestCollectionDefinition { }
using AR4Layers.Catalog.DataAccess.Data;
using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using AR4Layers.Catalog.Presentation.Scripts;
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;

namespace AR4Layers.Catalog.Presentation.Tests.Story.Scripts;

public class BrandScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех брендов товаров, " +
                          "чтобы отфильтровать список товаров по бренду.")]
    [AutoMoqData]
    public void StoryTestAllItems(BrandStorage storage)
    {
        var expected = "Бренд";
        storage.Save(new BrandEntry { Id = 1, Name = expected });
        DataRegistry.InitFake(storage);
        var sut = new BrandScript();

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }
}
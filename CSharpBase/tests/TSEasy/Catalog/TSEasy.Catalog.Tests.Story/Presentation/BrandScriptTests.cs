using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using TSEasy.Catalog.DataAccess;
using TSEasy.Catalog.DataAccess.Entries;
using TSEasy.Catalog.Presentation;

namespace TSEasy.Catalog.Tests.Story.Presentation;

public class BrandScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех брендов товаров, " +
                          "чтобы отфильтровать список товары по бренду.")]
    [AutoMoqData]
    public void StoryTestAllBrands(BrandStorage storage)
    {
        var expected = "Бренд";
        storage.Save(new BrandEntry { Id = 1, Name = expected });
        var sut = new BrandScript(storage);

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }
}
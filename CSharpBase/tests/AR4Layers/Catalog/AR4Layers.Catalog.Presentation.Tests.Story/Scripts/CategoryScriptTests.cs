using AR4Layers.Catalog.DataAccess.Data;
using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using AR4Layers.Catalog.Presentation.Scripts;
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;

namespace AR4Layers.Catalog.Presentation.Tests.Story.Scripts;

public class CategoryScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех категорий товаров, " +
                          "чтобы отфильтровать список товаров по категории.")]
    [AutoMoqData]
    public void StoryTestsAllItems(CategoryStorage storage)
    {
        var expected = "Категория";
        storage.Save(new CategoryEntry { Id = 1, Name = expected });
        DataRegistry.InitFake(storage);
        var sut = new CategoryScript();

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }
}
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using TSEasy.Catalog.DataAccess;
using TSEasy.Catalog.DataAccess.Entries;
using TSEasy.Catalog.Presentation;

namespace TSEasy.Catalog.Tests.Story.Presentation;

public class CategoryScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех категорий товаров, " +
                          "чтобы отфильтровать список товары по категории.")]
    [AutoMoqData]
    public void StoryTestAllCategories(CategoryStorage storage)
    {
        var expected = "Категория";
        storage.Save(new CategoryEntry { Id = 1, Name = expected });
        var sut = new CategoryScript(storage);

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }
}
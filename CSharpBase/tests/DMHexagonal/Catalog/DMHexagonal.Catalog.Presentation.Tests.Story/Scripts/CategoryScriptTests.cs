using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Presentation.Scripts;
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using Moq;

namespace DMHexagonal.Catalog.Presentation.Tests.Story.Scripts;

public class CategoryScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех категорий товаров, " +
                          "чтобы отфильтровать список товаров по категории.")]
    [AutoMoqData]
    public void StoryTestAllItems(Mock<ICategoryStorage> mock)
    {
        var expected = "Категория";
        var item = new CategoryItem(new CategoryId(Guid.NewGuid()), new CategoryNameValue(expected));
        mock.Setup(x => x.Load(It.IsAny<Predicate<CategoryEntry>>())).Returns([item]);
        var service = new CategoryApplicationService(mock.Object);
        var sut = new CategoryScript(service);

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }
}
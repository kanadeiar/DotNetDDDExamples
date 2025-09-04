using DMHexagonal.Catalog.Application.CatalogFeature;
using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Presentation.Scripts;
using FluentAssertions;
using Kanadeiar.Common.Functionals;
using Kanadeiar.Common.Tests;
using Moq;

namespace DMHexagonal.Catalog.Presentation.Tests.Story.Scripts;

public class BrandScriptTests
{
    [Theory(DisplayName = "История: Как пользователь, " +
                          "я хочу просмотреть список всех брендов товаров, " +
                          "чтобы отфильтровать список товаров по бренду.")]
    [AutoMoqData]
    public void StoryTestAllItems(Mock<IBrandStorage> mock)
    {
        var expected = "Бренд";
        var item = new BrandItem(new BrandId(Guid.NewGuid()), new BrandNameValue(expected));
        mock.Setup(x => x.Load(It.IsAny<Predicate<BrandEntry>>())).Returns([item]);
        var service = new BrandApplicationService(mock.Object);
        var sut = new BrandScript(service);

        var items = sut.AllItems()
            .Throw(() => throw new ApplicationException())
            .ToArray();

        items.Length.Should().Be(1);
        items.First().ToString().Should().Be(expected);
    }
}
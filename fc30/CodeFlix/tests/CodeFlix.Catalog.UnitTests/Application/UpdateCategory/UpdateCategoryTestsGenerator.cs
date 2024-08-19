using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

public class UpdateCategoryTestsGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int quantity) {
        var fixture = new UpdateCategoryTestsFixture();
        for (int i = 0; i < quantity; i++ ) {
            var category = fixture.GetValidCategory();
            var input = fixture.GetValidInput(category.Id);
            yield return new object[] { category, input };
        }
    }
}

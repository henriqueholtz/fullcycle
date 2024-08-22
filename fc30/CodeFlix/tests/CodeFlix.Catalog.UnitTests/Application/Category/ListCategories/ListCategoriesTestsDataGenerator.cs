using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

namespace CodeFlix.Catalog.UnitTests.Application.Category.ListCategories;

public class ListCategoriesTestsDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameters(int times)
    {
        var fixture = new ListCategoriesTestsFixture();
        var inputBase = fixture.GetValidInput();
        for (int i = 0; i < times; i++)
        {
            switch (i % 6)
            {
                case 0:
                    yield return new object[] { new ListCategoriesInput() };
                    break;
                case 1:
                    yield return new object[] { new ListCategoriesInput(inputBase.Page) };
                    break;
                case 2:
                    yield return new object[] { new ListCategoriesInput(inputBase.Page, inputBase.PerPage) };
                    break;
                case 3:
                    yield return new object[] { new ListCategoriesInput(inputBase.Page, inputBase.PerPage, inputBase.Search) };
                    break;
                case 4:
                    yield return new object[] { new ListCategoriesInput(inputBase.Page, inputBase.PerPage, inputBase.Search, inputBase.Sort) };
                    break;
                case 5:
                    yield return new object[] { inputBase };
                    break;
            }
        }
    }
}

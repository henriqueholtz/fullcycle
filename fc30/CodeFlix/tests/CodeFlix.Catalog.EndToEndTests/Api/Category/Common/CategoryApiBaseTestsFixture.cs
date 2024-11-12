using CodeFlix.Catalog.EndToEndTests.Base;

namespace CodeFlix.Catalog.EndToEndTests.Api.Category.Common;

public class CategoryApiBaseTestsFixture : BaseFixture
{
    public CategoryApiBaseTestsFixture() : base()
    {
        Persistence = new CategoryPersistence(CreateDbContext());
    }
    public CategoryPersistence Persistence { get; set; }

    public string GetValidCategoryName()
    {
        string categoryName = string.Empty;
        while (categoryName.Length < 2)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName.Substring(0, 255);

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        string categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription.Substring(0, 10_000);

        return categoryDescription;
    }

    public bool GetRandomBoolean() => new Random().NextDouble() > 0.5;

    public string GetInvalidNameTooShort()
    {
        string nameTooShort = GetValidCategoryName();
        return nameTooShort.Substring(0, 2);
    }

    public string GetInvalidNameTooLong()
    {
        string nameTooLong = GetValidCategoryName();
        while (nameTooLong.Length <= 255)
            nameTooLong += $" {GetValidCategoryName()}";
        return nameTooLong;
    }

    public string GetInvalidDescriptionTooLong()
    {
        string descriptionTooLong = GetValidCategoryDescription();
        while (descriptionTooLong.Length <= 10_000)
            descriptionTooLong += $" {GetValidCategoryDescription()}";
        return descriptionTooLong;
    }
}

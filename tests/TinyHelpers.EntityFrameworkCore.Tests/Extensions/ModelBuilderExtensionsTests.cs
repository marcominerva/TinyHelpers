using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

namespace TinyHelpers.EntityFrameworkCore.Tests.Extensions;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}

public interface ITenantEntity
{
    int TenantId { get; set; }
}

public abstract class DeletableEntity
{
    public bool IsDeleted { get; set; }
}

public class Person : DeletableEntity, ISoftDeletable, ITenantEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int TenantId { get; set; }
}

public class City : DeletableEntity, ISoftDeletable
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class ModelBuilderExtensionsTests
{
    [Fact]
    public void ApplyQueryFilter_WithBaseClass_AppliesFilterToInheritingEntities()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<DeletableEntity>(e => !e.IsDeleted);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyQueryFilter_WithInterface_AppliesFilterToImplementingEntities()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<ISoftDeletable>(e => !e.IsDeleted);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyQueryFilter_WithInterface_OnlyAppliesFilterToEntitiesThatImplementInterface()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<ITenantEntity>(e => e.TenantId == 1);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Empty(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyQueryFilter_ByPropertyName_AppliesFilterToEntitiesWithMatchingProperty()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter("IsDeleted", false);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyNamedQueryFilter_WithBaseClass_AppliesNamedFilterToInheritingEntities()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<DeletableEntity>("SoftDelete", e => !e.IsDeleted);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyNamedQueryFilter_WithInterface_AppliesNamedFilterToImplementingEntities()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<ISoftDeletable>("SoftDelete", e => !e.IsDeleted);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyNamedQueryFilter_WithInterface_OnlyAppliesFilterToEntitiesThatImplementInterface()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<ITenantEntity>("TenantFilter", e => e.TenantId == 1);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Empty(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyNamedQueryFilter_MultipleFilters_AllFiltersAreApplied()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<ISoftDeletable>("SoftDelete", e => !e.IsDeleted);
        modelBuilder.ApplyQueryFilter<ITenantEntity>("TenantFilter", e => e.TenantId == 1);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Equal(2, personFilters.Count);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyNamedQueryFilter_ByPropertyName_AppliesNamedFilterToEntitiesWithMatchingProperty()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter("SoftDelete", "IsDeleted", false);

        var personFilters = modelBuilder.Model.FindEntityType(typeof(Person))!.GetDeclaredQueryFilters();
        var cityFilters = modelBuilder.Model.FindEntityType(typeof(City))!.GetDeclaredQueryFilters();
        var countryFilters = modelBuilder.Model.FindEntityType(typeof(Country))!.GetDeclaredQueryFilters();

        Assert.Single(personFilters);
        Assert.Single(cityFilters);
        Assert.Empty(countryFilters);
    }

    [Fact]
    public void ApplyNamedQueryFilter_FindByName_ReturnsCorrectFilter()
    {
        var modelBuilder = CreateModelBuilder();

        modelBuilder.ApplyQueryFilter<ISoftDeletable>("SoftDelete", e => !e.IsDeleted);
        modelBuilder.ApplyQueryFilter<ITenantEntity>("TenantFilter", e => e.TenantId == 1);

        var personType = modelBuilder.Model.FindEntityType(typeof(Person))!;

        var softDeleteFilter = personType.FindDeclaredQueryFilter("SoftDelete");
        var tenantFilter = personType.FindDeclaredQueryFilter("TenantFilter");
        var nonExistentFilter = personType.FindDeclaredQueryFilter("NonExistent");

        Assert.NotNull(softDeleteFilter);
        Assert.NotNull(tenantFilter);
        Assert.Null(nonExistentFilter);
    }

    [Fact]
    public void GetEntityTypes_WithBaseClass_ReturnsInheritingTypes()
    {
        var modelBuilder = CreateModelBuilder();

        var types = modelBuilder.GetEntityTypes<DeletableEntity>().ToList();

        Assert.Contains(typeof(Person), types);
        Assert.Contains(typeof(City), types);
        Assert.DoesNotContain(typeof(Country), types);
    }

    [Fact]
    public void GetEntityTypes_WithInterface_ReturnsImplementingTypes()
    {
        var modelBuilder = CreateModelBuilder();

        var types = modelBuilder.GetEntityTypes<ISoftDeletable>().ToList();

        Assert.Contains(typeof(Person), types);
        Assert.Contains(typeof(City), types);
        Assert.DoesNotContain(typeof(Country), types);
    }

    private static ModelBuilder CreateModelBuilder()
    {
        var modelBuilder = new ModelBuilder();

        modelBuilder.Entity<Person>(b =>
        {
            b.Property(p => p.Id);
            b.Property(p => p.Name);
            b.Property(p => p.IsDeleted);
            b.Property(p => p.TenantId);
        });

        modelBuilder.Entity<City>(b =>
        {
            b.Property(c => c.Id);
            b.Property(c => c.Name);
            b.Property(c => c.IsDeleted);
        });

        modelBuilder.Entity<Country>(b =>
        {
            b.Property(c => c.Id);
            b.Property(c => c.Name);
        });

        return modelBuilder;
    }
}

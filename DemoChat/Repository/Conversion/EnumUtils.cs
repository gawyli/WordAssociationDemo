using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.Repository.Conversion;
public static class EnumUtils
{
    public static ModelBuilder RegisterEnumConverters(this ModelBuilder modelBuilder)
    {
        //
        // TODO When EfCore 6 is released, this method can hopefully be replaced by something much simpler, based on this: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0/whatsnew#pre-convention-model-configuration
        //

        // Iterate over all entities, then over all properties of each entity, seeking enum-valued properties.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                // Handle both enums & nullable enums.
                var propertyType = property.ClrType;
                //if (propertyType.IsNullable())
                //{
                //    propertyType = Nullable.GetUnderlyingType(propertyType);
                //}

                if (propertyType!.IsEnum)
                {
                    var converterType = typeof(EnumToStringConverter<>).MakeGenericType(propertyType);
                    var converter = Activator.CreateInstance(converterType, new ConverterMappingHints()) as ValueConverter;

                    property.SetValueConverter(converter);
                }
            }
        }

        return modelBuilder;
    }
}

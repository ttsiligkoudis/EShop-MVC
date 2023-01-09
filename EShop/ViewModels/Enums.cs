using System;
using System.ComponentModel;
using System.Linq;

namespace EShop.ViewModels
{
    public static class Enums
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Any())
                {
                    return ((DescriptionAttribute) attributes.ElementAt(0)).Description;
                }
            }

            return genericEnum.ToString();
        }
    }

    [TypeConverter]
    public enum UserType : short
    {
        Admin = 1,
        User = 2
    }

    [TypeConverter]
    public enum Category : short
    {
        Lips = 1,
        Eyes = 2,
        Face = 4,
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PandaSearch.Shared.enums
{
    public static class EnumHelper
    {
        public static Dictionary<string, string> ObtenerDescripcionesDeEnum(Type tipoEnum)
        {
            if (!tipoEnum.IsEnum)
                throw new ArgumentException("El tipo proporcionado no es un enum.");

            var descripciones = new Dictionary<string, string>();
            var valoresEnum = Enum.GetValues(tipoEnum);

            foreach (var valorEnum in valoresEnum)
            {
                descripciones.Add(GetDescription((Enum)valorEnum),valorEnum.ToString());
            }

            return descripciones;
        }
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }
            return value.ToString();
        }
    }
}

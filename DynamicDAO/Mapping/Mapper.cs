using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DynamicDAO.Mapping
{
    /// <summary>
    /// Realiza o mapeamento entre a entidade, POCO ou DTO, para execução de queries e/ou stored procedures.
    /// </summary>
    internal class Mapper
    {
        #region Internal Methods

        /// <summary>
        /// Mapeia uma lista de parâmetros para preenchimento do <see cref="IDbCommand"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser analisado.</typeparam>
        /// <returns>Um <see cref="Dictionary{TKey, TValue}"/> contendo os parâmetros definidos pelo usuário.</returns>
        /// <exception cref="System.TypeLoadException">TypeLoadException</exception>
        /// <exception cref="System.ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">ArgumentException</exception>
        /// <exception cref="System.InvalidOperationException">InvalidOperationException</exception>
        internal static Dictionary<string, object[]> GetParameters<T>()
        {
            Dictionary<string, object[]> dicParameters = new Dictionary<string, object[]>();

            foreach (PropertyInfo pInfo in typeof(T).GetProperties())
            {
                object[] attribute = pInfo.GetCustomAttributes(typeof(ParameterAttribute), true);
                object[] parameterData = new object[2];

                if (attribute.Length.Equals(1) == true)
                {
                    parameterData[0] = ((ParameterAttribute)attribute[0]).ParameterName;
                    parameterData[1] = ((ParameterAttribute)attribute[0]).Direction;

                    dicParameters.Add(((ParameterAttribute)attribute[0]).PropertyName, parameterData);
                }
            }

            return dicParameters;
        }

        /// <summary>
        /// Mapeia uma lista de campos para preenchimento da entidade, POCO ou DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser preenchido.</typeparam>
        /// <returns>Um <see cref="Dictionary{TKey, TValue}"/> contendo as propriedades a serem preenchidas.</returns>
        /// <exception cref="System.TypeLoadException">TypeLoadException</exception>
        /// <exception cref="System.ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">ArgumentException</exception>
        /// <exception cref="System.InvalidOperationException">InvalidOperationException</exception>
        internal static Dictionary<string, string> GetFields<T>()
        {
            Dictionary<string, string> dicFields = new Dictionary<string, string>();

            foreach (PropertyInfo pInfo in typeof(T).GetProperties())
            {
                object[] attribute = pInfo.GetCustomAttributes(typeof(FieldAttribute), true);

                if (attribute.Length.Equals(1) == true)
                {
                    dicFields.Add(((FieldAttribute)attribute[0]).RetrievedField, ((FieldAttribute)attribute[0]).PropertyName);
                }
            }

            return dicFields;
        }

        #endregion Internal Methods
    }
}
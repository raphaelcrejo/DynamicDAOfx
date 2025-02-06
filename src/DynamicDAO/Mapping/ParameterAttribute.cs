using System;
using System.Data;

namespace DynamicDAO.Mapping
{
    /// <summary>
    /// Marca os elementos utilizados como parâmetros nas queries e/ou stored procedures. Essa classe não pode ser herdada.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ParameterAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Nome da propriedade da entidade, POCO ou DTO.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Nome do parâmetro na stored procedure ou query.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Direção do parâmetro na stored procedure ou query (Input, Output, InputOutput, ReturnValue).
        /// </summary>
        public ParameterDirection Direction { get; set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ParameterAttribute"/> com propriedades padrão.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade, POCO ou DTO.</param>
        /// <param name="parameterName">Nome do parâmetro na stored procedure ou query.</param>
        /// <param name="parameterDirection">Direção do parâmetro na stored procedure ou query (Input, Output, InputOutput, ReturnValue).</param>
        public ParameterAttribute(string propertyName, string parameterName, ParameterDirection parameterDirection)
        {
            PropertyName = propertyName;
            ParameterName = parameterName;
            Direction = parameterDirection;
        }

        #endregion Constructors
    }
}
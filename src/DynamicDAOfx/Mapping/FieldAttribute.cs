using System;

namespace DynamicDAOfx.Mapping
{
    /// <summary>
    /// Marca os elementos utilizados como receptores de dados das queries e/ou stored procedures. Essa classe não pode ser herdada.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class FieldAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Nome do campo
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Campo retornado pela consulta
        /// </summary>
        public string RetrievedField { get; set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="FieldAttribute"/> com propriedades padrão.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade, POCO ou DTO.</param>
        /// <param name="retrievedField">Nome da coluna devolvida pela consulta.</param>
        public FieldAttribute(string propertyName, string retrievedField)
        {
            PropertyName = propertyName;
            RetrievedField = retrievedField;
        }

        #endregion Constructors
    }
}
using System.Data;

namespace DynamicDAO
{
    /// <summary>
    /// Define os dados de provedor e string de conexão para acesso à uma fonte de dados.
    /// </summary>
    public class ProviderInfo
    {
        #region Internal Properties

        /// <summary>
        /// Nome do provedor de dados.
        /// </summary>
        internal string ProviderName { get; set; }

        /// <summary>
        /// String de conexão com uma fonte de dados.
        /// </summary>
        internal string ConnectionString { get; set; }

        /// <summary>
        /// Identificador de parâmetros (@, p, p_, etc.).
        /// </summary>
        internal string Identifier { get; set; }

        /// <summary>
        /// Tipo de comando a ser executado.
        /// </summary>
        internal CommandType CommandType { get; set; }

        #endregion Internal Properties

        #region Constructors

        /// <summary>
        /// Inicializa uma nova instância da classe DynamicDAO.ProviderInfo com nome do provedor de dados e string de conexão especificados.
        /// </summary>
        /// <param name="providerName">Nome do provedor de dados.</param>
        /// <param name="connectionString">String de conexão com uma fonte de dados.</param>
        /// <param name="identifier">Identificador de parâmetros (@, p, p_, etc. Padrão: @).</param>
        /// <param name="commandType">Tipo de comando a ser executado (Padrão: CommandType.StoredProcedure).</param>
        public ProviderInfo(string providerName, string connectionString, string identifier = "@", CommandType commandType = CommandType.StoredProcedure)
        {
            ProviderName = providerName;
            ConnectionString = connectionString;
            Identifier = identifier;
            CommandType = commandType;
        }

        #endregion Constructors
    }
}
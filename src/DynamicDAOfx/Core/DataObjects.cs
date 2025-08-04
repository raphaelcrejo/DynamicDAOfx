using System.Data;
using System.Data.Common;

namespace DynamicDAOfx.Core
{
    /// <summary>
    /// Define a conexão e a instrução SQL para acesso À uma fonte de dados.
    /// </summary>
    internal class DataObjects
    {
        /// <summary>
        /// Cria uma conexão com uma fonte de dados, com provedor e string de conexão especificados.
        /// </summary>
        /// <param name="providerInfo">Informações do provedor de dados.</param>
        /// <returns>Uma conexão com uma fonte de dados.</returns>
        internal static IDbConnection CreateConnection(ProviderInfo providerInfo)
        {
            DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerInfo.ProviderName);

            IDbConnection connection = providerFactory.CreateConnection();
            connection.ConnectionString = providerInfo.ConnectionString;

            return connection;
        }

        /// <summary>
        /// Cria uma instrução SQL para execução em uma fonte de dados, com conexão e tipo de comando especificados.
        /// </summary>
        /// <param name="connection">Conexão com a fonte de dados.</param>
        /// <param name="providerInfo">Informações do provedor de dados.</param>
        /// <returns>Um <see cref="IDbCommand"/> associado ao <see cref="IDbConnection"/> informado.</returns>
        internal static IDbCommand CreateCommand(IDbConnection connection, ProviderInfo providerInfo)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandType = providerInfo.CommandType;
            command.CommandTimeout = providerInfo.CommandTimeout;

            return command;
        }

        /// <summary>
        /// Cria uma transação para execução de instrução SQL em uma fonte de dados.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="providerInfo">Informações do provedor de dados.</param>
        /// <returns>Um <see cref="IDbTransaction"/> associado ao <see cref="IDbConnection"/> informado.</returns>
        internal static IDbTransaction CreateTransaction(IDbConnection connection, ProviderInfo providerInfo)
        {
            IDbTransaction transaction = connection.BeginTransaction(providerInfo.IsolationLevel);

            return transaction;
        }
    }
}
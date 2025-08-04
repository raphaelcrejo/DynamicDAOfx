using System.ComponentModel;
using System.Data;

namespace DynamicDAOfx
{
    /// <summary>
    /// Define os dados de provedor e string de conexão para acesso à uma fonte de dados.
    /// </summary>
    public class ProviderInfo
    {
        #region Private Properties

        private string _providerName = "System.Data.SqlClient";

        private string _connectionString;

        private string _identifier = "@";

        private CommandType _commandType = CommandType.StoredProcedure;

        private int _commandTimeout = 30;

        private bool _lockTransaction = false;

        private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        #endregion

        #region Public Properties

        /// <summary>
        /// Nome do provedor de dados.
        /// </summary>
        [DefaultValue("System.Data.SqlClient")]
        public string ProviderName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }

        /// <summary>
        /// String de conexão com um banco de dados.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// Identificador de parâmetros (@, p, p_, etc.).
        /// </summary>
        [DefaultValue("@")]
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        /// <summary>
        /// Tipo de comando a ser executado.
        /// </summary>
        [DefaultValue(CommandType.StoredProcedure)]
        public CommandType CommandType
        {
            get { return _commandType; }
            set { _commandType = value; }
        }

        /// <summary>
        /// Tempo de espera de execução de comando no banco de dados. Valor padrão: 30s.
        /// </summary>
        [DefaultValue(30)]
        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value; }
        }

        /// <summary>
        /// Identificador de operação transacional.
        /// </summary>
        [DefaultValue(false)]
        public bool LockTransaction
        {
            get { return _lockTransaction; }
            set { _lockTransaction = value; }
        }

        /// <summary>
        /// Nível de isolamento da transação.
        /// </summary>
        [DefaultValue(IsolationLevel.ReadCommitted)]
        public IsolationLevel IsolationLevel
        {
            get { return _isolationLevel; }
            set { _isolationLevel = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Inicializa uma nova instância da classe DynamicDAO.ProviderInfo.
        /// </summary>
        public ProviderInfo() { }

        /// <summary>
        /// Inicializa uma nova instância da classe DynamicDAO.ProviderInfo com nome do provedor de dados e string de conexão especificados.
        /// </summary>
        /// <param name="connectionString">String de conexão com uma fonte de dados.</param>
        /// <param name="providerName">Nome do provedor de dados.</param>
        /// <param name="identifier">Identificador de parâmetros (@, p, p_, etc. Padrão: @).</param>
        /// <param name="commandType">Tipo de comando SQL a ser executado.</param>
        /// <param name="commandTimeout">Tempo de espera de execução de comando no banco de dados. Valor padrão: 30s.</param>
        public ProviderInfo(string connectionString, string providerName = "System.Data.SqlClient", string identifier = "@", CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
        {
            _connectionString = connectionString;
            _providerName = providerName;
            _identifier = identifier;
            _commandType = commandType;
            _commandTimeout = commandTimeout;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe DynamicDAO.ProviderInfo com nome do provedor de dados e string de conexão especificados.
        /// </summary>
        /// <param name="connectionString">String de conexão com uma fonte de dados.</param>
        /// <param name="providerName">Nome do provedor de dados.</param>
        /// <param name="identifier">Identificador de parâmetros (@, p, p_, etc. Padrão: @).</param>
        /// <param name="commandType">Tipo de comando SQL a ser executado.</param>
        /// <param name="commandTimeout">Tempo de espera de execução de comando no banco de dados. Valor padrão: 30s.</param>
        /// <param name="lockTransaction">Define se o comando a ser executado será transacional ou não.</param>
        /// <param name="isolationLevel">Nível de isolamento da transação</param>
        public ProviderInfo(string connectionString, string providerName = "System.Data.SqlClient", string identifier = "@", CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30, bool lockTransaction = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _connectionString = connectionString;
            _providerName = providerName;
            _identifier = identifier;
            _commandType = commandType;
            _commandTimeout = commandTimeout;
            _lockTransaction = lockTransaction;
            _isolationLevel = isolationLevel;
        }

        #endregion
    }
}